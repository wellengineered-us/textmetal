/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using WellEngineered.Solder.Serialization;

namespace WellEngineered.TextMetal.Model.File
{
	public class XmlSerializerModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XmlSerializerSourceStrategy class.
		/// </summary>
		public XmlSerializerModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetModelObject(IDictionary<string, IList<string>> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			const string CMDLN_TOKEN_XML_SERIALIZED_AQTN = "XmlSerializedType";
			string sourceFilePath = null;
			string xmlSerializedObjectAqtn;
			Type xmlSerializedObjectType = null;
			IList<string> values;
			object sourceObject;
			
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

			xmlSerializedObjectAqtn = null;
			if (properties.TryGetValue(CMDLN_TOKEN_XML_SERIALIZED_AQTN, out values))
			{
				if ((object)values != null && values.Count == 1)
				{
					xmlSerializedObjectAqtn = values[0];
					xmlSerializedObjectType = Type.GetType(xmlSerializedObjectAqtn, false);
				}
			}

			if ((object)xmlSerializedObjectType == null)
				throw new InvalidOperationException(string.Format("Failed to load the XML type '{0}' via Type.GetType(..).", xmlSerializedObjectAqtn));

			commonSerializationStrategy = new NativeXmlSerializationStrategy();
			sourceObject = commonSerializationStrategy.DeserializeObjectFromFile(sourceFilePath, xmlSerializedObjectType);

			return sourceObject;
		}

		#endregion
	}
}