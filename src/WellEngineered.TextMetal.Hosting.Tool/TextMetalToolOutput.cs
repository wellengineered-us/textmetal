/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Output;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public class TextMetalToolOutput : TextMetalOutput
	{
		#region Constructors/Destructors

		private TextMetalToolOutput(TextWriter textWriter, IDictionary<string, object> properties)
			: base(textWriter, properties)
		{
		}

		#endregion

		#region Fields/Constants

		private static ITextMetalOutput @default = new TextMetalToolOutput(Console.Out, new Dictionary<string, object>());

		#endregion

		#region Properties/Indexers/Events

		public static ITextMetalOutput Default
		{
			get
			{
				return @default;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreEnterScope(string scopeName, bool appendMode, Encoding encoding)
		{
		}

		protected override ValueTask CoreEnterScopeAsync(string scopeName, bool appendMode, Encoding encoding)
		{
			return default;
		}

		protected override void CoreLeaveScope(string scopeName)
		{
		}

		protected override ValueTask CoreLeaveScopeAsync(string scopeName)
		{
			return default;
		}

		protected override void CoreWriteObject(object obj, Uri objectUri)
		{
		}

		protected override ValueTask CoreWriteObjectAsync(object obj, Uri objectUri)
		{
			return default;
		}

		#endregion
	}
}