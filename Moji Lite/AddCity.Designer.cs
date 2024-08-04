namespace Moji_Lite
{
    partial class AddCity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            citiesListBox = new ListBox();
            searchTextBox = new TextBox();
            tipLabel = new Label();
            refreshLinkLabel = new LinkLabel();
            SuspendLayout();
            // 
            // citiesListBox
            // 
            citiesListBox.FormattingEnabled = true;
            citiesListBox.ItemHeight = 17;
            citiesListBox.Location = new Point(12, 75);
            citiesListBox.Name = "citiesListBox";
            citiesListBox.Size = new Size(254, 378);
            citiesListBox.TabIndex = 1;
            // 
            // searchTextBox
            // 
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Location = new Point(12, 41);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(254, 23);
            searchTextBox.TabIndex = 0;
            // 
            // tipLabel
            // 
            tipLabel.AutoSize = true;
            tipLabel.Location = new Point(12, 460);
            tipLabel.Name = "tipLabel";
            tipLabel.Size = new Size(0, 17);
            tipLabel.TabIndex = 3;
            // 
            // refreshLinkLabel
            // 
            refreshLinkLabel.ActiveLinkColor = Color.Black;
            refreshLinkLabel.AutoSize = true;
            refreshLinkLabel.LinkColor = Color.Black;
            refreshLinkLabel.Location = new Point(186, 460);
            refreshLinkLabel.Name = "refreshLinkLabel";
            refreshLinkLabel.Size = new Size(80, 17);
            refreshLinkLabel.TabIndex = 2;
            refreshLinkLabel.TabStop = true;
            refreshLinkLabel.Text = "重新加载列表";
            // 
            // AddCity
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(278, 486);
            Controls.Add(refreshLinkLabel);
            Controls.Add(tipLabel);
            Controls.Add(searchTextBox);
            Controls.Add(citiesListBox);
            Name = "AddCity";
            Resizable = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "添加城市";
            TitleBoxBackColor = Color.Linen;
            TitleButtons = TitleButtonStyle.CloseOnly;
            TitleTextAlign = HorizontalAlignment.Center;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox citiesListBox;
        private TextBox searchTextBox;
        private Label tipLabel;
        private LinkLabel refreshLinkLabel;
    }
}