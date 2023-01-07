using System;
namespace Mocha.Core
{
	public static class KnownInstanceGuids
	{
		[ExportEntities(Prefix = "IDC_", Suffix = null)]
		public static class Classes
		{
			public static Guid Class { get; } = new Guid("{B9C9B9B7-AD8A-4CBD-AA6B-E05784630B6B}");
			public static Guid Attribute { get; } = new Guid("{F9CD7751-EF62-4F7C-8A28-EBE90B8F46AA}");
			public static Guid Relationship { get; } = new Guid("{9B0A80F9-C325-4D36-997C-FB4106204648}");

			public static Guid InstanceDefinition { get; } = new Guid("{ee26f146-0b89-4cfe-a1af-ae6ac3533eae}");

			public static Guid Enumeration { get; } = new Guid("{70c3ee17-4d54-4342-9e7b-527cf73c93dd}");

			// Attribute subclasses
			public static Guid TextAttribute { get; } = new Guid("{C2F36542-60C3-4B9E-9A96-CA9B309C43AF}");
			public static Guid BooleanAttribute { get; } = new Guid("{EA830448-A403-4ED9-A3D3-048D5D5C3A03}");
			public static Guid NumericAttribute { get; } = new Guid("{9DE86AF1-EFD6-4B71-9DCC-202F247C94CB}");
			public static Guid DateAttribute { get; } = new Guid("{0B7B1812-DFB4-4F25-BF6D-CEB0E1DF8744}");

			public static Guid Element { get; } = new Guid("{91929595-3dbd-4eae-8add-6120a49797c7}");
			public static Guid ElementContent { get; } = new Guid("{f85d4f5e-c69f-4498-9913-7a8554e233a4}");

			public static Guid Translation { get; } = new Guid("{04A53CC8-3206-4A97-99C5-464DB8CAA6E6}");
			public static Guid TranslationValue { get; } = new Guid("{6D38E757-EC18-43AD-9C35-D15BB446C0E1}");

			public static Guid String { get; } = new Guid("{5AECB489-DBC2-432E-86AF-6BE349317238}");
			public static Guid StringComponent { get; } = new Guid("{F9E2B671-13F5-4172-A568-725ACD8BBFAB}");
			public static Guid ExtractSingleInstanceStringComponent { get; } = new Guid("{FCECCE4E-8D05-485A-AE34-B1B45E766661}");
			public static Guid InstanceAttributeStringComponent { get; } = new Guid("{623565D5-5AEE-49ED-A5A9-0CFE670507BC}");

			public static Guid Prompt { get; } = new Guid("{EC889225-416A-4F73-B8D1-2A42B37AF43E}");
			public static Guid TextPrompt { get; } = new Guid("{195DDDD7-0B74-4498-BF61-B0549FE05CF3}");
			public static Guid ChoicePrompt { get; } = new Guid("{5597AEEB-922D-48AF-AE67-DF7D951C71DB}");
			public static Guid InstancePrompt { get; } = new Guid("{F3ECBF1E-E732-4370-BE05-8FA7CC520F50}");
			public static Guid BooleanPrompt { get; } = new Guid("{a7b49c03-c9ce-4a79-a4b2-e94fc8cd8b29}");

			public static Guid Method { get; } = new Guid("{D2813913-80B6-4DD6-9AD6-56D989169734}");

			// public static Guid MethodCall { get; } = new Guid("{084A6D58-32C9-4A5F-9D2B-86C46F74E522}");

			public static Guid ConditionalEvaluationCase { get; } = new Guid("{ba18abdc-11ae-46af-850a-eb30280b0ffa}");
			public static Guid ConditionalSelectAttributeCase { get; } = new Guid("{a1115690-c400-4e3e-9c8f-24e2a9477e8f}");

			public static Guid AccessModifier { get; } = new Guid("{ca4fcc11-16c8-4872-a712-82e589d382ce}");

