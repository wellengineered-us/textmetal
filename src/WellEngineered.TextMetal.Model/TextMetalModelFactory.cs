/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.Solder.Component;
using WellEngineered.TextMetal.Primitives;

using __ITextMetalModel = System.Object;

namespace WellEngineered.TextMetal.Model
{
	public abstract class TextMetalModelFactory : TextMetalComponent, ITextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalModelFactory class.
		/// </summary>
		protected TextMetalModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected abstract __ITextMetalModel CoreGetModelObject(IDictionary<string, object> properties);

		protected abstract ValueTask<__ITextMetalModel> CoreGetModelObjectAsync(IDictionary<string, object> properties);

		public __ITextMetalModel GetModelObject(IDictionary<string, object> properties)
		{
			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return this.CoreGetModelObject(properties);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The model factory failed (see inner exception)."), ex);
			}
		}

		public async ValueTask<__ITextMetalModel> GetModelObjectAsync(IDictionary<string, object> properties)
		{
			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			try
			{
				return await this.CoreGetModelObjectAsync(properties);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The model factory failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}