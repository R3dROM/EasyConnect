namespace EasyConnect
{
    partial class NetworkingConfiguration
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridView1 = new DataGridView();
            textBoxEXPMANAGERIP = new TextBox();
            labelEXPERIENCEMANAGER = new Label();
            buttonGENERATE = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(152, 52);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(463, 342);
            dataGridView1.TabIndex = 0;
            // 
            // textBoxEXPMANAGERIP
            // 
            textBoxEXPMANAGERIP.Location = new Point(339, 15);
            textBoxEXPMANAGERIP.Name = "textBoxEXPMANAGERIP";
            textBoxEXPMANAGERIP.Size = new Size(125, 27);
            textBoxEXPMANAGERIP.TabIndex = 1;
            textBoxEXPMANAGERIP.TextChanged += textBoxEXPMANAGERIP_TextChanged;
            // 
            // labelEXPERIENCEMANAGER
            // 
            labelEXPERIENCEMANAGER.AutoSize = true;
            labelEXPERIENCEMANAGER.Location = new Point(152, 22);
            labelEXPERIENCEMANAGER.Name = "labelEXPERIENCEMANAGER";
            labelEXPERIENCEMANAGER.Size = new Size(167, 20);
            labelEXPERIENCEMANAGER.TabIndex = 2;
            labelEXPERIENCEMANAGER.Text = "Experience Manager IP :";
            // 
            // buttonGENERATE
            // 
            buttonGENERATE.Location = new Point(339, 400);
            buttonGENERATE.Name = "buttonGENERATE";
            buttonGENERATE.Size = new Size(94, 29);
            buttonGENERATE.TabIndex = 3;
            buttonGENERATE.Text = "GENERATE";
            buttonGENERATE.UseVisualStyleBackColor = true;
            buttonGENERATE.Click += buttonGENERATE_Click;
            // 
            // NetworkingConfiguration
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonGENERATE);
            Controls.Add(labelEXPERIENCEMANAGER);
            Controls.Add(textBoxEXPMANAGERIP);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "NetworkingConfiguration";
            Text = "NetworkingConfiguration";
            Load += NetworkingConfiguration_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox textBoxEXPMANAGERIP;
        private Label labelEXPERIENCEMANAGER;
        private Button buttonGENERATE;
    }
}