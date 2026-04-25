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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WINDOW));
            groupIPConnect = new GroupBox();
            textBoxPORT = new TextBox();
            textBoxIP = new TextBox();
            labelPort = new Label();
            labelIP = new Label();
            buttonAUTOSCANN = new Button();
            buttonCONNECT = new Button();
            groupBoxSERVER = new GroupBox();
            buttonDeploy = new Button();
            buttonUninstall = new Button();
            buttonINSTALL = new Button();
            buttonWEBSOCKETCONNECTION = new Button();
            buttonRESETADB = new Button();
            buttonMOVE = new Button();
            buttonDOWNLOAD = new Button();
            buttonNETWORKING = new Button();
            label3 = new Label();
            textBoxNEWDEVICE = new TextBox();
            labelBUNDLEID = new Label();
            listBoxLogs = new ListBox();
            labelCURRENTIP = new Label();
            labelIPDEVICE = new Label();
            groupBoxDEVICES = new GroupBox();
            dataGridView1 = new DataGridView();
            labelBUNDLE = new Label();
            deviceReportBindingSource = new BindingSource(components);
            adbServiceBindingSource = new BindingSource(components);
            adbServiceBindingSource1 = new BindingSource(components);
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            progressBar = new ProgressBar();
            labelServerIp = new Label();
            textBoxServerIp = new TextBox();
            buttonGenerate = new Button();
            groupIPConnect.SuspendLayout();
            groupBoxSERVER.SuspendLayout();
            groupBoxDEVICES.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deviceReportBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupIPConnect
            // 
            groupIPConnect.Controls.Add(textBoxPORT);
            groupIPConnect.Controls.Add(textBoxIP);
            groupIPConnect.Controls.Add(labelPort);
            groupIPConnect.Controls.Add(labelIP);
            groupIPConnect.Location = new Point(68, 51);
            groupIPConnect.Margin = new Padding(3, 2, 3, 2);
            groupIPConnect.Name = "groupIPConnect";
            groupIPConnect.Padding = new Padding(3, 2, 3, 2);
            groupIPConnect.Size = new Size(245, 102);
            groupIPConnect.TabIndex = 1;
            groupIPConnect.TabStop = false;
            groupIPConnect.Text = "Headset";
            // 
            // textBoxPORT
            // 
            textBoxPORT.Location = new Point(76, 60);
            textBoxPORT.Margin = new Padding(3, 2, 3, 2);
            textBoxPORT.Name = "textBoxPORT";
            textBoxPORT.Size = new Size(100, 32);
            textBoxPORT.TabIndex = 3;
            textBoxPORT.Text = "5555";
            textBoxPORT.TextAlign = HorizontalAlignment.Center;
            textBoxPORT.TextChanged += textBoxPORT_TextChanged;
            // 
            // textBoxIP
            // 
            textBoxIP.Location = new Point(76, 22);
            textBoxIP.Margin = new Padding(3, 2, 3, 2);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.Size = new Size(151, 32);
            textBoxIP.TabIndex = 2;
            textBoxIP.TextChanged += textBoxIP_TextChanged;
            // 
            // labelPort
            // 
            labelPort.AutoSize = true;
            labelPort.Location = new Point(7, 67);
            labelPort.Name = "labelPort";
            labelPort.Size = new Size(51, 20);
            labelPort.TabIndex = 1;
            labelPort.Text = "PORT:";
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new Point(33, 30);
            labelIP.Name = "labelIP";
            labelIP.Size = new Size(27, 20);
            labelIP.TabIndex = 0;
            labelIP.Text = "IP:";
            // 
            // buttonAUTOSCANN
            // 
            buttonAUTOSCANN.Location = new Point(192, 23);
            buttonAUTOSCANN.Margin = new Padding(3, 2, 3, 2);
            buttonAUTOSCANN.Name = "buttonAUTOSCANN";
            buttonAUTOSCANN.Size = new Size(137, 31);
            buttonAUTOSCANN.TabIndex = 12;
            buttonAUTOSCANN.Text = "AUTO SCAN";
            buttonAUTOSCANN.UseVisualStyleBackColor = true;
            buttonAUTOSCANN.Click += buttonAUTOSCANN_Click;
            // 
            // buttonCONNECT
            // 
            buttonCONNECT.Location = new Point(59, 23);
            buttonCONNECT.Margin = new Padding(3, 2, 3, 2);
            buttonCONNECT.Name = "buttonCONNECT";
            buttonCONNECT.Size = new Size(109, 31);
            buttonCONNECT.TabIndex = 4;
            buttonCONNECT.Text = "CONNECT";
            buttonCONNECT.UseVisualStyleBackColor = true;
            buttonCONNECT.Click += buttonCONNECT_Click;
            // 
            // groupBoxSERVER
            // 
            groupBoxSERVER.Controls.Add(buttonDeploy);
            groupBoxSERVER.Controls.Add(buttonUninstall);
            groupBoxSERVER.Controls.Add(buttonINSTALL);
            groupBoxSERVER.Controls.Add(buttonWEBSOCKETCONNECTION);
            groupBoxSERVER.Controls.Add(buttonRESETADB);
            groupBoxSERVER.Controls.Add(buttonMOVE);
            groupBoxSERVER.Controls.Add(buttonDOWNLOAD);
            groupBoxSERVER.Controls.Add(buttonNETWORKING);
            groupBoxSERVER.Location = new Point(473, 52);
            groupBoxSERVER.Margin = new Padding(3, 2, 3, 2);
            groupBoxSERVER.Name = "groupBoxSERVER";
            groupBoxSERVER.Padding = new Padding(3, 2, 3, 2);
            groupBoxSERVER.Size = new Size(361, 166);
            groupBoxSERVER.TabIndex = 2;
            groupBoxSERVER.TabStop = false;
            groupBoxSERVER.Text = "Actions";
            // 
            // buttonDeploy
            // 
            buttonDeploy.Location = new Point(182, 125);
            buttonDeploy.Name = "buttonDeploy";
            buttonDeploy.Size = new Size(170, 30);
            buttonDeploy.TabIndex = 16;
            buttonDeploy.Text = "DEPLOY";
            buttonDeploy.UseVisualStyleBackColor = true;
            buttonDeploy.Click += buttonDeploy_Click;
            // 
            // buttonUninstall
            // 
            buttonUninstall.Location = new Point(182, 90);
            buttonUninstall.Margin = new Padding(3, 2, 3, 2);
            buttonUninstall.Name = "buttonUninstall";
            buttonUninstall.Size = new Size(170, 30);
            buttonUninstall.TabIndex = 15;
            buttonUninstall.Text = "UNINSTALL";
            buttonUninstall.UseVisualStyleBackColor = true;
            buttonUninstall.Click += buttonUninstall_Click;
            // 
            // buttonINSTALL
            // 
            buttonINSTALL.Location = new Point(6, 125);
            buttonINSTALL.Margin = new Padding(3, 2, 3, 2);
            buttonINSTALL.Name = "buttonINSTALL";
            buttonINSTALL.Size = new Size(170, 30);
            buttonINSTALL.TabIndex = 9;
            buttonINSTALL.Text = "INSTALL";
            buttonINSTALL.UseVisualStyleBackColor = true;
            buttonINSTALL.Click += buttonINSTALL_Click;
            // 
            // buttonWEBSOCKETCONNECTION
            // 
            buttonWEBSOCKETCONNECTION.Location = new Point(6, 20);
            buttonWEBSOCKETCONNECTION.Margin = new Padding(3, 2, 3, 2);
            buttonWEBSOCKETCONNECTION.Name = "buttonWEBSOCKETCONNECTION";
            buttonWEBSOCKETCONNECTION.Size = new Size(170, 30);
            buttonWEBSOCKETCONNECTION.TabIndex = 12;
            buttonWEBSOCKETCONNECTION.Text = "STOP WEB SOCKET";
            buttonWEBSOCKETCONNECTION.UseVisualStyleBackColor = true;
            buttonWEBSOCKETCONNECTION.Click += buttonWEBSOCKETCONNECTION_Click;
            // 
            // buttonRESETADB
            // 
            buttonRESETADB.Location = new Point(182, 20);
            buttonRESETADB.Name = "buttonRESETADB";
            buttonRESETADB.Size = new Size(170, 30);
            buttonRESETADB.TabIndex = 13;
            buttonRESETADB.Text = "RESTART ADB";
            buttonRESETADB.UseVisualStyleBackColor = true;
            buttonRESETADB.Click += buttonRESETADB_Click;
            // 
            // buttonMOVE
            // 
            buttonMOVE.Location = new Point(6, 90);
            buttonMOVE.Margin = new Padding(3, 2, 3, 2);
            buttonMOVE.Name = "buttonMOVE";
            buttonMOVE.Size = new Size(170, 30);
            buttonMOVE.TabIndex = 8;
            buttonMOVE.Text = "MOVE";
            buttonMOVE.UseVisualStyleBackColor = true;
            buttonMOVE.Click += buttonMOVE_Click;
            // 
            // buttonDOWNLOAD
            // 
            buttonDOWNLOAD.Location = new Point(182, 54);
            buttonDOWNLOAD.Margin = new Padding(3, 2, 3, 2);
            buttonDOWNLOAD.Name = "buttonDOWNLOAD";
            buttonDOWNLOAD.Size = new Size(170, 30);
            buttonDOWNLOAD.TabIndex = 7;
            buttonDOWNLOAD.Text = "DOWNLOAD";
            buttonDOWNLOAD.UseVisualStyleBackColor = true;
            buttonDOWNLOAD.Click += buttonDOWNLOAD_Click;
            // 
            // buttonNETWORKING
            // 
            buttonNETWORKING.Location = new Point(6, 55);
            buttonNETWORKING.Name = "buttonNETWORKING";
            buttonNETWORKING.Size = new Size(170, 30);
            buttonNETWORKING.TabIndex = 14;
            buttonNETWORKING.Text = "CONFIGURATION";
            buttonNETWORKING.UseVisualStyleBackColor = true;
            buttonNETWORKING.Click += buttonNETWORKING_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 27);
            label3.Name = "label3";
            label3.Size = new Size(54, 20);
            label3.TabIndex = 13;
            label3.Text = "CODE:";
            // 
            // textBoxNEWDEVICE
            // 
            textBoxNEWDEVICE.Location = new Point(63, 21);
            textBoxNEWDEVICE.Margin = new Padding(4, 5, 4, 5);
            textBoxNEWDEVICE.Name = "textBoxNEWDEVICE";
            textBoxNEWDEVICE.Size = new Size(72, 32);
            textBoxNEWDEVICE.TabIndex = 7;
            textBoxNEWDEVICE.TextChanged += textBoxNEWDEVICE_TextChanged;
            // 
            // labelBUNDLEID
            // 
            labelBUNDLEID.AutoSize = true;
            labelBUNDLEID.Location = new Point(68, 231);
            labelBUNDLEID.Name = "labelBUNDLEID";
            labelBUNDLEID.Size = new Size(93, 20);
            labelBUNDLEID.TabIndex = 10;
            labelBUNDLEID.Text = "BUNDLE ID:";
            labelBUNDLEID.TextAlign = ContentAlignment.BottomLeft;
            // 
            // listBoxLogs
            // 
            listBoxLogs.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLogs.FormattingEnabled = true;
            listBoxLogs.HorizontalScrollbar = true;
            listBoxLogs.Location = new Point(32, 564);
            listBoxLogs.Margin = new Padding(3, 2, 3, 2);
            listBoxLogs.Name = "listBoxLogs";
            listBoxLogs.Size = new Size(857, 164);
            listBoxLogs.TabIndex = 6;
            listBoxLogs.SelectedIndexChanged += listBoxFILENAMES_SelectedIndexChanged;
            // 
            // labelCURRENTIP
            // 
            labelCURRENTIP.AutoSize = true;
            labelCURRENTIP.Location = new Point(117, 29);
            labelCURRENTIP.Name = "labelCURRENTIP";
            labelCURRENTIP.Size = new Size(101, 20);
            labelCURRENTIP.TabIndex = 3;
            labelCURRENTIP.Text = "CURRENT IP:";
            // 
            // labelIPDEVICE
            // 
            labelIPDEVICE.AutoSize = true;
            labelIPDEVICE.Location = new Point(216, 29);
            labelIPDEVICE.Name = "labelIPDEVICE";
            labelIPDEVICE.Size = new Size(33, 20);
            labelIPDEVICE.TabIndex = 4;
            labelIPDEVICE.Text = "aaa";
            labelIPDEVICE.Click += labelIPDEVICE_Click;
            // 
            // groupBoxDEVICES
            // 
            groupBoxDEVICES.Controls.Add(dataGridView1);
            groupBoxDEVICES.Location = new Point(29, 294);
            groupBoxDEVICES.Margin = new Padding(3, 2, 3, 2);
            groupBoxDEVICES.Name = "groupBoxDEVICES";
            groupBoxDEVICES.Padding = new Padding(3, 2, 3, 2);
            groupBoxDEVICES.Size = new Size(860, 255);
            groupBoxDEVICES.TabIndex = 5;
            groupBoxDEVICES.TabStop = false;
            groupBoxDEVICES.Text = "DEVICES";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.Menu;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Yu Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.Desktop;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Menu;
            dataGridViewCellStyle3.Font = new Font("Yu Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.Desktop;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.GridColor = SystemColors.ActiveCaptionText;
            dataGridView1.Location = new Point(7, 22);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.MaximumSize = new Size(1100, 225);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Yu Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.Desktop;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.Aquamarine;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(846, 225);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // labelBUNDLE
            // 
            labelBUNDLE.AutoSize = true;
            labelBUNDLE.Location = new Point(166, 230);
            labelBUNDLE.Margin = new Padding(4, 0, 4, 0);
            labelBUNDLE.Name = "labelBUNDLE";
            labelBUNDLE.Size = new Size(103, 20);
            labelBUNDLE.TabIndex = 11;
            labelBUNDLE.Text = "com.exam.ple";
            labelBUNDLE.Click += labelBUNDLE_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(buttonAUTOSCANN);
            groupBox1.Controls.Add(buttonCONNECT);
            groupBox1.Location = new Point(68, 157);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(399, 61);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Connection";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(textBoxNEWDEVICE);
            groupBox2.Location = new Point(319, 51);
            groupBox2.Margin = new Padding(3, 2, 3, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 2, 3, 2);
            groupBox2.Size = new Size(148, 60);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Android";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(32, 746);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(850, 29);
            progressBar.TabIndex = 16;
            // 
            // labelServerIp
            // 
            labelServerIp.AutoSize = true;
            labelServerIp.Location = new Point(68, 262);
            labelServerIp.Name = "labelServerIp";
            labelServerIp.Size = new Size(88, 20);
            labelServerIp.TabIndex = 17;
            labelServerIp.Text = "SERVER IP:";
            labelServerIp.TextAlign = ContentAlignment.BottomLeft;
            // 
            // textBoxServerIp
            // 
            textBoxServerIp.Location = new Point(166, 253);
            textBoxServerIp.Name = "textBoxServerIp";
            textBoxServerIp.Size = new Size(147, 32);
            textBoxServerIp.TabIndex = 18;
            textBoxServerIp.TextChanged += textBoxServerIp_TextChanged;
            // 
            // buttonGenerate
            // 
            buttonGenerate.DialogResult = DialogResult.OK;
            buttonGenerate.Location = new Point(325, 256);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(140, 30);
            buttonGenerate.TabIndex = 19;
            buttonGenerate.Text = "GENERATE";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            // 
            // WINDOW
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            BackColor = SystemColors.Control;
            ClientSize = new Size(917, 803);
            Controls.Add(buttonGenerate);
            Controls.Add(textBoxServerIp);
            Controls.Add(labelServerIp);
            Controls.Add(progressBar);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(labelBUNDLE);
            Controls.Add(labelBUNDLEID);
            Controls.Add(groupBoxDEVICES);
            Controls.Add(labelIPDEVICE);
            Controls.Add(listBoxLogs);
            Controls.Add(labelCURRENTIP);
            Controls.Add(groupBoxSERVER);
            Controls.Add(groupIPConnect);
            Font = new Font("Yu Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "WINDOW";
            Text = "EASY LINK";
            Load += Form1_Load;
            groupIPConnect.ResumeLayout(false);
            groupIPConnect.PerformLayout();
            groupBoxSERVER.ResumeLayout(false);
            groupBoxDEVICES.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)deviceReportBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupIPConnect;
        private System.Windows.Forms.TextBox textBoxPORT;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Button buttonCONNECT;
        private System.Windows.Forms.GroupBox groupBoxSERVER;
        private System.Windows.Forms.ListBox listBoxLogs;
        private System.Windows.Forms.Button buttonDOWNLOAD;
        private System.Windows.Forms.Label labelCURRENTIP;
        private System.Windows.Forms.Label labelIPDEVICE;
        private System.Windows.Forms.GroupBox groupBoxDEVICES;
        private System.Windows.Forms.Button buttonINSTALL;
        private System.Windows.Forms.Button buttonMOVE;
        private System.Windows.Forms.Label labelBUNDLEID;
        private System.Windows.Forms.Label labelBUNDLE;
        private System.Windows.Forms.Button buttonAUTOSCANN;
        private System.Windows.Forms.Button buttonWEBSOCKETCONNECTION;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource adbServiceBindingSource;
        private System.Windows.Forms.BindingSource adbServiceBindingSource1;
        private System.Windows.Forms.BindingSource deviceReportBindingSource;
        private Button buttonRESETADB;
        private Button buttonNETWORKING;
        private Label label3;
        private TextBox textBoxNEWDEVICE;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button buttonUninstall;
        private ProgressBar progressBar;
        private Label labelServerIp;
        private TextBox textBoxServerIp;
        private Button buttonGenerate;
        private Button buttonDeploy;
    }
}

