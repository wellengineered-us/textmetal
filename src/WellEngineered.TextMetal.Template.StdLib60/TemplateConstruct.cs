/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "Template", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Items)]
	public sealed class TemplateConstruct : TemplateXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TemplateConstruct class.
		/// </summary>
		public TemplateConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private bool debug;

		#endregion

		#region Properties/Indexers/Events

		protected override bool IsScopeBlock
		{
			get
			{
				return true;
			}
		}

		[XmlAttributeMapping(LocalName = "debug", NamespaceUri = "")]
		public bool Debug
		{
			get
			{
				return this.debug;
			}
			set
			{
				this.debug = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreExpandTemplate(ITemplatingContext templatingContext)
		{
			DynamicWildcardTokenReplacementStrategy dynamicWildcardTokenReplacementStrategy;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			if (this.Debug)
				templatingContext.LaunchDebugger();

			dynamicWildcardTokenReplacementStrategy = templatingContext.GetDynamicWildcardTokenReplacementStrategy();

			if ((object)this.Items != null)
			{
				foreach (ITemplateMechanism templateMechanism in this.Items)
					templateMechanism.ExpandTemplate(templatingContext);
			}
		}

		#endregion
	}
}