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
        public CartPanel()
        {
            this.Name = "Cart";
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.Margin = new Padding(0);
            //---
            this.BackColor = Color.White;
            //---
            AddToCart();
        }

        public void AddToCart()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Size = new Size(ClientSize.Width, 50),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0),
                ColumnCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            PictureBox picture = new PictureBox
            {
                Image = Image.FromFile("Placeholder0.png"),
                Dock = DockStyle.Left,
                Size = new Size(32,32),
                SizeMode = PictureBoxSizeMode.Zoom,
                
            };
            Label foo = new Label
            {
                Text = "Rice",
                AutoSize = true,
                Anchor = AnchorStyles.Top,
                
            };

            panel.Controls.Add(picture);
            panel.Controls.Add(foo);
            this.Controls.Add(panel);
        }
    }
}
