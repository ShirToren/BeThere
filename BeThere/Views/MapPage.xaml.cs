using BeThere.ViewModels;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using System.Net.NetworkInformation;
//using Map = Microsoft.Maui.Controls.Maps.Map;

namespace BeThere.Views;

public partial class MapPage : ContentPage
{
    private Location m_UsersCurrentLocation;
    private Location m_WishedLocation;
    private double m_WishedRadius;
    private Pin m_WishedLocationPin;
    private Circle m_RadiusCircle;


    public MapPage()
	{
		InitializeComponent();
        Task task = GetCurrentLocation();
        m_WishedRadius = 0;
        m_WishedLocationPin = new Pin
        {
            Label = "Ask someone here",
            Type = PinType.Place
        };
        m_RadiusCircle = new Circle
        {
            StrokeWidth = 8,
            FillColor = Color.FromArgb("#88FFC0CB")
        };
    }

    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    protected async override void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }


    public async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                m_UsersCurrentLocation = location;
                var mapSpan = MapSpan.FromCenterAndRadius(
                new Location(location.Latitude, location.Longitude),
                             Distance.FromKilometers(1)); // Adjust the desired radius for the map view
                mappy.MoveToRegion(mapSpan);
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
    }

    private async Task SetFirstLocation()
    {
        
        Location location = await Geolocation.Default.GetLastKnownLocationAsync();

        if (location != null)
        {
            var mapSpan = MapSpan.FromCenterAndRadius(
                new Location(location.Latitude, location.Longitude),
                Distance.FromKilometers(1)); // Adjust the desired radius for the map view

            mappy.MoveToRegion(mapSpan);
        }
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        string locationName = mapSearchBar.Text;

        var locations = await Geocoding.GetLocationsAsync(locationName);
        Location Searchedlocation = locations?.FirstOrDefault();

        if (Searchedlocation != null)
        {
            m_WishedLocation = Searchedlocation;

            if (m_RadiusCircle.Center is not null)
            {
                mappy.MapElements.Remove(m_RadiusCircle);
            }

            if (m_WishedLocationPin.Location is not null)
            {
                mappy.Pins.Remove(m_WishedLocationPin);
            }

            m_WishedLocationPin.Location = Searchedlocation;

            var mapSpan = MapSpan.FromCenterAndRadius(
                new Location(Searchedlocation.Latitude, Searchedlocation.Longitude),
                Distance.FromKilometers(0.2)); // Adjust the desired radius for the map view

            mappy.MoveToRegion(mapSpan);
            mappy.Pins.Add(m_WishedLocationPin);
        }

    }

    private void OnInhanceRadiusTapped(object sender, TappedEventArgs args)
    {
        if(m_WishedRadius == 1000 || m_WishedLocation is null)
        {
            return;
        }

        m_RadiusCircle.Center = m_WishedLocation;
        if (m_WishedRadius == 0)
        {
            mappy.MapElements.Add(m_RadiusCircle);
        }

        m_WishedRadius += 50;
        m_RadiusCircle.Radius = new Distance(m_WishedRadius);
    }

    void OnDecreaseRadiusTapped(System.Object sender, TappedEventArgs e)
    {
        if (m_WishedRadius == 0)
        {
            return;
        }

        m_RadiusCircle.Center = m_WishedLocation;
        if (m_WishedRadius == 0)
        {
            mappy.MapElements.Add(m_RadiusCircle);
        }

        m_WishedRadius -= 50;
        m_RadiusCircle.Radius = new Distance(m_WishedRadius);
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        m_WishedLocation = e.Location;

        if(m_RadiusCircle.Center is not null)
        {
            mappy.MapElements.Remove(m_RadiusCircle);
            m_WishedRadius = 0;
        }
        
        if (m_WishedLocationPin.Location is not null)
        {
            mappy.Pins.Remove(m_WishedLocationPin);
        }

        m_WishedLocationPin.Location = m_WishedLocation;
        mappy.Pins.Add(m_WishedLocationPin);

    }

    private async void SetQuestionTapped(object sender, TappedEventArgs args)
    {
        if (m_WishedLocation == null)
        {
            var location = await Geolocation.GetLocationAsync();
            m_WishedLocation = new Location(location.Latitude, location.Longitude);
        }
        string address = await GetLocationAdress();
        var navigationParameter = new Dictionary<string, object>
        {
            ["Location"] = m_WishedLocation,
            ["Address"] = address,
            ["Radius"] = m_WishedRadius
        };
        await Shell.Current.GoToAsync($"{nameof(SetQuestionPage)}", navigationParameter);
    }

    private async Task<string> GetLocationAdress()
    {
        string locationAdress = null;
        try
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(m_WishedLocation.Latitude, m_WishedLocation.Longitude);
            if (placemarks != null && placemarks.Any())
            {
                var placemark = placemarks.FirstOrDefault();
                locationAdress = $"{placemark.Thoroughfare}, {placemark.Locality}, {placemark.CountryName}";
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("hey");
        }


        return locationAdress;
    }
}


  