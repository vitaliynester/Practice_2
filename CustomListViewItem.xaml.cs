using System.Windows.Controls;

namespace Pract
{
    /// <summary>
    /// Логика взаимодействия для CustomListViewItem.xaml
    /// </summary>
    public partial class CustomListViewItem : UserControl
    {
        public JsonResponse JsonResponse { get; set; }
        public CustomListViewItem()
        {
            InitializeComponent();
        }
        public CustomListViewItem(JsonResponse item)
        {
            InitializeComponent();
            JsonResponse = item;
            tb_latitude.Text = $"Ширина: {item.Latitude}";
            tb_longitude.Text = $"Долгота: {item.Longitude}";
            tb_place.Text = $"Местоположение: {item.Place}";
            tb_city.Text = $"Город: {item.City}";
            var adress = item.Adress.Split(',');
            string line = string.Empty;
            int index = adress.Length - 1;
            foreach (var adr in adress)
            {
                if (adr != adress[index])
                {
                    line += $"{adr}\n";
                }
                else
                {
                    line += $"{adr}";
                }
            }
            tb_adress.Text = $"Адрес: {line}";
        }
    }
}
