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
    class Receipt: Form
    {
        public Receipt(List<Product> ppp, double d)
        {
            Height= 500;
            AutoScroll=true;
            TableLayoutPanel ReceiptPanel = new TableLayoutPanel
            {
                RowCount = 2,
                AutoSize = true,
                Dock = DockStyle.Fill,

            };
            Controls.Add(ReceiptPanel);

            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            TableLayoutPanel panelProductsPrices = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
            };
            ReceiptPanel.Controls.Add(panelProductsPrices);

            TableLayoutPanel panelTotlpINfo = new TableLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
            };
            ReceiptPanel.Controls.Add(panelTotlpINfo);

            Label labelTime = new Label
            {
                Text = "LAGERHUS \n" +
                "Date \t :   "+DateTime.Now,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial",9,FontStyle.Bold),
                Dock = DockStyle.Top,
                Height=50,
                Margin = new Padding(0, 0, 0, 0),
            };
            panelProductsPrices.Controls.Add(labelTime);
            panelProductsPrices.SetColumnSpan(labelTime, 3);

            foreach (Product item in ppp)
            {

                Label labelName = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial",9),
                    Dock = DockStyle.Top,
                };
                panelProductsPrices.Controls.Add(labelName);
                Label labelProductsAmount = new Label
                {
                    Text = item.price.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                panelProductsPrices.Controls.Add(labelProductsAmount);
                Label labelProductTotPrice = new Label
                {
                    Text = item.nrOfProducts.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                panelProductsPrices.Controls.Add(labelProductTotPrice);

            }

            if(d!= 0)
            {
                Label labelMoms = new Label
                {
                    Text = "         Moms: 25%         "+ d*0.25+
                    "\nKEEP YOUR RECEIPT!",
                    TextAlign = ContentAlignment.BottomLeft,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Bottom,
                    Height = 35,
                };
                panelProductsPrices.Controls.Add(labelMoms);
                panelProductsPrices.SetColumnSpan(labelMoms, 3);
            }

            Label labelTotalSum = new Label
            {
                Text = "The total amount is: "+d+"kr",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 9),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                //Margin = new Padding(0, 0, 0, 0),
            };
            panelTotlpINfo.Controls.Add(labelTotalSum);

            Label labelInfo = new Label
            {
                Text = "30 days to return without useing the produkts!",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 9),
                Dock = DockStyle.Fill,
               // Margin = new Padding(0, 0, 0, 0),
            };
            panelTotlpINfo.Controls.Add(labelInfo);
        }
    }
    class MyForm : Form
    {
        CartPanel cart;
        StorePanel store;
        
        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);
            this.MaximumSize = new Size(1000, 700);
            this.Font = new Font("Calibri", 12);

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
            

            mainPanel.Controls.Add(store, 0, 1);
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
