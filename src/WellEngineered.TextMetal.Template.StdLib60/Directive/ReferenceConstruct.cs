/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using WellEngineered.Solder.Extensions;
using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "Reference", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class ReferenceConstruct : TemplateXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ReferenceConstruct class.
		/// </summary>
		public ReferenceConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private string name;

		#endregion

		#region Properties/Indexers/Events

		[XmlAttributeMapping(LocalName = "name", NamespaceUri = "")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreExpandTemplate(ITemplatingContext templatingContext)
		{
			string name;
			Assembly assembly;
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;
			Type[] exportedTypes;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			name = templatingContext.Tokenizer.ExpandTokens(this.Name, dynamicWildcardTokenReplacementStrategy);

			if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(name))
			{
				templatingContext.ClearReferences();
				return;
			}

			assembly = templatingContext.Input.LoadAssembly(name);

			if ((object)assembly == null)
				throw new InvalidOperationException(string.Format("Failed to reference the assembly '{0}'.", name));

			exportedTypes = assembly.GetExportedTypes();

			if ((object)exportedTypes != null)
			{
				foreach (Type exportedType in exportedTypes)
				{
					var _exportedTypeInfo = exportedType.GetTypeInfo();

					if (!_exportedTypeInfo.IsAbstract &&
						typeof(IXmlObject).IsAssignableFrom(exportedType))
					{
						if (typeof(IXmlTextObject).IsAssignableFrom(exportedType))
							templatingContext.SetReference(exportedType);
						else
							templatingContext.AddReference(exportedType);
					}
				}
			}
		}

		#endregion
	}
}