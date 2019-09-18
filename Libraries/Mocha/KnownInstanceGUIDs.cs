//
//  KnownInstanceGUIDs.cs
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
	public static class KnownInstanceGUIDs
	{
		public static readonly Guid Class = new Guid("{B9C9B9B7-AD8A-4CBD-AA6b-E05784630B6B}");
		public static readonly Guid User = new Guid("{9C6871C1-9A7f-4A3A-900E-69D1D9E24486}");
		public static readonly Guid TranslatableTextConstant = new Guid("{04A53CC8-3206-4A97-99C5-464DB8CAA6E6}");
		public static readonly Guid TranslatableTextConstantValue = new Guid("{6D38E757-EC18-43AD-9C35-D15BB446C0E1}");

		public static class Relationship
		{
			public static readonly Guid Class__has_super__Class = new Guid("{100F0308-855D-4EC5-99FA-D8976CA20053}");
			public static readonly Guid Class__has_sub__Class = new Guid("{C14BC80D-879C-4E6F-9123-E8DFB13F4666}");
			public static readonly Guid Class__instance_labeled_by__String = new Guid("{F52FC851-D655-48A9-B526-C5FE0D7A29D2}");
			public static readonly Guid Class__has_title__Translatable_Text_Constant = new Guid("{B8BDB905-69DD-49CD-B557-0781F7EF2C50}");
			public static readonly Guid Instance__for__Class = new Guid("{494D5A6D-04BE-477B-8763-E3F57D0DD8C8}");
			public static readonly Guid Report__has_title__Translatable_Text_Constant = new Guid("{DF93EFB0-8B5E-49E7-8BC0-553F9E7602F9}");
			public static readonly Guid Report__has__Report_Column = new Guid("{7A8F57F1-A4F3-4BAF-84A5-E893FD79447D}");
			public static readonly Guid Report__has__Report_Data_Source = new Guid("{1DE7B484-F9E3-476A-A9D3-7D2A86B55845}");
			public static readonly Guid Relationship___Class__has_title__Translatable_Text_Constant = new Guid("{B8BDB905-69DD-49CD-B557-0781F7EF2C50}");
			public static readonly Guid Report_Data_Source__has_source__Method = new Guid("{2D5CB496-5839-46A0-9B94-30D4E2227B56}");
			public static readonly Guid Method__has__Method_Binding = new Guid("{D52500F1-1421-4B73-9987-223163BC9C04}");
			public static readonly Guid Select_Instance_Set_Method_Binding__has_source__Class = new Guid("{EE7A3049-8E09-410C-84CB-C2C0D652CF40}");
			public static readonly Guid String__has__String_Component = new Guid("{3B6C4C25-B7BC-4242-8ED1-BA6D01B834BA}");
			public static readonly Guid Translatable_Text_Constant__has__Translatable_Text_Constant_Value = new Guid("{F9B60C00-FF1D-438F-AC74-6EDFA8DD7324}");
			public static readonly Guid Extract_Single_Instance_String_Component__has__Relationship = new Guid("{5E499753-F50F-4A9E-BF53-DC013820499C}");
			public static readonly Guid Report_Column__has__Report_Field = new Guid("{B9026910-7E91-4EE1-B5F2-D7B740614831}");
			public static readonly Guid Class__has_default__Task = new Guid("{CF396DAA-75EA-4148-8468-C66A71F2262D}");
		}
	}
}
