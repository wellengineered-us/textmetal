/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Reflection;

using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Model;
using WellEngineered.TextMetal.Output;
using WellEngineered.TextMetal.Primitives;
using WellEngineered.TextMetal.Template;

namespace WellEngineered.TextMetal.Hosting
{
	public abstract class TextMetalHost : TextMetalComponent, ITextMetalHost
	{
		#region Constructors/Destructors

		protected TextMetalHost()
		{
		}

		#endregion

		#region Fields/Constants

		private ITextMetalModelFactory modelFactory;
		private ITextMetalOutputFactory outputFactory;
		private ITextMetalTemplateFactory templateFactory;

		#endregion

		#region Properties/Indexers/Events

		public ITextMetalModelFactory ModelFactory
		{
			get
			{
				return this.modelFactory;
			}
		}

		public ITextMetalOutputFactory OutputFactory
		{
			get
			{
				return this.outputFactory;
			}
		}

		public ITextMetalTemplateFactory TemplateFactory
		{
			get
			{
				return this.templateFactory;
			}
		}

		#endregion

		#region Methods/Operators

		protected abstract ITextMetalContext CoreCreateContext();

		protected abstract bool CoreLaunchDebugger();

		protected abstract Assembly CoreLoadAssembly(string assemblyName);

		public ITextMetalContext CreateContext()
		{
			try
			{
				return this.CoreCreateContext();
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public bool LaunchDebugger()
		{
			try
			{
				return this.CoreLaunchDebugger();
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		public Assembly LoadAssembly(string assemblyName)
		{
			if ((object)assemblyName == null)
				throw new ArgumentNullException(nameof(assemblyName));

			try
			{
				return this.CoreLoadAssembly(assemblyName);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The host failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}