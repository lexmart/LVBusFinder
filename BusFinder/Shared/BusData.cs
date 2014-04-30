using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Json;
using System.Text.RegularExpressions;

namespace BusFinder
{
	public class BusData
	{
		private int pageMin = 201, pageMax = 219;

		public BusData ()
		{

		}

		public async void UnitTest() 
		{
			string html = await GetHTML (GenerateURL(201));
			List<Data> data = GetBusDataFromHTML (html);
			for (int i = 0; i < data.Count; i++) {
				Console.WriteLine (data[i].vehicle_id + ", " + data[i].direction + ", " + data[i].lat.ToString() + ", " + data[i].lon.ToString());
			}
		}

		public async Task<List<Data>> GetBusData() {
			List<Data> data = new List<Data> ();
			for (int i = pageMin; i <= pageMax; i++) {
				string html = await GetHTML (GenerateURL (i));
				List<Data> subData = GetBusDataFromHTML (html);
				foreach (Data element in subData) {
					data.Add (element);
				}
			}
			return data;
		}

		public string GenerateURL(int id)
		{
			return String.Format ("http://rtcws.rtcsnv.com/trak.cfc?method=getVehiclesOnRoute&returnFormat=json&rte={0}&vdr=3", id.ToString());
		}

		public List<Data> GetBusDataFromHTML(string html)
		{
			List<Data> data = new List<Data> ();

			JsonValue json = JsonValue.Parse (html);

			for (int i = 0; i < json ["DATA"].Count; i++) {
				Data row = new Data(
					Regex.Replace(json["DATA"][i][0].ToString(), "\"", ""),
					Regex.Replace(json["DATA"][i][1].ToString(), "\"", ""),
					Regex.Replace(json["DATA"][i][2].ToString(), "\"", ""),
					Regex.Replace(json["DATA"][i][3].ToString(), "\"", ""),
					Regex.Replace(json["DATA"][i][4].ToString(), "\"", ""),
					double.Parse((json["DATA"][i][5].ToString().Replace('"',' '))),
					double.Parse(json["DATA"][i][6].ToString().Replace('"',' '))
				);

				data.Add (row);
			}

			return data;
		}

		private async Task<string> GetHTML(string url) 
		{
			var httpClient = new HttpClient ();
			Task<string> htmlTask = httpClient.GetStringAsync (url);
			return await htmlTask; 
		}

		public string GetTimeNowString() 
		{
			int hour = DateTime.Now.Hour;
			int minute = DateTime.Now.Minute;

			string hour_string = "";
			string minute_string = "";

			if (minute < 10) {
				minute_string = "0" + minute.ToString ();
			} else {
				minute_string = minute.ToString ();
			}

			if (hour >= 13) {
				hour_string = (hour - 12).ToString ();
			} else {
				hour_string = hour.ToString ();
			}

			return hour_string + ":" + minute_string;
		}
	}

	public class Data 
	{
		public string vehicle_id;
		public string current_route_id;
		public string run_id;
		public string date_time;
		public string direction;
		public double lon;
		public double lat;

		public Data(string vehicle_id, string current_route_id, string run_id, string date_time, string direction, double lon, double lat) {
			this.vehicle_id = vehicle_id;
			this.current_route_id = current_route_id;
			this.run_id = run_id;
			this.date_time = date_time;
			this.direction = direction;
			this.lat = lat;
			this.lon = lon;
		}
	}
}

