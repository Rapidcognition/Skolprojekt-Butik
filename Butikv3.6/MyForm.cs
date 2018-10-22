using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Butikv3._6
{
    class MyForm : Form
    {
        StorePanel store = new StorePanel();

        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);

            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                RowCount = 2,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            this.Controls.Add(mainPanel);

            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                Margin = new Padding(0),
            };
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            mainPanel.Controls.Add(topPanel);

            Button shopButton = new Button
            {
                Text = "Shop",
                Dock = DockStyle.Fill
            };
            topPanel.Controls.Add(shopButton);

            Label shopTitle = new Label
            {
                Text = "[Placeholder]",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            topPanel.Controls.Add(shopTitle);

            Button cartButton = new Button
            {
                Text = "Cart",
                Dock = DockStyle.Fill,
            };
            topPanel.Controls.Add(cartButton);

            mainPanel.Controls.Add(store.GetPanel());
        }
        

        private void ItemButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hej!");
        }
    }
}
