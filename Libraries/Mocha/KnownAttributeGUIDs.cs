//
//  KnownAttributeGUIDs.cs
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
namespace Mocha
{
	public static class KnownAttributeGUIDs
	{
		public static readonly Guid UserName = new Guid("{960FAF02-5C59-40F7-91A7-20012A99D9ED}");
		public static readonly Guid PasswordHash = new Guid("{F377FC29-4DF1-4AFB-9643-4191F37A00A9}");
		public static readonly Guid PasswordSalt = new Guid("{8C5A99BC-40ED-4FA2-B23F-F373C1F3F4BE}");
		public static readonly Guid Value = new Guid("{041DD7FD-2D9C-412B-8B9D-D7125C166FE0}");
	}
}
