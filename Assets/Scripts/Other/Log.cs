using System.IO;
using UnityEngine;

/*
 * provides a variety of print methods in alternative to Debug.*
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

  private static void OutputMessage(int type, string sourcePath, string message) {

    // get base name of executing file instead of whole path
    sourcePath = Path.GetFileName(sourcePath);

    // pad message id with leading zeros
    string thisID = (messageID+"").PadLeft((messageID + "").Length + 3, '0');

    // add additional attributes to message
    message = $"{thisID} {sourcePath}:{message}";

    // increase id for next message -> make it unique
    messageID++;

    switch (type) {
      // print
      case 1:
        Debug.Log(message);
        break;
      // warn
      case 2:
        Debug.LogWarning(message);
        break;
      // error
      case 3:
        Debug.LogError(message);
        break;
    }

  }





  /*
   ================
   === EXTERNAL ===
   ================
   */

  public static void Print(
    string message,
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
    [System.Runtime.CompilerServices.CallerMemberName] string methodName = ""
  ) {

    /*
     * generates a custom message akin Debug.Log()
     * with additional information about the calling method and script
     */

    OutputMessage(1, sourcePath, $"{lineNumber}:{methodName}() -> {message}");

  }

  public static void Warn(
    string message,
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
    [System.Runtime.CompilerServices.CallerMemberName] string methodName = ""
  ) {

    /*
     * generates a custom message akin Debug.LogWarning()
     * with additional information about the calling method and script
     */

    OutputMessage(2, sourcePath, $"{lineNumber}:{methodName}() -> {message}");

  }

  public static void Error(
    string message,
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
    [System.Runtime.CompilerServices.CallerMemberName] string methodName = ""
  ) {

    /*
     * generates a custom message akin Debug.LogError()
     * with additional information about the calling method and script
     */

    OutputMessage(3, sourcePath, $"{lineNumber}:{methodName}() -> {message}");

  }

}
