//
//  KnownRelationshipGuids.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
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
namespace Mocha.Core
{
	public static class KnownRelationshipGuids
	{
		public static Guid Class__has_super__Class => new Guid("{100F0308-855D-4EC5-99FA-D8976CA20053}");
		public static Guid Class__has_sub__Class => new Guid("{C14BC80D-879C-4E6F-9123-E8DFB13F4666}");

		public static Guid Class__has__Instance => new Guid("{7EB41D3C-2AE9-4884-83A4-E59441BCAEFB}");
		public static Guid Instance__for__Class => new Guid("{494D5A6D-04BE-477B-8763-E3F57D0DD8C8}");

		public static Guid Class__has__Attribute => new Guid("{DECBB61A-2C6C-4BC8-9042-0B5B701E08DE}");
		public static Guid Class__has_title__Translatable_Text_Constant => new Guid("{B8BDB905-69DD-49CD-B557-0781F7EF2C50}");
		public static Guid Class__has_default__Task => new Guid("{CF396DAA-75EA-4148-8468-C66A71F2262D}");

		public static Guid Class__has_owner__User => new Guid("{D1A25625-C90F-4A73-A6F2-AFB530687705}");
		public static Guid User__owner_for__Class => new Guid("{04DD2E6B-EA57-4840-8DD5-F0AA943C37BB}");

		public static Guid Class__has__Object_Source => new Guid("{B62F9B81-799B-4ABE-A4AF-29B45347DE54}");
		public static Guid Object_Source__for__Class => new Guid("{FBB9391D-C4A2-4326-9F85-7801F377253C}");

		public static Guid Relationship__has_source__Class => new Guid("{7FB5D234-042E-45CB-B11D-AD72F8D45BD3}");
		public static Guid Class__source_for__Relationship => new Guid("{20FFFDE8-11A5-48D6-894B-21C6B234B811}");
		public static Guid Relationship__has_destination__Class => new Guid("{F220F1C2-0499-4E87-A32E-BDBF80C1F8A4}");
		public static Guid Class__destination_for__Relationship => new Guid("{A66CD08C-A155-42AF-8995-A1D96C5A0C06}");
		public static Guid Relationship__has_sibling__Relationship => new Guid("{656110FF-4502-48B8-A7F3-D07F017AEA3F}");

		public static Guid Translatable_Text_Constant__has__Translatable_Text_Constant_Value => new Guid("{F9B60C00-FF1D-438F-AC74-6EDFA8DD7324}");
		public static Guid Translatable_Text_Constant_Value__has__Language => new Guid("{3655AEC2-E2C9-4DDE-8D98-0C4D3CE1E569}");

		public static Guid String__has__String_Component => new Guid("{3B6C4C25-B7BC-4242-8ED1-BA6D01B834BA}");
		public static Guid Extract_Single_Instance_String_Component__has__Relationship => new Guid("{5E499753-F50F-4A9E-BF53-DC013820499C}");
		public static Guid Instance_Attribute_String_Component__has__Attribute => new Guid("{E15D4277-69FB-4F19-92DB-8D087F361484}");

		public static Guid Class__instance_labeled_by__String => new Guid("{F52FC851-D655-48A9-B526-C5FE0D7A29D2}");
		public static Guid Class__has_summary__Report_Field => new Guid("{D11050AD-7376-4AB7-84DE-E8D0336B74D2}");

		public static Guid Task__has_title__Translatable_Text_Constant => new Guid("{D97AE03C-261F-4060-A06D-985E26FA662C}");
		public static Guid Task__has_instructions__Translatable_Text_Constant => new Guid("{A9387898-9DC0-4006-94F1-1FB02EB3ECD7}");
		public static Guid Task__has__Prompt => new Guid("{929B106F-7E3E-4D30-BB84-E450A4FED063}");
		public static Guid Task__executes__Method_Call => new Guid("{D8C0D4D4-F8C6-4B92-A2C1-8BF16B16203D}");

		public static Guid Instance_Prompt__has_valid__Class => new Guid("{D5BD754B-F401-4FD8-A707-82684E7E25F0}");

		public static Guid Instance_Prompt_Value__has__Instance => new Guid("{512B518E-A892-44AB-AC35-4E9DBCABFF0B}");

		public static Guid Method__has__Method_Binding => new Guid("{D52500F1-1421-4B73-9987-223163BC9C04}");

		public static Guid Method_Call__has__Method => new Guid("{3D3B601B-4EF0-49F3-AF05-86CEA0F00619}");
		public static Guid Method_Call__has__Prompt_Value => new Guid("{765BD0C9-117D-4D0E-88C9-2CEBD4898135}");

		public static Guid Prompt_Value__has__Prompt => new Guid("{7CD62362-DDCE-4BFC-87B9-B5499B0BC141}");

		public static Guid User__has_display_name__Translatable_Text_Constant => new Guid("{6C29856C-3B10-4F5B-A291-DD3CA4C04A2F}");

		public static Guid Return_Instance_Set_Method_Binding__has_source__Class => new Guid("{EE7A3049-8E09-410C-84CB-C2C0D652CF40}");

