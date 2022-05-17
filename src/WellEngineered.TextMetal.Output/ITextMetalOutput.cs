/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Output
{
	public interface ITextMetalOutput : ITextMetalComponent
	{
		#region Properties/Indexers/Events

		IDictionary<string, object> Properties
		{
			get;
		}

		TextWriter TextWriter
		{
			get;
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Enters (pushes) an output scope as delineated by scope name.
		/// Scope name semantics are implementation specific.
		/// </summary>
		/// <param name="scopeName"> The scope name to push. </param>
		/// <param name="appendMode"> A value indicating whether to append or not. </param>
		/// <param name="encoding"> A text encoding in-effect for this new scope. </param>
		void EnterScope(string scopeName, bool appendMode, Encoding encoding);

		ValueTask EnterScopeAsync(string scopeName, bool appendMode, Encoding encoding);

		/// <summary>
		/// Leaves (pops) an output scope as delineated by scope name.
		/// Scope name semantics are implementation specific.
		/// </summary>
		/// <param name="scopeName"> The scope name to pop. </param>
		void LeaveScope(string scopeName);

		ValueTask LeaveScopeAsync(string scopeName);

		/// <summary>
		/// Writes a serialized object to a location specified by object name.
		/// Object URI semantics are implementation specific.
		/// </summary>
		/// <param name="obj"> The object to serialize. </param>
		/// <param name="objectUri"> The object URI for which to write. </param>
		void WriteObject(object obj, Uri objectUri);

		ValueTask WriteObjectAsync(object obj, Uri objectUri);

		#endregion
	}
}