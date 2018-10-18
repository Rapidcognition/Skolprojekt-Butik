using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._5
{
    class StorePanel : TableLayoutPanel
    {

        public StorePanel()
        {
            this.Name = "Store";
            this.Dock = DockStyle.Fill;

            Label foo = new Label
            {
                Text = "Store Form.",
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(foo);
        }
    }
}
