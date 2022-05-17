/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WellEngineered.TextMetal.Primitives;

using __ITextMetalModel = System.Object;

namespace WellEngineered.TextMetal.Model
{
	/// <summary>
	/// Provides a factory pattern around acquiring model objects.
	/// </summary>
	public interface ITextMetalModelFactory : ITextMetalComponent
	{
		#region Methods/Operators

		/// <summary>
		/// Gets the source object.
		/// </summary>
		/// <param name="properties"> A list of arbitrary properties (key/value pairs). </param>
		/// <returns> An source object instance or null. </returns>
		__ITextMetalModel GetModelObject(IDictionary<string, object> properties);

		ValueTask<__ITextMetalModel> GetModelObjectAsync(IDictionary<string, object> properties);

		#endregion
	}
}