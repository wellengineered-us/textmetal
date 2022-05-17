/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.TextMetal.Template.Associative;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "Expando", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class ExpandoConstruct : TemplateXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ExpandoConstruct class.
		/// </summary>
		public ExpandoConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private IAssociativeContainerConstruct dynamic;

		#endregion

		#region Properties/Indexers/Events

		[XmlChildElementMapping(ChildElementType = ChildElementType.ParentQualified, LocalName = "Dynamic", NamespaceUri = "http://www.textmetal.com/api/v6.0.0")]
		public IAssociativeContainerConstruct Dynamic
		{
			get
			{
				return this.dynamic;
			}
			set
			{
				this.dynamic = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreExpandTemplate(ITemplatingContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			// add global cleanup into context to suppro this....
			//templatingContext.IteratorModels.Push(this.Dynamic.Content);
			// BUG: this never gets popped !!!
		}

		#endregion
	}
}