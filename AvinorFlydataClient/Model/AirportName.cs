using System.Xml.Serialization;

namespace AvinorFlydataClient.Model;

[XmlRoot(ElementName="airportName")]
public class AirportName {
    [XmlAttribute(AttributeName="code")]
    public string Code { get; set; }
    [XmlAttribute(AttributeName="name")]
    public string Name { get; set; }
}

[XmlRoot(ElementName="airportNames")]
public class AirportNames {
    [XmlElement(ElementName="airportName")]
    public List<AirportName> AirportName { get; set; }
}