/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Serialization;
using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Model.File
{
	public class XyzlSerializerModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XyzlSerializerModelFactory class.
		/// </summary>
		public XyzlSerializerModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		protected override object CoreGetModelObject(IDictionary<string, IList<string>> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			
			string sourceFilePath = null;
			IList<string> values;
			IConfigurationObject sourceObject;
			
			ICommonSerializationStrategy nativeXmlSerializationStrategy;
			IXyzlSerializer xyzlSerializer;

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
			
			xyzlSerializer = this.CoreGetXyzlSerializer(properties);
			nativeXmlSerializationStrategy = new NativeXyzlSerializationStrategy(xyzlSerializer);
			
			sourceObject = nativeXmlSerializationStrategy.DeserializeObjectFromFile<IConfigurationObject>(sourceFilePath);

			return sourceObject;
		}

		protected virtual IXyzlSerializer CoreGetXyzlSerializer(IDictionary<string, IList<string>> properties)
		{
			const string PROP_TOKEN_KNOWN_XML_OBJECT_AQTN = "KnownXmlObjectType";
			const string PROP_TOKEN_KNOWN_XML_TEXT_OBJECT_AQTN = "KnownXmlTextObjectType";
			IXyzlSerializer xyzlSerializer;
			IList<string> values;
			string configuredObjectAqtn;
			Type configuredObjectType = null;

			if (properties == null)
				throw new ArgumentNullException(nameof(properties));

			xyzlSerializer = new XyzlSerializer(SolderFascadeAccessor.DataTypeFascade, SolderFascadeAccessor.ReflectionFascade);
			configuredObjectAqtn = null;

			if (properties.TryGetValue(PROP_TOKEN_KNOWN_XML_TEXT_OBJECT_AQTN, out values))
			{
				if ((object)values != null && values.Count == 1)
				{
					configuredObjectAqtn = values[0];
					configuredObjectType = Type.GetType(configuredObjectAqtn, false);
				}

				if ((object)configuredObjectType == null)
					throw new InvalidOperationException(string.Format("Failed to load the XML text object type '{0}' via Type.GetType(..).", configuredObjectAqtn));

				if (!typeof(IXyzlValueObject).IsAssignableFrom(configuredObjectType))
					throw new InvalidOperationException(string.Format("The XML text object type is not assignable to type '{0}'.", typeof(IXyzlValueObject).FullName));

				xyzlSerializer.RegisterKnownValueObject(configuredObjectType);
			}

			if (properties.TryGetValue(PROP_TOKEN_KNOWN_XML_OBJECT_AQTN, out values))
			{
				if ((object)values != null)
				{
					foreach (string value in values)
					{
						configuredObjectAqtn = value;
						configuredObjectType = Type.GetType(configuredObjectAqtn, false);

						if ((object)configuredObjectType == null)
							throw new InvalidOperationException(string.Format("Failed to load the XML object type '{0}' via Type.GetType(..).", configuredObjectAqtn));

						if (!typeof(IConfigurationObject).IsAssignableFrom(configuredObjectType))
							throw new InvalidOperationException(string.Format("The XML object type is not assignable to type '{0}'.", typeof(IConfigurationObject).FullName));

						xyzlSerializer.RegisterKnownObject(configuredObjectType);
					}
				}
			}

			return xyzlSerializer;
		}

		#endregion
	}
}