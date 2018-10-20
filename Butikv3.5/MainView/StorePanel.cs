﻿using System;
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
        TableLayoutPanel temp;
        PictureBox storePic;

        List<Product> storeList = new List<Product>();
        public StorePanel()
        {
            this.Name = "Store";
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.AutoScroll = true;
        }

        public void AddItemToStorePanel(List<Product> l)
        {
            foreach (var item in l)
            {
                this.storeList.Add(item);
            }
            foreach (var item in storeList)
            {
                this.RowCount++;
                this.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

                temp = new TableLayoutPanel
                {
                    BackColor = Color.LightCyan,
                    Dock = DockStyle.Top,
                    ColumnCount = 3,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                };
                temp.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
                this.Controls.Add(temp);

                storePic = new PictureBox
                {
                    ImageLocation = item.pictureBox.ImageLocation,
                    Anchor = AnchorStyles.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Margin = new Padding(),
                };
                temp.Controls.Add(storePic);

                Label b = new Label { Text = item.name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, };
                temp.Controls.Add(b);

                Label p = new Label { Text = item.price.ToString() + "kr", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, };
                temp.Controls.Add(p);
            }
        }
    }
}
