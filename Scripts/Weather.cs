using SQLite4Unity3d;

public class Weather  {

    
    public string location { get; set; }
    public float Lat { get; set; }
    public float Lon { get; set; }
    public float temp { get; set; }
    public float wind { get; set; }
    public float windDeg { get; set; }
    public float visibility { get; set; }
    public float pressure { get; set; }
    public float humidity { get; set; }
    public float clouds { get; set; }
    public int weatherID { get; set; }
    public int fetchTime { get; set; }
    public int setID { get; set; }
    
    public override string ToString ()
    {
        return string.Format ("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t",
                            location, Lat, Lon, temp, wind, windDeg, visibility, pressure, humidity, clouds, weatherID, fetchTime, setID);
    }
   
}
