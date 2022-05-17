/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.TextMetal.Template.Expression;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "Assign", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class AssignConstruct : TemplateXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AssignConstruct class.
		/// </summary>
		public AssignConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private IExpressionContainerConstruct expression;
		private string token;

		#endregion

		#region Properties/Indexers/Events

		[XmlChildElementMapping(ChildElementType = ChildElementType.ParentQualified, LocalName = "Expression", NamespaceUri = "http://www.textmetal.com/api/v6.0.0")]
		public IExpressionContainerConstruct Expression
		{
			get
			{
				return this.expression;
			}
			set
			{
				this.expression = value;
			}
		}

		[XmlAttributeMapping(LocalName = "token", NamespaceUri = "")]
		public string Token
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreExpandTemplate(ITemplatingContext templatingContext)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;
			object obj = null;
			string token;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			token = templatingContext.Tokenizer.ExpandTokens(this.Token, dynamicWildcardTokenReplacementStrategy);

			if ((object)this.Expression != null)
				obj = this.Expression.EvaluateExpression(templatingContext);

			if (!dynamicWildcardTokenReplacementStrategy.SetByToken(token, obj))
				throw new InvalidOperationException(string.Format("The facet name '{0}' was not found on the target model.", token));
		}

		#endregion
	}
}