using System;
using System.Drawing;
using System.Net;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using Google.Maps;

namespace BusFinder
{
	public class GoogleMaps
	{
		private const string key = "AIzaSyDKgNWb2yMvIBswmoqUL4RfW99dWbWrxQc";

		public GoogleMaps ()
		{

		}

		public void ProvideApiKey()
		{
			MapServices.ProvideAPIKey (key);
		}

		public MapView GetMapView(float lat, float lon, int z, bool myLocation)
		{
			var camera = CameraPosition.FromCamera (latitude: lat, longitude: lon, zoom: z);
			var mapView = MapView.FromCamera (RectangleF.Empty, camera);
			mapView.MyLocationEnabled = true;
			mapView.Settings.MyLocationButton = true;
			mapView.Settings.RotateGestures = false;
			mapView.SetMinMaxZoom (11, 20);
			return mapView;
		}

		public void AddMarker(double lat, double lon, UIColor color, Data data)
		{
			var position = new CLLocationCoordinate2D (lat, lon);
			var marker = Marker.FromPosition (position);
			marker.Title = "Bus #" + data.vehicle_id;
			marker.Snippet = "Updated as of " + data.date_time + "\n";
			marker.Snippet += "On route #" + data.current_route_id + "\n";
			marker.Snippet += "Bearing " + data.direction + "\n";
			marker.Snippet += "Coordinates " + Math.Round (data.lat, 2).ToString () + "," + Math.Round (data.lon, 2).ToString ();
			marker.AppearAnimation = MarkerAnimation.Pop;

			marker.Icon = UIImage.FromFile ("bus.png");
			marker.Flat = false;
			marker.Map = BusFinderViewController.mapView;
		}

		public void AddBus(double lat, double lon)
		{
			var position = new CLLocationCoordinate2D (lat, lon);
			var bus = Circle.FromPosition (position, 150f);
			bus.Title = "It Worked!";
			bus.FillColor = UIColor.FromRGB (0f, 0f, 1f);
			bus.StrokeColor = UIColor.FromRGB (0f, 0f, 0f);
			bus.StrokeWidth = 1f;
			bus.Map = BusFinderViewController.mapView;
		}
	}
}

