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
        public string type;
        public string summary;
        public string imageLocation;
        public int nrOfProducts;

        public int interestPoints;
        public bool stage1;
        public bool stage2;
        public bool stage3;

        public Product()
        {
            price = 0;
            name = string.Empty;
            type = string.Empty;
            summary = string.Empty;
            imageLocation = string.Empty;
            nrOfProducts = 0;
        }

        public string ToCSV()
        {
            return $"{interestPoints},{price},{name},{type},{summary},{imageLocation},{nrOfProducts}";
        }
        public string ToDatabaseCSV()
        {
            return $"{interestPoints},{price},{name},{type},{summary},{imageLocation}";
        }

        private static Product p;
        /// <summary>
        /// Abstracted away logic for how we sort items into our lists of products.
        /// Class-method.
        /// </summary>
        /// <param name="CSVLine"></param>
        /// <returns></returns>
        public static Product FromCSV(string CSVLine)
        {
            string[] tmp = CSVLine.Split(',');
            try
            {
                p = new Product
                {
                    interestPoints = int.Parse(tmp[0]),
                    price = int.Parse(tmp[1]),
                    name = tmp[2],
                    type = tmp[3],
                    summary = tmp[4],
                    imageLocation = tmp[5],
                };
                if (tmp.Length <= 6)
                {
                    p.nrOfProducts = 1;
                }
                else
                {
                    p.nrOfProducts = int.Parse(tmp[6]);
                }
                return p;
            }
            catch (Exception e)
            {
                // FormatException
                MessageBox.Show("Problem med CSV-filens format.\n" + e.Message);
                Environment.Exit(1);
                return p;
            }
        }

        public void CalculateInterestPoints()
        {
            if (stage1)
            {
                interestPoints++;
            }
            if (stage2)
            {
                interestPoints++;
            }
            if (stage3)
            {
                interestPoints++;
            }
        }
    }
}
