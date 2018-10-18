using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Butikv2
{
    class Fruit
    {
        public string Item { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public PictureBox picture { get; set; }

        public string RetItem(string i)
        {
            return Item = i;
        }
        public int RetPrice(string p)
        {
            return Price = int.Parse(p);
        }
        public int RetAmount(int a) //Method to calculate price of amount
        {
            return a;
        }
    }
    class MyForm : Form
    {
        List<Fruit> fruits = new List<Fruit>();
        List<Fruit> fruitsInCart = new List<Fruit>();
        PictureBox fruitPic;
        Label fruitName;
        Label fruitPrice;

        FlowLayoutPanel leftPanel;
        FlowLayoutPanel rightPanel;
        TableLayoutPanel menuPanel;
        TableLayoutPanel cartPanel;
        public MyForm()
        {
            #region Main panels
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.Aqua,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.BlueViolet,
                AutoScroll = true,
            };
            rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
            };
            Controls.Add(mainPanel);
            mainPanel.Controls.Add(leftPanel);
            mainPanel.Controls.Add(rightPanel);
            #endregion

            QueryFromCSVToList();
            QueryFromListToMenu(fruits);
            this.MinimumSize = new System.Drawing.Size(500, 400);
        }

        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).Where(x => x[0] != "" && x[1] != "").ToArray();
            for (int i = 0; i < path.GetLength(0); i++)
            {
                Fruit f = new Fruit();
                f.RetItem(path[i][0]);
                f.RetPrice(path[i][1]);
                fruits.Add(f);
            }
            string[] path1 = Directory.GetFiles(@"Store");
            for (int i = 0; i < path1.Length; i++)
            {
                fruitPic = new PictureBox
                {
                    Height = 100,
                    Width = 100,
                    BorderStyle = BorderStyle.Fixed3D,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    ImageLocation = path1[i]
                };
                fruits[i].picture = fruitPic;
            }
        }

        private void QueryFromListToMenu(List<Fruit> temp)
        {
            foreach (var item in temp)
            {
                menuPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    BackColor = Color.OrangeRed,
                    ColumnCount = 3,
                };
                fruitPic = new PictureBox { };
                fruitName = new Label { Text = item.Item, Dock = DockStyle.Fill, };
                fruitPrice = new Label { Text = item.Price.ToString() + "kr", Dock = DockStyle.Fill };
                fruitPic = item.picture;
                menuPanel.Controls.Add(fruitPic);
                menuPanel.Controls.Add(fruitName);
                menuPanel.Controls.Add(fruitPrice);
                leftPanel.Controls.Add(menuPanel);
                fruitPic.Tag = menuPanel;
                fruitPic.Click += OnFruitPictureClickEvent;
            }
        }

        private void OnFruitPictureClickEvent(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            cartPanel = new TableLayoutPanel { ColumnCount = 3, Dock = DockStyle.Top, };
            cartPanel.Controls.Add((TableLayoutPanel)p.Tag);
            rightPanel.Controls.Add(cartPanel);
        }
    }
}
