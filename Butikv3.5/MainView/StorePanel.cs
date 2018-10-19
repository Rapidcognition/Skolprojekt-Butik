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
        FlowLayoutPanel storePanel;

        public StorePanel()
        {
            this.Name = "Store";
            this.Dock = DockStyle.Fill;

            storePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Blue,
                FlowDirection = FlowDirection.LeftToRight,
            };
            Controls.Add(storePanel);

            TableLayoutPanel temp = new TableLayoutPanel
            {
                BackColor = Color.Orange,
                Height = 50,
                Width = 300,
            };
            Label foo = new Label
            {
                Text = "Store Form.",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Orange,
                Height = 50,
                Width = 300,
            };
            storePanel.Controls.Add(foo);
        }
    }
}
