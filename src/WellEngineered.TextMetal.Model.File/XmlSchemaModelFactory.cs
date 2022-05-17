/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace WellEngineered.TextMetal.Model.File
{
	public class XmlSchemaModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the XmlSchemaSourceStrategy class.
		/// </summary>
		public XmlSchemaModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		private static void EnumSchema(dynamic parent, XmlSchemaObjectCollection currentXmlSchemaObjectCollection)
		{
			XmlSchemaElement xmlSchemaElement;
			XmlSchemaComplexType xmlSchemaComplexType;
			XmlSchemaSequence xmlSchemaSequence;
			XmlSchemaSimpleType xmlSchemaSimpleType;
			
			dynamic current;
			IList<dynamic> items;
			
			dynamic child;
			IList<dynamic> childItems;

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			if ((object)currentXmlSchemaObjectCollection == null)
				throw new ArgumentNullException(nameof(currentXmlSchemaObjectCollection));

			items = new List<dynamic>();
			parent.XmlSchemaElements = items;
			
			foreach (XmlSchemaObject xmlSchemaObject in currentXmlSchemaObjectCollection)
			{
				current = new Dictionary<string, object>();
				items.Add(current);

				xmlSchemaElement = xmlSchemaObject as XmlSchemaElement;

				if ((object)xmlSchemaElement != null)
				{
					if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(xmlSchemaElement.Name) &&
						!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(xmlSchemaElement.RefName.Name))
					{
						current.XmlSchemaElementIsRef = true;
						current.XmlSchemaElementLocalName = xmlSchemaElement.RefName.Name;
						current.XmlSchemaElementNamespace = xmlSchemaElement.RefName.Namespace;
					}
					else
					{
						current.XmlSchemaElementIsRef = false;
						current.XmlSchemaElementLocalName = xmlSchemaElement.QualifiedName.Name;
						current.XmlSchemaElementNamespace = xmlSchemaElement.QualifiedName.Namespace;
						
						xmlSchemaComplexType = xmlSchemaElement.ElementSchemaType as XmlSchemaComplexType;
						xmlSchemaSimpleType = xmlSchemaElement.ElementSchemaType as XmlSchemaSimpleType;

						if ((object)xmlSchemaSimpleType != null)
						{
							current.XmlSchemaElementSimpleType = xmlSchemaSimpleType.Datatype.TypeCode;
						}
						else if ((object)xmlSchemaComplexType != null)
						{
							childItems = new List<dynamic>();
							current.XmlSchemaAttributes = childItems;

							if ((object)xmlSchemaComplexType.Attributes != null)
							{
								foreach (XmlSchemaAttribute xmlSchemaAttribute in xmlSchemaComplexType.Attributes)
								{
									child = new Dictionary<string, object>();
									childItems.Add(current);

									child.XmlSchemaElementLocalName = xmlSchemaAttribute.QualifiedName.Name;
									child.XmlSchemaElementNamespace = xmlSchemaAttribute.QualifiedName.Namespace;
									child.XmlSchemaElementNamespace = xmlSchemaAttribute.AttributeSchemaType.TypeCode;
								}
							}

							xmlSchemaSequence = xmlSchemaComplexType.ContentTypeParticle as XmlSchemaSequence;

							if ((object)xmlSchemaSequence != null)
								EnumSchema(current, xmlSchemaSequence.Items);
						}
					}
				}
			}
		}

		private static void ValidationCallback(object sender, ValidationEventArgs args)
		{
			Console.WriteLine(args.Message);
		}

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			string sourceFilePath = null;
			IList<string> values;
			object sourceObject;
			
			XmlSchemaSet xmlSchemaSet;
			XmlSchema xmlSchema;
			
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

			using (Stream stream = System.IO.File.Open(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				xmlSchema = XmlSchema.Read(stream, ValidationCallback);

			xmlSchemaSet = new XmlSchemaSet();
			xmlSchemaSet.Add(xmlSchema);
			xmlSchemaSet.Compile();

			xmlSchema = xmlSchemaSet.Schemas().Cast<XmlSchema>().FirstOrDefault();
			
			sourceObject = new Dictionary<string, object>();

			EnumSchema(sourceObject, xmlSchema.Items);

			return sourceObject;
		}

		#endregion
	}
}