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
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Hosting.Email
{
	public sealed class StringWriterOutput : TextMetalOutput
	{
		#region Constructors/Destructors

		public StringWriterOutput(StringWriter stringWriter, IDictionary<string, object> properties)
			: base(stringWriter, properties)
		{
			if ((object)stringWriter == null)
				throw new ArgumentNullException(nameof(stringWriter));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			this.stringWriter = stringWriter;
		}

		#endregion

		#region Fields/Constants

		private readonly StringWriter stringWriter;

		#endregion

		#region Properties/Indexers/Events

		public StringWriter StringWriter
		{
			get
			{
				return this.stringWriter;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreEnterScope(string scopeName, bool appendMode, Encoding encoding)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override ValueTask CoreEnterScopeAsync(string scopeName, bool appendMode, Encoding encoding)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override void CoreLeaveScope(string scopeName)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override ValueTask CoreLeaveScopeAsync(string scopeName)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override void CoreWriteObject(object obj, Uri objectUri)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override ValueTask CoreWriteObjectAsync(object obj, Uri objectUri)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		#endregion
	}
}