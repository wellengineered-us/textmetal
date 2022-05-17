/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Model;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Template;

namespace WellEngineered.TextMetal.Hosting
{
	public interface ITextMetalHost : ITextMetalContextFactory
	{
		#region Properties/Indexers/Events

		ITextMetalModelFactory ModelFactory
		{
			get;
		}

		ITextMetalOutputFactory OutputFactory
		{
			get;
		}

		ITextMetalTemplateFactory TemplateFactory
		{
			get;
		}

		#endregion

		#region Methods/Operators

		bool LaunchDebugger();

		Assembly LoadAssembly(string assemblyName);

		#endregion
	}
}