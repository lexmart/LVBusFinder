using System;
using System.Drawing;
using Google.Maps;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;

namespace BusFinder
{
	public class GoogleMaps
	{
		private const string key = "AIzaSyDKgNWb2yMvIBswmoqUL4RfW99dWbWrxQc";
		private MapView mapView;

		public GoogleMaps()
		{

		}

		public void ProvideKey() {
			MapServices.ProvideAPIKey (key);
		}

		public void LoadMap(UIView view) {
			var camera = CameraPosition.FromCamera (latitude: 37.797865, 
				longitude: -122.402526, 
				zoom: 6);
			mapView = MapView.FromCamera (RectangleF.Empty, camera);
			mapView.MyLocationEnabled = true;
			view = mapView;
		}

		public void StartRendering() {
			mapView.StartRendering ();
		}

		public void StopRendering() {
			mapView.StopRendering ();
		}
	}
}

