using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6.Views
{
    class CartPanel : TableLayoutPanel
    {

        public CartPanel()
        {
            this.ColumnCount = 3;
            this.Dock = DockStyle.Fill;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

            //this.ColumnStyles.Add;
        }
    }
}
