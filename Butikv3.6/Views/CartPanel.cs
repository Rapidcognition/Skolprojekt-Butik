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
            TableLayoutPanel productPanel = new TableLayoutPanel
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
        public void AddToCart(/*Product product*/)
        {
            TableLayoutPanel item = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            item.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        }


        private void TempAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
