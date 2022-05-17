/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.TextMetal.Primitives
{
	/// <summary>
	/// The exception thrown when a TextMetal runtime error occurs.
	/// </summary>
	public sealed class TextMetalException : Exception
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalException class.
		/// </summary>
		public TextMetalException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the TextMetalException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		public TextMetalException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the TextMetalException class.
		/// </summary>
		/// <param name="message"> The message that describes the error. </param>
		/// <param name="innerException"> The inner exception. </param>
		public TextMetalException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		#endregion
	}
}