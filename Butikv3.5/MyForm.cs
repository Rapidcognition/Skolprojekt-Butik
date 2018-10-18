using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._5
{
    class MyForm : Form
    {
        #region Main form variables
        TableLayoutPanel mainPanel;
        TableLayoutPanel topLeftSidePanel;
        TableLayoutPanel topRightSidePanel;
        TextBox searchBox;
        #endregion

        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);

            #region Main form panels

            mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Blue,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                RowCount = 2,
                ColumnCount = 2,
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            topLeftSidePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Bisque,
                RowCount = 2,
            };
            topLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            topLeftSidePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

            topRightSidePanel = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.BlanchedAlmond,
            };
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            topRightSidePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            Controls.Add(mainPanel);
            mainPanel.Controls.Add(topLeftSidePanel);
            mainPanel.Controls.Add(topRightSidePanel);
            Label searchLabel = new Label { Text = "Filter items.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, };
            searchBox = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            topLeftSidePanel.Controls.Add(searchLabel);
            topLeftSidePanel.Controls.Add(searchBox);
            #endregion


        }
    }
}
