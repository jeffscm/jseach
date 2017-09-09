using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

//import javax.crypto.Cipher;
//import javax.crypto.spec.*;
//import java.util.Base64;
//import java.util.Arrays;

public static class Util {


	public static string ExtractStr(Dictionary<string, string> dict)
	{
		string response = "---";

		if (dict != null)
		{
			if (dict.ContainsKey("fi"))
			{
				response = dict["fi"];
			}
			else if (dict.ContainsKey("sv"))
			{
				response = dict["sv"];
			}
		}
		return response;
	}

	public static string FormatDuration(string d)
	{
		string output = string.Empty;
		if (!string.IsNullOrEmpty(d))
		{
			int idx = d.IndexOf("T");
			if (idx < 0) return d;
			string temp = d.Substring(idx+1, d.Length - idx-1);


			for (int i = 0 ; i < temp.Length-1; i++)
			{
				if (!Char.IsDigit(temp[i]))
					output += ":";
				else
					output += temp[i];
			}
		}
		return output;

	}

	public static string GetUrl (string url)
	{

		string secret = "283994b1088a41bc";
		string data = url; 

		var tdes = new AesManaged();
		tdes.Key 		= Encoding.UTF8.GetBytes(secret);
		tdes.Mode 		= CipherMode.CBC;
		tdes.Padding 	= PaddingMode.PKCS7;

		byte[] plain = Convert.FromBase64String(data);

		byte [] first16 = new byte[16];

		Array.Copy(plain,0,first16, 0, 16);

		byte [] message16 = new byte[plain.Length - 16];

		Array.Copy(plain,16,message16, 0, plain.Length - 16);

		tdes.IV = first16;

		ICryptoTransform crypt = tdes.CreateDecryptor();

		byte[] cipher = crypt.TransformFinalBlock(message16, 0,message16.Length);

		sbyte[] signed = Array.ConvertAll(cipher, b => unchecked((sbyte)b));

		string result = System.Text.Encoding.UTF8.GetString(cipher);

		return result;
	}

}
