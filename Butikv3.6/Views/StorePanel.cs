using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Butikv3._6
{
    class StorePanel
    {
        private class Product : TableLayoutPanel
        {
            public int price;
            public string name;
            public string type;
            public string summary;
            public string imageLocation;

            private void Product_Click(object sender, EventArgs e)
            {
                MessageBox.Show(this.summary);
            }

            public Product GetProduct()
            {
                return this;
            }
        }

        TableLayoutPanel storePanel;
        TableLayoutPanel productPanel;
        FlowLayoutPanel itemPanel;
        Label nameLabel;
        Label priceLabel;

        List<Product> productList = new List<Product>();

        public StorePanel()
        {
            QueryFromCSVToList();
            storePanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
            };
            storePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            storePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));

            TableLayoutPanel leftPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.Aquamarine,
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            storePanel.Controls.Add(leftPanel);

            Button searchButton = new Button
            {
                Text = "Filter",
                Dock = DockStyle.Fill,
                BackColor = Color.Orange,
            };
            leftPanel.Controls.Add(searchButton);

            TextBox searchBox = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            leftPanel.Controls.Add(searchBox);

            TableLayoutPanel rightPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Orange,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 60));
            storePanel.Controls.Add(rightPanel);

            itemPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            rightPanel.Controls.Add(itemPanel);
            PopulateStore(productList);

            TableLayoutPanel descriptionPanel = new TableLayoutPanel
            {
                RowCount = 4,
                Dock = DockStyle.Fill,
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            rightPanel.Controls.Add(descriptionPanel);

            PictureBox pictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            descriptionPanel.Controls.Add(pictureBox);

            nameLabel = new Label
            {
                Text = "Items name",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(nameLabel);

            priceLabel = new Label
            {
                Text = "Items summary",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(priceLabel);

            Button addToCartButton = new Button
            {
                Text = "Add to cart",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Popup,
                BackColor = Color.Coral,
            };
            addToCartButton.Click += AddToCartButton_Click;
            descriptionPanel.Controls.Add(addToCartButton);
        }

        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            
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
                    Width = 410,
                };
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                PictureBox pictureBox = new PictureBox
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Top,
                    Image = Image.FromFile(item.imageLocation),
                };
                productPanel.Controls.Add(pictureBox);

                Label nameLabel = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                productPanel.Controls.Add(nameLabel);

                Label priceLabel = new Label
                {
                    Text = item.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                };
                productPanel.Controls.Add(priceLabel);

                Button addToCartButton = new Button
                {
                    Text = "Add to cart",
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Popup,
                    BackColor = Color.Coral,
                    Dock = DockStyle.Fill,
                };
                productPanel.Controls.Add(addToCartButton);
            }
        }

        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).
                Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "" && x[4] != "").
                ToArray();
            

            for(int i = 0; i < path.Length; i++)
            {
                Product tmp = new Product(int.Parse(path[i][0]), path[i][1], path[i][2], path[i][3], path[i][4]);
                
                productList.Add(tmp);

            }
        }
        
        public TableLayoutPanel GetPanel()
        {
            return storePanel;
        }
    }
}
