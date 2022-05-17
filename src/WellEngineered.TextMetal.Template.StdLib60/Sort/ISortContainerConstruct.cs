/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Sort
{
	[XmlConfiguredObjectKnownElementMapping(LocalName = "SortContainer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Items)]
	public interface ISortContainerConstruct : IItemsContainerXmlObject<ISortXmlObject>, ISortXmlObject
	{
	}
}