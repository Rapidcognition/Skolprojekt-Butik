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
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
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
                Margin = new Padding(6),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                BorderStyle = BorderStyle.Fixed3D,
            };
            this.Controls.Add(itemPanel);
            #endregion

            #region Description panel
            TableLayoutPanel descriptionPanel = new TableLayoutPanel
            {
                Name = "descriptionPanel",
                Dock = DockStyle.Fill,
                RowCount = 3,
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            this.Controls.Add(descriptionPanel);

            PictureBox descriptionPicture = new PictureBox
            {
                Name = "descriptionPicture",
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            descriptionPanel.Controls.Add(descriptionPicture);

            Label descriptionProductName = new Label
            {
                Name = "descriptionProductName",
                Text = "No product selected",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(descriptionProductName);

            Label descriptionProductSummary = new Label
            {
                Name = "descriptionProductSummary",
                Text = " ---",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(descriptionProductSummary);
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

        private void ProductCounter_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown productCounterRef = (NumericUpDown)sender;

            if (productCounterRef.Value == 0)
            {
                // get the selected product from cart and remove it from cartItems
                Product p = (Product)(productCounterRef.Parent as TableLayoutPanel).Tag;
                cartItems.Remove(p);

                // dispose the parent container when the counter reaches 0
                productCounterRef.Parent.Dispose();
            }
            else
            {
                TableLayoutPanel productPanelRef = (TableLayoutPanel)productCounterRef.Parent;
                Product productRef = (Product)productPanelRef.Tag;
                Label priceLabelRef = (Label)productPanelRef.Controls["priceLabel"];
                priceLabelRef.Text = (productRef.price * productCounterRef.Value) + "kr";
            }
        }

        public void AddToCart(Product product)
        {
            if (itemPanel.Controls.ContainsKey(product.name))
            {
                TableLayoutPanel productPanelRef = (TableLayoutPanel)itemPanel.Controls[product.name];
                NumericUpDown productCounterRef = (NumericUpDown)productPanelRef.Controls["productCounter"];
                Label priceLabelRef = (Label)productPanelRef.Controls["priceLabel"];
                Product productRef = (Product)productPanelRef.Tag;

                productCounterRef.Value++;
                priceLabelRef.Text = (productRef.price * productCounterRef.Value) + "kr";
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
                    Width = 385,
                };
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.Click += ProductPanel_Click;
                this.itemPanel.Controls.Add(productPanel);

                PictureBox productPicture = new PictureBox
                {
                    ImageLocation = product.imageLocation,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Left,
                    BorderStyle = BorderStyle.Fixed3D,
                };
                productPicture.Click += ProductPanel_Click;
                productPanel.Controls.Add(productPicture);

                Label productLabel = new Label
                {
                    Text = product.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left
                };
                productLabel.Click += ProductPanel_Click;
                productPanel.Controls.Add(productLabel);

                Label priceLabel = new Label
                {
                    Name = "priceLabel",
                    Text = (product.price * product.nrOfProducts) + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Right,
                };
                priceLabel.Click += ProductPanel_Click;
                productPanel.Controls.Add(priceLabel);

                NumericUpDown productCounter = new NumericUpDown
                {
                    Name = "productCounter",
                    Dock = DockStyle.Left,
                    AutoSize = true,
                    Value = product.nrOfProducts,
                };
                productCounter.ValueChanged += ProductCounter_ValueChanged;
                productPanel.Controls.Add(productCounter);

                //
                productPanel.Tag = product;
            }
        }

        private void ProductPanel_Click(object sender, EventArgs e)
        {
            TableLayoutPanel descriptionPanelRef = (TableLayoutPanel)this.Controls["descriptionPanel"];
            TableLayoutPanel productPanelRef;
            Product productRef;

            if (sender.GetType() == typeof(TableLayoutPanel))
            {
                productPanelRef = (TableLayoutPanel)sender;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
            }
            else if(sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                productPanelRef = (TableLayoutPanel)p.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
            }
            else if(sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                productPanelRef = (TableLayoutPanel)l.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
            }
        }

        private void UpdateDescriptionPanel(TableLayoutPanel descriptionPanelRef, Product productRef)
        {
            (descriptionPanelRef.Controls["descriptionPicture"] as PictureBox).ImageLocation = productRef.imageLocation;

            (descriptionPanelRef.Controls["descriptionProductName"] as Label).Text = productRef.name;

            (descriptionPanelRef.Controls["descriptionProductSummary"]).Text = productRef.summary;
        }
    }
}