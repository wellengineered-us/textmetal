/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Sort
{
	[XyzlElementMapping(LocalName = "SortContainer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Items)]
	public sealed class SortContainerConstruct : SortXmlObject, ISortContainerConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SortContainerConstruct class.
		/// </summary>
		public SortContainerConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private string id;

		#endregion

		#region Properties/Indexers/Events

		public new IXmlObjectCollection<ISortXmlObject> Items
		{
			get
			{
				return new ContravariantListAdapter<ISortXmlObject, IXmlObject>(base.Items);
			}
		}

		public override bool? SortDirection
		{
			get
			{
				return null;
			}
		}

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

		#region Methods/Operators

		protected override IEnumerable CoreEvaluateSort(ITemplatingContext templatingContext, IEnumerable values)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			if ((object)values == null)
				throw new ArgumentNullException(nameof(values));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			if ((object)this.Items != null)
			{
				foreach (ISortXmlObject child in this.Items)
					values = child.EvaluateSort(templatingContext, values);
			}

			return values;
		}

		#endregion
	}
}