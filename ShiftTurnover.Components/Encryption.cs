using System;
using System.Security.Cryptography;
using System.Text;

namespace ShiftTurnover.Components
{
	/// <summary>
	/// Summary description for Encryption.
	/// </summary>
	public class Encryption
	{
		public Encryption()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region ENCRYPTION ... 
		/// <summary>
		/// Encrypts a string
		/// </summary>
		/// <param name="originalString">the string to be encrypted</param>
		/// <param name="key">the password to use as a key</param>
		/// <returns>the encrypted string</returns>
		public string Encrypt(string original)
		{
			return Encrypt(original, (string) getKey());
		}
		private string Encrypt(string original, string key)
		{
			TripleDESCryptoServiceProvider des;
			MD5CryptoServiceProvider hashmd5;
			byte[] keyhash, buff;
			string encrypted;
			hashmd5 = new MD5CryptoServiceProvider();
			keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
			hashmd5 = null;
			des = new TripleDESCryptoServiceProvider();
			des.Key = keyhash;
			des.Mode = CipherMode.ECB;
			buff = ASCIIEncoding.ASCII.GetBytes(original);
			encrypted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
			return encrypted;
		}
		/// <summary>
		/// Decrypts a string
		/// </summary>
		/// <param name="encrypted">the encrypted string</param>
		/// <param name="key">the key used in encryption</param>
		/// <returns>the decrypted string</returns>
		public string Decrypt(string original)
		{
			return Decrypt(original, (string) getKey());
		}
		private string Decrypt(string encrypted, string key)
		{
			TripleDESCryptoServiceProvider des;
			MD5CryptoServiceProvider hashmd5;
			byte[] keyhash, buff;
			string decrypted;
			hashmd5 = new MD5CryptoServiceProvider();
			keyhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
			hashmd5 = null;
			des = new TripleDESCryptoServiceProvider();
			des.Key = keyhash;
			des.Mode = CipherMode.ECB;
			buff = Convert.FromBase64String(encrypted);
			decrypted = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
			return decrypted;
		}
		private string getKey()
		{
			return "aTESix7fIvE.309_E-ine";
		}
		#endregion

	}
}
