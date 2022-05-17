/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.Solder.Primitives;
using WellEngineered.Solder.Serialization.JsonNet;
using WellEngineered.Solder.Utilities;
using WellEngineered.TextMetal.Hosting.Tool.Configuration;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	/// <summary>
	/// Entry point class for the application.
	/// </summary>
	public class TextMetalConsoleApplication : ExecutableApplicationFascade, ITextMetalToolHostFactory
	{
		#region Constructors/Destructors

		[DependencyInjection]
		public TextMetalConsoleApplication([DependencyInjection] IDataTypeFascade dataTypeFascade,
			[DependencyInjection] IAppConfigFascade appConfigFascade,
			[DependencyInjection] IReflectionFascade reflectionFascade,
			[DependencyInjection] IAssemblyInformationFascade assemblyInformationFascade)
			: base(dataTypeFascade, appConfigFascade, reflectionFascade, assemblyInformationFascade)
		{
		}

		#endregion

		#region Fields/Constants

		private const string ARG_PROPERTY_ITEM = "property";

		#endregion

		#region Properties/Indexers/Events

		public Guid ComponentId
		{
			get
			{
				return default;
			}
		}

		public bool IsReusable
		{
			get
			{
				return default;
			}
		}

		#endregion

		#region Methods/Operators

		// TODO use this in library
		private static T NewAutoWiredObjectFromType<T>(Type type)
		{
			Type typeOfT;
			object obj;

			if ((object)type == null)
				throw new ArgumentNullException(nameof(type));

			typeOfT = typeof(T);

			// T = t?
			if (!typeOfT.IsAssignableFrom(type))
				throw new InvalidOperationException(string.Format("Type '{0} is not assignable from type '{1}.", typeOfT, type));

			obj = TransientActivatorAutoWiringDependencyResolution
				.From(AssemblyDomain.Default.DependencyManager, type)
				.Resolve(AssemblyDomain.Default.DependencyManager, typeof(void), string.Empty);
			;

			return (T)obj;
		}

		[DependencyMagicMethod]
		public static void OnDependencyMagic(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			//dependencyManager.AddResolution<ExecutableApplicationFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<ExecutableApplicationFascade>(TransientActivatorAutoWiringDependencyResolution<TextMetalConsoleApplication>.From(dependencyManager)));
			//dependencyManager.AddResolution<ITextMetalToolHost>(string.Empty, false, new SingletonWrapperDependencyResolution<ITextMetalToolHost>(TransientActivatorAutoWiringDependencyResolution<TextMetalToolHost>.From(dependencyManager)));
			dependencyManager.AddResolution<IAdoNetBufferingFascade>(string.Empty, false, new SingletonWrapperDependencyResolution<IAdoNetBufferingFascade>(TransientActivatorAutoWiringDependencyResolution<AdoNetBufferingFascade>.From(dependencyManager)));
		}

		public ITextMetalToolHost CreateToolHost(TextMetalToolHostConfiguration textMetalToolHostConfiguration)
		{
			Type textMetalToolHostType;
			ITextMetalToolHost textMetalToolHost;

			if ((object)textMetalToolHostConfiguration == null)
				throw new ArgumentNullException(nameof(textMetalToolHostConfiguration));

			textMetalToolHostType = textMetalToolHostConfiguration.GetHostType();

			if ((object)textMetalToolHostType == null)
				throw new TextMetalException(string.Format("Failed to load host type from AQTN: '{0}'.", textMetalToolHostConfiguration.HostAqtn));

			textMetalToolHost = NewAutoWiredObjectFromType<ITextMetalToolHost>(textMetalToolHostType);

			if ((object)textMetalToolHost == null)
				throw new TextMetalException(string.Format("Failed to instantiate host type: '{0}'.", textMetalToolHostConfiguration.HostAqtn));

			return textMetalToolHost;
		}

		protected override IDictionary<string, ArgumentSpec> GetArgumentMap()
		{
			IDictionary<string, ArgumentSpec> argumentMap;

			argumentMap = new Dictionary<string, ArgumentSpec>();
			argumentMap.Add(ARG_PROPERTY_ITEM, new ArgumentSpec<string>(false, false));

			return argumentMap;
		}

		private TextMetalToolHostConfiguration GetToolHostConfiguration(string toolHostConfigPath)
		{
			TextMetalToolHostConfiguration textMetalToolHostConfiguration;
			string toolHostConfigUrl;
			Uri toolHostConfigUri;
			IEnumerable<IMessage> messages;

			toolHostConfigPath = Path.GetFullPath(toolHostConfigPath);
			toolHostConfigUrl = string.Format("{0}:///{1}", Uri.UriSchemeFile, toolHostConfigPath);
			toolHostConfigUri = new Uri(toolHostConfigUrl);

			if (!toolHostConfigUri.IsFile)
				throw new TextMetalException(string.Format("Failed to load host configuration from URI: '{0}'.", toolHostConfigUri));

			var json = new NativeJsonSerializationStrategy();
			textMetalToolHostConfiguration = json.DeserializeObjectFromFile<TextMetalToolHostConfiguration>(toolHostConfigUri.LocalPath);

			if ((object)textMetalToolHostConfiguration == null)
				throw new TextMetalException(string.Format("Failed to load host configuration from URI: '{0}'.", toolHostConfigUri));

			messages = textMetalToolHostConfiguration.Validate("Host");

			if ((object)messages != null)
			{
				int count = 0;
				foreach (IMessage message in messages)
				{
					if (message == null)
						continue;

					Console.Out.WriteLine(string.Format("{0}[{1}] => {2}", message.Severity, (count + 1), message.Description));

					count++;
				}

				if (count > 0)
					throw new TextMetalException(string.Format("Host configuration validation failed with error count: {0}", count));
			}

			return textMetalToolHostConfiguration;
		}

		private async ValueTask<TextMetalToolHostConfiguration> GetToolHostConfigurationAsync(string toolHostConfigPath)
		{
			TextMetalToolHostConfiguration textMetalToolHostConfiguration;
			string toolHostConfigUrl;
			Uri toolHostConfigUri;
			IAsyncEnumerable<IMessage> messages;

			toolHostConfigPath = Path.GetFullPath(toolHostConfigPath);
			toolHostConfigUrl = string.Format("{0}:///{1}", Uri.UriSchemeFile, toolHostConfigPath);
			toolHostConfigUri = new Uri(toolHostConfigUrl);

			if (!toolHostConfigUri.IsFile)
				throw new TextMetalException(string.Format("Failed to load host configuration from URI: '{0}'.", toolHostConfigUri));

			var json = new NativeJsonSerializationStrategy();
			textMetalToolHostConfiguration = await json.DeserializeObjectFromFileAsync<TextMetalToolHostConfiguration>(toolHostConfigUri.LocalPath);

			if ((object)textMetalToolHostConfiguration == null)
				throw new TextMetalException(string.Format("Failed to load host configuration from URI: '{0}'.", toolHostConfigUri));

			messages = textMetalToolHostConfiguration.ValidateAsync("Host");

			if ((object)messages != null)
			{
				int count = 0;
				await foreach (IMessage message in messages)
				{
					if (message == null)
						continue;

					await Console.Out.WriteLineAsync(string.Format("{0}[{1}] => {2}", message.Severity, (count + 1), message.Description));

					count++;
				}

				if (count > 0)
					throw new TextMetalException(string.Format("Host configuration validation failed with error count: {0}", count));
			}

			return textMetalToolHostConfiguration;
		}

		protected override int OnStartup(string[] args, IDictionary<string, IList<object>> arguments)
		{
			IList<object> argumentValues;

			IDictionary<string, IList<object>> properties;
			object propertyValue;
			IList<object> propertyValues;

			bool hasProperties;

			if ((object)args == null)
				throw new ArgumentNullException(nameof(args));

			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			// required
			properties = new Dictionary<string, IList<object>>();
			hasProperties = arguments.TryGetValue(ARG_PROPERTY_ITEM, out argumentValues);

			if (hasProperties)
			{
				if ((object)argumentValues != null)
				{
					foreach (string argumentValue in argumentValues)
					{
						string key, value;

						if (!this.TryParseCommandLineArgumentProperty(argumentValue, out key, out value))
							continue;

						if (!properties.TryGetValue(key, out propertyValues))
							properties.Add(key, propertyValues = new List<object>());

						// duplicate values are ignored
						//if (propertyValues.Contains(value))
						//continue;

						propertyValues.Add(value);
					}
				}
			}

			//+++

			string fileName = "appconfig.json";
			TextMetalToolHostConfiguration textMetalToolHostConfiguration;

			textMetalToolHostConfiguration = this.GetToolHostConfiguration(fileName);

			using (ITextMetalToolHost textMetalToolHost = this.CreateToolHost(textMetalToolHostConfiguration))
			{
				textMetalToolHost.Configuration = textMetalToolHostConfiguration;
				textMetalToolHost.Create();

				textMetalToolHost.Host(arguments, properties);
			}

			return 0;
		}

		protected async override ValueTask<int> OnStartupAsync(string[] args, IDictionary<string, IList<object>> arguments)
		{
			bool? strictMatching = true;
			bool? debuggerLaunch = false;

			string hostAqtn = null;
			string contextAqtn = null;
			string templateFactoryAqtn = null;
			string modelFactoryAqtn = null;
			string outputFactoryAqtn = null;

			IList<object> argumentValues;

			IDictionary<string, IList<object>> properties;
			object propertyValue;
			IList<object> propertyValues;

			bool hasProperties;

			if ((object)args == null)
				throw new ArgumentNullException(nameof(args));

			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			// required
			properties = new Dictionary<string, IList<object>>();
			hasProperties = arguments.TryGetValue(ARG_PROPERTY_ITEM, out argumentValues);

			if (hasProperties)
			{
				if ((object)argumentValues != null)
				{
					foreach (string argumentValue in argumentValues)
					{
						string key, value;

						if (!this.TryParseCommandLineArgumentProperty(argumentValue, out key, out value))
							continue;

						if (!properties.TryGetValue(key, out propertyValues))
							properties.Add(key, propertyValues = new List<object>());

						// duplicate values are ignored
						//if (propertyValues.Contains(value))
						//continue;

						propertyValues.Add(value);
					}
				}
			}

			//+++

			string fileName = "appconfig.json";
			TextMetalToolHostConfiguration textMetalToolHostConfiguration;

			textMetalToolHostConfiguration = await this.GetToolHostConfigurationAsync(fileName);

			await using (ITextMetalToolHost textMetalToolHost = this.CreateToolHost(textMetalToolHostConfiguration))
			{
				textMetalToolHost.Configuration = textMetalToolHostConfiguration;
				await textMetalToolHost.CreateAsync();

				await textMetalToolHost.HostAsync(arguments, properties);
			}

			return 0;
		}

		#endregion
	}
}