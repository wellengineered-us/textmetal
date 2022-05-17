/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;

using WellEngineered.Solder.Executive;
using WellEngineered.Solder.Injection;
using WellEngineered.Solder.Injection.Resolutions;
using WellEngineered.TextMetal.Hosting.Tool;

namespace WellEngineered.TextMetal.Host.Cli
{
	/// <summary>
	/// Entry point static class for the application.
	/// </summary>
	public static class Program
	{
		#region Methods/Operators

		[STAThread]
		public async static Task<int> __Main(string[] args)
		{
			return await ExecutableApplicationFascade.RunAsync<TextMetalConsoleApplication>(args);
		}

		/// <summary>
		/// The entry point method for this application.
		/// </summary>
		/// <param name="args"> The command line arguments passed from the executing environment. </param>
		/// <returns> The resulting exit code. </returns>
		[STAThread]
		public static int Main(string[] args)
		{
			return ExecutableApplicationFascade.Run<TextMetalConsoleApplication>(args);
		}

		[DependencyMagicMethod]
		public static void OnDependencyMagic(IDependencyManager dependencyManager)
		{
			if ((object)dependencyManager == null)
				throw new ArgumentNullException(nameof(dependencyManager));

			dependencyManager.AddResolution<TextMetalConsoleApplication>(string.Empty, false, new SingletonWrapperDependencyResolution<TextMetalConsoleApplication>(TransientActivatorAutoWiringDependencyResolution<TextMetalConsoleApplication>.From(dependencyManager)));
		}

		#endregion
	}
}