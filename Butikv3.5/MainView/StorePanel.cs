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
        TableLayoutPanel temp;

        List<Product> storeList = new List<Product>();
        public StorePanel()
        {
            this.Name = "Store";
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.AutoScroll = true;

        }

        public void AddItemToStorePanel(List<Product> l)
        {
            foreach (var item in l)
            {
                this.storeList.Add(item);
            }
            foreach (var item in storeList)
            {
                temp = new TableLayoutPanel
                {
                    BackColor = Color.LightCyan,
                    Dock = DockStyle.Top,
                    ColumnCount = 3,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                };
                temp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                Label b = new Label { Text = item.name, Dock = DockStyle.Fill, };
                this.Controls.Add(temp);
                temp.Controls.Add(b);
            }
        }
    }
}
