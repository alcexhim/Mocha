using System;
namespace Mocha.Web
{
	public struct LoginTokenInfo
	{
		private bool _IsValid;
		public bool IsEmpty { get { return !_IsValid; } }

		public Guid LoginToken;
		public DateTime Expires;

		public LoginTokenInfo(Guid loginToken, DateTime expires)
		{
			LoginToken = loginToken;
			Expires = expires;
			_IsValid = true;
		}
	}
}
