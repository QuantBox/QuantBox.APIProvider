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
using XAPI;

namespace QuantBox.APIProvider.UI
{
    public partial class ApiControlForm : Form
    {
        public ApiControlForm()
        {
            InitializeComponent();
        }

        private SingleProvider provider;
        public void Init(SingleProvider provider)
        {
            this.provider = provider;
        }

        private void button_QueryOrder_Click(object sender, EventArgs e)
        {
            ReqQueryField query = new ReqQueryField();
            query.PortfolioID1 = textBox_PortfolioID1.Text;
            query.PortfolioID2 = textBox_PortfolioID2.Text;
            query.PortfolioID3 = textBox_PortfolioID3.Text;
            query.Business = (BusinessType)Enum.Parse(typeof(BusinessType),comboBox_BusinessType.Text);
            
            provider._QueryApi.ReqQuery(QueryType.ReqQryOrder, query);
        }

        private void button_QueryTrade_Click(object sender, EventArgs e)
        {
            ReqQueryField query = new ReqQueryField();
            query.PortfolioID1 = textBox_PortfolioID1.Text;
            query.PortfolioID2 = textBox_PortfolioID2.Text;
            query.PortfolioID3 = textBox_PortfolioID3.Text;
            query.Business = (BusinessType)Enum.Parse(typeof(BusinessType), comboBox_BusinessType.Text);

            provider._QueryApi.ReqQuery(QueryType.ReqQryTrade, query);
        }

        private void button_QueryAccount_Click(object sender, EventArgs e)
        {
            ReqQueryField query = new ReqQueryField();
            query.PortfolioID1 = textBox_PortfolioID1.Text;
            query.PortfolioID2 = textBox_PortfolioID2.Text;
            query.PortfolioID3 = textBox_PortfolioID3.Text;
            query.Business = (BusinessType)Enum.Parse(typeof(BusinessType), comboBox_BusinessType.Text);

            provider._QueryApi.ReqQuery(QueryType.ReqQryTradingAccount, query);
        }

        private void button_QueryPosition_Click(object sender, EventArgs e)
        {
            ReqQueryField query = new ReqQueryField();
            query.PortfolioID1 = textBox_PortfolioID1.Text;
            query.PortfolioID2 = textBox_PortfolioID2.Text;
            query.PortfolioID3 = textBox_PortfolioID3.Text;
            query.Business = (BusinessType)Enum.Parse(typeof(BusinessType), comboBox_BusinessType.Text);

            provider._QueryApi.ReqQuery(QueryType.ReqQryInvestorPosition, query);
        }

        private void ApiControlForm_Load(object sender, EventArgs e)
        {
            comboBox_BusinessType.DataSource = Enum.GetValues(typeof(BusinessType));
        }
    }
}
