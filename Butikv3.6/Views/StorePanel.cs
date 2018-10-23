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
    class Product
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

    class StorePanel : TableLayoutPanel
    {
        public string SelectedProduct { get; set; }

        // CartPanel 
        CartPanel cartPanelRef;

        TableLayoutPanel leftPanel;
        //TableLayoutPanel storePanel;
        TableLayoutPanel productPanel;

        // Controls connected to description panel
        PictureBox descriptionPicture;
        Label descriptionNameLabel;
        Label descriptionSummaryLabel;
        TableLayoutPanel descriptionPanel;

        //This label is used in productPanel and descriptionPanel.
        Label nameLabel;
        Label priceLabel;
        PictureBox pictureBox;
        Button addToCartButton;
        // The four controls listed above is in itemPanel when it's added to storePanel,
        // in function PopulateStore.
        FlowLayoutPanel itemPanel;
        List<Product> productList = new List<Product>();

        public StorePanel(CartPanel reference)
        {
            cartPanelRef = reference;

            this.ColumnCount = 2;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Azure;
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));

            leftPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.Aquamarine,
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            this.Controls.Add(leftPanel);

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
            this.Controls.Add(rightPanel);

            itemPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            rightPanel.Controls.Add(itemPanel);
            PopulateStore(productList);

            descriptionPanel = new TableLayoutPanel
            {
                RowCount = 4,
                Dock = DockStyle.Fill,
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            rightPanel.Controls.Add(descriptionPanel);

            descriptionPicture = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill,
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
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(descriptionSummaryLabel);

            addToCartButton = new Button
            {
                Text = "Add to cart",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Popup,
                BackColor = Color.Coral,
            };
            addToCartButton.Click += AddToCartButton_Click;
            descriptionPanel.Controls.Add(addToCartButton);

            QueryFromCSVToList();

            PopulateStore(productList);
        }

        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
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
                };
                productPanel.Controls.Add(nameLabel);

                Label priceLabel = new Label
                {
                    Text = item.price + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                };
                productPanel.Controls.Add(priceLabel);

                addToCartButton = new Button
                {
                    Text = "Add to cart",
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Popup,
                    BackColor = Color.Coral,
                    Dock = DockStyle.Fill,
                };
                productPanel.Controls.Add(addToCartButton);
                pictureBox.Click += PictureBox_Click;
                pictureBox.Tag = item;

                //
                productPanel.Click += PictureBox_Click;
                nameLabel.Click += PictureBox_Click;
                priceLabel.Click += PictureBox_Click;

                productPanel.Tag = item;
                nameLabel.Tag = item;
                priceLabel.Tag = item;
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(TableLayoutPanel))
            {
                TableLayoutPanel t = (TableLayoutPanel)sender;
                UpdateProductView((Product)t.Tag);
            }
            else if(sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                UpdateProductView((Product)p.Tag);
            }
            else if(sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                UpdateProductView((Product)l.Tag);
            }
        }

        private void UpdateProductView(Product tag)
        {
            descriptionPicture.ImageLocation = tag.imageLocation;
            descriptionNameLabel.Text = tag.name;
            descriptionSummaryLabel.Text = tag.summary;
        }

        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).
                Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "" && x[4] != "").
                ToArray();

            for (int i = 0; i < path.Length; i++)
            {
                Product tmp = new Product
                {
                    price = int.Parse(path[i][0]),
                    name = path[i][1],
                    type = path[i][2],
                    summary = path[i][3],
                    imageLocation = path[i][4],
                };
                productList.Add(tmp);
            }
        }
    }
}
