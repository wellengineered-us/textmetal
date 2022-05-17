/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.Solder.Utilities;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Output;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public class TextMetalToolContext : TextMetalContext, ITextMetalToolContext
	{
		#region Constructors/Destructors

		public TextMetalToolContext(ITextMetalToolHost toolHost,
			IDataTypeFascade dataTypeFascade,
			IReflectionFascade reflectionFascade,
			Tokenizer tokenizer)
			: base(dataTypeFascade, reflectionFascade, tokenizer)
		{
			if ((object)toolHost == null)
				throw new ArgumentNullException(nameof(toolHost));

			this.toolHost = toolHost;
		}

		#endregion

		#region Fields/Constants

		private readonly ITextMetalToolHost toolHost;

		#endregion

		#region Properties/Indexers/Events

		public ITextMetalToolHost ToolHost
		{
			get
			{
				return this.toolHost;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreAddTemplatingReference(Type templateObjectType)
		{
		}

		protected override void CoreAddTemplatingReference(IXyzlName xmlName, Type templateObjectType)
		{
		}

		protected override void CoreClearTemplatingReferences()
		{
		}

		protected override ITextMetalOutput CoreGetDefaultOutput()
		{
			return TextMetalToolOutput.Default;
		}

		protected override Type CoreGetTemplatingReference()
		{
			return null;
		}

		protected override DynamicWildcardTokenReplacementStrategy CoreGetWildcardReplacer(bool strict)
		{
			return this.__GetWildcardReplacer(strict);
		}

		protected override IDictionary<IXyzlName, Type> CoreListTemplatingReferences()
		{
			return null;
		}

		protected override void CoreSetTemplatingReference(Type templateObjectType)
		{
		}

		#endregion
	}
}