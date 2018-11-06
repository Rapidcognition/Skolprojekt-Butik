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
            PopulateHomePanelList();
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
        }
    }
}
