using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class CartPanel : TableLayoutPanel
    {
        private const string saveFolder = "saveFolder";
        private const string tempSaveFile = "saveFile.csv";

        FlowLayoutPanel itemPanel;
        List<Product> cartItems = new List<Product>();

        public CartPanel()
        {
            this.ColumnCount = 3;
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            #region left menu
            TableLayoutPanel leftMenuPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            this.Controls.Add(leftMenuPanel);

            Button checkoutButton = new Button
            {
                Text = "Checkout",
                Dock = DockStyle.Top,
            };
            leftMenuPanel.Controls.Add(checkoutButton);

            Button saveCartButton = new Button
            {
                Text = "Save Cart",
                Dock = DockStyle.Top,
            };
            saveCartButton.Click += SaveCartButton_Click;
            leftMenuPanel.Controls.Add(saveCartButton);

            Button loadCartButton = new Button
            {
                Text ="Read cart from CSV",
                Dock = DockStyle.Top,
            };
            loadCartButton.Click += LoadCartButton_Click;
            leftMenuPanel.Controls.Add(loadCartButton);

            Button clearCartButton = new Button
            {
                Text = "Clear Cart",
                Dock = DockStyle.Top,
            };
            clearCartButton.Click += ClearCartButton_Click;
            leftMenuPanel.Controls.Add(clearCartButton);
            #endregion

            #region Product panel
            itemPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
            };
            this.Controls.Add(itemPanel);
            #endregion

            #region Description panel
            TableLayoutPanel descriptionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            this.Controls.Add(descriptionPanel);
            #endregion

        }

        private void ClearCart()
        {
            itemPanel.Controls.Clear();
            foreach (Product p in cartItems)
            {
                p.nrOfProducts = 1;
            }
            cartItems.Clear();
        }

        private void LoadCartButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(saveFolder)) 
            {
                Directory.CreateDirectory(saveFolder);
            }

            ClearCart();
            if (File.Exists(saveFolder + "/" + tempSaveFile)) 
            {
                string[][] path = File.ReadAllLines(saveFolder + "/" + tempSaveFile).Select(x => x.Split(',')).
                Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "" && x[4] != "" && x[5] != "").
                ToArray();

                for(int i = 0; i < path.Length; i++)
                {
                    Product tmp = new Product
                    {
                        price = int.Parse(path[i][0]),
                        name = path[i][1],
                        type = path[i][2],
                        summary = path[i][3],
                        imageLocation = path[i][4],
                        nrOfProducts = int.Parse(path[i][5]),
                    };
                    AddToCart(tmp);
                }
            }
        }

        private void SaveCartButton_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            if(cartItems.Count != 0)
            {
                string[] lines = new string[cartItems.Count];

                for(int i = 0; i < cartItems.Count; i++)
                {
                    lines[i] = cartItems[i].ToCSV();
                }

                File.WriteAllLines(saveFolder + "/" + tempSaveFile, lines);
            }
        }

        private void ClearCartButton_Click(object sender, EventArgs e)
        {
            ClearCart();
        }

        public void AddToCart(Product product)
        {
            if (itemPanel.Controls.ContainsKey(product.name))
            {
                NumericUpDown numberOfProductsRef = (NumericUpDown)itemPanel.Controls[product.name].Tag;
                numberOfProductsRef.Value++;

                Label priceLabelRef = (Label)numberOfProductsRef.Tag;
                priceLabelRef.Text = (product.price * numberOfProductsRef.Value) + "kr";
            }
            else
            {
                cartItems.Add(product);
                TableLayoutPanel productPanel = new TableLayoutPanel
                {
                    Name = product.name,
                    ColumnCount = 4,
                    RowCount = 1,
                    Anchor = AnchorStyles.Top,
                    Height = 60,
                    Width = 410,
                };
                this.itemPanel.Controls.Add(productPanel);

                PictureBox productPicture = new PictureBox
                {
                    ImageLocation = product.imageLocation,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Top,
                };
                productPanel.Controls.Add(productPicture);

                Label productLabel = new Label
                {
                    Text = product.name,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Left,
                };
                productPanel.Controls.Add(productLabel);

                Label priceLabel = new Label
                {
                    Text = (product.price * product.nrOfProducts) + "kr",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Left,
                };
                productPanel.Controls.Add(priceLabel);

                NumericUpDown productCounter = new NumericUpDown
                {
                    Dock = DockStyle.Left,
                    AutoSize = true,
                    Value = product.nrOfProducts,
                };
                productCounter.ValueChanged += ProductCounter_ValueChanged;
                productPanel.Controls.Add(productCounter);

                // 
                productPanel.Tag = productCounter;
                productCounter.Tag = priceLabel;
                priceLabel.Tag = product;
            }
        }

        private void ProductCounter_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numberOfProducts = (NumericUpDown)sender;

            if (numberOfProducts.Value == 0)
            {
                numberOfProducts.Parent.Dispose();
            }
            else
            {
                Label priceLabelRef = (Label)numberOfProducts.Tag;
                Product productRef = (Product)priceLabelRef.Tag;
                productRef.nrOfProducts = int.Parse(numberOfProducts.Value.ToString());
                priceLabelRef.Text = (productRef.price * numberOfProducts.Value) + "kr";
            }
        }
    }
}