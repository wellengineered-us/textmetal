/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Associative
{
	/// <summary>
	/// Provides an XML construct for associative objects (not a base class however).
	/// </summary>
	[XyzlElementMapping(LocalName = "Object", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Items)]
	public class ObjectConstruct : AssociativeXmlObject, IObjectReference
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ObjectConstruct class.
		/// </summary>
		public ObjectConstruct()
		{
		}

		#endregion
	}
}