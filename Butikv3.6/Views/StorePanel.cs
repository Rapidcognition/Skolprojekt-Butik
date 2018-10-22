using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class StorePanel
    {
        TableLayoutPanel storePanel;
        public StorePanel()
        {
            storePanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
            };
            storePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            storePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            TableLayoutPanel leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Aquamarine,
            };
            storePanel.Controls.Add(leftPanel);

            
        }

        

        public TableLayoutPanel GetPanel()
        {
            return storePanel;
        }
    }
}
