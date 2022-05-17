/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.Solder.Component;
using WellEngineered.TextMetal.Hosting.Tool.Configuration;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public interface ITextMetalToolHost : ITextMetalHost, IConfigurableComponent<TextMetalToolHostConfiguration>
	{
		#region Methods/Operators

		void Host(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties);

		ValueTask HostAsync(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties);

		#endregion
	}
}