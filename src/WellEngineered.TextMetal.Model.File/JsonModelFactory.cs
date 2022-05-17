/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using WellEngineered.Solder.Serialization;
using WellEngineered.Solder.Serialization.JsonNet;

namespace WellEngineered.TextMetal.Model.File
{
	public class JsonModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the JsonSourceStrategy class.
		/// </summary>
		public JsonModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			const string CMDLN_TOKEN_JSON_SERIALIZED_AQTN = "JsonSerializedType";
			string sourceFilePath = null;
			string jsonSerializedObjectAqtn;
			Type jsonSerializedObjectType = null;
			object value;
			object sourceObject;
			
			ICommonSerializationStrategy commonSerializationStrategy;

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			if (properties.TryGetValue(PROP_TOKEN_SOURCE_FILE_PATH, out value))
			{
				if ((object)value != null)
					sourceFilePath = (string)value;
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(sourceFilePath))
				throw new InvalidOperationException(String.Format("The source file path cannot be null or whitespace."));

			sourceFilePath = Path.GetFullPath(sourceFilePath);
			
			jsonSerializedObjectAqtn = null;
			if (properties.TryGetValue(CMDLN_TOKEN_JSON_SERIALIZED_AQTN, out value))
			{
				if ((object)value != null)
				{
					jsonSerializedObjectAqtn = (string)value;
					jsonSerializedObjectType = Type.GetType(jsonSerializedObjectAqtn, false);
				}
			}

			if ((object)jsonSerializedObjectType == null)
				throw new InvalidOperationException(string.Format("Failed to load the JSON type '{0}' via Type.GetType(..).", jsonSerializedObjectAqtn));

			commonSerializationStrategy = new NativeJsonSerializationStrategy();
			sourceObject = commonSerializationStrategy.DeserializeObjectFromFile(sourceFilePath, jsonSerializedObjectType);

			return sourceObject;
		}

		protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			const string CMDLN_TOKEN_JSON_SERIALIZED_AQTN = "JsonSerializedType";
			string sourceFilePath = null;
			string jsonSerializedObjectAqtn;
			Type jsonSerializedObjectType = null;
			object value;
			object sourceObject;
			
			ICommonSerializationStrategy commonSerializationStrategy;

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			if (properties.TryGetValue(PROP_TOKEN_SOURCE_FILE_PATH, out value))
			{
				if ((object)value != null)
					sourceFilePath = (string)value;
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(sourceFilePath))
				throw new InvalidOperationException(String.Format("The source file path cannot be null or whitespace."));

			sourceFilePath = Path.GetFullPath(sourceFilePath);
			
			jsonSerializedObjectAqtn = null;
			if (properties.TryGetValue(CMDLN_TOKEN_JSON_SERIALIZED_AQTN, out value))
			{
				if ((object)value != null)
				{
					jsonSerializedObjectAqtn = (string)value;
					jsonSerializedObjectType = Type.GetType(jsonSerializedObjectAqtn, false);
				}
			}

			if ((object)jsonSerializedObjectType == null)
				throw new InvalidOperationException(string.Format("Failed to load the JSON type '{0}' via Type.GetType(..).", jsonSerializedObjectAqtn));

			commonSerializationStrategy = new NativeJsonSerializationStrategy();
			sourceObject = await commonSerializationStrategy.DeserializeObjectFromFileAsync(sourceFilePath, jsonSerializedObjectType);

			return sourceObject;
		}

		#endregion
	}
}