/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Expression
{
	[XyzlElementMapping(LocalName = "Facet", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class FacetConstruct : SurfaceConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the FacetConstruct class.
		/// </summary>
		public FacetConstruct()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreEvaluateExpression(ITemplatingContext templatingContext)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;
			object obj;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			if (!dynamicWildcardTokenReplacementStrategy.GetByToken(this.Name, out obj))
				throw new InvalidOperationException(string.Format("The facet name '{0}' was not found on the target model.", this.Name));

			return obj;
		}

		#endregion
	}
}