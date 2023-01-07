//
//  KnownResponses..cs
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
using MBS.Networking.Protocols.HyperTextTransfer;
namespace Mocha.OMS
{
	public static class KnownResponses
	{
		private const string PROTOCOL = "OMS/1.0";

		public static Response OK { get { return new Response(200, "OK", PROTOCOL); } }

		public static Response BadRequest { get { return new Response(400, "Bad Request", PROTOCOL); } }
		public static Response NotFound { get { return new Response(404, "Not Found", PROTOCOL); } }
	}
}
