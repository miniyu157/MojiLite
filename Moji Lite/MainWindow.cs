using KlxPiaoAPI;
using KlxPiaoControls;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Text;

namespace Moji_Lite
{
    public partial class MainWindow : KlxPiaoForm
    {
        public Dictionary<string, string> SaveCities { get; set; } = [];
        private readonly string CitiesFile = $"{Path.Combine(Application.StartupPath, "cache/Cities.dat")}";
        private readonly string CityLinksFile = $"{Path.Combine(Application.StartupPath, "cache/CityLinks.dat")}";
        private readonly string CachePath = $"{Path.Combine(Application.StartupPath, "cache")}";
        public MainWindow()
        {
            InitializeComponent();

            Text = $"{Application.ProductName} {Application.ProductVersion}";
            Icon? icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (icon != null)
            {
                Bitmap bitmap = icon.ToBitmap();
                Bitmap resizedBitmap = new(bitmap, new Size(20, 20));
                Icon resizedIcon = Icon.FromHandle(resizedBitmap.GetHicon());
                Icon = resizedIcon;
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

            richTextBox1.SelectionIndent = 8;
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
                    MessageBox.Show($"请求失败：{link}\r\n错误消息：{failMessage}", Application.ProductName);
                    richTextBox1.Clear();
                    return;
                }

                (bool IsSuccess15, string originalText15, string failMessage15) = await NetworkOperations.TryGetHTMLContentAsync(link15);
                if (!IsSuccess15)
                {
                    MessageBox.Show($"请求失败：{link}\r\n错误消息：{failMessage15}", Application.ProductName);
                    richTextBox1.Clear();
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
                List<string> morningWeather = _data4_weather.Where((item, index) => index % 2 == 0).Select(item2 => { if (item2.Length == 1) item2 += "    "; return item2; }).ToList();
                List<string> eveningWeather = _data4_weather.Where((item, index) => index % 2 == 1).Select(item2 => { if (item2.Length == 1) item2 += "    "; return item2; }).ToList();

                List<string> _data4_icon = _data4.ExtractAllBetween("src=\"", "\"");
                List<string> morningIcon = _data4_icon.Where((item, index) => index % 2 == 0).ToList();
                List<string> eveningIcon = _data4_icon.Where((item, index) => index % 2 == 1).ToList();

                List<string> _data4_temperature = _data4.ExtractAllBetween("<div class=\"tree clearfix\">", "</div>", MatchMode.StringIndex);
                List<string> morningTemperature = _data4_temperature.Select(item => item.ExtractBetween("<b>", "</b>")).ToList();
                List<string> eveningTemperature = _data4_temperature.Select(item => item.ExtractBetween("<strong>", "</strong>")).ToList();

                if (citiesListBox.Items.Count > 0)
                {
                    async Task RichTextBoxAddImage(RichTextBox richTextBox, string url, int newWidth, int newHeight)
                    {
                        string cacheFileName = Path.Combine(CachePath, Path.GetFileName(url));
                        Bitmap showImage;
                        if (File.Exists(cacheFileName))
                        {
                            showImage = new(Image.FromFile(cacheFileName), new Size(newWidth, newHeight));
                        }
                        else
                        {
                            showImage = await NetworkOperations.GetImageFromUrlAsync(url, new Size(newWidth, newHeight));
                            showImage.Save(cacheFileName); //缓存
                        }
                        showImage = showImage.ReplaceColor(Color.White, Color.FromArgb(237, 237, 237), false);
                        Bitmap clearBitmap = new(newWidth, newHeight);
                        using (Graphics g = Graphics.FromImage(clearBitmap))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.CompositingQuality = CompositingQuality.HighQuality;

                            g.Clear(richTextBox1.BackColor); //修正背景色
                            g.DrawImage(showImage, 0, 0);
                        }
                        Clipboard.SetImage(clearBitmap);
                        richTextBox.Paste();

                    }
                    using (RichTextBox richTextBox = new())
                    {
                        richTextBox.Font = richTextBox1.Font;
                        richTextBox.SelectionIndent = richTextBox1.SelectionIndent;
                        richTextBox.AppendText($"位置: {location}\r\n");
                        richTextBox.AppendText($"温度: {temperature}\r\n");
                        await RichTextBoxAddImage(richTextBox, icon, 50, 50);
                        richTextBox.AppendText($"{weather}\r\n");
                        richTextBox.AppendText($"更新: {updateTime}\r\n");
                        richTextBox.AppendText($"湿度: {humidity}\r\n");
                        richTextBox.AppendText($"风力: {windSpeed}\r\n");
                        richTextBox.AppendText($"提示: {tip}\r\n");
                        richTextBox.AppendText($"链接: {link}\r\n");
                        richTextBox.AppendText($"{new string('-', 300)}\r\n");
                        for (int i = 0; i < day.Count; i++)
                        {
                            richTextBox.AppendText($"{week[i]} {day[i]} {morningWeather[i]}({morningTemperature[i]})");
                            await RichTextBoxAddImage(richTextBox, morningIcon[i], 30, 30);
                            richTextBox.AppendText($" / {eveningWeather[i]}({eveningTemperature[i]})");
                            await RichTextBoxAddImage(richTextBox, eveningIcon[i], 30, 30);
                            if (i == 1) richTextBox.AppendText("<- 今天");
                            richTextBox.AppendText("\r\n");
                        }
                        richTextBox1.Rtf = richTextBox.Rtf;
                    }
                }
                else
                {
                    richTextBox1.Clear();
                }
            }
            else
            {
                richTextBox1.Clear();
            }
        }

        private void AddCityBut_Click(object? sender, EventArgs e)
        {
            new AddCity(this).ShowDialog();
            if (citiesListBox.Items.Count > 0)
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
                SaveCities.Remove(selectedItem);
                citiesListBox.Items.RemoveAt(index);
                citiesListBox.SelectedIndex = citiesListBox.Items.Count == index ? index - 1 : index;
            }
        }

        private void MainWindow_FormClosed(object? sender, FormClosedEventArgs e)
        {
            //保存数据
            StringBuilder citiesSb = new();
            StringBuilder cityLinksSb = new();
            SaveCities.ToList().ForEach(item =>
            {
                citiesSb.AppendLine(item.Key);
                cityLinksSb.AppendLine(item.Value);
            });
            using (var streamWriter = new StreamWriter(CitiesFile))
            {
                streamWriter.Write(citiesSb.ToString());
            }
            using (var streamWriter = new StreamWriter(CityLinksFile))
            {
                streamWriter.Write(cityLinksSb.ToString());
            }
        }

        private void MainWindow_Load(object? sender, EventArgs e)
        {
            if (File.Exists(CitiesFile)
                && File.Exists(CityLinksFile)
                && File.ReadAllLines(CitiesFile).Length == File.ReadAllLines(CityLinksFile).Length)
            {
                //读取数据
                SaveCities = DataUtility.MergeListsToDictionary([.. File.ReadAllLines(CitiesFile)], [.. File.ReadAllLines(CityLinksFile)]);

                //更新列表
                citiesListBox.Items.Clear();
                citiesListBox.Items.AddRange([.. SaveCities.Keys]);

                if (citiesListBox.Items.Count > 0)
                {
                    citiesListBox.SelectedIndex = 0;
                }
            }
        }
    }
}