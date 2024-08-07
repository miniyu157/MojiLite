using KlxPiaoControls;

namespace Moji_Lite
{
    partial class Setting
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
            label1 = new Label();
            listLayout_Single_Radiu = new RadioButton();
            listLayout_Two_Radiu = new RadioButton();
            removeCacheLinkLabel = new LinkLabel();
            showVerLabel = new Label();
            githubLinkLabel = new LinkLabel();
            panel1 = new Panel();
            highlightToday_False_Radiu = new RadioButton();
            highlightToday_True_Radiu = new RadioButton();
            label2 = new Label();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 51);
            label1.Name = "label1";
            label1.Size = new Size(79, 13);
            label1.TabIndex = 1;
            label1.Text = "预报列表布局";
            // 
            // listLayout_Single_Radiu
            // 
            listLayout_Single_Radiu.AutoSize = true;
            listLayout_Single_Radiu.Location = new Point(0, 0);
            listLayout_Single_Radiu.Name = "listLayout_Single_Radiu";
            listLayout_Single_Radiu.Size = new Size(49, 17);
            listLayout_Single_Radiu.TabIndex = 3;
            listLayout_Single_Radiu.TabStop = true;
            listLayout_Single_Radiu.Text = "单列";
            listLayout_Single_Radiu.UseVisualStyleBackColor = true;
            // 
            // listLayout_Two_Radiu
            // 
            listLayout_Two_Radiu.AutoSize = true;
            listLayout_Two_Radiu.Location = new Point(55, 0);
            listLayout_Two_Radiu.Name = "listLayout_Two_Radiu";
            listLayout_Two_Radiu.Size = new Size(49, 17);
            listLayout_Two_Radiu.TabIndex = 4;
            listLayout_Two_Radiu.TabStop = true;
            listLayout_Two_Radiu.Text = "并列";
            listLayout_Two_Radiu.UseVisualStyleBackColor = true;
            // 
            // removeCacheLinkLabel
            // 
            removeCacheLinkLabel.ActiveLinkColor = Color.FromArgb(128, 128, 255);
            removeCacheLinkLabel.AutoSize = true;
            removeCacheLinkLabel.Font = new Font("等线", 9F);
            removeCacheLinkLabel.LinkColor = Color.FromArgb(128, 128, 255);
            removeCacheLinkLabel.Location = new Point(28, 343);
            removeCacheLinkLabel.Name = "removeCacheLinkLabel";
            removeCacheLinkLabel.Size = new Size(55, 13);
            removeCacheLinkLabel.TabIndex = 6;
            removeCacheLinkLabel.TabStop = true;
            removeCacheLinkLabel.Text = "清除缓存";
            // 
            // showVerLabel
            // 
            showVerLabel.AutoSize = true;
            showVerLabel.Location = new Point(346, 343);
            showVerLabel.Name = "showVerLabel";
            showVerLabel.Size = new Size(79, 13);
            showVerLabel.TabIndex = 8;
            showVerLabel.Text = "showVerLabel";
            // 
            // githubLinkLabel
            // 
            githubLinkLabel.ActiveLinkColor = Color.FromArgb(128, 128, 255);
            githubLinkLabel.AutoSize = true;
            githubLinkLabel.Font = new Font("等线", 9F);
            githubLinkLabel.LinkColor = Color.FromArgb(128, 128, 255);
            githubLinkLabel.Location = new Point(89, 343);
            githubLinkLabel.Name = "githubLinkLabel";
            githubLinkLabel.Size = new Size(43, 13);
            githubLinkLabel.TabIndex = 10;
            githubLinkLabel.TabStop = true;
            githubLinkLabel.Text = "Github";
            // 
            // panel1
            // 
            panel1.Controls.Add(highlightToday_False_Radiu);
            panel1.Controls.Add(highlightToday_True_Radiu);
            panel1.Location = new Point(113, 81);
            panel1.Name = "panel1";
            panel1.Size = new Size(198, 26);
            panel1.TabIndex = 14;
            // 
            // highlightToday_False_Radiu
            // 
            highlightToday_False_Radiu.AutoSize = true;
            highlightToday_False_Radiu.Location = new Point(55, 0);
            highlightToday_False_Radiu.Name = "highlightToday_False_Radiu";
            highlightToday_False_Radiu.Size = new Size(49, 17);
            highlightToday_False_Radiu.TabIndex = 16;
            highlightToday_False_Radiu.TabStop = true;
            highlightToday_False_Radiu.Text = "不要";
            highlightToday_False_Radiu.UseVisualStyleBackColor = true;
            // 
            // highlightToday_True_Radiu
            // 
            highlightToday_True_Radiu.AutoSize = true;
            highlightToday_True_Radiu.Location = new Point(0, 0);
            highlightToday_True_Radiu.Name = "highlightToday_True_Radiu";
            highlightToday_True_Radiu.Size = new Size(49, 17);
            highlightToday_True_Radiu.TabIndex = 15;
            highlightToday_True_Radiu.TabStop = true;
            highlightToday_True_Radiu.Text = "好的";
            highlightToday_True_Radiu.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 83);
            label2.Name = "label2";
            label2.Size = new Size(79, 13);
            label2.TabIndex = 16;
            label2.Text = "高亮当前日期";
            // 
            // panel2
            // 
            panel2.Controls.Add(listLayout_Single_Radiu);
            panel2.Controls.Add(listLayout_Two_Radiu);
            panel2.Location = new Point(113, 49);
            panel2.Name = "panel2";
            panel2.Size = new Size(198, 26);
            panel2.TabIndex = 18;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(443, 380);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(githubLinkLabel);
            Controls.Add(showVerLabel);
            Controls.Add(removeCacheLinkLabel);
            Controls.Add(label1);
            Font = new Font("等线", 9F);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Setting";
            Resizable = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "设置";
            TitleBoxBackColor = Color.Linen;
            TitleButtons = TitleButtonStyle.CloseOnly;
            TitleFont = new Font("等线", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            TitleTextAlign = HorizontalAlignment.Center;
            TitleTextOffset = new Point(0, 1);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private RadioButton listLayout_Single_Radiu;
        private RadioButton listLayout_Two_Radiu;
        private LinkLabel removeCacheLinkLabel;
        private Label showVerLabel;
        private LinkLabel githubLinkLabel;
        private Panel panel1;
        private RadioButton highlightToday_False_Radiu;
        private RadioButton highlightToday_True_Radiu;
        private Label label2;
        private Panel panel2;
    }
}