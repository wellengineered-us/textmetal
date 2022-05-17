/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Component;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Hosting
{
	public abstract class TextMetalHostFactory : TextMetalComponent, ITextMetalHostFactory
	{
		#region Constructors/Destructors

		protected TextMetalHostFactory()
		{
		}

		#endregion
	}
}