/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Output;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public class TextMetalToolOutputFactory : TextMetalOutputFactory
	{
		#region Constructors/Destructors

		public TextMetalToolOutputFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override ITextMetalOutput CoreGetOutputObject(Uri baseUri, IDictionary<string, object> properties)
		{
			return TextMetalToolOutput.Default;
		}

		protected async override ValueTask<ITextMetalOutput> CoreGetOutputObjectAsync(Uri baseUri, IDictionary<string, object> properties)
		{
			await Task.CompletedTask;
			return TextMetalToolOutput.Default;
		}

		#endregion
	}
}