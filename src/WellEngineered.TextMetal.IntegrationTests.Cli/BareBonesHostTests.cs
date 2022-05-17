/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using WellEngineered.Solder.Extensions;
using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Hosting;
using WellEngineered.TextMetal.Model;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Template;

using __ITextMetalModel = System.Object;

namespace WellEngineered.TextMetal.IntegrationTests.Cli
{
	[TestFixture]
	public class BareBonesHostTests
	{
		#region Constructors/Destructors

		public BareBonesHostTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			using (StubHost textMetalHost = new StubHost())
			{
				textMetalHost.Create();
				textMetalHost.Host();
			}
		}

		[Test]
		public async Task ShouldCreateTestAsync()
		{
			await using (StubHost textMetalHost = new StubHost())
			{
				await textMetalHost.CreateAsync();
				await textMetalHost.HostAsync();
			}
		}
		
		#endregion

		#region Classes/Structs/Interfaces/Enums/Delegates

		public class StubContext : TextMetalContext
		{
			#region Constructors/Destructors

			public StubContext(ITextMetalHost host, Tokenizer tokenizer)
				: base(SolderFascadeAccessor.DataTypeFascade,
					SolderFascadeAccessor.ReflectionFascade, tokenizer)
			{
			}

			#endregion

			#region Methods/Operators

			protected override void CoreAddTemplatingReference(Type templateObjectType)
			{
			}

			protected override void CoreAddTemplatingReference(IXyzlName xmlName, Type templateObjectType)
			{
			}

			protected override void CoreClearTemplatingReferences()
			{
			}

			protected override ITextMetalOutput CoreGetDefaultOutput()
			{
				return StubOutput.Default;
			}

			protected override Type CoreGetTemplatingReference()
			{
				return null;
			}

			protected override DynamicWildcardTokenReplacementStrategy CoreGetWildcardReplacer(bool strict)
			{
				return this.__GetWildcardReplacer(strict);
			}

			protected override IDictionary<IXyzlName, Type> CoreListTemplatingReferences()
			{
				return null;
			}

			protected override void CoreSetTemplatingReference(Type templateObjectType)
			{
			}

			#endregion
		}

		public class StubHost : TextMetalHost
		{
			#region Methods/Operators

			protected override ITextMetalContext CoreCreateContext()
			{
				const bool STRICT_MATCHING = true;
				Dictionary<string, ITokenReplacementStrategy> tokenReplacementStrategies;

				tokenReplacementStrategies = new Dictionary<string, ITokenReplacementStrategy>();

				return new StubContext(this, new Tokenizer(SolderFascadeAccessor.DataTypeFascade,
					SolderFascadeAccessor.ReflectionFascade, tokenReplacementStrategies, STRICT_MATCHING));
			}

			protected override bool CoreLaunchDebugger()
			{
				return false;
			}

			protected override Assembly CoreLoadAssembly(string assemblyName)
			{
				return null;
			}

			public void Host()
			{
				ITextMetalTemplate textMetalTemplate;
				__ITextMetalModel textMetalModel;
				ITextMetalOutput textMetalOutput;

				Dictionary<string, object> properties;

				properties = new Dictionary<string, object>();

				using (ITextMetalTemplateFactory textMetalTemplateFactory = new StubTemplateFactory())
				{
					//textMetalTemplateFactory.Configuration = ...
					textMetalTemplateFactory.Create();

					textMetalTemplate = textMetalTemplateFactory.GetTemplateObject(new Uri("urn:null"), properties);

					using (ITextMetalModelFactory textMetalModelFactory = new StubModelFactory())
					{
						//textMetalModelFactory.Configuration = ...
						textMetalModelFactory.Create();

						textMetalModel = textMetalModelFactory.GetModelObject(properties);

						using (ITextMetalOutputFactory textMetalOutputFactory = new StubTextMetalOutputFactory())
						{
							//textMetalOutputFactory.Configuration = ...
							textMetalOutputFactory.Create();

							textMetalOutput = textMetalOutputFactory.GetOutputObject(new Uri("urn:null"), properties);

							//textMetalOutput.TextWriter.WriteLine("xxx");

							using (ITextMetalContext textMetalContext = this.CreateContext())
							{
								textMetalContext.IteratorModels.Push(textMetalModel);
								textMetalTemplate.ExpandTemplate(textMetalContext);
								textMetalContext.IteratorModels.Pop();
							}
						}
					}
				}
			}
			
