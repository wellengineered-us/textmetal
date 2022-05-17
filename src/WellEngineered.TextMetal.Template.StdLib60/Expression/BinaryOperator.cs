/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.TextMetal.Template.Expression
{
	public enum BinaryOperator
	{
		[OperatorText("")]
		Undefined = 0,

		[OperatorText("+")]
		Add,

		[OperatorText("-")]
		Sub,

		[OperatorText("/")]
		Div,

		[OperatorText("*")]
		Mul,

		[OperatorText("%")]
		Mod,

		[OperatorText("&&")]
		And,

		[OperatorText("||")]
		Or,

		[OperatorText("^^")]
		Xor,

		[OperatorText("==")]
		Eq,

		[OperatorText("!=")]
		Ne,

		[OperatorText("<")]
		Lt,

		[OperatorText("<=")]
		Le,

		[OperatorText(">")]
		Gt,

		[OperatorText(">=")]
		Ge,

		[OperatorText("{like}")]
		StrLk,

		[OperatorText("{as}")]
		ObjAs,

		[OperatorText("{is}")]
		ObjIs,

		[OperatorText(":=")]
		VarPut,

		[OperatorText("&")]
		Band,

		[OperatorText("|")]
		Bor,

		[OperatorText("^")]
		Bxor,

		[OperatorText("<<")]
		Bls,

		[OperatorText(">>")]
		Brs,

		[OperatorText(">^>")]
		Bsr,

		[OperatorText(">_>")]
		Bur,

		[OperatorText("{parse}")]
		Parse,
	}
}