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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Title = new System.Windows.Forms.Label();
            this.groupIPConnect = new System.Windows.Forms.GroupBox();
            this.buttonAUTOSCANN = new System.Windows.Forms.Button();
            this.textBoxNEWDEVICE = new System.Windows.Forms.TextBox();
            this.checkBoxNEWDEVICE = new System.Windows.Forms.CheckBox();
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
            this.labelBUNDLEID = new System.Windows.Forms.Label();
            this.listBoxFILENAMES = new System.Windows.Forms.ListBox();
            this.labelFILENAME = new System.Windows.Forms.Label();
            this.labelCURRENTIP = new System.Windows.Forms.Label();
            this.labelIPDEVICE = new System.Windows.Forms.Label();
            this.groupBoxDEVICES = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labelBUNDLE = new System.Windows.Forms.Label();
            this.buttonWEBSOCKETCONNECTION = new System.Windows.Forms.Button();
            this.deviceReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.adbServiceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.adbServiceBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.groupIPConnect.SuspendLayout();
            this.groupBoxSERVER.SuspendLayout();
            this.groupBoxDEVICES.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceReportBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adbServiceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adbServiceBindingSource1)).BeginInit();
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
            this.groupIPConnect.Controls.Add(this.buttonAUTOSCANN);
            this.groupIPConnect.Controls.Add(this.textBoxNEWDEVICE);
            this.groupIPConnect.Controls.Add(this.checkBoxNEWDEVICE);
            this.groupIPConnect.Controls.Add(this.buttonCONNECT);
            this.groupIPConnect.Controls.Add(this.textBoxPORT);
            this.groupIPConnect.Controls.Add(this.textBoxIP);
            this.groupIPConnect.Controls.Add(this.labelPort);
            this.groupIPConnect.Controls.Add(this.labelIP);
            this.groupIPConnect.Location = new System.Drawing.Point(28, 64);
            this.groupIPConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupIPConnect.Name = "groupIPConnect";
            this.groupIPConnect.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupIPConnect.Size = new System.Drawing.Size(399, 193);
            this.groupIPConnect.TabIndex = 1;
            this.groupIPConnect.TabStop = false;
            this.groupIPConnect.Text = "IP Connection";
            // 
            // buttonAUTOSCANN
            // 
            this.buttonAUTOSCANN.Location = new System.Drawing.Point(245, 153);
            this.buttonAUTOSCANN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAUTOSCANN.Name = "buttonAUTOSCANN";
            this.buttonAUTOSCANN.Size = new System.Drawing.Size(137, 23);
            this.buttonAUTOSCANN.TabIndex = 12;
            this.buttonAUTOSCANN.Text = "AUTO SCANN";
            this.buttonAUTOSCANN.UseVisualStyleBackColor = true;
            this.buttonAUTOSCANN.Click += new System.EventHandler(this.buttonAUTOSCANN_Click);
            // 
            // textBoxNEWDEVICE
            // 
            this.textBoxNEWDEVICE.Location = new System.Drawing.Point(160, 106);
            this.textBoxNEWDEVICE.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNEWDEVICE.Name = "textBoxNEWDEVICE";
            this.textBoxNEWDEVICE.Size = new System.Drawing.Size(132, 22);
            this.textBoxNEWDEVICE.TabIndex = 7;
            this.textBoxNEWDEVICE.Visible = false;
            this.textBoxNEWDEVICE.TextChanged += new System.EventHandler(this.textBoxNEWDEVICE_TextChanged);
            // 
            // checkBoxNEWDEVICE
            // 
            this.checkBoxNEWDEVICE.AutoSize = true;
            this.checkBoxNEWDEVICE.Location = new System.Drawing.Point(27, 110);
            this.checkBoxNEWDEVICE.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxNEWDEVICE.Name = "checkBoxNEWDEVICE";
            this.checkBoxNEWDEVICE.Size = new System.Drawing.Size(113, 20);
            this.checkBoxNEWDEVICE.TabIndex = 6;
            this.checkBoxNEWDEVICE.Text = "NEW DEVICE";
            this.checkBoxNEWDEVICE.UseVisualStyleBackColor = true;
            this.checkBoxNEWDEVICE.CheckedChanged += new System.EventHandler(this.checkBoxNEWDEVICE_CheckedChanged);
            // 
            // buttonCONNECT
            // 
            this.buttonCONNECT.Location = new System.Drawing.Point(85, 151);
            this.buttonCONNECT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCONNECT.Name = "buttonCONNECT";
            this.buttonCONNECT.Size = new System.Drawing.Size(109, 25);
            this.buttonCONNECT.TabIndex = 4;
            this.buttonCONNECT.Text = "CONNECT";
            this.buttonCONNECT.UseVisualStyleBackColor = true;
            this.buttonCONNECT.Click += new System.EventHandler(this.buttonCONNECT_Click);
            // 
            // textBoxPORT
            // 
            this.textBoxPORT.Location = new System.Drawing.Point(160, 63);
            this.textBoxPORT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(151, 22);
            this.textBoxIP.TabIndex = 2;
            this.textBoxIP.TextChanged += new System.EventHandler(this.textBoxIP_TextChanged);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(91, 69);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(48, 16);
            this.labelPort.TabIndex = 1;
            this.labelPort.Text = "PORT:";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(117, 32);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(22, 16);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "IP:";
            // 
            // groupBoxSERVER
            // 
            this.groupBoxSERVER.Controls.Add(this.buttonINSTALL);
            this.groupBoxSERVER.Controls.Add(this.buttonMOVE);
            this.groupBoxSERVER.Controls.Add(this.buttonDOWNLOAD);
            this.groupBoxSERVER.Controls.Add(this.buttonSERVERCONNECTION);
            this.groupBoxSERVER.Controls.Add(this.textBoxSERVERPORT);
            this.groupBoxSERVER.Controls.Add(this.textBoxSERVERIP);
            this.groupBoxSERVER.Controls.Add(this.labelSERVERPORT);
            this.groupBoxSERVER.Controls.Add(this.labelSERVERIP);
            this.groupBoxSERVER.Location = new System.Drawing.Point(444, 64);
            this.groupBoxSERVER.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSERVER.Name = "groupBoxSERVER";
            this.groupBoxSERVER.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSERVER.Size = new System.Drawing.Size(396, 193);
            this.groupBoxSERVER.TabIndex = 2;
            this.groupBoxSERVER.TabStop = false;
            this.groupBoxSERVER.Text = "SERVER Connection";
            // 
            // buttonINSTALL
            // 
            this.buttonINSTALL.Location = new System.Drawing.Point(267, 152);
            this.buttonINSTALL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonINSTALL.Name = "buttonINSTALL";
            this.buttonINSTALL.Size = new System.Drawing.Size(91, 23);
            this.buttonINSTALL.TabIndex = 9;
            this.buttonINSTALL.Text = "INSTALL";
            this.buttonINSTALL.UseVisualStyleBackColor = true;
            this.buttonINSTALL.Click += new System.EventHandler(this.buttonINSTALL_Click);
            // 
            // buttonMOVE
            // 
            this.buttonMOVE.Location = new System.Drawing.Point(171, 152);
            this.buttonMOVE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMOVE.Name = "buttonMOVE";
            this.buttonMOVE.Size = new System.Drawing.Size(75, 23);
            this.buttonMOVE.TabIndex = 8;
            this.buttonMOVE.Text = "MOVE";
            this.buttonMOVE.UseVisualStyleBackColor = true;
            this.buttonMOVE.Click += new System.EventHandler(this.buttonMOVE_Click);
            // 
            // buttonDOWNLOAD
            // 
            this.buttonDOWNLOAD.Location = new System.Drawing.Point(43, 152);
            this.buttonDOWNLOAD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDOWNLOAD.Name = "buttonDOWNLOAD";
            this.buttonDOWNLOAD.Size = new System.Drawing.Size(109, 23);
            this.buttonDOWNLOAD.TabIndex = 7;
            this.buttonDOWNLOAD.Text = "DOWNLOAD";
            this.buttonDOWNLOAD.UseVisualStyleBackColor = true;
            this.buttonDOWNLOAD.Click += new System.EventHandler(this.buttonDOWNLOAD_Click);
            // 
            // buttonSERVERCONNECTION
            // 
            this.buttonSERVERCONNECTION.Location = new System.Drawing.Point(155, 118);
            this.buttonSERVERCONNECTION.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSERVERCONNECTION.Name = "buttonSERVERCONNECTION";
            this.buttonSERVERCONNECTION.Size = new System.Drawing.Size(109, 25);
            this.buttonSERVERCONNECTION.TabIndex = 4;
            this.buttonSERVERCONNECTION.Text = "CONNECT";
            this.buttonSERVERCONNECTION.UseVisualStyleBackColor = true;
            this.buttonSERVERCONNECTION.Click += new System.EventHandler(this.buttonSERVERCONNECTION_Click);
            // 
            // textBoxSERVERPORT
            // 
            this.textBoxSERVERPORT.Location = new System.Drawing.Point(155, 65);
            this.textBoxSERVERPORT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxSERVERPORT.Name = "textBoxSERVERPORT";
            this.textBoxSERVERPORT.Size = new System.Drawing.Size(100, 22);
            this.textBoxSERVERPORT.TabIndex = 3;
            this.textBoxSERVERPORT.TextChanged += new System.EventHandler(this.textBoxSERVERPORT_TextChanged);
            // 
            // textBoxSERVERIP
            // 
            this.textBoxSERVERIP.Location = new System.Drawing.Point(155, 26);
            this.textBoxSERVERIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxSERVERIP.Name = "textBoxSERVERIP";
            this.textBoxSERVERIP.Size = new System.Drawing.Size(151, 22);
            this.textBoxSERVERIP.TabIndex = 2;
            this.textBoxSERVERIP.TextChanged += new System.EventHandler(this.textBoxSERVERIP_TextChanged);
            // 
            // labelSERVERPORT
            // 
            this.labelSERVERPORT.AutoSize = true;
            this.labelSERVERPORT.Location = new System.Drawing.Point(88, 65);
            this.labelSERVERPORT.Name = "labelSERVERPORT";
            this.labelSERVERPORT.Size = new System.Drawing.Size(48, 16);
            this.labelSERVERPORT.TabIndex = 1;
            this.labelSERVERPORT.Text = "PORT:";
            // 
            // labelSERVERIP
            // 
            this.labelSERVERIP.AutoSize = true;
            this.labelSERVERIP.Location = new System.Drawing.Point(112, 26);
            this.labelSERVERIP.Name = "labelSERVERIP";
            this.labelSERVERIP.Size = new System.Drawing.Size(22, 16);
            this.labelSERVERIP.TabIndex = 0;
            this.labelSERVERIP.Text = "IP:";
            // 
            // labelBUNDLEID
            // 
            this.labelBUNDLEID.AutoSize = true;
            this.labelBUNDLEID.Location = new System.Drawing.Point(849, 60);
            this.labelBUNDLEID.Name = "labelBUNDLEID";
            this.labelBUNDLEID.Size = new System.Drawing.Size(81, 16);
            this.labelBUNDLEID.TabIndex = 10;
            this.labelBUNDLEID.Text = "BUNDLE ID:";
            this.labelBUNDLEID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxFILENAMES
            // 
            this.listBoxFILENAMES.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxFILENAMES.FormattingEnabled = true;
            this.listBoxFILENAMES.HorizontalScrollbar = true;
            this.listBoxFILENAMES.ItemHeight = 16;
            this.listBoxFILENAMES.Location = new System.Drawing.Point(846, 77);
            this.listBoxFILENAMES.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxFILENAMES.Name = "listBoxFILENAMES";
            this.listBoxFILENAMES.Size = new System.Drawing.Size(313, 180);
            this.listBoxFILENAMES.TabIndex = 6;
            this.listBoxFILENAMES.SelectedIndexChanged += new System.EventHandler(this.listBoxFILENAMES_SelectedIndexChanged);
            // 
            // labelFILENAME
            // 
            this.labelFILENAME.AutoSize = true;
            this.labelFILENAME.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFILENAME.Location = new System.Drawing.Point(964, 38);
            this.labelFILENAME.Name = "labelFILENAME";
            this.labelFILENAME.Size = new System.Drawing.Size(74, 20);
            this.labelFILENAME.TabIndex = 5;
            this.labelFILENAME.Text = "FILES";
            // 
            // labelCURRENTIP
            // 
            this.labelCURRENTIP.AutoSize = true;
            this.labelCURRENTIP.Location = new System.Drawing.Point(88, 42);
            this.labelCURRENTIP.Name = "labelCURRENTIP";
            this.labelCURRENTIP.Size = new System.Drawing.Size(92, 16);
            this.labelCURRENTIP.TabIndex = 3;
            this.labelCURRENTIP.Text = "CURRENT IP:";
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
            this.groupBoxDEVICES.Controls.Add(this.dataGridView1);
            this.groupBoxDEVICES.Location = new System.Drawing.Point(12, 283);
            this.groupBoxDEVICES.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxDEVICES.Name = "groupBoxDEVICES";
            this.groupBoxDEVICES.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxDEVICES.Size = new System.Drawing.Size(1124, 204);
            this.groupBoxDEVICES.TabIndex = 5;
            this.groupBoxDEVICES.TabStop = false;
            this.groupBoxDEVICES.Text = "DEVICES";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView1.Location = new System.Drawing.Point(7, 18);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.MaximumSize = new System.Drawing.Size(1100, 180);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Aquamarine;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1100, 180);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // labelBUNDLE
            // 
            this.labelBUNDLE.AutoSize = true;
            this.labelBUNDLE.Location = new System.Drawing.Point(947, 59);
            this.labelBUNDLE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBUNDLE.Name = "labelBUNDLE";
            this.labelBUNDLE.Size = new System.Drawing.Size(91, 16);
            this.labelBUNDLE.TabIndex = 11;
            this.labelBUNDLE.Text = "com.exam.ple";
            this.labelBUNDLE.Click += new System.EventHandler(this.labelBUNDLE_Click);
            // 
            // buttonWEBSOCKETCONNECTION
            // 
            this.buttonWEBSOCKETCONNECTION.Location = new System.Drawing.Point(236, 492);
            this.buttonWEBSOCKETCONNECTION.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWEBSOCKETCONNECTION.Name = "buttonWEBSOCKETCONNECTION";
            this.buttonWEBSOCKETCONNECTION.Size = new System.Drawing.Size(701, 23);
            this.buttonWEBSOCKETCONNECTION.TabIndex = 12;
            this.buttonWEBSOCKETCONNECTION.Text = "STOP WEB SOCKET";
            this.buttonWEBSOCKETCONNECTION.UseVisualStyleBackColor = true;
            this.buttonWEBSOCKETCONNECTION.Click += new System.EventHandler(this.buttonWEBSOCKETCONNECTION_Click);
            // 
            // 
            // WINDOW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 597);
            this.Controls.Add(this.buttonWEBSOCKETCONNECTION);
            this.Controls.Add(this.labelBUNDLE);
            this.Controls.Add(this.labelBUNDLEID);
            this.Controls.Add(this.groupBoxDEVICES);
            this.Controls.Add(this.labelIPDEVICE);
            this.Controls.Add(this.listBoxFILENAMES);
            this.Controls.Add(this.labelFILENAME);
            this.Controls.Add(this.labelCURRENTIP);
            this.Controls.Add(this.groupBoxSERVER);
            this.Controls.Add(this.groupIPConnect);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "WINDOW";
            this.Text = "EASY CONNECTION";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupIPConnect.ResumeLayout(false);
            this.groupIPConnect.PerformLayout();
            this.groupBoxSERVER.ResumeLayout(false);
            this.groupBoxSERVER.PerformLayout();
            this.groupBoxDEVICES.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceReportBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adbServiceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adbServiceBindingSource1)).EndInit();
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
        private System.Windows.Forms.Button buttonINSTALL;
        private System.Windows.Forms.Button buttonMOVE;
        private System.Windows.Forms.Label labelBUNDLEID;
        private System.Windows.Forms.Label labelBUNDLE;
        private System.Windows.Forms.CheckBox checkBoxNEWDEVICE;
        private System.Windows.Forms.TextBox textBoxNEWDEVICE;
        private System.Windows.Forms.Button buttonAUTOSCANN;
        private System.Windows.Forms.Button buttonWEBSOCKETCONNECTION;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource adbServiceBindingSource;
        private System.Windows.Forms.BindingSource adbServiceBindingSource1;
        private System.Windows.Forms.BindingSource deviceReportBindingSource;
    }
}

