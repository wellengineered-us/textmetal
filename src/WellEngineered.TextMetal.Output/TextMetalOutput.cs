/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WellEngineered.Solder.Component;
using WellEngineered.TextMetal.Primitives;

namespace WellEngineered.TextMetal.Output
{
	public abstract class TextMetalOutput : TextMetalComponent, ITextMetalOutput
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the TextMetalOutput class.
		/// </summary>
		protected TextMetalOutput(TextWriter textWriter, IDictionary<string, object> properties)
		{
			if ((object)textWriter == null)
				throw new ArgumentNullException(nameof(textWriter));

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			this.textWriter = textWriter;
			this.properties = properties;
		}

		#endregion

		#region Fields/Constants

		private readonly IDictionary<string, object> properties;

		private readonly TextWriter textWriter;

		#endregion

		#region Properties/Indexers/Events

		public IDictionary<string, object> Properties
		{
			get
			{
				return this.properties;
			}
		}

		public TextWriter TextWriter
		{
			get
			{
				return this.textWriter;
			}
		}

		#endregion

		#region Methods/Operators

		protected override void CoreCreate(bool creating)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				base.CoreCreate(creating);
				// do nothing
			}
		}

		protected async override ValueTask CoreCreateAsync(bool creating, CancellationToken cancellationToken = default)
		{
			if (this.IsCreated)
				return;

			if (creating)
			{
				await base.CoreCreateAsync(creating, cancellationToken);
				// do nothing
			}
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				if ((object)this.TextWriter != null)
				{
					this.TextWriter.Flush();
					this.TextWriter.Dispose();
				}

				if ((object)this.Properties != null)
				{
					this.Properties.Clear();
				}

				base.CoreDispose(disposing);
			}
		}

		protected async override ValueTask CoreDisposeAsync(bool disposing, CancellationToken cancellationToken = default)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				if ((object)this.TextWriter != null)
				{
					await this.TextWriter.FlushAsync();
					await this.TextWriter.DisposeAsync();
				}

				if ((object)this.Properties != null)
				{
					this.Properties.Clear();
				}

				await base.CoreDisposeAsync(disposing, cancellationToken);
			}
		}

		protected abstract void CoreEnterScope(string scopeName, bool appendMode, Encoding encoding);

		protected abstract ValueTask CoreEnterScopeAsync(string scopeName, bool appendMode, Encoding encoding);

		protected abstract void CoreLeaveScope(string scopeName);

		protected abstract ValueTask CoreLeaveScopeAsync(string scopeName);

		protected abstract void CoreWriteObject(object obj, Uri objectUri);

		protected abstract ValueTask CoreWriteObjectAsync(object obj, Uri objectUri);

		public void EnterScope(string scopeName, bool appendMode, Encoding encoding)
		{
			if ((object)scopeName == null)
				throw new ArgumentNullException(nameof(scopeName));

			if ((object)encoding == null)
				throw new ArgumentNullException(nameof(encoding));

			try
			{
				this.CoreEnterScope(scopeName, appendMode, encoding);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output failed (see inner exception)."), ex);
			}
		}

		public async ValueTask EnterScopeAsync(string scopeName, bool appendMode, Encoding encoding)
		{
			if ((object)scopeName == null)
				throw new ArgumentNullException(nameof(scopeName));

			if ((object)encoding == null)
				throw new ArgumentNullException(nameof(encoding));

			try
			{
				await this.CoreEnterScopeAsync(scopeName, appendMode, encoding);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output failed (see inner exception)."), ex);
			}
		}

		public void LeaveScope(string scopeName)
		{
			if ((object)scopeName == null)
				throw new ArgumentNullException(nameof(scopeName));

			try
			{
				this.CoreLeaveScope(scopeName);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output failed (see inner exception)."), ex);
			}
		}

		public async ValueTask LeaveScopeAsync(string scopeName)
		{
			if ((object)scopeName == null)
				throw new ArgumentNullException(nameof(scopeName));

			try
			{
				await this.CoreLeaveScopeAsync(scopeName);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output failed (see inner exception)."), ex);
			}
		}

		public void WriteObject(object obj, Uri objectUri)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			if ((object)objectUri == null)
				throw new ArgumentNullException(nameof(objectUri));

			try
			{
				this.CoreWriteObject(obj, objectUri);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output failed (see inner exception)."), ex);
			}
		}

		public async ValueTask WriteObjectAsync(object obj, Uri objectUri)
		{
			if ((object)obj == null)
				throw new ArgumentNullException(nameof(obj));

			if ((object)objectUri == null)
				throw new ArgumentNullException(nameof(objectUri));

			try
			{
				await this.CoreWriteObjectAsync(obj, objectUri);
			}
			catch (Exception ex)
			{
				throw new TextMetalException(string.Format("The output failed (see inner exception)."), ex);
			}
		}

		#endregion
	}
}