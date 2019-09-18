using System;

namespace Mocha
{
	public static class Authentication
	{
		public static string HashPass(string pass, string salt)
		{
			System.Security.Cryptography.HashAlgorithm ha = new System.Security.Cryptography.SHA512Managed();

			ha.Initialize();
			byte[] result = ha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass + salt));

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				sb.Append(result[i].ToString("x").PadLeft(2, '0'));
			}
			return sb.ToString();
		}
		public static string RandomSalt()
		{
			string w = Guid.NewGuid().ToString();
			w = w.Replace("-", String.Empty);
			return w;
		}
	}
}