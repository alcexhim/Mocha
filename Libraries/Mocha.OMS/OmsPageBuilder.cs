//
//  PageBuilder.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2021 Mike Becker's Software
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
using Mocha.Core;
using Mocha.OMS;
using Mocha.OMS.OMSComponents;

namespace Mocha.OMS
{
	internal static class OmsPageBuilder
	{
		public static OMSComponent CreatePageComponent(Oms oms, Instance inst, OmsContext context)
		{
			context.GlobalVariables.Add(new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst));

			Instance parentClassInstance = oms.GetParentClass(inst);
			if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.SequentialContainerPageComponent)
			{
				return CreateSequentialContainerPageComponent(oms, inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ImagePageComponent)
			{
				return CreateImagePageComponent(oms, inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.HeadingPageComponent)
			{
				return CreateHeadingPageComponent(oms, inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ParagraphPageComponent)
			{
				string text = oms.ExecuteMethodReturningTextOrTranslation(oms.GetRelatedInstance(inst, KnownRelationshipGuids.Content_Page_Component__gets_content_from__Method), context);
				return new OMSParagraphComponent()
				{
					Text = text
				};
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
			{
				OMSPanelComponent panel = new OMSPanelComponent();
				panel.InstanceID = oms.GetInstanceKey(inst);

				Instance[] instHeaderComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_header__Page_Component);
				Instance[] instContentComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
				Instance[] instFooterComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
				for (int i = 0; i < instHeaderComponents.Length; i++)
				{
					OMSComponent c1 = CreatePageComponent(oms, instHeaderComponents[i], context);
					if (c1 == null)
						continue;

					panel.HeaderComponents.Add(c1);
				}
				for (int i = 0; i < instContentComponents.Length; i++)
				{
					OMSComponent c1 = CreatePageComponent(oms, instContentComponents[i], context);
					if (c1 == null)
						continue;

					panel.ContentComponents.Add(c1);
				}
				for (int i = 0; i < instFooterComponents.Length; i++)
				{
					OMSComponent c1 = CreatePageComponent(oms, instFooterComponents[i], context);
					if (c1 == null)
						continue;

					panel.FooterComponents.Add(c1);
				}
				return panel;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ButtonPageComponent)
			{
				OMSCommandComponent item = new OMSCommandComponent();
				item.InstanceID = oms.GetInstanceKey(inst);

				// item.Text = OMS.GetTranslationValue(inst, KnownRelationshipGuids.Button_Page_Component__has_text__Translatable_Text_Constant);
				return item;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.DetailPageComponent)
			{
				return CreateDetailPageComponent(oms, inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.SummaryPageComponent)
			{
				OMSSummaryComponent item = new OMSSummaryComponent();
				item.InstanceID = oms.GetInstanceKey(inst);

				return item;
			}
			throw new NotSupportedException();
		}

		private static OMSDetailComponent CreateDetailPageComponent(Oms oms, Instance inst, OmsContext context)
		{
			OMSDetailComponent item = new OMSDetailComponent();
			item.InstanceID = oms.GetInstanceKey(inst);

			Instance instColumnSourceRSMB = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Detail_Page_Component__has_column_source__Method_Binding);
			// column source can return an Instance Set of columns, or a method returning an instance set of columns
			object columnSourceValue = oms.ExecuteMethod(instColumnSourceRSMB, context);

			Instance instRowSourceRSMB = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Detail_Page_Component__has_row_source__Method_Binding);

			return item;
		}

		private static OMSComponent CreateSequentialContainerPageComponent(Oms oms, Instance inst, OmsContext context)
		{
			OMSComponent component = new OMSComponents.OMSSequentialContainerComponent();
			Instance instOrientation = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Sequential_Container_Page_Component__has__Sequential_Container_Orientation);
			if (instOrientation.GlobalIdentifier == KnownInstanceGuids.Orientation.Vertical)
			{
				// ((OMSComponents.OMSContainerComponent)component).Orientation = MBS.Framework.UserInterface.Orientation.Vertical;
			}
			else if (instOrientation.GlobalIdentifier == KnownInstanceGuids.Orientation.Horizontal)
			{
				// ((OMSComponents.OMSContainerComponent)component).Orientation = MBS.Framework.UserInterface.Orientation.Horizontal;
			}
			// TODO: set the orientation of the Panel for the Sequential Container Page Component

			Instance[] instComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Container_Page_Component__has__Page_Component);
			for (int i = 0; i < instComponents.Length; i++)
			{
				OMSComponent component2 = CreatePageComponent(oms, instComponents[i], context);
				if (component2 == null) continue;

				((OMSContainerComponent)component).Components.Add(component2);
			}

			return component;
		}
		private static OMSComponent CreateHeadingPageComponent(Oms oms, Instance inst, OmsContext context)
		{
			OMSHeaderComponent item = new OMSHeaderComponent();
			item.Level = (int)oms.GetAttributeValue<decimal>(inst, KnownAttributeGuids.Numeric.Level);

			item.Text = oms.ExecuteMethodReturningTextOrTranslation(oms.GetInstance(KnownRelationshipGuids.Content_Page_Component__gets_content_from__Method), context);
			return item;
		}

		// FIXME: BIG PROBLEM!!!
		// right now the CLIENT is bouncing back and forth requesting EVERY LITTLE INSTANCE From the server
		// especially when loading Pages, the CLIENT should make ONE SINGLE REQUEST for the page
		// the server should then push out EVERYTHING The client needs to make sense of the page
		// (e.g. push out an entire JSON response representing all the page components on the page)
		// rather than the client having to do this... handling 1000+ requests to and from the OMS each time.

		// tl;dr : THERE SHOULD ONLY BE ONE REQUEST TO THE OMS PER PAGE!!!

		private static OMSImageComponent CreateImagePageComponent(Oms oms, Instance inst, OmsContext context)
		{
			OMSImageComponent image = new OMSImageComponent();

			Instance instMethod = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Image_Page_Component__has_source__Method);
			// should return an Instance Set

			Instance instRet = null;
			object _instRet = oms.ExecuteMethod(instMethod, context);
			if (_instRet is Instance[])
			{
				if (((Instance[])_instRet).Length == 1)
				{
					instRet = ((Instance[])_instRet)[0];
				}
			}

			if (instRet == null)
				return null;

			InstanceKey ikTargetFile = oms.GetInstanceKey(instRet);
			if (ikTargetFile == InstanceKey.Empty)
				return null;

			image.TargetFileInstanceID = ikTargetFile;
			/*
			string attfmt = (OMS.GetAttributeValue(instTargetFile, KnownAttributeGuids.Text.ContentType) as string);
			string attvalue = (OMS.GetAttributeValue(instTargetFile, KnownAttributeGuids.Text.Value) as string);
			// image.ImageUrl = String.Format("data:{0};base64,{1}", attfmt, attvalue);
			image.ImageUrl = String.Format("~/Images/Uploads/{0}.png", instTargetFile.GlobalIdentifier.ToString("b"));
			*/
			return image;
		}
	}
}
