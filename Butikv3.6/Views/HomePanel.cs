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
        private List<List<Product>> mainProductList = new List<List<Product>>();
        private CartPanel cartPanelRef;
        private StorePanel storePanelRef;

        public HomePanel(CartPanel cartRef, StorePanel storeRef)
        {
            cartPanelRef = cartRef;
            storePanelRef = storeRef;
            
            // Create this.tablelayoutpanel with three rows and three columns.
            #region HomePanels details about rows, columns etc.
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.ColumnCount = 3;
            this.RowCount = 3;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            Label topThree = new Label
            {
                Text = "Topp tre kategorier och deras populäraste produkter.",
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
        /// <summary>
        /// Populate homePanelList with the most popular
        /// items, afterwards we call BubbelSort to put them in 
        /// the right order.
        /// </summary>
        public void PopulateHomePanelList()
        {
            foreach(Product p in cartPanelRef.GetProductList().OrderByDescending(x => x.interestPoints).ToList())
            {
                Product tp = new Product();
                List<Product> popularItems = new List<Product>();
                int innerCounter = 0;
                foreach (Product item in cartPanelRef.GetProductList())
                {
                    if (item.type == p.type && item.stage1 == false
                        && innerCounter < 3 && item.stage2 == false)
                    {
                        popularItems.Add(item);
                        item.stage1 = true;
                        innerCounter++;
                        p.totalPoints += item.interestPoints;
                    }
                    else if (popularItems.Count != 3 || item.stage1 == true)
                        continue;
                }
                if (popularItems.Count == 3)
                {
                    int foo = CalculateTotalInterestPoints(popularItems);
                    popularItems[0].totalPoints = foo;

                    mainProductList.Add(popularItems);
                }
                else
                {
                    p.stage2 = true;
                }
            }

            BubbleSort(mainProductList);

            // To set all the "stages" of our products back to false,
            // to prevent their interestPoints from being corrupted.
            foreach (Product item in cartPanelRef.GetProductList())
            {
                item.stage1 = false;
                item.stage2 = false;
                item.stage3 = false;
            }
        }

        /// <summary>
        /// Fill our homePanel with the most popular categories,
        /// and their three most popular items.
        /// </summary>
        public void PopulateHomePanel()
        {
            int counter = 0;
            foreach (List<Product> listItem in mainProductList)
            {
                if (counter > 2)
                    break;
                else
                {
                    Label title = new Label
                    {
                        Dock = DockStyle.Fill,
                        Text = (counter+1) + ": " + listItem[counter].type,
                        Font = new Font("Calibri", 14, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleLeft,
                    };
                    this.Controls.Add(title, counter, 1);
                    FlowLayoutPanel panel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        Dock = DockStyle.Fill,
                        Width = 320,
                    };
                    this.Controls.Add(panel);
                    foreach (Product item in listItem)
                    {
                        TableLayoutPanel productPanel = new TableLayoutPanel
                        {
                            ColumnCount = 3,
                            RowCount = 2,
                            Anchor = AnchorStyles.Top,
                            Height = 70,
                            Width = 300,
                            Margin = new Padding(0),
                        };
                        productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                        productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                        productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37));
                        productPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                        productPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                        panel.Controls.Add(productPanel);
                        productPanel.Click += ProductPanel_Click;
                        productPanel.Tag = item;

                        PictureBox itemPictureBox = new PictureBox
                        {
                            BorderStyle = BorderStyle.Fixed3D,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Dock = DockStyle.Fill,
                            Image = Image.FromFile(item.imageLocation),
                        };
                        productPanel.SetRowSpan(itemPictureBox, 2);
                        productPanel.Controls.Add(itemPictureBox);
                        itemPictureBox.Click += ProductPanel_Click;
                        itemPictureBox.Tag = item;

                        Label itemNameLabel = new Label
                        {
                            Text = item.name,
                            TextAlign = ContentAlignment.TopLeft,
                            Anchor = AnchorStyles.Top,
                            Font = new Font("Calibri", 12, FontStyle.Bold),
                        };
                        productPanel.Controls.Add(itemNameLabel);
                        itemNameLabel.Click += ProductPanel_Click;
                        itemNameLabel.Tag = item;

                        Label itemPriceLabel = new Label
                        {
                            Text = item.price + "kr",
                            TextAlign = ContentAlignment.TopLeft,
                            Anchor = AnchorStyles.Top,
                            Font = new Font("Calibri", 9, FontStyle.Bold),
                        };
                        productPanel.Controls.Add(itemPriceLabel, 1,1);
                        itemPriceLabel.Click += ProductPanel_Click;
                        itemPriceLabel.Tag = item;

                        Label itemPointLabel = new Label
                        {
                            Text = "Intresse-\npoäng:\n" + item.interestPoints,
                            TextAlign = ContentAlignment.TopLeft,
                            Dock = DockStyle.Fill,
                            Font = new Font("Calibri", 10, FontStyle.Italic),
                        };
                        productPanel.SetRowSpan(itemPointLabel, 2);
                        productPanel.Controls.Add(itemPointLabel);
                        itemPointLabel.Click += ProductPanel_Click;
                        itemPointLabel.Tag = item;
                    }
                    counter++;
                }
            }
        }

        /// <summary>
        /// On Product_Panel.Click display the selected item in StorePanels descriptionPanel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductPanel_Click(object sender, EventArgs e)
        {
            TableLayoutPanel productPanelRef;
            Product productRef;

            if (sender.GetType() == typeof(TableLayoutPanel))
            {
                productPanelRef = (TableLayoutPanel)sender;
                productRef = (Product)productPanelRef.Tag;
                this.Hide();
                storePanelRef.Show();
                storePanelRef.OnClickedHomePanelProduct(productPanelRef, productRef);
            }
            else if (sender.GetType() == typeof(PictureBox))
            {
                PictureBox p = (PictureBox)sender;
                productPanelRef = (TableLayoutPanel)p.Parent;
                productRef = (Product)productPanelRef.Tag;
                this.Hide();
                storePanelRef.Show();
                storePanelRef.OnClickedHomePanelProduct(productPanelRef, productRef);
            }
            else if (sender.GetType() == typeof(Label))
            {
                Label l = (Label)sender;
                productPanelRef = (TableLayoutPanel)l.Parent;
                productRef = (Product)productPanelRef.Tag;
                this.Hide();
                storePanelRef.Show();
                storePanelRef.OnClickedHomePanelProduct(productPanelRef, productRef);
            }
        }

        /// <summary>
        /// Logic behind which categories and respective products that
        /// we display in HomePanel.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int CalculateTotalInterestPoints(List<Product> list)
        {
            int sum = 0;
            sum = list.Select(x => x.interestPoints).Sum();
            return sum;
        }
        /// <summary>
        /// Logic for sorting the categories based on the top three products'
        /// combined interestPoints.
        /// </summary>
        /// <param name="mainProductList"></param>
        public void BubbleSort(List<List<Product>> mainProductList)
        {
            for (int i = 0; i < mainProductList.Count - 1; i++)
            {
                for (int k = 0; k < mainProductList.Count - 1; k++)
                {
                    if (mainProductList[k][0].totalPoints < mainProductList[k + 1][0].totalPoints)
                    {
                        var tmp = mainProductList[k + 1];
                        mainProductList[k + 1] = mainProductList[k];
                        mainProductList[k] = tmp;
                    }
                }
            }
        }
    }
}