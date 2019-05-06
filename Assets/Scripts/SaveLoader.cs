using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoader : MonoBehaviour
{

  private string encoding_pass_phrase = "tensquaredSaveFileCipher";

  private class VolumeSettings
  {
    public string music = "0.0f";
    public string sound = "0.0f";
    public string speech = "0.0f";
  }

  private class Progress
  {
    
  }

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

    string save_data = "";

    if (exportSettings.isOn == true)
    {

      VolumeSettings volume = GetVolumeSettings();

      save_data += "music_volume=" + volume.music + "|";
      save_data += "sound_volume=" + volume.sound + "|";
      save_data += "speech_volume=" + volume.speech + "|";

    }

    save_data += "progess";


    // encode with base64
    byte[] bytesToEncode = Encoding.UTF8.GetBytes(save_data);
    save_data = Convert.ToBase64String(bytesToEncode);

    // encrypt the save data
    save_data = Cipher.Encrypt(save_data, encoding_pass_phrase);

    exportField.text = save_data;

  }

  private VolumeSettings GetVolumeSettings()
  {

    VolumeSettings volume = new VolumeSettings();

    volume.music = PlayerPrefs.GetFloat("music_volume").ToString();
    volume.sound = PlayerPrefs.GetFloat("sound_volume").ToString();
    volume.speech = PlayerPrefs.GetFloat("speech_volume").ToString();

    return volume;

  }

}
