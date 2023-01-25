using System.Xml.Serialization;

namespace AvinorFlydataClient.Model;

[XmlRoot(ElementName="flightStatus")]
public class FlightStatus {
    [XmlAttribute(AttributeName="code")]
    public string Code { get; set; }
    [XmlAttribute(AttributeName="statusTextEn")]
    public string StatusTextEn { get; set; }
    [XmlAttribute(AttributeName="statusTextNo")]
    public string StatusTextNo { get; set; }
}

[XmlRoot(ElementName="flightStatuses")]
public class FlightStatuses {
    [XmlElement(ElementName="flightStatus")]
    public List<FlightStatus> FlightStatus { get; set; }
}