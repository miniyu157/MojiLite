using KlxPiaoAPI;
using KlxPiaoControls;

namespace Moji_Lite
{
    public partial class AddCity : KlxPiaoForm
    {
        private readonly MainWindow mainWindow;

        private const string requestLink = "https://tianqi.moji.com/weather/china";
        private const string linkPrefix = "https://tianqi.moji.com/";
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
            citiesListBox.DrawItem += CitiesListBox_DrawItem;
            mainWindow = parentForm;

            citiesListBox.DrawMode = DrawMode.OwnerDrawFixed;
            citiesListBox.ItemHeight = 20;
        }

        private void CitiesListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            //清除背景
            e.DrawBackground();

            //设置绘制区域
            var itemText = citiesListBox.Items[e.Index].ToString();
            Font? font = e.Font;
            if (font != null)
            {
                var textSize = e.Graphics.MeasureString(itemText, font);

                //计算文本的居中位置
                var textX = e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2;
                var textY = e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2;

                //如果是选中项，设置背景色为蓝色，前景色为绿色
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 220, 220)), e.Bounds); //设置背景色
                    e.Graphics.DrawString(itemText, font, Brushes.Black, textX, textY); //设置前景色
                }
                else
                {
                    e.Graphics.DrawString(itemText, font, Brushes.Black, textX, textY); //默认前景色
                }

                //绘制焦点矩形（如果需要）
                e.DrawFocusRectangle();
            }
        }

        //重新加载列表
        private void RefreshLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists(cacheCitiesFile)) File.Delete(cacheCitiesFile);
            if (File.Exists(cacheCityLinksFile)) File.Delete(cacheCityLinksFile);

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

            if (File.Exists(cacheCitiesFile)
                && File.Exists(cacheCityLinksFile)
                && File.ReadAllLines(cacheCitiesFile).Length == File.ReadAllLines(cacheCityLinksFile).Length)
            {
                //读取缓存
                cityData = DataUtility.MergeListsToDictionary([.. File.ReadAllLines(cacheCitiesFile)], [.. File.ReadAllLines(cacheCityLinksFile)]);
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
                File.WriteAllLines(cacheCitiesFile, cityData.Keys.ToList());
                File.WriteAllLines(cacheCityLinksFile, cityData.Values.ToList());
            }

            //更新列表
            citiesListBox.Items.Clear();
            citiesListBox.Items.AddRange([.. cityData.Keys]);

            tipLabel.Text = $"已加载城市：{citiesListBox.Items.Count}";
        }
    }
}