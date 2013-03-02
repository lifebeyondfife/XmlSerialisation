using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace ExampleSerialisation
{
	[Serializable]
	public class FootballClub
	{
		[XmlAttribute("Version")]
		public string Version { get; set; }

		[XmlAttribute("TeamName")]
		public string Name { get; set; }

		[XmlElement("Stadium")]
		public Stadium Stadium { get; set; }

		[XmlElement("Manager")]
		public Manager Manager { get; set; }

		[XmlArray("Players")]
		public Players Players { get; set; }
	}

	[Serializable]
	public class Players : Collection<Player>
	{
	}

	[Serializable]
	public class Player
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }

		[XmlAttribute("Number")]
		public int Number { get; set; }

		[XmlAttribute("Nationality")]
		public string Nationality { get; set; }

		[XmlAttribute("Position")]
		public string Position { get; set; }
	}

	[Serializable]
	public class Manager
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }

		[XmlAttribute("Age")]
		public int Age { get; set; }

		[XmlAttribute("Nationality")]
		public string Nationality { get; set; }
	}

	[Serializable]
	public class Stadium
	{
		[XmlAttribute("Name")]
		public string Name { get; set; }

		[XmlAttribute("Capacity")]
		public int Capacity { get; set; }

		[XmlAttribute("Opened")]
		public DateTime Opened { get; set; }

		[XmlArray("Stands")]
		[XmlArrayItem("Stand")]
		public List<string> Stands { get; set; }
	}
}
