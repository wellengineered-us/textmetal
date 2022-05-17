/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

namespace WellEngineered.TextMetal.Primitives
{
	public interface ITextMetalItemsContainerObject<TItemsRestriction> : ITextMetalContainerObject
		where TItemsRestriction : ITextMetalConfigurationObject
	{
		#region Properties/Indexers/Events

		new ITextMetalConfigurationObjectCollection<TItemsRestriction> Items
		{
			get;
		}

		#endregion
	}
}