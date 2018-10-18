using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Butikv1
{
    class Movies
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Summary { get; set; }
        public PictureBox Picture { get; set; }
        public int Cost = 10;
    }
    class MyForm : Form
    {
        TableLayoutPanel mainPanel;
        FlowLayoutPanel leftPanel;
        TableLayoutPanel rightPanel;
        TableLayoutPanel rightInnerPanel;
        TableLayoutPanel movieTable;
        PictureBox moviePicture;
        Label movieTitle;
        Label movieGenre;
        Label moviePrice;
        List<Movies> movies = new List<Movies>();
        public MyForm()
        {
            #region Main panel and left/right side panels

            mainPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            leftPanel = new FlowLayoutPanel
            {
                BackColor = Color.Olive,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
            };
            rightPanel = new TableLayoutPanel
            {
                BackColor = Color.Orange,
                Dock = DockStyle.Fill,
                RowCount = 2,
            };
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            Controls.Add(mainPanel);
            mainPanel.Controls.Add(leftPanel);
            mainPanel.Controls.Add(rightPanel);

            #endregion

            #region Right side innerPanel
            rightInnerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                BackColor = Color.HotPink,
                AutoScroll = true,
            };
            Button shop = new Button
            {
                Text = "Buy",
                Dock = DockStyle.Fill,
                BackColor = Color.Bisque,
            };
            rightPanel.Controls.Add(rightInnerPanel);
            rightPanel.Controls.Add(shop);
            #endregion

            QueryFromCSVToList();
            QueryFromListToMenu(movies);

            this.MinimumSize = new System.Drawing.Size(500, 500);
        }

        private void QueryFromCSVToList()
        {
            string[][] path = File.ReadAllLines(@"TextFile1.csv").Select(x => x.Split(',')).Where(x => x[0] != "" && x[1] != "" && x[2] != "" && x[3] != "").ToArray();

            for (int i = 0; i < path.Length; i++)
            {
                Movies tmp = new Movies
                {
                    Title = path[i][0],
                    Year = int.Parse(path[i][1]),
                    Genre = path[i][2],
                    Summary = path[i][3],
                    Cost = 10,
                    Picture = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Height = 150,
                        Width = 150,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        ImageLocation = @"Movies\" + i + ".jpg"
                    }
                };
                movies.Add(tmp);
            }
        }

        private void QueryFromListToMenu(List<Movies> temp)
        {
            foreach (var item in temp)
            {
                movieTable = new TableLayoutPanel
                {
                    ColumnCount = 2,
                    BackColor = Color.WhiteSmoke
                };
                movieTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
                movieTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
                moviePicture = new PictureBox { BorderStyle = BorderStyle.Fixed3D, SizeMode = PictureBoxSizeMode.Zoom };
                movieTitle = new Label
                {
                    Dock = DockStyle.Fill,
                    Text = item.Title,
                    Font = new Font(DefaultFont, FontStyle.Bold),
                    Margin = new Padding(0, 3, 0, 0),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.WhiteSmoke,
                };
                movieGenre = new Label { Text = item.Genre };
                moviePrice = new Label { Text = item.Cost.ToString() + "kr" };

                moviePicture = item.Picture;
                movieTitle.Text = movieTitle.Text + "\n\n" + movieGenre.Text + "\n\n" + moviePrice.Text;
                movieTable.Controls.Add(moviePicture);
                movieTable.Controls.Add(movieTitle);
                leftPanel.Controls.Add(movieTable);
                //moviePicture.MouseClick += MovieTable_MouseClick;
                moviePicture.Tag = "Title: " + item.Title + "\nReleased: " + item.Year + "\nGenre: " + item.Genre + "\n\n" + item.Summary;
            }
        }
    }
}
