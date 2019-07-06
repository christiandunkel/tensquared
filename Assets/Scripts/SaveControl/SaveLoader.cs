using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

/* 
 * can import and export save data as base64-encoded, encrypted text
 * loads given save data into player prefs
 */

public class SaveLoader : MonoBehaviour {

  /*
   ==================
   === ATTRIBUTES ===
   ==================
   */

  private string encoding_pass_phrase = "tensquaredSaveFileCipher";

  // default value / format of timer
  private static string def_timer = "00:00:000";
  private static Regex timerRegex = new Regex(@"^[0-9]{2}:[0-9]{2}:[0-9]{3}$", RegexOptions.Compiled);





  /*
   ==================
   === COMPONENTS ===
   ==================
   */

  // 'import save' components
  [SerializeField] private InputField importField = null;
  [SerializeField] private CanvasGroup successMessage = null;
  [SerializeField] private CanvasGroup errorMessage = null;

  // 'export save' components
  [SerializeField] private InputField exportField = null;
  [SerializeField] private Toggle exportSettings = null;





  /*
   ================
   === INTERNAL ===
   ================
   */

  private void Start() {

    setDefaultValues();

  }

  // set default values, if player prefs aren't set
  private void setDefaultValues() {

    /*
     * set default values for settings and progress, 
     * if there are no player prefs set yet
     */

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







  /*
   ================
   === EXTERNAL ===
   ================
   */

  public static bool timerIsInRightFormat(string timer) {

    /*
     * tests if a given timer is in the right format
     * and returns the result as a boolean
     */

    return timerRegex.IsMatch(timer);

  }

  public static string getDefaultTimer() {
    
    /*
     * returns the default format for the timer
     * in form of a string
     */

    return def_timer;

  }

  public void ResetProgress() {

    /*
     * reset saved progress data
     */

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

  public void UnlockAllLevels() {

    /*
     * unlocks all levels in the level manager
     * and all levels in the level menu
     */

    PlayerPrefs.SetInt("lvls_unlocked", LevelManager.Instance.getInt("maxLevelsUnlockable"));

    LevelManager.Instance.LoadLevelProgess();

  }

  public void ImportSave() {

    /*
     * imports a save text from the 'import save' text field
     * and loads the settings/progress data into player prefs
     */

    string save_data = importField.text;

    try { 

      // decrypt string
      save_data = Cipher.Decrypt(save_data, encoding_pass_phrase);

      // decode base64
      byte[] decodedBytes = System.Convert.FromBase64String(save_data);
      save_data = Encoding.UTF8.GetString(decodedBytes);

    }
    catch (System.Exception e) {

      Log.Warn($"Could not import save data: {e}", this);

      // load error message
      errorMessage.gameObject.SetActive(true);
      errorMessage.alpha = 1;
      errorMessage.interactable = true;

      return;
    }

    // test if settings had been exported as well
    bool exported_settings = Regex.IsMatch(save_data, "music_volume");

    Log.Print($"Importing save data {(exported_settings ? " with export settings" : "")}.", this);

    // convert JSON string back to object
    if (exported_settings) {

      Save save = new Save();
      save = JsonUtility.FromJson<Save>(save_data);

      PlayerPrefs.SetFloat("music_volume", float.Parse(save.music_volume));
      PlayerPrefs.SetFloat("sound_volume", float.Parse(save.sound_volume));
      PlayerPrefs.SetFloat("speech_volume", float.Parse(save.speech_volume));

      saveInPlayerPrefs(save.lvls_unlocked, new string[10] {
         save.lvl1_timer, save.lvl2_timer, save.lvl3_timer, save.lvl4_timer, save.lvl5_timer,
         save.lvl6_timer, save.lvl7_timer, save.lvl8_timer, save.lvl9_timer, save.lvl10_timer
      });

      VolumeController.Instance.loadPlayerPrefs();

    }
    else {

      SaveNoSettings save = new SaveNoSettings();
      save = JsonUtility.FromJson<SaveNoSettings>(save_data);

      saveInPlayerPrefs(save.lvls_unlocked, new string[10] {
         save.lvl1_timer, save.lvl2_timer, save.lvl3_timer, save.lvl4_timer, save.lvl5_timer,
         save.lvl6_timer, save.lvl7_timer, save.lvl8_timer, save.lvl9_timer, save.lvl10_timer
      });

    }

    void saveInPlayerPrefs(string lvls_unlocked, string[] timers) {

      /*
       * saves the given progress settings in player prefs
       */

      // add amount of levels that were unlocked
      PlayerPrefs.SetInt("lvls_unlocked", System.Int32.Parse(lvls_unlocked));

      // go through all timer values and add them to player prefs
      for (int i = 0; i < 10; i++) {

        // test if timer has proper format
        if (timerIsInRightFormat(timers[i])) {
          PlayerPrefs.SetString("lvl" + (i + 1) + "_timer", timers[i]);
        }
        else {
          // if format is wrong, set default timer
          PlayerPrefs.SetString("lvl" + (i + 1) + "_timer", def_timer);
        }
      }

    }

    LevelManager.Instance.LoadLevelProgess();

    // reset 'import save' input field
    importField.text = "";

    // load 'successful import' message
    successMessage.gameObject.SetActive(true);
    successMessage.alpha = 1;
    successMessage.interactable = true;

  }

  public void LoadExportData() {

    /*
     * generates a save string as export data
     * and places it in the export field
     */

    if (exportField == null) {
      Log.Warn("No 'export field' defined.", this);
      return;
    }

    if (exportSettings == null) {
      Log.Warn("No 'export settings button'.", this);
      return;
    }

    Log.Print("Exporting save data.", this);

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
