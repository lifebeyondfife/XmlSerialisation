using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LbF.XmlSerialisation
{
	public static class XmlSerialisationHelper
	{
		/// <summary>
		///	Validates an XML file against a given XSD schema.
		/// </summary>
		/// <param name="xmlFileUrl">File location of the XML file to validate.</param>
		/// <param name="xmlSchemaFile">File location of the XSD schema to validate the XML file against.</param>
		/// <returns>True if a valid XML file according to the XSD schema, false otherwise.</returns>
		public static bool IsValidXml(string xmlFileUrl, string xmlSchemaFile)
		{
			IList<Tuple<object, XmlSchemaException>> _;
			
			return IsValidXml(xmlFileUrl, xmlSchemaFile, out _);
		}
		
		/// <summary>
		///	Validates an XML file against a given XSD schema with validation error messages.
		/// </summary>
		/// <param name="xmlFileUrl">File location of the XML file to validate.</param>
		/// <param name="xmlSchemaFile">File location of the XSD schema to validate the XML file against.</param>
		/// <param name="validationErrors">Collection of validation error information objects if the XML file violates the XSD schema.</param>
		/// <returns>True if a valid XML file according to the XSD schema, false otherwise.</returns>
		public static bool IsValidXml(string xmlFileUrl, string xmlSchemaFile, out IList<Tuple<object, XmlSchemaException>> validationErrors)
		{
			var internalValidationErrors = new List<Tuple<object, XmlSchemaException>>();
			var readerSettings = XmlSchemaReader(xmlSchemaFile,
				(obj, eventArgs) => internalValidationErrors.Add(new Tuple<object, XmlSchemaException>(obj, eventArgs.Exception))
			);

			using (var xmlReader = new XmlTextReader(xmlFileUrl))
			using (var objXmlReader = XmlReader.Create(xmlReader, readerSettings))
			{
				try
				{
					while (objXmlReader.Read()) { }
				}
				catch (XmlSchemaException exception)
				{
					internalValidationErrors.Add(new Tuple<object, XmlSchemaException>(objXmlReader, exception));
				}
			}

			validationErrors = internalValidationErrors;

			return !validationErrors.Any();
		}

		private static XmlReaderSettings XmlSchemaReader(string xmlSchemaFile, Action<object, ValidationEventArgs> validationFunction)
		{
			var xsdReader = new StreamReader(xmlSchemaFile);
			var schema = XmlSchema.Read(xsdReader, (obj, eventArgs) => validationFunction(obj, eventArgs));

			var readerSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
			readerSettings.Schemas.Add(schema);

			return readerSettings;
		}
	}

	public class XmlSerialisationHelper<T>
	{
		private Type SerialiseType { get; set; }

		/// <summary>
		/// Creates a helper object to serialise and deserialise XML and objects of type <see cref="T"/>.
		/// </summary>
		public XmlSerialisationHelper()
		{
			SerialiseType = typeof(T);
		}

		/// <summary>
		/// Serialises an object of type <see cref="T"/> by writing an XML file representation to disk.
		/// </summary>
		/// <param name="xmlFileUrl">File location to write the XML data to.</param>
		/// <param name="obj">Object of type <see cref="T"/> to be serialised.</param>
		public void Serialise(string xmlFileUrl, T obj)
		{
			using (TextWriter textWriter = new StreamWriter(xmlFileUrl))
			{
				var serializer = new XmlSerializer(SerialiseType);
				serializer.Serialize(textWriter, obj);
			}
		}

		/// <summary>
		/// Deserialises an XML file representation of object of type <see cref="T"/>.
		/// </summary>
		/// <param name="xmlFileUrl">The file location of the XML data to deserialise.</param>
		/// <param name="xmlRootAttribute">The name of the root XML node.</param>
		/// <returns>Deserialised object of type <see cref="T"/>.</returns>
		public T Deserialise(string xmlFileUrl, XmlRootAttribute xmlRootAttribute)
		{
			using (TextReader textReader = new StreamReader(xmlFileUrl))
			{
				var deserializer = new XmlSerializer(SerialiseType, xmlRootAttribute);
				return (T) deserializer.Deserialize(textReader);
			}
		}
	}
}