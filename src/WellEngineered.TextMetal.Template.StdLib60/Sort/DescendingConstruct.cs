/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Sort
{
	[XyzlElementMapping(LocalName = "Descending", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class DescendingConstruct : OrderConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DescendingConstruct class.
		/// </summary>
		public DescendingConstruct()
			: base(false)
		{
		}

		#endregion
	}
}