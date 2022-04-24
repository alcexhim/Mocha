using System;
namespace Mocha.Core
{
    public static class KnownInstanceGuids
    {
		public static class Classes
		{
			public static Guid Class { get; } = new Guid("{B9C9B9B7-AD8A-4CBD-AA6B-E05784630B6B}");
			public static Guid Attribute { get; } = new Guid("{F9CD7751-EF62-4F7C-8A28-EBE90B8F46AA}");
			public static Guid Relationship { get; } = new Guid("{9B0A80F9-C325-4D36-997C-FB4106204648}");

			// Attribute subclasses
			public static Guid TextAttribute { get; } = new Guid("{35BF9AC5-0212-4DBA-BB9B-D1A6E9955827}");
			public static Guid BooleanAttribute { get; } = new Guid("{35BF9AC5-0212-4DBA-BB9B-D1A6E9955827}");
			public static Guid NumericAttribute { get; } = new Guid("{35BF9AC5-0212-4DBA-BB9B-D1A6E9955827}");
			public static Guid DateAttribute { get; } = new Guid("{35BF9AC5-0212-4DBA-BB9B-D1A6E9955827}");

			public static Guid Language { get; } = new Guid("{61102B47-9B2F-4CF3-9840-D168B84CF1E5}");
			public static Guid Translation { get; } = new Guid("{04A53CC8-3206-4A97-99C5-464DB8CAA6E6}");
			public static Guid TranslatableTextConstantValue { get; } = new Guid("{6D38E757-EC18-43AD-9C35-D15BB446C0E1}");

			public static Guid ExtractSingleInstanceStringComponent { get; } = new Guid("{FCECCE4E-8D05-485A-AE34-B1B45E766661}");
			public static Guid InstanceAttributeStringComponent { get; } = new Guid("{623565D5-5AEE-49ED-A5A9-0CFE670507BC}");
			public static Guid TextConstantStringComponent { get; } = new Guid("{35BF9AC5-0212-4DBA-BB9B-D1A6E9955827}");

			public static Guid TextPrompt { get; } = new Guid("{195DDDD7-0B74-4498-BF61-B0549FE05CF3}");
			public static Guid ChoicePrompt { get; } = new Guid("{5597AEEB-922D-48AF-AE67-DF7D951C71DB}");
			public static Guid InstancePrompt { get; } = new Guid("{F3ECBF1E-E732-4370-BE05-8FA7CC520F50}");
			public static Guid DatePrompt { get; } = new Guid("{44b31ee1-d242-47ea-bf62-83a20b1c9d62}");
			public static Guid BooleanPrompt { get; } = new Guid("{a7b49c03-c9ce-4a79-a4b2-e94fc8cd8b29}");

			public static Guid MethodCall { get; } = new Guid("{084A6D58-32C9-4A5F-9D2B-86C46F74E522}");

			public static Guid ReturnInstanceSetMethodBinding { get; } = new Guid("{AADC20F9-7559-429B-AEF0-97E059295C76}");

			public static Guid ReportObject { get; } = new Guid("{ff7d5757-d9d9-48ab-ab04-5932e7341a90}");

			public static Guid PrimaryObjectReportField { get; } = new Guid("{59EA0C72-4800-48BA-84A4-DDFE10E5F4D0}");
			public static Guid RelationshipReportField { get; } = new Guid("{FC4E3BB5-1EA7-44FF-B828-2EA54CDD4ABB}");
			public static Guid AttributeReportField { get; } = new Guid("{C06E0461-A956-4599-9708-012C8FE04D94}");

			public static Guid Report { get; } = new Guid("{19D947B6-CE82-4EEE-92EC-A4E01E27F2DB}");
			public static Guid StandardReport { get; } = new Guid("{FDF4A498-DE83-417D-BA01-707372125C8D}");

			public static Guid Task { get; } = new Guid("{D4F2564B-2D11-4A5C-8AA9-AF52D4EACC13}");
			public static Guid UITask { get; } = new Guid("{BFD07772-178C-4885-A6CE-C85076C8461C}");

			public static Guid Tenant { get; } = new Guid("{703F9D65-C584-4D9F-A656-D0E3C247FF1F}");

			public static Guid User { get; } = new Guid("{9C6871C1-9A7F-4A3A-900E-69D1D9E24486}");

			public static Guid MenuItemCommand { get; } = new Guid("{9D3EDE23-6DB9-4664-9145-ABCBD3A0A2C2}");
			public static Guid MenuItemSeparator { get; } = new Guid("{798DECAB-5119-49D7-B0AD-D4BF45807188}");
			public static Guid MenuItemHeader { get; } = new Guid("{1F148873-8A97-4409-A79B-C19D5D380CA4}");
			public static Guid MenuItemInstance { get; } = new Guid("{6E3AA9AF-96B9-4208-BEA9-291A72C68418}");

			public static Guid ButtonPageComponent { get; } = new Guid("{F480787D-F51E-498A-8972-72128D808AEB}");
			public static Guid HeadingPageComponent { get; } = new Guid("{FD86551E-E4CE-4B8B-95CB-BEC1E6A0EE2B}");
			public static Guid ImagePageComponent { get; } = new Guid("{798B67FA-D4BE-42B9-B4BD-6F8E02C953C0}");
			public static Guid PanelPageComponent { get; } = new Guid("{D349C489-9684-4A5A-9843-B906A7F803BC}");
			public static Guid ParagraphPageComponent { get; } = new Guid("{ADFF93CE-9E85-4168-A7D4-5239B99BE36D}");
			public static Guid SequentialContainerPageComponent { get; } = new Guid("{A66D9AE2-3BEC-4083-A5CB-7DE3B03A9CC7}");
			public static Guid SummaryPageComponent { get; } = new Guid("{5EBA7BD6-BA0A-45B2-835C-C92489FD7E74}");
		}
		public static class Languages
		{
			public static Guid English { get; } = new Guid("{68BB6038-A4B5-4EE1-AAE9-326494942062}");
		}
		public static class Methods
		{
			public static class ControlTransaction
			{
				public static Guid CreateInstance { get; } = new Guid("{4E92D64E-AC91-4ABF-8052-96366DF93996}");
			}
		}
		public static class MethodClasses
		{
			public static Guid ControlTransactionMethod { get; } = new Guid("{BF73CEAC-F972-46E8-BFA1-7807F39F1B91}");
			public static Guid SelectInstanceSetMethod { get; } = new Guid("{130637B4-17A7-4394-8F4D-E83A79114E6C}");
		}
		public static class PromptValueClasses
		{
			public static Guid InstancePromptValue { get; } = new Guid("{0A6E2CF2-97B6-4339-B1DA-1DBBFE9806C6}");
		}
		public static class Users
		{
			public static Guid XQEnvironments { get; } = new Guid("{B066A54B-B160-4510-A805-436D3F90C2E6}");
		}
		public static class ObjectSources
		{
			public static Guid System { get; } = new Guid("{9547EB35-07FB-4B64-B82C-6DA1989B9165}");
			public static Guid Delivered { get; } = new Guid("{062B57B5-D728-4DF8-903A-AD79D843B5CA}");
			public static Guid Custom { get; } = new Guid("{63D91CCD-D196-4C3A-8609-6DF371E2406F}");
		}
		public static class Pages
		{
			public static Guid LoginPage { get; } = new Guid("{9E272BC3-0358-4EB7-8B3B-581964A59634}");
		}
	}
}
