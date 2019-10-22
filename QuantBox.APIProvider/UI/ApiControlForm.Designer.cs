namespace QuantBox.APIProvider.UI
{
#if NET48
    partial class ApiControlForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_PortfolioID3 = new System.Windows.Forms.TextBox();
            this.textBox_PortfolioID2 = new System.Windows.Forms.TextBox();
            this.textBox_PortfolioID1 = new System.Windows.Forms.TextBox();
            this.comboBox_BusinessType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_QueryOrder = new System.Windows.Forms.Button();
            this.button_QueryAccount = new System.Windows.Forms.Button();
            this.button_QueryTrade = new System.Windows.Forms.Button();
            this.button_QueryPosition = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_PortfolioID3);
            this.groupBox1.Controls.Add(this.textBox_PortfolioID2);
            this.groupBox1.Controls.Add(this.textBox_PortfolioID1);
            this.groupBox1.Controls.Add(this.comboBox_BusinessType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 274);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Query";
            // 
            // textBox_PortfolioID3
            // 
            this.textBox_PortfolioID3.Location = new System.Drawing.Point(123, 85);
            this.textBox_PortfolioID3.Name = "textBox_PortfolioID3";
            this.textBox_PortfolioID3.Size = new System.Drawing.Size(100, 20);
            this.textBox_PortfolioID3.TabIndex = 2;
            this.textBox_PortfolioID3.Text = "8888";
            // 
            // textBox_PortfolioID2
            // 
            this.textBox_PortfolioID2.Location = new System.Drawing.Point(123, 55);
            this.textBox_PortfolioID2.Name = "textBox_PortfolioID2";
            this.textBox_PortfolioID2.Size = new System.Drawing.Size(100, 20);
            this.textBox_PortfolioID2.TabIndex = 2;
            this.textBox_PortfolioID2.Text = "888800";
            // 
            // textBox_PortfolioID1
            // 
            this.textBox_PortfolioID1.Location = new System.Drawing.Point(123, 26);
            this.textBox_PortfolioID1.Name = "textBox_PortfolioID1";
            this.textBox_PortfolioID1.Size = new System.Drawing.Size(100, 20);
            this.textBox_PortfolioID1.TabIndex = 2;
            this.textBox_PortfolioID1.Text = "8888";
            // 
            // comboBox_BusinessType
            // 
            this.comboBox_BusinessType.FormattingEnabled = true;
            this.comboBox_BusinessType.Location = new System.Drawing.Point(121, 116);
            this.comboBox_BusinessType.Name = "comboBox_BusinessType";
            this.comboBox_BusinessType.Size = new System.Drawing.Size(121, 21);
            this.comboBox_BusinessType.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "BusinessType:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "PortfolioID3:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "PortfolioID2:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PortfolioID1:";
            // 
            // button_QueryOrder
            // 
            this.button_QueryOrder.Location = new System.Drawing.Point(293, 22);
            this.button_QueryOrder.Name = "button_QueryOrder";
            this.button_QueryOrder.Size = new System.Drawing.Size(101, 23);
            this.button_QueryOrder.TabIndex = 1;
            this.button_QueryOrder.Text = "QueryOrder";
            this.button_QueryOrder.UseVisualStyleBackColor = true;
            this.button_QueryOrder.Click += new System.EventHandler(this.button_QueryOrder_Click);
            // 
            // button_QueryAccount
            // 
            this.button_QueryAccount.Location = new System.Drawing.Point(293, 95);
            this.button_QueryAccount.Name = "button_QueryAccount";
            this.button_QueryAccount.Size = new System.Drawing.Size(101, 23);
            this.button_QueryAccount.TabIndex = 2;
            this.button_QueryAccount.Text = "QueryAccount";
            this.button_QueryAccount.UseVisualStyleBackColor = true;
            this.button_QueryAccount.Click += new System.EventHandler(this.button_QueryAccount_Click);
            // 
            // button_QueryTrade
            // 
            this.button_QueryTrade.Location = new System.Drawing.Point(293, 59);
            this.button_QueryTrade.Name = "button_QueryTrade";
            this.button_QueryTrade.Size = new System.Drawing.Size(101, 23);
            this.button_QueryTrade.TabIndex = 3;
            this.button_QueryTrade.Text = "QueryTrade";
            this.button_QueryTrade.UseVisualStyleBackColor = true;
            this.button_QueryTrade.Click += new System.EventHandler(this.button_QueryTrade_Click);
            // 
            // button_QueryPosition
            // 
            this.button_QueryPosition.Location = new System.Drawing.Point(293, 138);
            this.button_QueryPosition.Name = "button_QueryPosition";
            this.button_QueryPosition.Size = new System.Drawing.Size(101, 23);
            this.button_QueryPosition.TabIndex = 4;
            this.button_QueryPosition.Text = "QueryPosition";
            this.button_QueryPosition.UseVisualStyleBackColor = true;
            this.button_QueryPosition.Click += new System.EventHandler(this.button_QueryPosition_Click);
            // 
            // ApiControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 298);
            this.Controls.Add(this.button_QueryPosition);
            this.Controls.Add(this.button_QueryTrade);
            this.Controls.Add(this.button_QueryAccount);
            this.Controls.Add(this.button_QueryOrder);
            this.Controls.Add(this.groupBox1);
            this.Name = "ApiControlForm";
            this.Text = "ApiControlForm";
            this.Load += new System.EventHandler(this.ApiControlForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

    #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_QueryOrder;
        private System.Windows.Forms.Button button_QueryAccount;
        private System.Windows.Forms.Button button_QueryTrade;
        private System.Windows.Forms.Button button_QueryPosition;
        private System.Windows.Forms.ComboBox comboBox_BusinessType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_PortfolioID3;
        private System.Windows.Forms.TextBox textBox_PortfolioID2;
        private System.Windows.Forms.TextBox textBox_PortfolioID1;
    }
#endif
}