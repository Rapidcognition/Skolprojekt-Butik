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
        CartPanel cart;
        StorePanel store;
        
        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);

            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                RowCount = 2,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                Padding = new Padding(0),
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
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            mainPanel.Controls.Add(topPanel);

            Button storeButton = new Button
            {
                Name = "Store",
                BackgroundImage = Image.FromFile("Icons/store.png"),
                BackgroundImageLayout = ImageLayout.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            storeButton.Click += ViewChangedButton_Click;
            topPanel.Controls.Add(storeButton);

            Label shopTitle = new Label
            {
                Text = "[Placeholder]",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            topPanel.Controls.Add(shopTitle);

            Button cartButton = new Button
            {
                Name = "Cart",
                BackgroundImage = Image.FromFile("Icons/cart.png"),
                BackgroundImageLayout = ImageLayout.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            cartButton.Click += ViewChangedButton_Click;
            topPanel.Controls.Add(cartButton);

            cart = new CartPanel();
            store = new StorePanel(cart);

            mainPanel.Controls.Add(store, 0,1);
            mainPanel.Controls.Add(cart, 0, 1);
            cart.Hide();
        }

        private void ViewChangedButton_Click(object sender, EventArgs e)
        {
            if((sender as Button).Name == "Store")
            {
                cart.Hide();
                store.Show();
                ActiveControl = null;
            }
            else if((sender as Button).Name == "Cart")
            {
                store.Hide();
                cart.Show();
                ActiveControl = null;
            }
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hej!");
        }
    }
}
