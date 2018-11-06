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
        List<Product> homePanelList;
        public HomePanel(CartPanel reference)
        {
            cartPanelRef = reference;
            #region specs
            this.ColumnCount = 3;
            this.RowCount = 2;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            #endregion

        }
        public void PopulateHomePanel()
        {
            List<List<Product>> mainProductList = new List<List<Product>>();

            List<Product> products = cartPanelRef.GetProductList().OrderByDescending(x => x.interestPoints).ToList();
            Product tmp = new Product();
            int sum = 0;
            List<Product> popularItems = new List<Product>();
            int outerCounter = 0;
            while(outerCounter < 3)
            {
                foreach (Product item in products)
                {
                    int counter = 0;
                    Product foo = item;

                    foreach (Product p in products)
                    {
                        if(foo.type == p.type && !popularItems.Contains(foo) && counter < 3)
                        {
                            popularItems.Add(p);
                            counter++;
                        }
                    }
                }
                // Suspicious line of code!
                if(!mainProductList.Contains(popularItems))
                    mainProductList.Add(popularItems);

                outerCounter++;
            }


            foreach (Product item in products)
            {
                if(tmp.type == item.type)
                {
                    sum += item.interestPoints;
                }
            }
            popularItems.Add(tmp);
        }
    }
}
