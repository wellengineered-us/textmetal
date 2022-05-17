/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;

using WellEngineered.Solder.Primitives;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Model;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Primitives;
using WellEngineered.TextMetal.Template;

namespace WellEngineered.TextMetal.Hosting.Tool.Configuration
{
	public class TextMetalToolHostConfiguration : TextMetalComponentConfigurationObject
	{
		#region Constructors/Destructors

		public TextMetalToolHostConfiguration()
		{
		}

		#endregion

		#region Fields/Constants

		private static readonly Version currentConfigurationVersion = Version.Parse("1.0.0");
		private static readonly Version currentEngineVersion = Version.Parse("0.1.0");
		private static readonly Version minimumConfigurationVersion = Version.Parse("1.0.0");

		private string configurationVersion;
		private string contextAqtn;
		private string hostAqtn;
		private string modelFactoryAqtn;
		private string outputFactoryAqtn;
		private bool? strictMatching;

		private string targetEngineVersion;
		private string templateFactoryAqtn;

		#endregion

		#region Properties/Indexers/Events

		public static Version CurrentConfigurationVersion
		{
			get
			{
				return currentConfigurationVersion;
			}
		}

		public static Version CurrentEngineVersion
		{
			get
			{
				return currentEngineVersion;
			}
		}

		public static Version MinimumConfigurationVersion
		{
			get
			{
				return minimumConfigurationVersion;
			}
		}

		public string ConfigurationVersion
		{
			get
			{
				return this.configurationVersion;
			}
			set
			{
				this.configurationVersion = value;
			}
		}

		public string ContextAqtn
		{
			get
			{
				return this.contextAqtn;
			}
			set
			{
				this.contextAqtn = value;
			}
		}

		public string HostAqtn
		{
			get
			{
				return this.hostAqtn;
			}
			set
			{
				this.hostAqtn = value;
			}
		}

		public string ModelFactoryAqtn
		{
			get
			{
				return this.modelFactoryAqtn;
			}
			set
			{
				this.modelFactoryAqtn = value;
			}
		}

		public string OutputFactoryAqtn
		{
			get
			{
				return this.outputFactoryAqtn;
			}
			set
			{
				this.outputFactoryAqtn = value;
			}
		}

		public bool? StrictMatching
		{
			get
			{
				return this.strictMatching;
			}
			set
			{
				this.strictMatching = value;
			}
		}

		public string TargetEngineVersion
		{
			get
			{
				return this.targetEngineVersion;
			}
			set
			{
				this.targetEngineVersion = value;
			}
		}

		public string TemplateFactoryAqtn
		{
			get
			{
				return this.templateFactoryAqtn;
			}
			set
			{
				this.templateFactoryAqtn = value;
			}
		}

		#endregion

		#region Methods/Operators

