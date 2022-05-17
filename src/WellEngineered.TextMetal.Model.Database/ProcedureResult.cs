/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WellEngineered.TextMetal.Model.Database
{
	public class ProcedureResult
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ProcedureResult class.
		/// </summary>
		public ProcedureResult()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly List<ProcedureColumn> columns = new List<ProcedureColumn>();
		private int resultIndex;

		#endregion

		#region Properties/Indexers/Events

		[XmlArray(ElementName = "Columns")]
		[XmlArrayItem(ElementName = "Column")]
		public List<ProcedureColumn> Columns
		{
			get
			{
				return this.columns;
			}
		}

		[XmlIgnore]
		public bool HasAnyMappedResultColumns
		{
			get
			{
				return this.Columns.Any();
			}
		}

		[XmlAttribute]
		public int ResultIndex
		{
			get
			{
				return this.resultIndex;
			}
			set
			{
				this.resultIndex = value;
			}
		}

		#endregion
	}
}