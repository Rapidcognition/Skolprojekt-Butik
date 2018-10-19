using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._5
{
    class StorePanel : TableLayoutPanel
    {
        FlowLayoutPanel storePanel;
        TableLayoutPanel temp;

        List<Product> storeList = new List<Product>();
        public StorePanel()
        {
            this.Name = "Store";
            this.Dock = DockStyle.Fill;

            storePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                FlowDirection = FlowDirection.LeftToRight,
            };
            Controls.Add(storePanel);

            Label foo = new Label
            {
                Text = "Store Form.",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Orange,
                Height = 50,
                Width = 300,
            };
            storePanel.Controls.Add(foo);
            ListToStorePanel();
        }

        public void AddItemToStorePanel(Product p)
        {
            Product temp = new Product(p);
            storeList.Add(temp);
        }
        public void ListToStorePanel()
        {
            foreach (var item in storeList)
            {
                temp = new TableLayoutPanel
                {
                    BackColor = Color.Orange,
                    Height = 50,
                    Width = 300,
                    ColumnCount = 3,
                };
                temp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
                Button b = new Button { Text = item.name };
                storePanel.Controls.Add(temp);
                temp.Controls.Add(b);
            }
        }
    }
}
