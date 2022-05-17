/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Output
{
	public interface ITextMetalOutputFactory : ITextMetalComponent
	{
		#region Methods/Operators

		ITextMetalOutput GetOutputObject(Uri baseUri, IDictionary<string, object> properties);

		ValueTask<ITextMetalOutput> GetOutputObjectAsync(Uri baseUri, IDictionary<string, object> properties);

		#endregion
	}
}