		protected override IEnumerable<IMessage> CoreValidate(object context)
		{
			List<Message> messages;

			Type textMetalHostType;
			Type textMetalContextType;
			Type textMetalTemplateFactoryType;
			Type textMetalModelFactoryType;
			Type textMetalOutputFactoryType;

			ITextMetalHost textMetalHost;
			ITextMetalContext textMetalContext;
			ITextMetalTemplateFactory textMetalTemplateFactory;
			ITextMetalModelFactory textMetalModelFactory;
			ITextMetalOutputFactory textMetalOutputFactory;

			messages = new List<Message>();

			if ((object)MinimumConfigurationVersion == null ||
				(object)CurrentConfigurationVersion == null)
				throw new TextMetalException(string.Format("{0} ({1}) and/or {2} ({3}) is null.", nameof(MinimumConfigurationVersion), MinimumConfigurationVersion, nameof(CurrentConfigurationVersion), CurrentConfigurationVersion));

			if (MinimumConfigurationVersion > CurrentConfigurationVersion)
				throw new TextMetalException(string.Format("{0} ({1}) is greater than {2} ({3}).", nameof(MinimumConfigurationVersion), MinimumConfigurationVersion, nameof(CurrentConfigurationVersion), CurrentConfigurationVersion));

			if (string.IsNullOrEmpty(this.ConfigurationVersion) ||
				!Version.TryParse(this.ConfigurationVersion, out Version cv))
				messages.Add(new Message(string.Empty, string.Format("{0} configuration error: missing or invalid property {1}.", context, nameof(this.ConfigurationVersion)), Severity.Error));
			else if ((object)cv == null ||
					cv < MinimumConfigurationVersion ||
					cv > CurrentConfigurationVersion)
				messages.Add(new Message(string.Empty, string.Format("{0} configuration error: value of property {1} ({2}) is out of range [{3} ({4}) , {5} ({6})].", context, nameof(this.ConfigurationVersion), this.ConfigurationVersion,
					nameof(MinimumConfigurationVersion), MinimumConfigurationVersion, nameof(CurrentConfigurationVersion), CurrentConfigurationVersion), Severity.Error));

			if (string.IsNullOrWhiteSpace(this.HostAqtn))
				messages.Add(new Message(string.Empty, string.Format("{0} AQTN is required.", context), Severity.Error));
			else
			{
				textMetalHostType = this.GetHostType(messages);

				if ((object)textMetalHostType == null)
					messages.Add(new Message(string.Empty, string.Format("{0} AQTN failed to load.", context), Severity.Error));
				else if (!typeof(ITextMetalHost).IsAssignableFrom(textMetalHostType))
					messages.Add(new Message(string.Empty, string.Format("{0} AQTN loaded an unrecognized type.", context), Severity.Error));
				else
				{
					// new-ing up via default public constructor should be low friction
					//textMetalHost = (ITextMetalHost)Activator.CreateInstance(textMetalHostType);

					//if ((object)textMetalHost == null)
					//messages.Add(new Message(string.Empty, string.Format("{0} AQTN failed to instantiate type.", context), Severity.Error));
				}
			}

			if (string.IsNullOrWhiteSpace(this.ContextAqtn))
				messages.Add(new Message(string.Empty, string.Format("{0} context AQTN is required.", context), Severity.Error));
			else
			{
				textMetalContextType = this.GetContextType(messages);

				if ((object)textMetalContextType == null)
					messages.Add(new Message(string.Empty, string.Format("{0} context AQTN failed to load.", context), Severity.Error));
				else if (!typeof(ITextMetalContext).IsAssignableFrom(textMetalContextType))
					messages.Add(new Message(string.Empty, string.Format("{0} context AQTN loaded an unrecognized type.", context), Severity.Error));
				else
				{
					// new-ing up via default public constructor should be low friction
					//textMetalContext = (ITextMetalContext)Activator.CreateInstance(textMetalContextType);

					//if ((object)textMetalContext == null)
					//messages.Add(new Message(string.Empty, string.Format("{0} context AQTN failed to instantiate type.", context), Severity.Error));
				}
			}

			if (string.IsNullOrWhiteSpace(this.TemplateFactoryAqtn))
				messages.Add(new Message(string.Empty, string.Format("{0} template factory AQTN is required.", context), Severity.Error));
			else
			{
				textMetalTemplateFactoryType = this.GetTemplateFactoryType(messages);

				if ((object)textMetalTemplateFactoryType == null)
					messages.Add(new Message(string.Empty, string.Format("{0} template factory AQTN failed to load.", context), Severity.Error));
				else if (!typeof(ITextMetalTemplateFactory).IsAssignableFrom(textMetalTemplateFactoryType))
					messages.Add(new Message(string.Empty, string.Format("{0} template factory AQTN loaded an unrecognized type.", context), Severity.Error));
				else
				{
					// new-ing up via default public constructor should be low friction
					textMetalTemplateFactory = (ITextMetalTemplateFactory)Activator.CreateInstance(textMetalTemplateFactoryType);

					if ((object)textMetalTemplateFactory == null)
						messages.Add(new Message(string.Empty, string.Format("{0} template factory AQTN failed to instantiate type.", context), Severity.Error));
				}
			}

			if (string.IsNullOrWhiteSpace(this.ModelFactoryAqtn))
				messages.Add(new Message(string.Empty, string.Format("{0} model factory AQTN is required.", context), Severity.Error));
			else
			{
				textMetalModelFactoryType = this.GetModelFactoryType(messages);

				if ((object)textMetalModelFactoryType == null)
					messages.Add(new Message(string.Empty, string.Format("{0} model factory AQTN failed to load.", context), Severity.Error));
				else if (!typeof(ITextMetalModelFactory).IsAssignableFrom(textMetalModelFactoryType))
					messages.Add(new Message(string.Empty, string.Format("{0} model factory AQTN loaded an unrecognized type.", context), Severity.Error));
				else
				{
					// new-ing up via default public constructor should be low friction
					textMetalModelFactory = (ITextMetalModelFactory)Activator.CreateInstance(textMetalModelFactoryType);

					if ((object)textMetalModelFactory == null)
						messages.Add(new Message(string.Empty, string.Format("{0} model factory AQTN failed to instantiate type.", context), Severity.Error));
				}
			}

			if (string.IsNullOrWhiteSpace(this.OutputFactoryAqtn))
				messages.Add(new Message(string.Empty, string.Format("{0} output factory AQTN is required.", context), Severity.Error));
			else
			{
				textMetalOutputFactoryType = this.GetOutputFactoryType(messages);

				if ((object)textMetalOutputFactoryType == null)
					messages.Add(new Message(string.Empty, string.Format("{0} output factory AQTN failed to load.", context), Severity.Error));
				else if (!typeof(ITextMetalOutputFactory).IsAssignableFrom(textMetalOutputFactoryType))
					messages.Add(new Message(string.Empty, string.Format("{0} output factory AQTN loaded an unrecognized type.", context), Severity.Error));
				else
				{
					// new-ing up via default public constructor should be low friction
					textMetalOutputFactory = (ITextMetalOutputFactory)Activator.CreateInstance(textMetalOutputFactoryType);

					if ((object)textMetalOutputFactory == null)
						messages.Add(new Message(string.Empty, string.Format("{0} output factory AQTN failed to instantiate type.", context), Severity.Error));
				}
			}

			return messages;
		}

		protected override IAsyncEnumerable<IMessage> CoreValidateAsync(object context)
		{
			return null;
		}

		public Type GetContextType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.ContextAqtn, messages);
		}

		public Type GetHostType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.HostAqtn, messages);
		}

		public Type GetModelFactoryType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.ModelFactoryAqtn, messages);
		}

		public Type GetOutputFactoryType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.OutputFactoryAqtn, messages);
		}

		public Type GetTemplateFactoryType(IList<Message> messages = null)
		{
			return GetTypeFromString(this.TemplateFactoryAqtn, messages);
		}

		#endregion
	}
}