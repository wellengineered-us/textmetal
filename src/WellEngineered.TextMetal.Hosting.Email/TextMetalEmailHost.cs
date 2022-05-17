/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

using WellEngineered.Solder.Extensions;
using WellEngineered.Solder.Serialization;
using WellEngineered.Solder.Serialization.Xyzl;
using WellEngineered.Solder.Tokenization;
using WellEngineered.TextMetal.Context;
using WellEngineered.TextMetal.Primitives;
using WellEngineered.TextMetal.Template;

using __ITextMetalModel = System.Object;

namespace WellEngineered.TextMetal.Hosting.Email
{
	public sealed class TextMetalEmailHost : TextMetalHost, ITextMetalEmailHost
	{
		#region Constructors/Destructors

		public TextMetalEmailHost()
			: this(new StringWriter())
		{
		}

		public TextMetalEmailHost(StringWriter stringWriter)
		{
			if ((object)stringWriter == null)
				throw new ArgumentNullException(nameof(stringWriter));

			this.stringWriter = stringWriter;
		}

		#endregion

		#region Fields/Constants

		private readonly StringWriter stringWriter;

		#endregion

		#region Properties/Indexers/Events

		public StringWriter StringWriter
		{
			get
			{
				return this.stringWriter;
			}
		}

		#endregion

		#region Methods/Operators

		protected override ITextMetalContext CoreCreateContext()
		{
			const bool STRICT_MATCHING = true;
			IDictionary<string, ITokenReplacementStrategy> tokenReplacementStrategies;

			tokenReplacementStrategies = new Dictionary<string, ITokenReplacementStrategy>();

			return new TextMetalEmailContext(this.StringWriter,
				SolderFascadeAccessor.DataTypeFascade,
				SolderFascadeAccessor.ReflectionFascade,
				new Tokenizer(SolderFascadeAccessor.DataTypeFascade, SolderFascadeAccessor.ReflectionFascade,
					tokenReplacementStrategies, STRICT_MATCHING));
		}

		protected override void CoreDispose(bool disposing)
		{
			if (this.IsDisposed)
				return;

			if (disposing)
			{
				if ((object)this.StringWriter != null)
					this.StringWriter.Dispose();

				base.CoreDispose(disposing);
			}
		}

		protected override bool CoreLaunchDebugger()
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		protected override Assembly CoreLoadAssembly(string assemblyName)
		{
			throw new TextMetalException("Unsupported host operation.");
		}

		public EmailMessage Host(bool strictMatching, EmailPrototype emailPrototype, __ITextMetalModel textMetalModel)
		{
			ITextMetalTemplateObject textMetalTemplate;
			EmailMessage emailMessage;

			INativeXmlSerializationStrategy nativeXmlSerializationStrategy;
			IXyzlSerializer xyzlSerializer;
			XmlTextReader templateXmlTextReader;

			if ((object)emailPrototype == null)
				throw new ArgumentNullException(nameof(emailPrototype));

			emailMessage = new EmailMessage();

			xyzlSerializer = new XyzlSerializer(SolderFascadeAccessor.DataTypeFascade, SolderFascadeAccessor.ReflectionFascade);
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "From", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "To", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "ReplyTo", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "Sender", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "CarbonCopy", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "BlindCarbonCopy", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "Subject", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "Body", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });

			//xyzlSerializer.RegisterKnownValueObject<TextMetalTemplateTextObject>();

			nativeXmlSerializationStrategy = new NativeXyzlSerializationStrategy(xyzlSerializer);

