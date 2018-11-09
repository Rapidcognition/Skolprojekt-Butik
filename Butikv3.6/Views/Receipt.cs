using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Butikv3._6
{
    class Receipt : Form
    {
        private TableLayoutPanel panelProductsPrices;
        private TableLayoutPanel panelTotlpINfo;
        private bool cashIsKing;
        public Receipt(List<Product> ppp, double d, bool refCashIsKing)
        {
            cashIsKing = refCashIsKing;

            this.Height = 500;
            this.MinimumSize = new Size(350, 500);
            this.Dock = DockStyle.Fill;
            
            #region Graphical details as to how the form should look.
            TableLayoutPanel ReceiptPanel = new TableLayoutPanel
            {
                RowCount = 3,
                Dock = DockStyle.Fill,
            };
            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            Controls.Add(ReceiptPanel);

            Label labelTime = new Label
            {
                Text = "LAGERHUS \n" +
                "Datum \t :   " + DateTime.Now,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                Margin = new Padding(0, 0, 0, 0),
            };
            ReceiptPanel.Controls.Add(labelTime);
            ReceiptPanel.SetColumnSpan(labelTime, 3);

            panelProductsPrices = new TableLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            ReceiptPanel.Controls.Add(panelProductsPrices);

            panelTotlpINfo = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Dock = DockStyle.Fill,
            };
            panelTotlpINfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panelTotlpINfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ReceiptPanel.Controls.Add(panelTotlpINfo);
            #endregion

            CreateReceipt(ppp, d);
        }

        public void CreateReceipt(List<Product> ppp, double d)
        {
            int amount = 0;
            foreach (Product item in ppp)
            {
                amount += item.nrOfProducts;

                TableLayoutPanel itemPanel = new TableLayoutPanel
                {
                    ColumnCount = 3,
                    Dock = DockStyle.Fill,
                    Height = 20,
                };
                panelProductsPrices.Controls.Add(itemPanel);

                Label labelName = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                itemPanel.Controls.Add(labelName);
                Label labelProductsAmount = new Label
                {
                    Text = item.price.ToString() + "kr",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                itemPanel.Controls.Add(labelProductsAmount);
                Label labelProductTotPrice = new Label
                {
                    Text = item.nrOfProducts.ToString() + "st",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                itemPanel.Controls.Add(labelProductTotPrice);
            }

            if(cashIsKing == false)
            {
                if (d != 0)
                {
                    Label labelMoms = new Label
                    {
                        Text = "Moms 25%: " + d * 0.25 +
                        "kr",
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = new Font("Arial", 9),
                        Dock = DockStyle.Fill,
                    };
                    panelTotlpINfo.Controls.Add(labelMoms);
                    Label labelSum = new Label
                    {
                        Text = "Antal produkter: " + amount + "st",
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = new Font("Arial", 9),
                        Dock = DockStyle.Fill,
                    };
                    panelTotlpINfo.Controls.Add(labelSum);
                }

                Label labelTotalSum = new Label
                {
                    Text = "Totala kostnaden är: " + d + "kr",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.FixedSingle,
                    Height = 40,
                    //Margin = new Padding(0, 0, 0, 0),
                };
                panelTotlpINfo.SetColumnSpan(labelTotalSum, 3);
                panelTotlpINfo.Controls.Add(labelTotalSum);

                Label labelInfo = new Label
                {
                    Text = "Behåll kvittot, 30 dagar öppet köp.",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Fill,
                    // Margin = new Padding(0, 0, 0, 0),
                };
                panelTotlpINfo.SetColumnSpan(labelInfo, 3);
                panelTotlpINfo.Controls.Add(labelInfo);
            }
            else
            {
                Label free = new Label
                {
                    Text = "Everything is free!",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                };
                panelTotlpINfo.SetColumnSpan(free, 2);
                panelTotlpINfo.Controls.Add(free);

                PictureBox p = new PictureBox
                {
                    ImageLocation = @"gimmeYourMoney.gif",
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill,
                };
                panelTotlpINfo.SetColumnSpan(p, 2);
                panelTotlpINfo.Controls.Add(p);
            }
        }
    }
}
