/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.TextMetal.Template.Expression
{
	/// <summary>
	/// Provides for expression mechanics.
	/// </summary>
	public interface IExpressionMechanism
	{
		#region Methods/Operators

		/// <summary>
		/// Evaluates at run-time, an expression tree yielding an object value result.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <returns> An expression return value or null. </returns>
		object EvaluateExpression(ITemplatingContext templatingContext);

		#endregion
	}
}