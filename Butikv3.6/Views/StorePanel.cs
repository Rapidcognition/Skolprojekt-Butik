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

        public int interestPoints;
        public bool stage1;
        public bool stage2;
        public bool stage3;

        public string ToCSV()
        {
            return $"{interestPoints},{price},{name},{type},{summary},{imageLocation},{nrOfProducts}";
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
                    interestPoints = int.Parse(tmp[0]),
                    price = int.Parse(tmp[1]),
                    name = tmp[2],
                    type = tmp[3],
                    summary = tmp[4],
                    imageLocation = tmp[5],
                };
                if(tmp.Length <= 6)
                {
                    p.nrOfProducts = 1;
                }
                else
                {
                    p.nrOfProducts = int.Parse(tmp[6]);
                }
                return p;
            }
            catch(Exception e)
            {
                // FormatException
                MessageBox.Show("Problem med CSV-filens format.\n" + e.Message);
                Environment.Exit(1);
                return p;
            }
        }
    }

    class StorePanel : ViewPanel
    {
        TextBox searchBox;
        Button searchButton;
        FlowLayoutPanel PanelWithTypes;
        // typeList is a database with every type of product.
        private List<string> typeList = new List<string>();

        // Database of products.
        // Where we display our collection of products
        FlowLayoutPanel PanelWithProducts;

        /// <summary>
        /// When an item is clicked in PanelWithProducts,
        /// this variable is used as reference when the visual 
        /// appearance of the controller changes.
        /// </summary>
        private TableLayoutPanel productPanelRef;

        CartPanel cartPanelRef;
        public StorePanel(CartPanel reference)
        {
            cartPanelRef = reference;

            CreateTypeList();
            CreateTypePanel();
            PopulateTypePanel(typeList);

            CreateStorePanel();
            //TODO: implementera metod i cartPanel för GetProductList().
            PopulateStorePanel(cartPanelRef.GetProductList());
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
            // A-Za-z for all english letters, \p{L} is for "non" english letters.
            // \x20 is hexadecimal for space.
            // So any trace of aforementioned will trigger the first condition.
            var rgxstring = new Regex(@"[A-Z a-z \p{L} \x20]");

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
        private void CreateTypeList()
        {
            List<Product> productList = cartPanelRef.GetProductList();
            typeList = productList.Select(x => x.type).Distinct().OrderBy(x => x).ToList();
        }

        #region Methods related to click events on LeftPanel.
        private void SearchButton_Click(object sender, EventArgs e)
        {
            var productList = cartPanelRef.GetProductList();
            if (searchBox.Text == string.Empty)
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
            var productList = cartPanelRef.GetProductList();
            if (e.KeyCode == Keys.Enter)
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
            var productList = cartPanelRef.GetProductList();
            Button b = (Button)sender;
            searchButton.Focus();
            PanelWithProducts.Controls.Clear();
            PopulateStoreByFilter(productList, b.Tag.ToString());
        }
        #endregion
        private void CreateTypePanel()
        {
            TableLayoutPanel leftPanel = new TableLayoutPanel
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

            TableLayoutPanel searchPanel = new TableLayoutPanel
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
                Button typeButton = new Button
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

            Product productRef = (Product)b.Tag;
            productRef.stage1 = true;
            productRef.stage2 = true;
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
                productRef.stage1 = true;
            }
            else if(sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                productPanelRef = (TableLayoutPanel)p.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
                UpdateSelectedProduct(productPanelRef);
                productRef.stage1 = true;
            }
            else if(sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                productPanelRef = (TableLayoutPanel)l.Parent;
                productRef = (Product)productPanelRef.Tag;
                UpdateDescriptionPanel(descriptionPanelRef, productRef);
                UpdateSelectedProduct(productPanelRef);
                productRef.stage1 = true;
            }
        }
        #endregion
        private void CreateStorePanel()
        {
            TableLayoutPanel MiddlePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 0),
            };
            this.SetRowSpan(MiddlePanel, 2);
            this.Controls.Add(MiddlePanel, 1, 0);

            PanelWithProducts = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(0, 4, 0, 0),
            };
            MiddlePanel.Controls.Add(PanelWithProducts);

        }
        /// <summary>
        /// Where populate store with products.
        /// </summary>
        /// <param name="productList"></param>
        private void PopulateStorePanel(List<Product> productList)
        {
            foreach (Product item in productList)
            {
                TableLayoutPanel itemPanel = new TableLayoutPanel
                {
                    ColumnCount = 4,
                    RowCount = 1,
                    Anchor = AnchorStyles.Top,
                    Height = 60,
                    Width = 300,
                    Margin = new Padding(0),
                };
                #region itemPanel ColumnStyles
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                itemPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                itemPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                #endregion
                PanelWithProducts.Controls.Add(itemPanel);
                itemPanel.Click += ProductPanel_Click;
                itemPanel.Tag = item;

                PictureBox itemPictureBox = new PictureBox
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Top,
                    Image = Image.FromFile(item.imageLocation),
                };
                itemPanel.Controls.Add(itemPictureBox);
                itemPictureBox.Click += ProductPanel_Click;
                itemPictureBox.Tag = item;

                Label itemNameLabel = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 10,FontStyle.Bold),
                };
                itemPanel.Controls.Add(itemNameLabel);
                itemNameLabel.Click += ProductPanel_Click;
                itemNameLabel.Tag = item;

                Label itemPriceLabel = new Label
                {
                    Text = item.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 9, FontStyle.Bold),

                };
                itemPanel.Controls.Add(itemPriceLabel);
                itemPriceLabel.Click += ProductPanel_Click;
                itemPriceLabel.Tag = item;

                Button itemAddToCartButton = new Button
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
                itemAddToCartButton.Click += AddToCartButton_Click;
                itemAddToCartButton.Tag = item;
            }
        }
    }
}