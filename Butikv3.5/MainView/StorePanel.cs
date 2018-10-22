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
        TableLayoutPanel descriptionPanel;
        PictureBox descriptionPicture;
        Label nameLabel;
        Label descriptionLabel;
        Button purchaseItem;

        TableLayoutPanel itemPanel;
        PictureBox storePic;

        TableLayoutPanel temp;
        List<Product> storeList = new List<Product>();
        public StorePanel()
        {
            this.Name = "Store";
            this.Dock = DockStyle.Fill;
            this.ColumnCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            itemPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            this.Controls.Add(itemPanel);

            descriptionPanel = new TableLayoutPanel
            {
                RowCount = 4,
                Dock = DockStyle.Fill,
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            this.Controls.Add(descriptionPanel);

            descriptionPicture = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.Fixed3D, };
            descriptionPanel.Controls.Add(descriptionPicture);

            nameLabel = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Popup, BackColor = Color.WhiteSmoke };
            descriptionPanel.Controls.Add(nameLabel);

            descriptionLabel = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Popup, BackColor = Color.WhiteSmoke };
            descriptionPanel.Controls.Add(descriptionLabel);

            purchaseItem = new Button { Text = "Add to cart", FlatStyle = FlatStyle.Popup, TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill, };
            descriptionPanel.Controls.Add(purchaseItem);
        }

        public void AddItemToStorePanel(List<Product> l)
        {
            foreach (var item in l)
            {
                this.storeList.Add(item);
            }
            foreach (var item in storeList)
            {
                itemPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

                temp = new TableLayoutPanel
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Top,
                    ColumnCount = 3,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                };
                temp.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
                temp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

                itemPanel.Controls.Add(temp);

                storePic = new PictureBox
                {
                    ImageLocation = item.pictureBox.ImageLocation,
                    Anchor = AnchorStyles.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Margin = new Padding(),
                };
                storePic = item.pictureBox;
                temp.Controls.Add(storePic);

                Label b = new Label { Text = item.name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, };
                temp.Controls.Add(b);

                Label p = new Label { Text = item.price.ToString() + "kr", Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft, };
                temp.Controls.Add(p);

                storePic.MouseClick += StorePic_Click;
                storePic.Name = item.name;
                storePic.Tag = item.description;
            }
        }

        private void StorePic_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            descriptionPicture.Image = p.Image;
            nameLabel.Text = p.Name;
            descriptionLabel.Text = p.Tag.ToString();
        }
    }
}
