/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.TextMetal.Template.Expression
{
	public abstract class ExpressionXmlObject : XmlObject, IExpressionXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ExpressionXmlObject class.
		/// </summary>
		protected ExpressionXmlObject()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract object CoreEvaluateExpression(ITemplatingContext templatingContext);

		public object EvaluateExpression(ITemplatingContext templatingContext)
		{
			return this.CoreEvaluateExpression(templatingContext);
		}

		#endregion
	}
}