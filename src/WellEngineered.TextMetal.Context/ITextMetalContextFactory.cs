/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Context
{
	public interface ITextMetalContextFactory : ITextMetalComponent
	{
		#region Methods/Operators

		ITextMetalContext CreateContext();

		#endregion
	}
}