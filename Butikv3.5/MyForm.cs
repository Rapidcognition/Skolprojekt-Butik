using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._5
{
    class MyForm : Form
    {
        #region Main form variables
        TableLayoutPanel mainPanel;
        TableLayoutPanel topLeftSidePanel;
        TableLayoutPanel topRightSidePanel;
        TextBox searchBox;

        CartPanel cart = new CartPanel();
        StorePanel store = new StorePanel();
        #endregion

        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);

            #region Main form panels

            mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Blue,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                RowCount = 2,
                ColumnCount = 2,
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            Controls.Add(mainPanel);
            mainPanel.Controls.Add(store, 1, 1);
            mainPanel.Controls.Add(cart, 1, 1);
            cart.Hide();

            topLeftSidePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Bisque,
                RowCount = 2,
            };
            topLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            topLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            mainPanel.Controls.Add(topLeftSidePanel);

            Label searchLabel = new Label { Text = "Filter items.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, };
            topLeftSidePanel.Controls.Add(searchLabel);
            searchBox = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            topLeftSidePanel.Controls.Add(searchBox);

            topRightSidePanel = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.BlanchedAlmond,
            };
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            mainPanel.Controls.Add(topRightSidePanel);

            Label shopTitle = new Label
            {
                Text = "[ SHOP TITLE ]",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            topRightSidePanel.Controls.Add(shopTitle);
            Button storeButton = new Button
            {
                Text = "Store",
                Dock = DockStyle.Fill,
            };
            topRightSidePanel.Controls.Add(storeButton);
            storeButton.Click += ChangeStoreView_Click;

            Button cartButton = new Button
            {
                Text = "Cart",
                Dock = DockStyle.Fill,
            };
            topRightSidePanel.Controls.Add(cartButton);
            cartButton.Click += ChangeStoreView_Click;


            #endregion


        }

        private void ChangeStoreView_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Store")
            {
                cart.Hide();
                store.Show();
            }
            else if ((sender as Button).Text == "Cart")
            {
                store.Hide();
                cart.Show();
            }
        }
    }
}
