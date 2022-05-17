/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using WellEngineered.Solder.Serialization;
using WellEngineered.Solder.Tokenization;

using __Record = System.Collections.Generic.IDictionary<string, object>;

namespace WellEngineered.TextMetal.Model.File
{
	public class SqlDataModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the SqlDataSourceStrategy class.
		/// </summary>
		public SqlDataModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		private static void WriteSqlQuery(IEnumerable<SqlQuery> sqlQueries, dynamic parent, Type connectionType, string connectionString, bool getSchemaOnly)
		{
			dynamic current;
			IList<dynamic> items;
			Tokenizer tokenizer;

			IEnumerable<IDictionary<string, object>> records;
			string commandText_;
			int count = 0;

			if ((object)sqlQueries == null)
				throw new ArgumentNullException(nameof(sqlQueries));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			if ((object)connectionType == null)
				throw new ArgumentNullException(nameof(connectionType));

			if ((object)connectionString == null)
				throw new ArgumentNullException(nameof(connectionString));

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(connectionString))
				throw new ArgumentOutOfRangeException(nameof(connectionString));

			tokenizer = new Tokenizer(SolderFascadeAccessor.DataTypeFascade, SolderFascadeAccessor.ReflectionFascade,
				new Dictionary<string, ITokenReplacementStrategy>(StringComparer.CurrentCultureIgnoreCase), true);
			
