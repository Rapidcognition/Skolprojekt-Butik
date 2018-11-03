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

        public Product GetProduct()
        {
            return this;
        }

        public string ToCSV()
        {
            return $"{price},{name},{type},{summary},{imageLocation},{nrOfProducts}";
        }

        private static Product p;
        /// <summary>
        /// Abstracted away logic for how we sort items into our lists of products.
        /// Class-method.
        /// </summary>
        /// <param name="CSVLine"></param>
        /// <returns></returns>
        public static Product FromCSV(string CSVLine)
        {
            string[] tmp = CSVLine.Split(',');
            try
            {
                p = new Product
                {
                    price = int.Parse(tmp[0]),
                    name = tmp[1],
                    type = tmp[2],
                    summary = tmp[3],
                    imageLocation = tmp[4],
                };
                if(tmp.Length <= 5)
                {
                    p.nrOfProducts = 1;
                }
                else
                {
                    p.nrOfProducts = int.Parse(tmp[5]);
                }
                return p;
            }
            catch(Exception e)
            {
                // FormatException
                MessageBox.Show(e.Message);
                Environment.Exit(1);
                return p;
            }
        }
    }

    class StorePanel : ViewPanel
    {
        #region Controls used in leftPanel and typePanel.
        TableLayoutPanel leftPanel;
        TableLayoutPanel searchPanel;
        TextBox searchBox;
        Button searchButton;
        FlowLayoutPanel PanelWithTypes;
        Button typeButton;
        #endregion
        // typeList is a database with products' types.
        private List<string> typeList = new List<string>();

        #region Controls used in CreateStorePanel.
        FlowLayoutPanel PanelWithProducts;
        TableLayoutPanel itemPanel;
        PictureBox itemPictureBox;
        Label itemNameLabel;
        Label itemPriceLabel;
        Button itemAddToCartButton;
        #endregion
        // productList is a database of products.
        private List<Product> productList = new List<Product>();
        
        /// <summary>
        /// When an item is clicked in PanelWithProducts,
        /// this variable is used as reference when the visual 
        /// appearance of the controller changes.
        /// </summary>
        private TableLayoutPanel productPanelRef;

        CartPanel cartPanelRef;
        public StorePanel(CartPanel reference)
        {
            // A downstream link to cartPanel,
            // so when "Add to cart button" is clicked,
            // the product ends up in cartPanel.
            cartPanelRef = reference;

            QueryFromCSVToList();

            CreateTypePanel();
            PopulateTypePanel(typeList);

            CreateStorePanel();
            PopulateStorePanel(productList);
        }
        /// <summary>
        /// Method to ReadAllLines from database and store in (products)list,
        /// also store all the different types in a (string)list.
        /// </summary>
        private void QueryFromCSVToList()
        {
            productList = File.ReadAllLines(@"TextFile1.csv").Select(x => Product.FromCSV(x)).
                OrderBy(x => x.name).OrderBy(x => x.type).ToList();

            typeList = productList.Select(x => x.type).Distinct().OrderBy(x => x).ToList();
        }
        /// <summary>
        /// This method is called upon when the search-function, search-button and
        /// when the typeButtons are used, to filter store.
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="text"></param>
        private void PopulateStoreByFilter(List<Product> productList, string text)
        {
            text = text.Trim();
            string rgxtext = Regex.Escape(text).Replace("\\*", ".*").Replace("\\?", ".");
            var rgxstring = new Regex(@"[A-Za-z\p{L}]");

            if(rgxstring.IsMatch(rgxtext))
            {
                var tmp = productList.
                    Where(p => Regex.IsMatch(p.name, rgxtext, RegexOptions.IgnoreCase)
                    || Regex.IsMatch(rgxtext, p.type, RegexOptions.IgnoreCase)).
                    OrderByDescending(p => Regex.IsMatch(rgxtext, p.name, RegexOptions.IgnoreCase) 
                    || Regex.IsMatch(p.type, rgxtext, RegexOptions.IgnoreCase)).ToList();

                PopulateStorePanel(tmp);
            }
            else
            {
                var tmp = productList.Where(p => p.price <= int.Parse(rgxtext)).OrderByDescending(p => p.price).ToList();
                PopulateStorePanel(tmp);
            }
        }


        #region Methods related to click events on LeftPanel.
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if(searchBox.Text == string.Empty)
            {
                PanelWithProducts.Controls.Clear();
                PopulateStorePanel(productList);
            }
            else
            {
                PanelWithProducts.Controls.Clear();
                PopulateStoreByFilter(productList, searchBox.Text);
            }
        }
        private void SearchBox_Enter(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(searchBox.Text == "")
                {
                    PanelWithProducts.Controls.Clear();
                    PopulateStorePanel(productList);
                    e.SuppressKeyPress = true;
                }
                else
                {
                    PanelWithProducts.Controls.Clear();
                    PopulateStoreByFilter(productList, searchBox.Text);
                    e.SuppressKeyPress = true;
                }
            }
        }
        private void TypeButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            searchButton.Focus();
            PanelWithProducts.Controls.Clear();
            PopulateStoreByFilter(productList, b.Tag.ToString());
        }
        #endregion
        private void CreateTypePanel()
        {
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
            this.SetRowSpan(leftPanel, 2);
            this.Controls.Add(leftPanel, 0, 0);

            searchPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Height = 15,
                Width = 55,
            };
            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));
            leftPanel.Controls.Add(searchPanel);

            searchBox = new TextBox
            {
                Anchor = AnchorStyles.Top,
                Margin = new Padding(-20, 1, -10, 0),
                Width = 200,
            };
            searchPanel.Controls.Add(searchBox);
            searchBox.KeyDown += new KeyEventHandler(SearchBox_Enter);
            searchButton = new Button
            {
                BackgroundImage = Image.FromFile(@"Icons/searchButton.png"),
                BackgroundImageLayout = ImageLayout.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 10),
                Height = 25,
            };
            searchButton.Click += SearchButton_Click;
            searchPanel.Controls.Add(searchButton);

            // Only to create a small space between filterbox and typebuttons.
            Label l = new Label
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
            };
            leftPanel.Controls.Add(l);

            PanelWithTypes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Width = 130,
                Padding = new Padding(0),
                AutoScroll = true,
            };
            leftPanel.Controls.Add(PanelWithTypes);
        }
        /// <summary>
        /// Method to populate typePanel with the difference types of products in
        /// (string)typeList.
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
                    Width = 112,
                    Margin = new Padding(0, 5, 10, 0),
                    Font = new Font("Calibri", 10, FontStyle.Bold),
                };
                PanelWithTypes.Controls.Add(typeButton);
                typeButton.Click += TypeButton_Click;
                typeButton.Tag = item;
            }
        }


        #region Methods related to click events on StorePanel.
        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            cartPanelRef.AddToCart((Product)b.Tag);
            productPanelRef = (TableLayoutPanel)b.Parent;
            UpdateSelectedProduct(productPanelRef);
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
        #endregion
        private void CreateStorePanel()
        {
            TableLayoutPanel rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 0),
            };
            this.SetRowSpan(rightPanel, 2);
            this.Controls.Add(rightPanel, 1, 0);

            PanelWithProducts = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(0, 4, 0, 0),
            };
            rightPanel.Controls.Add(PanelWithProducts);

        }
        /// <summary>
        /// Where populate store with products.
        /// </summary>
        /// <param name="productList"></param>
        private void PopulateStorePanel(List<Product> productList)
        {
            foreach (Product item in productList)
            {
                itemPanel = new TableLayoutPanel
                {
                    ColumnCount = 4,
                    RowCount = 1,
                    Anchor = AnchorStyles.Top,
                    Height = 60,
                    Width = 300,
                    Margin = new Padding(0),
                };
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                itemPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                PanelWithProducts.Controls.Add(itemPanel);

                itemPictureBox = new PictureBox
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Top,
                    Image = Image.FromFile(item.imageLocation),
                };
                itemPanel.Controls.Add(itemPictureBox);

                itemNameLabel = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 10,FontStyle.Bold),
                };
                itemPanel.Controls.Add(itemNameLabel);

                itemPriceLabel = new Label
                {
                    Text = item.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 9, FontStyle.Bold),

                };
                itemPanel.Controls.Add(itemPriceLabel);

                itemAddToCartButton = new Button
                {
                    Text = "Lägg i kundvagn",
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Popup,
                    BackColor = Color.DarkKhaki,
                    Anchor = AnchorStyles.Left,
                    Height = 50,
                    Width = 82,
                    Font = new Font("Calibri", 9, FontStyle.Bold),
                };
                itemPanel.Controls.Add(itemAddToCartButton);

                itemPanel.Click += ProductPanel_Click;
                itemPictureBox.Click += ProductPanel_Click;
                itemNameLabel.Click += ProductPanel_Click;
                itemPriceLabel.Click += ProductPanel_Click;
                itemAddToCartButton.Click += AddToCartButton_Click;

                itemPanel.Tag = item;
                itemPictureBox.Tag = item;
                itemNameLabel.Tag = item;
                itemPriceLabel.Tag = item;
                itemAddToCartButton.Tag = item;
            }
        }
    }
}