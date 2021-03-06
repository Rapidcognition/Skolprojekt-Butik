﻿using System;
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
        private bool codeActive = false;
        private const string SaveFolder = "saveFolder";
        private const string TempSaveFile = "saveFile.csv";
        private bool CashIsKing = false;

        private List<Product> cartItems = new List<Product>();
        private List<Product> productList = new List<Product>();

        public CartPanel()
        {
            QueryFromCSVToList();

            #region left menu
            base.leftPanel.RowCount = 2;
            base.leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            base.leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            TableLayoutPanel discountPanel = new TableLayoutPanel
            {
                Name = "discountPanel",
                RowCount = 5,
                Dock = DockStyle.Fill,
                Width = 10,
            };
            discountPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            discountPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            discountPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            discountPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            discountPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            base.leftPanel.Controls.Add(discountPanel);

            Button checkoutButton = new Button
            {
                Text = "Gå till kassa",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            checkoutButton.Click += CheckoutButton_Click;
            discountPanel.Controls.Add(checkoutButton);

            Label DiscountCodeLable = new Label
            {
                Text = "Ange din rabatt kod!",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.BottomCenter,
            };
            discountPanel.Controls.Add(DiscountCodeLable);
            
            TextBox DiscountCodeBox = new TextBox
            {
                Name = "discountCodeBox",
                Text = "Rabatt kod",
                Dock = DockStyle.Top,
                Font = new Font("Arial", 10),
                AutoSize = true,
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.White,
            };
            discountPanel.Controls.Add(DiscountCodeBox);
            DiscountCodeBox.GotFocus += ClearText;
            DiscountCodeBox.KeyPress += CheckCode;

            sumBeforDis = new Label
            {
                Text = "Total kostnad\nföre rabatt: " + GetSumOfProducts() + " kr",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            discountPanel.Controls.Add(sumBeforDis);

            sumAfterDis = new Label
            {
                Text = "Total kostnad\nefter rabatt: " + GetSumOfProducts() + " kr",
                Font = new Font("Arial", 9),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            discountPanel.Controls.Add(sumAfterDis);

            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                Width = 10,
            };
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            base.leftPanel.Controls.Add(buttonPanel);

            Button saveCartButton = new Button
            {
                Text = "Spara varukorg",
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            saveCartButton.Click += SaveCartButton_Click;
            buttonPanel.Controls.Add(saveCartButton);

            Button loadCartButton = new Button
            {
                Text = "Hämta varukorg",
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            loadCartButton.Click += LoadCartButton_Click;
            buttonPanel.Controls.Add(loadCartButton);

            Button clearCartButton = new Button
            {
                Text = "Töm varukorg",
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Calibri", 10, FontStyle.Bold),
                BackColor = Color.DarkKhaki,
                FlatStyle = FlatStyle.Popup,
            };
            clearCartButton.Click += ClearCartButton_Click;
            buttonPanel.Controls.Add(clearCartButton);
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
            List<string> DisCodList = File.ReadAllLines(@"Rabattkoder.csv").ToList();
            foreach (string item in DisCodList)
            {
                if(txtbcode.Text == "$$$")
                {
                    CashIsKing = true;
                    txtbcode.BackColor = Color.LightGreen;
                    codeActive = true;
                    sumAfterDis.Font = new Font("Arial", 9, FontStyle.Bold);
                    sumAfterDis.Text = "'Påsk-event'\naktiverat\n#cringe";
                    (descriptionPanel.Controls["descriptionPicture"] as PictureBox).Image = Image.FromFile(@"gimmeYourMoney.gif");
                    (descriptionPanel.Controls["descriptionProductName"] as Label).Text = "Everything is free!";
                    break;
                }
                if (txtbcode.Text == item)
                {
                    DisCodList.Remove(item);
                    txtbcode.BackColor = Color.LightGreen;
                    sumAfterDis.Text = "Kostnad efter rabatt: " + GetSumOfProductsAfterDis() + " kr";
                    codeActive = true;
                    break;
                }
            }
            File.WriteAllText(@"Rabattkoder.csv", string.Empty);
            File.WriteAllLines(@"Rabattkoder.csv", DisCodList);

            if(codeActive)
            {
                txtbcode.Enabled = false;
            }
        }

        private void CheckoutButton_Click(object sender, EventArgs e)
        {
            if(cartItems.Count == 0)
            {
                return;
            }
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

            receiptForm = new Receipt(cartItems ,Sum,CashIsKing);
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
                MessageBox.Show("Kan inte spara en tom varukorg!", "༼つಠ益ಠ༽つ", MessageBoxButtons.OK);
            }
            else
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Csv file|*.csv";
                fileDialog.Title = "Spara varukorg.";
                fileDialog.InitialDirectory = SaveFolder;

                DialogResult result = fileDialog.ShowDialog();
                string saveFilePath = fileDialog.FileName;

                if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Varukorgen blev inte sparad.", "☉ ‿ ⚆", MessageBoxButtons.OK);
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
            fileDialog.Title = "Hämta sparad varukorg.";
            fileDialog.InitialDirectory = SaveFolder;

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show("Ingen fil vald.", @"¯\_(ツ)_/¯", MessageBoxButtons.OK);
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


            (base.leftPanel.Controls["discountPanel"] as TableLayoutPanel).
                Controls["discountCodeBox"].Enabled = true;

            (base.leftPanel.Controls["discountPanel"] as TableLayoutPanel).
                Controls["discountCodeBox"].Text = "Discount Code";

            (base.leftPanel.Controls["discountPanel"] as TableLayoutPanel).
                Controls["discountCodeBox"].BackColor = Color.White;

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
                    Margin = new Padding(0, 0, 15, 5),
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
                    Maximum = 99,
                };
                productCounter.ValueChanged += ProductCounter_ValueChanged;
                productPanel.Controls.Add(productCounter);
                
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

        private void UpdateSummaryPanel()
        {
            if(codeActive)
            {
                sumBeforDis.Text = "Kostnad före rabattkod: " + GetSumOfProducts() + " kr";
                sumAfterDis.Text = "Kostnad efter rabattkod:\n" + GetSumOfProductsAfterDis() + " kr";
            }
            else
            {
                sumBeforDis.Text = "Kostnad före rabattkod: " + GetSumOfProducts() + " kr";
                sumAfterDis.Text = "Kostnad efter rabattkod:\n" + GetSumOfProducts() + " kr";
            }

        }

        private string GetNrOfProducts()
        {
            int nrOfProducts = 0;
            nrOfProducts = cartItems.Select(x => x.nrOfProducts).Sum();
            return nrOfProducts.ToString();
        }

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