			public static Guid MethodBinding { get; } = new Guid("{CB36098E-B9BF-4D22-87FA-4186EC632C89}");
			public static Guid ReturnAttributeMethodBinding { get; } = new Guid("{30FB6BA6-2BBB-41D2-B91A-709C00A07790}");
			public static Guid ReturnInstanceSetMethodBinding { get; } = new Guid("{AADC20F9-7559-429B-AEF0-97E059295C76}");

			public static Guid Executable { get; } = new Guid("{6A1F66F7-8EA6-43D1-B2AF-198F63B84710}");
			public static Guid ExecutableReturningAttribute { get; } = new Guid("{50b2db7a-3623-4be4-b40d-98fab89d3ff5}");
			public static Guid ExecutableReturningInstanceSet { get; } = new Guid("{d5fbc5cb-13fb-4e68-b3ad-46b4ab8909f7}");
			public static Guid ExecutableReturningElement { get; } = new Guid("{a15a4f52-1f1a-4ef3-80a7-033d45cc0548}");
			public static Guid ExecutableReturningWorkData { get; } = new Guid("{a0365b76-ad1f-462e-84da-d6a1d5b9c88c}");

			public static Guid Event { get; set; } = new Guid("{ca727ecd-8536-4aeb-9e75-352dbb958767}");

			public static Guid WorkData { get; set; } = new Guid("{05e8f023-88cb-416b-913e-75299e665eb2}");
			public static Guid WorkSet { get; set; } = new Guid("{c4c171d8-994b-485b-b0ac-053d11b963ab}");

			public static Guid Parameter { get; } = new Guid("{1ab99be0-43f1-495a-b85e-ec38ea606713}");
			public static Guid ParameterAssignment { get; } = new Guid("{c7aa0c7c-50d8-44fd-9b05-a558a38a6954}"); // 28
			public static Guid ElementContentDisplayOption { get; } = new Guid("{bd68052a-daa4-43b9-8965-d38095473170}");

			public static Guid ReportObject { get; } = new Guid("{ff7d5757-d9d9-48ab-ab04-5932e7341a90}");

			public static Guid Validation { get; } = new Guid("{3E45AA17-6E8E-41DB-9C94-E84B4B4176E8}");
			public static Guid ValidationClassification { get; } = new Guid("{8f43d578-a671-436b-afdc-c8689a5bd9b6}");

			public static Guid PrimaryObjectReportField { get; } = new Guid("{59EA0C72-4800-48BA-84A4-DDFE10E5F4D0}");
			public static Guid RelationshipReportField { get; } = new Guid("{FC4E3BB5-1EA7-44FF-B828-2EA54CDD4ABB}");

			public static Guid ReportField { get; } = new Guid("{655A04D9-FE35-4F89-9AAB-B8FA34989D03}");
			public static Guid AttributeReportField { get; } = new Guid("{C06E0461-A956-4599-9708-012C8FE04D94}");

			public static Guid Module { get; } = new Guid("{e009631d-6b9d-445c-95df-79f4ef8c8fff}");

			public static Guid Report { get; } = new Guid("{19D947B6-CE82-4EEE-92EC-A4E01E27F2DB}");
			public static Guid ReportColumn { get; } = new Guid("{BEFE99A1-B2EB-4365-A2C9-061C6609037B}");
			public static Guid StandardReport { get; } = new Guid("{FDF4A498-DE83-417D-BA01-707372125C8D}");

			public static Guid TaskCategory { get; } = new Guid("{e8d8060f-a20c-442f-8384-03488b63247f}");
			public static Guid Task { get; } = new Guid("{D4F2564B-2D11-4A5C-8AA9-AF52D4EACC13}");
			public static Guid UITask { get; } = new Guid("{BFD07772-178C-4885-A6CE-C85076C8461C}");

			public static Guid Tenant { get; } = new Guid("{703F9D65-C584-4D9F-A656-D0E3C247FF1F}");

