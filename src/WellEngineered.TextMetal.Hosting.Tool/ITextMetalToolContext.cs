/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.TextMetal.Context;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public interface ITextMetalToolContext : ITextMetalContext
	{
		#region Properties/Indexers/Events

		ITextMetalToolHost ToolHost
		{
			get;
		}

		#endregion
	}
}