			foreach (SqlQuery sqlQuery in sqlQueries.OrderBy(c => c.Key).ThenBy(c => c.Order))
			{
				items = new List<dynamic>();
				parent[sqlQuery.Key] = items;
				
				commandText_ = tokenizer.ExpandTokens(sqlQuery.Text, new DynamicWildcardTokenReplacementStrategy(SolderFascadeAccessor.DataTypeFascade, SolderFascadeAccessor.ReflectionFascade,new object[] { parent }, tokenizer.StrictMatching));

				// one hell of a polyfill ;)
				Func<CommandType, string, IEnumerable<DbParameter>, Action<long>, IEnumerable<__Record>> executeRecordsCallback =
					(CommandType commandType, string commandText, IEnumerable<DbParameter> dbParameters, Action<long> resultCallback) =>
					{
						const bool _schemaOnly = false;
						Type _connectionType = connectionType;
						string _connectionString = connectionString;
						const bool _transactional = false;
						const IsolationLevel _isolationLevel = IsolationLevel.Unspecified;

						return SolderFascadeAccessor.AdoNetBufferingFascade.ExecuteRecords(_schemaOnly, _connectionType, _connectionString, _transactional, _isolationLevel, commandType, commandText, dbParameters, resultCallback);
					};

				Func<CommandType, string, IEnumerable<DbParameter>, Action<long>, IEnumerable<__Record>> executeSchemaRecordsCallback =
					(CommandType commandType, string commandText, IEnumerable<DbParameter> dbParameters, Action<long> resultCallback) =>
					{
						const bool _schemaOnly = true;
						Type _connectionType = connectionType;
						string _connectionString = connectionString;
						const bool _transactional = false;
						const IsolationLevel _isolationLevel = IsolationLevel.Unspecified;

						return SolderFascadeAccessor.AdoNetBufferingFascade.ExecuteRecords(_schemaOnly, _connectionType, _connectionString, _transactional, _isolationLevel, commandType, commandText, dbParameters, resultCallback);
					};

				Func<string, ParameterDirection, DbType, int, byte, byte, bool, string, object, DbParameter> createParameterCallback =
					(string sourceColumn, ParameterDirection parameterDirection, DbType parameterDbType, int parameterSize, byte parameterPrecision, byte parameterScale, bool parameterNullable, string parameterName, object parameterValue) =>
					{
						Type _connectionType = connectionType;

						return SolderFascadeAccessor.AdoNetBufferingFascade.CreateParameter(_connectionType, sourceColumn, parameterDirection, parameterDbType, parameterSize, parameterPrecision, parameterScale, parameterNullable, parameterName, parameterValue);
					};

				var unitOfWork = new
				{
					ExecuteRecords = executeRecordsCallback,
					ExecuteSchemaRecords = executeSchemaRecordsCallback,
					CreateParameter = createParameterCallback
				};

				// using (null)
				{
					if (getSchemaOnly)
						records = unitOfWork.ExecuteRecords(sqlQuery.Type, commandText_, new DbParameter[] { }, null);
					else
						records = unitOfWork.ExecuteSchemaRecords(sqlQuery.Type, commandText_, new DbParameter[] { }, null);

					records = records.ToArray(); // force eager load
				}

				if ((object)records != null)
				{
					foreach (IDictionary<string, object> record in records)
					{
						current = new Dictionary<string, object>();
						items.Add(current);

						if ((object)record != null)
						{
							foreach (KeyValuePair<string, object> keyValuePair in record)
							{
								current[keyValuePair.Key] = keyValuePair.Value;
							}
						}

						// correlated
						WriteSqlQuery(sqlQuery.SubQueries, current, connectionType, connectionString, getSchemaOnly);

						count++;
					}

					// no longer report row count given impedance mismatch of using dynamic
				}
			}
		}

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			const string PROP_TOKEN_CONNECTION_AQTN = "ConnectionType";
			const string PROP_TOKEN_CONNECTION_STRING = "ConnectionString";
			const string PROP_TOKEN_GET_SCHEMA_ONLY = "GetSchemaOnly";
			string sourceFilePath = null;
			string connectionAqtn;
			Type connectionType = null;
			string connectionString = null;
			bool getSchemaOnly = false;
			IList<string> values;

			dynamic sourceObject;
			SqlQuery sqlQuery;

			ICommonSerializationStrategy commonSerializationStrategy;

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));
			
			if (properties.TryGetValue(PROP_TOKEN_SOURCE_FILE_PATH, out values))
			{
				if ((object)values != null && values.Count == 1)
					sourceFilePath = values[0];
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(sourceFilePath))
				throw new InvalidOperationException(String.Format("The source file path cannot be null or whitespace."));

			sourceFilePath = Path.GetFullPath(sourceFilePath);

			connectionAqtn = null;
			if (properties.TryGetValue(PROP_TOKEN_CONNECTION_AQTN, out values))
			{
				if ((object)values != null && values.Count == 1)
				{
					connectionAqtn = values[0];
					connectionType = Type.GetType(connectionAqtn, false);
				}
			}

			if ((object)connectionType == null)
				throw new InvalidOperationException(string.Format("Failed to load the connection type '{0}' via Type.GetType(..).", connectionAqtn));

			if (!typeof(DbConnection).IsAssignableFrom(connectionType))
				throw new InvalidOperationException(string.Format("The connection type is not assignable to type '{0}'.", typeof(DbConnection).FullName));

			if (properties.TryGetValue(PROP_TOKEN_CONNECTION_STRING, out values))
			{
				if ((object)values != null && values.Count == 1)
					connectionString = values[0];
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(connectionString))
				throw new InvalidOperationException(string.Format("The connection string cannot be null or whitespace."));

			if (properties.TryGetValue(PROP_TOKEN_GET_SCHEMA_ONLY, out values))
			{
				if ((object)values != null && values.Count == 1)
					SolderFascadeAccessor.DataTypeFascade.TryParse<bool>(values[0], out getSchemaOnly);
			}

			commonSerializationStrategy = new NativeXmlSerializationStrategy();
			sqlQuery = commonSerializationStrategy.DeserializeObjectFromFile<SqlQuery>(sourceFilePath);

			sourceObject = new Dictionary<string, object>();

			sourceObject.SourceFullPath = sourceFilePath;
			sourceObject.ConnectionType = connectionType;
			sourceObject.ConnectionString = connectionString;
			sourceObject.GetSchemaOnly = getSchemaOnly;

			WriteSqlQuery(new SqlQuery[] { sqlQuery }, sourceObject, connectionType, connectionString, getSchemaOnly);

			return sourceObject;
		}

		protected override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			return default;
		}

		#endregion
	}
}