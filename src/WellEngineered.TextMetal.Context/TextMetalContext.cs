/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Component;
using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.Solder.Utilities;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Context
{
	public abstract class TextMetalContext : TextMetalComponent, ITextMetalContext
	{
		#region Constructors/Destructors

		protected TextMetalContext(IDataTypeFascade dataTypeFascade,
			IReflectionFascade reflectionFascade,
			Tokenizer tokenizer)
		{
			if ((object)dataTypeFascade == null)
				throw new ArgumentNullException(nameof(dataTypeFascade));

			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			if ((object)tokenizer == null)
				throw new ArgumentNullException(nameof(tokenizer));

			this.dataTypeFascade = dataTypeFascade;
			this.reflectionFascade = reflectionFascade;
			this.tokenizer = tokenizer;
		}

		#endregion

		#region Fields/Constants

		private readonly IDataTypeFascade dataTypeFascade;

		private readonly Stack<object> iteratorModels = new Stack<object>();
		private readonly Stack<ITextMetalOutput> outputs = new Stack<ITextMetalOutput>();
		private readonly IReflectionFascade reflectionFascade;
		private readonly Tokenizer tokenizer;
		private readonly Stack<Dictionary<string, object>> variableTables = new Stack<Dictionary<string, object>>();
		private ITextMetalOutput diagnosticOutput;

		#endregion

		#region Properties/Indexers/Events

		public object CurrentIteratorModel
		{
			get
			{
				return this.IteratorModels.Count > 0 ? this.IteratorModels.Peek() : null;
			}
		}

		public ITextMetalOutput CurrentOutput
		{
			get
			{
				return this.Outputs.Count > 0 ? this.Outputs.Peek() : this.CoreGetDefaultOutput();
			}
		}

		public IDictionary<string, object> CurrentVariableTable
		{
			get
			{
				return this.VariableTables.Count > 0 ? this.VariableTables.Peek() : null;
			}
		}

		protected IDataTypeFascade DataTypeFascade
		{
			get
			{
				return this.dataTypeFascade;
			}
		}

		public Stack<object> IteratorModels
		{
			get
			{
				return this.iteratorModels;
			}
		}

		protected Stack<ITextMetalOutput> Outputs
		{
			get
			{
				return this.outputs;
			}
		}

		protected IReflectionFascade ReflectionFascade
		{
			get
			{
				return this.reflectionFascade;
			}
		}

		public Tokenizer Tokenizer
		{
			get
			{
				return this.tokenizer;
			}
		}

		public Stack<Dictionary<string, object>> VariableTables
		{
			get
			{
				return this.variableTables;
			}
		}

		public ITextMetalOutput DiagnosticOutput
		{
			get
			{
				return this.diagnosticOutput ?? this.CoreGetDefaultOutput();
			}
			protected set
			{
				this.diagnosticOutput = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected DynamicWildcardTokenReplacementStrategy __GetWildcardReplacer(bool strict)
		{
			List<object> temp;
			List<Dictionary<string, object>> temp2;

			// need to review how top level objects are accessed?
			temp = new List<object>(this.IteratorModels);
			temp2 = new List<Dictionary<string, object>>(this.VariableTables);

			temp.InsertRange(0, temp2.ToArray());

			return new DynamicWildcardTokenReplacementStrategy(this.DataTypeFascade, this.ReflectionFascade, temp.ToArray(), strict);
		}

		public void AddTemplatingReference(Type templateObjectType)
		{
			if ((object)templateObjectType == null)
				throw new ArgumentNullException(nameof(templateObjectType));

			try
			{
				this.CoreAddTemplatingReference(templateObjectType);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public void AddTemplatingReference(IXyzlName xmlName, Type templateObjectType)
		{
			if ((object)xmlName == null)
				throw new ArgumentNullException(nameof(xmlName));

			if ((object)templateObjectType == null)
				throw new ArgumentNullException(nameof(templateObjectType));

			try
			{
				this.CoreAddTemplatingReference(xmlName, templateObjectType);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public void ClearTemplatingReferences()
		{
			try
			{
				this.CoreClearTemplatingReferences();
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		protected abstract void CoreAddTemplatingReference(Type templateObjectType);

		protected abstract void CoreAddTemplatingReference(IXyzlName xmlName, Type templateObjectType);

		protected abstract void CoreClearTemplatingReferences();

		protected abstract ITextMetalOutput CoreGetDefaultOutput();

		protected abstract Type CoreGetTemplatingReference();

		protected abstract DynamicWildcardTokenReplacementStrategy CoreGetWildcardReplacer(bool strict);

		protected abstract IDictionary<IXyzlName, Type> CoreListTemplatingReferences();

		protected abstract void CoreSetTemplatingReference(Type templateObjectType);

		public Type GetTemplatingReference()
		{
			try
			{
				return this.CoreGetTemplatingReference();
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public DynamicWildcardTokenReplacementStrategy GetWildcardReplacer()
		{
			try
			{
				return this.CoreGetWildcardReplacer(this.Tokenizer.StrictMatching);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public DynamicWildcardTokenReplacementStrategy GetWildcardReplacer(bool strict)
		{
			try
			{
				return this.CoreGetWildcardReplacer(strict);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The context failed (see inner exception)."), ex);
			}
		}

		public IDictionary<IXyzlName, Type> ListTemplatingReferences()
		{
			try
			{
				return this.CoreListTemplatingReferences();
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public void SetTemplatingReference(Type templateObjectType)
		{
			if ((object)templateObjectType == null)
				throw new ArgumentNullException(nameof(templateObjectType));

			try
			{
				this.CoreSetTemplatingReference(templateObjectType);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}