/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

namespace WellEngineered.TextMetal.Hosting.Email
{
	public interface ITextMetalEmailHost : ITextMetalHost
	{
		#region Methods/Operators

		EmailMessage Host(bool strictMatching, EmailPrototype emailPrototype, object textMetalModel);

		ValueTask<EmailMessage> HostAsync(bool strictMatching, EmailPrototype emailPrototype, object textMetalModel);

		#endregion
	}
}