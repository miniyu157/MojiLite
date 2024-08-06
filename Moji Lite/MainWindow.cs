using INIParser;
using KlxPiaoAPI;
using KlxPiaoControls;
using Moji_Lite.Properties;
using System.Diagnostics;
using System.Reflection;

namespace Moji_Lite
{
    public partial class MainWindow : KlxPiaoForm
    {
        private CancellationTokenSource? cts;
        private int[] customColors = [];
        internal Dictionary<string, string> SaveCities { get; set; } = [];
        internal string? ListLayout { get; set; } = "two-column";

        private const char FULL_WIDTH_SPACE = '��';
        internal const string githubLink = "https://github.com/miniyu157/MojiLite";
        private const string sourceLink = "https://www.moji.com";
        private readonly string configPath = $"{Path.Combine(Application.StartupPath, "config")}";
        private readonly string citiesFile = $"{Path.Combine(Application.StartupPath, "config/Cities.dat")}";
        private readonly string cityLinksFile = $"{Path.Combine(Application.StartupPath, "config/CityLinks.dat")}";
        private readonly string configFile = $"{Path.Combine(Application.StartupPath, "config/Config.ini")}";
        internal readonly string cachePath = $"{Path.Combine(Application.StartupPath, "cache")}";
        internal readonly string cacheCitiesFile = $"{Path.Combine(Application.StartupPath, "cache/Cities.cache")}";
        internal readonly string cacheCityLinksFile = $"{Path.Combine(Application.StartupPath, "cache/CityLinks.cache")}";

        public MainWindow()
        {
            InitializeComponent();

            ContextMenuStrip themeMenuStrip = new()
            {
                Font = TitleFont
            };

            Text = $"{Application.ProductName} {GetProductVersion()}";
            Icon? appicon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appicon != null)
            {
                Icon = appicon.ResetImage(new Size(20, 20));
            }

            addCityBut.Click += AddCityBut_Click;
            delCityBut.Click += DelCityBut_Click;
            citiesListBox.SelectedIndexChanged += CitiesListBox_SelectedIndexChanged;
            FormClosed += MainWindow_FormClosed;
            SizeChanged += MainWindow_SizeChanged;
            richTextBox1.LinkClicked += RichTextBox1_LinkClicked;
            panel1.SizeChanged += Panel1_SizeChanged;
            panel2.SizeChanged += Panel2_SizeChanged;
            citiesListBox.DrawItem += CitiesListBox_DrawItem;
            upItemBut.Click += UpItemBut_Click;
            downItemBut.Click += DownItemBut_Click;
            themeBut.MouseClick += ThemeBut_MouseClick;
            refreshBut.Click += RefreshBut_Click;
            homeBut.Click += HomeBut_Click;
            settingBut.Click += SettingBut_Click;

            richTextBox1.SelectionIndent = 8;
            citiesListBox.DrawMode = DrawMode.OwnerDrawFixed;
            citiesListBox.ItemHeight = 30;

            Color[] colors =
            [
                Color.Linen,
                Color.FromArgb(211, 233, 209),
                Color.FromArgb(210, 204, 233),
                Color.FromArgb(229, 208, 236),
                Color.FromArgb(244, 195, 196),
                Color.FromArgb(208, 221, 245),
                Color.FromArgb(248, 225, 233),
                Color.FromArgb(203, 228, 227)
            ];
            foreach (Color color in colors)
            {
                Bitmap bitmap = new(24, 24);
                using Graphics g = Graphics.FromImage(bitmap);
                g.Clear(color);
                themeMenuStrip.Items.Add(color.ToHex() + ((color == colors[0]) ? " [Default]" : ""), bitmap, MenuItem_Click);
            }
            themeMenuStrip.Items.Add("[More...]", null, MenuItem_Click);
            void ThemeBut_MouseClick(object? sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    themeMenuStrip.Show(themeBut, e.Location);
                }
            }

            //WinForms �Դ��� ImageLayout û�п����
            refreshBut.BackgroundImage = Resources.refreshicon.ResetImage(new Size(24, 24), null);
            themeBut.BackgroundImage = Resources.themeicon.ResetImage(new Size(24, 24), null);
            homeBut.BackgroundImage = Resources.homeicon.ResetImage(new Size(24, 24), null);
            settingBut.BackgroundImage = Resources.settingicon.ResetImage(new Size(24, 24), null);

            CreateCache();

            IniFile ini = new(configFile);
            string? themeColor = ini["ColorConfig", "Color"];
            string? originalCustomColors = ini["ColorConfig", "CustomColors"];
            string? listLayout = ini["Config", "ListLayout"];
            if (originalCustomColors != null && originalCustomColors != "")
            {
                customColors = originalCustomColors.Split(',').Select(item => item.Trim()).Select(item => int.Parse(item)).ToArray();
            }
            if (themeColor != null)
            {
                SetThemeColor(ColorProcessor.FromHex(themeColor));
            }
            if (listLayout != null)
            {
                ListLayout = listLayout;
            }

