/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading.Tasks;
using System.Xml;

using NUnit.Framework;

using WellEngineered.TextMetal.Hosting.Email;

using __ITextMetalModel = System.Object;

namespace WellEngineered.TextMetal.IntegrationTests.Cli
{
	[TestFixture]
	public class TextMetalEmailHostTests
	{
		#region Constructors/Destructors

		public TextMetalEmailHostTests()
		{
		}

		#endregion

		#region Methods/Operators

		[Test]
		public void ShouldCreateTest()
		{
			const bool STRICT_MATCHING = true;
			EmailPrototype emailPrototype;
			EmailMessage emailMessage;
			__ITextMetalModel textMetalModel;

			XmlDocument xmlDocument;

			using (TextMetalEmailHost textMetalHost = new TextMetalEmailHost())
			{
				emailPrototype = new EmailPrototype();
				textMetalModel = new object();

				xmlDocument = new XmlDocument();
				emailPrototype.FromXml = xmlDocument.CreateElement("From", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.ToXml = xmlDocument.CreateElement("To", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.ReplyToXml = xmlDocument.CreateElement("ReplyTo", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.SenderXml = xmlDocument.CreateElement("Sender", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.CarbonCopyXml = xmlDocument.CreateElement("CarbonCopy", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.BlindCarbonCopyXml = xmlDocument.CreateElement("BlindCarbonCopy", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.SubjectXml = xmlDocument.CreateElement("Subject", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.BodyXml = xmlDocument.CreateElement("Body", "http://textmetal.wellengineered.us/stdlib/v6.0.0");

				textMetalHost.Create();
				emailMessage = textMetalHost.Host(STRICT_MATCHING, emailPrototype, textMetalHost);

				Assert.IsNotNull(emailMessage);

				Console.WriteLine("From:            {0}~", emailMessage.From);
				Console.WriteLine("To:              {0}~", emailMessage.To);
				Console.WriteLine("ReplyTo:         {0}~", emailMessage.ReplyTo);
				Console.WriteLine("Sender:          {0}~", emailMessage.Sender);
				Console.WriteLine("CarbonCopy:      {0}~", emailMessage.CarbonCopy);
				Console.WriteLine("BlindCarbonCopy: {0}~", emailMessage.BlindCarbonCopy);
				Console.WriteLine("Subject:         {0}~", emailMessage.Subject);
				Console.WriteLine("IsBodyHtml:      {0}~", emailMessage.IsBodyHtml);
				Console.WriteLine("Body:            {0}~", emailMessage.Body);
				Console.WriteLine("Processed:       {0}~", emailMessage.Processed);
			}
		}
		
		[Test]
		public async Task ShouldCreateTestAsync()
		{
			const bool STRICT_MATCHING = true;
			EmailPrototype emailPrototype;
			EmailMessage emailMessage;
			__ITextMetalModel textMetalModel;

			XmlDocument xmlDocument;

			await using (TextMetalEmailHost textMetalHost = new TextMetalEmailHost())
			{
				emailPrototype = new EmailPrototype();
				textMetalModel = new object();

				xmlDocument = new XmlDocument();
				emailPrototype.FromXml = xmlDocument.CreateElement("From", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.ToXml = xmlDocument.CreateElement("To", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.ReplyToXml = xmlDocument.CreateElement("ReplyTo", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.SenderXml = xmlDocument.CreateElement("Sender", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.CarbonCopyXml = xmlDocument.CreateElement("CarbonCopy", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.BlindCarbonCopyXml = xmlDocument.CreateElement("BlindCarbonCopy", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.SubjectXml = xmlDocument.CreateElement("Subject", "http://textmetal.wellengineered.us/stdlib/v6.0.0");
				emailPrototype.BodyXml = xmlDocument.CreateElement("Body", "http://textmetal.wellengineered.us/stdlib/v6.0.0");

				await textMetalHost.CreateAsync();
				emailMessage = await textMetalHost.HostAsync(STRICT_MATCHING, emailPrototype, textMetalHost);

				Assert.IsNotNull(emailMessage);

				await Console.Out.WriteLineAsync(string.Format("From:            {0}~", emailMessage.From));
				await Console.Out.WriteLineAsync(string.Format("To:              {0}~", emailMessage.To));
				await Console.Out.WriteLineAsync(string.Format("ReplyTo:         {0}~", emailMessage.ReplyTo));
				await Console.Out.WriteLineAsync(string.Format("Sender:          {0}~", emailMessage.Sender));
				await Console.Out.WriteLineAsync(string.Format("CarbonCopy:      {0}~", emailMessage.CarbonCopy));
				await Console.Out.WriteLineAsync(string.Format("BlindCarbonCopy: {0}~", emailMessage.BlindCarbonCopy));
				await Console.Out.WriteLineAsync(string.Format("Subject:         {0}~", emailMessage.Subject));
				await Console.Out.WriteLineAsync(string.Format("IsBodyHtml:      {0}~", emailMessage.IsBodyHtml));
				await Console.Out.WriteLineAsync(string.Format("Body:            {0}~", emailMessage.Body));
				await Console.Out.WriteLineAsync(string.Format("Processed:       {0}~", emailMessage.Processed));
			}
		}

		#endregion
	}
}