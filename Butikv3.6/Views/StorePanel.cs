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
        FlowLayoutPanel itemPanel;
        Label nameLabel;
        Label summaryLabel;

        public StorePanel()
        {
            storePanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
            };
            storePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            storePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));

            TableLayoutPanel leftPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.Aquamarine,
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            storePanel.Controls.Add(leftPanel);

            Button searchButton = new Button
            {
                Text = "Filter",
                Dock = DockStyle.Fill,
                BackColor = Color.Orange,
            };
            leftPanel.Controls.Add(searchButton);

            TextBox searchBox = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            leftPanel.Controls.Add(searchBox);

            TableLayoutPanel rightPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Orange,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 60));
            storePanel.Controls.Add(rightPanel);

            itemPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            rightPanel.Controls.Add(itemPanel);
            TableLayoutPanel descriptionPanel = new TableLayoutPanel
            {
                RowCount = 4,
                Dock = DockStyle.Fill,
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            rightPanel.Controls.Add(descriptionPanel);

            PictureBox pictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            descriptionPanel.Controls.Add(pictureBox);

            nameLabel = new Label
            {
                Text = "Items name",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(nameLabel);

            summaryLabel = new Label
            {
                Text = "Items summary",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            descriptionPanel.Controls.Add(summaryLabel);

            Button addToCartButton = new Button
            {
                Text = "Add to cart",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Popup,
                BackColor = Color.Coral,
            };
            descriptionPanel.Controls.Add(addToCartButton);

        }

        public void PopulateStore(List<Product> temp)
        {
            foreach (var item in temp)
            {
                TableLayoutPanel tmp = new TableLayoutPanel
                {
                    ColumnCount = 3,
                    RowCount = 1,
                    Anchor = AnchorStyles.Top
                };
                tmp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                tmp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                tmp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                tmp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                itemPanel.Controls.Add(tmp);

                PictureBox thumbnail = new PictureBox
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Fill,
                };
                thumbnail = item.pictureBox;
                tmp.Controls.Add(thumbnail);

                nameLabel = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                tmp.Controls.Add(nameLabel);

                summaryLabel = new Label
                {
                    Text = item.price.ToString() + "kr",
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                tmp.Controls.Add(summaryLabel);

            }
        }
        public void PopulateStore(List<Product> temp, string filterBy)
        {

        }
        public TableLayoutPanel GetPanel()
        {
            return storePanel;
        }
    }
}
