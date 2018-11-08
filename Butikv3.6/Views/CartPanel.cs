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
        Receipt receiptForm;
        static double Sum { get; set; }
        private Label sumBeforDis;
        private Label sumAfterDis;
        private Label sumLabel;
        private bool codeActive = false;
        private const string SaveFolder = "saveFolder";
        private const string TempSaveFile = "saveFile.csv";

        private List<Product> cartItems = new List<Product>();
        private List<Product> productList = new List<Product>();

        public CartPanel()
        {
            QueryFromCSVToList();

            #region left menu
            Button checkoutButton = new Button
            {
                Text = "Checkout",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,

            };
            checkoutButton.Click += CheckoutButton_Click;
            leftPanel.Controls.Add(checkoutButton);

            Button saveCartButton = new Button
            {
                Text = "Save Cart",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            saveCartButton.Click += SaveCartButton_Click;
            leftPanel.Controls.Add(saveCartButton);

            Button loadCartButton = new Button
            {
                Text ="Read cart from CSV",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            loadCartButton.Click += LoadCartButton_Click;
            leftPanel.Controls.Add(loadCartButton);

            Button clearCartButton = new Button
            {
                Text = "Clear Cart",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            clearCartButton.Click += ClearCartButton_Click;
            leftPanel.Controls.Add(clearCartButton);

            Label DiscountCodeLable = new Label
            {
                Text = "Discount Code here!",
                Font = new Font("Arial",9),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.BottomCenter,
            };
            leftPanel.Controls.Add(DiscountCodeLable);

            TextBox DiscountCodeBox = new TextBox
            {
                Name = "discountCodeBox",
                Text = "Discount code",
                Dock= DockStyle.Top,
                Font = new Font("Arial", 10),
                AutoSize = true,
                TextAlign= HorizontalAlignment.Center,
                BackColor = Color.White,
            };
            leftPanel.Controls.Add(DiscountCodeBox);
            DiscountCodeBox.GotFocus += ClearText;
            DiscountCodeBox.KeyPress += CheckCode;

            sumBeforDis = new Label
            {
                Text = "Your amout befor discount:"+ GetSumOfProducts() + " kr",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height=55,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            leftPanel.Controls.Add(sumBeforDis);

            sumAfterDis = new Label
            {
                Text = "Your amout After discount:" + GetSumOfProducts() + " kr",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height=50,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            leftPanel.Controls.Add(sumAfterDis);
            #endregion

            #region Bottom panel
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
                Text = "Sum: " + GetSumOfProducts() + " kr",

            };
            sumOfProductsPanel.Controls.Add(sumLabel);
            #endregion
        }

        public List<Product> GetProductList()
        {
            return productList;
        }

        /// <summary>
        /// Method to ReadAllLines from database and store in (products)list,
        /// also store all the different types in a (string)list.
        /// </summary>
        private void QueryFromCSVToList()
        {
            productList = File.ReadAllLines(@"TextFile1.csv").Select(x => Product.FromCSV(x)).
                OrderByDescending(x => x.name).
                OrderByDescending(x => x.interestPoints).ToList(); 
        }

        private void ClearText(object sender, EventArgs e)
        {
            TextBox textB = (TextBox)sender;
            if (textB.BackColor != Color.LightGreen)
            {
                textB.Clear();
                textB.BackColor = Color.White;
            }
            else
                textB.AcceptsReturn = false;
        }

        private void CheckCode(object sender, EventArgs e)
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
                    break;
                }
            }
            File.WriteAllText(@"RabatCoder.csv", string.Empty);
            File.WriteAllLines(@"RabatCoder.csv", DisCodList);

            if(codeActive)
            {
                txtbcode.Enabled = false;
            }
        }


        private void CheckoutButton_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            
            foreach (Product product in productList)
            {
                if(product.stage2)
                {
                    product.stage3 = true;
                }
                product.CalculateInterestPoints();
                
                lines.Add(product.ToDatabaseCSV());
            }

            foreach (Product product in productList)
            {
                Console.WriteLine($"{product.name}: {product.stage1}, {product.stage2}, {product.stage3}");
            }

            receiptForm = new Receipt(cartItems ,Sum);
            receiptForm.Show();
            
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

        private void SaveCartButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Cannot save an empty cart.", "༼つಠ益ಠ༽つ", MessageBoxButtons.OK);
            }
            else
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Csv file|*.csv";
                fileDialog.Title = "Save shopping cart";
                fileDialog.InitialDirectory = SaveFolder;

                DialogResult result = fileDialog.ShowDialog();
                string saveFilePath = fileDialog.FileName;

                if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Shopping cart was not saved.", "☉ ‿ ⚆", MessageBoxButtons.OK);
                }
                else if (result == DialogResult.OK)
                {
                    string[] lines = new string[cartItems.Count];

                    for (int i = 0; i < cartItems.Count; i++)
                    {
                        lines[i] = cartItems[i].ToCSV();
                    }

                    File.WriteAllLines(saveFilePath, lines);
                }
            }
        }

        private void LoadCartButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Csv file|*.csv";
            fileDialog.Title = "Read from save file";
            fileDialog.InitialDirectory = SaveFolder;

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show("No save file selected.", @"¯\_(ツ)_/¯", MessageBoxButtons.OK);
            }
            else if (result == DialogResult.OK)
            {
                ClearCart();
                string saveFilePath = fileDialog.FileName;
                var saveFileContents = File.ReadAllLines(saveFilePath).
                    Select(x => Product.FromCSV(x)).
                    OrderBy(x => x.name).OrderBy(x => x.type).ToList();

                foreach (Product product in saveFileContents)
                {
                    AddToCart(product);
                }
            }
        }

        private void ClearCartButton_Click(object sender, EventArgs e)
        {
            ClearCart();
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

            this.Controls["leftMenuPanel"].Controls["discountCodeBox"].Enabled = true;
            this.Controls["leftMenuPanel"].Controls["discountCodeBox"].Text = "Discount Code";
            this.Controls["leftMenuPanel"].Controls["discountCodeBox"].BackColor = Color.White;
            codeActive = false;
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
                priceLabelRef.Text = productRef.price + "kr";
            }
            else
            {
                product.stage2 = true;
                cartItems.Add(product);
                TableLayoutPanel productPanel = new TableLayoutPanel
                {
                    Name = product.name,
                    ColumnCount = 4,
                    RowCount = 1,
                    Anchor = AnchorStyles.Top,
                    Height = 60,
                    Width = 300,
                    Margin = new Padding(0),
                };
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                productPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
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
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 10, FontStyle.Bold),
                };
                productLabel.Click += ProductPanel_Click;
                productPanel.Controls.Add(productLabel);

                Label priceLabel = new Label
                {
                    Name = "priceLabel",
                    Text = product.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 9, FontStyle.Bold),
                };
                priceLabel.Click += ProductPanel_Click;
                productPanel.Controls.Add(priceLabel);

                NumericUpDown productCounter = new NumericUpDown
                {
                    Name = "productCounter",
                    Dock = DockStyle.Left,
                    Anchor = AnchorStyles.Left,
                    Value = product.nrOfProducts,
                    Font = new Font("Calibri", 9, FontStyle.Bold),
                };
                productCounter.ValueChanged += ProductCounter_ValueChanged;
                productPanel.Controls.Add(productCounter);

                //
                productPanel.Tag = product;
            }
            UpdateSummaryPanel();
        }

        private void ProductCounter_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown productCounterRef = (NumericUpDown)sender;
            

            if (productCounterRef.Value == 0)
            {
                // get the selected product from cart and remove it from cartItems
                Product p = (Product)(productCounterRef.Parent as TableLayoutPanel).Tag;
                p.stage2 = false;
                cartItems.Remove(p);

                // dispose the parent container when the counter reaches 0
                productCounterRef.Parent.Dispose();
            }
            else
            {
                TableLayoutPanel productPanelRef = (TableLayoutPanel)productCounterRef.Parent;
                Product productRef = (Product)productPanelRef.Tag;
                Label priceLabelRef = (Label)productPanelRef.Controls["priceLabel"];
                priceLabelRef.Text = productRef.price + "kr";
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

                Console.WriteLine(productRef.stage1);
                Console.WriteLine(productRef.stage2);
                Console.WriteLine(productRef.stage3);

            }
            else if(sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                productPanelRef = (TableLayoutPanel)p.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
                UpdateSelectedProduct(productPanelRef);

                Console.WriteLine(productRef.stage1);
                Console.WriteLine(productRef.stage2);
                Console.WriteLine(productRef.stage3);
            }
            else if(sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                productPanelRef = (TableLayoutPanel)l.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
                UpdateSelectedProduct(productPanelRef);

                Console.WriteLine(productRef.stage1);
                Console.WriteLine(productRef.stage2);
                Console.WriteLine(productRef.stage3);
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
            nrOfProducts = cartItems.Select(x => x.nrOfProducts).Sum();
            return nrOfProducts.ToString();
        }

        /// <summary>
        /// Returns a string representing the sum of all products in cart.
        /// </summary>
        private double GetSumOfProducts()
        {
            Sum = cartItems.Select(x => x.price * x.nrOfProducts).Sum();
            return Sum;
        }

        private double GetSumOfProductsAfterDis()
        {
            Sum = cartItems.Select(x => x.price * x.nrOfProducts).Sum();
            Sum -= Math.Round(Sum * 15, 2)/100;
            return Sum;
        }
    }
}