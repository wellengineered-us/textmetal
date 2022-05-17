/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.TextMetal.Template.Expression;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "For", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class ForConstruct : ConditionalIterationConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ForConstruct class.
		/// </summary>
		public ForConstruct()
			: base(true, true)
		{
		}

		#endregion

		#region Fields/Constants

		private IExpressionContainerConstruct initializer;
		private IExpressionContainerConstruct iterator;

		#endregion

		#region Properties/Indexers/Events

		[XmlChildElementMapping(ChildElementType = ChildElementType.ParentQualified, LocalName = "Initializer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0")]
		public IExpressionContainerConstruct Initializer
		{
			get
			{
				return this.initializer;
			}
			set
			{
				this.initializer = value;
			}
		}

		[XmlChildElementMapping(ChildElementType = ChildElementType.ParentQualified, LocalName = "Iterator", NamespaceUri = "http://www.textmetal.com/api/v6.0.0")]
		public IExpressionContainerConstruct Iterator
		{
			get
			{
				return this.iterator;
			}
			set
			{
				this.iterator = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreConditionalIterationInitialize(ITemplatingContext templatingContext)
		{
			object value;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			if ((object)this.Initializer != null)
				value = this.Initializer.EvaluateExpression(templatingContext);
		}

		protected override void CoreConditionalIterationStep(ITemplatingContext templatingContext, uint indexOneBase)
		{
			object value;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			if ((object)this.Iterator != null)
				value = this.Iterator.EvaluateExpression(templatingContext);
		}

		protected override void CoreConditionalIterationTerminate(ITemplatingContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));
		}

		#endregion
	}
}