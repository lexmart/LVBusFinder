using System;
using System.Drawing;
using System.Net;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Google.Maps;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusFinder
{
	public partial class BusFinderViewController : UIViewController
	{
		public static float camera_lat = 36.166705f;
		public static float camera_lon = -115.179291f;

		public static MapView mapView;

		private BusData busData;
		private GoogleMaps googleMaps;
		private UILabel title;
		private UINavigationBar bar;
		private UIButton refresh;
		private UILabel refreshText;

		private float width, height;

		public BusFinderViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			width = UIScreen.MainScreen.Bounds.Width;
			height = UIScreen.MainScreen.Bounds.Height;

			busData = new BusData ();

			googleMaps = new GoogleMaps ();
			googleMaps.ProvideApiKey ();

			BusFinderViewController.mapView = googleMaps.GetMapView (camera_lat, camera_lon, 11, true);
			View = mapView;

			GenerateUI ();

			RefreshClicked ();
		}

		public 

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion

		public void GenerateUI () {
			bar = new UINavigationBar (new RectangleF(0, 0, 640, 70));
			View.Add (bar);

			title = new UILabel (new RectangleF (0, 13, width, 50));
			title.Text = "Las Vegas Bus Finder";
			title.TextAlignment = UITextAlignment.Center;
			View.Add (title);

			refresh = new UIButton (UIButtonType.Custom);
			UIImage refreshIcon = UIImage.FromFile ("refresh.png");
			refresh.SetImage (refreshIcon, UIControlState.Normal);
			refresh.Frame = new RectangleF (width - 50, 25, 35, 35);
			refresh.TouchUpInside += delegate {
				RefreshClicked();
			};
			View.Add (refresh);

			refreshText = new UILabel (new RectangleF (0, 30, width, 50));
			refreshText.Text = "Refreshed";
			refreshText.TextAlignment = UITextAlignment.Center;
			refreshText.Font = UIFont.FromName("Helvetica", 12f);
			View.Add (refreshText);
		}

		async void RefreshClicked() {
			refreshText.Text = "Refreshing...";
			mapView.Clear ();
			List<Data> data = await busData.GetBusData ();
			foreach (Data element in data) {
				googleMaps.AddMarker (element.lat, element.lon, UIColor.FromRGB(0, 0, 0), element);
			}

			refreshText.Text = "Refreshed at " + busData.GetTimeNowString ();
		}
	}
}

