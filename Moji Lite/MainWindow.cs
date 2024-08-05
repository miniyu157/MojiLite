using KlxPiaoAPI;
using KlxPiaoControls;
using System.Diagnostics;
using System.Reflection;

namespace Moji_Lite
{
    public partial class MainWindow : KlxPiaoForm
    {
        private const char FULL_WIDTH_SPACE = '　';
        private const char EN_SPACE = ' ';
        private const string githubLink = "https://github.com/miniyu157/MojiLite";
        private readonly string citiesFile = $"{Path.Combine(Application.StartupPath, "cache/Cities.dat")}";
        private readonly string cityLinksFile = $"{Path.Combine(Application.StartupPath, "cache/CityLinks.dat")}";
        private readonly string cachePath = $"{Path.Combine(Application.StartupPath, "cache")}";

        internal Dictionary<string, string> SaveCities { get; set; } = [];

        public MainWindow()
        {
            InitializeComponent();

            Text = $"{Application.ProductName} {GetProductVersion()}";
            Icon? appicon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appicon != null)
            {
                Icon = appicon.ResetImage(new Size(20, 20));
            }

            addCityBut.Click += AddCityBut_Click;
            delCityBut.Click += DelCityBut_Click;
            citiesListBox.SelectedIndexChanged += CitiesListBox_SelectedIndexChanged;
            Load += MainWindow_Load;
            FormClosed += MainWindow_FormClosed;
            SizeChanged += MainWindow_SizeChanged;
            richTextBox1.LinkClicked += RichTextBox1_LinkClicked;
            panel1.SizeChanged += Panel1_SizeChanged;
            panel2.SizeChanged += Panel2_SizeChanged;
            citiesListBox.DrawItem += CitiesListBox_DrawItem;

            richTextBox1.SelectionIndent = 8;
            citiesListBox.DrawMode = DrawMode.OwnerDrawFixed;
            citiesListBox.ItemHeight = 30;
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

