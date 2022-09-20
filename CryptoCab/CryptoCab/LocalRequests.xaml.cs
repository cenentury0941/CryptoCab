using Mapsui;
using Mapsui.Projections;
using Mapsui.UI.Maui;
using System;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Org.BouncyCastle.Ocsp;

namespace CryptoCab;

public partial class LocalRequests : ContentPage
{

    MapControl mapControl;
    public List<RequestData> Data;
    Pin targetPin;

    public LocalRequests()
    {
        InitializeComponent();
        Data = new List<RequestData>();
        mapControl = new Mapsui.UI.Maui.MapControl();
        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        //App.Current.MainPage.DisplayAlert("Got", "Here", "OK");
        LoadingAnim.IsAnimationPlaying = true;
        LoadingAnim.IsVisible = true;
        setupmap();

        targetPin = new Pin(MapView)
        {
            Label = "Target",
            Position = new Position(0, 0),
            Type = PinType.Pin,
            Scale = 1f,
            Color = new Color(0, 200, 0)
        };

    }


    async public Task setupmap()
    {
        Location location = null;
        location = (await GetCurrentLocation());
        App.current_location = "null";
        if (location != null)
        {
            App.current_location = location.ToString();
            var smc = SphericalMercator.FromLonLat(location.Longitude, location.Latitude);
            mapControl.Map.Home = n => n.NavigateTo(new MPoint(smc.x, smc.y), mapControl.Map.Resolutions[16]);

            //0 zoomed out-19 zoomed in
            //App.Current.MainPage.DisplayAlert("Alert", "Hi" , "OK");

            MapView.Pins.Add(new Pin(MapView)
            {
                Label = "You",
                Position = new Position(location.Latitude, location.Longitude),
                Type = PinType.Pin,
                Scale = 1f,

            });


            MapView.Content = mapControl;
            MapView.Map = mapControl.Map;
            //MapView.MapClicked += OnMapClicked;
            //MapView.PinClicked += PinClicked;

            GetRequests();


        }


    }


    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    public async Task<Location> GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();
            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                //Current.Text = $"------Current Location------  Lat:{location.Latitude.ToString().Substring(0, 6)} Long:{location.Longitude.ToString().Substring(0, 6)}";
                Loading.IsVisible = false;
                LoadingAnim.IsVisible = false;
                App.current = location;
                return location;
            }
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }
        Loading.IsVisible = false;
        return null;
    }






    async void GetRequests()
    {
        var privateKey = App.userID;

        var account = new Account(privateKey);

        var web3 = new Web3("http://192.168.43.61:8545"); //https://polygon-rpc.com

        string globalhub_abi = "[{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"Lat\",\"type\":\"int256\"},{\"internalType\":\"int256\",\"name\":\"Long\",\"type\":\"int256\"}],\"name\":\"getLocalHubAddr\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"Lat\",\"type\":\"int256\"},{\"internalType\":\"int256\",\"name\":\"Long\",\"type\":\"int256\"},{\"internalType\":\"string\",\"name\":\"Addr\",\"type\":\"string\"}],\"name\":\"setLocalHubAddr\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string localhub_abi = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"Request\",\"type\":\"string\"}],\"name\":\"addRequest\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getRequests\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        string global_addr = App.GlobalHub;

        var globalhub = web3.Eth.GetContract(globalhub_abi, global_addr);

        int lat = App.current.Latitude > 0 ? 1 : -1;
        int lon = App.current.Longitude > 0 ? 1 : -1;

        var localhub_addr = await globalhub.GetFunction("getLocalHubAddr").CallAsync<string>(lat, lon);
        var localhub = web3.Eth.GetContract(localhub_abi, localhub_addr);

        Location cur, tar;
        cur = App.current;
        tar = App.destination;

        //string req = account.Address + "|" + cur.Latitude + "|" + cur.Longitude + "|" + tar.Latitude + "|" + tar.Longitude + "|" + App.type;

        //await localhub.GetFunction("addRequest").SendTransactionAsync(account.Address, new HexBigInteger(700000), new HexBigInteger(0), req);

        string data = (await localhub.GetFunction("getRequests").CallAsync<string>());//.SendTransactionAsync(account.Address, new HexBigInteger(700000), new HexBigInteger(0));
        Console.WriteLine(data);


        string[] data_list = data.Split(",");
        int index = 0;
        foreach (string s in data_list)
        {
            if (s.Length < 2)
            {
                continue;
            }
            string[] quantum = s.Split("|");
            RequestData rd = new RequestData(quantum[0], quantum[1], quantum[2], quantum[3], quantum[4], quantum[5], quantum[6], index++);
            Data.Add(rd);


            MapView.Pins.Add(new Pin(MapView)
            {
                Label = "" + (Data.Count - 1),
                Position = new Position(rd.Latitude1, rd.Longitude1),
                Type = PinType.Pin,
                Scale = 1f,
                Color = new Color(0, 0, 255)
            });


            MapView.Content = mapControl;
            MapView.Map = mapControl.Map;



        }

        //Data.Reverse();
        //RequestList.ItemsSource = Data;

    }


    void PinClicked(object sender, PinClickedEventArgs args)
    {
        try
        {
            Pin pin = args.Pin;
            if (pin.Label != "You" && pin.Label != "Target")
            {
                int index = (int)Double.Parse(pin.Label);
                RequestData data = Data[index];
                targetPin.Position = new Position(data.Latitude2, data.Longitude2);
                if (MapView.Pins.Contains(targetPin) != true)
                {
                    MapView.Pins.Add(targetPin);
                }
                MapView.Content = mapControl;
                MapView.Map = mapControl.Map;
                Current.Text = $"------Current Location------  Lat:{data.Latitude1.ToString().Substring(0, 6)} Long:{data.Longitude1.ToString().Substring(0, 6)}";
                Destination.Text = $"------Destination Location------  Lat:{data.Latitude2.ToString().Substring(0, 6)} Long:{data.Longitude2.ToString().Substring(0, 6)}";
                double cost = Location.CalculateDistance(data.Latitude1, data.Longitude1, new Location(data.Latitude2, data.Longitude2) , DistanceUnits.Kilometers);
                Cost.Text = $"{cost:00.0000} ETH";
            }
        }
        catch (Exception e)
        {
            Current.Text = e.Message ;
        }
    }





}