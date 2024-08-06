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

            FormClosed += Setting_FormClosed;
            removeCacheLinkLabel.LinkClicked += RemoveCacheLinkLabel_LinkClicked;
            githubLinkLabel.LinkClicked += GithubLinkLabel_LinkClicked;

            mainWindow = parentForm;
            showVerLabel.Text = $"Version: {MainWindow.GetProductVersion()}";

            switch (mainWindow.ListLayout)
            {
                case "two-column":
                    listLayout_Two_Radiu.Checked = true;
                    listLayout_Single_Radiu.Checked = false;
                    break;

                case "single-column":
                    listLayout_Two_Radiu.Checked = false;
                    listLayout_Single_Radiu.Checked = true;
                    break;
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

        private void Setting_FormClosed(object? sender, FormClosedEventArgs e)
        {
            mainWindow.ListLayout = listLayout_Single_Radiu.Checked ? "single-column" : "two-column";
        }
    }
}