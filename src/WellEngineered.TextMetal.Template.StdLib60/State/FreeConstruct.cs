/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "Free", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class FreeConstruct : TemplateXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the FreeConstruct class.
		/// </summary>
		public FreeConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private string token;

		#endregion

		#region Properties/Indexers/Events

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
			string token;
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			token = templatingContext.Tokenizer.ExpandTokens(this.Token, dynamicWildcardTokenReplacementStrategy);

			if (!templatingContext.CurrentVariableTable.ContainsKey(token))
			{
				if (templatingContext.Tokenizer.StrictMatching)
					throw new InvalidOperationException(string.Format("When strict matching semantics are enabled, the variable '{0}' cannot be freed if it has not been defined previously in the given scope block.", token));

				return;
			}

			templatingContext.CurrentVariableTable.Remove(token);
		}

		#endregion
	}
}