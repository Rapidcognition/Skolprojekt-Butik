using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class RecieptForm : Form
    {
        private Point mouseLocation;
        FlowLayoutPanel itemPanel;

        public RecieptForm()
        {
            this.Dispose();
        }

        public RecieptForm(List<Product> products)
        {
            this.Font = new Font("Calibri", 11);
            this.Size = new Size(300, 500);
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            mouseLocation = Point.Empty;

            TableLayoutPanel recieptPanel = new TableLayoutPanel
            {
                RowCount = 3,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                BorderStyle = BorderStyle.FixedSingle,
            };
            recieptPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            recieptPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            recieptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            recieptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            recieptPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 5));

            this.Controls.Add(recieptPanel);

            PictureBox recieptPicture = new PictureBox
            {
                ImageLocation = "Background/Header1.png",
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            recieptPicture.MouseDown += RecieptPicture_MouseDown;
            recieptPicture.MouseMove += new MouseEventHandler(RecieptPicture_Move);
            recieptPicture.MouseUp += RecieptPicture_MouseUp;
            recieptPanel.Controls.Add(recieptPicture);
            recieptPanel.SetColumnSpan(recieptPicture, 2);

            itemPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            recieptPanel.Controls.Add(itemPanel);
            recieptPanel.SetColumnSpan(itemPanel, 2);
            int sum = PopulateReciept(products);

            Button acceptButton = new Button
            {
                Text = "Accept reciept",
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
            };
            acceptButton.Click += AcceptButton_Click;
            recieptPanel.Controls.Add(acceptButton, 0, 2);

            Label sumLabel = new Label
            {
                Font = new Font("Calibri", 12, FontStyle.Bold),
                Text = $"Total: {sum} kr",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomCenter,
                Margin = new Padding(0),
            };
            recieptPanel.Controls.Add(sumLabel, 1, 2);
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Populates the reciept with products and returns the sum of all products
        /// </summary>
        private int PopulateReciept(List<Product> products)
        {
            int sum = 0;
            foreach(Product product in products)
            {
                TableLayoutPanel productPanel = new TableLayoutPanel
                {
                    Size = new Size(265, 20),
                    ColumnCount = 3,
                };
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                productPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                itemPanel.Controls.Add(productPanel);

                Label nrOfProducts = new Label
                {
                    Text = product.nrOfProducts.ToString(),
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                productPanel.Controls.Add(nrOfProducts);

                Label productName = new Label
                {
                    Text = product.name,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                productPanel.Controls.Add(productName);

                Label productSumPrice = new Label
                {
                    Text = (product.price * product.nrOfProducts) + " kr",
                    TextAlign = ContentAlignment.MiddleRight,
                };
                productPanel.Controls.Add(productSumPrice);

                sum += (product.price * product.nrOfProducts);
            }
            return sum;
        }
        
        /// <summary>
        /// Sets current mouse coordinates when pressing the header picture.
        /// </summary>
        private void RecieptPicture_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && mouseLocation.IsEmpty)
            {
                mouseLocation = new Point(e.X, e.Y);
            }
        }

        /// <summary>
        /// Calculates windows position based on mouse movement.
        /// 
        /// Left            = window leftmost coordinate.
        /// e.X             = current mouse horizontal coordinate.
        /// mouseLocation.X = mouse horizontal coordinate when pressing the window header.
        /// 
        /// Top             = window topmost coordinate.
        /// e.Y             = current mouse vertical coordinate.
        /// mouseLocation.Y = mouse vertical coordinate when pressing the window header.
        /// </summary>
        private void RecieptPicture_Move(object sender, MouseEventArgs e)
        {
            if(mouseLocation != Point.Empty)
            {
                Point windowPos = new Point
                {
                    X = this.Left + (e.X - mouseLocation.X),
                    Y = this.Top + (e.Y - mouseLocation.Y),
                };
                this.Location = windowPos;
            }
        }

        /// <summary>
        /// Sets current mouse coordinates to none.
        /// </summary>
        private void RecieptPicture_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                mouseLocation = Point.Empty;
            }
        }
    }
}
