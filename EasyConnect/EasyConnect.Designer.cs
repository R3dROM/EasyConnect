using EasyConnect.Managers;

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
            buttonCONNECT = new Button();
            groupBoxSERVER = new GroupBox();
            buttonSTARTEXPERIENCE = new Button();
            CANCEL = new Button();
            numericUpDownDEVICESDEPLOYMENT = new NumericUpDown();
            label1 = new Label();
            buttonDeploy = new Button();
            buttonUninstall = new Button();
            labelCURRENTIP = new Label();
            labelIPDEVICE = new Label();
            textBoxServerIp = new TextBox();
            labelServerIp = new Label();
            labelBUNDLEID = new Label();
            labelBUNDLE = new Label();
            listBoxLogs = new ListBox();
            groupBoxDEVICES = new GroupBox();
            counter = new Label();
            dataGridView1 = new DataGridView();
            deviceManagerBindingSource = new BindingSource(components);
            deviceInfoBindingSource = new BindingSource(components);
            adbServiceBindingSource1 = new BindingSource(components);
            deviceReportBindingSource = new BindingSource(components);
            adbServiceBindingSource = new BindingSource(components);
            groupBox1 = new GroupBox();
            buttonDisconnect = new Button();
            progressBar = new ProgressBar();
            fileSystemWatcher1 = new FileSystemWatcher();
            labelAPKNAME = new Label();
            labelAPK = new Label();
            buttonDeployPath = new Button();
            deployPath = new TextBox();
            label3 = new Label();
            groupIPConnect.SuspendLayout();
            groupBoxSERVER.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDEVICESDEPLOYMENT).BeginInit();
            groupBoxDEVICES.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deviceManagerBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deviceInfoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deviceReportBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // groupIPConnect
            // 
            groupIPConnect.Controls.Add(textBoxPORT);
            groupIPConnect.Controls.Add(textBoxIP);
            groupIPConnect.Controls.Add(labelPort);
            groupIPConnect.Controls.Add(labelIP);
            groupIPConnect.Location = new Point(166, 12);
            groupIPConnect.Margin = new Padding(3, 2, 3, 2);
            groupIPConnect.Name = "groupIPConnect";
            groupIPConnect.Padding = new Padding(3, 2, 3, 2);
            groupIPConnect.Size = new Size(435, 76);
            groupIPConnect.TabIndex = 1;
            groupIPConnect.TabStop = false;
            groupIPConnect.Text = "Headset";
            // 
            // textBoxPORT
            // 
            textBoxPORT.Location = new Point(303, 22);
            textBoxPORT.Margin = new Padding(3, 2, 3, 2);
            textBoxPORT.Name = "textBoxPORT";
            textBoxPORT.Size = new Size(100, 27);
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
            textBoxIP.Size = new Size(151, 27);
            textBoxIP.TabIndex = 2;
            textBoxIP.TextChanged += textBoxIP_TextChanged;
            // 
            // labelPort
            // 
            labelPort.AutoSize = true;
            labelPort.Location = new Point(234, 29);
            labelPort.Name = "labelPort";
            labelPort.Size = new Size(42, 16);
            labelPort.TabIndex = 1;
            labelPort.Text = "PORT:";
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new Point(33, 30);
            labelIP.Name = "labelIP";
            labelIP.Size = new Size(22, 16);
            labelIP.TabIndex = 0;
            labelIP.Text = "IP:";
            // 
            // buttonCONNECT
            // 
            buttonCONNECT.Location = new Point(87, 21);
            buttonCONNECT.Margin = new Padding(3, 2, 3, 2);
            buttonCONNECT.Name = "buttonCONNECT";
            buttonCONNECT.Size = new Size(140, 30);
            buttonCONNECT.TabIndex = 4;
            buttonCONNECT.Text = "CONNECT";
            buttonCONNECT.UseVisualStyleBackColor = true;
            buttonCONNECT.Click += buttonCONNECT_Click;
            // 
            // groupBoxSERVER
            // 
            groupBoxSERVER.Controls.Add(buttonSTARTEXPERIENCE);
            groupBoxSERVER.Controls.Add(CANCEL);
            groupBoxSERVER.Controls.Add(numericUpDownDEVICESDEPLOYMENT);
            groupBoxSERVER.Controls.Add(label1);
            groupBoxSERVER.Controls.Add(buttonDeploy);
            groupBoxSERVER.Controls.Add(buttonUninstall);
            groupBoxSERVER.Controls.Add(labelCURRENTIP);
            groupBoxSERVER.Controls.Add(labelIPDEVICE);
            groupBoxSERVER.Location = new Point(629, 12);
            groupBoxSERVER.Margin = new Padding(3, 2, 3, 2);
            groupBoxSERVER.Name = "groupBoxSERVER";
            groupBoxSERVER.Padding = new Padding(3, 2, 3, 2);
            groupBoxSERVER.Size = new Size(254, 262);
            groupBoxSERVER.TabIndex = 2;
            groupBoxSERVER.TabStop = false;
            groupBoxSERVER.Text = "Actions";
            // 
            // buttonSTARTEXPERIENCE
            // 
            buttonSTARTEXPERIENCE.Location = new Point(45, 206);
            buttonSTARTEXPERIENCE.Margin = new Padding(3, 2, 3, 2);
            buttonSTARTEXPERIENCE.Name = "buttonSTARTEXPERIENCE";
            buttonSTARTEXPERIENCE.Size = new Size(170, 30);
            buttonSTARTEXPERIENCE.TabIndex = 23;
            buttonSTARTEXPERIENCE.Text = "START EXPERIENCE";
            buttonSTARTEXPERIENCE.UseVisualStyleBackColor = true;
            buttonSTARTEXPERIENCE.Click += buttonSTARTEXPERIENCE_Click;
            // 
            // CANCEL
            // 
            CANCEL.Location = new Point(45, 137);
            CANCEL.Name = "CANCEL";
            CANCEL.Size = new Size(170, 30);
            CANCEL.TabIndex = 22;
            CANCEL.Text = "CANCEL DEPLOY";
            CANCEL.UseVisualStyleBackColor = true;
            CANCEL.Click += CANCEL_Click;
            // 
            // numericUpDownDEVICESDEPLOYMENT
            // 
            numericUpDownDEVICESDEPLOYMENT.Location = new Point(191, 52);
            numericUpDownDEVICESDEPLOYMENT.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDownDEVICESDEPLOYMENT.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDEVICESDEPLOYMENT.Name = "numericUpDownDEVICESDEPLOYMENT";
            numericUpDownDEVICESDEPLOYMENT.Size = new Size(50, 27);
            numericUpDownDEVICESDEPLOYMENT.TabIndex = 20;
            numericUpDownDEVICESDEPLOYMENT.TextAlign = HorizontalAlignment.Center;
            numericUpDownDEVICESDEPLOYMENT.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 55);
            label1.Name = "label1";
            label1.Size = new Size(140, 16);
            label1.TabIndex = 15;
            label1.Text = "MAX DEVICES DEPLOY:";
            // 
            // buttonDeploy
            // 
            buttonDeploy.Location = new Point(45, 101);
            buttonDeploy.Name = "buttonDeploy";
            buttonDeploy.Size = new Size(170, 30);
            buttonDeploy.TabIndex = 16;
            buttonDeploy.Text = "START DEPLOY";
            buttonDeploy.UseVisualStyleBackColor = true;
            buttonDeploy.Click += buttonDeploy_Click;
            // 
            // buttonUninstall
            // 
            buttonUninstall.Location = new Point(45, 172);
            buttonUninstall.Margin = new Padding(3, 2, 3, 2);
            buttonUninstall.Name = "buttonUninstall";
            buttonUninstall.Size = new Size(170, 30);
            buttonUninstall.TabIndex = 15;
            buttonUninstall.Text = "UNINSTALL";
            buttonUninstall.UseVisualStyleBackColor = true;
            buttonUninstall.Click += buttonUninstall_Click;
            // 
            // labelCURRENTIP
            // 
            labelCURRENTIP.AutoSize = true;
            labelCURRENTIP.Location = new Point(15, 20);
            labelCURRENTIP.Name = "labelCURRENTIP";
            labelCURRENTIP.Size = new Size(82, 16);
            labelCURRENTIP.TabIndex = 3;
            labelCURRENTIP.Text = "CURRENT IP:";
            // 
            // labelIPDEVICE
            // 
            labelIPDEVICE.AutoSize = true;
            labelIPDEVICE.Location = new Point(114, 20);
            labelIPDEVICE.Name = "labelIPDEVICE";
            labelIPDEVICE.Size = new Size(58, 16);
            labelIPDEVICE.TabIndex = 4;
            labelIPDEVICE.Text = "127.0.0.1";
            // 
            // textBoxServerIp
            // 
            textBoxServerIp.Location = new Point(289, 171);
            textBoxServerIp.Name = "textBoxServerIp";
            textBoxServerIp.Size = new Size(147, 27);
            textBoxServerIp.TabIndex = 18;
            textBoxServerIp.TextChanged += textBoxServerIp_TextChanged;
            // 
            // labelServerIp
            // 
            labelServerIp.AutoSize = true;
            labelServerIp.Location = new Point(169, 174);
            labelServerIp.Name = "labelServerIp";
            labelServerIp.Size = new Size(71, 16);
            labelServerIp.TabIndex = 17;
            labelServerIp.Text = "SERVER IP:";
            labelServerIp.TextAlign = ContentAlignment.BottomLeft;
            // 
            // labelBUNDLEID
            // 
            labelBUNDLEID.AutoSize = true;
            labelBUNDLEID.Location = new Point(169, 254);
            labelBUNDLEID.Name = "labelBUNDLEID";
            labelBUNDLEID.Size = new Size(75, 16);
            labelBUNDLEID.TabIndex = 10;
            labelBUNDLEID.Text = "BUNDLE ID:";
            labelBUNDLEID.TextAlign = ContentAlignment.BottomLeft;
            // 
            // labelBUNDLE
            // 
            labelBUNDLE.AutoSize = true;
            labelBUNDLE.Location = new Point(289, 254);
            labelBUNDLE.Margin = new Padding(4, 0, 4, 0);
            labelBUNDLE.Name = "labelBUNDLE";
            labelBUNDLE.Size = new Size(83, 16);
            labelBUNDLE.TabIndex = 11;
            labelBUNDLE.Text = "com.exam.ple";
            // 
            // listBoxLogs
            // 
            listBoxLogs.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxLogs.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxLogs.FormattingEnabled = true;
            listBoxLogs.HorizontalScrollbar = true;
            listBoxLogs.Location = new Point(86, 602);
            listBoxLogs.Margin = new Padding(3, 2, 3, 2);
            listBoxLogs.Name = "listBoxLogs";
            listBoxLogs.Size = new Size(857, 116);
            listBoxLogs.TabIndex = 6;
            // 
            // groupBoxDEVICES
            // 
            groupBoxDEVICES.Controls.Add(counter);
            groupBoxDEVICES.Controls.Add(dataGridView1);
            groupBoxDEVICES.Location = new Point(83, 327);
            groupBoxDEVICES.Margin = new Padding(3, 2, 3, 2);
            groupBoxDEVICES.Name = "groupBoxDEVICES";
            groupBoxDEVICES.Padding = new Padding(3, 2, 3, 2);
            groupBoxDEVICES.Size = new Size(860, 255);
            groupBoxDEVICES.TabIndex = 5;
            groupBoxDEVICES.TabStop = false;
            groupBoxDEVICES.Text = "DEVICES";
            // 
            // counter
            // 
            counter.AutoSize = true;
            counter.Location = new Point(86, 0);
            counter.Name = "counter";
            counter.Size = new Size(14, 16);
            counter.TabIndex = 23;
            counter.Text = "0";
            // 
            // dataGridView1
            // 
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
            // 
            // deviceManagerBindingSource
            // 
            deviceManagerBindingSource.DataSource = typeof(DeviceManager);
            // 
            // deviceInfoBindingSource
            // 
            deviceInfoBindingSource.DataSource = typeof(Models.DeviceInfo);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(buttonDisconnect);
            groupBox1.Controls.Add(buttonCONNECT);
            groupBox1.Location = new Point(166, 101);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(435, 61);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Connection";
            // 
            // buttonDisconnect
            // 
            buttonDisconnect.Location = new Point(236, 21);
            buttonDisconnect.Margin = new Padding(3, 2, 3, 2);
            buttonDisconnect.Name = "buttonDisconnect";
            buttonDisconnect.Size = new Size(153, 30);
            buttonDisconnect.TabIndex = 13;
            buttonDisconnect.Text = "DISCONNECT";
            buttonDisconnect.UseVisualStyleBackColor = true;
            buttonDisconnect.Click += buttonDisconnect_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(86, 742);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(857, 29);
            progressBar.TabIndex = 16;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // labelAPKNAME
            // 
            labelAPKNAME.AutoSize = true;
            labelAPKNAME.Location = new Point(169, 285);
            labelAPKNAME.Name = "labelAPKNAME";
            labelAPKNAME.Size = new Size(72, 16);
            labelAPKNAME.TabIndex = 19;
            labelAPKNAME.Text = "APK NAME:";
            labelAPKNAME.TextAlign = ContentAlignment.BottomLeft;
            // 
            // labelAPK
            // 
            labelAPK.AutoSize = true;
            labelAPK.Location = new Point(289, 285);
            labelAPK.Margin = new Padding(4, 0, 4, 0);
            labelAPK.Name = "labelAPK";
            labelAPK.Size = new Size(83, 16);
            labelAPK.TabIndex = 20;
            labelAPK.Text = "com.exam.ple";
            // 
            // buttonDeployPath
            // 
            buttonDeployPath.Location = new Point(442, 212);
            buttonDeployPath.Name = "buttonDeployPath";
            buttonDeployPath.Size = new Size(94, 29);
            buttonDeployPath.TabIndex = 23;
            buttonDeployPath.Text = "Browse";
            buttonDeployPath.UseVisualStyleBackColor = true;
            buttonDeployPath.Click += buttonDeployPath_Click;
            // 
            // deployPath
            // 
            deployPath.BackColor = SystemColors.Window;
            deployPath.Font = new Font("Segoe UI", 9F);
            deployPath.Location = new Point(289, 212);
            deployPath.Name = "deployPath";
            deployPath.ReadOnly = true;
            deployPath.Size = new Size(147, 23);
            deployPath.TabIndex = 22;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(169, 216);
            label3.Name = "label3";
            label3.Size = new Size(93, 16);
            label3.TabIndex = 21;
            label3.Text = "DEPLOY PATH:";
            // 
            // WINDOW
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1056, 803);
            Controls.Add(buttonDeployPath);
            Controls.Add(deployPath);
            Controls.Add(label3);
            Controls.Add(labelAPKNAME);
            Controls.Add(labelAPK);
            Controls.Add(progressBar);
            Controls.Add(textBoxServerIp);
            Controls.Add(groupBox1);
            Controls.Add(labelServerIp);
            Controls.Add(groupBoxDEVICES);
            Controls.Add(labelBUNDLEID);
            Controls.Add(labelBUNDLE);
            Controls.Add(listBoxLogs);
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
            groupBoxSERVER.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDEVICESDEPLOYMENT).EndInit();
            groupBoxDEVICES.ResumeLayout(false);
            groupBoxDEVICES.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)deviceManagerBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)deviceInfoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)deviceReportBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
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
        private System.Windows.Forms.Label labelCURRENTIP;
        private System.Windows.Forms.Label labelIPDEVICE;
        private System.Windows.Forms.GroupBox groupBoxDEVICES;
        private System.Windows.Forms.Label labelBUNDLEID;
        private System.Windows.Forms.Label labelBUNDLE;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource adbServiceBindingSource;
        private System.Windows.Forms.BindingSource adbServiceBindingSource1;
        private System.Windows.Forms.BindingSource deviceReportBindingSource;
        private GroupBox groupBox1;
        private Button buttonUninstall;
        private ProgressBar progressBar;
        private Label labelServerIp;
        private TextBox textBoxServerIp;
        private Button buttonDeploy;
        private FileSystemWatcher fileSystemWatcher1;
        private Button buttonDisconnect;
        private Label labelAPKNAME;
        private Label labelAPK;
        private Label label1;
        private NumericUpDown numericUpDownDEVICESDEPLOYMENT;
        private BindingSource deviceManagerBindingSource;
        private BindingSource deviceInfoBindingSource;
        private Button CANCEL;
        private Label counter;
        private Button buttonDeployPath;
        private TextBox deployPath;
        private Label label3;
        private Button buttonSTARTEXPERIENCE;
    }
}

