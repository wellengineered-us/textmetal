/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Template;

namespace WellEngineered.TextMetal.Hosting.Email
{
	[XyzlElementMapping(LocalName = "echo", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0")]
	public sealed class EmailTemplateEchoFragment : TextMetalTemplateObject
	{
		#region Constructors/Destructors

		public EmailTemplateEchoFragment()
		{
		}

		#endregion

		#region Methods/Operators

		protected override void CoreExpandTemplate(ITextMetalContext templatingContext)
		{
			templatingContext.CurrentOutput.TextWriter.Write(string.Format("hello:{0:O}", DateTime.Now));
		}

		protected async override ValueTask CoreExpandTemplateAsync(ITextMetalContext templatingContext)
		{
			await templatingContext.CurrentOutput.TextWriter.WriteAsync(string.Format("hello:{0:O}", DateTime.Now));
		}

		#endregion
	}
}