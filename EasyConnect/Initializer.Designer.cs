namespace EasyConnect
{
    partial class Initializer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Initializer));
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            caddyPath = new TextBox();
            buttonCaddyPath = new Button();
            buttonManifestPath = new Button();
            manifestPath = new TextBox();
            deployPath = new TextBox();
            buttonDeployPath = new Button();
            buttonContinue = new Button();
            progressBarInitializer = new ProgressBar();
            listBoxStartingLogs = new ListBox();
            buttonMdmFile = new Button();
            devicesList = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(121, 102);
            label2.Name = "label2";
            label2.Size = new Size(143, 20);
            label2.TabIndex = 1;
            label2.Text = "Manifest script path:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(121, 135);
            label3.Name = "label3";
            label3.Size = new Size(94, 20);
            label3.TabIndex = 2;
            label3.Text = "Deploy path:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(121, 69);
            label4.Name = "label4";
            label4.Size = new Size(88, 20);
            label4.TabIndex = 3;
            label4.Text = "Caddy path:";
            // 
            // caddyPath
            // 
            caddyPath.BackColor = SystemColors.Window;
            caddyPath.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            caddyPath.Location = new Point(270, 62);
            caddyPath.Name = "caddyPath";
            caddyPath.ReadOnly = true;
            caddyPath.Size = new Size(270, 27);
            caddyPath.TabIndex = 4;
            // 
            // buttonCaddyPath
            // 
            buttonCaddyPath.Location = new Point(546, 60);
            buttonCaddyPath.Name = "buttonCaddyPath";
            buttonCaddyPath.Size = new Size(94, 29);
            buttonCaddyPath.TabIndex = 5;
            buttonCaddyPath.Text = "Browse";
            buttonCaddyPath.UseVisualStyleBackColor = true;
            buttonCaddyPath.Click += buttonCaddyPath_Click;
            // 
            // buttonManifestPath
            // 
            buttonManifestPath.Location = new Point(546, 93);
            buttonManifestPath.Name = "buttonManifestPath";
            buttonManifestPath.Size = new Size(94, 29);
            buttonManifestPath.TabIndex = 9;
            buttonManifestPath.Text = "Browse";
            buttonManifestPath.UseVisualStyleBackColor = true;
            buttonManifestPath.Click += buttonManifestPath_Click;
            // 
            // manifestPath
            // 
            manifestPath.BackColor = SystemColors.Window;
            manifestPath.Font = new Font("Segoe UI", 9F);
            manifestPath.Location = new Point(270, 95);
            manifestPath.Name = "manifestPath";
            manifestPath.ReadOnly = true;
            manifestPath.Size = new Size(270, 27);
            manifestPath.TabIndex = 8;
            // 
            // deployPath
            // 
            deployPath.BackColor = SystemColors.Window;
            deployPath.Font = new Font("Segoe UI", 9F);
            deployPath.Location = new Point(270, 128);
            deployPath.Name = "deployPath";
            deployPath.ReadOnly = true;
            deployPath.Size = new Size(270, 27);
            deployPath.TabIndex = 10;
            // 
            // buttonDeployPath
            // 
            buttonDeployPath.Location = new Point(546, 128);
            buttonDeployPath.Name = "buttonDeployPath";
            buttonDeployPath.Size = new Size(94, 29);
            buttonDeployPath.TabIndex = 11;
            buttonDeployPath.Text = "Browse";
            buttonDeployPath.UseVisualStyleBackColor = true;
            buttonDeployPath.Click += buttonDeployPath_Click;
            // 
            // buttonContinue
            // 
            buttonContinue.Location = new Point(663, 429);
            buttonContinue.Name = "buttonContinue";
            buttonContinue.Size = new Size(94, 29);
            buttonContinue.TabIndex = 12;
            buttonContinue.Text = "START";
            buttonContinue.UseVisualStyleBackColor = true;
            buttonContinue.Click += buttonContinue_Click;
            // 
            // progressBarInitializer
            // 
            progressBarInitializer.Location = new Point(41, 380);
            progressBarInitializer.Name = "progressBarInitializer";
            progressBarInitializer.Size = new Size(716, 29);
            progressBarInitializer.TabIndex = 14;
            // 
            // listBoxStartingLogs
            // 
            listBoxStartingLogs.BackColor = SystemColors.ButtonFace;
            listBoxStartingLogs.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxStartingLogs.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxStartingLogs.FormattingEnabled = true;
            listBoxStartingLogs.Location = new Point(41, 233);
            listBoxStartingLogs.Name = "listBoxStartingLogs";
            listBoxStartingLogs.Size = new Size(716, 124);
            listBoxStartingLogs.TabIndex = 16;
            // 
            // buttonMdmFile
            // 
            buttonMdmFile.Location = new Point(546, 163);
            buttonMdmFile.Name = "buttonMdmFile";
            buttonMdmFile.Size = new Size(94, 29);
            buttonMdmFile.TabIndex = 17;
            buttonMdmFile.Text = "Browse";
            buttonMdmFile.UseVisualStyleBackColor = true;
            buttonMdmFile.Click += buttonMdmFile_Click;
            // 
            // devicesList
            // 
            devicesList.BackColor = SystemColors.Window;
            devicesList.Font = new Font("Segoe UI", 9F);
            devicesList.Location = new Point(270, 165);
            devicesList.Name = "devicesList";
            devicesList.ReadOnly = true;
            devicesList.Size = new Size(270, 27);
            devicesList.TabIndex = 18;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(121, 168);
            label1.Name = "label1";
            label1.Size = new Size(123, 20);
            label1.TabIndex = 19;
            label1.Text = "Devices List path:";
            // 
            // Initializer
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            ClientSize = new Size(800, 474);
            Controls.Add(label1);
            Controls.Add(devicesList);
            Controls.Add(buttonMdmFile);
            Controls.Add(listBoxStartingLogs);
            Controls.Add(progressBarInitializer);
            Controls.Add(buttonContinue);
            Controls.Add(buttonDeployPath);
            Controls.Add(deployPath);
            Controls.Add(buttonManifestPath);
            Controls.Add(manifestPath);
            Controls.Add(buttonCaddyPath);
            Controls.Add(caddyPath);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Initializer";
            Text = "Initializer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox caddyPath;
        private Button buttonCaddyPath;
        private Button buttonManifestPath;
        private TextBox manifestPath;
        private TextBox deployPath;
        private Button buttonDeployPath;
        private Button buttonContinue;
        private ProgressBar progressBarInitializer;
        private ListBox listBoxStartingLogs;
        private Button buttonMdmFile;
        private TextBox devicesList;
        private Label label1;
    }
}