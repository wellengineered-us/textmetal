/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Expression
{
	[XyzlElementMapping(LocalName = "ExpressionContainer", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Content)]
	public sealed class ExpressionContainerConstruct : ExpressionXmlObject, IExpressionContainerConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ExpressionContainerConstruct class.
		/// </summary>
		public ExpressionContainerConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private string id;

		#endregion

		#region Properties/Indexers/Events

		public new IExpressionXmlObject Content
		{
			get
			{
				return (IExpressionXmlObject)base.Content;
			}
			set
			{
				base.Content = value;
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

		protected override object CoreEvaluateExpression(ITemplatingContext templatingContext)
		{
			object ovalue;
			string svalue;
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			if ((object)this.Content != null)
				ovalue = ((IExpressionXmlObject)this.Content).EvaluateExpression(templatingContext);
			else
				ovalue = null;

			svalue = ovalue as string;

			if ((object)svalue != null)
				ovalue = templatingContext.Tokenizer.ExpandTokens(svalue, dynamicWildcardTokenReplacementStrategy);

			return ovalue;
		}

		#endregion
	}
}