/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

namespace WellEngineered.TextMetal.Model.Database
{
	[Obsolete("Provided for model breaking change compatability only.")]
	public interface ITabular
	{
		#region Properties/Indexers/Events

		IEnumerable<Column> Columns
		{
			get;
		}

		DateTime CreationTimestamp
		{
			get;
		}

		string CSharpIsViewLiteral
		{
			get;
		}

		bool HasAnyMappedColumns
		{
			get;
		}

		bool HasIdentityColumns
		{
			get;
		}

		bool HasNoDefinedPrimaryKeyColumns
		{
			get;
		}

		bool HasSingleColumnServerGeneratedPrimaryKey
		{
			get;
		}

		bool IsImplementationDetail
		{
			get;
		}

		bool IsView
		{
			get;
		}

		DateTime ModificationTimestamp
		{
			get;
		}

		string PrimaryKeyName
		{
			get;
		}

		int TableId
		{
			get;
		}

		string TableName
		{
			get;
		}

		string TableNameCamelCase
		{
			get;
		}

		string TableNameConstantCase
		{
			get;
		}

		string TableNamePascalCase
		{
			get;
		}

		string TableNamePluralCamelCase
		{
			get;
		}

		string TableNamePluralConstantCase
		{
			get;
		}

		string TableNamePluralPascalCase
		{
			get;
		}

		string TableNameSingularCamelCase
		{
			get;
		}

		string TableNameSingularConstantCase
		{
			get;
		}

		string TableNameSingularPascalCase
		{
			get;
		}

		#endregion
	}
}