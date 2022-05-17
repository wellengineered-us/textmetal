/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;

namespace WellEngineered.TextMetal.Template.Sort
{
	public abstract class SortXmlObject : XmlObject, ISortXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SortXmlObject class.
		/// </summary>
		protected SortXmlObject()
		{
		}

		#endregion

		#region Properties/Indexers/Events

		public abstract bool? SortDirection
		{
			get;
		}

		#endregion

		#region Methods/Operators

		protected abstract IEnumerable CoreEvaluateSort(ITemplatingContext templatingContext, IEnumerable values);

		public IEnumerable EvaluateSort(ITemplatingContext templatingContext, IEnumerable values)
		{
			return this.CoreEvaluateSort(templatingContext, values);
		}

		#endregion
	}
}