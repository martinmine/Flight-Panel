namespace FlightListener.OpenSkyNetwork;

public class State
{
    /// <summary>
    /// Unique ICAO 24-bit address of the transponder in hex string representation.
    /// </summary>
    public string Icao24 { get; init; }
    
    /// <summary>
    /// Callsign of the vehicle (8 chars). Can be null if no callsign has been received. 
    /// </summary>
    public string Callsign { get; init; }
    
    /// <summary>
    /// Country name inferred from the ICAO 24-bit address.
    /// </summary>
    public string OriginCountry { get; init; }
    
    /// <summary>
    /// Unix timestamp (seconds) for the last position update. Can be null if no position report was received by OpenSky within the past 15s.
    /// </summary>
    public int TimePosition { get; init; }
    
    /// <summary>
    /// Unix timestamp (seconds) for the last update in general. This field is updated for any new, valid message received from the transponder.
    /// </summary>
    public int LastContact { get; init; }
    
    /// <summary>
    /// WGS-84 longitude in decimal degrees. Can be null.
    /// </summary>
    public double Longitude { get; init; }
    
    /// <summary>
    /// WGS-84 latitude in decimal degrees. Can be null.
    /// </summary>
    public double Latitude { get; init; }
    
    /// <summary>
    /// Barometric altitude in meters. Can be null.
    /// </summary>
    public double? BaroAltitude { get; init; }
    
    /// <summary>
    /// Boolean value which indicates if the position was retrieved from a surface position report.
    /// </summary>
    public bool OnGround { get; init; }
    
    /// <summary>
    /// Velocity over ground in m/s. Can be null.
    /// </summary>
    public double Velocity { get; init; }
    
    /// <summary>
    /// True track in decimal degrees clockwise from north (north=0°). Can be null.
    /// </summary>
    public double TrueTrack { get; init; }
    
    /// <summary>
    /// Vertical rate in m/s. A positive value indicates that the airplane is climbing, a negative value indicates that it descends. Can be null.
    /// </summary>
    public double VerticalRate { get; init; }
    
    /// <summary>
    /// IDs of the receivers which contributed to this state vector. Is null if no filtering for sensor was used in the request.
    /// </summary>
    public int[] Sensors { get; init; }
    
    /// <summary>
    /// Geometric altitude in meters. Can be null.
    /// </summary>
    public double GeoAltitude { get; init; }
    
    /// <summary>
    /// The transponder code aka Squawk. Can be null.
    /// </summary>
    public string Squawk { get; init; }
    
    /// <summary>
    /// Whether flight status indicates special purpose indicator.
    /// </summary>
    public bool Spi { get; init; }
    
    /// <summary>
    /// Origin of this state’s position.
    /// 0 = ADS-B
    /// 1 = ASTERIX
    /// 2 = MLAT
    /// 3 = FLARM
    /// </summary>
    public int PositionSource { get; init; }
    
    /// <summary>
    /// Aircraft category.
    /// 0 = No information at all
    /// 1 = No ADS-B Emitter Category Information
    /// 2 = Light (< 15500 lbs)
    /// 3 = Small (15500 to 75000 lbs)
    /// 4 = Large (75000 to 300000 lbs)
    /// 5 = High Vortex Large (aircraft such as B-757)
    /// 6 = Heavy (> 300000 lbs)
    /// 7 = High Performance (> 5g acceleration and 400 kts)
    /// 8 = Rotorcraft
    /// 9 = Glider / sailplane
    /// 10 = Lighter-than-air
    /// 11 = Parachutist / Skydiver
    /// 12 = Ultralight / hang-glider / paraglider
    /// 13 = Reserved
    /// 14 = Unmanned Aerial Vehicle
    /// 15 = Space / Trans-atmospheric vehicle
    /// 16 = Surface Vehicle – Emergency Vehicle
    /// 17 = Surface Vehicle – Service Vehicle
    /// 18 = Point Obstacle (includes tethered balloons)
    /// 19 = Cluster Obstacle
    /// 20 = Line Obstacle
    /// </summary>
    public int Category { get; init; }
}