using System.Xml.Serialization;

namespace AvinorFlydataClient.Model;

[XmlRoot(ElementName="airlineNames")]
public class AirlineNames
{
    [XmlElement(ElementName="airlineName")]
    public List<AirlineName> AirlineName { get; set; }
}

[XmlRoot(ElementName = "airlineName")]
public class AirlineName
{
    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }
    
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
}