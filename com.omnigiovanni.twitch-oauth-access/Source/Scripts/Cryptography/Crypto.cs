using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

namespace OmniGiovanni.Cryptography
{

	public class Crypto
	{
    
		public static byte[] EncodeString(string S)
		{
			return Encoding.UTF8.GetBytes(S);
		}
        
	   	public static string DecryptData(byte[] encryptedData, string key, byte[] iv)
	    {
		    using (Aes aesAlg = Aes.Create())
		    {
			    aesAlg.Key = Encoding.UTF8.GetBytes(key);
			    aesAlg.IV = iv;

			    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			    using (var msDecrypt = new System.IO.MemoryStream(encryptedData))
				    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					    using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
					    {
						    return srDecrypt.ReadToEnd();
					    }
		    }
	    }	             
        
    }
}
