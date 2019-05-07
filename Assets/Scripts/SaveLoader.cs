using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoader : MonoBehaviour
{

  private string encoding_pass_phrase = "tensquaredSaveFileCipher";

  // default value / format of timer
  private string def_timer = "00:00:000";

  void Start()
  {

    SetDefaultValues();

  }


  // set default values, if player pref isn't set
  private void SetDefaultValues()
  {
   
    // settings
    
    if (!PlayerPrefs.HasKey("music_volume"))
    {
      PlayerPrefs.SetFloat("music_volume", 1.0f);
    }

    if (!PlayerPrefs.HasKey("sound_volume"))
    {
      PlayerPrefs.SetFloat("sound_volume", 1.0f);
    }

    if (!PlayerPrefs.HasKey("speech_volume"))
    {
      PlayerPrefs.SetFloat("speech_volume", 1.0f);
    }

    // progress

    if (!PlayerPrefs.HasKey("lvls_unlocked"))
    {
      PlayerPrefs.SetInt("lvls_unlocked", 1);
    }

    // levels

    if (!PlayerPrefs.HasKey("lvl1_timer"))
    {
      PlayerPrefs.SetString("lvl1_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl2_timer"))
    {
      PlayerPrefs.SetString("lvl2_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl3_timer"))
    {
      PlayerPrefs.SetString("lvl3_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl4_timer"))
    {
      PlayerPrefs.SetString("lvl4_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl5_timer"))
    {
      PlayerPrefs.SetString("lvl5_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl6_timer"))
    {
      PlayerPrefs.SetString("lvl6_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl7_timer"))
    {
      PlayerPrefs.SetString("lvl7_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl8_timer"))
    {
      PlayerPrefs.SetString("lvl8_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl9_timer"))
    {
      PlayerPrefs.SetString("lvl9_timer", def_timer);
    }

    if (!PlayerPrefs.HasKey("lvl10_timer"))
    {
      PlayerPrefs.SetString("lvl10_timer", def_timer);
    }

  }


  // reset saved progress data
  public void ResetProgress()
  {

    // progress

    PlayerPrefs.SetInt("lvls_unlocked", 1);

    // levels

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
    

  }




  public InputField exportField = null;
  public Toggle exportSettings = null;

  public void LoadExportData()
  {

    if (exportField == null)
    {
      Debug.Log("No 'export field' set in SaveLoader.");
      return;
    }

    if (exportSettings == null)
    {
      Debug.Log("No 'export settings button' set in SaveLoader.");
      return;
    }

    string save_data = "";

    if (exportSettings.isOn == true)
    {

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
    else
    {

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
    save_data = Convert.ToBase64String(bytesToEncode);

    // encrypt the save data
    save_data = Cipher.Encrypt(save_data, encoding_pass_phrase);

    exportField.text = save_data;

  }

}
