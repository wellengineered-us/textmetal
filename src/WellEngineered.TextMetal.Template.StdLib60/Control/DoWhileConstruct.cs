/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;

using WellEngineered.Solder.Serialization.Xyzl;

namespace WellEngineered.TextMetal.Template.Core
{
	[XyzlElementMapping(LocalName = "DoWhile", NamespaceUri = "http://www.textmetal.com/api/v6.0.0", ChildElementModel = ChildElementModel.Sterile)]
	public sealed class DoWhileConstruct : ConditionalIterationConstruct
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the DoWhileConstruct class.
		/// </summary>
		public DoWhileConstruct()
			: base(false, true)
		{
		}

		#endregion

		#region Methods/Operators

		protected override void CoreConditionalIterationInitialize(ITemplatingContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));
		}

		protected override void CoreConditionalIterationStep(ITemplatingContext templatingContext, uint indexOneBase)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));
		}

		protected override void CoreConditionalIterationTerminate(ITemplatingContext templatingContext)
		{
			if ((object)templatingContext == null)
				throw new ArgumentNullException(nameof(templatingContext));
		}

		#endregion
	}
}