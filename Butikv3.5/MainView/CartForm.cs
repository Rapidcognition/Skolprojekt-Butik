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

            Label foo = new Label
            {
                Text = "Cart Form.",
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(foo);
        }
    }
}