        private static string? GetProductVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            AssemblyInformationalVersionAttribute? productVersion =
                (AssemblyInformationalVersionAttribute?)Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute));

            if (productVersion?.InformationalVersion is string versionStr)
            {
                var plusSymbolIndex = versionStr.IndexOf('+');
                if (plusSymbolIndex != -1)
                {
                    versionStr = versionStr[..plusSymbolIndex];
                }

                return versionStr;
            }

            return "Unknown Version";
        }

        private void CreateCacheFolder()
        {
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
        }

        #region 适应界面大小
        private void Panel2_SizeChanged(object? sender, EventArgs e)
        {
            citiesListBox.Height = panel2.Height - 10;
        }

        private void Panel1_SizeChanged(object? sender, EventArgs e)
        {
            richTextBox1.Size = panel1.Size - new Size(6, 6);
        }

        private void MainWindow_SizeChanged(object? sender, EventArgs e)
        {
            panel1.Height = Height - panel1.Top - 12;
            panel2.Height = Height - panel2.Top - 12;
            panel1.Width = Width - panel1.Left - citiesListBox.Width - 24;
            panel2.Left = Width - citiesListBox.Width - panel1.Left - 2;
            delCityBut.Left = panel2.Left - 1;
            addCityBut.Left = delCityBut.Left + delCityBut.Width + 8;
        }
        #endregion

        private static void RichTextBox1_LinkClicked(object? sender, LinkClickedEventArgs e)
        {
            if (e.LinkText != null)
            {
                string url = e.LinkText;
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }

        private async void CitiesListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int index = citiesListBox.SelectedIndex;
            int count = citiesListBox.Items.Count;
            if (count > 0 && index > -1)
            {
                richTextBox1.Text = "正在加载...";

                string link = SaveCities.Values.ToList()[index];
                string link15 = link.Replace("https://tianqi.moji.com/weather/", "https://tianqi.moji.com/forecast15/");

                (bool IsSuccess, string originalText, string failMessage) = await NetworkOperations.TryGetHTMLContentAsync(link);
                if (!IsSuccess)
                {
                    richTextBox1.Text = "加载失败";
                    ShowMessage($"请求失败：{link}\r\n错误消息：{failMessage}", Application.ProductName);
                    return;
                }

                (bool IsSuccess15, string originalText15, string failMessage15) = await NetworkOperations.TryGetHTMLContentAsync(link15);
                if (!IsSuccess15)
                {
                    richTextBox1.Text = "加载失败"; ;
                    ShowMessage($"请求失败：{link15}\r\n错误消息：{failMessage15}", Application.ProductName);
                    return;
                }

                string _data0 = originalText.ExtractBetween("<div class=\"search_default\">", "</div>");
                string location = _data0.ExtractBetween("<em>", "</em>").Replace(" ", "");

                string _data1 = originalText.ExtractBetween("<div class=\"wea_weather clearfix\">", "</div>");
                string temperature = _data1.ExtractBetween("<em>", "</em>");
                string icon = _data1.ExtractBetween("<img src=\"", "\"");
                string weather = _data1.ExtractBetween("<b>", "</b>");
                string updateTime = _data1.ExtractBetween("<strong class=\"info_uptime\">", "</strong>");

                string _data2 = originalText.ExtractBetween("<div class=\"wea_about clearfix\">", "</div>");
                string humidity = _data2.ExtractBetween("<span>", "</span>");
                string windSpeed = _data2.ExtractBetween("<em>", "</em>");

                string _data3 = originalText.ExtractBetween("<div class=\"wea_tips clearfix\">", "</div>");
                string tip = _data3.ExtractBetween("<em>", "</em>");

                string _data4 = originalText15.ExtractBetween("<div class=\"wea_list clearfix\">", "<div class=\"next\"></div>");

                List<string> _data4_date = _data4.ExtractAllBetween("<span class=\"week\">", "</span>");
                List<string> week = _data4_date.Where((item, index) => index % 2 == 0).ToList();
                List<string> day = _data4_date.Where((item, index) => index % 2 == 1).ToList();

                List<string> _data4_weather = _data4.ExtractAllBetween("<span class=\"wea\">", "</span>");
                List<string> morningWeather = _data4_weather.Where((item, index) => index % 2 == 0).ToList();
                List<string> eveningWeather = _data4_weather.Where((item, index) => index % 2 == 1).ToList();
                //获取最长文本用于对其列表
                int longMorningWeatherLength = morningWeather.OrderByDescending(s => s.Length).First().Length;
                int longEveningWeatherLength = eveningWeather.OrderByDescending(s => s.Length).First().Length;
                morningWeather = morningWeather.Select(item2 => { return item2.PadRight(longMorningWeatherLength, FULL_WIDTH_SPACE) + ' '; }).ToList();
                eveningWeather = eveningWeather.Select(item2 => { return item2.PadRight(longEveningWeatherLength, FULL_WIDTH_SPACE) + ' '; }).ToList();

                List<string> _data4_icon = _data4.ExtractAllBetween("src=\"", "\"");
                List<string> morningIcon = _data4_icon.Where((item, index) => index % 2 == 0).ToList();
                List<string> eveningIcon = _data4_icon.Where((item, index) => index % 2 == 1).ToList();

                List<string> _data4_temperature = _data4.ExtractAllBetween("<div class=\"tree clearfix\">", "</div>", MatchMode.StringIndex);
                List<string> morningTemperature = _data4_temperature.Select(item => item.ExtractBetween("<b>", "</b>")).ToList();
                List<string> eveningTemperature = _data4_temperature.Select(item => item.ExtractBetween("<strong>", "</strong>")).ToList();

                async Task RichTextBoxAddImage(RichTextBox richTextBox, string url, string cachePath, int newWidth, int newHeight)
                {
                    try
                    {
                        string cacheFileName = Path.Combine(cachePath, Path.GetFileName(url));
                        //从缓存或互联网加载图片
                        Bitmap showImage = File.Exists(cacheFileName)
                            ? new(Image.FromFile(cacheFileName))
                            : await NetworkOperations.GetImageFromUrlAsync(url, cacheFileName);
                        Clipboard.SetImage(showImage
                            .ReplaceColor(Color.White, Color.FromArgb(237, 237, 237), false)
                            .ResetImage(new Size(newWidth, newHeight), Color.White));
                        richTextBox.Paste();
                        Clipboard.Clear();
                    }
                    catch
                    {
                        richTextBox1.Text = "加载失败";
                        ShowMessage($"图片加载失败：{url}\r\n错误消息：{failMessage}", Application.ProductName);
                        return;
                    }
                }
                using (RichTextBox richTextBox = new())
                {
                    using Bitmap bitmap = new(1, 1);
                    using Graphics g = Graphics.FromImage(bitmap);

                    richTextBox.Font = richTextBox1.Font;
                    richTextBox.SelectionIndent = richTextBox1.SelectionIndent;

                    Color menuColor = Color.FromArgb(69, 176, 225);
                    Color dayColor = Color.FromArgb(119, 32, 109);
                    Color todayColor = Color.Red;
                    Color temperatureColor = Color.FromArgb(241, 169, 131);
                    float temperatureFontSize = 42;
                    Size smallIconSize = new(30, 30);
                    int bigIconWidth = (int)g.MeasureString(weather, new Font(richTextBox.Font.FontFamily, temperatureFontSize)).Height;
                    Size bigIconSize = new(bigIconWidth, bigIconWidth);

                    richTextBox.InsertText("位置: ", menuColor);
                    richTextBox.AppendText($"{location}\r\n");

                    richTextBox.InsertText("温度: \r\n", menuColor);
                    await RichTextBoxAddImage(richTextBox, icon, cachePath, bigIconSize.Width, bigIconSize.Height);
                    richTextBox.InsertText($"{temperature}℃", temperatureColor, temperatureFontSize);
                    richTextBox.AppendText($"{weather}\r\n");

                    richTextBox.InsertText("更新: ", menuColor);
                    richTextBox.AppendText($"{updateTime}\r\n");

                    richTextBox.InsertText("湿度: ", menuColor);
                    richTextBox.AppendText($"{humidity}\r\n");

                    richTextBox.InsertText("风力: ", menuColor);
                    richTextBox.AppendText($"{windSpeed}\r\n");

                    richTextBox.InsertText("提示: ", menuColor);
                    richTextBox.AppendText($"{tip}\r\n");

                    richTextBox.InsertText("链接: ", menuColor);
                    richTextBox.AppendText($"{link}\r\n");

                    richTextBox.AppendText($"{new string('-', 300)}\r\n");
                    for (int i = 0; i < day.Count; i++)
                    {
                        if (i == 1)
                        {
                            richTextBox.InsertText($"{week[i]} {day[i]} ", todayColor);
                        }
                        else
                        {
                            richTextBox.InsertText($"{week[i]} {day[i]} ", dayColor);
                        }
                        richTextBox.AppendText($"{morningWeather[i]}({morningTemperature[i]})");
                        await RichTextBoxAddImage(richTextBox, morningIcon[i], cachePath, smallIconSize.Width, smallIconSize.Height);
                        richTextBox.AppendText($" / {eveningWeather[i]}({eveningTemperature[i]})");
                        await RichTextBoxAddImage(richTextBox, eveningIcon[i], cachePath, smallIconSize.Width, smallIconSize.Height);
                        richTextBox.AppendText("\r\n");
                    }
                    //若用户临时删除了列表，那么数据也临时不显示了
                    if (citiesListBox.Items.Count > 0)
                    {
                        richTextBox1.Rtf = richTextBox.Rtf;
                    }
                }
            }
            else
            {
                Welcome();
            }
        }

        private void AddCityBut_Click(object? sender, EventArgs e)
        {
            int oldIndex = citiesListBox.SelectedIndex;
            new AddCity(this).ShowDialog();
            if (citiesListBox.Items.Count > 0 && oldIndex == -1)
            {
                citiesListBox.SelectedIndex = 0;
            }
        }

        private void DelCityBut_Click(object? sender, EventArgs e)
        {
            int index = citiesListBox.SelectedIndex;
            var selectedItem = citiesListBox.SelectedItem?.ToString();
            if (index > -1 && selectedItem != null)
            {
                //不直接 Remove SelectItem 是因为列表中的过长项目会缩减
                SaveCities.Remove(SaveCities.Keys.ToList()[citiesListBox.SelectedIndex]);
                citiesListBox.Items.RemoveAt(index);
                citiesListBox.SelectedIndex = citiesListBox.Items.Count == index ? index - 1 : index;
            }

            if (citiesListBox.Items.Count == 0)
            {
                Welcome();
            }
        }

        private void MainWindow_FormClosed(object? sender, FormClosedEventArgs e)
        {
            CreateCacheFolder();

            File.WriteAllLines(citiesFile, SaveCities.Keys.ToList());
            File.WriteAllLines(cityLinksFile, SaveCities.Values.ToList());
        }

        private void MainWindow_Load(object? sender, EventArgs e)
        {
            CreateCacheFolder();

            if (File.Exists(citiesFile)
                && File.Exists(cityLinksFile)
                && File.ReadAllLines(citiesFile).Length == File.ReadAllLines(cityLinksFile).Length)
            {
                //读取数据
                SaveCities = DataUtility.MergeListsToDictionary([.. File.ReadAllLines(citiesFile)], [.. File.ReadAllLines(cityLinksFile)]);

                //更新列表
                citiesListBox.Items.Clear();
                citiesListBox.Items.AddRange([.. SaveCities.Keys.ToList().TruncateToFitWidth(citiesListBox.Width, citiesListBox.Font)]);

                if (citiesListBox.Items.Count > 0)
                {
                    citiesListBox.SelectedIndex = 0;
                }
                else
                {
                    Welcome();
                }
            }
            else
            {
                if (citiesListBox.Items.Count == 0)
                {
                    Welcome();
                }
            }
        }

        private void Welcome()
        {
            richTextBox1.Clear();
            richTextBox1.InsertText("欢迎使用 Moji Lite\r\n", null, 24);
            richTextBox1.AppendText("单击\"添加\"以选择你的城市\r\n");
            richTextBox1.AppendText($"仓库地址: {githubLink}");
        }

        internal void ShowMessage(string? content, string? title)
        {
            using KlxPiaoForm klxfm = new()
            {
                BorderColor = Color.FromArgb(147, 135, 248),
                TitleBoxBackColor = TitleBoxBackColor,
                TitleButtons = TitleButtonStyle.CloseOnly,
                TitleTextAlign = HorizontalAlignment.Center,
                StartPosition = FormStartPosition.CenterParent,
                ShowInTaskbar = false,
                Resizable = false,
                Text = title,
                Size = new Size(250, 150),
                ShowIcon = false,
            };
            using (Bitmap bmp = new(1, 1))
            {
                using Graphics g = Graphics.FromImage(bmp);
                string? text = content;
                klxfm.Width = (int)g.MeasureString(text, klxfm.Font).Width + 24;
            }
            using Button button = new()
            {
                Text = "确定",
                FlatStyle = FlatStyle.System,
                Size = new Size(88, 33)
            };
            button.Location = new Point((klxfm.Width - button.Width) / 2, klxfm.Height - button.Height - 24);
            button.Click += Button_Click;
            void Button_Click(object? sender, EventArgs e) => klxfm.CloseForm();
            using KlxPiaoLabel tip = new()
            {
                AutoSize = false,
                Size = klxfm.GetClientSize(),
                Text = content,
                Location = klxfm.GetClientRectangle().Location,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 0, 0, klxfm.TitleBoxHeight / 2 + 12)
            };
            klxfm.Controls.Add(tip);
            klxfm.Controls.Add(button);
            button.BringToFront();
            klxfm.ShowDialog();
        }
    }
}