			public static Guid User { get; } = new Guid("{9C6871C1-9A7F-4A3A-900E-69D1D9E24486}");
			public static Guid UserLogin { get; } = new Guid("{64F4BCDB-38D0-4373-BA30-8AE99AF1A5F7}");

			public static Guid MenuItemCommand { get; } = new Guid("{9D3EDE23-6DB9-4664-9145-ABCBD3A0A2C2}");
			public static Guid MenuItemSeparator { get; } = new Guid("{798DECAB-5119-49D7-B0AD-D4BF45807188}");
			public static Guid MenuItemHeader { get; } = new Guid("{1F148873-8A97-4409-A79B-C19D5D380CA4}");
			public static Guid MenuItemInstance { get; } = new Guid("{6E3AA9AF-96B9-4208-BEA9-291A72C68418}");

			public static Guid Style { get; } = new Guid("{A48C843A-B24B-4BC3-BE6F-E2D069229B0A}");
			public static Guid StyleRule { get; } = new Guid("{C269A1F3-E014-4230-B78D-38EAF6EA8A81}");
			public static Guid StyleClass { get; } = new Guid("{a725f089-7763-4887-af37-da52358c378c}");

			public static Guid Page { get; } = new Guid("{D9626359-48E3-4840-A089-CD8DA6731690}");
			public static Guid ContainerPageComponent { get; } = new Guid("{6AD6BD1C-7D1C-4AC9-9642-FEBC61E9D6FF}");
			public static Guid ButtonPageComponent { get; } = new Guid("{F480787D-F51E-498A-8972-72128D808AEB}");
			public static Guid HeadingPageComponent { get; } = new Guid("{FD86551E-E4CE-4B8B-95CB-BEC1E6A0EE2B}");
			public static Guid ImagePageComponent { get; } = new Guid("{798B67FA-D4BE-42B9-B4BD-6F8E02C953C0}");
			public static Guid PanelPageComponent { get; } = new Guid("{D349C489-9684-4A5A-9843-B906A7F803BC}");
			public static Guid ParagraphPageComponent { get; } = new Guid("{ADFF93CE-9E85-4168-A7D4-5239B99BE36D}");
			public static Guid SequentialContainerPageComponent { get; } = new Guid("{A66D9AE2-3BEC-4083-A5CB-7DE3B03A9CC7}");
			public static Guid SummaryPageComponent { get; } = new Guid("{5EBA7BD6-BA0A-45B2-835C-C92489FD7E74}");
			public static Guid TabContainerPageComponent { get; } = new Guid("{E52B02D7-895C-4642-9B03-EB0232868190}");
			public static Guid DetailPageComponent { get; } = new Guid("{41F9508E-6CF0-4F3D-8762-FF17CD52C466}");
			public static Guid ElementPageComponent { get; } = new Guid("{122d5565-9df9-4656-b6af-ba5df6630d32}");

			public static Guid IntegrationID { get; } = new Guid("{49a5ebda-baaa-4ede-bba5-decc250ce1a3}");
			public static Guid IntegrationIDUsage { get; } = new Guid("{86084b9f-3860-4857-a41f-2f8b6877aff1}");

			public static Guid Dashboard { get; } = new Guid("{896B529C-452D-42AC-98C5-170ED4F826C6}");
			public static Guid DashboardContent { get; } = new Guid("{2720ea77-cb09-4099-9f74-ccdacdd146e8}");
			public static Guid Instance { get; } = new Guid("{263C4882-945F-4DE9-AED8-E0D6516D4903}");
			public static Guid InstanceSet { get; } = new Guid("{53aac86e-ce60-4509-a869-417c38c305e0}");

			public static Guid AuditLine { get; } = new Guid("{a4124a76-02f6-406a-9583-a197675b493b}");

			public static Guid Theme { get; } = new Guid("{7c2cc4b5-8323-4478-863b-1759d7adf62e}");

