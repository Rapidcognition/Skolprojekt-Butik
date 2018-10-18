using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3
{
    class FormForCart : Form
    {
        public FormForCart()
        {
            TableLayoutPanel paymentPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.Crimson,
                RowCount = 2,
                ColumnCount = 2,
            };
            paymentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            paymentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            paymentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            paymentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            Controls.Add(paymentPanel);

            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Aqua,
                ColumnCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble,
            };
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            paymentPanel.Controls.Add(topPanel);
            paymentPanel.SetColumnSpan(topPanel, 2);


            TableLayoutPanel itemsInCart = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.BlueViolet,
            };
            paymentPanel.Controls.Add(itemsInCart);

            TableLayoutPanel itemDescriptionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                BackColor = Color.Goldenrod,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            paymentPanel.Controls.Add(itemDescriptionPanel);
            this.MinimumSize = new System.Drawing.Size(500, 500);
        }
    }
    class MyForm : Form
    {
        TableLayoutPanel mainPanel;
        TableLayoutPanel topPanel;
        FlowLayoutPanel bottomPanel;
        Button cart;
        TableLayoutPanel itemPanel;

        public MyForm()
        {
            #region Main panel and two sub panels

            mainPanel = new TableLayoutPanel
            {
                RowCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.AliceBlue,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble,
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));

            topPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Orange,
            };
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            bottomPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
            };
            Controls.Add(mainPanel);
            mainPanel.Controls.Add(topPanel);
            mainPanel.Controls.Add(bottomPanel);
            #endregion

            #region Top panel controls

            Label store = new Label
            {
                Text = "Store",
                Dock = DockStyle.Fill
            };
            cart = new Button
            {
                Text = "Cart",
                Dock = DockStyle.Fill,
                BackColor = Color.MediumVioletRed,
            };
            cart.Click += Cart_Click;
            topPanel.Controls.Add(store);
            topPanel.Controls.Add(cart);
            
            #endregion

            itemPanel = new TableLayoutPanel
            {
                BackColor = Color.Brown,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            bottomPanel.Controls.Add(itemPanel);

        }

        private void Cart_Click(object sender, EventArgs e)
        {
            FormForCart ffc = new FormForCart();
            ffc.Show();
            this.Hide();
        }
    }
}
