/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace WellEngineered.TextMetal.Template.Associative
{
	/// <summary>
	/// Represents an associative XML object.
	/// </summary>
	public interface IAssociativeXmlObject : IAssociativeMechanism, IXmlObject
	{
		#region Properties/Indexers/Events

		/// <summary>
		/// Gets the associative name of the current associative XML object.
		/// </summary>
		string Name
		{
			get;
		}

		#endregion
	}
}