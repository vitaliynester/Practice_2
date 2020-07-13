using GMap.NET;
using GMap.NET.MapProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Pract
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                IPHostEntry e = Dns.GetHostEntry("www.google.com");
                var items = getItems();
                var sortedItems = items.OrderBy(o => o.City).ToList();
                writeToFile(in sortedItems);
                listView_items.Items.Clear();
                foreach (var item in sortedItems)
                {
                    listView_items.Items.Add(new CustomListViewItem(new JsonResponse
                    {
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                        Adress = item.Adress,
                        Place = item.Place,
                        City = item.City
                    }));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Для работы приложения необходим интернет", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            mapView.MapProvider = GMapProviders.GoogleHybridMap;
            mapView.MinZoom = 2;
            mapView.MaxZoom = 20;
            mapView.Zoom = 14;
            mapView.MouseWheelZoomType = MouseWheelZoomType.ViewCenter;
            mapView.CanDragMap = true;
            mapView.DragButton = MouseButton.Right;
            setNewPoint(52.60249885, 39.50675114199363);
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private List<JsonResponse> getItems()
        {
            WebRequest request = WebRequest.Create("https://api.privatbank.ua/p24api/infrastructure?json&tso&address=&city=");
            var response = request.GetResponse();
            var resonseStream = response.GetResponseStream();
            var reader = new StreamReader(resonseStream);
            var data = reader.ReadToEnd();
            reader.Close();
            var innerJson = JObject.Parse(data)["devices"].ToString();
            var jsonData = JsonConvert.DeserializeObject<List<JsonResponse>>(innerJson);
            fixCityName(ref jsonData);
            return jsonData;
        }

        private void listView_items_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (CustomListViewItem)listView_items.SelectedItem;
            if (item != null)
            {
                var json = item.JsonResponse;
                setNewPoint(json.Longitude, json.Latitude);
            }
        }

        private void setNewPoint(double lat, double lng)
        {
            var point = new PointLatLng(lat, lng);
            mapView.Position = point;
        }

        private void fixCityName(ref List<JsonResponse> jsons)
        {
            foreach (var item in jsons)
            {
                if (item.City[0] == ' ')
                {
                    item.City = item.City.Replace(" ", string.Empty);
                }
            }
            jsons = jsons.Distinct().ToList();
        }

        private void writeToFile(in List<JsonResponse> jsons)
        {
            string fileName = "adress";
            using (StreamWriter writeText = new StreamWriter(File.Open($"{fileName}_UTF8.txt", FileMode.Create), System.Text.Encoding.UTF8))
            {
                string line = string.Empty;
                foreach (var item in jsons)
                {
                    line = $"Город: {item.City} Местоположение: {item.Place} Адрес: {item.Adress} Широта: {item.Latitude} Долгота: {item.Longitude}";
                    writeText.WriteLine(line);
                }
            }
            using (StreamWriter writeText = new StreamWriter(File.Open($"{fileName}_UTF16.txt", FileMode.Create), System.Text.Encoding.Unicode))
            {
                string line = string.Empty;
                foreach (var item in jsons)
                {
                    line = $"Город: {item.City} Местоположение: {item.Place} Адрес: {item.Adress} Широта: {item.Latitude} Долгота: {item.Longitude}";
                    writeText.WriteLine(line);
                }
            }
        }
    }
}
