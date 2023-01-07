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
	[ExportEntities(Prefix = "IDR_", Suffix = null)]
	public static class KnownRelationshipGuids
	{
		public static Guid Class__has_super__Class { get; } = new Guid("{100F0308-855D-4EC5-99FA-D8976CA20053}");
		public static Guid Class__has_sub__Class { get; } = new Guid("{C14BC80D-879C-4E6F-9123-E8DFB13F4666}");

		public static Guid Class__has__Method { get; } = new Guid("{2DA28271-5E46-4472-A6F5-910B5DD6F608}");
		public static Guid Method__for__Class { get; } = new Guid("{76D2CE0D-AA4D-460B-B844-2BD76EF9902B}");

		public static Guid Class__has__Instance { get; } = new Guid("{7EB41D3C-2AE9-4884-83A4-E59441BCAEFB}");
		public static Guid Instance__for__Class { get; } = new Guid("{494D5A6D-04BE-477B-8763-E3F57D0DD8C8}");

		public static Guid Class__has__Relationship { get; } = new Guid("{3997CAAD-3BFA-4EA6-9CFA-2AA63E20F08D}");
		public static Guid Relationship__for__Class { get; } = new Guid("{C00BD63F-10DA-4751-B8EA-0905436EA098}");

		public static Guid Class__has_title__Translation { get; } = new Guid("{B8BDB905-69DD-49CD-B557-0781F7EF2C50}");

		public static Guid Class__has__Attribute { get; } = new Guid("{DECBB61A-2C6C-4BC8-9042-0B5B701E08DE}");
		public static Guid Attribute__for__Class { get; } = new Guid("{FFC8E435-B9F8-4495-8C85-4DDA67F7E2A8}");

		public static Guid Class__has_default__Task { get; } = new Guid("{CF396DAA-75EA-4148-8468-C66A71F2262D}");

		public static Guid Class__has_owner__User { get; } = new Guid("{D1A25625-C90F-4A73-A6F2-AFB530687705}");
		public static Guid User__owner_for__Class { get; } = new Guid("{04DD2E6B-EA57-4840-8DD5-F0AA943C37BB}");

		public static Guid Instance__for__Module { get; } = new Guid("{c60fda3c-2c0a-4229-80e8-f644e4471d28}");
		public static Guid Module__has__Instance { get; } = new Guid("{c3e5d7e3-5e11-4f8d-a212-b175327b2648}");

		public static Guid Class__has__Object_Source { get; } = new Guid("{B62F9B81-799B-4ABE-A4AF-29B45347DE54}");
		public static Guid Object_Source__for__Class { get; } = new Guid("{FBB9391D-C4A2-4326-9F85-7801F377253C}");

		public static Guid Relationship__has_source__Class { get; } = new Guid("{7FB5D234-042E-45CB-B11D-AD72F8D45BD3}");
		public static Guid Relationship__has_destination__Class { get; } = new Guid("{F220F1C2-0499-4E87-A32E-BDBF80C1F8A4}");
		public static Guid Relationship__has_sibling__Relationship { get; } = new Guid("{656110FF-4502-48B8-A7F3-D07F017AEA3F}");

		public static Guid Translation__has__Translation_Value { get; } = new Guid("{F9B60C00-FF1D-438F-AC74-6EDFA8DD7324}");
		public static Guid Translation_Value__has__Language { get; } = new Guid("{3655AEC2-E2C9-4DDE-8D98-0C4D3CE1E569}");

		public static Guid String__has__String_Component { get; } = new Guid("{3B6C4C25-B7BC-4242-8ED1-BA6D01B834BA}");
		public static Guid Extract_Single_Instance_String_Component__has__Relationship { get; } = new Guid("{5E499753-F50F-4A9E-BF53-DC013820499C}");
		public static Guid Instance_Attribute_String_Component__has__Attribute { get; } = new Guid("{E15D4277-69FB-4F19-92DB-8D087F361484}");
		public static Guid String_Component__has_source__Method { get; } = new Guid("{1ef1c965-e120-48be-b682-aa040573b5fb}");

		public static Guid Class__instance_labeled_by__String { get; } = new Guid("{F52FC851-D655-48A9-B526-C5FE0D7A29D2}");
		public static Guid Class__has_summary__Report_Field { get; } = new Guid("{D11050AD-7376-4AB7-84DE-E8D0336B74D2}");
		public static Guid Class__has_related__Task { get; } = new Guid("{4D8670E1-2AF1-4E7C-9C87-C910BD7B319B}");

		public static Guid Report_Object__has__Report_Field { get; } = new Guid("{0af62656-42bc-40a5-b0bc-dbba67c347f6}");
		public static Guid Report_Field__for__Report_Object { get; } = new Guid("{b46c8caa-3f46-465f-ba11-7d6f385425a2}");

		public static Guid Task__has_title__Translation { get; } = new Guid("{D97AE03C-261F-4060-A06D-985E26FA662C}");
		public static Guid Task__has_instructions__Translation { get; } = new Guid("{A9387898-9DC0-4006-94F1-1FB02EB3ECD7}");
		public static Guid Task__has__Prompt { get; } = new Guid("{929B106F-7E3E-4D30-BB84-E450A4FED063}");
		public static Guid Task__has__Task_Category { get; } = new Guid("{84048159-430d-4f6c-9361-115c8629c517}");
		public static Guid Task__executes__Method_Call { get; } = new Guid("{D8C0D4D4-F8C6-4B92-A2C1-8BF16B16203D}");
		public static Guid Task__has_preview_action_title__Translation { get; } = new Guid("{3f65ce02-11dd-4829-a46b-b9ea1b43e56a}");

		public static Guid Prompt__has__Report_Field { get; } = new Guid("{922CCB05-61EA-441D-96E0-63D58231D202}"); // 3de784b9-4561-42f0-946f-b1e90d80029e
		public static Guid Report_Field__for__Prompt { get; } = new Guid("{5DED3DB4-6864-45A9-A5FF-8E5A35AD6E6F}"); // b5f59216-a1f1-4979-8642-a4845e59daa8

		public static Guid Instance_Prompt__has_valid__Class { get; } = new Guid("{D5BD754B-F401-4FD8-A707-82684E7E25F0}");

		public static Guid Instance_Prompt_Value__has__Instance { get; } = new Guid("{512B518E-A892-44AB-AC35-4E9DBCABFF0B}");

		public static Guid Method__has__Method_Binding { get; } = new Guid("{D52500F1-1421-4B73-9987-223163BC9C04}");
		public static Guid Method_Binding__for__Method { get; } = new Guid("{B782A592-8AF5-4228-8296-E3D0B24C70A8}");

		public static Guid Method__has_return_type__Class { get; } = new Guid("{1241c599-e55d-4dcf-9200-d0e48c217ef8}");

		public static Guid Method_Binding__has__Parameter_Assignment { get; } = new Guid("{24938109-94f1-463a-9314-c49e667cf45b}");
		public static Guid Parameter_Assignment__for__Method_Binding { get; } = new Guid("{19c4a5db-fd26-44b8-b431-e081e6ffff8a}");

		public static Guid Parameter__has_data_type__Instance { get; } = new Guid("{ccb5200c-5faf-4a3a-9e8e-2edf5c2e0785}");
		public static Guid Instance__data_type_for__Parameter { get; } = new Guid("{ea1e6305-b2e4-4ba5-91b4-1ecfbebfa490}");

		public static Guid Instance_Set__has__Instance { get; } = new Guid("{7c9010a2-69f1-4029-99c8-72e05c78c41e}");

		public static Guid Parameter_Assignment__assigns_to__Parameter { get; } = new Guid("{a6d30e78-7bff-4fcc-b109-ee96681b0a9e}");
		public static Guid Parameter__assigned_from__Parameter_Assignment { get; } = new Guid("{2085341e-5e7e-4a7f-bb8d-dfa58f6030d9}");

		public static Guid Method_Binding__assigned_to__Parameter_Assignment { get; } = new Guid("{cbcb23b7-10c4-49eb-a1ca-b9da73fe8b83}");
		public static Guid Parameter_Assignment__assigns_from__Method_Binding { get; } = new Guid("{1e055d30-a968-49d8-93fe-541994fc0c51}");

		public static Guid Method_Call__has__Method { get; } = new Guid("{3D3B601B-4EF0-49F3-AF05-86CEA0F00619}");
		public static Guid Method_Call__has__Prompt_Value { get; } = new Guid("{765BD0C9-117D-4D0E-88C9-2CEBD4898135}");

		public static Guid Validation__has__Validation_Classification { get; } = new Guid("{BCDB6FFD-D2F2-4B63-BD7E-9C2CCD9547E0}");
		public static Guid Validation__has_true_condition__Executable { get; } = new Guid("{AA2D3B51-4153-4599-A983-6B4A13ADCBCB}");
		public static Guid Validation__has_false_condition__Executable { get; } = new Guid("{419047F8-852B-4A4D-B161-A8BD022FD8EB}");

		public static Guid Validation__has_failure_message__Translation { get; } = new Guid("{E15A97DD-2A1D-4DC0-BD6B-A957B63D9802}");
		public static Guid Translation__failure_message_for__Validation { get; } = new Guid("{46a7dfcb-8848-47d5-9ad3-d27fbd8b423f}");

		public static Guid Get_Attribute_Method__has__Attribute { get; } = new Guid("{5eca9b3f-be75-4f6e-8495-781480774833}");

		public static Guid Get_Referenced_Instance_Set_Method__loop_on__Instance_Set { get; } = new Guid("{2978238f-7cb0-4ba3-8c6f-473df782cfef}");
		public static Guid Get_Referenced_Instance_Set_Method__has_relationship__Method { get; } = new Guid("{6a65819e-c8cb-4575-9af8-ee221364049b}");

		public static Guid Get_Referenced_Attribute_Method__has__Attribute { get; } = new Guid("{87f90fe9-5ec6-4b09-8f51-b8a4d1544cae}");
		public static Guid Get_Referenced_Attribute_Method__loop_on__Instance_Set { get; } = new Guid("{c7ecd498-6d05-4e07-b1bc-f7127d0d6666}");

		public static Guid Get_Specific_Instances_Method__has__Instance { get; } = new Guid("{dea1aa0b-2bef-4bac-b4f9-0ce8cf7006fc}");

		public static Guid Evaluate_Boolean_Expression_Method__has_source_attribute__Method { get; } = new Guid("{45d76d56-01ed-4641-9f68-cfe0c7d0d265}");
		public static Guid Evaluate_Boolean_Expression_Method__equal_to_attribute__Method { get; } = new Guid("{0646df91-7e3e-4d59-be71-b978a22ced8e}");

		public static Guid Prompt_Value__has__Prompt { get; } = new Guid("{7CD62362-DDCE-4BFC-87B9-B5499B0BC141}");

		public static Guid User__has_display_name__Translatable_Text_Constant { get; } = new Guid("{6C29856C-3B10-4F5B-A291-DD3CA4C04A2F}");
		public static Guid User_Login__has__User { get; } = new Guid("{85B40E4B-849B-4006-A9C0-4E201B25975F}");

		public static Guid User__has_default__Page { get; } = new Guid("{f00cda6f-eded-4e0f-b6c5-9675ed664a75}");

		public static Guid Dashboard__has__Dashboard_Content { get; } = new Guid("{d26acf18-afa5-4ccd-8629-e1d9dac394ed}");
		public static Guid Dashboard_Content__for__Dashboard { get; } = new Guid("{9f236d2d-1f45-4096-a69c-42f37abbeebc}");
		public static Guid Dashboard_Content__has__Instance { get; } = new Guid("{1f0c4075-2b7d-42c2-8488-c7db06e91f5a}");
		public static Guid Instance__for__Dashboard_Content { get; } = new Guid("{376951c9-252b-4843-8e1d-ca89c94ddfa6}");

		public static Guid Return_Instance_Set_Method_Binding__has_source__Class { get; } = new Guid("{EE7A3049-8E09-410C-84CB-C2C0D652CF40}");

		public static Guid Report__has__Report_Column { get; } = new Guid("{7A8F57F1-A4F3-4BAF-84A5-E893FD79447D}");
		public static Guid Report__has__Report_Data_Source { get; } = new Guid("{1DE7B484-F9E3-476A-A9D3-7D2A86B55845}");
		public static Guid Report__has__Prompt { get; } = new Guid("{5D112697-303F-419F-886F-3F09F0670B07}");

		public static Guid Report_Column__has__Report_Field { get; } = new Guid("{B9026910-7E91-4EE1-B5F2-D7B740614831}");
		public static Guid Report_Column__has__Report_Column_Option { get; } = new Guid("{41FFF5F0-B467-4986-A6FD-46FAF4A479E9}");

		public static Guid Report_Data_Source__has_source__Method { get; } = new Guid("{2D5CB496-5839-46A0-9B94-30D4E2227B56}");

		public static Guid Report_Field__has_source__Method { get; } = new Guid("{5db86b76-69bf-421f-96e7-4c49452db82e}");
		public static Guid Attribute_Report_Field__has_target__Attribute { get; } = new Guid("{37964301-26FD-41D8-8661-1F73684C0E0A}");
		public static Guid Relationship_Report_Field__has_target__Relationship { get; } = new Guid("{134B2790-F6DF-4F97-9AB5-9878C4A715E5}");


		public static Guid Tenant__has__Application { get; } = new Guid("{22936f51-2629-4503-a30b-a02d61a6c0e0}");
		public static Guid Tenant__has_sidebar__Menu_Item { get; } = new Guid("{D62DFB9F-48D5-4697-AAAD-1CAD0EA7ECFA}");
		public static Guid Tenant__has__Tenant_Type { get; } = new Guid("{E94B6C9D-3307-4858-9726-F24B7DB21E2D}");

		public static Guid Tenant__has_company_logo_image__File { get; } = new Guid("{3540c81c-b229-4eac-b9b5-9d4b4c6ad1eb}");

		public static Guid Menu__has__Menu_Section { get; } = new Guid("{a22d949f-f8d1-4dcc-a3eb-d9f910228dfd}");
		public static Guid Menu_Item__has_title__Translatable_Text_Constant { get; } = new Guid("{65E3C87E-A2F7-4A33-9FA7-781EFA801E02}");
		public static Guid Menu_Section__has__Menu_Item { get; } = new Guid("{5b659d7c-58f9-453c-9826-dd3205c3c97f}");

		public static Guid Command_Menu_Item__has__Icon { get; } = new Guid("{8859DAEF-01F7-46FA-8F3E-7B2F28E0A520}");

		public static Guid Instance_Menu_Item__has_target__Instance { get; } = new Guid("{C599C20E-F01A-4B12-A919-5DC3B0F545C2}");

		public static Guid Page__has_title__Translation { get; } = new Guid("{7BE6522A-4BE8-4CD3-8701-C8353F7DF630}");
		public static Guid Page__has__Style { get; } = new Guid("{6E6E1A85-3EA9-4939-B13E-CBF645CB8B59}");
		public static Guid Page__has__Page_Component { get; } = new Guid("{24F6C596-D77D-4754-B023-00321DEBA924}");

		public static Guid Style__has__Style_Rule { get; } = new Guid("{4CC8A654-B2DF-4B17-A956-24939530790E}");
		public static Guid Style_Rule__has__Style_Property { get; } = new Guid("{B69C2708-E78D-413A-B491-ABB6F1D2A6E0}");

		public static Guid Page__has_master__Page { get; } = new Guid("{9bdbfd64-0915-419f-83fd-e8cf8bcc74ae}");
		public static Guid Page__master_for__Page { get; } = new Guid("{7fe8f2a2-c94d-4010-83aa-9300cc99d71d}");

		public static Guid Page_Component__has__Style { get; } = new Guid("{818CFF50-7D42-43B2-B6A7-92C3C54D450D}");
		public static Guid Style__for__Page_Component { get; } = new Guid("{007563E7-7277-4436-8C82-06D5F156D8E1}");

		public static Guid Button_Page_Component__has_text__Translatable_Text_Constant { get; } = new Guid("{C25230B1-4D23-4CFE-8B75-56C33E8293AF}");
		public static Guid Image_Page_Component__has_source__Method { get; } = new Guid("{481E3FBE-B82A-4C76-9DDF-D66C6BA8C590}");

		public static Guid Sequential_Container_Page_Component__has__Sequential_Container_Orientation { get; } = new Guid("{DD55F506-8718-4240-A894-21346656E804}");
		public static Guid Container_Page_Component__has__Page_Component { get; } = new Guid("{CB7B8162-1C9E-4E72-BBB8-C1C37CA69CD5}");

		public static Guid Panel_Page_Component__has_header__Page_Component { get; } = new Guid("{223B4073-F417-49CD-BCA1-0E0749144B9D}");
		public static Guid Panel_Page_Component__has_content__Page_Component { get; } = new Guid("{AD8C5FAE-2444-4700-896E-C5F968C0F85B}");
		public static Guid Panel_Page_Component__has_footer__Page_Component { get; } = new Guid("{56E339BD-6189-4BAC-AB83-999543FB8060}");

		public static Guid Element_Page_Component__has__Element { get; } = new Guid("{fe833426-e25d-4cde-8939-2a6c9baac20c}");
		public static Guid Element_Page_Component__has__Element_Content_Display_Option { get; } = new Guid("{74e3c13a-04fd-4f49-be70-05a32cdcdfe7}");
		public static Guid Element_Content_Display_Option__for__Element_Page_Component { get; } = new Guid("{7d3a7045-0925-49db-9b7d-24863c9848a6}");

		public static Guid Element__for__Element_Page_Component { get; } = new Guid("{963c5c60-3979-47fa-b201-a26839b9ded9}");

		public static Guid Tenant__has_logo_image__File { get; } = new Guid("{4C399E80-ECA2-4A68-BFB4-26A5E6E97047}");
		public static Guid Tenant__has_background_image__File { get; } = new Guid("{39B0D963-4BE0-49C8-BFA2-607051CB0101}");
		public static Guid Tenant__has_icon_image__File { get; } = new Guid("{CC4E65BD-7AAA-40DA-AECA-C607D7042CE3}");
		public static Guid Tenant__has_login_header__Translatable_Text_Constant { get; } = new Guid("{41D66ACB-AFDE-4B6F-892D-E66255F10DEB}");
		public static Guid Tenant__has_login_footer__Translatable_Text_Constant { get; } = new Guid("{A6203B6B-5BEB-4008-AE49-DB5E7DDBA45B}");
		public static Guid Tenant__has_application_title__Translation { get; } = new Guid("{76683437-67ba-46d9-a5e7-2945be635345}");
		public static Guid Tenant__has_mega__Menu { get; } = new Guid("{cdd743cb-c74a-4671-9922-652c7db9f2d8}");

		public static Guid Tenant_Type__has_title__Translatable_Text_Constant { get; } = new Guid("{79AAE09C-5690-471C-8442-1B230610456C}");

		public static Guid Prompt__has_title__Translatable_Text_Constant { get; } = new Guid("{081ee211-7534-43c4-99b5-24bd9537babc}");

		public static Guid Report__has_title__Translatable_Text_Constant { get; } = new Guid("{DF93EFB0-8B5E-49E7-8BC0-553F9E7602F9}");
		public static Guid Report__has_description__Translatable_Text_Constant { get; } = new Guid("{D5AA18A7-7ACD-4792-B039-6C620A151BAD}");
		public static Guid Report_Field__has_title__Translatable_Text_Constant { get; } = new Guid("{6780BFC2-DBC0-40AE-83EE-BFEF979F0054}");

		public static Guid Content_Page_Component__gets_content_from__Method { get; } = new Guid("{0E002E6F-AA79-457C-93B8-2CCE1AEF5F7E}");
		public static Guid Method__provides_content_for__Content_Page_Component { get; } = new Guid("{5E75000D-2421-4AD4-9E5F-B9FDD9CB4744}");

		public static Guid Securable_Item__secured_by__Method { get; } = new Guid("{15199c49-9595-4288-846d-13b0ad5dcd4b}");
		public static Guid Get_Relationship_Method__has__Relationship { get; } = new Guid("{321581d6-60c1-4547-8344-9d5bda027adc}");

		public static Guid Integration_ID__has__Integration_ID_Usage { get; } = new Guid("{6cd30735-df83-4253-aabe-360d6e1a3701}");
		public static Guid Integration_ID_Usage__for__Integration_ID { get; } = new Guid("{d8d981ec-7686-40da-b89f-006c85042444}");

		public static Guid Conditional_Method__has__Conditional_Method_Case { get; } = new Guid("{df2059e6-650c-49a8-8188-570ccbe4fd2d}");
		public static Guid Conditional_Method_Case__for__Conditional_Method { get; } = new Guid("{be7a6285-d700-40f3-868e-c0878a3e94a6}");

		public static Guid Conditional_Method_Case__has_return_attribute__Method_Binding { get; } = new Guid("{b6715132-b438-4073-b81d-3ecf19584b7d}");
		public static Guid Method_Binding__returns_attribute_for__Conditional_Method_Case { get; } = new Guid("{1c868a06-8fb7-432d-839b-7a5a02a50eb6}");

		public static Guid Conditional_Method_Case__has_true_condition__Method_Binding { get; } = new Guid("{d955da3f-7ef4-4374-8b86-167c73434f75}");
		public static Guid Method_Binding__true_condition_for__Conditional_Method_Case { get; } = new Guid("{1acdefd1-e1b4-45bb-99e1-bf73dea71da5}");

		public static Guid Conditional_Method_Case__has_false_condition__Method_Binding { get; } = new Guid("{e46dbc4f-ae8d-4ad8-b9e6-10cedfa68b73}");
		public static Guid Method_Binding__false_condition_for__Conditional_Method_Case { get; } = new Guid("{633af008-bf7f-4da1-94d6-67a9a80586d6}");

		public static Guid Audit_Line__has__Instance { get; } = new Guid("{c91807ed-0d73-4729-990b-d90750764fb5}");
		public static Guid Audit_Line__has__User { get; } = new Guid("{7c1e048d-3dcb-4473-9f2e-e21014a76aa5}");

		public static Guid Method__has__Method_Parameter { get; } = new Guid("{c455dc79-ba9b-4a7c-af8e-9ca59dbe511f}");
		public static Guid Method_Parameter__for__Method { get; } = new Guid("{0bcb6e5b-5885-4747-843c-ed4c3d3dc234}");
		public static Guid Method__returns__Attribute { get; } = new Guid("{eb015d32-0d4f-4647-b9b8-715097f4434b}");

		public static Guid Detail_Page_Component__has_caption__Translation { get; } = new Guid("{4a15fa44-fb7b-4e26-8ce2-f36652792b48}");

		public static Guid Element__has__Element_Content { get; } = new Guid("{c1d32481-02f9-48c6-baf8-37d93fa8da23}");
		public static Guid Element_Content__for__Element { get; } = new Guid("{2eff7f58-0edd-40b7-9c06-00774257649e}");

		public static Guid Element__has_label__Translation { get; } = new Guid("{7147ea90-9f45-4bb9-b151-025b6e2bd834}");

		public static Guid Element_Content__has__Instance { get; } = new Guid("{315b71ba-953d-45fc-87e5-4f0a268242a9}");
		public static Guid Instance__for__Element_Content { get; } = new Guid("{c3959f84-248d-4ede-a3f2-f262917c7b56}");

		public static Guid Element_Content__has__Element_Content_Display_Option { get; } = new Guid("{f070dfa7-6260-4488-a779-fae291903f2d}");

		public static Guid Element_Content__has__Parameter_Assignment { get; } = new Guid("{51214ef0-458a-44fa-8b9d-f3d9d2309388}");
		public static Guid Parameter__assigned_from__Element_Content { get; } = new Guid("{dcef180b-a2ca-4d87-bb05-b946c319b562}");

		public static Guid Element_Content__has__Validation { get; } = new Guid("{265637cd-2099-416b-88fa-4f5ed88a87e3}");
		public static Guid Validation__for__Element_Content { get; } = new Guid("{3a4677e8-9c78-4149-80ad-46e5ac3b12f5}");

		public static Guid Instance__has__Instance_Definition { get; } = new Guid("{329c54ee-17b8-4550-ae80-be5dee9ac53c}");

		public static Guid Task__has_initiating__Element { get; } = new Guid("{78726736-f5b7-4466-b114-29cbaf6c9329}");
		public static Guid Element__initiates_for__Task { get; } = new Guid("{36964c5d-348e-4f88-8a62-0a795b43bc14}");

		public static Guid Detail_Page_Component__has_row_source__Method_Binding { get; } = new Guid("{54FBD056-0BD4-44F4-921C-11FB0C77996E}");
		public static Guid Detail_Page_Component__has_column_source__Method_Binding { get; } = new Guid("{ddabeeda-aa26-4d87-a457-4e7da921a293}");

		public static Guid Work_Set__has_valid__Class { get; } = new Guid("{08087462-41a5-4271-84bc-a9bcd31a2c21}");
		public static Guid Class__valid_for__Work_Set { get; } = new Guid("{73c65dcf-7810-47d4-98c0-898ca1b17a68}");

		public static Guid Conditional_Select_Attribute_Case__invokes__Executable_returning_Attribute { get; } = new Guid("{dbd97430-9c55-430d-815c-77fce9887ba7}");
		public static Guid Executable_returning_Attribute__invoked_by__Conditional_Select_Attribute_Case { get; } = new Guid("{f4b04072-abe8-452a-b8f8-e0369dde24d4}");

		public static Guid Style__gets_background_image_File_from__Return_Instance_Set_Method_Binding { get; } = new Guid("{4b4a0a3e-426c-4f70-bc15-b6efeb338486}");

		public static Guid Style__has__Style_Class { get; } = new Guid("{2cebd830-52aa-44ff-818d-b2d6ee273a1f}");
		public static Guid Style_Class__for__Style { get; } = new Guid("{b2fbce51-455a-42b5-9fd1-c28bb0cbe613}");

		public static Guid Style__implements__Style { get; } = new Guid("{99b1c16a-f2cb-4cc5-a3bb-82a96335aa39}");
		public static Guid Style__implemented_for__Style { get; } = new Guid("{271ef816-1e94-4f02-a805-4f9536c28dca}");
	}
}
