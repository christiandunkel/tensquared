using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoader : MonoBehaviour
{

  private string encoding_pass_phrase = "tensquaredSaveFileCipher";

  void Start()
  {

    

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
    /*
    string save_data = "";

    if (exportSettings.isOn == true)
    {

      VolumeSettings volume = GetVolumeSettings();

      save_data += "music_volume=" + volume.music + "|";
      save_data += "sound_volume=" + volume.sound + "|";
      save_data += "speech_volume=" + volume.speech + "|";

    }

    save_data += "progess";*/

    Save save = new Save();

    if (exportSettings.isOn == true)
    {

      String[] volume = GetVolumeSettings();

      save.music_volume = volume[0];
      save.sound_volume = volume[1];
      save.speech_volume = volume[2];

    }

    string save_data = JsonUtility.ToJson(save);

    // encode with base64
    byte[] bytesToEncode = Encoding.UTF8.GetBytes(save_data);
    save_data = Convert.ToBase64String(bytesToEncode);

    // encrypt the save data
    save_data = Cipher.Encrypt(save_data, encoding_pass_phrase);

    exportField.text = save_data;

  }

  private String[] GetVolumeSettings()
  {

    String[] volume = {
      PlayerPrefs.GetFloat("music_volume").ToString(),
      PlayerPrefs.GetFloat("sound_volume").ToString(),
      PlayerPrefs.GetFloat("speech_volume").ToString()
    };

    return volume;

  }

}