			public static Guid CommonBoolean { get; } = new Guid("{5b025da3-b7bd-45a9-b084-48c4a922bf72}");
			public static Guid CommonInstanceSet { get; } = new Guid("{3382da21-4fc5-45dc-bbd1-f7ba3ece1a1b}");
		}
		public static class Methods
		{
			public static class ControlTransaction
			{
				public static Guid CreateInstance { get; } = new Guid("{4E92D64E-AC91-4ABF-8052-96366DF93996}");
			}
			public static class GetInstanceSetBySystemRoutine
			{
				public static Guid Tenant__get__Current_Tenant { get; } = new Guid("{22ec44d5-bede-48bb-96ca-03481611422c}");
				public static Guid Instance__get__This_Instance { get; } = new Guid("{d78fe3ca-3c15-41a0-bccf-0a701b4712a4}");
			}
		}

		[ExportEntities(Prefix = "IDC_", Suffix = null)]
		public static class MethodClasses
		{
			public static Guid ControlTransactionMethod { get; } = new Guid("{BF73CEAC-F972-46E8-BFA1-7807F39F1B91}"); // 8
			// PU - Process Updates Method - 11
			public static Guid GetAttributeMethod { get; } = new Guid("{c3ecf8c9-597f-417b-ad65-fae0401719c6}"); // 12
			// Perform System Routine Method - 13
			public static Guid GetReferencedAttributeMethod { get; } = new Guid("{9205c54e-921a-484c-9be2-3d3deb877474}"); // 18
			public static Guid SelectFromInstanceSetMethod { get; } = new Guid("{130637B4-17A7-4394-8F4D-E83A79114E6C}"); // 19
			public static Guid EvaluateBooleanExpressionMethod { get; } = new Guid("{e1529108-2f84-4a4c-89b3-a647bc3e41d7}"); // 24
			// AR - Assign Relationship Method - 25
			public static Guid GetReferencedInstanceSetMethod { get; } = new Guid("{bcfd0d64-3eba-4a97-9622-f3a960877d24}"); // 26
			// BEM - Build Element Method - 29
			public static Guid BuildAttributeMethod { get; } = new Guid("{e5879955-0093-48c8-8042-813168578af2}"); // 30
			// CN - Calculate Numeric Method - 35
			/// <summary>
			/// GSI - Get Specified Instances Method
			/// Equivalent to YP: `static function $verb$name() : $returnType = $inst`
			/// </summary>
			/// <value>The get specified instances method.</value>
			public static Guid GetSpecifiedInstancesMethod { get; } = new Guid("{7794c7d0-b15d-4b23-aa40-63bb3d1f53ac}"); // 40

			// GSP - Get Instance Set from Parameters Method - 51
			// SA - Select Attribute Method - 52
			// PRU - Process Related Updates Method - 54
			// BUIR - Build UI Response Method - 62
			// GEP - Get Element from Parameters Method - 63
			// GAP - Get Attribute from Parameters Method - 64
			public static Guid GetInstanceSetBySystemRoutineMethod { get; } = new Guid("{7bdc53a1-81f0-458d-96f4-5637e6e1cf49}"); // 65
			// GAS - Get Attribute by System Routine Method - 66
			// GES - Get Element by System Routine Method - 67
			// US - Update by System Routine Method - 68
			// CS - Compare Instance Sets Method - 91
			public static Guid CalculateDateMethod { get; } = new Guid("{1e58e284-56eb-42b4-88b9-e0a691559fa6}"); // 92
			// EC - Evaluate Conditions Method - 94
			// SIM - Spawn Internal Message Method - 169
			// BWSR - Build Web Service Response Method - 171
			// GRE - Get Referenced Element Method - 197
			public static Guid GetRelationshipMethod { get; } = new Guid("{d53c9232-89ef-4cca-8520-261da6787450}"); // 207
			// IWS - Invoke Web Service Method - 208
			public static Guid InstanceOpMethod { get; } = new Guid("{4c814982-938f-4116-bdc1-827bae6a5f71}");
			public static Guid ConditionalSelectAttributeMethod { get; } = new Guid("{d534a369-321e-4c32-bd7f-8ff2017f191e}"); // 13038
			// SSC - Conditional Select from Instance Set Method - 13039
		}
		public static class PromptValueClasses
		{
			public static Guid InstancePromptValue { get; } = new Guid("{0A6E2CF2-97B6-4339-B1DA-1DBBFE9806C6}");

