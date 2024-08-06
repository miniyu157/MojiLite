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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 49);
            label1.Name = "label1";
            label1.Size = new Size(79, 13);
            label1.TabIndex = 1;
            label1.Text = "预报列表布局";
            // 
            // listLayout_Single_Radiu
            // 
            listLayout_Single_Radiu.AutoSize = true;
            listLayout_Single_Radiu.Location = new Point(113, 47);
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
            listLayout_Two_Radiu.Location = new Point(168, 47);
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
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(443, 380);
            Controls.Add(githubLinkLabel);
            Controls.Add(showVerLabel);
            Controls.Add(removeCacheLinkLabel);
            Controls.Add(listLayout_Two_Radiu);
            Controls.Add(listLayout_Single_Radiu);
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
    }
}