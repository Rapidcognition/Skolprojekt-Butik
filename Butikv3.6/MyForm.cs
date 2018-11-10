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
        HomePanel home;
        
        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);
            this.Font = new Font("Calibri", 12);
            this.FormClosed += MyForm_IsClosed;

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
                BackColor = Color.White,
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

            PictureBox headerPicture = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Image.FromFile("Background/Header1.jpg"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.Fixed3D,
            };
            headerPicture.Click += HeaderPicture_Click;
            topPanel.Controls.Add(headerPicture);

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

            // Send store as parameter to homePanel to connect them so we
            // can display clicked product in the descriptionPanel in storePanel.
            home = new HomePanel(cart, store);

            mainPanel.Controls.Add(store, 0, 1);
            mainPanel.Controls.Add(cart, 0, 1);
            mainPanel.Controls.Add(home, 0, 1);
            cart.Hide();
            store.Hide();
            home.Select();
            store.PopulateStorePanel(cart.GetProductList());
        }

        private void MyForm_IsClosed(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            foreach (Product product in cart.GetProductList())
            {
                product.CalculateInterestPoints();
                lines.Add(product.ToDatabaseCSV());
            }
            try
            {
                File.WriteAllLines("TextFile1.csv", lines);
            }
            catch
            {
                // If the program decides to crash, or something.
                File.WriteAllLines("TextFile1.csv", lines);
            }
        }

        private void HeaderPicture_Click(object sender, EventArgs e)
        {
            home.Show();
            cart.Hide();
            store.Hide();
            ActiveControl = null;
        }

        private void ViewChangedButton_Click(object sender, EventArgs e)
        {
            if((sender as Button).Name == "Store")
            {
                cart.Hide();
                home.Hide();
                store.Show();
                ActiveControl = null;
            }
            else if((sender as Button).Name == "Cart")
            {
                store.Hide();
                home.Hide();
                cart.Show();
                ActiveControl = null;
            }
        }
    }
}