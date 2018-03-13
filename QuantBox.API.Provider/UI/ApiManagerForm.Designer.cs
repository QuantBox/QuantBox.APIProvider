namespace QuantBox.APIProvider.UI
{
    partial class ApiManagerForm
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
            this.components = new System.ComponentModel.Container();
            this.listBox_UserList = new System.Windows.Forms.ListBox();
            this.userItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_CopyUser = new System.Windows.Forms.Button();
            this.button_RemoveUser = new System.Windows.Forms.Button();
            this.button_AddUser = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_CopyServer = new System.Windows.Forms.Button();
            this.button_RemoveServer = new System.Windows.Forms.Button();
            this.listBox_ServerList = new System.Windows.Forms.ListBox();
            this.serverItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button_AddServer = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_CopyApi = new System.Windows.Forms.Button();
            this.button_RemoveApi = new System.Windows.Forms.Button();
            this.listBox_ApiList = new System.Windows.Forms.ListBox();
            this.apiItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button_AddApi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.userItemBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverItemBindingSource)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apiItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox_UserList
            // 
            this.listBox_UserList.DataSource = this.userItemBindingSource;
            this.listBox_UserList.FormattingEnabled = true;
            this.listBox_UserList.HorizontalScrollbar = true;
            this.listBox_UserList.ItemHeight = 15;
            this.listBox_UserList.Location = new System.Drawing.Point(19, 22);
            this.listBox_UserList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox_UserList.Name = "listBox_UserList";
            this.listBox_UserList.Size = new System.Drawing.Size(416, 124);
            this.listBox_UserList.TabIndex = 0;
            this.listBox_UserList.SelectedIndexChanged += new System.EventHandler(this.listBox_UserList_SelectedIndexChanged);
            // 
            // userItemBindingSource
            // 
            this.userItemBindingSource.DataSource = typeof(QuantBox.APIProvider.Single.UserItem);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(585, -1);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(580, 540);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_CopyUser);
            this.groupBox1.Controls.Add(this.button_RemoveUser);
            this.groupBox1.Controls.Add(this.button_AddUser);
            this.groupBox1.Controls.Add(this.listBox_UserList);
            this.groupBox1.Location = new System.Drawing.Point(16, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(561, 164);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UserList";
            // 
            // button_CopyUser
            // 
            this.button_CopyUser.Location = new System.Drawing.Point(444, 89);
            this.button_CopyUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CopyUser.Name = "button_CopyUser";
            this.button_CopyUser.Size = new System.Drawing.Size(100, 27);
            this.button_CopyUser.TabIndex = 1;
            this.button_CopyUser.Text = "Copy";
            this.button_CopyUser.UseVisualStyleBackColor = true;
            this.button_CopyUser.Click += new System.EventHandler(this.button_CopyUser_Click);
            // 
            // button_RemoveUser
            // 
            this.button_RemoveUser.Location = new System.Drawing.Point(444, 55);
            this.button_RemoveUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_RemoveUser.Name = "button_RemoveUser";
            this.button_RemoveUser.Size = new System.Drawing.Size(100, 27);
            this.button_RemoveUser.TabIndex = 1;
            this.button_RemoveUser.Text = "Remove";
            this.button_RemoveUser.UseVisualStyleBackColor = true;
            this.button_RemoveUser.Click += new System.EventHandler(this.button_RemoveUser_Click);
            // 
            // button_AddUser
            // 
            this.button_AddUser.Location = new System.Drawing.Point(444, 22);
            this.button_AddUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_AddUser.Name = "button_AddUser";
            this.button_AddUser.Size = new System.Drawing.Size(100, 27);
            this.button_AddUser.TabIndex = 1;
            this.button_AddUser.Text = "Add";
            this.button_AddUser.UseVisualStyleBackColor = true;
            this.button_AddUser.Click += new System.EventHandler(this.button_AddUser_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_CopyServer);
            this.groupBox2.Controls.Add(this.button_RemoveServer);
            this.groupBox2.Controls.Add(this.listBox_ServerList);
            this.groupBox2.Controls.Add(this.button_AddServer);
            this.groupBox2.Location = new System.Drawing.Point(16, 185);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(561, 164);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ServerList";
            // 
            // button_CopyServer
            // 
            this.button_CopyServer.Location = new System.Drawing.Point(444, 92);
            this.button_CopyServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CopyServer.Name = "button_CopyServer";
            this.button_CopyServer.Size = new System.Drawing.Size(100, 27);
            this.button_CopyServer.TabIndex = 1;
            this.button_CopyServer.Text = "Copy";
            this.button_CopyServer.UseVisualStyleBackColor = true;
            this.button_CopyServer.Click += new System.EventHandler(this.button_CopyServer_Click);
            // 
            // button_RemoveServer
            // 
            this.button_RemoveServer.Location = new System.Drawing.Point(444, 59);
            this.button_RemoveServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_RemoveServer.Name = "button_RemoveServer";
            this.button_RemoveServer.Size = new System.Drawing.Size(100, 27);
            this.button_RemoveServer.TabIndex = 1;
            this.button_RemoveServer.Text = "Remove";
            this.button_RemoveServer.UseVisualStyleBackColor = true;
            this.button_RemoveServer.Click += new System.EventHandler(this.button_RemoveServer_Click);
            // 
            // listBox_ServerList
            // 
            this.listBox_ServerList.DataSource = this.serverItemBindingSource;
            this.listBox_ServerList.FormattingEnabled = true;
            this.listBox_ServerList.HorizontalScrollbar = true;
            this.listBox_ServerList.ItemHeight = 15;
            this.listBox_ServerList.Location = new System.Drawing.Point(19, 22);
            this.listBox_ServerList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox_ServerList.Name = "listBox_ServerList";
            this.listBox_ServerList.Size = new System.Drawing.Size(416, 124);
            this.listBox_ServerList.TabIndex = 0;
            this.listBox_ServerList.SelectedIndexChanged += new System.EventHandler(this.listBox_ServerList_SelectedIndexChanged);
            // 
            // button_AddServer
            // 
            this.button_AddServer.Location = new System.Drawing.Point(444, 25);
            this.button_AddServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_AddServer.Name = "button_AddServer";
            this.button_AddServer.Size = new System.Drawing.Size(100, 27);
            this.button_AddServer.TabIndex = 1;
            this.button_AddServer.Text = "Add";
            this.button_AddServer.UseVisualStyleBackColor = true;
            this.button_AddServer.Click += new System.EventHandler(this.button_AddServer_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.button_CopyApi);
            this.groupBox3.Controls.Add(this.button_RemoveApi);
            this.groupBox3.Controls.Add(this.listBox_ApiList);
            this.groupBox3.Controls.Add(this.button_AddApi);
            this.groupBox3.Location = new System.Drawing.Point(16, 362);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(561, 164);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ApiList";
            // 
            // button_CopyApi
            // 
            this.button_CopyApi.Location = new System.Drawing.Point(444, 88);
            this.button_CopyApi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CopyApi.Name = "button_CopyApi";
            this.button_CopyApi.Size = new System.Drawing.Size(100, 27);
            this.button_CopyApi.TabIndex = 1;
            this.button_CopyApi.Text = "Copy";
            this.button_CopyApi.UseVisualStyleBackColor = true;
            this.button_CopyApi.Click += new System.EventHandler(this.button_CopyApi_Click);
            // 
            // button_RemoveApi
            // 
            this.button_RemoveApi.Location = new System.Drawing.Point(444, 54);
            this.button_RemoveApi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_RemoveApi.Name = "button_RemoveApi";
            this.button_RemoveApi.Size = new System.Drawing.Size(100, 27);
            this.button_RemoveApi.TabIndex = 1;
            this.button_RemoveApi.Text = "Remove";
            this.button_RemoveApi.UseVisualStyleBackColor = true;
            this.button_RemoveApi.Click += new System.EventHandler(this.button_RemoveApi_Click);
            // 
            // listBox_ApiList
            // 
            this.listBox_ApiList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_ApiList.DataSource = this.apiItemBindingSource;
            this.listBox_ApiList.FormattingEnabled = true;
            this.listBox_ApiList.HorizontalScrollbar = true;
            this.listBox_ApiList.ItemHeight = 15;
            this.listBox_ApiList.Location = new System.Drawing.Point(19, 22);
            this.listBox_ApiList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox_ApiList.Name = "listBox_ApiList";
            this.listBox_ApiList.Size = new System.Drawing.Size(416, 124);
            this.listBox_ApiList.TabIndex = 0;
            this.listBox_ApiList.SelectedIndexChanged += new System.EventHandler(this.listBox_ApiList_SelectedIndexChanged);
            // 
            // apiItemBindingSource
            // 
            this.apiItemBindingSource.DataSource = typeof(QuantBox.APIProvider.Single.ApiItem);
            // 
            // button_AddApi
            // 
            this.button_AddApi.Location = new System.Drawing.Point(444, 21);
            this.button_AddApi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_AddApi.Name = "button_AddApi";
            this.button_AddApi.Size = new System.Drawing.Size(100, 27);
            this.button_AddApi.TabIndex = 1;
            this.button_AddApi.Text = "Add";
            this.button_AddApi.UseVisualStyleBackColor = true;
            this.button_AddApi.Click += new System.EventHandler(this.button_AddApi_Click);
            // 
            // ApiManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 540);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.propertyGrid);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ApiManagerForm";
            this.Text = "ApiManagerForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ApiManagerForm_FormClosed);
            this.Load += new System.EventHandler(this.ApiManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.userItemBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serverItemBindingSource)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.apiItemBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_UserList;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox_ServerList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox_ApiList;
        private System.Windows.Forms.Button button_AddUser;
        private System.Windows.Forms.Button button_RemoveUser;
        private System.Windows.Forms.Button button_CopyUser;
        private System.Windows.Forms.Button button_CopyServer;
        private System.Windows.Forms.Button button_RemoveServer;
        private System.Windows.Forms.Button button_AddServer;
        private System.Windows.Forms.Button button_CopyApi;
        private System.Windows.Forms.Button button_RemoveApi;
        private System.Windows.Forms.Button button_AddApi;
        private System.Windows.Forms.BindingSource userItemBindingSource;
        private System.Windows.Forms.BindingSource serverItemBindingSource;
        private System.Windows.Forms.BindingSource apiItemBindingSource;
    }
}