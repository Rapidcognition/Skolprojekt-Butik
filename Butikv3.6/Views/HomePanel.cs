using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Butikv3._6
{
    class HomePanel : TableLayoutPanel
    {
        CartPanel cartPanelRef;
        List<List<Product>> mainProductList = new List<List<Product>>();
        public HomePanel(CartPanel reference)
        {
            cartPanelRef = reference;
            #region specs
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.ColumnCount = 3;
            this.RowCount = 3;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            Label topThree = new Label
            {
                Text = "Top three types and top three items of each type",
                Font = new Font("Calibri", 13, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,

            };
            this.SetColumnSpan(topThree, 3);
            this.Controls.Add(topThree);
            #endregion

            PopulateHomePanelList();
            PopulateHomePanel();

        }
        public void PopulateHomePanelList()
        {

            List<Product> products = cartPanelRef.GetProductList().
                OrderByDescending(x => x.interestPoints).ToList();
            Product tmp = new Product();
            int outerCounter = 0;
            while(outerCounter < 3)
            {
                foreach (Product p in products)
                {
                    List<Product> popularItems = new List<Product>();
                    int innerCounter = 0;
                    if (p.stage1 == true)
                        continue;
                    else
                    {
                        foreach (Product item in products)
                        {
                            if (item.type == p.type && item.stage1 == false && innerCounter < 3 && item.stage2 == false)
                            {
                                popularItems.Add(item);
                                item.stage1 = true;
                                innerCounter++;
                            }
                            if (popularItems.Count != 3 || item.stage1 == true)
                                continue;
                            else
                                break;

                        }
                        if (popularItems.Count == 3)
                        {
                            mainProductList.Add(popularItems);
                            outerCounter++;
                        }
                        else
                            p.stage2 = true;
                    }
                }
            }

            // To set all the "stages" of our products back to false,
            // to prevent their interestPoints from being corrupted.
            foreach (Product item in products)
            {
                item.stage1 = false;
                item.stage2 = false;
                item.stage3 = false;
            }
        }

        public void PopulateHomePanel()
        {
            int counter = 0;
            foreach (List<Product> listItem in mainProductList)
            {
                Label title = new Label
                {
                    Text = listItem[counter].type,
                    Font = new Font("Calibri", 13, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                };
                this.Controls.Add(title, counter, 1);
                FlowLayoutPanel panel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    Dock = DockStyle.Fill,
                };
                this.Controls.Add(panel);
                foreach (Product item in listItem)
                {
                    TableLayoutPanel productPanel = new TableLayoutPanel
                    {
                        ColumnCount = 3,
                        Anchor = AnchorStyles.Top,
                        Height = 60,
                        Width = 300,
                        Margin = new Padding(0),
                    };
                    productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                    productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                    productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                    productPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                    panel.Controls.Add(productPanel);

                    PictureBox itemPictureBox = new PictureBox
                    {
                        BorderStyle = BorderStyle.Fixed3D,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Dock = DockStyle.Top,
                        Image = Image.FromFile(item.imageLocation),
                    };
                    productPanel.Controls.Add(itemPictureBox);
                    Label itemNameLabel = new Label
                    {
                        Text = item.name,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Anchor = AnchorStyles.Left,
                        Font = new Font("Calibri", 10, FontStyle.Bold),
                    };
                    productPanel.Controls.Add(itemNameLabel);
                    Label itemPriceLabel = new Label
                    {
                        Text = item.price + "kr",
                        TextAlign = ContentAlignment.MiddleLeft,
                        Anchor = AnchorStyles.Left,
                        Font = new Font("Calibri", 9, FontStyle.Bold),
                    };
                    productPanel.Controls.Add(itemPriceLabel);
                }
                counter++;
            }
        }
    }
}
