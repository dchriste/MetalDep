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
        bool MouseOnPanel = false;
        bool PanelIsMoving = false;
        int x = 0;
        string[] portNames = new string[10];
        string[] Machines = { "PVD", "Lesker", "Leybold", "Veeco", "PCD Sputt" };
        string BaseFileName = "MetalDep_CollectedData";
        string CurrentFileName = "";
        string RX_Data = "";
        string tempString = "";
        #endregion

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
                SerialPort.Close(); //tie up loose ends..
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FatherTime_Tick(object sender, EventArgs e)
        {
            if ((pnlActions.Location.Y > 471) && (ActionsClicked) && (!ActionsReClicked))
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
            else if ((pnlActions.Location.Y < 563) && (ActionsClicked) && (ActionsReClicked))
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
                    FatherTime.Enabled = false;
                    this.Focus();
                    PanelIsMoving = false;
                    ActionPanelShowing = false;
                }

            }
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
            //load preferences in here...
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

                    if (Directory.Exists("C:\\Users\\" + Environment.UserName))
                    {
                        saveFileDialog.InitialDirectory = "C:\\Users\\" + Environment.UserName;
                    }
                    else if (Directory.Exists("C:\\Documents and Settings\\" + Environment.UserName))
                    {
                        saveFileDialog.InitialDirectory = "C:\\Documents and Settings\\" + Environment.UserName;
                    }

                    saveFileDialog.FileName = BaseFileName;
                    saveFileDialog.ShowDialog();
                    CurrentFileName = GenerateFileName(saveFileDialog.FileName);
                    txtbxOutput.Text = "Collecting Data in file: " + CurrentFileName;

                    // this shows the operation of csv creation
                    WriteToFile("Silver,0.2,11");
                    WriteToFile("Gold,0.1,12");
                    

                    //start data collection...
                    timer_SerialRead.Enabled = true;
                    ActionsMenuToggle(sender, e); //hide action panel
                    btnStart.Text = "Stop Collection";
                }
                else
                {
                    //stop data collection
                    timer_SerialRead.Enabled = false;
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
                    WriteToFile(RX_Data);
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
                this.ControlBox = false;
                btnQuit.Font = new Font("Century Gothic", 10F, FontStyle.Bold | FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else
            {
                this.ControlBox = true;
                btnQuit.Font = new Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

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
                TrayIcon.BalloonTipTitle = "Metal Deposition";
                TrayIcon.BalloonTipText = "Collection process is: " + (CollectionRunning ? "(Running) " : "(Idle) ");

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
        }
        /*Allows close from systray*/
        private void cntxtMenuStrp_tray_Click(object sender, EventArgs e)
        {
            if (chkbxAllowClose.Checked)
            {
                this.Close();
            }
        }

    }
}
