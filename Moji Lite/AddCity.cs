using KlxPiaoAPI;
using KlxPiaoControls;

namespace Moji_Lite
{
    public partial class AddCity : KlxPiaoForm
    {
        private readonly MainWindow mainWindow;

        private const string requestLink = "https://tianqi.moji.com/weather/china";
        private const string linkPrefix = "https://tianqi.moji.com/";

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
            citiesListBox.DrawItem += CitiesListBox_DrawItem;
            mainWindow = parentForm;

            TitleBoxBackColor = mainWindow.TitleBoxBackColor;
            TitleBoxForeColor = mainWindow.TitleBoxForeColor;
            citiesListBox.DrawMode = DrawMode.OwnerDrawFixed;
            citiesListBox.ItemHeight = 20;
        }

        private void CitiesListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            var itemText = citiesListBox.Items[e.Index].ToString();
            Font? font = e.Font;
            if (font != null)
            {
                var textSize = e.Graphics.MeasureString(itemText, font);
                var textPos = LayoutUtilities.CalculateAlignedPosition(e.Bounds, textSize, ContentAlignment.MiddleCenter);

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 220, 220)), e.Bounds);
                    e.Graphics.DrawString(itemText, font, Brushes.Black, textPos.X, textPos.Y);
                }
                else
                {
                    e.Graphics.DrawString(itemText, font, Brushes.Black, textPos.X, textPos.Y);
                }
                e.DrawFocusRectangle();
            }
        }

        //重新加载列表
        private void RefreshLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists(mainWindow.cacheCitiesFile)) File.Delete(mainWindow.cacheCitiesFile);
            if (File.Exists(mainWindow.cacheCityLinksFile)) File.Delete(mainWindow.cacheCityLinksFile);

            CloseForm();
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
                mainWindow.citiesListBox.Items.Add(key.TruncateStringToFitWidth(mainWindow.citiesListBox.Width, mainWindow.citiesListBox.Font));
            }
        }

        private async void AddCity_Load(object? sender, EventArgs e)
        {
            searchTextBox.Clear();

            cts = new();
            var token = cts.Token;

            if (File.Exists(mainWindow.cacheCitiesFile)
                && File.Exists(mainWindow.cacheCityLinksFile)
                && File.ReadAllLines(mainWindow.cacheCitiesFile).Length == File.ReadAllLines(mainWindow.cacheCityLinksFile).Length)
            {
                //读取缓存
                cityData = DataUtility.MergeListsToDictionary([.. File.ReadAllLines(mainWindow.cacheCitiesFile)], [.. File.ReadAllLines(mainWindow.cacheCityLinksFile)]);
            }
            else
            {
                tipLabel.Text = $"正在获取基础数据...";
                (bool IsSuccess, string originalText, string failMessage) = await NetworkOperations.TryGetHTMLContentAsync(requestLink);
                if (!IsSuccess)
                {
                    tipLabel.Text = "加载失败";
                    mainWindow.ShowMessage($"请求失败：{requestLink}\r\n错误消息：{failMessage}", Application.ProductName);
                    return;
                }
                List<string> provinceOriginalList = originalText.ExtractBetween("<div class=\"city_title\">全部省份</div>", "</div>").ExtractAllBetween("<li>", "</li>");
                List<string> provinceList = provinceOriginalList.Select(item => item.ExtractBetween(">", "<")).ToList();
                List<string> provinceLinkList = provinceOriginalList.Select(item => linkPrefix + item.ExtractBetween("<a href=\"", "\"")).ToList();
                Dictionary<string, string> provinceDataList = DataUtility.MergeListsToDictionary(provinceList, provinceLinkList);

                List<string> cityList = [];
                List<string> cityLinkList = [];

                for (int i = 0; i < provinceDataList.Count; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    tipLabel.Text = $"正在加载({(float)i / (provinceDataList.Count - 1) * 100:F0}%)...";

                    string provinceName = provinceDataList.ToList()[i].Key;
                    string provinceLink = provinceDataList.ToList()[i].Value;
                    (bool IsSuccess, string content, string failMessage) cityRequest = await NetworkOperations.TryGetHTMLContentAsync(provinceLink);
                    if (!cityRequest.IsSuccess)
                    {
                        tipLabel.Text = "加载失败";
                        mainWindow.ShowMessage($"请求失败：{provinceLink}\r\n错误消息：{cityRequest.failMessage}", Application.ProductName);
                        return;
                    }
                    List<string> cityOriginalList = cityRequest.content.ExtractBetween("<div class=\"city_hot\">", "</div>").ExtractAllBetween("<li>", "</li>", MatchMode.StringIndex);
                    cityList.AddRange(cityOriginalList.Select(item => provinceName + " " + item.ExtractBetween(">", "<")).ToList());
                    cityLinkList.AddRange(cityOriginalList.Select(item => item.ExtractBetween("href=\"", "\"")).ToList());
                }
                tipLabel.Text = "";

                cityData = DataUtility.MergeListsToDictionary(cityList, cityLinkList);

                //缓存数据
                File.WriteAllLines(mainWindow.cacheCitiesFile, cityData.Keys.ToList());
                File.WriteAllLines(mainWindow.cacheCityLinksFile, cityData.Values.ToList());
            }

            //更新列表
            citiesListBox.Items.Clear();
            citiesListBox.Items.AddRange([.. cityData.Keys]);

            tipLabel.Text = $"已加载城市：{citiesListBox.Items.Count}";
        }
    }
}