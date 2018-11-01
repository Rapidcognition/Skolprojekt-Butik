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

        public static Product FromCSV(string CSVLine)
        {
            string[] tmp = CSVLine.Split(',');
            Product p = new Product
            {
                price = int.Parse(tmp[0]),
                name = tmp[1],
                type = tmp[2],
                summary = tmp[3],
                imageLocation = tmp[4],
                nrOfProducts = 1,
            };
            return p;
        }
    }

    class StorePanel : ViewPanel
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
        
        // Used when productpanel.Click to display
        // a graphical change on clicked productPanel.
        private TableLayoutPanel productPanelRef;

        // Lists that contains our products and the type of our products
        // when we populate typePanel and PopulateStore.
        private List<Product> productList = new List<Product>();
        private List<string> typeList = new List<string>();

        public StorePanel(CartPanel reference)
        {
            // So we always point to the same memory place when we add items to cartPanel.
            cartPanelRef = reference;

            LeftPanel();
            MiddlePanel();
            QueryFromCSVToList();
            PopulateTypePanel(typeList);
            PopulateStore(productList);
        }

        #region Methods related to click events in LeftPanel.
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if(searchBox.Text == string.Empty)
            {
                itemPanel.Controls.Clear();
                PopulateStore(productList);
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
                    PopulateStore(productList);
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
            PopulateStoreByFilter(productList, b.Tag.ToString());
        }
        #endregion
        
        /// <summary>
        /// Abstracted away logic for left-side panel for readability.
        /// </summary>
        private void LeftPanel()
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

            searchControlerPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Height = 15,
                Width = 55,
            };
            searchControlerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            searchControlerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));
            leftPanel.Controls.Add(searchControlerPanel);

            searchBox = new TextBox
            {
                Anchor = AnchorStyles.Top,
                Margin = new Padding(-20, 1, -10, 0),
                Width = 200,
            };
            searchControlerPanel.Controls.Add(searchBox);
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
        }


        #region Methods related to click events on MiddlePanel.
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
        
        /// <summary>
        /// Abstracted away logic for the right-side panel for readability.
        /// </summary>
        private void MiddlePanel()
        {
            TableLayoutPanel rightPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 0),
            };
            this.SetRowSpan(rightPanel, 2);
            this.Controls.Add(rightPanel, 1, 0);

            itemPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(0, 4, 0, 0),
            };
            rightPanel.Controls.Add(itemPanel);

        }


        /// <summary>
        /// Single method to deal with the logic as to how we display 
        /// products in productPanel store.
        /// </summary>
        /// <param name="productList"></param>
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
                    Width = 300,
                    Margin = new Padding(0),
                };
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
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
                    Font = new Font("Calibri", 10,FontStyle.Bold),
                };
                productPanel.Controls.Add(nameLabel);

                priceLabel = new Label
                {
                    Text = item.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Left,
                    Font = new Font("Calibri", 9, FontStyle.Bold),

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
                    Font = new Font("Calibri", 9, FontStyle.Bold),
                };
                productPanel.Controls.Add(addToCartButton);
                pictureBox.Click += ProductPanel_Click;
                pictureBox.Tag = item;

                productPanel.Click += ProductPanel_Click;
                nameLabel.Click += ProductPanel_Click;
                priceLabel.Click += ProductPanel_Click;
                addToCartButton.Click += AddToCartButton_Click;

                productPanel.Tag = item;
                nameLabel.Tag = item;
                priceLabel.Tag = item;
                addToCartButton.Tag = item;
            }
        }

        /// <summary>
        /// This method is called upon when the search-function is used.
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

                PopulateStore(tmp);
            }
            else
            {
                var tmp = productList.Where(p => p.price <= int.Parse(rgxtext)).OrderByDescending(p => p.price).ToList();
                PopulateStore(tmp);
            }
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
                    Margin = new Padding(0,5,10,0),
                    Font = new Font("Calibri", 10, FontStyle.Bold),
                };
                typePanel.Controls.Add(typeButton);
                typeButton.Click += TypeButton_Click;
                typeButton.Tag = item;
            }
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
    }
}