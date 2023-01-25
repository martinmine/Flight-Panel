using System.Xml.Serialization;

namespace AvinorFlydataClient.Model;

[XmlRoot(ElementName = "status")]
public class Status
{
    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }
    [XmlAttribute(AttributeName = "time")]
    public DateTime Time { get; set; }
}

[XmlRoot(ElementName = "flight")]
public class Flight
{
    [XmlElement(ElementName = "airline")]
    public string Airline { get; set; }

    [XmlElement(ElementName = "flight_id")]
    public string Flight_id { get; set; }

    [XmlElement(ElementName = "dom_int")]
    public string Dom_int { get; set; }

    [XmlElement(ElementName = "schedule_time")]
    public DateTime Schedule_time { get; set; }

    [XmlElement(ElementName = "arr_dep")]
    public string Arr_dep { get; set; }
    [XmlElement(ElementName = "airport")]
    public string Airport { get; set; }
    [XmlElement(ElementName = "status")]
    public Status Status { get; set; }

    [XmlAttribute(AttributeName = "uniqueID")]
    public string UniqueID { get; set; }

    [XmlElement(ElementName = "check_in")]
    public string Check_in { get; set; }
    [XmlElement(ElementName = "gate")]
    public string Gate { get; set; }
    [XmlElement(ElementName = "delayed")]
    public string Delayed { get; set; }

    [XmlElement(ElementName = "via_airport")]
    public string Via_airport { get; set; }
}

[XmlRoot(ElementName = "flights")]
public class Flights
{
    [XmlElement(ElementName = "flight")]
    public List<Flight> Flight { get; set; }

    [XmlAttribute(AttributeName = "lastUpdate")]
    public string LastUpdate { get; set; }
}

[XmlRoot(ElementName = "airport")]
public class Airport
{
    [XmlElement(ElementName = "flights")]
    public Flights Flights { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
}