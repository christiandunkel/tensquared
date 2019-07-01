using System.IO;
using UnityEngine;

/*
 * provides a variety of print methods to the Unity console,
 * all of which are color-coded and with additional information
 * about the calling method / script
 */

public static class Log {
  
  /*
   ==================
   === ATTRIBUTES ===
   ==================
   */

  // add unique id to messages to make sure each is displayed 
  // in its own line inside unity and not combined with similar messages
  private static int messageID = 1;





  /*
   ================
   === Internal ===
   ================
   */


  private static string generateMessage(int type, string message, string sourcePath, int lineNumber, string methodName) {

    /*
     * generates and returns a color-coded message 
     * formatted for the Debug methods and the Unity console
     */

    // get base name of executing file instead of whole path and remove '.cs' file ending
    sourcePath = Path.GetFileName(sourcePath);
    sourcePath = sourcePath.Substring(0, sourcePath.Length - 3);

    // pad message id with leading zeros
    string thisID = (messageID+"").PadLeft((messageID + "").Length + 3, '0');

    // select the color coding for the message
    string color_header = "";
    string color_message = "";

    switch (type) {
      // grey, default message
      case 1:
        color_header = "#666666";
        color_message = "#3b3b3b";
        break;
      // yellow, warning
      case 2:
        color_header = "#787000";
        color_message = "#423e01";
        break;
      // red, error
      case 3:
        color_header = "#b80000";
        color_message = "#610000";
        break;
    }

    // add additional attributes to message
    message = $"<color={color_header}>" + 
                  $"<size=10>{thisID}</size> " + 
                  $"<b>{sourcePath}</b>:{lineNumber}:{methodName}()" + 
              $"</color> " + 
              $"<color={color_message}>{message}</color>";

    // increase id for next message -> make it unique
    messageID++;

    return message;

  }





  /*
   ================
   === EXTERNAL ===
   ================
   */

  public static void Print(
    string message,
    Object gameObject = null,
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
    [System.Runtime.CompilerServices.CallerMemberName] string methodName = ""
  ) {

    /*
     * print a custom, color-coded message akin Debug.Log()
     * with additional information about the calling method and script
     */

    Debug.Log(generateMessage(1, message, sourcePath, lineNumber, methodName), gameObject);

  }

  public static void Warn(
    string message,
    Object gameObject = null,
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
    [System.Runtime.CompilerServices.CallerMemberName] string methodName = ""
  ) {

    /*
     * print a custom, color-coded message akin Debug.LogWarning()
     * with additional information about the calling method and script
     */

    Debug.LogWarning(generateMessage(2, message, sourcePath, lineNumber, methodName), gameObject);

  }

  public static void Error(
    string message,
    Object gameObject = null,
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
    [System.Runtime.CompilerServices.CallerMemberName] string methodName = ""
  ) {

    /*
     * generates a custom, color-coded message akin Debug.LogError()
     * with additional information about the calling method and script
     */

    Debug.LogError(generateMessage(3, message, sourcePath, lineNumber, methodName), gameObject);

  }

}
