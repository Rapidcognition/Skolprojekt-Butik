using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Butikv3._6
{
    class Product
    {
        public int price;
        public string name;
        public string type;
        public string summary;
        public string imageLocation;
        public int nrOfProducts;

        private void Product_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.summary);
        }

        public Product GetProduct()
        {
            return this;
        }

        public string ToCSV()
        {
            return $"{price},{name},{type},{summary},{imageLocation},{nrOfProducts}";
        }
    }

    class StorePanel : TableLayoutPanel
    {
        #region properties used in storePanel and all functions
        // Object that is constant, we use this reference
        // to send objects from storePanel to cartPanel.
        CartPanel cartPanelRef;

        // Left panel and its child controls.
        TableLayoutPanel leftPanel;
        TableLayoutPanel searchControlerPanel;
        TextBox searchBox;
        Button searchButton;
        Button typeButton;
        FlowLayoutPanel typePanel;

        // Controls connected to description panel
        PictureBox descriptionPicture;
        Label descriptionNameLabel;
        Label descriptionSummaryLabel;
        TableLayoutPanel descriptionPanel;


        FlowLayoutPanel itemPanel;
        // Panel that we store all items in that is displayed in itemPanel.
        TableLayoutPanel productPanel;
        Label nameLabel;
        Label priceLabel;
        PictureBox pictureBox;
        Button addToCartButton;
        // The four controls listed above is used in storePanel,
        // in method PopulateStore.
        #endregion
        
        // Used when productpanel is clicked to give it
        // a graphical change.
        private TableLayoutPanel selectedProductPanel;

        // Lists that contains our products and the type of our products
        // when we populate typePanel and PopulateStore.
        List<Product> productList = new List<Product>();
        List<string> typeList = new List<string>();

        public StorePanel(CartPanel reference)
        {
            cartPanelRef = reference;

            // Implement event that autoscales the MyForm window
            // so that images retain their scale.
            #region Main panel, 2 columns, refered to as "this.".
            this.ColumnCount = 2;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Transparent;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82));
            #endregion

            #region Left side table of "this.".
            leftPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
            };
            leftPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            this.Controls.Add(leftPanel);

            searchControlerPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Height = 15,
                Width = 55,
            };
            searchControlerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            searchControlerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));
            leftPanel.Controls.Add(searchControlerPanel);

            searchBox = new TextBox
            {
                Anchor = AnchorStyles.Top,
                Margin = new Padding(-20,1,-10,0),
                Width = 200,
            };
            searchControlerPanel.Controls.Add(searchBox);
            searchBox.KeyDown += new KeyEventHandler(SearchBox_Enter);
            searchButton = new Button
            {
                BackgroundImage = Image.FromFile(@"Icons/searchButton.png"),
                BackgroundImageLayout = ImageLayout.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(0,0,0,10),
                Height = 25,
            };
            searchButton.Click += SearchButton_Click;
            searchControlerPanel.Controls.Add(searchButton);

            // Only to create a small space between filterbox and typebuttons.
            Label l = new Label
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
            };
            leftPanel.Controls.Add(l);

            typePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Width = 130,
                Padding = new Padding(0),
                AutoScroll = true,
            };

            leftPanel.Controls.Add(typePanel);
            #endregion

            #region Right side table of this, holds itemPanel (menu with products).
            TableLayoutPanel rightPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0,0,0,0),
            };
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 430));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 60));
            this.Controls.Add(rightPanel);

            itemPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(0,4,0,0),
            };
            rightPanel.Controls.Add(itemPanel);
            #endregion

            #region Panel with controls, nested inside rightPanel

            descriptionPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 1, 0, 0),
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            rightPanel.Controls.Add(descriptionPanel);

            descriptionPicture = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill,
                BackgroundImage = Image.FromFile("Icons/placeholder.png"),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            descriptionPanel.Controls.Add(descriptionPicture);

            descriptionNameLabel = new Label
            {
                Text = "Items name",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(descriptionNameLabel);

            descriptionSummaryLabel = new Label
            {
                Text = "Items summary",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                FlatStyle = FlatStyle.Popup,
            };
            descriptionPanel.Controls.Add(descriptionSummaryLabel);
            #endregion

            QueryFromCSVToList();
            PopulateTypePanel(typeList);
            PopulateStore(productList);
        }

        // Method to display picture, name and summary of item in storePanel.
        private void UpdateProductView(Product tag)
        {
            descriptionPicture.ImageLocation = tag.imageLocation;
            descriptionNameLabel.Text = tag.name;
            descriptionSummaryLabel.Text = tag.summary;
        }

        // On button click inside storePanel.

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if(searchBox.Text == string.Empty)
            {
                itemPanel.Controls.Clear();
                PopulateWithTheListThatIsntNull(productList);
            }
            else
            {
                itemPanel.Controls.Clear();
                PopulateStoreByFilter(productList, searchBox.Text);
            }
        }
        private void SearchBox_Enter(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(searchBox.Text == "")
                {
                    itemPanel.Controls.Clear();
                    PopulateWithTheListThatIsntNull(productList);
                    e.SuppressKeyPress = true;
                }
                else
                {
                    itemPanel.Controls.Clear();
                    PopulateStoreByFilter(productList, searchBox.Text);
                    e.SuppressKeyPress = true;
                }
            }
        }
        private void TypeButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            searchButton.Focus();
            itemPanel.Controls.Clear();
            PopulateStoreByType(productList, b.Tag.ToString());
        }
        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            cartPanelRef.AddToCart((Product)b.Tag);
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            TableLayoutPanel productPanelRef;
            if (sender.GetType() == typeof(TableLayoutPanel))
            {
                TableLayoutPanel t = (TableLayoutPanel)sender;
                productPanelRef = (TableLayoutPanel)sender;
                UpdateProductView((Product)t.Tag);
                UpdateSelectedProduct(productPanelRef);
            }
            else if(sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                productPanelRef = (TableLayoutPanel)p.Parent;
                UpdateProductView((Product)p.Tag);
                UpdateSelectedProduct(productPanelRef);
            }
            else if(sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                productPanelRef = (TableLayoutPanel)l.Parent;
                UpdateProductView((Product)l.Tag);
                UpdateSelectedProduct(productPanelRef);
            }
        }
        private void UpdateSelectedProduct(TableLayoutPanel clickedProductPanelRef)
        {
            if (selectedProductPanel == null)
            {
                selectedProductPanel = clickedProductPanelRef;
                selectedProductPanel.BorderStyle = BorderStyle.Fixed3D;
            }

            if (selectedProductPanel != clickedProductPanelRef)
            {
                selectedProductPanel.BorderStyle = BorderStyle.None;
                selectedProductPanel = clickedProductPanelRef;
                selectedProductPanel.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        /// <summary>
        /// Function used to populate the store with products with list(objects) of products or a list of filtered products.
        /// </summary>
        /// <param name="productList"><param name="tmp">
        private void PopulateWithTheListThatIsntNull(List<Product> productList = null, List<Product> tmp = null)
        {
            if(tmp == null)
            {
                PopulateStore(productList);
            }
            else
            {
                PopulateStore(tmp);
            }
        }

        private void PopulateStore(List<Product> productList)
        {
            foreach (Product item in productList)
            {
                productPanel = new TableLayoutPanel
                {
                    ColumnCount = 4,
                    RowCount = 1,
                    Anchor = AnchorStyles.Top,
                    Height = 60,
                    Width = 400,
                    Margin = new Padding(0),
                };
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                productPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                itemPanel.Controls.Add(productPanel);

                pictureBox = new PictureBox
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Top,
                    Image = Image.FromFile(item.imageLocation),
                };
                productPanel.Controls.Add(pictureBox);

                nameLabel = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                };
                productPanel.Controls.Add(nameLabel);

                priceLabel = new Label
                {
                    Text = item.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                };
                productPanel.Controls.Add(priceLabel);

                addToCartButton = new Button
                {
                    Text = "Lägg i kundvagn",
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Popup,
                    BackColor = Color.DarkKhaki,
                    Anchor = AnchorStyles.Left,
                    Height = 50,
                    Width = 82,
                };
                productPanel.Controls.Add(addToCartButton);
                pictureBox.Click += PictureBox_Click;
                pictureBox.Tag = item;

                productPanel.Click += PictureBox_Click;
                nameLabel.Click += PictureBox_Click;
                priceLabel.Click += PictureBox_Click;
                addToCartButton.Click += AddToCartButton_Click;

                productPanel.Tag = item;
                nameLabel.Tag = item;
                priceLabel.Tag = item;
                addToCartButton.Tag = item;
            }
        }


        /// <summary>
        /// Function to populate panel with types of product from a string-list.
        /// </summary>
        /// <param name="typeList"></param>

        private void PopulateTypePanel(List<string> typeList)
        {
            foreach (var item in typeList)
            {
                typeButton = new Button
                {
                    Text = item,
                    FlatStyle = FlatStyle.Popup,
                    BackColor = Color.DarkKhaki,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 30,
                    Width = 115,
                    Margin = new Padding(0,5,10,0),
                    
                };
                typePanel.Controls.Add(typeButton);
                typeButton.Click += TypeButton_Click;
                typeButton.Tag = item;
            }
        }

        /// <summary>
        /// Queries into a 'tmp' list with certain conditions, calls function PopulateStore on 'tmp'.
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="type"></param>
        private void PopulateStoreByType(List<Product> productList, string type)
        {
            var tmp = productList.Where(x => x.type == type).ToList();
            PopulateWithTheListThatIsntNull(tmp);
        }

        /// <summary>
        /// Queries into a 'tmp' list based on conditions and calls func PopulateStore(tmp).
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="text"></param>
        private void PopulateStoreByFilter(List<Product> productList, string text)
        {
            List<Product> foo = new List<Product>();
            text = text.TrimStart().TrimEnd();
            foreach (var item in productList)
            {
                // Condition that ignores casing when searching for a match in productList.
                if(Regex.IsMatch(item.name, text, RegexOptions.IgnoreCase) == true 
                    || Regex.IsMatch(item.type, text, RegexOptions.IgnoreCase) && !foo.Contains(item))
                    foo.Add(item);
            }
            if (foo == null)
            {
                var tmp = productList.Where(x => x.name == text || x.type == text || x.price.ToString() == text).ToList();
                PopulateWithTheListThatIsntNull(tmp, null);
            }
            else
            {
                PopulateWithTheListThatIsntNull(null, foo);
            }
        }

        /// <summary>
        /// Function to collect data from CSV and store in one list(objects) of products, and one list(string) of types,
        /// the lists are later used in function "PopulateStore"(list of objects) and function "PopulateTypePanel"(list of strings).
        /// </summary>
        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).
                Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "" && x[4] != "").
                ToArray();

            for (int i = 0; i < path.Length; i++)
            {
                if (!typeList.Contains(path[i][2]))
                {
                    typeList.Add(path[i][2]);
                }
                Product tmp = new Product
                {
                    price = int.Parse(path[i][0]),
                    name = path[i][1],
                    type = path[i][2],
                    summary = path[i][3],
                    imageLocation = path[i][4],
                    nrOfProducts = 1,
                };
                productList.Add(tmp);
            }
            productList = productList.OrderBy(x => x.type).ToList();
            typeList = typeList.OrderBy(x => x).ToList();
        }
    }
}