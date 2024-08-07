using KlxPiaoControls;
using System.Diagnostics;

namespace Moji_Lite
{
    public partial class Setting : KlxPiaoForm
    {
        private readonly MainWindow mainWindow;
        public Setting(MainWindow parentForm)
        {
            InitializeComponent();

            mainWindow = parentForm;
            showVerLabel.Text = $"Version: {MainWindow.GetProductVersion()}";

            //功能事件
            removeCacheLinkLabel.LinkClicked += RemoveCacheLinkLabel_LinkClicked;
            githubLinkLabel.LinkClicked += GithubLinkLabel_LinkClicked;

            InitializeConfig(
                listLayout_Two_Radiu,
                listLayout_Single_Radiu,
                "two-column",
                "single-column",
                mainWindow.ListLayout,
                value => mainWindow.ListLayout = value
            );

            InitializeConfig(
                highlightToday_True_Radiu,
                highlightToday_False_Radiu,
                "true",
                "false",
                mainWindow.HighlightToday,
                value => mainWindow.HighlightToday = value
            );
        }

        private void InitializeConfig(RadioButton trueRadio, RadioButton falseRadio, string trueValue, string falseValue, string? getValue, Action<string> setValue)
        {
            trueRadio.Checked = getValue == trueValue;
            falseRadio.Checked = getValue == falseValue;
            trueRadio.CheckedChanged += (sender, e) => RadioButton_CheckedChanged(trueRadio, trueValue, falseValue, setValue);
            falseRadio.CheckedChanged += (sender, e) => RadioButton_CheckedChanged(trueRadio, trueValue, falseValue, setValue);

            void RadioButton_CheckedChanged(RadioButton trueRadio, string trueValue, string falseValue, Action<string> setValue)
            {
                setValue(trueRadio.Checked ? trueValue : falseValue);
                mainWindow.refreshBut.PerformClick();
            }
        }

        private void GithubLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo() { FileName = MainWindow.githubLink, UseShellExecute = true });
        }

        private void RemoveCacheLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(mainWindow.cachePath))
            {
                foreach (string file in Directory.GetFiles(mainWindow.cachePath))
                {
                    try { File.Delete(file); } catch { }
                }
            }

            mainWindow.ShowMessage("清除缓存完成。", Application.ProductName);
        }
    }
}