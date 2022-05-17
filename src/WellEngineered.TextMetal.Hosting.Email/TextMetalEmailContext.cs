/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.Solder.Utilities;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Hosting.Email
{
	public sealed class TextMetalEmailContext : TextMetalContext
	{
		#region Constructors/Destructors

		public TextMetalEmailContext(StringWriter stringWriter,
			IDataTypeFascade dataTypeFascade,
			IReflectionFascade reflectionFascade,
			Tokenizer tokenizer)
			: base(dataTypeFascade, reflectionFascade, tokenizer)
		{
			if ((object)stringWriter == null)
				throw new ArgumentNullException(nameof(stringWriter));

			this.stringWriter = stringWriter;
		}

		#endregion

		#region Fields/Constants

		private readonly StringWriter stringWriter;

		#endregion

		#region Properties/Indexers/Events

		public StringWriter StringWriter
		{
			get
			{
				return this.stringWriter;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreAddTemplatingReference(Type templateObjectType)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override void CoreAddTemplatingReference(IXyzlName xmlName, Type templateObjectType)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override void CoreClearTemplatingReferences()
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override ITextMetalOutput CoreGetDefaultOutput()
		{
			return new StringWriterOutput(this.StringWriter, new Dictionary<string, object>());
		}

		protected override Type CoreGetTemplatingReference()
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override DynamicWildcardTokenReplacementStrategy CoreGetWildcardReplacer(bool strict)
		{
			return this.__GetWildcardReplacer(strict);
		}

		protected override IDictionary<IXyzlName, Type> CoreListTemplatingReferences()
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override void CoreSetTemplatingReference(Type templateObjectType)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		#endregion
	}
}