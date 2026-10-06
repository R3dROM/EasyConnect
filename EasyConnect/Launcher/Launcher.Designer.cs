namespace EasyConnect
{
    partial class LauncherView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherView));
            label2 = new Label();
            label4 = new Label();
            caddyPath = new TextBox();
            buttonCaddyPath = new Button();
            buttonManifestPath = new Button();
            scriptsPath = new TextBox();
            buttonContinue = new Button();
            progressBarInitializer = new ProgressBar();
            listBoxStartingLogs = new ListBox();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 80);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 1;
            label2.Text = " Scripts path:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 47);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 3;
            label4.Text = "Caddy path:";
            // 
            // caddyPath
            // 
            caddyPath.BackColor = SystemColors.Window;
            caddyPath.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            caddyPath.Location = new Point(190, 41);
            caddyPath.Name = "caddyPath";
            caddyPath.ReadOnly = true;
            caddyPath.Size = new Size(463, 23);
            caddyPath.TabIndex = 4;
            // 
            // buttonCaddyPath
            // 
            buttonCaddyPath.Location = new Point(659, 43);
            buttonCaddyPath.Name = "buttonCaddyPath";
            buttonCaddyPath.Size = new Size(94, 29);
            buttonCaddyPath.TabIndex = 5;
            buttonCaddyPath.Text = "Browse";
            buttonCaddyPath.UseVisualStyleBackColor = true;
            buttonCaddyPath.Click += buttonCaddyPath_Click;
            // 
            // buttonManifestPath
            // 
            buttonManifestPath.Location = new Point(659, 76);
            buttonManifestPath.Name = "buttonManifestPath";
            buttonManifestPath.Size = new Size(94, 29);
            buttonManifestPath.TabIndex = 9;
            buttonManifestPath.Text = "Browse";
            buttonManifestPath.UseVisualStyleBackColor = true;
            buttonManifestPath.Click += buttonManifestPath_Click;
            // 
            // scriptsPath
            // 
            scriptsPath.BackColor = SystemColors.Window;
            scriptsPath.Font = new Font("Segoe UI", 9F);
            scriptsPath.Location = new Point(190, 74);
            scriptsPath.Name = "scriptsPath";
            scriptsPath.ReadOnly = true;
            scriptsPath.Size = new Size(463, 23);
            scriptsPath.TabIndex = 8;
            // 
            // buttonContinue
            // 
            buttonContinue.Location = new Point(38, 252);
            buttonContinue.Name = "buttonContinue";
            buttonContinue.Size = new Size(95, 30);
            buttonContinue.TabIndex = 12;
            buttonContinue.Text = "START";
            buttonContinue.UseVisualStyleBackColor = true;
            buttonContinue.Click += buttonContinue_Click;
            // 
            // progressBarInitializer
            // 
            progressBarInitializer.Location = new Point(138, 253);
            progressBarInitializer.Name = "progressBarInitializer";
            progressBarInitializer.Size = new Size(615, 29);
            progressBarInitializer.TabIndex = 14;
            // 
            // listBoxStartingLogs
            // 
            listBoxStartingLogs.BackColor = SystemColors.ButtonFace;
            listBoxStartingLogs.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxStartingLogs.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxStartingLogs.FormattingEnabled = true;
            listBoxStartingLogs.Location = new Point(38, 119);
            listBoxStartingLogs.Name = "listBoxStartingLogs";
            listBoxStartingLogs.Size = new Size(716, 100);
            listBoxStartingLogs.TabIndex = 16;
            // 
            // LauncherView
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            ClientSize = new Size(800, 339);
            Controls.Add(listBoxStartingLogs);
            Controls.Add(progressBarInitializer);
            Controls.Add(buttonContinue);
            Controls.Add(buttonManifestPath);
            Controls.Add(scriptsPath);
            Controls.Add(buttonCaddyPath);
            Controls.Add(caddyPath);
            Controls.Add(label4);
            Controls.Add(label2);
            Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "LauncherView";
            Text = "EASYLINK";
            Load += LauncherView_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label4;
        private TextBox caddyPath;
        private Button buttonCaddyPath;
        private Button buttonManifestPath;
        private TextBox scriptsPath;
        private Button buttonContinue;
        private ProgressBar progressBarInitializer;
        private ListBox listBoxStartingLogs;
    }
}