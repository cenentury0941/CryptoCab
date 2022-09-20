using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCab;
public class RequestData
{
    public string Type { get; set; }
    public double Longitude1 { get; set; }
    public double Latitude1 { get; set; }
    public double Longitude2 { get; set; }
    public double Latitude2 { get; set; }
    public string WalletID { get; set; }
    public string DriverID { get; set; }
    public double Cost { get; set; }
    public string Driver { get; set; }
    public string Location_String1 { get; set; }
    public string Location_String2 { get; set; }
    public int Index { get; set; }

    public RequestData( string walletid , string lat1 , string lon1 , string lat2 , string lon2 , string type , string driver , int index)
    { 
        WalletID = walletid;
        Latitude1 = Double.Parse(lat1);
        Longitude1 = Double.Parse(lon1);
        Latitude2 = Double.Parse(lat2);
        Longitude2 = Double.Parse(lon2);
        Location_String1 = $"------Current Location------  Lat:{Latitude1:00.0000} Long:{Longitude1:00.0000}";
        Location_String2 = $"------Destination Location------  Lat:{Latitude2:00.0000} Long:{Longitude2:00.0000}";
        Type = type;
        Driver = driver;
        Index = index;
    }

}
