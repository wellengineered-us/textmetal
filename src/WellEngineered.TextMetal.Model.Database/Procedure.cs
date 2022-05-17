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
	public class Procedure
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the Procedure class.
		/// </summary>
		public Procedure()
		{
		}

		#endregion

		#region Fields/Constants

		private readonly List<Parameter> parameters = new List<Parameter>();
		private readonly List<ProcedureResult> results = new List<ProcedureResult>();
		private string procedureExecuteSchemaExceptionText;
		private bool procedureExecuteSchemaThrewException;
		private string procedureName;
		private string procedureNameCamelCase;
		private string procedureNameConstantCase;
		private string procedureNamePascalCase;
		private string procedureNamePluralCamelCase;
		private string procedureNamePluralConstantCase;
		private string procedureNamePluralPascalCase;
		private string procedureNameSingularCamelCase;
		private string procedureNameSingularConstantCase;
		private string procedureNameSingularPascalCase;

		#endregion

		#region Properties/Indexers/Events

		[Obsolete("Provided for model breaking change compatability only.")]
		[XmlIgnore]
		public List<ProcedureColumn> Columns
		{
			get
			{
				ProcedureResult procedureResult;

				procedureResult = this.Results.SingleOrDefault(rs => rs.ResultIndex == 0);

				if ((object)procedureResult == null)
					return null;

				return procedureResult.Columns;
			}
		}

		[Obsolete("Provided for model breaking change compatability only.")]
		[XmlIgnore]
		public bool HasAnyMappedResultColumns
		{
			get
			{
				if ((object)this.Columns == null)
					return false;

				return this.Columns.Any();
			}
		}

		[XmlArray(ElementName = "Parameters")]
		[XmlArrayItem(ElementName = "Parameter")]
		public List<Parameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		[XmlArray(ElementName = "Results")]
		[XmlArrayItem(ElementName = "Result")]
		public List<ProcedureResult> Results
		{
			get
			{
				return this.results;
			}
		}

		[XmlAttribute]
		public string ProcedureExecuteSchemaExceptionText
		{
			get
			{
				return this.procedureExecuteSchemaExceptionText;
			}
			set
			{
				this.procedureExecuteSchemaExceptionText = value;
			}
		}

		[XmlAttribute]
		public bool ProcedureExecuteSchemaThrewException
		{
			get
			{
				return this.procedureExecuteSchemaThrewException;
			}
			set
			{
				this.procedureExecuteSchemaThrewException = value;
			}
		}

		[XmlAttribute]
		public string ProcedureName
		{
			get
			{
				return this.procedureName;
			}
			set
			{
				this.procedureName = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNameCamelCase
		{
			get
			{
				return this.procedureNameCamelCase;
			}
			set
			{
				this.procedureNameCamelCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNameConstantCase
		{
			get
			{
				return this.procedureNameConstantCase;
			}
			set
			{
				this.procedureNameConstantCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNamePascalCase
		{
			get
			{
				return this.procedureNamePascalCase;
			}
			set
			{
				this.procedureNamePascalCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNamePluralCamelCase
		{
			get
			{
				return this.procedureNamePluralCamelCase;
			}
			set
			{
				this.procedureNamePluralCamelCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNamePluralConstantCase
		{
			get
			{
				return this.procedureNamePluralConstantCase;
			}
			set
			{
				this.procedureNamePluralConstantCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNamePluralPascalCase
		{
			get
			{
				return this.procedureNamePluralPascalCase;
			}
			set
			{
				this.procedureNamePluralPascalCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNameSingularCamelCase
		{
			get
			{
				return this.procedureNameSingularCamelCase;
			}
			set
			{
				this.procedureNameSingularCamelCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNameSingularConstantCase
		{
			get
			{
				return this.procedureNameSingularConstantCase;
			}
			set
			{
				this.procedureNameSingularConstantCase = value;
			}
		}

		[XmlAttribute]
		public string ProcedureNameSingularPascalCase
		{
			get
			{
				return this.procedureNameSingularPascalCase;
			}
			set
			{
				this.procedureNameSingularPascalCase = value;
			}
		}

		#endregion
	}
}