			/// <summary>
			/// Return Instance Set Method Binding: `Instance@get This Instance (GSS)*P*S [rsmb]`
			/// </summary>
			public static Guid This_Instance { get; } = new Guid("{a476372a-24e7-41d1-9a21-ba241b1a5067}");
			/// <summary>
			/// Return Instance Set Method Binding: `Instance@get Related Instance (GSS)*P*S [rsmb]`
			/// </summary>
			public static Guid Related_Instance { get; } = new Guid("{01e09f71-0bf6-4e33-a9f5-d45d5fb375c7}");

			public static Guid Report_Field_Instance { get; } = new Guid("{236d54d3-3f69-477d-bafa-ae84ea0e7d6d}");

			/// <summary>
			/// Parameter: Class [CL]
			/// </summary>
			public static Guid Class_Instance_for_SS_Method { get; } = new Guid("{53675af2-67e1-425f-b121-6f1d4459179a}");
		}
		public static class Users
		{
			public static Guid XQEnvironments { get; } = new Guid("{B066A54B-B160-4510-A805-436D3F90C2E6}");
		}

		[ExportEntities(Prefix = "IDI_ObjectSource_", Suffix = null)]
		public static class ObjectSources
		{
			public static Guid System { get; } = new Guid("{9547EB35-07FB-4B64-B82C-6DA1989B9165}");
			public static Guid Delivered { get; } = new Guid("{062B57B5-D728-4DF8-903A-AD79D843B5CA}");
			public static Guid Custom { get; } = new Guid("{63D91CCD-D196-4C3A-8609-6DF371E2406F}");
		}
		public static class ReportColumnOptions
		{
			public static Guid DisplayAsCount { get; } = new Guid("{5C9B4C79-995B-4E6A-81C0-39C174BF9F6D}");
		}

		[ExportEntities(Prefix = "IDI_Page_", Suffix = null)]
		public static class Pages
		{
			public static Guid LoginPage { get; } = new Guid("{9E272BC3-0358-4EB7-8B3B-581964A59634}");
		}

		[ExportEntities(Prefix = "IDI_Style_", Suffix = null)]
		public static class Styles
		{
			public static Guid BlockElementHorizontallyCentered { get; } = new Guid("{2ccfdd1b-74ab-4a35-885b-667d21465704}");
		}

		[ExportEntities(Prefix = "IDI_StyleProperty_", Suffix = null)]
		public static class StyleProperties
		{
			public static Guid BackgroundAttachment { get; } = new Guid("{0b4bcb85-d3c1-4c96-a203-23b29dec0b0c}");
			public static Guid Display { get; } = new Guid("{10a121b6-6d91-4a99-9c51-42168fd14065}");

			public static Guid Padding { get; } = new Guid("{354D0A44-6816-4E65-BC84-4910CE0CE6F5}");
			public static Guid PaddingLeft { get; } = new Guid("{dffd0a45-ce3c-44da-bfdb-9b77df6e16a7}");
			public static Guid PaddingRight { get; } = new Guid("{b63f804f-e7e8-4401-a893-54448452ed89}");
			public static Guid PaddingTop { get; } = new Guid("{1ab15d91-d124-4d49-8c53-088dfead5866}");
			public static Guid PaddingBottom { get; } = new Guid("{67db661d-10e8-4475-8cb1-983e65893766}");

