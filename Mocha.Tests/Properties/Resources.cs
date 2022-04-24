//
//  Resources.cs
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
namespace Mocha.Tests.Properties
{
	public static class Resources
	{
		public static byte[] Mocha_Core_v1_0_mcl { get; private set; } = null;

		static Resources()
		{
			System.Reflection.Assembly asm = typeof(Resources).Assembly;
			System.IO.Stream st = asm.GetManifestResourceStream("Mocha.Tests.Properties.Resources.Mocha.Core_v1.0.mcl");

			byte[] buf = new byte[st.Length];
			st.Read(buf, 0, (int)st.Length);

			Mocha_Core_v1_0_mcl = buf;
		}
	}
}
