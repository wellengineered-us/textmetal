/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Model;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public class TextMetalToolModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		public TextMetalToolModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			return null;
		}

		protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			await Task.CompletedTask;
			return null;
		}

		#endregion
	}
}