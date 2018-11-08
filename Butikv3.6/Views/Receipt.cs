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
        public Receipt(List<Product> ppp, double d)
        {
            this.Height = 500;
            this.MinimumSize = new Size(350, 500);
            this.Dock = DockStyle.Fill;
            
            #region Graphical details as to how the form should look.
            TableLayoutPanel ReceiptPanel = new TableLayoutPanel
            {
                RowCount = 2,
                Dock = DockStyle.Fill,
            };
            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            ReceiptPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Controls.Add(ReceiptPanel);

            panelProductsPrices = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                AutoScroll = true,
            };
            ReceiptPanel.Controls.Add(panelProductsPrices);

            panelTotlpINfo = new TableLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
            };
            ReceiptPanel.Controls.Add(panelTotlpINfo);

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
            panelProductsPrices.Controls.Add(labelTime);
            panelProductsPrices.SetColumnSpan(labelTime, 3);
            #endregion

            CreateReceipt(ppp, d);
        }

        public void CreateReceipt(List<Product> ppp, double d)
        {
            foreach (Product item in ppp)
            {
                Label labelName = new Label
                {
                    Text = item.name,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                panelProductsPrices.Controls.Add(labelName);
                Label labelProductsAmount = new Label
                {
                    Text = item.price.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                panelProductsPrices.Controls.Add(labelProductsAmount);
                Label labelProductTotPrice = new Label
                {
                    Text = item.nrOfProducts.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Top,
                };
                panelProductsPrices.Controls.Add(labelProductTotPrice);
            }

            if (d != 0)
            {
                Label labelMoms = new Label
                {
                    Text = "Moms: 25%         " + d * 0.25 +
                    "\nBehåll kvittot!",
                    TextAlign = ContentAlignment.BottomLeft,
                    Font = new Font("Arial", 9),
                    Dock = DockStyle.Bottom,
                    Height = 35,
                };
                panelProductsPrices.Controls.Add(labelMoms);
                panelProductsPrices.SetColumnSpan(labelMoms, 3);
            }

            Label labelTotalSum = new Label
            {
                Text = "Totala kostnaden är: " + d + "kr",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 9),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                //Margin = new Padding(0, 0, 0, 0),
            };
            panelTotlpINfo.Controls.Add(labelTotalSum);

            Label labelInfo = new Label
            {
                Text = "30 dagar öppet köp!",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 9),
                Dock = DockStyle.Fill,
                // Margin = new Padding(0, 0, 0, 0),
            };
            panelTotlpINfo.Controls.Add(labelInfo);
        }
    }
}
