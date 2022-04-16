using ACE.BIT.ADEV.CarWash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Jin.Hyeryung.RRCAG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            salesQuoteMenuItem.Click += SalesQuoteMenuItem_Click;
            carWashMenuItem.Click += CarWashMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;
            vehicleMenuItem.Click += VehicleMenuItem_Click;
            aboutMenuItem.Click += AboutMenuItem_Click;
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void VehicleMenuItem_Click(object sender, EventArgs e)
        {
            ACE.BIT.ADEV.Forms.VehicleDataForm vehicleData = new ACE.BIT.ADEV.Forms.VehicleDataForm();
            vehicleData.MdiParent = this;
            vehicleData.Show();
            vehicleData.Activate();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        List<Package> packages = new List<Package>();
        ACE.BIT.ADEV.Forms.CarWashForm carWash;
        ACE.BIT.ADEV.Forms.CarWashInvoiceForm carWashInvoice;
        ComboBox cboPackage;
        ComboBox cboFragrance;
        ListBox lstInterior;
        ListBox lstExterior;
        private void CarWashMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                carWash = new ACE.BIT.ADEV.Forms.CarWashForm();
                ACE.BIT.ADEV.CarWash.Package p1 = new ACE.BIT.ADEV.CarWash.Package()
                {
                    Description = "Standard",
                    Price = 7.50m,
                    ExteriorSerivces = new System.Collections.Generic.List<string>(new string[] { "Hand Wash" }),
                    InteriorServices = new System.Collections.Generic.List<string>(new string[] { "Fragrance" })
                };
                ACE.BIT.ADEV.CarWash.Package p2 = new ACE.BIT.ADEV.CarWash.Package()
                {
                    Description = "Deluxe",
                    Price = 15.0m,
                    ExteriorSerivces = new System.Collections.Generic.List<string>(new string[] { "Hand Wash", "Hand Wax" }),
                    InteriorServices = new System.Collections.Generic.List<string>(new string[] { "Fragrance", "Shampoo Carpets" })
                };
                ACE.BIT.ADEV.CarWash.Package p3 = new ACE.BIT.ADEV.CarWash.Package()
                {
                    Description = "Executive",
                    Price = 35.0m,
                    ExteriorSerivces = new System.Collections.Generic.List<string>(new string[] { "Hand Wash", "Hand Wax", "Wheel Polish" }),
                    InteriorServices = new System.Collections.Generic.List<string>(new string[] { "Fragrance", "Shampoo Carpets", "Shampoo Upholstery" })
                };
                ACE.BIT.ADEV.CarWash.Package p4 = new ACE.BIT.ADEV.CarWash.Package()
                {
                    Description = "Luxury",
                    Price = 55.0m,
                    ExteriorSerivces = new System.Collections.Generic.List<string>(new string[] { "Hand Wash", "Hand Wax", "Wheel Polish", "Detail Engine Compartment" }),
                    InteriorServices = new System.Collections.Generic.List<string>(new string[] { "Fragrance", "Shampoo Carpets", "Shampoo Upholstery", "Interior Protection Coat" })
                };
                packages.Add(p1);
                packages.Add(p2);
                packages.Add(p3);
                packages.Add(p4);
                cboPackage = carWash.Controls["cboPackage"] as ComboBox;
                cboFragrance = carWash.Controls["cboFragrance"] as ComboBox;
                cboPackage.DropDown += CboPackage_DropDown;
                AddFragrance(cboFragrance);
                carWash.Text = "Car Wash";
                carWash.MdiParent = this;
                carWash.Show();
                carWash.Activate();
                if (cboFragrance != null)
                    cboFragrance.SelectedValueChanged += CboFragrance_SelectedValueChanged;
                if (cboPackage != null)
                    cboPackage.SelectedValueChanged += CboPackage_SelectedValueChanged;
                var menuFileItem = menuStrip1.Items[0] as ToolStripMenuItem;
                var menuToolItem = menuStrip1.Items[2] as ToolStripMenuItem;
                var invoiceBtn = menuToolItem.DropDownItems[0] as ToolStripMenuItem;
                var closeBtn = menuFileItem.DropDownItems[2] as ToolStripMenuItem;
                invoiceBtn.Click += InvoiceBtn_Click;
                closeBtn.Click += CloseBtn_Click;
            }
            catch (Exception)
            {
                carWash.Close();
            }
        }

        private void CboPackage_DropDown(object sender, EventArgs e)
        {
            cboPackage.Items.Clear();
            AddPackage(cboPackage, packages);
        }

        private void InvoiceBtn_Click(object sender, EventArgs e)
        {
            carWashInvoice = new ACE.BIT.ADEV.Forms.CarWashInvoiceForm();
            carWashInvoice.Show();
            DisplayInvoice();
            carWashInvoice.Text = "Car Wash Invoice";
            carWashInvoice.FormClosing += CarWashInvoice_FormClosing;
        }

        private void CarWashInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetCarWashData();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            carWash.Close();
        }

        Label gstLbl;
        Label pstLbl;
        Label subtotalLbl;
        Label totalLbl;
        private void CboPackage_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                gstLbl = carWash.Controls["lblGoodsAndServicesTax"] as Label;
                pstLbl = carWash.Controls["lblProvincialSalesTax"] as Label;
                subtotalLbl = carWash.Controls["lblSubtotal"] as Label;
                totalLbl = carWash.Controls["lblTotal"] as Label;
                lstInterior = carWash.Controls["gbServices"].Controls["lstInterior"] as ListBox;
                lstExterior = carWash.Controls["gbServices"].Controls["lstExterior"] as ListBox;
                var exteriors = packages.First(x => x.Description == cboPackage.Text).ExteriorSerivces;
                var interiors = packages.First(x => x.Description == cboPackage.Text).InteriorServices;
                var packageCost = packages.First(x => x.Description == cboPackage.Text).Price;
                lstExterior.Items.Clear();
                lstInterior.Items.Clear();
                foreach (var exterior in exteriors)
                {
                    lstExterior.Items.Add(exterior);
                }
                foreach (var interior in interiors)
                {
                    var value = interior;
                    if (interior.Contains("Fragrance"))
                        value = $"Fragrance - {cboFragrance.Text}";
                    lstInterior.Items.Add(value);
                }
                Business.CarWashInvoice invoice = new Business.CarWashInvoice(0, 5);
                invoice.PackageCost = packageCost;
                subtotalLbl.Text = invoice.SubTotal.ToString("C2");
                totalLbl.Text = invoice.Total.ToString("C2");
                pstLbl.Text = invoice.ProvincialSalesTaxCharged.ToString("0.00");
                gstLbl.Text = invoice.GoodsAndServicesTaxCharged.ToString("0.00");
            }
            catch (Exception)
            {
            }
        }

        private void CboFragrance_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lstInterior != null)
                lstInterior.Items[0] = $"Fragrance - {cboFragrance.Text}";
        }

        /// <summary>
        /// Add package description to CarWash Form Package comboBox
        /// </summary>
        /// <param name="cboPackage"></param>
        /// <param name="packages"></param>
        private void AddPackage(ComboBox cboPackage, List<Package> packages)
        {
            if (cboPackage != null)
            {
                cboPackage.Items.Add(packages[0].Description);
                cboPackage.Items.Add(packages[1].Description);
                cboPackage.Items.Add(packages[2].Description);
                cboPackage.Items.Add(packages[3].Description);
            }
        }
        private void AddFragrance(ComboBox cboFragrance)
        {
            try
            {
                if (cboFragrance != null)
                {
                    foreach (var fragrance in GetFragrances)
                    {
                        cboFragrance.Items.Add(fragrance.Description);
                    }
                    cboFragrance.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Get Fragrance data from the text file
        /// </summary>
        /// <returns></returns>
        private List<Fragrance> GetFragrances
        {
            get
            {
                List<Fragrance> fragrances = new List<Fragrance>();
                var path = $@"{Directory.GetCurrentDirectory()}\fragrances.txt";
                try
                {
                    var lines = File.ReadLines(path);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        Fragrance fragrance = new Fragrance()
                        {
                            Description = parts[0],
                            Price = decimal.Parse(parts[1])
                        };
                        fragrances.Add(fragrance);
                    }
                }
                catch (System.IO.IOException ex)
                {
                    var result = MessageBox.Show("Fragrances data file not found.", "Data File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                        carWash.Close();

                }

                return fragrances;
            }
        }
        private void SalesQuoteMenuItem_Click(object sender, EventArgs e)
        {
            VehicleSalesQuote vehicleSalesQuote = new VehicleSalesQuote();
            vehicleSalesQuote.MdiParent = this;
            vehicleSalesQuote.Show();
            vehicleSalesQuote.Activate();
        }

        private void ResetCarWashData()
        {
            gstLbl.Text = "";
            pstLbl.Text = "";
            subtotalLbl.Text = "";
            totalLbl.Text = "";
            lstExterior.Items.Clear();
            lstInterior.Items.Clear();
            cboPackage.Text = "";
            cboPackage.Items.Clear();
            cboFragrance.Text = "";
        }

        private void DisplayInvoice()
        {
            if (cboPackage.Text != "")
            {
                Business.CarWashInvoice invoice = new Business.CarWashInvoice(0, 5);
                var packageCost = packages.First(x => x.Description == cboPackage.Text).Price;
                invoice.PackageCost = packageCost;
                var lblFragrancePrice = carWashInvoice.Controls["lblFragrancePrice"] as Label;
                var lblGoodsAndServicesTax = carWashInvoice.Controls["lblGoodsAndServicesTax"] as Label;
                var lblPackagePrice = carWashInvoice.Controls["lblPackagePrice"] as Label;
                var lblSubtotal = carWashInvoice.Controls["lblSubtotal"] as Label;
                var lblTotal = carWashInvoice.Controls["lblTotal"] as Label;
                var lblProvincialSalesTax = carWashInvoice.Controls["lblProvincialSalesTax"] as Label;

                lblPackagePrice.Text = invoice.PackageCost.ToString("C2");
                lblSubtotal.Text = invoice.SubTotal.ToString("C2");
                lblTotal.Text = invoice.Total.ToString("C2");
                lblProvincialSalesTax.Text = invoice.ProvincialSalesTaxCharged.ToString("0.00");
                lblGoodsAndServicesTax.Text = invoice.GoodsAndServicesTaxCharged.ToString("0.00");
                lblFragrancePrice.Text = invoice.FragranceCost.ToString("0.00");
            }
        }
    }
}
