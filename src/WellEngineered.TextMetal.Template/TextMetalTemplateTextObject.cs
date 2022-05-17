/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.TextMetal.Context;

namespace WellEngineered.TextMetal.Template
{
	[XyzlElementMapping(ChildElementMode = XyzlChildMode.Value)]
	public sealed class TextMetalTemplateTextObject : TextMetalTemplateObject, IXyzlValueObject<string>
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalTemplateTextObject class.
		/// </summary>
		public TextMetalTemplateTextObject()
		{
		}

		#endregion

		#region Fields/Constants

		private IXyzlName name;
		private string text;

		#endregion

		#region Properties/Indexers/Events

		public IXyzlName Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		object IXyzlValueObject.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (string)value;
			}
		}

		public string Value
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreExpandTemplate(ITextMetalContext templatingContext)
		{
			string text;
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetWildcardReplacer();

			text = templatingContext.Tokenizer.ExpandTokens(this.Value, dynamicWildcardTokenReplacementStrategy);

			templatingContext.CurrentOutput.TextWriter.Write(text);
		}

		protected override async ValueTask CoreExpandTemplateAsync(ITextMetalContext templatingContext)
		{
			string text;
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetWildcardReplacer();

			text = templatingContext.Tokenizer.ExpandTokens(this.Value, dynamicWildcardTokenReplacementStrategy);

			await templatingContext.CurrentOutput.TextWriter.WriteAsync(text);
		}

		#endregion
	}
}