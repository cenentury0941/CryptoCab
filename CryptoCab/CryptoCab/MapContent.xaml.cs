//using Android.Graphics.Drawables;
using Mapsui;
using Mapsui.Projections;
using Mapsui.UI.Maui;
using System;
using Location = Microsoft.Maui.Devices.Sensors.Location;


namespace CryptoCab;

public partial class MapContent : ContentView
{

    MapControl mapControl;

    public MapContent()
	{
		InitializeComponent();
        mapControl = new Mapsui.UI.Maui.MapControl();
        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        //App.Current.MainPage.DisplayAlert("Got", "Here", "OK");
        LoadingAnim.IsAnimationPlaying = true;
        LoadingAnim.IsVisible = true;
        setupmap();
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

            MapView.Pins.Add(new Pin(MapView) {
                Label = "You",
                Position = new Position( location.Latitude , location.Longitude ),
                Type = PinType.Pin,
                Scale = 1f,
                
            });

            
            MapView.Content = mapControl;
            MapView.Map = mapControl.Map;
            MapView.MapClicked += OnMapClicked;
        }


    }

    public async Task<string> GetCachedLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            App.Current.MainPage.DisplayAlert("fns", fnsEx.Message , "OK");
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            App.Current.MainPage.DisplayAlert("fne", fneEx.Message, "OK");

            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            App.Current.MainPage.DisplayAlert("pex", pEx.Message, "OK");

            // Handle permission exception
        }
        catch (Exception ex)
        {
            App.Current.MainPage.DisplayAlert("ex", ex.Message, "OK");

            // Unable to get location
        }

        return "None";
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
                Current.Text = $"------Current Location------  Lat:{location.Latitude.ToString().Substring(0,6)} Long:{location.Longitude.ToString().Substring(0,6)}";
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


    public void selectcab(object sender, EventArgs args)
    {
        Navigation.PushAsync( new CabSelect() );
    }



    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        //App.Current.MainPage.DisplayAlert("ex", e.ToString() , "OK");

        Destination.Text = $" ------Target Location------  Lat:{e.Point.Latitude.ToString().Substring(0, 6)} Long:{e.Point.Longitude.ToString().Substring(0, 6)}";
        try
        {
            MapView.Pins.RemoveAt(1);
        }
        catch(Exception)
        {

        }
        MapView.Pins.Add(new Pin(MapView)
        {
            Label = "Target",
            Position = new Position(e.Point.Latitude, e.Point.Longitude),
            Type = PinType.Pin,
            Scale = 1f,
            Color = new Color( 0 , 255, 0)
        }) ;
        App.destination = new Location( e.Point.Latitude , e.Point.Longitude );
        //e.Handled = Clicker != null ? (Clicker?.Invoke(sender as MapView, e) ?? false) : false;
        //Samples.SetPins(mapView, e);
        //Samples.DrawPolylines(mapView, e);
    }



}