            if (File.Exists(citiesFile)
                && File.Exists(cityLinksFile)
                && File.ReadAllLines(citiesFile).Length == File.ReadAllLines(cityLinksFile).Length)
            {
                //��ȡ����
                SaveCities = DataUtility.MergeListsToDictionary([.. File.ReadAllLines(citiesFile)], [.. File.ReadAllLines(cityLinksFile)]);

                //�����б�
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

        #region ��������ť
        private void HomeBut_Click(object? sender, EventArgs e)
        {
            citiesListBox.SelectedIndex = -1;
            Welcome();
        }

        private void RefreshBut_Click(object? sender, EventArgs e)
        {
            CitiesListBox_SelectedIndexChanged(sender, e);
        }

        private void MenuItem_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item)
            {
                Image? bitmap = item.Image;
                Color color;
                if (bitmap == null) //������ More
                {
                    ColorDialog colorDialog = new()
                    {
                        Color = TitleBoxBackColor,
                        CustomColors = customColors,
                        FullOpen = true
                    };
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        color = colorDialog.Color;
                        customColors = colorDialog.CustomColors;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    color = ((Bitmap)bitmap).GetPixel(1, 1);
                }

                SetThemeColor(color);
            }
        }

        //ʹ�� KlxPiaoForm.SetGlobalTheme �����Ῠ������������д�˸�����
        private void SetThemeColor(Color color)
        {
            TitleBoxBackColor = color;
            TitleBoxForeColor = ColorProcessor.GetBrightness(color) > 127 ? Color.Black : Color.White;
        }

        //����ǽ������ڵ���Ŀ
        private bool isSwap = false;
        private void SwapListItem(ListBox listBox, int index1, int index2)
        {
            isSwap = true;
            var selectedItem = listBox.SelectedItem;
            listBox.Items.RemoveAt(index1);
            listBox.Items.Insert(index2, selectedItem ?? "Fail.");
            listBox.SelectedIndex = index2;
            isSwap = false;
        }

        private void UpItemBut_Click(object? sender, EventArgs e)
        {
            int index1 = citiesListBox.SelectedIndex;
            if (index1 == 0 || index1 == -1) return; ;
            SaveCities = DataUtility.SwapDictionaryElements(SaveCities, index1, index1 - 1);
            SwapListItem(citiesListBox, index1, index1 - 1);
        }

        private void DownItemBut_Click(object? sender, EventArgs e)
        {
            int index1 = citiesListBox.SelectedIndex;
            if (index1 == citiesListBox.Items.Count - 1 || index1 == -1) return;
            SaveCities = DataUtility.SwapDictionaryElements(SaveCities, index1, index1 + 1);
            SwapListItem(citiesListBox, index1, index1 + 1);

        }

        private void SettingBut_Click(object? sender, EventArgs e)
        {
            Setting setting = new(this)
            {
                TitleBoxBackColor = TitleBoxBackColor,
                TitleBoxForeColor = TitleBoxForeColor
            };
            setting.ShowDialog();
            refreshBut.PerformClick();
        }
        #endregion

        #region ��Ӧ�����С
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
            panel3.Left = Width - panel3.Width - 12;
            panel1.Width = Width - panel1.Left - citiesListBox.Width - 24;
            panel2.Left = Width - citiesListBox.Width - panel1.Left - 2;
            delCityBut.Left = panel2.Left - 1;
            addCityBut.Left = delCityBut.Left + delCityBut.Width + 8;

            panel1.Invalidate();
            panel2.Invalidate();
        }
        #endregion

        private void CitiesListBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            //�������
            e.DrawBackground();

            //���û�������
            var itemText = citiesListBox.Items[e.Index].ToString();
            Font? font = e.Font;
            if (font != null)
            {
                var textSize = e.Graphics.MeasureString(itemText, font);

                //�����ı��ľ���λ��
                var textX = e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2;
                var textY = e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2;

                //�����ѡ������ñ���ɫΪ��ɫ��ǰ��ɫΪ��ɫ
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 220, 220)), e.Bounds); //���ñ���ɫ
                    e.Graphics.DrawString(itemText, font, Brushes.Black, textX, textY); //����ǰ��ɫ
                }
                else
                {
                    e.Graphics.DrawString(itemText, font, Brushes.Black, textX, textY); //Ĭ��ǰ��ɫ
                }

                //���ƽ�����Σ������Ҫ��
                e.DrawFocusRectangle();
            }
        }

        private async void LoadDataToListBox(CancellationToken token)
        {
            try
            {
                int index = citiesListBox.SelectedIndex;
                int count = citiesListBox.Items.Count;
                if (count > 0 && index > -1)
                {
                    richTextBox1.Text = "���ڼ���...";

                    string link = SaveCities.Values.ToList()[index];
                    string link15 = link.Replace("https://tianqi.moji.com/weather/", "https://tianqi.moji.com/forecast15/");

                    (bool IsSuccess, string originalText, string failMessage) = await NetworkOperations.TryGetHTMLContentAsync(link);
                    if (!IsSuccess)
                    {
                        richTextBox1.Text = "����ʧ��";
                        ShowMessage($"����ʧ�ܣ�{link}\r\n������Ϣ��{failMessage}", Application.ProductName);
                        return;
                    }

                    (bool IsSuccess15, string originalText15, string failMessage15) = await NetworkOperations.TryGetHTMLContentAsync(link15);
                    if (!IsSuccess15)
                    {
                        richTextBox1.Text = "����ʧ��"; ;
                        ShowMessage($"����ʧ�ܣ�{link15}\r\n������Ϣ��{failMessage15}", Application.ProductName);
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
                    int longWeatherLength = _data4_weather.OrderByDescending(s => s.Length).First().Length;
                    List<string> morningWeather = _data4_weather.Where((item, index) => index % 2 == 0).Select(item2 => item2.PadRight(longWeatherLength, FULL_WIDTH_SPACE) + ' ').ToList();
                    List<string> eveningWeather = _data4_weather.Where((item, index) => index % 2 == 1).Select(item2 => item2.PadRight(longWeatherLength, FULL_WIDTH_SPACE) + ' ').ToList();

                    List<string> _data4_icon = _data4.ExtractAllBetween("src=\"", "\"");
                    List<string> morningIcon = _data4_icon.Where((item, index) => index % 2 == 0).ToList();
                    List<string> eveningIcon = _data4_icon.Where((item, index) => index % 2 == 1).ToList();

                    List<string> _data4_temperature = _data4.ExtractAllBetween("<div class=\"tree clearfix\">", "</div>", MatchMode.StringIndex);
                    List<string> morningTemperature = _data4_temperature.Select(item => item.ExtractBetween("<b>", "</b>")).ToList();
                    List<string> eveningTemperature = _data4_temperature.Select(item => item.ExtractBetween("<strong>", "</strong>")).ToList();

                    async void RichTextBoxAddImage(RichTextBox richTextBox, string url, string cachePath, int newWidth, int newHeight)
                    {
                        try
                        {
                            string cacheFileName = Path.Combine(cachePath, Path.GetFileName(url));
                            //�ӻ������������ͼƬ
                            Bitmap showImage = File.Exists(cacheFileName)
                                ? new(Image.FromFile(cacheFileName))
                                : await NetworkOperations.GetImageFromUrlAsync(url, cacheFileName);
                            Clipboard.SetImage(showImage
                                .ReplaceColor(Color.White, Color.FromArgb(237, 237, 237), false)
                                .ResetImage(new Size(newWidth, newHeight), Color.White));
                            richTextBox.Paste();
                            Clipboard.Clear();
                        }
                        catch (HttpRequestException ex)
                        {
                            richTextBox1.Text = "����ʧ��";
                            ShowMessage($"����ʧ�ܣ�{url}\r\n������Ϣ��{ex.Message}", Application.ProductName);
                            return;
                        }
                    }
                    RichTextBox richTextBox = new();
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

                    richTextBox.InsertText("λ��: ", menuColor);
                    richTextBox.AppendText($"{location}\r\n");

                    richTextBox.InsertText("�¶�: \r\n", menuColor);
                    RichTextBoxAddImage(richTextBox, icon, cachePath, bigIconSize.Width, bigIconSize.Height);
                    richTextBox.InsertText($"{temperature}��", temperatureColor, temperatureFontSize);
                    richTextBox.AppendText($"{weather}\r\n");

                    richTextBox.InsertText("����: ", menuColor);
                    richTextBox.AppendText($"{updateTime}\r\n");

                    richTextBox.InsertText("ʪ��: ", menuColor);
                    richTextBox.AppendText($"{humidity}\r\n");

                    richTextBox.InsertText("����: ", menuColor);
                    richTextBox.AppendText($"{windSpeed}\r\n");

                    richTextBox.InsertText("��ʾ: ", menuColor);
                    richTextBox.AppendText($"{tip}\r\n");

                    richTextBox.InsertText("����: ", menuColor);
                    richTextBox.AppendText($"{link}\r\n");

                    richTextBox.AppendText($"{new string('-', 300)}\r\n");
                    switch (ListLayout)
                    {
                        case "two-column":
                            for (int i = 0; i < day.Count / 2; i++)
                            {
                                void AddItem(int index)
                                {
                                    Color showColor = index == 1 ? todayColor : dayColor;
                                    richTextBox.InsertText($"{week[index]} {day[index]} ", showColor);
                                    richTextBox.AppendText($"{morningWeather[index]}({morningTemperature[index]})");
                                    RichTextBoxAddImage(richTextBox, morningIcon[index], cachePath, smallIconSize.Width, smallIconSize.Height);
                                    richTextBox.AppendText($" / {eveningWeather[index]}({eveningTemperature[index]})");
                                }
                                AddItem(i);
                                richTextBox.AppendText(" ");
                                AddItem(i + day.Count / 2);
                                if (i != day.Count / 2 - 1)
                                    richTextBox.AppendText("\r\n");
                            }
                            break;

                        case "single-column":
                            for (int i = 0; i < day.Count; i++)
                            {
                                Color showColor = i == 1 ? todayColor : dayColor;
                                richTextBox.InsertText($"{week[i]} {day[i]} ", showColor);
                                richTextBox.AppendText($"{morningWeather[i]}({morningTemperature[i]})");
                                RichTextBoxAddImage(richTextBox, morningIcon[i], cachePath, smallIconSize.Width, smallIconSize.Height);
                                richTextBox.AppendText($" / {eveningWeather[i]}({eveningTemperature[i]})");
                                RichTextBoxAddImage(richTextBox, eveningIcon[i], cachePath, smallIconSize.Width, smallIconSize.Height);
                                richTextBox.AppendText("\r\n");
                            }
                            break;
                    }

                    //1.0.3 ������־
                    //��һ�μ��ز���ʾͼƬ�� bug����ʱ�����ַ�ʽ����ˡ�����(�����������������һ�� MessageBox��ͼƬ�ͳ�����ʾ����)
                    //���� 1.0.2 ��û����� bug �ġ�����
                    if (richTextBox.ContainsImage())
                    {
                        //���û���ʱɾ�����б���ô����Ҳ��ʱ����ʾ��
                        if (citiesListBox.Items.Count > 0 && !token.IsCancellationRequested)
                        {
                            richTextBox1.Rtf = richTextBox.Rtf;
                        }
                    }
                    else
                    {
                        refreshBut.PerformClick();
                    }

                }
            }
            catch (OperationCanceledException) { }
        }

        private void CitiesListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!isSwap) //�ƶ�Ԫ��ʱ�������¼�
            {
                //�����һ��֮ǰ�������������У�ȡ����
                if (cts != null)
                {
                    cts.Cancel();
                    cts.Dispose();
                }

                //Ϊ�µ����񴴽�һ���µ� CancellationTokenSource
                cts = new();
                LoadDataToListBox(cts.Token);
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
                //��ֱ�� Remove SelectItem ����Ϊ�б��еĹ�����Ŀ������
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
            CreateCache();

            File.WriteAllLines(citiesFile, SaveCities.Keys.ToList());
            File.WriteAllLines(cityLinksFile, SaveCities.Values.ToList());

            using var sw = new StreamWriter(configFile);
            sw.WriteLine("[Config]");
            sw.WriteLine($"ListLayout = {ListLayout}");
            sw.WriteLine();
            sw.WriteLine("[ColorConfig]");
            sw.WriteLine($"Color = {TitleBoxBackColor.ToHex()}");
            sw.WriteLine($"CustomColors = {string.Join(", ", customColors)}");
        }

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

        internal static string? GetProductVersion()
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

        private void CreateCache()
        {
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            if (!File.Exists(configFile))
            {
                File.Create(configFile).Close();
            }
        }

        private void Welcome()
        {
            richTextBox1.Clear();
            richTextBox1.InsertText("��ӭʹ�� Moji Lite\r\n", null, 24);
            richTextBox1.InsertText("����\"���\"��ѡ����ĳ���\r\n", Color.LightSeaGreen, 15);
            richTextBox1.AppendText($"�ֿ��ַ: {githubLink}\r\n");
            richTextBox1.AppendText($"������Դ: {sourceLink}");
        }

        internal void ShowMessage(string? content, string? title)
        {
            using KlxPiaoForm klxfm = new()
            {
                BorderColor = Color.FromArgb(147, 135, 248),
                TitleBoxBackColor = TitleBoxBackColor,
                TitleBoxForeColor = TitleBoxForeColor,
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
                Text = "ȷ��",
                FlatStyle = FlatStyle.System,
                Size = new Size(88, 33)
            };
            button.Location = new Point((klxfm.Width - button.Width) / 2 + 12, klxfm.Height - button.Height - 24);
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