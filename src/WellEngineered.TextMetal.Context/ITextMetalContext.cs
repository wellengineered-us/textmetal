/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Context
{
	public interface ITextMetalContext : ITextMetalComponent
	{
		#region Properties/Indexers/Events

		object CurrentIteratorModel
		{
			get;
		}

		ITextMetalOutput CurrentOutput
		{
			get;
		}

		IDictionary<string, object> CurrentVariableTable
		{
			get;
		}

		ITextMetalOutput DiagnosticOutput
		{
			get;
		}

		Stack<object> IteratorModels
		{
			get;
		}

		Tokenizer Tokenizer
		{
			get;
		}

		Stack<Dictionary<string, object>> VariableTables
		{
			get;
		}

		#endregion

		#region Methods/Operators

		void AddTemplatingReference(Type templateObjectType);

		void AddTemplatingReference(IXyzlName xmlName, Type templateObjectType);

		void ClearTemplatingReferences();

		Type GetTemplatingReference();

		DynamicWildcardTokenReplacementStrategy GetWildcardReplacer();

		DynamicWildcardTokenReplacementStrategy GetWildcardReplacer(bool strict);

		IDictionary<IXyzlName, Type> ListTemplatingReferences();

		void SetTemplatingReference(Type templateObjectType);

		#endregion
	}
}