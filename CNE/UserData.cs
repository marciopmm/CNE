using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CNE
{
	public class UserData
	{
		public static string Load()
		{		
			byte[] bytes = null;
#if WINDOWS_PHONE
			StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
			if (local != null) {
				var file = local.GetItem(Constants.UserDataFileName);
				using (StreamReader sr = new StreamReader(file.Path)) {
				bytes = sr.ReadBytes();
				}
			}
#else
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(docPath, Constants.UserDataFileName);

			if (File.Exists(filePath)) {
				bytes = File.ReadAllBytes(filePath);
			}
			#endif

			if (bytes != null)
				return Decrypt (bytes);
			else
				return null;
		}

		public static void Save(string sessionId)
		{
			byte[] bytes = Encrypt (sessionId);

			#if WINDOWS_PHONE
			StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
			var file = local.CreateFile(filename, CreateCollisionOption.ReplaceExisting);
			using (StreamWriter sw = new StreamWriter(file.OpenStreamForWrite())) {
				sw.WriteBytes(bytes);
			}
			#else
			string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(docPath, Constants.UserDataFileName);
			File.WriteAllBytes(filePath, bytes);
			#endif
		}

		private static byte[] Encrypt(string data)
		{
			byte[] toEncryptArray = Encoding.UTF8.GetBytes (data);
				
			// Cria um hash em cima da chave
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			var keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Constants.CryptoKey));
			hashmd5.Clear();

			// Gera o provedor de Criptografia
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			// Aplica o algoritmo Criptográfico
			ICryptoTransform cryptoTransform = tdes.CreateEncryptor();
			byte[] resultArray = cryptoTransform.TransformFinalBlock
				(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();

			return resultArray;
		}

		private static string Decrypt(byte[] bytes)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Constants.CryptoKey));
			hashmd5.Clear();

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform criptoTransform = tdes.CreateDecryptor();
			byte[] resultArray = criptoTransform.TransformFinalBlock
				(bytes, 0, bytes.Length);
			tdes.Clear();

			return Encoding.UTF8.GetString(resultArray);
		}
	}	
}

