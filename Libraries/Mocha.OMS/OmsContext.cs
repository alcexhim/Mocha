//
//  OmsContext.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2022 Mike Becker's Software
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
using Mocha.Core;

namespace Mocha.OMS
{
	public class OmsContext
	{
		public OmsMessage.OmsMessageCollection Messages { get; } = new OmsMessage.OmsMessageCollection();
		public System.Collections.Generic.Stack<OmsCallStack> CallStack { get; } = new System.Collections.Generic.Stack<OmsCallStack>();
		public OmsVariable.PromptValueCollection LocalVariables { get { return CallStack.Peek().PromptValues; } }

		public OmsVariable.PromptValueCollection GlobalVariables { get; } = new OmsVariable.PromptValueCollection();
		public bool IsPostback { get; set; } = false;
		public Instance RelatedInstance { get; set; } = null;

		public string PrintStackTrace()
		{
			int i = 0;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (OmsCallStack callStack in CallStack)
			{
				sb.Append(callStack.InstanceKey.ToString());
				i++;
				if (i < CallStack.Count)
				{
					sb.Append(":");
				}
			}
			return sb.ToString();
		}

		private System.Collections.Generic.Dictionary<Guid, object> _workdata = new System.Collections.Generic.Dictionary<Guid, object>();
		public void SetWorkData(Guid related_Instance, object value)
		{
			_workdata[related_Instance] = value;
		}
		public T GetWorkData<T>(Guid key, T defaultValue = default(T))
		{
			if (_workdata.ContainsKey(key))
			{
				if (_workdata[key] is T)
				{
					return (T)_workdata[key];
				}
			}
			return defaultValue;
		}
	}
}
