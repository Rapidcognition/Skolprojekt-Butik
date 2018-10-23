using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class CartPanel : TableLayoutPanel
    {
        TableLayoutPanel productPanel;

        public CartPanel()
        {
            this.ColumnCount = 3;
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            #region left menu
            TableLayoutPanel leftMenuPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            this.Controls.Add(leftMenuPanel);

            Button checkoutButton = new Button
            {
                Text = "Checkout",
                Dock = DockStyle.Top,
            };
            leftMenuPanel.Controls.Add(checkoutButton);

            Button saveCartButton = new Button
            {
                Text = "Save Cart",
                Dock = DockStyle.Top,
            };
            leftMenuPanel.Controls.Add(saveCartButton);
            #endregion

            #region Product panel
            productPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            this.Controls.Add(productPanel);
            #endregion

            #region Description panel
            TableLayoutPanel descriptionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            this.Controls.Add(descriptionPanel);

            Button tempAdd = new Button
            {
                Text = "Add to cart",
                Dock = DockStyle.Top,
            };
            tempAdd.Click += TempAdd_Click;
            descriptionPanel.Controls.Add(tempAdd);
            #endregion

        }
        public void AddToCart(Product product)
        {
            TableLayoutPanel item = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            item.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            item.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            item.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            productPanel.Controls.Add(item);

            PictureBox productPicture = new PictureBox
            {
                ImageLocation = product.imageLocation,
                Dock = DockStyle.Fill,
                BackgroundImageLayout = ImageLayout.Zoom,
            };
            item.Controls.Add(productPicture);

            Label productName = new Label
            {
                Text = product.name,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };


        }


        private void TempAdd_Click(object sender, EventArgs e)
        {
            Product product = new Product
            {
                name = "Äpple",
                imageLocation = "pictures/0.jpg",
                price = 5,
                type = "Fruit",
                summary = "Smaskig frukt"
            };
            AddToCart(product);
        }
    }
}
