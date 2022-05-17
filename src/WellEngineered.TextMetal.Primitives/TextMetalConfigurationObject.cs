/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using WellEngineered.Solder.Configuration;

namespace WellEngineered.TextMetal.Primitives
{
	public abstract class TextMetalConfigurationObject : SolderConfiguration, ITextMetalConfigurationObject
	{
		protected TextMetalConfigurationObject()
		{
		}
	}
}