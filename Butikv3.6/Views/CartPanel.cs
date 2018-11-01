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
    class CartPanel : ViewPanel
    {
        private double Sum = 0;
        private Label sumBeforDis;
        private Label sumAfterDis;
        private Label sumLabel;
        private bool codeActive = false;
        private const string SaveFolder = "saveFolder";
        private const string TempSaveFile = "saveFile.csv";

        private FlowLayoutPanel itemPanel;
        private List<Product> cartItems = new List<Product>();

        public CartPanel()
        {
            #region left menu
            TableLayoutPanel leftMenuPanel = new TableLayoutPanel
            {
                Name = "leftMenuPanel",
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            this.SetRowSpan(leftMenuPanel, 2);
            this.Controls.Add(leftMenuPanel, 0, 0);

            Button checkoutButton = new Button
            {
                Text = "Checkout",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,

            };
            checkoutButton.Click += CheckoutButton_Click;
            leftMenuPanel.Controls.Add(checkoutButton);

            Button saveCartButton = new Button
            {
                Text = "Save Cart",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            saveCartButton.Click += SaveCartButton_Click;
            leftMenuPanel.Controls.Add(saveCartButton);

            Button loadCartButton = new Button
            {
                Text ="Read cart from CSV",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            loadCartButton.Click += LoadCartButton_Click;
            leftMenuPanel.Controls.Add(loadCartButton);

            Button clearCartButton = new Button
            {
                Text = "Clear Cart",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            clearCartButton.Click += ClearCartButton_Click;
            leftMenuPanel.Controls.Add(clearCartButton);

            Label DiscountCodeLable = new Label
            {
                Text = "Discount Code here!",
                Font = new Font("Arial",9),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.BottomCenter,
            };
            leftMenuPanel.Controls.Add(DiscountCodeLable);

            TextBox DiscountCodeBox = new TextBox
            {
                Text = "Discount code",
                Dock= DockStyle.Top,
                Font = new Font("Arial", 10),
                AutoSize = true,
                TextAlign= HorizontalAlignment.Center,
                BackColor = Color.White,

            };
            leftMenuPanel.Controls.Add(DiscountCodeBox);
            DiscountCodeBox.GotFocus += ClearText;
            DiscountCodeBox.KeyPress += ChackCode;

            sumBeforDis = new Label
            {
                Text = "Your amout befor discount:"+ GetSumOfProducts() + " kr",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height=55,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            leftMenuPanel.Controls.Add(sumBeforDis);

            sumAfterDis = new Label
            {
                Text = "Your amout After discount:" + GetSumOfProducts() + " kr",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height=50,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            leftMenuPanel.Controls.Add(sumAfterDis);
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
            this.Controls.Add(itemPanel, 1, 0);

            TableLayoutPanel sumOfProductsPanel = new TableLayoutPanel
            {
                Name = "sumOfProductsPanel",
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 0, 6, 2),
                BorderStyle = BorderStyle.Fixed3D,
            };
            sumOfProductsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            sumOfProductsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            this.Controls.Add(sumOfProductsPanel, 1, 1);

            Label nrOfProductsLabel = new Label
            {
                Name = "nrOfProductsLabel",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Number of Products: " + GetNrOfProducts(),
            };
            sumOfProductsPanel.Controls.Add(nrOfProductsLabel);

            sumLabel = new Label
            {
                Name = "sumLabel",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Text = "Sum: " + GetSumOfProducts()+" kr",
                
            };
            sumOfProductsPanel.Controls.Add(sumLabel);
            #endregion
        }

        private void ClearText(object sender, EventArgs e)
        {
            TextBox textB = (TextBox)sender;
            if (textB.BackColor != Color.LightGreen)
            {
                textB.Clear();
                textB.BackColor = Color.White;
                //textB.Hide();
            }
            else
                textB.AcceptsReturn = false;
        }

        private void ChackCode(object sender, EventArgs e)
        {
            TextBox txtbcode = (TextBox)sender;
            List<string> DisCodList = File.ReadAllLines(@"RabatCoder.csv").ToList();
            foreach (string item in DisCodList)
            {
                if (txtbcode.Text == item)
                {
                    DisCodList.Remove(item);
                    txtbcode.BackColor = Color.LightGreen;
                    sumAfterDis.Text = "Your amout After discount:\n" + GetSumOfProductsAfterDis() + " kr";
                    sumLabel.Text = "Sum: " + GetSumOfProductsAfterDis() + " kr";
                    codeActive = true;
                }
                break;
            }
            //File.Delete(@"RabatCoder.csv");
            //File.Create(@"RabatCoder.csv");
            File.WriteAllText(@"RabatCoder.csv", string.Empty);
            File.WriteAllLines(@"RabatCoder.csv", DisCodList);
        }

        private void CheckoutButton_Click(object sender, EventArgs e)
        {
            
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

                // Temp bug fix
                // Tidy up
                //

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
                    Width = 397,
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
                    Anchor = AnchorStyles.Left,
                    //AutoSize = true,
                    Value = product.nrOfProducts,
                };
                productCounter.ValueChanged += ProductCounter_ValueChanged;
                productPanel.Controls.Add(productCounter);

                //
                productPanel.Tag = product;
            }
            UpdateSummaryPanel();
        }

        private void ClearCart()
        {
            itemPanel.Controls.Clear();
            foreach (Product p in cartItems)
            {
                p.nrOfProducts = 1;
            }
            cartItems.Clear();
            UpdateSummaryPanel();
        }

        private void SaveCartButton_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }

            if(cartItems.Count != 0)
            {
                string[] lines = new string[cartItems.Count];

                for(int i = 0; i < cartItems.Count; i++)
                {
                    lines[i] = cartItems[i].ToCSV();
                }

                File.WriteAllLines(SaveFolder + "/" + TempSaveFile, lines);
            }
        }

        private void LoadCartButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SaveFolder)) 
            {
                Directory.CreateDirectory(SaveFolder);
            }

            ClearCart();
            if (File.Exists(SaveFolder + "/" + TempSaveFile)) 
            {
                string[][] path = File.ReadAllLines(SaveFolder + "/" + TempSaveFile).Select(x => x.Split(',')).
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
                productRef.nrOfProducts = (int)productCounterRef.Value;
            }
            UpdateSummaryPanel();
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
                UpdateSelectedProduct(productPanelRef);
            }
            else if(sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                productPanelRef = (TableLayoutPanel)p.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
                UpdateSelectedProduct(productPanelRef);
            }
            else if(sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                productPanelRef = (TableLayoutPanel)l.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
                UpdateSelectedProduct(productPanelRef);
            }
        }

        /// <summary>
        /// Updates sum and total number of products in cart.
        /// </summary>
        private void UpdateSummaryPanel()
        {
            if(codeActive)
            {
            ((this.Controls["sumOfProductsPanel"] as TableLayoutPanel).Controls["nrOfProductsLabel"] as Label).
                Text = "Number of Products: " + GetNrOfProducts();

            ((this.Controls["sumOfProductsPanel"] as TableLayoutPanel).Controls["sumLabel"] as Label).
                Text = "Sum: " + GetSumOfProductsAfterDis() + " kr";
            sumBeforDis.Text = "Your amout befor discount: " + GetSumOfProducts() + " kr";

            sumAfterDis.Text = "Your amout After discount:\n" + GetSumOfProductsAfterDis() + " kr";
            }
            else
            {
                ((this.Controls["sumOfProductsPanel"] as TableLayoutPanel).Controls["nrOfProductsLabel"] as Label).
                Text = "Number of Products: " + GetNrOfProducts();

                ((this.Controls["sumOfProductsPanel"] as TableLayoutPanel).Controls["sumLabel"] as Label).
                    Text = "Sum: " + GetSumOfProducts() + " kr";
                sumBeforDis.Text = "Your amout befor discount: " + GetSumOfProducts() + " kr";

                sumAfterDis.Text = "Your amout After discount:\n" + GetSumOfProducts() + " kr";
            }

        }

        /// <summary>
        /// Returns a string representing the total number of products in cart.
        /// </summary>
        private string GetNrOfProducts()
        {
            int nrOfProducts = 0;
            foreach (Product product in cartItems)
            {
                nrOfProducts += product.nrOfProducts;
            }
            return nrOfProducts.ToString();
        }

        /// <summary>
        /// Returns a string representing the sum of all products in cart.
        /// </summary>
        private double GetSumOfProducts()
        {
            Sum = 0;
            foreach (Product product in cartItems)
            {
                Sum += (product.price * product.nrOfProducts);
            }
            return Sum;
        }

        private double GetSumOfProductsAfterDis()
        {
            double sum = 0;
            foreach (Product product in cartItems)
            {
                sum += (product.price * product.nrOfProducts);
            }
            sum -= Math.Round(sum * 15, 2)/100;
            return sum;
        }
    }
}