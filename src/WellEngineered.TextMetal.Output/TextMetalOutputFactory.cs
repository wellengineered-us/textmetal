/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.Solder.Component;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Output
{
	public abstract class TextMetalOutputFactory : TextMetalComponent, ITextMetalOutputFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalOutputFactory class.
		/// </summary>
		protected TextMetalOutputFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract ITextMetalOutput CoreGetOutputObject(Uri baseUri, IDictionary<string, object> properties);

		protected abstract ValueTask<ITextMetalOutput> CoreGetOutputObjectAsync(Uri baseUri, IDictionary<string, object> properties);

		public ITextMetalOutput GetOutputObject(Uri baseUri, IDictionary<string, object> properties)
		{
			if ((object)baseUri == null)
				throw new ArgumentNullException(nameof(baseUri));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return this.CoreGetOutputObject(baseUri, properties);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output factory failed (see inner exception)."), ex);
			}
		}

		public async ValueTask<ITextMetalOutput> GetOutputObjectAsync(Uri baseUri, IDictionary<string, object> properties)
		{
			if ((object)baseUri == null)
				throw new ArgumentNullException(nameof(baseUri));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return await this.CoreGetOutputObjectAsync(baseUri, properties);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}