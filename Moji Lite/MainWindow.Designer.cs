using KlxPiaoControls;

namespace Moji_Lite
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            addCityBut = new Button();
            delCityBut = new Button();
            citiesListBox = new ListBox();
            richTextBox1 = new RichTextBox();
            panel1 = new KlxPiaoPanel();
            themeBut = new Button();
            panel2 = new KlxPiaoPanel();
            downItemBut = new Button();
            upItemBut = new Button();
            panel3 = new Panel();
            settingBut = new Button();
            homeBut = new Button();
            refreshBut = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // addCityBut
            // 
            addCityBut.FlatStyle = FlatStyle.System;
            addCityBut.Font = new Font("等线", 9F);
            addCityBut.Location = new Point(765, 38);
            addCityBut.Name = "addCityBut";
            addCityBut.Size = new Size(84, 33);
            addCityBut.TabIndex = 7;
            addCityBut.Text = "添加";
            // 
            // delCityBut
            // 
            delCityBut.FlatStyle = FlatStyle.System;
            delCityBut.Font = new Font("等线", 9F);
            delCityBut.Location = new Point(675, 38);
            delCityBut.Name = "delCityBut";
            delCityBut.Size = new Size(84, 33);
            delCityBut.TabIndex = 8;
            delCityBut.Text = "删除";
            // 
            // citiesListBox
            // 
            citiesListBox.BorderStyle = BorderStyle.None;
            citiesListBox.Font = new Font("等线", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            citiesListBox.FormattingEnabled = true;
            citiesListBox.ItemHeight = 14;
            citiesListBox.Location = new Point(1, 5);
            citiesListBox.Name = "citiesListBox";
            citiesListBox.Size = new Size(172, 392);
            citiesListBox.TabIndex = 9;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.White;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Font = new Font("等线", 10.5F);
            richTextBox1.Location = new Point(3, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new Size(651, 466);
            richTextBox1.TabIndex = 17;
            richTextBox1.Text = "";
            richTextBox1.WordWrap = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(richTextBox1);
            panel1.CornerRadius = new KlxPiaoAPI.CornerRadius(12F, 12F, 12F, 12F);
            panel1.IsEnableShadow = false;
            panel1.Location = new Point(12, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(657, 472);
            panel1.TabIndex = 21;
            // 
            // themeBut
            // 
            themeBut.BackgroundImageLayout = ImageLayout.Zoom;
            themeBut.Location = new Point(90, 0);
            themeBut.Name = "themeBut";
            themeBut.Size = new Size(24, 24);
            themeBut.TabIndex = 18;
            themeBut.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(citiesListBox);
            panel2.CornerRadius = new KlxPiaoAPI.CornerRadius(12F, 12F, 12F, 12F);
            panel2.IsEnableShadow = false;
            panel2.Location = new Point(675, 106);
            panel2.Name = "panel2";
            panel2.Size = new Size(174, 404);
            panel2.TabIndex = 22;
            // 
            // downItemBut
            // 
            downItemBut.Font = new Font("等线", 9F);
            downItemBut.Location = new Point(60, 0);
            downItemBut.Name = "downItemBut";
            downItemBut.Size = new Size(24, 24);
            downItemBut.TabIndex = 26;
            downItemBut.Text = "↓";
            // 
            // upItemBut
            // 
            upItemBut.Font = new Font("等线", 9F);
            upItemBut.Location = new Point(30, 0);
            upItemBut.Name = "upItemBut";
            upItemBut.Size = new Size(24, 24);
            upItemBut.TabIndex = 27;
            upItemBut.Text = "↑";
            // 
            // panel3
            // 
            panel3.Controls.Add(settingBut);
            panel3.Controls.Add(homeBut);
            panel3.Controls.Add(refreshBut);
            panel3.Controls.Add(themeBut);
            panel3.Controls.Add(upItemBut);
            panel3.Controls.Add(downItemBut);
            panel3.Location = new Point(675, 77);
            panel3.Name = "panel3";
            panel3.Size = new Size(174, 23);
            panel3.TabIndex = 31;
            // 
            // settingBut
            // 
            settingBut.BackgroundImageLayout = ImageLayout.Zoom;
            settingBut.Location = new Point(150, 0);
            settingBut.Name = "settingBut";
            settingBut.Size = new Size(24, 24);
            settingBut.TabIndex = 30;
            settingBut.UseVisualStyleBackColor = true;
            // 
            // homeBut
            // 
            homeBut.BackgroundImageLayout = ImageLayout.Zoom;
            homeBut.Location = new Point(120, 0);
            homeBut.Name = "homeBut";
            homeBut.Size = new Size(24, 24);
            homeBut.TabIndex = 29;
            homeBut.UseVisualStyleBackColor = true;
            // 
            // refreshBut
            // 
            refreshBut.BackgroundImageLayout = ImageLayout.Zoom;
            refreshBut.Location = new Point(0, 0);
            refreshBut.Name = "refreshBut";
            refreshBut.Size = new Size(24, 24);
            refreshBut.TabIndex = 28;
            refreshBut.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(861, 522);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(delCityBut);
            Controls.Add(addCityBut);
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainWindow";
            TitleBoxBackColor = Color.Linen;
            TitleFont = new Font("等线", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            TitleTextAlign = HorizontalAlignment.Center;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        internal Button addCityBut;
        internal Button delCityBut;
        internal ListBox citiesListBox;
        private RichTextBox richTextBox1;
        private KlxPiaoPanel panel1;
        private KlxPiaoPanel panel2;
        internal Button downItemBut;
        internal Button upItemBut;
        private Panel panel3;
        private Button themeBut;
        private Button refreshBut;
        private Button homeBut;
        private Button settingBut;
    }
}
