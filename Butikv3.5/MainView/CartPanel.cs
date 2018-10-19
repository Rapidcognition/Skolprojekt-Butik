using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._5
{
    class CartItem : TableLayoutPanel
    {
        public Product product;

        public CartItem(Product toCartItem)
        {
            this.product = toCartItem;

            this.Dock = DockStyle.Top;
            this.ColumnCount = 3;
            this.Size = new Size(200, 50);
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.Margin = new Padding(0);
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));


            PictureBox picture = new PictureBox
            {
                ImageLocation = toCartItem.pictureBox.ImageLocation,
                Dock = DockStyle.Left,
                Size = new Size(42, 32),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.Fixed3D,
            };
            this.Controls.Add(picture);

            Label itemLabel = new Label
            {
                Text = toCartItem.name,
                AutoSize = true,
                Anchor = AnchorStyles.Top,
            };
            this.Controls.Add(itemLabel);

            NumericUpDown itemCount = new NumericUpDown
            {
                Dock = DockStyle.Right,
            };
            this.Controls.Add(itemCount);

        }
    };

    class CartPanel : TableLayoutPanel
    {
        private TableLayoutPanel leftPanel;
        private TableLayoutPanel middlePanel;
        private TableLayoutPanel rightPanel;
        private List<CartItem> cartItems = new List<CartItem>();

        public CartPanel()
        {
            this.Name = "Cart";
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.Margin = new Padding(0);
            //---
            this.BackColor = Color.White;
            //---
            leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                AutoScroll = true,
            };
            this.Controls.Add(leftPanel);

            Button tempAddButton = new Button
            {
                Text = "Add rice",
            };
            tempAddButton.Click += TempAddButton_Click;
            this.Controls.Add(tempAddButton, 1, 0);
        }

        private void TempAddButton_Click(object sender, EventArgs e)
        {
            Product toAdd = new Product();
            toAdd.name = "Rice";
            toAdd.pictureBox.ImageLocation = "Placeholder0.png";
            AddToCart(toAdd);
        }

        public void AddToCart(Product toCart)
        {
            CartItem temp = new CartItem(toCart);
            (temp.Controls[2] as NumericUpDown).Value = 1;

            if(cartItems.Count == 0)
            {
                cartItems.Add(temp);
                leftPanel.Controls.Add(temp);
            }
            else
            {
                foreach (CartItem item in cartItems)
                {
                    if (item.product.name == temp.product.name)
                    {
                        (item.Controls[2] as NumericUpDown).Value++;
                    }
                    else
                    {
                        cartItems.Add(temp);
                        leftPanel.Controls.Add(temp);
                    }
                }
            }
        }
    }
}
