/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Associative
{
	/// <summary>
	/// Provides an XML construct for associative model containers.
	/// </summary>
	[XyzlElementMapping(LocalName = "AssociativeContainer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Content)]
	public sealed class AssociativeContainerConstruct : AssociativeXmlObject, IAssociativeContainerConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AssociativeContainerConstruct class.
		/// </summary>
		public AssociativeContainerConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private string id;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets or sets the optional single XML object content as a strongly-typed associative XML object.
		/// </summary>
		public new IAssociativeXmlObject Content
		{
			get
			{
				return (IAssociativeXmlObject)base.Content;
			}
			set
			{
				base.Content = value;
			}
		}

		/// <summary>
		/// Gets the associative ID of the current associative model container XML object.
		/// </summary>
		[XmlAttributeMapping(LocalName = "id", NamespaceUri = "")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		#endregion
	}
}