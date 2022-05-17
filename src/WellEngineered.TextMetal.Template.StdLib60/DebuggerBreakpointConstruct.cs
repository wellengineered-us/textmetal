/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.Generic;

using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.TextMetal.Hosting;
using WellEngineered.TextMetal.Template.Associative;
using WellEngineered.TextMetal.Template.Core;
using WellEngineered.TextMetal.Template.Expression;
using WellEngineered.TextMetal.Template.Sort;

namespace WellEngineered.TextMetal.Template
{
	/// <summary>
	/// Allows an author of a TextMetal template file to declaratively set a CLR breakpoint anywhere in the object tree.
	/// </summary>
	[XyzlElementMapping(LocalName = "DebuggerBreakpoint", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public class DebuggerBreakpointConstruct : ITextMetalTemplateObject, IExpressionXmlObject, IAssociativeXmlObject, ISortXmlObject
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DebuggerBreakpointConstruct class.
		/// </summary>
		public DebuggerBreakpointConstruct()
		{
		}

		#endregion

		#region Fields/Constants

		private IXmlObject parent;
		private IXmlObjectCollection surround;

		#endregion

		#region Properties/Indexers/Events

		/// <summary>
		/// Gets an array of allowed child XML object types.
		/// </summary>
		public Type[] AllowedChildTypes
		{
			get
			{
				return new Type[] { typeof(IXmlObject) };
			}
		}

		/// <summary>
		/// Gets a list of XML object items. This implementation always return null.
		/// </summary>
		public IXmlObjectCollection<IXmlObject> Items
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the associative name of the current associative XML object.
		/// </summary>
		public string Name
		{
			get
			{
				return null;
			}
		}

		public bool? SortDirection
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the optional single XML object content. This implementation always return null.
		/// </summary>
		public IXmlObject Content
		{
			get
			{
				return null;
			}
			set
			{
				// do nothing
			}
		}

		/// <summary>
		/// Gets or sets the parent XML object or null if this is the document root.
		/// </summary>
		public IXmlObject Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		/// <summary>
		/// Gets or sets the surround XML object or null if this is not surrounded (in a collection).
		/// </summary>
		public IXmlObjectCollection Surround
		{
			get
			{
				return this.surround;
			}
			set
			{
				this.surround = value;
			}
		}

		#endregion

		#region Methods/Operators

		/// <summary>
		/// Evaluates at run-time, an expression tree yielding an object value result.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <returns> An expression return value or null. </returns>
		public object EvaluateExpression(ITextMetalContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();

			return null;
		}

		/// <summary>
		/// Re-orders an enumerable of values, yielding a re-ordered enumerable.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <param name="values"> </param>
		/// <returns> </returns>
		public IEnumerable EvaluateSort(ITextMetalContext templatingContext, IEnumerable values)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();

			return values;
		}

		/// <summary>
		/// Expands the template tree into the templating context current output.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		public void ExpandTemplate(ITextMetalContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();
		}

		/// <summary>
		/// Gets the enumerator for the current associative object instance.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <returns> An instance of IEnumerator or null. </returns>
		public IEnumerator GetAssociativeObjectEnumerator(ITextMetalContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();

			return null;
		}

		/// <summary>
		/// Gets the dictionary enumerator for the current associative object instance.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <returns> An instance of IDictionaryEnumerator or null. </returns>
		public IDictionaryEnumerator GetAssociativeObjectEnumeratorDict(ITextMetalContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();

			return null;
		}

		/// <summary>
		/// Gets the enumerator (tick one) for the current associative object instance.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <returns> An instance of IEnumerator`1 or null. </returns>
		public IEnumerator<KeyValuePair<string, object>> GetAssociativeObjectEnumeratorTickOne(ITextMetalContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();

			return null;
		}

		/// <summary>
		/// Gets the value of the current associative object instance.
		/// </summary>
		/// <param name="templatingContext"> The templating context. </param>
		/// <returns> A value or null. </returns>
		public object GetAssociativeObjectValue(ITextMetalContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));

			templatingContext.DebugBreak();

			return this;
		}

		#endregion
	}
}