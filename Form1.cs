using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace MetalDep
{
    public partial class frmMetalDep : Form
    {
        public frmMetalDep()
        {
            InitializeComponent();
            cntxtMenuStrp_tray.Click += new EventHandler(cntxtMenuStrp_tray_Click);
        }

        public ContextMenuStrip cntxtMenuStrp_tray = new ContextMenuStrip();

        #region Variables
        bool ActionsClicked = false;
        bool ActionsReClicked = false;
        bool ActionPanelShowing = false;
        bool CollectionRunning = false;
        bool MinimizeSoon = false;
        bool MouseOnPanel = false;
        bool PanelIsMoving = false;
        bool SettingsClicked = false;
        bool SettingsReClicked = false;
        bool SettingsPanelShowing = false;
        int x = 0;
        string[] portNames = new string[10];
        string[] Machines = { "PVD", "Lesker", "Leybold", "Veeco", "PCD Sputt", "CHA", "AIRCO", "Varian" };
        string BaseFileName = "MetalDep_CollectedData";
        string CurrentFileName = "";
        string RX_Data = "";
        string tempString = "";
        #endregion

        #region Element Table

        public struct Elements
        {
            public string[] Name;
            public string[] Symbol;
            public double[] Density;
            public double[] Zfactor;

            public Elements(string[] name, string[] symbol, double[] density, double[] zfactor)
            {
                Name = name;
                Symbol = symbol;
                Density = density;
                Zfactor = zfactor;
            }
        }
        private string[] materials = { "Aluminum", "Aluminum-Oxide", "Antimony", "Arsenic", "Barium", "Beryllium", 
                               "Bismuth", "Bismuth-Oxide", "Boron", "Cadmium", "Cadmium-Selenide", "Cadmium-Sulfide", 
                               "Cadmium-Telluride", "Calcium", "Calcium-Fluoride", "Carbon_Diamond", "Carbon_Graphite", 
                               "Cerium3-Fluoride", "Cerium4-Oxide", "Chromium", "Chromium3-Oxide", "Cobalt", "Copper", 
                               "Copper1-SulfideA", "Copper1-SulfideB", "Copper3-Sulfide", "Dysprosium", "Erbium", 
                               "Gadolinium", "Gallium", "Gallium-Arsenide", "Germanium", "Gold", "Hafnium", 
                               "Hafnium-Oxide", "Holnium", "Indium", "Indium-Intimonide", "Indium-Oxide", "Iridium", 
                               "Iron", "Lanthanum", "Lanthanum-Fluoride", "Lanthanum-Oxide", "Lead", "Lead-Sulfide", 
                               "Lithium", "Lithium-Fluoride", "Magnesium", "Magnesium-Fluoride", "Magnesium-Oxide", 
                               "Manganese", "Manganese2-Sulfide", "Mercury", "Molybdenum", "Neodynium-Fluoride", 
                               "Neodynium-Oxide", "Nickel", "Niobium", "Niobium5-Oxide", "Palladium", "Platinum", 
                               "Potasium-Chloride", "Rhenium", "Rhodium", "Rubidium", "Samarium", "Scandium", 
                               "Selenium", "Silicon", "Silicon2-Oxide", "Silicon-Dioxide", "Silver", "Silver-Bromide", 
                               "Silver-Chloride", "Sodium", "Sodium-Chloride", "Sulfur", "Tantalum", "Tantalum4-Oxide", 
                               "Tellurium", "Terbium", "Thallium", "Thorium4-Fluoride", "Tin", "Titanium", "Titanium4-Oxide", 
                               "Titanium-Oxide", "Tungsten", "Tungsten-Carbide", "Uranium", "Vanadium", "Ytterbium", 
                               "Yttrium", "Yttrium-Oxide", "Zinc", "Zinc-Oxide", "Zinc-Selenide", "Zinc-Sulfide", "Zirconium", 
                               "Zirconium-Oxide" };
        private string[] symbols = { "Al", "Al2O3", "Sb", "As", "Ba", "Be", "Bi", "Bi2O3", "B", "Cd", "Cdse", "Cds", "Cdte", 
                                     "Ca", "CaF2", "C", "C", "CeF3", "CeO2", "Cr", "Cr2O3", "Co", "Cu", "Cu2S-A", "Cu2S-B", 
                                     "CuS", "Dy", "Er", "Gd", "Ga", "GaAs", "Ge", "Au", "Hf", "HfO2", "Ho", "In", "InSb", 
                                     "In2O3", "Ir", "Fe", "La", "LaF3", "LaO3", "Pb", "PbS", "Li", "LiF", "Mg", "MgF2", 
                                     "MgO", "Mn", "MnS", "Hg", "Mo", "NdF3", "Nd2O3", "Ni", "Nb", "Nb2O5", "Pd", "Pt", "KCl", 
                                     "Re", "Rh", "Rb", "Sm", "Sc", "Se", "Si", "SiO", "SiO2", "Ag", "AgBr", "AgCl", "Na", 
                                     "NaCl", "S", "Ta", "Ta2O5", "Te", "Tb", "Tl", "ThF4", "Sn", "Ti", "TiO2", "TiO", "W", 
                                     "WC", "U", "V", "Yb", "Y", "Y2O3", "Zn", "ZnO", "ZnSe", "ZnS", "Zr", "ZrO2" };
        private double[] densities = { 2.73, 3.97, 6.62, 5.73, 3.5, 1.85, 9.8, 8.9, 2.54, 8.64, 5.81, 4.83, 5.85, 1.55, 3.18, 
                                       3.52, 2.25, 6.16, 7.13, 7.2, 5.21, 8.71, 8.93, 5.6, 5.8, 4.6, 8.54, 9.05, 7.89, 5.93, 
                                       5.31, 5.35, 19.3, 13.1, 9.63, 8.8, 7.3, 5.76, 7.18, 22.4, 7.86, 6.17, 5.94, 6.51, 11.3, 
                                       7.5, 0.53, 2.64, 1.74, 3, 3.58, 7.2, 3.99, 13.46, 10.2, 6.506, 7.24, 8.91, 8.57, 4.47, 
                                       12, 21.4, 1.98, 21.04, 12.41, 1.53, 7.54, 3, 4.82, 2.32, 2.13, 2.2, 10.5, 6.47, 5.56, 
                                       0.97, 2.17, 2.07, 16.6, 8.2, 6.25, 8.27, 11.85, 6.32, 7.3, 4.5, 4.26, 4.9, 19.3, 15.6, 
                                       18.7, 5.96, 6.98, 4.34, 5.01, 7.04, 5.61, 5.26, 4.09, 6.51, 5.6 };
        private double[] zfactors = { 1.08, 0.0, 0.768, 0.966, 2.1, 0.543, 0.79, 0.0, 0.389, 0.682, 0.0, 1.02, 0.98, 2.62, 0.775,
                                      0.22, 3.26, 0.0, 0.0, 0.305, 0.0, 0.343, 0.437, 0.69, 0.67, 0.82, 0.6, 0.74, 0.67, 0.593, 1.59, 
                                      0.516, 0.381, 0.36, 0.0, 0.58, 1.65, 0.769, 0.0, 0.129, 0.349, 0.92, 0.0, 0.0, 1.13, 0.566, 5.9, 
                                      0.774, 1.61, 0.0, 0.411, 0.377, 0.94, 0.74, 0.257, 0.0, 0.0, 0.331, 0.493, 0.0, 0.357, 0.245, 
                                      2.05, 0.15, 0.21, 2.54, 0.89, 0.91, 0.864, 0.712, 0.87, 1.07, 0.529, 1.18, 1.32, 4.8, 1.57, 2.29, 
                                      0.262, 0.3, 0.9, 0.66, 1.55, 0.0, 0.724, 0.628, 0.4, 0.0, 0.163, 0.151, 0.238, 0.53, 1.13, 0.835, 
                                      0.0, 0.514, 0.556, 0.722, 0.775, 0.6, 0.0 };
        private Elements ElementTable = new Elements();
        #endregion

        #region 880 Commands
        public struct Parameter
        {
            public string DENS;
            public string ZRAT;
            public string STHK;

            public Parameter(string dens, string zrat, string sthk)
            {
                DENS = dens;
                ZRAT = zrat;
                STHK = sthk;

            }
        }
        
        public struct Commands
        {
            public Parameter Param;
            public string EXCT_whatv;
            public string EXCT_rdfp;
            public string EXCT_wrfp;

            public Commands(Parameter param, string whatv, string rdfp, string wrfp)
            {
                EXCT_whatv = whatv;
                EXCT_rdfp = rdfp;
                EXCT_wrfp = wrfp;
                Param = param;

            }
            
        }

        private Commands cmd880 = new Commands();
        
        #endregion

        private void InitConstants()
        {
            ElementTable.Name = materials;
            ElementTable.Symbol = symbols;
            ElementTable.Density = densities;
            ElementTable.Zfactor = zfactors;

            /***Commands***/

            cmd880.EXCT_whatv = "@";
            cmd880.EXCT_rdfp = "A";
            cmd880.EXCT_wrfp = "B";

            /***Params***/
            cmd880.Param.DENS = "1";
            cmd880.Param.ZRAT = "2";
            cmd880.Param.STHK = "6";

        }

        private void DispMsg(string msg2disp)
        {
            MessageBox.Show(msg2disp, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /**courtesy of http://stackoverflow.com/questions/4657974/how-to-generate-unique-file-names-in-c-sharp **/
        public string GenerateFileName(string basename)
        {
            string[] FileNameParts = {""};
            string[] separators = {".","."};
            if (basename.Contains(".csv") || basename.Contains(".xls"))
            {
                FileNameParts = basename.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                basename = FileNameParts[0];
            }
            return basename + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + (FileNameParts[FileNameParts.Length-1] == "" ? "csv":FileNameParts[FileNameParts.Length-1]); 
        }

        public void WriteToFile(string data2write)
        {
            if (! File.Exists(CurrentFileName))
            {
                //write column headers for csv file
                using (StreamWriter SW = new StreamWriter(CurrentFileName, true))   //true makes it append to the file instead of overwrite
                {
                    SW.WriteLine("Material, Thickness, Run/Lot#"); //column headers for csv
                    SW.Close();
                }
            }
            using (StreamWriter SW = new StreamWriter(CurrentFileName, true))   //true makes it append to the file instead of overwrite
            {
                SW.WriteLine(data2write);
                SW.Close();
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this needs validation, check if collection is running etc...
            this.Close();
        }

        private void ActionsMenuToggle(object sender, EventArgs e)
        {
            //show the action panel
            if (!PanelIsMoving)
            {
                if (ActionsClicked)
                {
                    ActionsReClicked = true;
                    if(SettingsPanelShowing)
                    {
                        btnSettings_Click(sender, e); //hide settings panel first
                    }
                }
                else
                {
                    ActionsClicked = true;
                }
                FatherTime.Enabled = true;
            }
        }

        private void frmMetalDep_FormClosing(object sender, FormClosingEventArgs e)
        {
            //handle close events here
            //prompt about running collections!!
            string message = "Do you really want to quit?!?    :(    ";

            string caption = "The Program is Closing!";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                //the form will close, save prefs etc.. now
                SavePrefs();
                SerialPort.Close(); //tie up loose ends..
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FatherTime_Tick(object sender, EventArgs e)
        {
            #region Panel Actions Animate
            if ((pnlActions.Location.Y > 471) && (!ActionPanelShowing) && (ActionsClicked) && (!ActionsReClicked))
            {
                //location 6,562 not showing
                if (x < 10)
                {
                    if (x == 0)
                    {
                        PanelIsMoving = true;
                    }
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y - 4,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else if (x < 20)
                {
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y - 3,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else if (x < 25)
                {
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y - 1,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else if (x < 30)
                {
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y + 1,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else
                {
                    x = 0;
                    ActionsClicked = true;
                    FatherTime.Enabled = false;
                    pnlActions.Focus();
                    PanelIsMoving = false;
                    ActionPanelShowing = true;
                }

            }
            else if ((pnlActions.Location.Y < 563) && (!SettingsPanelShowing) && (ActionsClicked) && (ActionsReClicked))
            {
                //location 6,492 is showing
                if (x < 5)
                {
                    if (x == 0)
                    {
                        PanelIsMoving = true;
                    }
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y + 1,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else if (x < 10)
                {
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y + 4,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else if (x < 19)
                {
                    pnlActions.SetBounds(pnlActions.Location.X, pnlActions.Location.Y + 5,
                                            pnlActions.Size.Width, pnlActions.Size.Height);
                    x++;
                }
                else
                {
                    x = 0;
                    ActionsClicked = false;
                    ActionsReClicked = false;
                    if (!SettingsPanelShowing)
                    {
                        FatherTime.Enabled = false;
                    }
                    this.Focus();
                    PanelIsMoving = false;
                    ActionPanelShowing = false;
                    if (MinimizeSoon)
                    {
                        MinimizeSoon = false;
                        Minimize2Tray();
                    }
                }

            }
            #endregion

            #region Settings Panel Animate
            if (ActionPanelShowing || SettingsPanelShowing)
            {
                if ((pnlSettings.Location.Y > -1) && (SettingsClicked) && (!SettingsReClicked))
                {
                    //location 6,562 not showing
                    if (x < 10)
                    {
                        if (x == 0)
                        {
                            PanelIsMoving = true;
                        }
                        pnlSettings.SetBounds(pnlSettings.Location.X, pnlSettings.Location.Y - 4,
                                                pnlSettings.Size.Width, pnlSettings.Size.Height);
                        x++;
                    }
                    else if (x < 20)
                    {
                        pnlSettings.SetBounds(pnlSettings.Location.X, pnlSettings.Location.Y - 3,
                                                pnlSettings.Size.Width, pnlSettings.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        SettingsClicked = true;
                        FatherTime.Enabled = false;
                        pnlSettings.Focus();
                        PanelIsMoving = false;
                        SettingsPanelShowing = true;
                    }

                }
                else if ((pnlSettings.Location.Y < 71) && (SettingsClicked) && (SettingsReClicked))
                {
                    //location 6,492 is showing
                    if (x < 5)
                    {
                        if (x == 0)
                        {
                            PanelIsMoving = true;
                        }
                        pnlSettings.SetBounds(pnlSettings.Location.X, pnlSettings.Location.Y + 1,
                                                pnlSettings.Size.Width, pnlSettings.Size.Height);
                        x++;
                    }
                    else if (x < 10)
                    {
                        pnlSettings.SetBounds(pnlSettings.Location.X, pnlSettings.Location.Y + 4,
                                                pnlSettings.Size.Width, pnlSettings.Size.Height);
                        x++;
                    }
                    else if (x < 19)
                    {
                        pnlSettings.SetBounds(pnlSettings.Location.X, pnlSettings.Location.Y + 5,
                                                pnlSettings.Size.Width, pnlSettings.Size.Height);
                        x++;
                    }
                    else
                    {
                        x = 0;
                        SettingsClicked = false;
                        SettingsReClicked = false;
                        if (!ActionsReClicked)
                        {
                            FatherTime.Enabled = false;
                        }
                        this.Focus();
                        PanelIsMoving = false;
                        SettingsPanelShowing = false;
                    }

                }
            }
            #endregion
        }

        private void pnlActions_Leave(object sender, EventArgs e)
        {
            //this hides the actions panel when you click on a control outside of it
            if (ActionsClicked)
            {
                ActionsReClicked = true;
                FatherTime.Enabled = true;
            }
        }

        private void serialCOMcmbbx_Click(object sender, EventArgs e)
        {
            byte tmp = 0;
            serialCOMcmbbx.Items.Clear();
            portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string name in portNames)
            {
                serialCOMcmbbx.Items.Add(name);
                tmp++;
            }
            if (tmp == 1) //there's only 1 serial port, let's connect
            {
                serialCOMcmbbx.SelectedIndex = 0; //choose the first one
                serialCOMcmbbx_SelectedIndexChanged(sender, e);
            }
        }

        private void serialCOMcmbbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SerialPort.IsOpen == true)  //make sure we don't try to open a port that is already
                SerialPort.Close();

            SerialPort.PortName = portNames[serialCOMcmbbx.SelectedIndex];

            try
            {
                SerialPort.Open();
            }
            catch (Exception E)
            {
                MessageBox.Show("Serial Port selected could not be opened...");
                SerialPort.Close();
            }


        }

        private void frmMetalDep_Load(object sender, EventArgs e)
        {
            InitConstants(); //Load Structs
            //load preferences in here...
            chkbxAllowClose.Checked = Properties.Settings.Default.AllowClose;
            chkbxMinimize.Checked = Properties.Settings.Default.MinimizeAtStart;
            chkbxExcel.Checked = Properties.Settings.Default.RunExcel;
            cmbxMachine.SelectedIndex = Properties.Settings.Default.Machine;

            serialCOMcmbbx_Click(sender, e);//pre-load the combobox


        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbxMachine.SelectedIndex == -1 || serialCOMcmbbx.SelectedIndex == -1)
            {
                MessageBox.Show("You haven't given enough information. Select your configuration in the combo boxes.");
            }
            else
            {
                if (btnStart.Text == "Start Collection")
                {
                    //prep output area to output stuff
                    txtbxOutput.Clear();
                    txtbxOutput.TextAlign = HorizontalAlignment.Left;
                    txtbxOutput.Font = new Font("Century Gothic", 9F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    if (Directory.Exists("C:\\Users\\" + Environment.UserName))
                    {
                        saveFileDialog.InitialDirectory = "C:\\Users\\" + Environment.UserName;
                    }
                    else if (Directory.Exists("C:\\Documents and Settings\\" + Environment.UserName))
                    {
                        saveFileDialog.InitialDirectory = "C:\\Documents and Settings\\" + Environment.UserName;
                    }

                    DialogResult result; 

                    saveFileDialog.FileName = BaseFileName;
                    result = saveFileDialog.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        //the user decided against starting
                        txtbxOutput.TextAlign = HorizontalAlignment.Center;
                        txtbxOutput.Font = new Font("Century Gothic", 12F, FontStyle.Bold | FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        txtbxOutput.Text = "Start of collection aborted!";
                    }
                    else
                    {
                        //we are going through with it.
                        CurrentFileName = GenerateFileName(saveFileDialog.FileName);
                        txtbxOutput.Text = "Collecting Data in file: " + CurrentFileName;

                        // this shows the operation of csv creation
                        WriteToFile("Silver,0.2,11");
                        WriteToFile("Gold,0.1,12");


                        //start data collection...
                        CollectionRunning = true;
                        timer_SerialRead.Enabled = true;
                        ActionsMenuToggle(sender, e); //hide action panel
                        btnStart.Text = "Stop Collection";

                        //SendMSG(EXCT_whatv); 

                        if (chkbxMinimize.Checked)
                        {
                            MinimizeSoon = true;
                        }
                    }
                }
                else
                {
                    //stop data collection
                    timer_SerialRead.Enabled = false;
                    CollectionRunning = false;
                    btnStart.Text = "Start Collection";
                    ActionsMenuToggle(sender, e);
                    
                    if (chkbxExcel.Checked)
                    {
                        //open csv in excel
                        Excel.Application excelApp = new Excel.Application();
                        excelApp.Visible = true;
                        excelApp.Workbooks.OpenText(CurrentFileName);
                    }
                }
                this.Text = "Metal Deposition " + (CollectionRunning ? "| (Running) " : "| (Idle) ") + "| " + cmbxMachine.Items[cmbxMachine.SelectedIndex].ToString();
            }
        }

        private void timer_SerialRead_Tick(object sender, EventArgs e)
        {
            //string tmp_str = "";
            if (SerialPort.IsOpen == true)
            {
                SerialPort.ReadTimeout = 10; //in miliseconds
                tempString = null;
                try
                {
                    tempString = SerialPort.ReadLine();
                }
                catch { }

                if (tempString != null)
                {
                    //MessageBox.Show(tempString); //testing
                    RX_Data = tempString;
                    tempString = null;
                    
                    //call function to parse data?
                    switch (cmbxMachine.Items[cmbxMachine.SelectedIndex].ToString())
                    {
                        case ("PVD"):
                            //blah
                            break;
                        case ("Lesker"):
                            //blah
                            break;
                        case ("Leybold"):
                            //blah
                            Communicate2Inficon880(RX_Data);
                            break;
                        case ("Veeco"):
                            //blah
                            break;
                        case ("PCD Sputt"):
                            //blah
                            break;
                        default:
                            //what happened??!!??
                            DispMsg("REALLY!?!?");
                            break;
                    }
                }
            }
        }

        private void cmbxMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxMachine.SelectedIndex != -1)
            {
                this.Text = "Metal Deposition " + (CollectionRunning ? "| (Running) " : "| (Idle) ") + "| " + cmbxMachine.Items[cmbxMachine.SelectedIndex].ToString();
            }
            else 
            {
                this.Text = "Metal Deposition " + (CollectionRunning ? "| (Running) " : "| (Idle) ");
            }

            //change the way we talk based on machine
            //done in serial timer tick event
        }

        #region Mouse sensing
        private void lblTriggerActionPanel_MouseHover(object sender, EventArgs e)
        {
            ActionsMenuToggle(sender, e); //allow show on hover
        }

        private void pnlActions_MouseLeave(object sender, EventArgs e)
        {
            if (ActionPanelShowing && ! MouseOnPanel)
            {
                ActionsMenuToggle(sender, e); //hide on mouse leave
            }
        }

        private void pnlActions_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPanel = true;
            
        }

        private void btnClear_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPanel = true;
        }

        private void btnStart_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPanel = true;
        }

        private void lblOutFile_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPanel = true;
        }
        
        private void txtbxOutput_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPanel = false;
            pnlActions_MouseLeave(sender, e);
        }

        private void frmMetalDep_MouseMove(object sender, MouseEventArgs e)
        {
            MouseOnPanel = false;
            pnlActions_MouseLeave(sender, e);
        }
        #endregion

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (chkbxAllowClose.Checked)
            {
                this.Close();
            }
        }

        private void chkbxAllowClose_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkbxAllowClose.Checked)
            {
                chkbxAllowClose.Text = "Allow Close ?";
                this.ControlBox = false;
                btnQuit.Font = new Font("Century Gothic", 10F, FontStyle.Bold | FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else
            {
                chkbxAllowClose.Text = "Allow Close " + "\u2713"; //✓
                this.ControlBox = true;
                btnQuit.Font = new Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }
        /*Handles minimize to systray*/
        private void TrayIcon_MouseDown(object sender, MouseEventArgs e)
        {
            bool rightClick = (e.Button == System.Windows.Forms.MouseButtons.Right);
            bool leftClick = (e.Button == System.Windows.Forms.MouseButtons.Left);

            if (rightClick)
            {
                //show context menu at mouse click location aligned right
                TrayIcon.ContextMenuStrip = cntxtMenuStrp_tray;
                cntxtMenuStrp_tray.Items.Clear();
                cntxtMenuStrp_tray.Items.Add("Quit?");
                TrayIcon.ContextMenuStrip.Show(e.Location, ToolStripDropDownDirection.Default);
            }
            else if (leftClick)
            {
                Minimize2Tray();
            }
        }
        /*Allows close from systray*/
        private void cntxtMenuStrp_tray_Click(object sender, EventArgs e)
        {
            if (chkbxAllowClose.Checked)
            {
                this.Close();
            }
        }

        private void Minimize2Tray()
        {
            TrayIcon.BalloonTipTitle = "Metal Deposition";
            TrayIcon.BalloonTipText = "Collection process is: " + (CollectionRunning ? "(Running) on " + cmbxMachine.Items[cmbxMachine.SelectedIndex].ToString() : "(Idle) ");

            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.Hide();
                TrayIcon.ShowBalloonTip(500);
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (!PanelIsMoving)
            {
                if (SettingsClicked)
                {
                    SettingsReClicked = true;
                }
                else
                {
                    SettingsClicked = true;
                }
                FatherTime.Enabled = true;
            }
        }

        private void btnBack2Actions_Click(object sender, EventArgs e)
        {
            btnSettings_Click(sender, e);
        }

        private void Communicate2Inficon880(string DataRXd)
        {
            //for testing
            DispMsg("We received this: " + Environment.NewLine + DataRXd);
            if (chkbxDebug.Checked)
            {
                DisplaySerialData_Hex(DataRXd);
            }

        }

        /*Modified method, courtesy of: http://csharpindepth.com/Articles/General/strings.aspx */
        private void DisplaySerialData_Hex(string chars2disp)
        {
            string[] LowNames = 
            {"NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL", "BS",
                "HT", "LF", "VT", "FF", "CR", "SO", "SI", "DLE", "DC1", "DC2",
                "DC3", "DC4", "NAK", "SYN", "ETB", "CAN", "EM", "SUB", "ESC", 
                "FS", "GS", "RS", "US"};

            txtbxOutput.Text = String.Format("String length: {0}", chars2disp.Length) + Environment.NewLine;
            foreach (char c in chars2disp)
            {
                if (c < 32)
                {
                    txtbxOutput.Text += String.Format("<{0}>\t U+{1:x4}", LowNames[c], (int)c) + Environment.NewLine;
                }
                else if (c > 127)
                {
                    txtbxOutput.Text += String.Format("(Possibly non-printable) U+{0:x4}", (int)c) + Environment.NewLine;
                }
                else
                {
                    txtbxOutput.Text += String.Format("{0}\t U+{1:x4}", c, (int)c) + Environment.NewLine;
                }
            }
        }

        private void SavePrefs()
        {
            Properties.Settings.Default.AllowClose = chkbxAllowClose.Checked;
            Properties.Settings.Default.MinimizeAtStart = chkbxMinimize.Checked;
            Properties.Settings.Default.RunExcel = chkbxExcel.Checked;
            Properties.Settings.Default.Machine = (byte) cmbxMachine.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void chkbxExcel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxExcel.Checked)
            {
                chkbxExcel.Text = "Run Excel " + '\u2713';
            }
            else
            {
                chkbxExcel.Text = "Run Excel ?";
            }
        }

        private void chkbxMinimize_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxMinimize.Checked)
            {
                chkbxMinimize.Text = "Minimize at Start " + '\u2713';
            }
            else
            {
                chkbxMinimize.Text = "Minimize at Start ?";
            }
        }

        private void chkbxDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxDebug.Checked)
            {
                chkbxDebug.Text = "Debugging Mode " + '\u2713';
            }
            else
            {
                chkbxDebug.Text = "Debugging Mode ?";
            }
        }
        /*This method decides when the scroll bars ought to be shown*/
        private void txtbxOutput_TextChanged(object sender, EventArgs e)
        {
            if (txtbxOutput.Lines.Length >= 23)
            {
                txtbxOutput.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                txtbxOutput.ScrollBars = ScrollBars.None;
            }
        }

        private void SendMSG(string Message2Send)
        {
            if (SerialPort.IsOpen == true)
            {
                SerialPort.WriteLine('$' + Message2Send + '\x0D'); // begin with $ as stx and end with CR as etx
            }
            else
            {
                DispMsg("serial message send fail");
            }
        }
    }
}
