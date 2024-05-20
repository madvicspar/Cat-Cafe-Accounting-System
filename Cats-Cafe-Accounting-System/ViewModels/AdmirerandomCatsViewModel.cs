using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class AdmirerandomCatsViewModel : ObservableObject
    {
        public BitmapImage Image { get; set; }
        public ICommand GetNewCommand { get; set; }

        public AdmirerandomCatsViewModel()
        {
            GetNewCommand = new RelayCommand(async () => await ExecuteGetNewCommand());
        }

        private async Task ExecuteGetNewCommand()
        {
            string apiKey = ConfigurationManager.AppSettings["cats-api-key"];
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);
                HttpResponseMessage response = await client.GetAsync("https://api.thecatapi.com/v1/images/search");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var imageUrls = JArray.Parse(responseData);
                    if (imageUrls.Count > 0)
                    {
                        string imageUrl = imageUrls[0]["url"].ToString();
                        byte[] imageData = await client.GetByteArrayAsync(imageUrl);

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(imageData);
                        bitmap.EndInit();

                        Image = bitmap; // Здесь обращаемся к свойству экземпляра
                        OnPropertyChanged(nameof(Image));
                    }
                }
            }
        }
    }
}