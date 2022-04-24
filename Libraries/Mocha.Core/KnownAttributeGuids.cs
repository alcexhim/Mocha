using System;
namespace Mocha.Core
{
    public static class KnownAttributeGuids
    {
        public static class Text
        {
            public static Guid Name { get; } = new Guid("{9153A637-992E-4712-ADF2-B03F0D9EDEA6}");
			public static Guid Value { get; } = new Guid("{041DD7FD-2D9C-412B-8B9D-D7125C166FE0}");

			public static Guid TargetURL { get; } = new Guid("{970F79A0-9EFE-4E7D-9286-9908C6F06A67}");

			public static Guid UserName { get; } = new Guid("{960FAF02-5C59-40F7-91A7-20012A99D9ED}");
			public static Guid PasswordHash { get; } = new Guid("{F377FC29-4DF1-4AFB-9643-4191F37A00A9}");
			public static Guid PasswordSalt { get; } = new Guid("{8C5A99BC-40ED-4FA2-B23F-F373C1F3F4BE}");

			public static Guid ContentType { get; } = new Guid("{34142FCB-172C-490A-AF03-FF8451D00CAF}");

			public static Guid BackgroundColor { get; } = new Guid("{B817BE3B-D0AC-4A60-A98A-97F99E96CC89}");
			public static Guid ForegroundColor { get; } = new Guid("{BB4B6E0D-D9BA-403D-9E81-93E8F7FB31C8}");
		}
		public static class Boolean
		{
			public static Guid DisplayVersionInBadge { get; } = new Guid("{BE5966A4-C4CA-49A6-B504-B6E8759F392D}");
			public static Guid Editable { get; } = new Guid("{957fd8b3-fdc4-4f35-87d6-db1c0682f53c}");
		}
		public static class Numeric
		{
			public static Guid Index { get; } = new Guid("{0f31b9ca-e3e2-4c62-8c9e-b55f16eafbf9}");
			public static Guid Level { get; } = new Guid("{8C528FB0-4063-47B0-BC56-85E387A41BD2}");
		}
	}
}