			public static Guid Margin { get; } = new Guid("{6d503800-6454-43c4-9940-82bea7e62a8f}");
			public static Guid MarginLeft { get; } = new Guid("{fd8f58fc-8234-4b33-a7ab-2d60aedad5a5}");
			public static Guid MarginRight { get; } = new Guid("{207bb86c-f604-4700-8174-c771c59e864f}");
			public static Guid MarginTop { get; } = new Guid("{c250f1de-afec-4422-b29d-59ec9f533fdb}");
			public static Guid MarginBottom { get; } = new Guid("{059d7330-18ed-4716-a721-9c5a28c8ee84}");

			public static Guid Width { get; } = new Guid("{7a84cf37-c445-4e75-ab1a-075bb99067f2}");
		}

		[ExportEntities(Prefix = "IDI_Orientation_", Suffix = null)]
		public static class Orientation
		{
			public static Guid Horizontal { get; } = new Guid("{9B7B7F14-0925-456D-98E6-E3FFEFDC272C}");
			public static Guid Vertical { get; } = new Guid("{96423CAA-260E-453A-B609-E8090897EDDF}");
		}

		[ExportEntities(Prefix = "IDI_SecurityDomain_", Suffix = null)]
		public static class SecurityDomains
		{
			public static Guid Anyone { get; } = new Guid("{7c420ec0-988b-4987-89f6-004df29d7e6b}");
			public static Guid AuthenticatedUsers { get; } = new Guid("{f9d4f21d-2b2b-4002-b99d-7b30d4e67c6d}");
		}

		[ExportEntities(Prefix = "IDI_Task_", Suffix = null)]
		public static class Tasks
		{
			public static Guid OpenDefinitionInCodeEditor { get; } = new Guid("{4f8a0e8e-e139-4cc6-b8cf-a32e67bd192d}");
		}

		[ExportEntities(Prefix = "IDI_DisplayOption_", Suffix = null)]
		public static class ElementContentDisplayOptions
		{
			public static Guid DisplayAsPageTitle { get; } = new Guid("{369335cf-4a3d-4349-9300-62bd72dafeb6}");
			public static Guid NotEnterable { get; } = new Guid("{831e921b-5132-4b37-9303-da0994a0f828}");
			public static Guid SubmitNotEnterable { get; } = new Guid("{f480a1d6-cc9b-4606-bce1-d6757fa99bad}");
			public static Guid ShowSubelementsVertically { get; } = new Guid("{8bfd1ae2-1474-413d-b4dd-6cd4993080d1}");
			public static Guid Singular { get; } = new Guid("{683b3cdb-2e69-468e-bb36-4bd62716542c}");
			public static Guid DoNotShow { get; } = new Guid("{cc278872-4454-49fe-821a-ae657bffe062}");
			public static Guid InitializeForEntry { get; } = new Guid("{59410ca4-8214-4b59-ba49-1565d0cf16c7}");
			public static Guid DoNotShowLabel { get; } = new Guid("{a9fe1e02-0785-4363-b4dd-8b7ca989d4fd}");
			public static Guid WideText { get; } = new Guid("{dc0570c5-cfd5-4f87-9ba7-51fab4ccf551}");
			public static Guid Required { get; } = new Guid("{7e508c54-0507-4ae7-8ea2-7c9f197051c7}");
			public static Guid DoNotShowIfEmpty { get; } = new Guid("{055b9a42-caf5-42ad-bd2e-9a4c3527947d}");
			/// <summary>
			/// Obscured Text - displays text in a text box element as obscured (system-dependent, e.g. asterisk (*) or dot)
			/// </summary>
			/// <value>The GUID of the `Obscured Text` EC Display Option.</value>
			public static Guid ObscuredText { get; } = new Guid("{e42fb627-6559-42e7-a8fe-59c9d674eec4}");
		}
	}
}
