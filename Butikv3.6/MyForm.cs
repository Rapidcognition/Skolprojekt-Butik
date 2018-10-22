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
    class Product
    {
        public int price;
        public string name;
        public string item;
        public string description;
        public PictureBox pictureBox { get; set; }


    }

    class MyForm : Form
    {
        StorePanel store = new StorePanel();
        List<Product> list = new List<Product>();
        List<string> listItem = new List<string>();

        public MyForm()
        {
            this.MinimumSize = new Size(800, 500);

            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                RowCount = 2,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            this.Controls.Add(mainPanel);

            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                ColumnCount = 3,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                Margin = new Padding(0),
            };
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            mainPanel.Controls.Add(topPanel);

            Button shopButton = new Button
            {
                Text = "Shop",
                Dock = DockStyle.Fill
            };
            topPanel.Controls.Add(shopButton);

            Label shopTitle = new Label
            {
                Text = "[Placeholder]",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            topPanel.Controls.Add(shopTitle);

            Button cartButton = new Button
            {
                Text = "Cart",
                Dock = DockStyle.Fill,
            };
            topPanel.Controls.Add(cartButton);

            mainPanel.Controls.Add(store.GetPanel());
            QueryFromCSVToList();
            store.PopulateStore(list);
        }
        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).
                Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "").
                ToArray();

            foreach (var item in path)
            {
                if (!listItem.Contains(item[2]))
                    listItem.Add(item[2]);
            }

            for (int i = 0; i < path.Length; i++)
            {
                Product tmp = new Product
                {
                    price = int.Parse(path[i][0]),
                    name = path[i][1],
                    item = path[i][2],
                    description = path[i][3],
                    pictureBox = new PictureBox
                    {
                        Height = 150,
                        Width = 150,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        ImageLocation = @"pictures\" + i + ".jpg",
                    }
                };
                list.Add(tmp);
            }
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hej!");
        }
    }
}
