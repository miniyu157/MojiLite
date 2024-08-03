using KlxPiaoAPI;
using KlxPiaoControls;
using System.Text;

namespace Moji_Lite
{
    public partial class AddCity : KlxPiaoForm
    {
        private readonly MainWindow mainWindow;

        private const string requestLink = "https://tianqi.moji.com/weather/china";
        private readonly string cacheCitiesFile = $"{Path.Combine(Application.StartupPath, "cache/Cities.cache")}";
        private readonly string cacheCityLinksFile = $"{Path.Combine(Application.StartupPath, "cache/CityLinks.cache")}";

        private Dictionary<string, string> cityData = [];
        private Dictionary<string, string> searchCityData = [];

        private CancellationTokenSource? cts;
        public AddCity(MainWindow parentForm)
        {
            InitializeComponent();

            Load += AddCity_Load;
            FormClosing += AddCity_FormClosing;
            citiesListBox.SelectedIndexChanged += CityListBox_SelectedIndexChanged;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            refreshLinkLabel.LinkClicked += RefreshLinkLabel_LinkClicked;
            mainWindow = parentForm;
        }

        //重新加载列表
        private void RefreshLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists(cacheCitiesFile))
            {
                File.Delete(cacheCitiesFile);
            }

            if (File.Exists(cacheCityLinksFile))
            {
                File.Delete(cacheCityLinksFile);
            }

            Close();
            mainWindow.addCityBut.PerformClick();
        }

        private void AddCity_FormClosing(object? sender, FormClosingEventArgs e)
        {
            cts?.Cancel();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (searchText != null)
            {
                searchCityData.Clear();
                cityData.ToList().ForEach(c =>
                {
                    if (c.Key.Contains(searchText) || c.Value.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                    {
                        searchCityData.Add(c.Key, c.Value);
                    }
                });

                citiesListBox.Items.Clear();
                citiesListBox.Items.AddRange([.. searchCityData.Keys]);
            }
            else
            {
                searchCityData = cityData;
            }
        }

        private void CityListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int index = citiesListBox.SelectedIndex;

            var key = searchCityData.Count == 0 ? cityData.Keys.ToList()[index] : searchCityData.Keys.ToList()[index];
            var value = searchCityData.Count == 0 ? cityData.Values.ToList()[index] : searchCityData.Values.ToList()[index];

            if (mainWindow.SaveCities.TryAdd(key, value))
            {
                mainWindow.citiesListBox.Items.Add(key);
            }
        }

        private async void AddCity_Load(object? sender, EventArgs e)
        {
            searchTextBox.Clear();

            cts = new();
            var token = cts.Token;

            if (File.Exists(cacheCitiesFile)
                && File.Exists(cacheCityLinksFile)
                && File.ReadAllLines(cacheCitiesFile).Length == File.ReadAllLines(cacheCityLinksFile).Length)
            {
                //读取缓存
                cityData = DataUtility.MergeListsToDictionary([.. File.ReadAllLines(cacheCitiesFile)], [.. File.ReadAllLines(cacheCityLinksFile)]);
            }
            else
            {
                topLabel.Text = $"正在获取基础数据...";
                (bool IsSuccess, string originalText, string failMessage) = await NetworkOperations.TryGetHTMLContentAsync(requestLink);
                if (!IsSuccess)
                {
                    MessageBox.Show($"请求失败：{requestLink}\r\n错误消息：{failMessage}", Application.ProductName);
                    Close();
                    return;
                }
                List<string> provinceOriginalList = originalText.ExtractBetween("<div class=\"city_title\">全部省份</div>", "</div>").ExtractAllBetween("<li>", "</li>");
                List<string> provinceList = provinceOriginalList.Select(item => item.ExtractBetween(">", "<")).ToList();
                List<string> provinceLinkList = provinceOriginalList.Select(item => "https://tianqi.moji.com/" + item.ExtractBetween("<a href=\"", "\"")).ToList();
                Dictionary<string, string> provinceDataList = DataUtility.MergeListsToDictionary(provinceList, provinceLinkList);

                List<string> cityList = [];
                List<string> cityLinkList = [];

                for (int i = 0; i < provinceDataList.Count; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    topLabel.Text = $"正在加载({(float)i / (provinceDataList.Count - 1) * 100:F0}%)...";

                    string provinceName = provinceDataList.ToList()[i].Key;
                    string provinceLink = provinceDataList.ToList()[i].Value;
                    (bool IsSuccess, string content, string failMessage) cityRequest = await NetworkOperations.TryGetHTMLContentAsync(provinceLink);
                    if (!cityRequest.IsSuccess)
                    {
                        MessageBox.Show($"请求失败：{provinceLink}\r\n错误消息：{cityRequest.failMessage}", Application.ProductName);
                        Close();
                        return;
                    }
                    List<string> cityOriginalList = cityRequest.content.ExtractBetween("<div class=\"city_hot\">", "</div>").ExtractAllBetween("<li>", "</li>", MatchMode.StringIndex);
                    cityList.AddRange(cityOriginalList.Select(item => provinceName + " " + item.ExtractBetween(">", "<")).ToList());
                    cityLinkList.AddRange(cityOriginalList.Select(item => item.ExtractBetween("href=\"", "\"")).ToList());
                }
                topLabel.Text = "";

                cityData = DataUtility.MergeListsToDictionary(cityList, cityLinkList);

                //缓存数据
                StringBuilder citiesSb = new();
                StringBuilder cityLinksSb = new();
                cityData.ToList().ForEach(item =>
                {
                    citiesSb.AppendLine(item.Key);
                    cityLinksSb.AppendLine(item.Value);
                });
                using (var streamWriter = new StreamWriter(cacheCitiesFile))
                {
                    streamWriter.Write(citiesSb.ToString());
                }
                using (var streamWriter = new StreamWriter(cacheCityLinksFile))
                {
                    streamWriter.Write(cityLinksSb.ToString());
                }
            }

            //更新列表
            citiesListBox.Items.Clear();
            citiesListBox.Items.AddRange([.. cityData.Keys]);

            topLabel.Text = $"已加载城市：{citiesListBox.Items.Count}";
        }
    }
}