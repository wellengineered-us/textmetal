/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "If", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class IfConstruct : ConditionalBranchConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the IfConstruct class.
		/// </summary>
		public IfConstruct()
			: base(true)
		{
		}

		#endregion
	}
}