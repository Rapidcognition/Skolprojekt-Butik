﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Butikv3._6
{
    class ViewPanel : TableLayoutPanel
    {
        public TableLayoutPanel leftPanel;
        public FlowLayoutPanel itemPanel;
        public TableLayoutPanel descriptionPanel;
        private TableLayoutPanel selectedProductPanel;

        public ViewPanel()
        {
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));

            #region Left Panel
            leftPanel = new TableLayoutPanel
            {
                Name = "leftMenuPanel",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
            };
            this.Controls.Add(leftPanel, 0, 0);
            #endregion

            #region Product Panel
            itemPanel = new FlowLayoutPanel
            {
                Name = "itemPanel",
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(0, 4, 0, 0),
            };
            this.Controls.Add(itemPanel, 1, 0);
            #endregion

            #region Description panel
            descriptionPanel = new TableLayoutPanel
            {
                Name = "descriptionPanel",
                Dock = DockStyle.Fill,
                RowCount = 3,
            };
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 210));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            descriptionPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            this.Controls.Add(descriptionPanel, 2, 0);

            PictureBox descriptionPicture = new PictureBox
            {
                Name = "descriptionPicture",
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            descriptionPanel.Controls.Add(descriptionPicture);

            Label descriptionProductName = new Label
            {
                Name = "descriptionProductName",
                Text = "Ingen produkt vald.",
                Dock = DockStyle.Fill,
                Font = new Font("Calibri", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft,
            };
            descriptionPanel.Controls.Add(descriptionProductName);

            Label descriptionProductSummary = new Label
            {
                Name = "descriptionProductSummary",
                Text = " ---",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
            };
            descriptionPanel.Controls.Add(descriptionProductSummary);
            #endregion
        }

        /// <summary>
        /// Updates selected Product button collapse.
        /// </summary>
        public void UpdateSelectedProduct(TableLayoutPanel clickedProductPanelRef)
        {
            if (selectedProductPanel == null)
            {
                selectedProductPanel = clickedProductPanelRef;
                selectedProductPanel.BorderStyle = BorderStyle.Fixed3D;
            }

            if (selectedProductPanel != clickedProductPanelRef)
            {
                selectedProductPanel.BorderStyle = BorderStyle.None;
                selectedProductPanel = clickedProductPanelRef;
                selectedProductPanel.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        /// <summary>
        /// Updates description panel when a product is selected.
        /// </summary>
        public void UpdateDescriptionPanel(TableLayoutPanel descriptionPanelRef, Product productRef)
        {
            (descriptionPanelRef.Controls["descriptionPicture"] as PictureBox).ImageLocation = productRef.imageLocation;

            (descriptionPanelRef.Controls["descriptionProductName"] as Label).Text = productRef.name;

            (descriptionPanelRef.Controls["descriptionProductSummary"]).Text = productRef.summary;
        }
    }
}