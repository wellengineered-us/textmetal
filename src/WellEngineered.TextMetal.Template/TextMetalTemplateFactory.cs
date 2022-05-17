/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.Solder.Component;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Template
{
	public abstract class TextMetalTemplateFactory : TextMetalComponent, ITextMetalTemplateFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalTemplateFactory class.
		/// </summary>
		protected TextMetalTemplateFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract ITextMetalTemplate CoreGetTemplateObject(Uri templateUri, IDictionary<string, object> properties);

		protected abstract ValueTask<ITextMetalTemplate> CoreGetTemplateObjectAsync(Uri templateUri, IDictionary<string, object> properties);

		protected abstract string CoreLoadTemplateContent(Uri contentUri);

		protected abstract ValueTask<string> CoreLoadTemplateContentAsync(Uri contentUri);

		public ITextMetalTemplate GetTemplateObject(Uri templateUri, IDictionary<string, object> properties)
		{
			if ((object)templateUri == null)
				throw new ArgumentNullException(nameof(templateUri));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return this.CoreGetTemplateObject(templateUri, properties);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The template factory failed (see inner exception)."), ex);
			}
		}

		public async ValueTask<ITextMetalTemplate> GetTemplateObjectAsync(Uri templateUri, IDictionary<string, object> properties)
		{
			if ((object)templateUri == null)
				throw new ArgumentNullException(nameof(templateUri));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return await this.CoreGetTemplateObjectAsync(templateUri, properties);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The template factory failed (see inner exception)."), ex);
			}
		}

		public string LoadTemplateContent(Uri contentUri)
		{
			if ((object)contentUri == null)
				throw new ArgumentNullException(nameof(contentUri));

			try
			{
				return this.CoreLoadTemplateContent(contentUri);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The template factory failed (see inner exception)."), ex);
			}
		}

		public async ValueTask<string> LoadTemplateContentAsync(Uri contentUri)
		{
			if ((object)contentUri == null)
				throw new ArgumentNullException(nameof(contentUri));

			try
			{
				return await this.CoreLoadTemplateContentAsync(contentUri);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The template factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}