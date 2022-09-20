namespace CryptoCab;

using System;
using System.Threading;
using Mapsui.UI;
using System.Threading.Tasks;
using Mapsui.Extensions;
using Mapsui.Logging;
using Mapsui.Rendering.Skia;

using Mapsui.Styles;
using Mapsui.UI.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Devices.Sensors;
using Compass = Microsoft.Maui.Devices.Sensors.Compass;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;

public partial class MapPage : ContentPage
{
	public MapPage()
	{
		InitializeComponent();
    }


}
