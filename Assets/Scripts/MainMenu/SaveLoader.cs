using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

/* 
 * can import and export save data as base64-encoded, encrypted text
 * loads given save data into player prefs
 */

public class SaveLoader : MonoBehaviour {

  private string encoding_pass_phrase = "tensquaredSaveFileCipher";

  // default value / format of timer
  private string def_timer = "00:00:000";

  void Start() {

    SetDefaultValues();

  }


  // set default values, if player pref isn't set
  private void SetDefaultValues() {
   
    // settings
    
    if (!PlayerPrefs.HasKey("music_volume")) PlayerPrefs.SetFloat("music_volume", 1f);
    if (!PlayerPrefs.HasKey("sound_volume")) PlayerPrefs.SetFloat("sound_volume", 1f);
    if (!PlayerPrefs.HasKey("speech_volume")) PlayerPrefs.SetFloat("speech_volume", 1f);

    // progress

    if (!PlayerPrefs.HasKey("lvls_unlocked")) PlayerPrefs.SetInt("lvls_unlocked", 1);

    // levels

    if (!PlayerPrefs.HasKey("lvl1_timer")) PlayerPrefs.SetString("lvl1_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl2_timer")) PlayerPrefs.SetString("lvl2_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl3_timer")) PlayerPrefs.SetString("lvl3_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl4_timer")) PlayerPrefs.SetString("lvl4_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl5_timer")) PlayerPrefs.SetString("lvl5_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl6_timer")) PlayerPrefs.SetString("lvl6_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl7_timer")) PlayerPrefs.SetString("lvl7_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl8_timer")) PlayerPrefs.SetString("lvl8_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl9_timer")) PlayerPrefs.SetString("lvl9_timer", def_timer);
    if (!PlayerPrefs.HasKey("lvl10_timer")) PlayerPrefs.SetString("lvl10_timer", def_timer);

  }





  // reset saved progress data
  public void ResetProgress() {

    PlayerPrefs.SetInt("lvls_unlocked", 1);

    PlayerPrefs.SetString("lvl1_timer", def_timer);
    PlayerPrefs.SetString("lvl2_timer", def_timer);
    PlayerPrefs.SetString("lvl3_timer", def_timer);
    PlayerPrefs.SetString("lvl4_timer", def_timer);
    PlayerPrefs.SetString("lvl5_timer", def_timer);
    PlayerPrefs.SetString("lvl6_timer", def_timer);
    PlayerPrefs.SetString("lvl7_timer", def_timer);
    PlayerPrefs.SetString("lvl8_timer", def_timer);
    PlayerPrefs.SetString("lvl9_timer", def_timer);
    PlayerPrefs.SetString("lvl10_timer", def_timer);

    LevelManager.Instance.LoadLevelProgess();

  }




  public InputField importField = null;
  public CanvasGroup successMessage = null,
                     errorMessage = null;

  public void ImportSave() {

    string save_data = importField.text;

    try { 

      // decrypt string
      save_data = Cipher.Decrypt(save_data, encoding_pass_phrase);

      // decode base64
      byte[] decodedBytes = System.Convert.FromBase64String(save_data);
      save_data = Encoding.UTF8.GetString(decodedBytes);

    }
    catch (System.Exception e) {

      Debug.LogWarning("SaveLoader: Could not import save data: " + e);

      // load error message
      errorMessage.gameObject.SetActive(true);
      errorMessage.alpha = 1;
      errorMessage.interactable = true;

      return;
    }

    // test if settings had been exported as well
    bool exported_settings = Regex.IsMatch(save_data, "music_volume");

    Debug.Log("SaveLoader: Import save data" + 
              (exported_settings ? " with export settings" : "") + ".");

    // convert JSON string back to object
    if (exported_settings) {

      Save save = new Save();
      save = JsonUtility.FromJson<Save>(save_data);

      PlayerPrefs.SetFloat("music_volume", float.Parse(save.music_volume));
      PlayerPrefs.SetFloat("sound_volume", float.Parse(save.sound_volume));
      PlayerPrefs.SetFloat("speech_volume", float.Parse(save.speech_volume));

      PlayerPrefs.SetInt("lvls_unlocked", System.Int32.Parse(save.lvls_unlocked));

      PlayerPrefs.SetString("lvl1_timer", save.lvl1_timer);
      PlayerPrefs.SetString("lvl2_timer", save.lvl2_timer);
      PlayerPrefs.SetString("lvl3_timer", save.lvl3_timer);
      PlayerPrefs.SetString("lvl4_timer", save.lvl4_timer);
      PlayerPrefs.SetString("lvl5_timer", save.lvl5_timer);
      PlayerPrefs.SetString("lvl6_timer", save.lvl6_timer);
      PlayerPrefs.SetString("lvl7_timer", save.lvl7_timer);
      PlayerPrefs.SetString("lvl8_timer", save.lvl8_timer);
      PlayerPrefs.SetString("lvl9_timer", save.lvl9_timer);
      PlayerPrefs.SetString("lvl10_timer", save.lvl10_timer);

      VolumeController.Instance.loadPlayerPrefs();

    }
    else {

      SaveNoSettings save = new SaveNoSettings();
      save = JsonUtility.FromJson<SaveNoSettings>(save_data);

      PlayerPrefs.SetInt("lvls_unlocked", System.Int32.Parse(save.lvls_unlocked));

      PlayerPrefs.SetString("lvl1_timer", save.lvl1_timer);
      PlayerPrefs.SetString("lvl2_timer", save.lvl2_timer);
      PlayerPrefs.SetString("lvl3_timer", save.lvl3_timer);
      PlayerPrefs.SetString("lvl4_timer", save.lvl4_timer);
      PlayerPrefs.SetString("lvl5_timer", save.lvl5_timer);
      PlayerPrefs.SetString("lvl6_timer", save.lvl6_timer);
      PlayerPrefs.SetString("lvl7_timer", save.lvl7_timer);
      PlayerPrefs.SetString("lvl8_timer", save.lvl8_timer);
      PlayerPrefs.SetString("lvl9_timer", save.lvl9_timer);
      PlayerPrefs.SetString("lvl10_timer", save.lvl10_timer);

    }

    LevelManager.Instance.LoadLevelProgess();

    // reset 'import save' input field
    importField.text = "";

    // load 'successful import' message
    successMessage.gameObject.SetActive(true);
    successMessage.alpha = 1;
    successMessage.interactable = true;

  }





  public InputField exportField = null;
  public Toggle exportSettings = null;

  public void LoadExportData() {

    if (exportField == null) {
      Debug.LogWarning("SaveLoader: No 'export field' defined.");
      return;
    }

    if (exportSettings == null) {
      Debug.LogWarning("SaveLoader: No 'export settings button'.");
      return;
    }

    Debug.Log("SaveLoader: Exported save data");

    string save_data = "";

    if (exportSettings.isOn == true) {

      Save save = new Save();

      save.music_volume = PlayerPrefs.GetFloat("music_volume").ToString();
      save.sound_volume = PlayerPrefs.GetFloat("sound_volume").ToString();
      save.speech_volume = PlayerPrefs.GetFloat("speech_volume").ToString();

      save.lvls_unlocked = PlayerPrefs.GetInt("lvls_unlocked").ToString();

      save.lvl1_timer = PlayerPrefs.GetString("lvl1_timer");
      save.lvl2_timer = PlayerPrefs.GetString("lvl2_timer");
      save.lvl3_timer = PlayerPrefs.GetString("lvl3_timer");
      save.lvl4_timer = PlayerPrefs.GetString("lvl4_timer");
      save.lvl5_timer = PlayerPrefs.GetString("lvl5_timer");
      save.lvl6_timer = PlayerPrefs.GetString("lvl6_timer");
      save.lvl7_timer = PlayerPrefs.GetString("lvl7_timer");
      save.lvl8_timer = PlayerPrefs.GetString("lvl8_timer");
      save.lvl9_timer = PlayerPrefs.GetString("lvl9_timer");
      save.lvl10_timer = PlayerPrefs.GetString("lvl10_timer");

      save_data = JsonUtility.ToJson(save);

    }
    else {

      SaveNoSettings save = new SaveNoSettings();

      save.lvls_unlocked = PlayerPrefs.GetInt("lvls_unlocked").ToString();

      save.lvl1_timer = PlayerPrefs.GetString("lvl1_timer");
      save.lvl2_timer = PlayerPrefs.GetString("lvl2_timer");
      save.lvl3_timer = PlayerPrefs.GetString("lvl3_timer");
      save.lvl4_timer = PlayerPrefs.GetString("lvl4_timer");
      save.lvl5_timer = PlayerPrefs.GetString("lvl5_timer");
      save.lvl6_timer = PlayerPrefs.GetString("lvl6_timer");
      save.lvl7_timer = PlayerPrefs.GetString("lvl7_timer");
      save.lvl8_timer = PlayerPrefs.GetString("lvl8_timer");
      save.lvl9_timer = PlayerPrefs.GetString("lvl9_timer");
      save.lvl10_timer = PlayerPrefs.GetString("lvl10_timer");

      save_data = JsonUtility.ToJson(save);

    }

    // encode with base64
    byte[] bytesToEncode = Encoding.UTF8.GetBytes(save_data);
    save_data = System.Convert.ToBase64String(bytesToEncode);

    // encrypt the save data
    save_data = Cipher.Encrypt(save_data, encoding_pass_phrase);

    exportField.text = save_data;

  }

}
