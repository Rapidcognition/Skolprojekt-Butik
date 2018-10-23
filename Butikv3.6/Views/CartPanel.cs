using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class CartPanel : TableLayoutPanel
    {
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
            leftMenuPanel.Controls.Add(saveCartButton);
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
                    Text = product.price.ToString() + "kr",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Left,
                };
                productPanel.Controls.Add(priceLabel);

                NumericUpDown productCounter = new NumericUpDown
                {
                    Dock = DockStyle.Left,
                    AutoSize = true,
                    Value = 1,
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

                priceLabelRef.Text = (productRef.price * numberOfProducts.Value) + "kr";
            }
        }
    }
}