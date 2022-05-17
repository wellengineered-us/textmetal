/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.TextMetal.Primitives
{
	public interface ITextMetalContentContainerObject<TContentRestriction> : ITextMetalContainerObject
		where TContentRestriction : ITextMetalConfigurationObject
	{
		#region Properties/Indexers/Events

		new TContentRestriction Content
		{
			get;
			set;
		}

		#endregion
	}
}