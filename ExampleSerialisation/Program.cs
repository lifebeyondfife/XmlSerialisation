using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using NUnit.Framework;
using LbF.XmlSerialisation;

namespace ExampleSerialisation
{
	class Program
	{
		private const string XmlFile = @"..\..\St Johnstone.xml";
		private const string XmlSchemaFile = @"..\..\FootballClub.xsd";

		public static void Main(string[] args)
		{
			if (!Validate(XmlFile, XmlSchemaFile))
				return;

			var xmlHelper = new XmlSerialisationHelper<FootballClub>();

			var stJohnstone = xmlHelper.Deserialise(XmlFile);

			Console.WriteLine("Team Name:\t\t{0}", stJohnstone.Name);
			Console.WriteLine("Manager Name:\t\t{0}", stJohnstone.Manager.Name);
			Console.WriteLine("First Stadium Stand:\t{0}", stJohnstone.Stadium.Stands.First());
			Console.WriteLine("Number of Players:\t{0}", stJohnstone.Players.Count);

			Console.ReadKey();
		}

		private static bool Validate(string xmlFile, string xmlSchemaFile)
		{
			var validationErrors = default(IList<Tuple<object, XmlSchemaException>>);

			try
			{
				Assert.IsTrue(XmlSerialisationHelper.IsValidXml(xmlFile, xmlSchemaFile, out validationErrors));
			}
			catch (AssertionException)
			{
				if (validationErrors != null && validationErrors.Any())
				{
					foreach (var error in validationErrors)
						Console.WriteLine("{0}:\t{1}", error.Item1, error.Item2.Message);

					Console.ReadKey();
				}

				return false;
			}

			return true;
		}
	}
}
