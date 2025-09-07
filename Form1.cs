using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WeatherForecastApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private string LoadApiKey()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api_key.txt");

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath).Trim();
        }
        else
        {
            throw new Exception("API key file not found. Please create api_key.txt in the project folder.");
        }
    }
    
        private async void btnGetWeather_Click(object sender, EventArgs e)
        {
            string city = txtCity.Text;

            if (string.IsNullOrWhiteSpace(city))
            {
                lblResult.Text = "Please enter a city name.";
                return;
            }

            try
            {
                string json = await GetWeatherDataAsync(city);
                string result = ParseWeather(json);
                lblResult.Text = result;
            }
            catch (Exception ex)
            {
                lblResult.Text = $"Error: {ex.Message}";
            }
        }

        public async Task<string> GetWeatherDataAsync(string city)
        {
            string apiKey = LoadApiKey(); // 👈 Load from file
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }


        public string ParseWeather(string json)
        {
            JObject data = JObject.Parse(json);
            string description = data["weather"][0]["description"].ToString();
            string temp = data["main"]["temp"].ToString();
            return $"Temperature: {temp}°C\nCondition: {description}";
        }
    }
}