			public async ValueTask HostAsync()
			{
				ITextMetalTemplate textMetalTemplate;
				__ITextMetalModel textMetalModel;
				ITextMetalOutput textMetalOutput;

				Dictionary<string, object> properties;

				properties = new Dictionary<string, object>();

				await using (ITextMetalTemplateFactory textMetalTemplateFactory = new StubTemplateFactory())
				{
					//textMetalTemplateFactory.Configuration = ...
					await textMetalTemplateFactory.CreateAsync();

					textMetalTemplate = await textMetalTemplateFactory.GetTemplateObjectAsync(new Uri("urn:null"), properties);

					await using (ITextMetalModelFactory textMetalModelFactory = new StubModelFactory())
					{
						//textMetalModelFactory.Configuration = ...
						await textMetalModelFactory.CreateAsync();

						textMetalModel = await textMetalModelFactory.GetModelObjectAsync(properties);

						await using (ITextMetalOutputFactory textMetalOutputFactory = new StubTextMetalOutputFactory())
						{
							//textMetalOutputFactory.Configuration = ...
							await textMetalOutputFactory.CreateAsync();

							textMetalOutput = await textMetalOutputFactory.GetOutputObjectAsync(new Uri("urn:null"), properties);

							//textMetalOutput.TextWriter.WriteLine("xxx");

							await using (ITextMetalContext textMetalContext = this.CreateContext())
							{
								textMetalContext.IteratorModels.Push(textMetalModel);
								await textMetalTemplate.ExpandTemplateAsync(textMetalContext);
								textMetalContext.IteratorModels.Pop();
							}
						}
					}
				}
			}

			#endregion

			#region Classes/Structs/Interfaces/Enums/Delegates

			public class StubModelFactory : TextMetalModelFactory
			{
				#region Methods/Operators

				protected override object CoreGetModelObject(IDictionary<string, object> properties)
				{
					return new { yo = Guid.NewGuid().ToString() };
				}

				protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
				{
					await Task.CompletedTask;
					return new { yo = Guid.NewGuid().ToString() };
				}

				#endregion
			}

			public class StubTemplate : TextMetalTemplateObject
			{
				#region Methods/Operators

				protected override void CoreExpandTemplate(ITextMetalContext templatingContext)
				{
					//templatingContext.RequestDebugger();

					templatingContext.GetWildcardReplacer().GetByToken("yo", out object value);
					templatingContext.CurrentOutput.TextWriter.WriteLine("test: yo={0}", value);
					templatingContext.CurrentOutput.EnterScope("", false, Encoding.Default);
				}

				protected override async ValueTask CoreExpandTemplateAsync(ITextMetalContext templatingContext)
				{
					templatingContext.GetWildcardReplacer().GetByToken("yo", out object value);
					await templatingContext.CurrentOutput.TextWriter.WriteLineAsync(string.Format("test: yo={0}", value));
					await templatingContext.CurrentOutput.EnterScopeAsync("", false, Encoding.Default);
				}

				#endregion
			}

			public class StubTemplateFactory : TextMetalTemplateFactory
			{
				#region Methods/Operators

				protected override ITextMetalTemplate CoreGetTemplateObject(Uri templateUri, IDictionary<string, object> properties)
				{
					return new StubTemplate();
				}

				protected override string CoreLoadTemplateContent(Uri contentUri)
				{
					return null;
				}

				protected async override ValueTask<ITextMetalTemplate> CoreGetTemplateObjectAsync(Uri templateUri, IDictionary<string, object> properties)
				{
					await Task.CompletedTask;
					return new StubTemplate();
				}

				protected override ValueTask<string> CoreLoadTemplateContentAsync(Uri contentUri)
				{
					return default;
				}

				#endregion
			}

			#endregion
		}

		public class StubOutput : TextMetalOutput
		{
			#region Constructors/Destructors

			public StubOutput(TextWriter textWriter, IDictionary<string, object> properties)
				: base(textWriter, properties)
			{
			}

			#endregion

			#region Fields/Constants

			private static ITextMetalOutput @default = new StubOutput(TestContext.Out, new Dictionary<string, object>());

			#endregion

			#region Properties/Indexers/Events

			public static ITextMetalOutput Default
			{
				get
				{
					return @default;
				}
			}

			#endregion

			#region Methods/Operators

			protected override void CoreEnterScope(string scopeName, bool appendMode, Encoding encoding)
			{
			}

			protected override void CoreLeaveScope(string scopeName)
			{
			}

			protected override void CoreWriteObject(object obj, Uri objectUri)
			{
			}

			protected override ValueTask CoreEnterScopeAsync(string scopeName, bool appendMode, Encoding encoding)
			{
				return default;
			}

			protected override ValueTask CoreLeaveScopeAsync(string scopeName)
			{
				return default;
			}

			protected override ValueTask CoreWriteObjectAsync(object obj, Uri objectUri)
			{
				return default;
			}

			#endregion
		}

		public class StubTextMetalOutputFactory : TextMetalOutputFactory
		{
			#region Methods/Operators

			protected override ITextMetalOutput CoreGetOutputObject(Uri baseUri, IDictionary<string, object> properties)
			{
				return new StubOutput(TestContext.Out, properties);
			}

			protected async override ValueTask<ITextMetalOutput> CoreGetOutputObjectAsync(Uri baseUri, IDictionary<string, object> properties)
			{
				await Task.CompletedTask;
				return new StubOutput(TestContext.Out, properties);
			}

			#endregion
		}

		#endregion
	}
}