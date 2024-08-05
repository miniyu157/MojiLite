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
            panel2 = new KlxPiaoPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // addCityBut
            // 
            addCityBut.FlatStyle = FlatStyle.System;
            addCityBut.Font = new Font("等线", 9F);
            addCityBut.Location = new Point(593, 38);
            addCityBut.Name = "addCityBut";
            addCityBut.Size = new Size(88, 33);
            addCityBut.TabIndex = 7;
            addCityBut.Text = "添加";
            // 
            // delCityBut
            // 
            delCityBut.FlatStyle = FlatStyle.System;
            delCityBut.Font = new Font("等线", 9F);
            delCityBut.Location = new Point(499, 38);
            delCityBut.Name = "delCityBut";
            delCityBut.Size = new Size(88, 33);
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
            citiesListBox.Size = new Size(180, 308);
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
            richTextBox1.Size = new Size(475, 366);
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
            panel1.Size = new Size(481, 372);
            panel1.TabIndex = 21;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(citiesListBox);
            panel2.CornerRadius = new KlxPiaoAPI.CornerRadius(12F, 12F, 12F, 12F);
            panel2.IsEnableShadow = false;
            panel2.Location = new Point(499, 77);
            panel2.Name = "panel2";
            panel2.Size = new Size(182, 333);
            panel2.TabIndex = 22;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(693, 423);
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
            ResumeLayout(false);
        }

        #endregion

        internal Button addCityBut;
        internal Button delCityBut;
        internal ListBox citiesListBox;
        private RichTextBox richTextBox1;
        private KlxPiaoPanel panel1;
        private KlxPiaoPanel panel2;
    }
}
