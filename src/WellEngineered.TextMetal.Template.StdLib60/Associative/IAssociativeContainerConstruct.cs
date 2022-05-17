/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Associative
{
	[XmlConfiguredObjectKnownElementMapping(LocalName = "AssociativeContainer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Content)]
	public interface IAssociativeContainerConstruct : IContentContainerXmlObject<IAssociativeXmlObject>, IAssociativeXmlObject
	{
	}
}