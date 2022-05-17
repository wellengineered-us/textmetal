/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using WellEngineered.Solder.Configuration;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Tokenization;
using WellEngineered.Solder.Utilities;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Hosting.Tool.Configuration;
using WellEngineered.TextMetal.Model;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Template;

using __ITextMetalModel = System.Object;

namespace WellEngineered.TextMetal.Hosting.Tool
{
	public sealed class TextMetalToolHost : TextMetalHost, ITextMetalToolHost
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalToolHost class.
		/// </summary>
		[DependencyInjection]
		public TextMetalToolHost([DependencyInjection] IDataTypeFascade dataTypeFascade,
			[DependencyInjection] IReflectionFascade reflectionFascade)
		{
			if ((object)dataTypeFascade == null)
				throw new ArgumentNullException(nameof(dataTypeFascade));

			if ((object)reflectionFascade == null)
				throw new ArgumentNullException(nameof(reflectionFascade));

			this.dataTypeFascade = dataTypeFascade;
			this.reflectionFascade = reflectionFascade;
		}

		#endregion

		#region Fields/Constants

		private readonly IDataTypeFascade dataTypeFascade;
		private readonly IReflectionFascade reflectionFascade;
		private TextMetalToolHostConfiguration configuration;
		private Type configurationType;

		#endregion

		#region Properties/Indexers/Events

		public Type ConfigurationType
		{
			get
			{
				return this.configurationType;
			}
		}

		private IDataTypeFascade DataTypeFascade
		{
			get
			{
				return this.dataTypeFascade;
			}
		}

		private IReflectionFascade ReflectionFascade
		{
			get
			{
				return this.reflectionFascade;
			}
		}

