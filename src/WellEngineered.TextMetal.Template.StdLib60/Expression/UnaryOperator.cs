/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.TextMetal.Template.Expression
{
	public enum UnaryOperator
	{
		[OperatorText("")]
		Undefined = 0,

		[OperatorText("!")]
		Not,

		[OperatorText("{is_null}")]
		IsNull,

		[OperatorText("{is_not_null}")]
		IsNotNull, // yes, it is redundant

		[OperatorText("{is_defined}")]
		IsDef,

		[OperatorText("-")]
		Neg,

		[OperatorText("+")]
		Pos,

		[OperatorText("++")]
		Incr,

		[OperatorText("--")]
		Decr,

		[OperatorText("~")]
		BComp
	}
}