using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Butikv3._5
{
    class Product
    {
        public int ID;
        public string name;
        public string item;
        public string description;
        public PictureBox pictureBox;

        public Product()
        {
            ID = 0000;
            name = "Unknown Product";
            item = "Unknown Item";
            description = "Unknown";
            pictureBox = new PictureBox();
        }
        public Product(int ID, string name, string item, string description, PictureBox picture)
        {
            this.ID = ID;
            this.name = name;
            this.item = item;
            this.description = description;
            this.pictureBox = picture;
        }
        public Product(Product p)
        {
            ID = p.ID;
            name = p.name;
            item = p.item;
            description = p.description;
            pictureBox = p.pictureBox;
        }
    }

    class MyForm : Form
    {
        #region Main form variables
        TableLayoutPanel mainPanel;
        TableLayoutPanel topLeftSidePanel;
        TableLayoutPanel topRightSidePanel;
        TableLayoutPanel bottomLeftSidePanel;
        FlowLayoutPanel bottomLeftSideInnerPanel;
        TextBox searchBox;
        Button itemButton;
        Button homeButton;

        CartPanel cart = new CartPanel();
        StorePanel store = new StorePanel();
        #endregion

        List<Product> list = new List<Product>();
        List<string> listItem = new List<string>();


        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);

            #region Main form panels

            mainPanel = new TableLayoutPanel
            {
                Margin = new Padding(0, 0, 0, 0),
                Dock = DockStyle.Fill,
                BackColor = Color.Blue,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                RowCount = 2,
                ColumnCount = 2,
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            Controls.Add(mainPanel);
            
            mainPanel.Controls.Add(store, 1, 1);
            mainPanel.Controls.Add(cart, 1, 1);
            cart.Hide();

            topLeftSidePanel = new TableLayoutPanel
            {
                Margin = new Padding(0, 0, 0, 0),
                Dock = DockStyle.Fill,
                BackColor = Color.Bisque,
                RowCount = 2,
            };
            topLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            topLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            mainPanel.Controls.Add(topLeftSidePanel);

            Button searchLabel = new Button { Text = "Filter items.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.WhiteSmoke, FlatStyle = FlatStyle.Popup, };
            topLeftSidePanel.Controls.Add(searchLabel);
            searchBox = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            topLeftSidePanel.Controls.Add(searchBox);

            topRightSidePanel = new TableLayoutPanel
            {
                Margin = new Padding(0, 0, 0, 0),
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.BlanchedAlmond,
            };
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            mainPanel.Controls.Add(topRightSidePanel);

            Label shopTitle = new Label
            {
                Text = "[ SHOP TITLE ]",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            topRightSidePanel.Controls.Add(shopTitle);

            Button storeButton = new Button
            {
                Text = "Store",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Popup,
                BackColor = Color.LightBlue,
            };
            topRightSidePanel.Controls.Add(storeButton);
            storeButton.Click += ChangeStoreView_Click;

            Button cartButton = new Button
            {
                Text = "Cart",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Popup,
                BackColor = Color.LightBlue,
            };
            topRightSidePanel.Controls.Add(cartButton);
            cartButton.Click += ChangeStoreView_Click;

            bottomLeftSidePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = Color.Bisque,
                RowCount = 2,
            };
            bottomLeftSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            bottomLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            bottomLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100));
            mainPanel.Controls.Add(bottomLeftSidePanel);

            homeButton = new Button { Text = "Home", Dock = DockStyle.Fill, BackColor = Color.LightBlue, FlatStyle = FlatStyle.Popup, };
            bottomLeftSidePanel.Controls.Add(homeButton);
            bottomLeftSidePanel.Controls.Add(bottomLeftSideInnerPanel);

            #endregion
        }

        private void ChangeStoreView_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Store")
            {
                cart.Hide();
                store.Show();
            }
            else if ((sender as Button).Text == "Cart")
            {
                store.Hide();
                cart.Show();
            }
        }

        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "").ToArray();

            foreach (var item in path) //Läs in olika genre i lista
            {
                if (!listItem.Contains(item[2]))
                    listItem.Add(item[2]);
            }
            foreach (var item in listItem)
            {
                itemButton = new Button
                {
                    Text = item
                };
                itemButton.Click += ItemButton_Click;
                bottomLeftSideInnerPanel.Controls.Add(itemButton);
                // Implementera vart inlästa knappar ska hamna i controls och gör clickevent.
            
            } // Slut på dynamiska genre-inläsningen

            for (int i = 0; i < path.Length; i++)
            {
                Product tmp = new Product
                {
                    ID = int.Parse(path[i][0]),
                    name = path[i][1],
                    item = path[i][2],
                    description = path[i][3],

                    // Fix pictureBox imageLocation.
                    pictureBox = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Height = 150,
                        Width = 150,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        ImageLocation = @"Movies\" + i + ".jpg"
                    }
                };
                list.Add(tmp);
            }
        }
        
        // Funktion för att filtera produkter baserat på vilken knapp som klickas ned.
        private void ItemButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QueryFromListToMainPanel()
        {

        }
    }
}
