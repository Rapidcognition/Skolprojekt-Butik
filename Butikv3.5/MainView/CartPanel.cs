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
        public CartItem(Product toAdd)
        {
            this.Size = new Size(180, 50);
            this.Margin = new Padding(1);
            this.ColumnCount = 3;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.MouseEnter += CartItem_MouseEnter;
            this.MouseLeave += CartItem_MouseLeave;

            PictureBox picture = new PictureBox()
            {
                Size = new Size(60, 40),
                Anchor = AnchorStyles.Left,
                ImageLocation = toAdd.pictureBox.ImageLocation,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(0),
            };
            this.Controls.Add(picture);

            Label name = new Label
            {
                Text = toAdd.name,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            this.Controls.Add(name);

            NumericUpDown itemCount = new NumericUpDown
            {
                Dock = DockStyle.Top,
            };
            this.Controls.Add(itemCount);

        }
        private void CartItem_MouseEnter(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.Fixed3D;
        }
        private void CartItem_MouseLeave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }

    class CartPanel : TableLayoutPanel
    {
        private TableLayoutPanel leftPanel;
        private TableLayoutPanel middlePanel;
        private TableLayoutPanel rightPanel;

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

            Panel panel = new Panel
            {
                Bounds = this.Bounds,
                BorderStyle = BorderStyle.FixedSingle,
            };
            
            
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
            Product toAdd = new Product(0000,"rice","food","Good ol rice", "Placeholder0.png");
            toAdd.name = "Rice";
            toAdd.pictureBox.ImageLocation = "Placeholder0.png";
            AddToCart(toAdd);
        }

        public void AddToCart(Product toCart)
        {
            CartItem newCartItem = new CartItem(toCart);
            leftPanel.Controls.Add(newCartItem);
            
        }
    }
}
