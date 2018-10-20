using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._5
{
    class CartPanel : TableLayoutPanel
    {
        private class CartItem : TableLayoutPanel
        {
            private Timer timer;
            private Label priceLabel;
            private int price;
            public NumericUpDown itemCount;

            public CartItem(Product toAdd)
            {
                this.Name = toAdd.name;
                this.BackColor = Color.White;
                this.Size = new Size(400, 50);
                this.Margin = new Padding(1);
                this.BorderStyle = BorderStyle.FixedSingle;
                this.ColumnCount = 4;
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                this.Click += CartItem_Click;

                timer = new Timer { Interval = 100 };
                timer.Tick += Timer_Tick;

                PictureBox picture = new PictureBox()
                {
                    Size = new Size(60, 40),
                    Dock = DockStyle.Top,
                    ImageLocation = toAdd.pictureBox.ImageLocation,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = new Padding(0),
                };
                picture.Click += CartItem_Click;
                this.Controls.Add(picture);

                Label name = new Label
                {
                    Text = toAdd.name,
                    Dock = DockStyle.Left,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                name.Click += CartItem_Click;
                this.Controls.Add(name);

                itemCount = new NumericUpDown
                {
                    Value = 1,
                };
                itemCount.ValueChanged += ItemCount_ValueChanged;
                this.Controls.Add(itemCount);

                price = toAdd.price;
                priceLabel = new Label
                {
                    Text = (price + "kr"),
                    Dock = DockStyle.Left,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                this.Controls.Add(priceLabel);
            }

            private void ItemCount_ValueChanged(object sender, EventArgs e)
            {
                priceLabel.Text = (itemCount.Value * price).ToString() + "kr";
                if((sender as NumericUpDown).Value == 0)
                {
                    this.Dispose();
                }
            }
            private void Timer_Tick(object sender, EventArgs e)
            {
                this.BorderStyle = BorderStyle.FixedSingle;
                timer.Stop();
            }
            private void CartItem_Click(object sender, EventArgs e)
            {
                this.BorderStyle = BorderStyle.Fixed3D;
                timer.Start();
            }
        }
       
        private TableLayoutPanel leftPanel;
        private TableLayoutPanel rightPanel;

        public CartPanel()
        {
            this.Name = "Cart";
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.Margin = new Padding(0);

            leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                AutoScroll = true,
            };
            this.Controls.Add(leftPanel);

            // temporary add button
            Button tempAddButton = new Button
            {
                Text = "Add rice",
            };
            tempAddButton.Click += TempAddButton_Click;
            this.Controls.Add(tempAddButton, 1, 0);
            // ---
        }

        private void TempAddButton_Click(object sender, EventArgs e)
        {
            Product toAdd = new Product(10, "Rice", "food", "Good ol rice", "Placeholder0.png");
            AddToCart(toAdd);

            Product toAdd2 = new Product(7, "Apple", "food", "Good ol apple", "pictures/0.jpg");
            AddToCart(toAdd2);

            Product toAdd3 = new Product(12, "Apelsin", "food", "Good ol apelsin", "pictures/1.jpg");
            AddToCart(toAdd3);
        }

        public void AddToCart(Product toCart)
        {
            if(leftPanel.Controls.ContainsKey(toCart.name))
            {
                (leftPanel.Controls[toCart.name] as CartItem).itemCount.Value++;
            }
            else
            {
                leftPanel.Controls.Add(new CartItem(toCart));
            }
        }
    }
}
