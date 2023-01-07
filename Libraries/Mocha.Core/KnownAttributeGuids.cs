using System;
namespace Mocha.Core
{
    public static class KnownAttributeGuids
    {
		public static class Text
		{
			public static Guid Name { get; } = new Guid("{9153A637-992E-4712-ADF2-B03F0D9EDEA6}");
			public static Guid Verb { get; } = new Guid("{61345a5d-3397-4a96-8797-8863f03a476c}");
			public static Guid Value { get; } = new Guid("{041DD7FD-2D9C-412B-8B9D-D7125C166FE0}");
			public static Guid CSSValue { get; } = new Guid("{C0DD4A42-F503-4EB3-8034-7C428B1B8803}");
			public static Guid RelationshipType { get; } = new Guid("{71106B12-1934-4834-B0F6-D894637BAEED}");

			public static Guid TargetURL { get; } = new Guid("{970F79A0-9EFE-4E7D-9286-9908C6F06A67}");

			public static Guid UserName { get; } = new Guid("{960FAF02-5C59-40F7-91A7-20012A99D9ED}");
			public static Guid PasswordHash { get; } = new Guid("{F377FC29-4DF1-4AFB-9643-4191F37A00A9}");
			public static Guid PasswordSalt { get; } = new Guid("{8C5A99BC-40ED-4FA2-B23F-F373C1F3F4BE}");

			public static Guid ContentType { get; } = new Guid("{34142FCB-172C-490A-AF03-FF8451D00CAF}");

			public static Guid BackgroundColor { get; } = new Guid("{B817BE3B-D0AC-4A60-A98A-97F99E96CC89}");
			public static Guid ForegroundColor { get; } = new Guid("{BB4B6E0D-D9BA-403D-9E81-93E8F7FB31C8}");
			public static Guid IPAddress { get; } = new Guid("{ADE5A3C3-A84E-4798-BC5B-E08F21380208}");
			public static Guid Token { get; } = new Guid("{da7686b6-3803-4f15-97f6-7f8f3ae16668}");

			public static Guid DebugDefinitionFileName { get; } = new Guid("{03bf47c7-dc97-43c8-a8c9-c6147bee4e1f}");
		}
		public static class Boolean
		{
			public static Guid DisplayVersionInBadge { get; } = new Guid("{BE5966A4-C4CA-49A6-B504-B6E8759F392D}");
			public static Guid Editable { get; } = new Guid("{957fd8b3-fdc4-4f35-87d6-db1c0682f53c}");
			public static Guid Static { get; } = new Guid("{9A3A0719-64C2-484F-A55E-22CD4597D9FD}");
			public static Guid Singular { get; } = new Guid("{F1A06573-C447-4D85-B4E7-54A438C4A960}");
			public static Guid Required { get; } = new Guid("{4061c1c4-7ec3-439b-b72d-59c7df344a76}");
			public static Guid Null { get; } = new Guid("{745c6c38-594e-4528-82c9-e25b023705e4}");
			public static Guid Visible { get; } = new Guid("{ff73c8f6-f706-4944-b562-43a0acb7eade}");
			public static Guid RenderAsText { get; } = new Guid("{15dd9e49-ab6d-44f0-9039-27af81736406}");
			public static Guid UseAnyCondition { get; } = new Guid("{31a8a2c2-1f55-4dfe-b177-427a2219ef8c}");
			public static Guid ValidateOnlyOnSubmit { get; } = new Guid("{400fcd8e-823b-4f4a-aa38-b444f763259b}");
		}
		public static class Numeric
		{
			public static Guid Index { get; } = new Guid("{0f31b9ca-e3e2-4c62-8c9e-b55f16eafbf9}");
			public static Guid Level { get; } = new Guid("{8C528FB0-4063-47B0-BC56-85E387A41BD2}");
			public static Guid DebugDefinitionLineNumber { get; } = new Guid("{822be9b7-531d-4aa1-818a-6e4de1609057}");
			public static Guid DebugDefinitionColumnNumber { get; } = new Guid("{0f75c750-e738-4410-9b4e-deb422efc7aa}");
		}
	}
}
