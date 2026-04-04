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
            Title = new Label();
            groupIPConnect = new GroupBox();
            buttonAUTOSCANN = new Button();
            textBoxNEWDEVICE = new TextBox();
            checkBoxNEWDEVICE = new CheckBox();
            buttonCONNECT = new Button();
            textBoxPORT = new TextBox();
            textBoxIP = new TextBox();
            labelPort = new Label();
            labelIP = new Label();
            groupBoxSERVER = new GroupBox();
            buttonINSTALL = new Button();
            buttonMOVE = new Button();
            buttonDOWNLOAD = new Button();
            buttonSERVERCONNECTION = new Button();
            textBoxSERVERPORT = new TextBox();
            textBoxSERVERIP = new TextBox();
            labelSERVERPORT = new Label();
            labelSERVERIP = new Label();
            labelBUNDLEID = new Label();
            listBoxFILENAMES = new ListBox();
            labelFILENAME = new Label();
            labelCURRENTIP = new Label();
            labelIPDEVICE = new Label();
            groupBoxDEVICES = new GroupBox();
            dataGridView1 = new DataGridView();
            labelBUNDLE = new Label();
            buttonWEBSOCKETCONNECTION = new Button();
            deviceReportBindingSource = new BindingSource(components);
            adbServiceBindingSource = new BindingSource(components);
            adbServiceBindingSource1 = new BindingSource(components);
            buttonRESETADB = new Button();
            buttonNETWORKING = new Button();
            groupIPConnect.SuspendLayout();
            groupBoxSERVER.SuspendLayout();
            groupBoxDEVICES.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)deviceReportBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource1).BeginInit();
            SuspendLayout();
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Lucida Console", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Title.Location = new Point(585, 11);
            Title.Margin = new Padding(4, 0, 4, 0);
            Title.Name = "Title";
            Title.Size = new Size(235, 24);
            Title.TabIndex = 0;
            Title.Text = "EASY CONNECTION";
            // 
            // groupIPConnect
            // 
            groupIPConnect.Controls.Add(buttonAUTOSCANN);
            groupIPConnect.Controls.Add(textBoxNEWDEVICE);
            groupIPConnect.Controls.Add(checkBoxNEWDEVICE);
            groupIPConnect.Controls.Add(buttonCONNECT);
            groupIPConnect.Controls.Add(textBoxPORT);
            groupIPConnect.Controls.Add(textBoxIP);
            groupIPConnect.Controls.Add(labelPort);
            groupIPConnect.Controls.Add(labelIP);
            groupIPConnect.Location = new Point(35, 100);
            groupIPConnect.Margin = new Padding(4, 2, 4, 2);
            groupIPConnect.Name = "groupIPConnect";
            groupIPConnect.Padding = new Padding(4, 2, 4, 2);
            groupIPConnect.Size = new Size(499, 305);
            groupIPConnect.TabIndex = 1;
            groupIPConnect.TabStop = false;
            groupIPConnect.Text = "IP Connection";
            // 
            // buttonAUTOSCANN
            // 
            buttonAUTOSCANN.Location = new Point(306, 239);
            buttonAUTOSCANN.Margin = new Padding(4, 2, 4, 2);
            buttonAUTOSCANN.Name = "buttonAUTOSCANN";
            buttonAUTOSCANN.Size = new Size(171, 36);
            buttonAUTOSCANN.TabIndex = 12;
            buttonAUTOSCANN.Text = "AUTO SCANN";
            buttonAUTOSCANN.UseVisualStyleBackColor = true;
            buttonAUTOSCANN.Click += buttonAUTOSCANN_Click;
            // 
            // textBoxNEWDEVICE
            // 
            textBoxNEWDEVICE.Location = new Point(200, 165);
            textBoxNEWDEVICE.Margin = new Padding(5, 6, 5, 6);
            textBoxNEWDEVICE.Name = "textBoxNEWDEVICE";
            textBoxNEWDEVICE.Size = new Size(164, 31);
            textBoxNEWDEVICE.TabIndex = 7;
            textBoxNEWDEVICE.Visible = false;
            textBoxNEWDEVICE.TextChanged += textBoxNEWDEVICE_TextChanged;
            // 
            // checkBoxNEWDEVICE
            // 
            checkBoxNEWDEVICE.AutoSize = true;
            checkBoxNEWDEVICE.Location = new Point(34, 172);
            checkBoxNEWDEVICE.Margin = new Padding(5, 6, 5, 6);
            checkBoxNEWDEVICE.Name = "checkBoxNEWDEVICE";
            checkBoxNEWDEVICE.Size = new Size(140, 29);
            checkBoxNEWDEVICE.TabIndex = 6;
            checkBoxNEWDEVICE.Text = "NEW DEVICE";
            checkBoxNEWDEVICE.UseVisualStyleBackColor = true;
            checkBoxNEWDEVICE.CheckedChanged += checkBoxNEWDEVICE_CheckedChanged;
            // 
            // buttonCONNECT
            // 
            buttonCONNECT.Location = new Point(106, 236);
            buttonCONNECT.Margin = new Padding(4, 2, 4, 2);
            buttonCONNECT.Name = "buttonCONNECT";
            buttonCONNECT.Size = new Size(136, 39);
            buttonCONNECT.TabIndex = 4;
            buttonCONNECT.Text = "CONNECT";
            buttonCONNECT.UseVisualStyleBackColor = true;
            buttonCONNECT.Click += buttonCONNECT_Click;
            // 
            // textBoxPORT
            // 
            textBoxPORT.Location = new Point(200, 99);
            textBoxPORT.Margin = new Padding(4, 2, 4, 2);
            textBoxPORT.Name = "textBoxPORT";
            textBoxPORT.Size = new Size(124, 31);
            textBoxPORT.TabIndex = 3;
            textBoxPORT.Text = "5555";
            textBoxPORT.TextAlign = HorizontalAlignment.Center;
            textBoxPORT.TextChanged += textBoxPORT_TextChanged;
            // 
            // textBoxIP
            // 
            textBoxIP.Location = new Point(200, 40);
            textBoxIP.Margin = new Padding(4, 2, 4, 2);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.Size = new Size(188, 31);
            textBoxIP.TabIndex = 2;
            textBoxIP.TextChanged += textBoxIP_TextChanged;
            // 
            // labelPort
            // 
            labelPort.AutoSize = true;
            labelPort.Location = new Point(114, 108);
            labelPort.Margin = new Padding(4, 0, 4, 0);
            labelPort.Name = "labelPort";
            labelPort.Size = new Size(59, 25);
            labelPort.TabIndex = 1;
            labelPort.Text = "PORT:";
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new Point(146, 50);
            labelIP.Margin = new Padding(4, 0, 4, 0);
            labelIP.Name = "labelIP";
            labelIP.Size = new Size(31, 25);
            labelIP.TabIndex = 0;
            labelIP.Text = "IP:";
            // 
            // groupBoxSERVER
            // 
            groupBoxSERVER.Controls.Add(buttonINSTALL);
            groupBoxSERVER.Controls.Add(buttonMOVE);
            groupBoxSERVER.Controls.Add(buttonDOWNLOAD);
            groupBoxSERVER.Controls.Add(buttonSERVERCONNECTION);
            groupBoxSERVER.Controls.Add(textBoxSERVERPORT);
            groupBoxSERVER.Controls.Add(textBoxSERVERIP);
            groupBoxSERVER.Controls.Add(labelSERVERPORT);
            groupBoxSERVER.Controls.Add(labelSERVERIP);
            groupBoxSERVER.Location = new Point(555, 100);
            groupBoxSERVER.Margin = new Padding(4, 2, 4, 2);
            groupBoxSERVER.Name = "groupBoxSERVER";
            groupBoxSERVER.Padding = new Padding(4, 2, 4, 2);
            groupBoxSERVER.Size = new Size(495, 305);
            groupBoxSERVER.TabIndex = 2;
            groupBoxSERVER.TabStop = false;
            groupBoxSERVER.Text = "SERVER Connection";
            // 
            // buttonINSTALL
            // 
            buttonINSTALL.Location = new Point(334, 238);
            buttonINSTALL.Margin = new Padding(4, 2, 4, 2);
            buttonINSTALL.Name = "buttonINSTALL";
            buttonINSTALL.Size = new Size(114, 36);
            buttonINSTALL.TabIndex = 9;
            buttonINSTALL.Text = "INSTALL";
            buttonINSTALL.UseVisualStyleBackColor = true;
            buttonINSTALL.Click += buttonINSTALL_Click;
            // 
            // buttonMOVE
            // 
            buttonMOVE.Location = new Point(214, 238);
            buttonMOVE.Margin = new Padding(4, 2, 4, 2);
            buttonMOVE.Name = "buttonMOVE";
            buttonMOVE.Size = new Size(94, 36);
            buttonMOVE.TabIndex = 8;
            buttonMOVE.Text = "MOVE";
            buttonMOVE.UseVisualStyleBackColor = true;
            buttonMOVE.Click += buttonMOVE_Click;
            // 
            // buttonDOWNLOAD
            // 
            buttonDOWNLOAD.Location = new Point(54, 238);
            buttonDOWNLOAD.Margin = new Padding(4, 2, 4, 2);
            buttonDOWNLOAD.Name = "buttonDOWNLOAD";
            buttonDOWNLOAD.Size = new Size(136, 36);
            buttonDOWNLOAD.TabIndex = 7;
            buttonDOWNLOAD.Text = "DOWNLOAD";
            buttonDOWNLOAD.UseVisualStyleBackColor = true;
            buttonDOWNLOAD.Click += buttonDOWNLOAD_Click;
            // 
            // buttonSERVERCONNECTION
            // 
            buttonSERVERCONNECTION.Location = new Point(194, 185);
            buttonSERVERCONNECTION.Margin = new Padding(4, 2, 4, 2);
            buttonSERVERCONNECTION.Name = "buttonSERVERCONNECTION";
            buttonSERVERCONNECTION.Size = new Size(136, 39);
            buttonSERVERCONNECTION.TabIndex = 4;
            buttonSERVERCONNECTION.Text = "CONNECT";
            buttonSERVERCONNECTION.UseVisualStyleBackColor = true;
            buttonSERVERCONNECTION.Click += buttonSERVERCONNECTION_Click;
            // 
            // textBoxSERVERPORT
            // 
            textBoxSERVERPORT.Location = new Point(194, 101);
            textBoxSERVERPORT.Margin = new Padding(4, 2, 4, 2);
            textBoxSERVERPORT.Name = "textBoxSERVERPORT";
            textBoxSERVERPORT.Size = new Size(124, 31);
            textBoxSERVERPORT.TabIndex = 3;
            textBoxSERVERPORT.TextChanged += textBoxSERVERPORT_TextChanged;
            // 
            // textBoxSERVERIP
            // 
            textBoxSERVERIP.Location = new Point(194, 40);
            textBoxSERVERIP.Margin = new Padding(4, 2, 4, 2);
            textBoxSERVERIP.Name = "textBoxSERVERIP";
            textBoxSERVERIP.Size = new Size(188, 31);
            textBoxSERVERIP.TabIndex = 2;
            textBoxSERVERIP.TextChanged += textBoxSERVERIP_TextChanged;
            // 
            // labelSERVERPORT
            // 
            labelSERVERPORT.AutoSize = true;
            labelSERVERPORT.Location = new Point(110, 101);
            labelSERVERPORT.Margin = new Padding(4, 0, 4, 0);
            labelSERVERPORT.Name = "labelSERVERPORT";
            labelSERVERPORT.Size = new Size(59, 25);
            labelSERVERPORT.TabIndex = 1;
            labelSERVERPORT.Text = "PORT:";
            // 
            // labelSERVERIP
            // 
            labelSERVERIP.AutoSize = true;
            labelSERVERIP.Location = new Point(140, 40);
            labelSERVERIP.Margin = new Padding(4, 0, 4, 0);
            labelSERVERIP.Name = "labelSERVERIP";
            labelSERVERIP.Size = new Size(31, 25);
            labelSERVERIP.TabIndex = 0;
            labelSERVERIP.Text = "IP:";
            // 
            // labelBUNDLEID
            // 
            labelBUNDLEID.AutoSize = true;
            labelBUNDLEID.Location = new Point(1058, 84);
            labelBUNDLEID.Margin = new Padding(4, 0, 4, 0);
            labelBUNDLEID.Name = "labelBUNDLEID";
            labelBUNDLEID.Size = new Size(104, 25);
            labelBUNDLEID.TabIndex = 10;
            labelBUNDLEID.Text = "BUNDLE ID:";
            labelBUNDLEID.TextAlign = ContentAlignment.BottomLeft;
            // 
            // listBoxFILENAMES
            // 
            listBoxFILENAMES.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxFILENAMES.FormattingEnabled = true;
            listBoxFILENAMES.HorizontalScrollbar = true;
            listBoxFILENAMES.Location = new Point(1058, 112);
            listBoxFILENAMES.Margin = new Padding(4, 2, 4, 2);
            listBoxFILENAMES.Name = "listBoxFILENAMES";
            listBoxFILENAMES.Size = new Size(390, 284);
            listBoxFILENAMES.TabIndex = 6;
            listBoxFILENAMES.SelectedIndexChanged += listBoxFILENAMES_SelectedIndexChanged;
            // 
            // labelFILENAME
            // 
            labelFILENAME.AutoSize = true;
            labelFILENAME.Font = new Font("Lucida Console", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFILENAME.Location = new Point(1201, 50);
            labelFILENAME.Margin = new Padding(4, 0, 4, 0);
            labelFILENAME.Name = "labelFILENAME";
            labelFILENAME.Size = new Size(85, 24);
            labelFILENAME.TabIndex = 5;
            labelFILENAME.Text = "FILES";
            // 
            // labelCURRENTIP
            // 
            labelCURRENTIP.AutoSize = true;
            labelCURRENTIP.Location = new Point(110, 65);
            labelCURRENTIP.Margin = new Padding(4, 0, 4, 0);
            labelCURRENTIP.Name = "labelCURRENTIP";
            labelCURRENTIP.Size = new Size(112, 25);
            labelCURRENTIP.TabIndex = 3;
            labelCURRENTIP.Text = "CURRENT IP:";
            // 
            // labelIPDEVICE
            // 
            labelIPDEVICE.AutoSize = true;
            labelIPDEVICE.Location = new Point(234, 65);
            labelIPDEVICE.Margin = new Padding(4, 0, 4, 0);
            labelIPDEVICE.Name = "labelIPDEVICE";
            labelIPDEVICE.Size = new Size(39, 25);
            labelIPDEVICE.TabIndex = 4;
            labelIPDEVICE.Text = "aaa";
            labelIPDEVICE.Click += labelIPDEVICE_Click;
            // 
            // groupBoxDEVICES
            // 
            groupBoxDEVICES.Controls.Add(dataGridView1);
            groupBoxDEVICES.Location = new Point(15, 442);
            groupBoxDEVICES.Margin = new Padding(4, 2, 4, 2);
            groupBoxDEVICES.Name = "groupBoxDEVICES";
            groupBoxDEVICES.Padding = new Padding(4, 2, 4, 2);
            groupBoxDEVICES.Size = new Size(1405, 319);
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
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.Desktop;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Menu;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.Desktop;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.GridColor = SystemColors.ActiveCaptionText;
            dataGridView1.Location = new Point(9, 28);
            dataGridView1.Margin = new Padding(5, 6, 5, 6);
            dataGridView1.MaximumSize = new Size(1375, 281);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
            dataGridView1.Size = new Size(1375, 281);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // labelBUNDLE
            // 
            labelBUNDLE.AutoSize = true;
            labelBUNDLE.Location = new Point(1180, 82);
            labelBUNDLE.Margin = new Padding(5, 0, 5, 0);
            labelBUNDLE.Name = "labelBUNDLE";
            labelBUNDLE.Size = new Size(121, 25);
            labelBUNDLE.TabIndex = 11;
            labelBUNDLE.Text = "com.exam.ple";
            labelBUNDLE.Click += labelBUNDLE_Click;
            // 
            // buttonWEBSOCKETCONNECTION
            // 
            buttonWEBSOCKETCONNECTION.Location = new Point(295, 769);
            buttonWEBSOCKETCONNECTION.Margin = new Padding(4, 2, 4, 2);
            buttonWEBSOCKETCONNECTION.Name = "buttonWEBSOCKETCONNECTION";
            buttonWEBSOCKETCONNECTION.Size = new Size(876, 36);
            buttonWEBSOCKETCONNECTION.TabIndex = 12;
            buttonWEBSOCKETCONNECTION.Text = "STOP WEB SOCKET";
            buttonWEBSOCKETCONNECTION.UseVisualStyleBackColor = true;
            buttonWEBSOCKETCONNECTION.Click += buttonWEBSOCKETCONNECTION_Click;
            // 
            // buttonRESETADB
            // 
            buttonRESETADB.Location = new Point(295, 811);
            buttonRESETADB.Margin = new Padding(4);
            buttonRESETADB.Name = "buttonRESETADB";
            buttonRESETADB.Size = new Size(876, 36);
            buttonRESETADB.TabIndex = 13;
            buttonRESETADB.Text = "RESTART ADB";
            buttonRESETADB.UseVisualStyleBackColor = true;
            buttonRESETADB.Click += buttonRESETADB_Click;
            // 
            // buttonNETWORKING
            // 
            buttonNETWORKING.Location = new Point(295, 855);
            buttonNETWORKING.Margin = new Padding(4);
            buttonNETWORKING.Name = "buttonNETWORKING";
            buttonNETWORKING.Size = new Size(876, 36);
            buttonNETWORKING.TabIndex = 14;
            buttonNETWORKING.Text = "NETWORKING CONFIGURATION";
            buttonNETWORKING.UseVisualStyleBackColor = true;
            buttonNETWORKING.Click += buttonNETWORKING_Click;
            // 
            // WINDOW
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1464, 932);
            Controls.Add(buttonNETWORKING);
            Controls.Add(buttonRESETADB);
            Controls.Add(buttonWEBSOCKETCONNECTION);
            Controls.Add(labelBUNDLE);
            Controls.Add(labelBUNDLEID);
            Controls.Add(groupBoxDEVICES);
            Controls.Add(labelIPDEVICE);
            Controls.Add(listBoxFILENAMES);
            Controls.Add(labelFILENAME);
            Controls.Add(labelCURRENTIP);
            Controls.Add(groupBoxSERVER);
            Controls.Add(groupIPConnect);
            Controls.Add(Title);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 2, 4, 2);
            Name = "WINDOW";
            Text = "EASY LINK";
            Load += Form1_Load;
            groupIPConnect.ResumeLayout(false);
            groupIPConnect.PerformLayout();
            groupBoxSERVER.ResumeLayout(false);
            groupBoxSERVER.PerformLayout();
            groupBoxDEVICES.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)deviceReportBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)adbServiceBindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private Button buttonRESETADB;
        private Button buttonNETWORKING;
    }
}

