using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Country_city_state
{
    public class SecurityPurpose
    {
        public string key ="key";
        public static string Encrtyption(string text)
        {
            byte[] addDataText = ASCIIEncoding.ASCII.GetBytes(text);
            string encryptname = Convert.ToBase64String(addDataText);
            return encryptname;
        }
        public static string DecrtyptionData(string text)
        {
            byte[] encryptName = Convert.FromBase64String(text);
            string decryptName = ASCIIEncoding.ASCII.GetString(encryptName);
            return decryptName;
        }


        /* public static string Encrypt(string text)
          {
              byte[] iv = new byte[32];
              byte[] array;
              using (Aes aes = Aes.Create())
              {
                  aes.Key = Encoding.UTF8.GetBytes(key);
                  aes.IV = iv;
                  ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                  using (MemoryStream ms=new MemoryStream()){
                      using(CryptoStream cryptoStream=new CryptoStream((Stream)ms,encrypt,CryptoStreamMode.Write))
                      {
                          using(StreamWriter streamWriter=new StreamWriter((Stream) cryptoStream))
                          {
                              streamWriter.Write(text);
                          }
                          array = ms.ToArray();
                      }
                  }

              }
              return Convert.ToBase64String(array);
          }
          public static string Decrypt(string text)
          {
              byte[] iv = new byte[16];
              byte[] buffer = Convert.FromBase64String(text);
              using(Aes aes = Aes.Create())
              {
                  aes.Key = Encoding.UTF8.GetBytes(key);
                  aes.IV = iv;
                  ICryptoTransform decrypto = aes.CreateDecryptor(aes.Key, aes.IV);
                  using(MemoryStream ms=new MemoryStream(buffer))
                  {
                      using (CryptoStream cryptoStream = new CryptoStream((Stream)ms, decrypto, CryptoStreamMode.Read))
                      {
                          using (StreamReader sr=new StreamReader(cryptoStream))
                          {
                              return sr.ReadToEnd();
                          }
                      }
                  }
              }

        }
*/
    }
}
