//
//  SessionContext.cs
//
//  Author:
//       beckermj <>
//
//  Copyright (c) 2022 ${CopyrightHolder}
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web.Server
{
	public class SessionContext
	{
		public string DefaultApplicationName { get; set; } = "Mocha";

		private string _ApplicationName = null;
		public string ApplicationName
		{
			get
			{
				if (_ApplicationName != null)
					return _ApplicationName;
				return DefaultApplicationName;
			}
			set
			{
				_ApplicationName = value;
			}
		}

		public string TenantName { get; set; }

		public string PageTitle
		{
			get
			{
				if (TenantName != null)
				{
					return String.Format("{0} {1}", ApplicationName, TenantName);
				}
				return ApplicationName;
			}
		}

		private static System.Collections.Generic.Dictionary<string, Oms> _Oms = new System.Collections.Generic.Dictionary<string, Oms>();


		private static Dictionary<string, Dictionary<InstanceKey, byte[]>> entropy = new Dictionary<string, Dictionary<InstanceKey, byte[]>>();
		public Dictionary<InstanceKey, byte[]> GetOmsAttachmentEntropy()
		{
			lock (entropy)
			{
				if (!entropy.ContainsKey(TenantName))
				{
					entropy[TenantName] = new Dictionary<InstanceKey, byte[]>();
				}
			}
			return entropy[TenantName];
		}

		public Oms GetOms()
		{
			if (!_Oms.ContainsKey(TenantName))
			{
				Mocha.OMS.Oms oms = new Mocha.OMS.LocalOms();
				((Mocha.OMS.LocalOms)oms).Environment = new OMS.OmsEnvironment(new Mocha.Storage.Local.LocalStorageProvider("/usr/share/mocha/system"));
				((Mocha.OMS.LocalOms)oms).Environment.Initialize();
				oms.TenantName = TenantName;

				_Oms[TenantName] = oms;
			}
			return _Oms[TenantName];
		}

		public string ApplicationPath { get; set; }

		public string GetAttachmentUrl(Instance instFile, Dictionary<InstanceKey, byte[]> dictionary)
		{
			return ApplicationPath + GetOms().GetAttachmentUrl(instFile, dictionary);
		}
	}
}
