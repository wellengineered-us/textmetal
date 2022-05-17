/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Expression
{
	[XyzlElementMapping(LocalName = "NullaryExpression", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class NullaryExpressionConstruct : ExpressionXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the NullaryExpressionConstruct class.
		/// </summary>
		public NullaryExpressionConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly NullaryOperator nullaryOperator = NullaryOperator.Nop;

		#endregion

		#region Properties/Indexers/Events

		public NullaryOperator NullaryOperator
		{
			get
			{
				return this.nullaryOperator;
			}
		}

		#endregion

		#region Methods/Operators

		protected override object CoreEvaluateExpression(ITemplatingContext templatingContext)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			return null;
		}

		#endregion
	}
}