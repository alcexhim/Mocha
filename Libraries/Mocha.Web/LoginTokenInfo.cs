using System;
namespace Mocha.Web
{
	public struct LoginTokenInfo
	{
		private bool _IsValid;
		public bool IsEmpty { get { return !_IsValid; } }

		public Guid LoginToken;
		public DateTime Expires;

		public string UserName;
		public string TenantName;

		public LoginTokenInfo(Guid loginToken, DateTime expires, string userName, string tenantName)
		{
			LoginToken = loginToken;
			Expires = expires;
			_IsValid = true;
			UserName = userName;
			TenantName = tenantName;
		}
	}
}
