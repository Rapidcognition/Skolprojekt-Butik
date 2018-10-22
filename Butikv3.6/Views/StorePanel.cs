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

            public Product(int _price, string _name, string _type, string _summary, string _imageLocation)
            {
                price = _price;
                name = _name;
                type = _type;
                summary = _summary;
                imageLocation = _imageLocation;

                this.ColumnCount = 3;
                this.RowCount = 1;
                this.Anchor = AnchorStyles.Top;
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                PictureBox pictureBox = new PictureBox
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Fill,
                    Image = Image.FromFile(imageLocation),
                };
                this.Controls.Add(pictureBox);
                pictureBox.Click += Product_Click;

                Label nameLabel = new Label
                {
                    Text = name,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                this.Controls.Add(nameLabel);
                nameLabel.Click += Product_Click;

                Label priceLabel = new Label
                {
                    Text = price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                this.Controls.Add(priceLabel);
                priceLabel.Click += Product_Click;

                this.Click += Product_Click;
            }

            private void Product_Click(object sender, EventArgs e)
            {
                
            }

            public Product GetProduct()
            {
                return this;
            }
        }

        TableLayoutPanel storePanel;
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
                FlowDirection = FlowDirection.TopDown,
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
            foreach(Product p in productList)
            {
                itemPanel.Controls.Add(p.GetProduct());
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
