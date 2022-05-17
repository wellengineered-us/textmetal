/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Tokenization;
using WellEngineered.TextMetal.Context;

namespace WellEngineered.TextMetal.Template
{
	public abstract class TextMetalTemplateObject : ConfigurationObject, ITextMetalTemplateObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalTemplateObject class.
		/// </summary>
		protected TextMetalTemplateObject()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		protected virtual bool IsScopeBlock
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract void CoreExpandTemplate(ITextMetalContext templatingContext);

		protected abstract ValueTask CoreExpandTemplateAsync(ITextMetalContext templatingContext);

		public void ExpandTemplate(ITextMetalContext templatingContext)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetWildcardReplacer();

			if (this.IsScopeBlock)
				templatingContext.VariableTables.Push(new Dictionary<string, object>());

			this.CoreExpandTemplate(templatingContext);

			if (this.IsScopeBlock)
				templatingContext.VariableTables.Pop();
		}

		public async ValueTask ExpandTemplateAsync(ITextMetalContext templatingContext)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetWildcardReplacer();

			if (this.IsScopeBlock)
				templatingContext.VariableTables.Push(new Dictionary<string, object>());

			await this.CoreExpandTemplateAsync(templatingContext);

			if (this.IsScopeBlock)
				templatingContext.VariableTables.Pop();
		}

		#endregion
	}
}