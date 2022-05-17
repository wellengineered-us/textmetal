/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Hosting
{
	[XyzlKnownElementMapping(LocalName = "TemplateContainer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementMode = XyzlChildMode.Items)]
	public interface ITemplateContainer : ITextMetalItemsContainerObject<ITextMetalConfigurationObject>, ITextMetalConfigurationObject
	{
	}
}