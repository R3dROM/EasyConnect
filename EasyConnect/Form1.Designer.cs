namespace EasyConnect
{
    partial class WINDOW
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Title = new System.Windows.Forms.Label();
            this.groupIPConnect = new System.Windows.Forms.GroupBox();
            this.buttonCONNECT = new System.Windows.Forms.Button();
            this.textBoxPORT = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.groupBoxSERVER = new System.Windows.Forms.GroupBox();
            this.buttonINSTALL = new System.Windows.Forms.Button();
            this.buttonMOVE = new System.Windows.Forms.Button();
            this.buttonDOWNLOAD = new System.Windows.Forms.Button();
            this.buttonSERVERCONNECTION = new System.Windows.Forms.Button();
            this.textBoxSERVERPORT = new System.Windows.Forms.TextBox();
            this.textBoxSERVERIP = new System.Windows.Forms.TextBox();
            this.labelSERVERPORT = new System.Windows.Forms.Label();
            this.labelSERVERIP = new System.Windows.Forms.Label();
            this.listBoxFILENAMES = new System.Windows.Forms.ListBox();
            this.labelFILENAME = new System.Windows.Forms.Label();
            this.labelCURRENTIP = new System.Windows.Forms.Label();
            this.labelIPDEVICE = new System.Windows.Forms.Label();
            this.groupBoxDEVICES = new System.Windows.Forms.GroupBox();
            this.listBoxDEVICES = new System.Windows.Forms.ListBox();
            this.progressBarDOWNLOAD = new System.Windows.Forms.ProgressBar();
            this.labelBUNDLEID = new System.Windows.Forms.Label();
            this.textBoxBUNDLEID = new System.Windows.Forms.TextBox();
            this.groupIPConnect.SuspendLayout();
            this.groupBoxSERVER.SuspendLayout();
            this.groupBoxDEVICES.SuspendLayout();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(140, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(204, 20);
            this.Title.TabIndex = 0;
            this.Title.Text = "EASY CONNECTION";
            // 
            // groupIPConnect
            // 
            this.groupIPConnect.Controls.Add(this.buttonCONNECT);
            this.groupIPConnect.Controls.Add(this.textBoxPORT);
            this.groupIPConnect.Controls.Add(this.textBoxIP);
            this.groupIPConnect.Controls.Add(this.labelPort);
            this.groupIPConnect.Controls.Add(this.labelIP);
            this.groupIPConnect.Location = new System.Drawing.Point(12, 64);
            this.groupIPConnect.Name = "groupIPConnect";
            this.groupIPConnect.Size = new System.Drawing.Size(441, 150);
            this.groupIPConnect.TabIndex = 1;
            this.groupIPConnect.TabStop = false;
            this.groupIPConnect.Text = "IP Connection";
            // 
            // buttonCONNECT
            // 
            this.buttonCONNECT.Location = new System.Drawing.Point(160, 105);
            this.buttonCONNECT.Name = "buttonCONNECT";
            this.buttonCONNECT.Size = new System.Drawing.Size(110, 25);
            this.buttonCONNECT.TabIndex = 4;
            this.buttonCONNECT.Text = "CONNECT";
            this.buttonCONNECT.UseVisualStyleBackColor = true;
            this.buttonCONNECT.Click += new System.EventHandler(this.buttonCONNECT_Click);
            // 
            // textBoxPORT
            // 
            this.textBoxPORT.Location = new System.Drawing.Point(160, 63);
            this.textBoxPORT.Name = "textBoxPORT";
            this.textBoxPORT.Size = new System.Drawing.Size(100, 22);
            this.textBoxPORT.TabIndex = 3;
            this.textBoxPORT.Text = "5555";
            this.textBoxPORT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxPORT.TextChanged += new System.EventHandler(this.textBoxPORT_TextChanged);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(160, 26);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(150, 22);
            this.textBoxIP.TabIndex = 2;
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(91, 63);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(48, 16);
            this.labelPort.TabIndex = 1;
            this.labelPort.Text = "PORT:";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(117, 26);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(22, 16);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "IP:";
            // 
            // groupBoxSERVER
            // 
            this.groupBoxSERVER.Controls.Add(this.textBoxBUNDLEID);
            this.groupBoxSERVER.Controls.Add(this.labelBUNDLEID);
            this.groupBoxSERVER.Controls.Add(this.buttonINSTALL);
            this.groupBoxSERVER.Controls.Add(this.buttonMOVE);
            this.groupBoxSERVER.Controls.Add(this.buttonDOWNLOAD);
            this.groupBoxSERVER.Controls.Add(this.buttonSERVERCONNECTION);
            this.groupBoxSERVER.Controls.Add(this.textBoxSERVERPORT);
            this.groupBoxSERVER.Controls.Add(this.textBoxSERVERIP);
            this.groupBoxSERVER.Controls.Add(this.labelSERVERPORT);
            this.groupBoxSERVER.Controls.Add(this.labelSERVERIP);
            this.groupBoxSERVER.Location = new System.Drawing.Point(12, 220);
            this.groupBoxSERVER.Name = "groupBoxSERVER";
            this.groupBoxSERVER.Size = new System.Drawing.Size(441, 321);
            this.groupBoxSERVER.TabIndex = 2;
            this.groupBoxSERVER.TabStop = false;
            this.groupBoxSERVER.Text = "SERVER Connection";
            this.groupBoxSERVER.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // buttonINSTALL
            // 
            this.buttonINSTALL.Location = new System.Drawing.Point(275, 215);
            this.buttonINSTALL.Name = "buttonINSTALL";
            this.buttonINSTALL.Size = new System.Drawing.Size(91, 23);
            this.buttonINSTALL.TabIndex = 9;
            this.buttonINSTALL.Text = "INSTALL";
            this.buttonINSTALL.UseVisualStyleBackColor = true;
            this.buttonINSTALL.Click += new System.EventHandler(this.buttonINSTALL_Click);
            // 
            // buttonMOVE
            // 
            this.buttonMOVE.Location = new System.Drawing.Point(178, 215);
            this.buttonMOVE.Name = "buttonMOVE";
            this.buttonMOVE.Size = new System.Drawing.Size(75, 23);
            this.buttonMOVE.TabIndex = 8;
            this.buttonMOVE.Text = "MOVE";
            this.buttonMOVE.UseVisualStyleBackColor = true;
            this.buttonMOVE.Click += new System.EventHandler(this.buttonMOVE_Click);
            // 
            // buttonDOWNLOAD
            // 
            this.buttonDOWNLOAD.Location = new System.Drawing.Point(51, 215);
            this.buttonDOWNLOAD.Name = "buttonDOWNLOAD";
            this.buttonDOWNLOAD.Size = new System.Drawing.Size(110, 23);
            this.buttonDOWNLOAD.TabIndex = 7;
            this.buttonDOWNLOAD.Text = "DOWNLOAD";
            this.buttonDOWNLOAD.UseVisualStyleBackColor = true;
            this.buttonDOWNLOAD.Click += new System.EventHandler(this.buttonDOWNLOAD_Click);
            // 
            // buttonSERVERCONNECTION
            // 
            this.buttonSERVERCONNECTION.Location = new System.Drawing.Point(156, 165);
            this.buttonSERVERCONNECTION.Name = "buttonSERVERCONNECTION";
            this.buttonSERVERCONNECTION.Size = new System.Drawing.Size(110, 25);
            this.buttonSERVERCONNECTION.TabIndex = 4;
            this.buttonSERVERCONNECTION.Text = "CONNECT";
            this.buttonSERVERCONNECTION.UseVisualStyleBackColor = true;
            this.buttonSERVERCONNECTION.Click += new System.EventHandler(this.buttonSERVERCONNECTION_Click);
            // 
            // textBoxSERVERPORT
            // 
            this.textBoxSERVERPORT.Location = new System.Drawing.Point(160, 57);
            this.textBoxSERVERPORT.Name = "textBoxSERVERPORT";
            this.textBoxSERVERPORT.Size = new System.Drawing.Size(100, 22);
            this.textBoxSERVERPORT.TabIndex = 3;
            this.textBoxSERVERPORT.TextChanged += new System.EventHandler(this.textBoxSERVERPORT_TextChanged);
            // 
            // textBoxSERVERIP
            // 
            this.textBoxSERVERIP.Location = new System.Drawing.Point(160, 19);
            this.textBoxSERVERIP.Name = "textBoxSERVERIP";
            this.textBoxSERVERIP.Size = new System.Drawing.Size(150, 22);
            this.textBoxSERVERIP.TabIndex = 2;
            this.textBoxSERVERIP.TextChanged += new System.EventHandler(this.textBoxSERVERIP_TextChanged);
            // 
            // labelSERVERPORT
            // 
            this.labelSERVERPORT.AutoSize = true;
            this.labelSERVERPORT.Location = new System.Drawing.Point(94, 57);
            this.labelSERVERPORT.Name = "labelSERVERPORT";
            this.labelSERVERPORT.Size = new System.Drawing.Size(48, 16);
            this.labelSERVERPORT.TabIndex = 1;
            this.labelSERVERPORT.Text = "PORT:";
            // 
            // labelSERVERIP
            // 
            this.labelSERVERIP.AutoSize = true;
            this.labelSERVERIP.Location = new System.Drawing.Point(117, 19);
            this.labelSERVERIP.Name = "labelSERVERIP";
            this.labelSERVERIP.Size = new System.Drawing.Size(22, 16);
            this.labelSERVERIP.TabIndex = 0;
            this.labelSERVERIP.Text = "IP:";
            // 
            // listBoxFILENAMES
            // 
            this.listBoxFILENAMES.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxFILENAMES.FormattingEnabled = true;
            this.listBoxFILENAMES.HorizontalScrollbar = true;
            this.listBoxFILENAMES.ItemHeight = 16;
            this.listBoxFILENAMES.Location = new System.Drawing.Point(519, 277);
            this.listBoxFILENAMES.Name = "listBoxFILENAMES";
            this.listBoxFILENAMES.Size = new System.Drawing.Size(408, 100);
            this.listBoxFILENAMES.TabIndex = 6;
            this.listBoxFILENAMES.SelectedIndexChanged += new System.EventHandler(this.listBoxFILENAMES_SelectedIndexChanged);
            // 
            // labelFILENAME
            // 
            this.labelFILENAME.AutoSize = true;
            this.labelFILENAME.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFILENAME.Location = new System.Drawing.Point(677, 245);
            this.labelFILENAME.Name = "labelFILENAME";
            this.labelFILENAME.Size = new System.Drawing.Size(74, 20);
            this.labelFILENAME.TabIndex = 5;
            this.labelFILENAME.Text = "FILES";
            this.labelFILENAME.Click += new System.EventHandler(this.labelFILENAME_Click);
            // 
            // labelCURRENTIP
            // 
            this.labelCURRENTIP.AutoSize = true;
            this.labelCURRENTIP.Location = new System.Drawing.Point(88, 42);
            this.labelCURRENTIP.Name = "labelCURRENTIP";
            this.labelCURRENTIP.Size = new System.Drawing.Size(92, 16);
            this.labelCURRENTIP.TabIndex = 3;
            this.labelCURRENTIP.Text = "CURRENT IP:";
            this.labelCURRENTIP.Click += new System.EventHandler(this.labelCURRENTIP_Click);
            // 
            // labelIPDEVICE
            // 
            this.labelIPDEVICE.AutoSize = true;
            this.labelIPDEVICE.Location = new System.Drawing.Point(187, 42);
            this.labelIPDEVICE.Name = "labelIPDEVICE";
            this.labelIPDEVICE.Size = new System.Drawing.Size(31, 16);
            this.labelIPDEVICE.TabIndex = 4;
            this.labelIPDEVICE.Text = "aaa";
            this.labelIPDEVICE.Click += new System.EventHandler(this.labelIPDEVICE_Click);
            // 
            // groupBoxDEVICES
            // 
            this.groupBoxDEVICES.Controls.Add(this.listBoxDEVICES);
            this.groupBoxDEVICES.Location = new System.Drawing.Point(502, 64);
            this.groupBoxDEVICES.Name = "groupBoxDEVICES";
            this.groupBoxDEVICES.Size = new System.Drawing.Size(431, 150);
            this.groupBoxDEVICES.TabIndex = 5;
            this.groupBoxDEVICES.TabStop = false;
            this.groupBoxDEVICES.Text = "DEVICES";
            // 
            // listBoxDEVICES
            // 
            this.listBoxDEVICES.Enabled = false;
            this.listBoxDEVICES.FormattingEnabled = true;
            this.listBoxDEVICES.ItemHeight = 16;
            this.listBoxDEVICES.Location = new System.Drawing.Point(6, 26);
            this.listBoxDEVICES.Name = "listBoxDEVICES";
            this.listBoxDEVICES.Size = new System.Drawing.Size(419, 116);
            this.listBoxDEVICES.TabIndex = 0;
            this.listBoxDEVICES.SelectedIndexChanged += new System.EventHandler(this.listBoxDEVICES_SelectedIndexChanged);
            // 
            // progressBarDOWNLOAD
            // 
            this.progressBarDOWNLOAD.Location = new System.Drawing.Point(172, 547);
            this.progressBarDOWNLOAD.Name = "progressBarDOWNLOAD";
            this.progressBarDOWNLOAD.Size = new System.Drawing.Size(100, 23);
            this.progressBarDOWNLOAD.TabIndex = 6;
            // 
            // labelBUNDLEID
            // 
            this.labelBUNDLEID.AutoSize = true;
            this.labelBUNDLEID.Location = new System.Drawing.Point(58, 100);
            this.labelBUNDLEID.Name = "labelBUNDLEID";
            this.labelBUNDLEID.Size = new System.Drawing.Size(81, 16);
            this.labelBUNDLEID.TabIndex = 10;
            this.labelBUNDLEID.Text = "BUNDLE ID:";
            this.labelBUNDLEID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxBUNDLEID
            // 
            this.textBoxBUNDLEID.Location = new System.Drawing.Point(160, 100);
            this.textBoxBUNDLEID.Name = "textBoxBUNDLEID";
            this.textBoxBUNDLEID.Size = new System.Drawing.Size(100, 22);
            this.textBoxBUNDLEID.TabIndex = 11;
            this.textBoxBUNDLEID.TextChanged += new System.EventHandler(this.textBoxBUNDLEID_TextChanged);
            // 
            // WINDOW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 597);
            this.Controls.Add(this.progressBarDOWNLOAD);
            this.Controls.Add(this.groupBoxDEVICES);
            this.Controls.Add(this.labelIPDEVICE);
            this.Controls.Add(this.listBoxFILENAMES);
            this.Controls.Add(this.labelFILENAME);
            this.Controls.Add(this.labelCURRENTIP);
            this.Controls.Add(this.groupBoxSERVER);
            this.Controls.Add(this.groupIPConnect);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WINDOW";
            this.Text = "EASY CONNECTION";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupIPConnect.ResumeLayout(false);
            this.groupIPConnect.PerformLayout();
            this.groupBoxSERVER.ResumeLayout(false);
            this.groupBoxSERVER.PerformLayout();
            this.groupBoxDEVICES.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.GroupBox groupIPConnect;
        private System.Windows.Forms.TextBox textBoxPORT;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Button buttonCONNECT;
        private System.Windows.Forms.GroupBox groupBoxSERVER;
        private System.Windows.Forms.TextBox textBoxSERVERPORT;
        private System.Windows.Forms.TextBox textBoxSERVERIP;
        private System.Windows.Forms.Label labelSERVERPORT;
        private System.Windows.Forms.Label labelSERVERIP;
        private System.Windows.Forms.Button buttonSERVERCONNECTION;
        private System.Windows.Forms.Label labelFILENAME;
        private System.Windows.Forms.ListBox listBoxFILENAMES;
        private System.Windows.Forms.Button buttonDOWNLOAD;
        private System.Windows.Forms.Label labelCURRENTIP;
        private System.Windows.Forms.Label labelIPDEVICE;
        private System.Windows.Forms.GroupBox groupBoxDEVICES;
        private System.Windows.Forms.ListBox listBoxDEVICES;
        private System.Windows.Forms.ProgressBar progressBarDOWNLOAD;
        private System.Windows.Forms.Button buttonINSTALL;
        private System.Windows.Forms.Button buttonMOVE;
        private System.Windows.Forms.Label labelBUNDLEID;
        private System.Windows.Forms.TextBox textBoxBUNDLEID;
    }
}

