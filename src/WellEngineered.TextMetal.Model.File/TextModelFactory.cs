/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WellEngineered.TextMetal.Model.File
{
	public class TextModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextSourceStrategy class.
		/// </summary>
		public TextModelFactory()
		{
		}
		
		#endregion

		#region Methods/Operators

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			const string PROP_TOKEN_FIRST_RECORD_CONTAINS_COLUMN_HEADINGS = "FirstRecordIsHeader";
			const string PROP_TOKEN_HEADER_NAMES = "HeaderName";
			const string PROP_TOKEN_FIELD_DELIMITER = "FieldDelimiter";
			const string PROP_TOKEN_RECORD_DELIMITER = "RecordDelimiter";
			const string PROP_TOKEN_QUOTE_VALUE = "QuoteValue";

			string sourceFilePath = null;
			
			object value;
			bool firstRecordIsHeader;
			string recordDelimiter;
			string fieldDelimiter;
			string quoteValue;
			string[] headerNames;

			dynamic sourceObject;
			IList<dynamic> items;
			dynamic current;

			string line;
			string[] headers = null;

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

			firstRecordIsHeader = false;
			if (properties.TryGetValue(PROP_TOKEN_FIRST_RECORD_CONTAINS_COLUMN_HEADINGS, out value))
			{
				if ((object)value != null)
				{
					if (!SolderFascadeAccessor.DataTypeFascade.TryParse<bool>((string)value, out firstRecordIsHeader))
						firstRecordIsHeader = false;
				}
			}

			headerNames = null;
			if (properties.TryGetValue(PROP_TOKEN_HEADER_NAMES, out value))
			{
				if ((object)value != null)
					headerNames = (string[])value;
			}

			fieldDelimiter = string.Empty;
			if (properties.TryGetValue(PROP_TOKEN_FIELD_DELIMITER, out value))
			{
				if ((object)value != null)
					fieldDelimiter = (string)value;
			}

			recordDelimiter = string.Empty;
			if (properties.TryGetValue(PROP_TOKEN_RECORD_DELIMITER, out value))
			{
				if ((object)value != null)
					recordDelimiter = (string)value;
			}

			quoteValue = string.Empty;
			if (properties.TryGetValue(PROP_TOKEN_QUOTE_VALUE, out value))
			{
				if ((object)value != null)
					quoteValue = (string)value;
			}

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fieldDelimiter))
			{
				fieldDelimiter = fieldDelimiter.Replace("\\\\t", "\t");
				fieldDelimiter = fieldDelimiter.Replace("\\\\r", "\r");
				fieldDelimiter = fieldDelimiter.Replace("\\\\n", "\n");
				fieldDelimiter = fieldDelimiter.Replace("\\\"", "\"");
			}

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(recordDelimiter))
			{
				recordDelimiter = recordDelimiter.Replace("\\\\t", "\t");
				recordDelimiter = recordDelimiter.Replace("\\\\r", "\r");
				recordDelimiter = recordDelimiter.Replace("\\\\n", "\n");
				recordDelimiter = recordDelimiter.Replace("\\\"", "\"");
			}

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(quoteValue))
			{
				quoteValue = quoteValue.Replace("\\\\t", "\t");
				quoteValue = quoteValue.Replace("\\\\r", "\r");
				quoteValue = quoteValue.Replace("\\\\n", "\n");
				quoteValue = quoteValue.Replace("\\\"", "\"");
			}

			sourceObject = new Dictionary<string, object>();
			
			current = new Dictionary<string, object>();
			sourceObject.DelimitedTextSpec = current;

			current.FirstRecordIsHeader = firstRecordIsHeader.ToString();
			current.RecordDelimiter = recordDelimiter;
			current.FieldDelimiter = fieldDelimiter;
			current.QuoteValue = quoteValue;

			using (StreamReader streamReader = System.IO.File.OpenText(sourceFilePath))
			{
				int i = 0;

				items = new List<dynamic>();
				sourceObject.Records = items;
				
				while ((line = (streamReader.ReadLine() ?? string.Empty)).Trim() != string.Empty)
				{
					string[] fields;

					fields = line.Split(fieldDelimiter.ToCharArray());

					if (firstRecordIsHeader && i == 0)
					{
						headers = fields;
						i++;
						continue;
					}

					current = new Dictionary<string, object>();
					items.Add(current);

					if ((object)fields != null)
					{
						int j = 0;

						foreach (string field in fields)
						{
							string propertyName;
							if (firstRecordIsHeader && (object)headers != null)
								propertyName = string.Format("{0}", headers[j++]);
							else
								propertyName = string.Format("Field{0}", (j++) + 1);

							current[propertyName] = field;
						}
					}

					i++;
				}
			}

			return sourceObject;
		}

		protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_PATH = "SourceFilePath";
			const string PROP_TOKEN_FIRST_RECORD_CONTAINS_COLUMN_HEADINGS = "FirstRecordIsHeader";
			const string PROP_TOKEN_HEADER_NAMES = "HeaderName";
			const string PROP_TOKEN_FIELD_DELIMITER = "FieldDelimiter";
			const string PROP_TOKEN_RECORD_DELIMITER = "RecordDelimiter";
			const string PROP_TOKEN_QUOTE_VALUE = "QuoteValue";

			string sourceFilePath = null;
			
			object value;
			bool firstRecordIsHeader;
			string recordDelimiter;
			string fieldDelimiter;
			string quoteValue;
			string[] headerNames;

			dynamic sourceObject;
			IList<dynamic> items;
			dynamic current;

			string line;
			string[] headers = null;

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

			firstRecordIsHeader = false;
			if (properties.TryGetValue(PROP_TOKEN_FIRST_RECORD_CONTAINS_COLUMN_HEADINGS, out value))
			{
				if ((object)value != null)
				{
					if (!SolderFascadeAccessor.DataTypeFascade.TryParse<bool>((string)value, out firstRecordIsHeader))
						firstRecordIsHeader = false;
				}
			}

			headerNames = null;
			if (properties.TryGetValue(PROP_TOKEN_HEADER_NAMES, out value))
			{
				if ((object)value != null)
					headerNames = (string[])value;
			}

			fieldDelimiter = string.Empty;
			if (properties.TryGetValue(PROP_TOKEN_FIELD_DELIMITER, out value))
			{
				if ((object)value != null)
					fieldDelimiter = (string)value;
			}

			recordDelimiter = string.Empty;
			if (properties.TryGetValue(PROP_TOKEN_RECORD_DELIMITER, out value))
			{
				if ((object)value != null)
					recordDelimiter = (string)value;
			}

			quoteValue = string.Empty;
			if (properties.TryGetValue(PROP_TOKEN_QUOTE_VALUE, out value))
			{
				if ((object)value != null)
					quoteValue = (string)value;
			}

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(fieldDelimiter))
			{
				fieldDelimiter = fieldDelimiter.Replace("\\\\t", "\t");
				fieldDelimiter = fieldDelimiter.Replace("\\\\r", "\r");
				fieldDelimiter = fieldDelimiter.Replace("\\\\n", "\n");
				fieldDelimiter = fieldDelimiter.Replace("\\\"", "\"");
			}

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(recordDelimiter))
			{
				recordDelimiter = recordDelimiter.Replace("\\\\t", "\t");
				recordDelimiter = recordDelimiter.Replace("\\\\r", "\r");
				recordDelimiter = recordDelimiter.Replace("\\\\n", "\n");
				recordDelimiter = recordDelimiter.Replace("\\\"", "\"");
			}

			if (!SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(quoteValue))
			{
				quoteValue = quoteValue.Replace("\\\\t", "\t");
				quoteValue = quoteValue.Replace("\\\\r", "\r");
				quoteValue = quoteValue.Replace("\\\\n", "\n");
				quoteValue = quoteValue.Replace("\\\"", "\"");
			}

			sourceObject = new Dictionary<string, object>();
			
			current = new Dictionary<string, object>();
			sourceObject.DelimitedTextSpec = current;

			current.FirstRecordIsHeader = firstRecordIsHeader.ToString();
			current.RecordDelimiter = recordDelimiter;
			current.FieldDelimiter = fieldDelimiter;
			current.QuoteValue = quoteValue;

			using (StreamReader streamReader = System.IO.File.OpenText(sourceFilePath))
			{
				int i = 0;

				items = new List<dynamic>();
				sourceObject.Records = items;
				
				// TODO: IAsyncEnumerable here
				
				while ((line = (await streamReader.ReadLineAsync() ?? string.Empty)).Trim() != string.Empty)
				{
					string[] fields;

					fields = line.Split(fieldDelimiter.ToCharArray());

					if (firstRecordIsHeader && i == 0)
					{
						headers = fields;
						i++;
						continue;
					}

					current = new Dictionary<string, object>();
					items.Add(current);

					if ((object)fields != null)
					{
						int j = 0;

						foreach (string field in fields)
						{
							string propertyName;
							if (firstRecordIsHeader && (object)headers != null)
								propertyName = string.Format("{0}", headers[j++]);
							else
								propertyName = string.Format("Field{0}", (j++) + 1);

							current[propertyName] = field;
						}
					}

					i++;
				}
			}

			return sourceObject;
		}

		#endregion
	}
}