﻿//
//  MochaTenantDefinitionDataFormat.cs
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
using System.Collections.Generic;
using System.Linq;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Markup;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaTenantDefinition;

namespace UniversalEditor.Plugins.Mocha.DataFormats.MochaTenantDefinition
{
	public class MochaTenantDefinitionDataFormat : XMLDataFormat
	{
		protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeLoadInternal(objectModels);
			objectModels.Push(new MarkupObjectModel());
		}
		protected override void AfterLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.AfterLoadInternal(objectModels);

			MarkupObjectModel mom = objectModels.Pop() as MarkupObjectModel;
			MochaTenantDefinitionObjectModel tenants = objectModels.Pop() as MochaTenantDefinitionObjectModel;

			MarkupTagElement tagTenants = mom.FindElementUsingSchema(XMLSchemas.Mocha, "tenants") as MarkupTagElement;
			if (tagTenants == null) throw new InvalidDataFormatException();

			foreach (MarkupTagElement tagTenant in tagTenants.Elements.OfType<MarkupTagElement>())
			{
				if (!(tagTenant.Name == "tenant" && tagTenant.XMLSchema == XMLSchemas.Mocha))
				{
					continue;
				}

				Tenant tenant = new Tenant();
				tenant.Name = tagTenant.Attributes["name"]?.Value;

				MarkupTagElement tagLibraryReferences = tagTenant.FindElementUsingSchema(XMLSchemas.Mocha, "libraryReferences") as MarkupTagElement;
				if (tagLibraryReferences != null)
				{
					foreach (MarkupTagElement tagLibraryReference in tagLibraryReferences.Elements.OfType<MarkupTagElement>())
					{
						if (tagLibraryReference.Name == "libraryReference" && tagLibraryReference.XMLSchema == XMLSchemas.Mocha)
						{
							LibraryReference lref = new LibraryReference();
							lref.Path = tagLibraryReference.Attributes["path"]?.Value;
							tenant.LibraryReferences.Add(lref);
						}
					}
				}
				tenants.Tenants.Add(tenant);
			}
		}
		protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeSaveInternal(objectModels);
		}
	}
}
