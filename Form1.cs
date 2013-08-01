
/*******************************************************************************
 * Copyright (C) 2013  David V. Christensen
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *********************************************************************************/
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
using System.Text.RegularExpressions;

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
        bool AskedIfProcRunning = false;
        bool AskedForDensity = false;
        bool AskedForZfactor = false;
        bool AskedForThickness = false;
        bool AskedForCurrentRunNO = false;
        bool AskedForLastRunNO = false;
        bool AskedForDepRate = false;
        bool AskedForRunTime = false;
        bool BuildString2Write = false;
        bool CollectionRunning = false;
        bool MinimizeSoon = false;
        bool MouseOnPanel = false;
        bool PanelIsMoving = false;
        bool RecordData = false;
        bool SettingsClicked = false;
        bool SettingsReClicked = false;
        bool SettingsPanelShowing = false;
        byte[] RX_Buff = new byte[18]; 
        int x = 0;
        int SerialPoll_Ticks = 0;
        int SerialPoll_WaitTime = 5; //change the poll interval for the dep check
        string[] portNames = new string[10];
        string[] Machines = { "PVD", "Lesker", "Leybold", "Veeco", "PVD Sputt", "CHA", "AIRCO", "Varian" };
        string BaseFileName = "MetalDep_CollectedData";
        string CurrentFileName = "";
        string CurrentRunNO = "";
        string CurrentDensity = "";
        string CurrentZfactor = "";
        string CurrentThickness = "";
        string CurrentDepRate = "";
        string CurrentRunTime = "";
        string CurrentMaterial = "";
        string CurrentSymbol = "";
        string LastRunNO = "";
        string StoreRunNO = "";
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
                    SW.WriteLine("Run/Lot#, Material, Symbol, Ending Thickness, Density, Z Factor, Deposition Rate, Running Time"); //column headers for csv
                    SW.Close();
                }
            }
            if (data2write != "")
            {
                using (StreamWriter SW = new StreamWriter(CurrentFileName, true))   //true makes it append to the file instead of overwrite
                {
                    SW.WriteLine(data2write);
                    SW.Close();
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this needs validation, check if collection is running etc...
            this.Close();
        }

        private void frmMetalDep_FormClosing(object sender, FormClosingEventArgs e)
        {
            //handle close events here
            //prompt about running collections!!
            if (btnStart.Text == "Stop Collection")
            {
                DispMsg("Collection Running!!!! Stop collection for safety."); 
            }

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
                        //WriteToFile("Silver,0.2,11");
                        //WriteToFile("Gold,0.1,12");
                        WriteToFile(""); //create file with headers


                        //start data collection...
                        CollectionRunning = true;
                        timer_SerialRead.Enabled = true;
                        ActionsMenuToggle(sender, e); //hide action panel
                        btnStart.Text = "Stop Collection";

                        //initiate communication
                        SendMSGwChkSm(cmd880.EXCT_whatv); //ask for the version 
                        //SendMSG(cmd880.EXCT_rstat + cmd880.Param.RNMD);

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
            char tmp_chr = ' ';
            int lengthOfMessage = ' ';
            int i = 0;
            int chksum = 0;
            if (SerialPort.IsOpen == true)
            {      
                SerialPort.ReadTimeout = 10; //in miliseconds
                tempString = null;
                try
                {
                    //tempString = SerialPort.ReadLine();
                    tmp_chr = (char)SerialPort.ReadByte();
                    if (tmp_chr == '\x02')
                    {
                        lengthOfMessage = (int)SerialPort.ReadByte();
                        for (i = 0; i < lengthOfMessage; i++)
                        {
                            RX_Buff[i] = (byte)SerialPort.ReadByte();
                        }
                    }
                    chksum = SerialPort.ReadByte();
                    for (i = 0; i < RX_Buff.Length; i++)
                    {
                        tempString += (char)RX_Buff[i];
                    }
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
                            //break;
                        case ("Lesker"):
                            //blah
                            //break;
                        case ("Leybold"):
                            //Communicate2Inficon880(RX_Data);
                            //break;
                        case ("Veeco"):
                            //blah
                            //break;
                        case ("PVD Sputt"):
                            //blah
                            //break;
                        case ("CHA"):
                            //blah
                           // break;
                        case ("Varian"):
                            //blah
                            //break;
                        case ("AIRCO"):
                            //blah
                            //break;
                        default:
                            Communicate2Inficon880(RX_Data);
                            break;
                    }
                }
                else
                {
                    //tempString is null, increment counter poll for changed run #
                    if ((SerialPoll_Ticks < SerialPoll_WaitTime)  && !RecordData)
                    {
                        SerialPoll_Ticks++;
                    }
                    else
                    {
                        //5 seconds has elapsed since hearing from the device
                        //get the current run #
                        SendMSGwChkSm(cmd880.EXCT_rdsp + cmd880.Param.RUNNO);
                        AskedForCurrentRunNO = true;
                        SerialPoll_Ticks = 0; //reset ticks
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
        /*Attempts to communicate with the sycon error checking protocol used by the 880*/
        private void SendMSGwChkSm(string Message2Send)
        {
            if (SerialPort.IsOpen == true)
            {
                //perform checksum
                byte check = GetChecksum(Message2Send);
                byte length = (byte)Message2Send.Length;
                //byte[] bytes = Encoding.ASCII.GetBytes("\x02" + ToUnprintableHex(length) + Message2Send + Convert.ToChar(check)); // convert to raw bytes

                // + "\x0D"
                //SerialPort.WriteLine("\x02" + ToUnprintableHex(length) + Message2Send + (char)check); // stx (Data_Length) (DATA) (Checksum)
                SerialPort.Write("\x02" + ToUnprintableHex(length) + Message2Send + Convert.ToChar(check));
                //DisplaySerialData_Hex("\x02" + ToUnprintableHex(length) + Message2Send + Convert.ToChar(check));
            }
            else
            {
                DispMsg("serial message send fail");
            }
        }

        private void SendMSGwASCII(string Message2Send)
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtbxOutput.Clear();
        }

        private int WhatMaterial(string Density, string ZFactor)
        {
            double tmpDens = Convert.ToDouble(Density);
            double tmpZf = Convert.ToDouble(ZFactor);
            int elementNum = 0;
            int i = 0;

            for (i = 0; i < materials.Length; i++)
            {
                if (tmpDens == ElementTable.Density[i])
                {
                    if (tmpZf == ElementTable.Zfactor[i])
                    {
                        elementNum = i; //this is the material
                    }
                }
            }

            return elementNum;
        }

        private string ToUnprintableHex(int length)
        {
            string hexOutput = "";

            switch (length)
            {
                case (1):
                    hexOutput = "\x01";
                    break;
                case (2):
                    hexOutput = "\x02";
                    break;
                case (3):
                    hexOutput = "\x03";
                    break;
                case (4):
                    hexOutput = "\x04";
                    break;
                case (5):
                    hexOutput = "\x05";
                    break;
                case (6):
                    hexOutput = "\x06";
                    break;
                case (7):
                    hexOutput = "\x07";
                    break;
                case (8):
                    hexOutput = "\x08";
                    break;
                case (9):
                    hexOutput = "\x09";
                    break;
                case (10):
                    hexOutput = "\x0A";
                    break;
                case (11):
                    hexOutput = "\x0B";
                    break;
                case (12):
                    hexOutput = "\x0C";
                    break;
                case (13):
                    hexOutput = "\x0D";
                    break; 
                default:
                    hexOutput = "\x00";
                    break;
            }
            
            return hexOutput;
        }

        private byte GetChecksum(string data)
        {
            byte[] dataArr = Encoding.ASCII.GetBytes(data);
            byte check = 0;
            byte length = (byte)data.Length;
            /**Method 1 & 2 addition and XOR **/
            for (int i = 0; i < dataArr.Length; i++)
            {
                //check ^= dataArr[i];
                check += dataArr[i];
            }
            //check = (byte)(-check);
            check = (byte)(256 - ((uint)check%256));

            return check;
        }
    }
}
