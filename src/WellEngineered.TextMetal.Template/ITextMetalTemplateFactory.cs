/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Template
{
	/// <summary>
	/// Provides a factory pattern around acquiring template objects.
	/// </summary>
	public interface ITextMetalTemplateFactory : ITextMetalComponent
	{
		#region Methods/Operators

		ITextMetalTemplate GetTemplateObject(Uri templateUri, IDictionary<string, object> properties);

		ValueTask<ITextMetalTemplate> GetTemplateObjectAsync(Uri templateUri, IDictionary<string, object> properties);

		string LoadTemplateContent(Uri contentUri);

		ValueTask<string> LoadTemplateContentAsync(Uri contentUri);

		#endregion
	}
}