/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System.Threading.Tasks;

using WellEngineered.TextMetal.Context;

namespace WellEngineered.TextMetal.Template
{
	public interface ITextMetalTemplate /* : ITextMetalComponent ??? */
	{
		#region Methods/Operators

		/// <summary>
		/// Expands the template tree into the templating context current output.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		void ExpandTemplate(ITextMetalContext templatingContext);

		ValueTask ExpandTemplateAsync(ITextMetalContext templatingContext);

		#endregion
	}
}