		public static Guid Report__has__Report_Column => new Guid("{7A8F57F1-A4F3-4BAF-84A5-E893FD79447D}");
		public static Guid Report__has__Report_Data_Source => new Guid("{1DE7B484-F9E3-476A-A9D3-7D2A86B55845}");
		public static Guid Report__has__Prompt => new Guid("{5D112697-303F-419F-886F-3F09F0670B07}");

		public static Guid Report_Column__has__Report_Field => new Guid("{B9026910-7E91-4EE1-B5F2-D7B740614831}");

		public static Guid Report_Data_Source__has_source__Method => new Guid("{2D5CB496-5839-46A0-9B94-30D4E2227B56}");

		public static Guid Attribute_Report_Field__has_target__Attribute => new Guid("{37964301-26FD-41D8-8661-1F73684C0E0A}");
		public static Guid Relationship_Report_Field__has_target__Relationship => new Guid("{134B2790-F6DF-4F97-9AB5-9878C4A715E5}");

		public static Guid Page__has_title__Translatable_Text_Constant => new Guid("{7BE6522A-4BE8-4CD3-8701-C8353F7DF630}");

		public static Guid Tenant__has_sidebar__Menu_Item => new Guid("{D62DFB9F-48D5-4697-AAAD-1CAD0EA7ECFA}");
		public static Guid Tenant__has__Tenant_Type => new Guid("{E94B6C9D-3307-4858-9726-F24B7DB21E2D}");

		public static Guid Menu_Item__has_title__Translatable_Text_Constant => new Guid("{65E3C87E-A2F7-4A33-9FA7-781EFA801E02}");

		public static Guid Command_Menu_Item__has__Icon => new Guid("{8859DAEF-01F7-46FA-8F3E-7B2F28E0A520}");

		public static Guid Instance_Menu_Item__has_target__Instance => new Guid("{C599C20E-F01A-4B12-A919-5DC3B0F545C2}");

		public static Guid Page__has__Style => new Guid("{6E6E1A85-3EA9-4939-B13E-CBF645CB8B59}");
		public static Guid Page__has__Page_Component => new Guid("{24F6C596-D77D-4754-B023-00321DEBA924}");
		public static Guid Page_Component__has__Style => new Guid("{818CFF50-7D42-43B2-B6A7-92C3C54D450D}");

		public static Guid Button_Page_Component__has_text__Translatable_Text_Constant => new Guid("{C25230B1-4D23-4CFE-8B75-56C33E8293AF}");
		public static Guid Image_Page_Component__has_target__File => new Guid("{481E3FBE-B82A-4C76-9DDF-D66C6BA8C590}");

		public static Guid Sequential_Container_Page_Component__has__Sequential_Container_Orientation => new Guid("{DD55F506-8718-4240-A894-21346656E804}");
		public static Guid Container_Page_Component__has__Page_Component => new Guid("{CB7B8162-1C9E-4E72-BBB8-C1C37CA69CD5}");

		public static Guid Panel_Page_Component__has_header__Page_Component => new Guid("{223B4073-F417-49CD-BCA1-0E0749144B9D}");
		public static Guid Panel_Page_Component__has_content__Page_Component => new Guid("{AD8C5FAE-2444-4700-896E-C5F968C0F85B}");
		public static Guid Panel_Page_Component__has_footer__Page_Component => new Guid("{56E339BD-6189-4BAC-AB83-999543FB8060}");

		public static Guid Tenant__has_logo_image__File => new Guid("{4C399E80-ECA2-4A68-BFB4-26A5E6E97047}");
		public static Guid Tenant__has_background_image__File => new Guid("{39B0D963-4BE0-49C8-BFA2-607051CB0101}");
		public static Guid Tenant__has_icon_image__File => new Guid("{CC4E65BD-7AAA-40DA-AECA-C607D7042CE3}");
		public static Guid Tenant__has_login_header__Translatable_Text_Constant => new Guid("{41D66ACB-AFDE-4B6F-892D-E66255F10DEB}");
		public static Guid Tenant__has_login_footer__Translatable_Text_Constant => new Guid("{A6203B6B-5BEB-4008-AE49-DB5E7DDBA45B}");

		public static Guid Tenant_Type__has_title__Translatable_Text_Constant => new Guid("{79AAE09C-5690-471C-8442-1B230610456C}");

		public static Guid Prompt__has_title__Translatable_Text_Constant => new Guid("{7FB5D234-042E-45CB-B11D-AD72F8D45BD3}");

		public static Guid Report__has_title__Translatable_Text_Constant => new Guid("{DF93EFB0-8B5E-49E7-8BC0-553F9E7602F9}");
		public static Guid Report__has_description__Translatable_Text_Constant => new Guid("{D5AA18A7-7ACD-4792-B039-6C620A151BAD}");
		public static Guid Report_Field__has_title__Translatable_Text_Constant => new Guid("{6780BFC2-DBC0-40AE-83EE-BFEF979F0054}");

		public static Guid Standard_Report__has__Report_Column => new Guid("{7A8F57F1-A4F3-4BAF-84A5-E893FD79447D}");
	}
}
