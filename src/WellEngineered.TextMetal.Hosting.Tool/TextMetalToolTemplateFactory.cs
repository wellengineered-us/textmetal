/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Template;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public class TextMetalToolTemplateFactory : TextMetalTemplateFactory
	{
		#region Constructors/Destructors

		public TextMetalToolTemplateFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override ITextMetalTemplate CoreGetTemplateObject(Uri templateUri, IDictionary<string, object> properties)
		{
			StringBuilder value;

			value = new StringBuilder();

			value.Append("<Root>");
			value.Append(Environment.NewLine);
			
			for (int i = 0; i < 1000; i++)
			{
				bool first = true;
				
				value.Append(string.Format("\t<Loop index=\"{0:000}\">", i));
				value.Append(Environment.NewLine);
				
				foreach (KeyValuePair<string, object> keyValuePair in properties)
				{
					value.Append(string.Format("\t\t<Property guid=\"${{Environment.LazyGuid}}\" date=\"${{Environment.GetDate}}\" name=\"{0}.{1}\" value=\"${{{0}.{1}}}\" />", "Properties", keyValuePair.Key));
					value.Append(Environment.NewLine);
					
					if (first)
						first = false;
				}
				
				value.Append(i.ToString("\t</Loop>"));
				value.Append(Environment.NewLine);
			}
			
			value.Append("</Root>");
			value.Append(Environment.NewLine);

			return new TextMetalTemplateTextObject() { Value = value.ToString() };
		}

		protected override ValueTask<ITextMetalTemplate> CoreGetTemplateObjectAsync(Uri templateUri, IDictionary<string, object> properties)
		{
			return default;
		}

		protected override string CoreLoadTemplateContent(Uri contentUri)
		{
			return null;
		}

		protected override ValueTask<string> CoreLoadTemplateContentAsync(Uri contentUri)
		{
			return default;
		}

		#endregion
	}
}