			using (ITextMetalContext textMetalContext = this.CreateContext())
			{
				textMetalContext.IteratorModels.Push(textMetalModel);

				// FROM
				if ((object)emailPrototype.FromXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.FromXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.From = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// SENDER
				if ((object)emailPrototype.SenderXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.SenderXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.Sender = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// REPLY_TO
				if ((object)emailPrototype.ReplyToXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.ReplyToXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.ReplyTo = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// TO
				if ((object)emailPrototype.ToXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.ToXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.To = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// CC
				if ((object)emailPrototype.CarbonCopyXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.CarbonCopyXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.CarbonCopy = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// BCC
				if ((object)emailPrototype.BlindCarbonCopyXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.BlindCarbonCopyXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.BlindCarbonCopy = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// SUBJECT
				if ((object)emailPrototype.SubjectXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.SubjectXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.Subject = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// IS_BODY_HTML
				emailMessage.IsBodyHtml = emailPrototype.IsBodyHtml;

				// BODY
				if ((object)emailPrototype.BodyXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.BodyXml.OuterXml)))
						textMetalTemplate = nativeXmlSerializationStrategy.DeserializeObjectFromNative<ITextMetalTemplateObject>(templateXmlTextReader);

					textMetalTemplate.ExpandTemplate(textMetalContext);

					emailMessage.Body = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				textMetalContext.IteratorModels.Pop();
			}

			return emailMessage;
		}

		public async ValueTask<EmailMessage> HostAsync(bool strictMatching, EmailPrototype emailPrototype, object textMetalModel)
		{
			ITextMetalTemplateObject textMetalTemplate;
			EmailMessage emailMessage;

			INativeXmlSerializationStrategy nativeXmlSerializationStrategy;
			IXyzlSerializer xyzlSerializer;
			XmlTextReader templateXmlTextReader;

			if ((object)emailPrototype == null)
				throw new ArgumentNullException(nameof(emailPrototype));

			emailMessage = new EmailMessage();

			xyzlSerializer = new XyzlSerializer(SolderFascadeAccessor.DataTypeFascade, SolderFascadeAccessor.ReflectionFascade);
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "From", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "To", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "ReplyTo", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "Sender", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "CarbonCopy", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "BlindCarbonCopy", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "Subject", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });
			xyzlSerializer.RegisterKnownObject<EmailTemplateEchoFragment>(new XyzlName() { LocalName = "Body", NamespaceUri = "http://textmetal.wellengineered.us/stdlib/v6.0.0" });

			//xyzlSerializer.RegisterKnownValueObject<TextMetalTemplateTextObject>();

			nativeXmlSerializationStrategy = new NativeXyzlSerializationStrategy(xyzlSerializer);

			await using (ITextMetalContext textMetalContext = this.CreateContext())
			{
				textMetalContext.IteratorModels.Push(textMetalModel);

				// FROM
				if ((object)emailPrototype.FromXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.FromXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.From = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// SENDER
				if ((object)emailPrototype.SenderXml != null)
				{ 
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.SenderXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.Sender = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// REPLY_TO
				if ((object)emailPrototype.ReplyToXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.ReplyToXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.ReplyTo = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// TO
				if ((object)emailPrototype.ToXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.ToXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.To = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// CC
				if ((object)emailPrototype.CarbonCopyXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.CarbonCopyXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.CarbonCopy = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// BCC
				if ((object)emailPrototype.BlindCarbonCopyXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.BlindCarbonCopyXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.BlindCarbonCopy = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// SUBJECT
				if ((object)emailPrototype.SubjectXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.SubjectXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.Subject = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				// IS_BODY_HTML
				emailMessage.IsBodyHtml = emailPrototype.IsBodyHtml;

				// BODY
				if ((object)emailPrototype.BodyXml != null)
				{
					using (templateXmlTextReader = new XmlTextReader(new StringReader(emailPrototype.BodyXml.OuterXml)))
						textMetalTemplate = await nativeXmlSerializationStrategy.DeserializeObjectFromNativeAsync<ITextMetalTemplateObject>(templateXmlTextReader);

					await textMetalTemplate.ExpandTemplateAsync(textMetalContext);

					emailMessage.Body = this.StringWriter.ToString();
					this.StringWriter.GetStringBuilder().Clear();
				}

				textMetalContext.IteratorModels.Pop();
			}

			return emailMessage;
		}

		#endregion
	}
}