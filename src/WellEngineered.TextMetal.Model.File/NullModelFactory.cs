/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WellEngineered.TextMetal.Model.File
{
	public class NullModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the NullSourceStrategy class.
		/// </summary>
		public NullModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			return string.Empty;
		}
		
		protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			await Task.CompletedTask;
			return string.Empty;
		}

		#endregion
	}
}