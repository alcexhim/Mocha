//
//  InstanceRequestedQueryType.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2019 
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
using System.ComponentModel;
using Mocha.Core;

namespace Mocha.OMS
{
	public enum InstanceRequestedQueryType
	{
		None = 0,
		All,
		SpecificInstance
	}
	public enum InstanceRequestedIDType
	{
		None = 0,
		GlobalIdentifier,
		InstanceId
	}
	public class InstanceRequestedEventArgs : CancelEventArgs
	{
		public string Query { get; private set; } = null;
		public InstanceRequestedQueryType QueryType { get; private set; } = InstanceRequestedQueryType.None;
		public InstanceRequestedIDType IDType { get; private set; } = InstanceRequestedIDType.GlobalIdentifier;
		public Instance[] Instances { get; set; } = null;

		public InstanceRequestedEventArgs(InstanceRequestedQueryType queryType, InstanceRequestedIDType idType, string query)
		{
			QueryType = queryType;
			IDType = idType;
			Query = query;
		}
	}
	public delegate void InstanceRequestedEventHandler(object sender, InstanceRequestedEventArgs e);
}
