using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

using UnityEngine;

/*
 * script to securely encrypt and decrypt a plaintext
 * the basic script is from https://stackoverflow.com/a/10177020
 */

public class Cipher : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // keysize of the encryption algorithm in bits
  private const int Keysize = 256;

  // number of iterations for 'password bytes generation' function
  private const int DerivationIterations = 1000;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */
   
  private static byte[] RandomEntropy() {

    /*
     * generate 256 bits of random entropy
     */

    // 32 bites * 8 = 256 bits
    var randomBytes = new byte[Keysize / 8];

    using (var rngCsp = new RNGCryptoServiceProvider()) {
      // fill with cryptographically secure random bytes
      rngCsp.GetBytes(randomBytes);
    }

    return randomBytes;

  }

  private static string EncryptPlainText(string text, string passPhrase) {

    // salt and IV are randomly generated each time, 
    // but preprended to encrypted cipher for decryption
    var saltStringBytes = RandomEntropy();
    var ivStringBytes = RandomEntropy();
    var plainTextBytes = Encoding.UTF8.GetBytes(text);

    using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations)) {
      var keyBytes = password.GetBytes(Keysize / 8);
      using (var symmetricKey = new RijndaelManaged()) {

        symmetricKey.BlockSize = 256;
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.Padding = PaddingMode.PKCS7;

        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes)) {
          using (var memoryStream = new MemoryStream()) {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
              cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
              cryptoStream.FlushFinalBlock();
              // create final bytes as concatenation of random salt, iv and cipher bytes
              var cipherTextBytes = saltStringBytes;
              cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
              cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
              memoryStream.Close();
              cryptoStream.Close();
              return Convert.ToBase64String(cipherTextBytes);
            }
          }
        }
      }
    }

  }

  private static string DecryptCipherText(string text, string passPhrase) {

    // get complete stream of bytes that represent:
    // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
    var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(text);

    // get saltbytes by extracting the first 32 bytes from the supplied cipherText bytes
    var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();

    // get IV bytes by extracting the next 32 bytes from the supplied cipherText bytes
    var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();

    // get actual cipher text bytes by removing the first 64 bytes from the cipherText string
    var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

    using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations)) {
      var keyBytes = password.GetBytes(Keysize / 8);
      using (var symmetricKey = new RijndaelManaged()) {

        symmetricKey.BlockSize = 256;
        symmetricKey.Mode = CipherMode.CBC;
        symmetricKey.Padding = PaddingMode.PKCS7;

        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes)) {
          using (var memoryStream = new MemoryStream(cipherTextBytes)) {
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
              var plainTextBytes = new byte[cipherTextBytes.Length];
              var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
              memoryStream.Close();
              cryptoStream.Close();
              return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
          }
        }
      }
    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public static string Encrypt(string plainText, string passPhrase) {

    Log.Print($"Encrypting string '{plainText.Substring(0, 5)}...'.");

    string cipherText = "";

    try {
      cipherText = EncryptPlainText(plainText, passPhrase);
    }
    catch (Exception e) {
      Log.Error($"Encrypting string '{plainText.Substring(0, 5)}...' failed: {e}");
    }

    return cipherText;

  }

  public static string Decrypt(string cipherText, string passPhrase) {

    Log.Print($"Decrypting string '{cipherText.Substring(0, 5)}...'.");

    string plainText = "";

    try {
      plainText = DecryptCipherText(cipherText, passPhrase);
    }
    catch (Exception e) {
      Log.Error($"Decrypting string '{cipherText.Substring(0, 5)}...' failed: {e}");
    }

    return plainText;

  }

}