		public TextMetalToolHostConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
			set
			{
				this.configuration = value;
			}
		}

		IConfigurationObject IConfigurable.Configuration
		{
			get
			{
				return this.Configuration;
			}
			set
			{
				this.Configuration = (TextMetalToolHostConfiguration)value;
			}
		}

		#endregion

		#region Methods/Operators

		// TODO use this in library
		private static T NewObjectFromType<T>(Type type)
		{
			Type typeOfT;
			object obj;

			if ((object)type == null)
				throw new ArgumentNullException(nameof(type));

			typeOfT = typeof(T);

			// T = t?
			if (!typeOfT.IsAssignableFrom(type))
				throw new InvalidOperationException(string.Format("Type '{0} is not assignable from type '{1}.", typeOfT, type));

			obj = Activator.CreateInstance(type);

			return (T)obj;
		}

		public static object printf(ITextMetalContext templatingContext, string[] parameters)
		{
			const int CNT_P = 2; // token, format
			object value;

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			if ((object)parameters == null)
				throw new ArgumentNullException(nameof(parameters));

			if (parameters.Length != CNT_P)
				throw new InvalidOperationException(string.Format("printf expects '{1}' parameter(s) but received '{0}' parameter(s).", parameters.Length, CNT_P));

			var x = templatingContext.GetWildcardReplacer();

			if (!x.GetByToken(parameters[0], out value))
				return null;

			return string.Format(value?.ToString() ?? string.Empty, parameters[1]);
			//return value.SafeToString(parameters[1]);
		}

		protected override ITextMetalContext CoreCreateContext()
		{
			ITextMetalContext textMetalContext;
			Tokenizer tokenizer;
			Dictionary<string, ITokenReplacementStrategy> tokenReplacementStrategies;

			tokenReplacementStrategies = new Dictionary<string, ITokenReplacementStrategy>();

			tokenizer = new Tokenizer(this.DataTypeFascade,
				this.ReflectionFascade, tokenReplacementStrategies,
				this.Configuration.StrictMatching.GetValueOrDefault());

			textMetalContext = new TextMetalToolContext(this, this.DataTypeFascade,
				this.ReflectionFascade, tokenizer);

			this.RegisterWellKnownTokenReplacementStrategies(tokenizer, textMetalContext);

			return textMetalContext;
		}

		protected override bool CoreLaunchDebugger()
		{
			return false;
		}

		protected override Assembly CoreLoadAssembly(string assemblyName)
		{
			return null;
		}

		public void Host(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties)
		{
			DateTime startUtc, endUtc;

			ITextMetalTemplate textMetalTemplate;
			__ITextMetalModel textMetalModel;
			ITextMetalOutput textMetalOutput;

			Dictionary<string, object> arguments_;
			Dictionary<string, object> properties_;
			
			Dictionary<string, object> globalVariableTable;

			IAssemblyInformationFascade assemblyInformationFascade;

			if ((object)arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			startUtc = DateTime.UtcNow;

			arguments_ = arguments.ToDictionary(kvp => kvp.Key,
				kvp => kvp.Value.Count == 1 ? kvp.Value[0] : kvp.Value.ToArray());
			
			properties_ = properties.ToDictionary(kvp => kvp.Key,
				kvp => kvp.Value.Count == 1 ? kvp.Value[0] : kvp.Value.ToArray());

			assemblyInformationFascade = new AssemblyInformationFascade(this.ReflectionFascade, typeof(TextMetalToolHost).GetTypeInfo().Assembly);

			Lazy<object> lazyGuid = new Lazy<object>(() => Guid.NewGuid());
			Func<object> getDate = () => DateTime.UtcNow;
			
			var environment = new
							{
								Arguments = Environment.GetCommandLineArgs(),
								Variables = Environment.GetEnvironmentVariables(),
								NewLine = Environment.NewLine,
								Version = Environment.Version,
								MachineName = Environment.MachineName,
								UserName = Environment.UserName,
								OSVersion = Environment.OSVersion,
								UserDomainName = Environment.UserDomainName,
								Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
								Is64BitProcess = Environment.Is64BitProcess,
								ProcessorCount = Environment.ProcessorCount,
								GetDate = getDate,
								LazyGuid = lazyGuid
							};

			var tooling = new
						{
							AssemblyVersion = assemblyInformationFascade.AssemblyVersion,
							InformationalVersion = assemblyInformationFascade.InformationalVersion,
							NativeFileVersion = assemblyInformationFascade.NativeFileVersion,
							ModuleName = assemblyInformationFascade.ModuleName,
							Company = assemblyInformationFascade.Company,
							Configuration = assemblyInformationFascade.Configuration,
							Copyright = assemblyInformationFascade.Copyright,
							Description = assemblyInformationFascade.Description,
							Product = assemblyInformationFascade.Product,
							Title = assemblyInformationFascade.Title,
							Trademark = assemblyInformationFascade.Trademark,
							StartedWhenUtc = startUtc
						};
			
			// globals (GVT)
			globalVariableTable = new Dictionary<string, object>();
			globalVariableTable.Add("Tooling", tooling);
			globalVariableTable.Add("Environment", environment);
			
			globalVariableTable.Add("Properties", properties_);
			globalVariableTable.Add("Arguments", arguments_);

			// add arguments to GVT
			/*foreach (KeyValuePair<string, IList<object>> argument in arguments)
			{
				if (argument.Value.Count == 0)
					continue;

				globalVariableTable.Add(argument.Key,
					argument.Value.Count == 1 ? argument.Value[0] : argument.Value);
			}*/
			
			// add properties to GVT
			/*foreach (KeyValuePair<string, IList<object>> property in properties)
			{
				if (property.Value.Count == 0)
					continue;

				globalVariableTable.Add(property.Key,
					property.Value.Count == 1 ? property.Value[0] : property.Value);
			}*/
			
			// do the deal...
			using (ITextMetalTemplateFactory textMetalTemplateFactory = NewObjectFromType<ITextMetalTemplateFactory>(this.Configuration.GetTemplateFactoryType()))
			{
				//textMetalTemplateFactory.Configuration = ...
				textMetalTemplateFactory.Create();

				textMetalTemplate = textMetalTemplateFactory.GetTemplateObject(new Uri("urn:null"), properties_);

				using (ITextMetalModelFactory textMetalModelFactory = NewObjectFromType<ITextMetalModelFactory>(this.Configuration.GetModelFactoryType()))
				{
					//textMetalModelFactory.Configuration = ...
					textMetalModelFactory.Create();

					textMetalModel = textMetalModelFactory.GetModelObject(properties_);

					using (ITextMetalOutputFactory textMetalOutputFactory = NewObjectFromType<ITextMetalOutputFactory>(this.Configuration.GetOutputFactoryType()))
					{
						//textMetalOutputFactory.Configuration = ...
						textMetalOutputFactory.Create();

						textMetalOutput = textMetalOutputFactory.GetOutputObject(new Uri("urn:null"), properties_);

						//textMetalOutput.TextWriter.WriteLine("xxx");

						using (ITextMetalContext textMetalContext = this.CreateContext())
						{
							//textMetalContext.Configuration = ...
							textMetalContext.Create();

							//textMetalContext.DiagnosticOutput.WriteObject(textMetalTemplate, "#template.xml");
							//textMetalContext.DiagnosticOutput.WriteObject(textMetalModel, "#model.xml");

							textMetalContext.DiagnosticOutput.TextWriter.WriteLine("['{0:O}' (UTC)]\tText templating started.", tooling.StartedWhenUtc);

							textMetalContext.VariableTables.Push(globalVariableTable);
							textMetalContext.IteratorModels.Push(textMetalModel);

							textMetalTemplate.ExpandTemplate(textMetalContext);

							textMetalContext.IteratorModels.Pop();
							textMetalContext.VariableTables.Pop();

							endUtc = DateTime.UtcNow;
							textMetalContext.DiagnosticOutput.TextWriter.WriteLine("['{0:O}' (UTC)]\tText templating completed with duration: '{1}'.", endUtc, (endUtc - startUtc));
						}
					}
				}
			}
		}

		public ValueTask HostAsync(IDictionary<string, IList<object>> arguments,
			IDictionary<string, IList<object>> properties)
		{
			return default;
		}

		private void RegisterWellKnownTokenReplacementStrategies(Tokenizer tokenizer, ITextMetalContext templatingContext)
		{
			if ((object)tokenizer == null)
				throw new ArgumentNullException(nameof(tokenizer));

			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			tokenizer.TokenReplacementStrategies.Add("StaticMethodResolver", new DynamicValueTokenReplacementStrategy(this.DataTypeFascade, this.ReflectionFascade, DynamicValueTokenReplacementStrategy.StaticPropertyResolver));
			//tokenizer.TokenReplacementStrategies.Add("js", new ContextualDynamicValueTokenReplacementStrategy<ITextMetalContext>(JavaScriptConstruct.JavaScriptExpressionResolver, templatingContext));
			tokenizer.TokenReplacementStrategies.Add("printf", new ContextualDynamicValueTokenReplacementStrategy<ITextMetalContext>(printf, templatingContext));
		}

		#endregion
	}
}