using QuantBox.APIProvider.Single;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuantBox.APIProvider.UI
{
    public partial class ApiManagerForm : Form
    {
        public ApiManagerForm()
        {
            InitializeComponent();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //if (propertyGrid.SelectedObject is UserItem)
            //{
            //    if (listBox_UserList.SelectedIndex >= 0) {
            //        listBox_UserList.Items[listBox_UserList.SelectedIndex] = propertyGrid.SelectedObject;
            //    }
            //}
            //else if (propertyGrid.SelectedObject is ServerItem)
            //{
            //    if (listBox_ServerList.SelectedIndex >= 0)
            //    {
            //        listBox_ServerList.Items[listBox_ServerList.SelectedIndex] = propertyGrid.SelectedObject;
            //    }
            //}
            //else if (propertyGrid.SelectedObject is ApiItem)
            //{
            //    if (listBox_ApiList.SelectedIndex >= 0)
            //    {
            //        listBox_ApiList.Items[listBox_ApiList.SelectedIndex] = propertyGrid.SelectedObject;
            //    }
            //}
        }

        private void listBox_UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_UserList.SelectedItems.Count > 0)
            {
                UserItem item = (listBox_UserList.SelectedItems[0] as UserItem);

                propertyGrid.SelectedObject = item;
            }
        }

        private void button_AddUser_Click(object sender, EventArgs e)
        {
            provider.UserList.Add(new UserItem());
        }

        private void button_RemoveUser_Click(object sender, EventArgs e)
        {
            if (listBox_UserList.SelectedItems.Count > 0)
            {
                UserItem item = (listBox_UserList.SelectedItems[0] as UserItem);

                provider.UserList.Remove(item);
            }
        }

        private void button_CopyUser_Click(object sender, EventArgs e)
        {
            if (listBox_UserList.SelectedItems.Count > 0)
            {
                UserItem item = (listBox_UserList.SelectedItems[0] as UserItem);
                UserItem _item = (UserItem)item.Clone();

                provider.UserList.Add(_item);
            }
        }

        private void button_AddServer_Click(object sender, EventArgs e)
        {
            provider.ServerList.Add(new ServerItem());
        }

        private void button_RemoveServer_Click(object sender, EventArgs e)
        {
            if (listBox_ServerList.SelectedItems.Count > 0)
            {
                ServerItem item = (listBox_ServerList.SelectedItems[0] as ServerItem);

                provider.ServerList.Remove(item);
            }
        }

        private void button_CopyServer_Click(object sender, EventArgs e)
        {
            if (listBox_ServerList.SelectedItems.Count > 0)
            {
                ServerItem item = (listBox_ServerList.SelectedItems[0] as ServerItem);
                ServerItem _item = (ServerItem)item.Clone();

                provider.ServerList.Add(_item);
            }
        }

        private void listBox_ServerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_ServerList.SelectedItems.Count > 0)
            {
                ServerItem item = (listBox_ServerList.SelectedItems[0] as ServerItem);

                propertyGrid.SelectedObject = item;
            }
        }

        private void button_AddApi_Click(object sender, EventArgs e)
        {
            provider.ApiList.Add(new ApiItem() { LinkedUserList = provider.UserList, LinkedServerList = provider.ServerList });
        }

        private void button_RemoveApi_Click(object sender, EventArgs e)
        {
            if (listBox_ApiList.SelectedItems.Count > 0)
            {
                ApiItem item = (listBox_ApiList.SelectedItems[0] as ApiItem);

                provider.ApiList.Remove(item);
            }
        }

        private void button_CopyApi_Click(object sender, EventArgs e)
        {
            if (listBox_ApiList.SelectedItems.Count > 0)
            {
                ApiItem item = (listBox_ApiList.SelectedItems[0] as ApiItem);
                ApiItem _item = (ApiItem)item.Clone();

                provider.ApiList.Add(_item);
            }
        }

        private void listBox_ApiList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_ApiList.SelectedItems.Count > 0)
            {
                ApiItem item = (listBox_ApiList.SelectedItems[0] as ApiItem);

                propertyGrid.SelectedObject = item;
            }
        }

        private SingleProvider provider;
        public void Init(SingleProvider provider)
        {
            this.provider = provider;
            provider.Load();
        }

        private void ApiManagerForm_Load(object sender, EventArgs e)
        {
            userItemBindingSource.DataSource = provider.UserList;
            serverItemBindingSource.DataSource = provider.ServerList;
            apiItemBindingSource.DataSource = provider.ApiList;
        }

        private void ApiManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            provider.Save();
        }
    }
}
