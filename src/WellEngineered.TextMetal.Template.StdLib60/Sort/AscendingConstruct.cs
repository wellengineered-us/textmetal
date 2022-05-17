/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Sort
{
	[XyzlElementMapping(LocalName = "Ascending", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class AscendingConstruct : OrderConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the AscendingConstruct class.
		/// </summary>
		public AscendingConstruct()
			: base(true)
		{
		}

		#endregion
	}
}