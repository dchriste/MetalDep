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
        private void Communicate2Inficon880(string DataRXd)
        {
            char firstChar = ' ';
            int tmpint = 0;
            string Str2Write = "";
            firstChar = DataRXd[0];
            firstChar.ToString().ToUpper().ToCharArray();

            if (firstChar == ReturnValue.AOK || firstChar == ReturnValue.AOKR)
            {
                //host ack'd
                DataRXd.TrimStart(ReturnValue.AOK);
                DataRXd.TrimStart(ReturnValue.AOKR);
                //do whatever should happen with good reply
                DispMsg(DataRXd + " was an AOK reply");

                if (CurrentRunNO == "") // we haven't collected anything since program start
                {
                    if (!AskedIfProcRunning && !AskedForCurrentRunNO)// yet to check if the machine is running a process
                    {
                        SendMSG(cmd880.EXCT_rstat + "," + cmd880.Param.RNMD); //should ask if something is running
                        AskedIfProcRunning = true;
                    }
                    else if (AskedIfProcRunning && (Convert.ToDouble(DataRXd) != 0) && CurrentRunNO == "") //a process is running
                    {
                        //get the current run #
                        SendMSG(cmd880.EXCT_rdsp + "," + cmd880.Param.RUNNO);
                        AskedIfProcRunning = false;
                        AskedForCurrentRunNO = true;
                    }
                    else if (AskedForCurrentRunNO && CurrentRunNO == "")
                    {
                        //store currentRunNO
                        CurrentRunNO = DataRXd;
                        if (LastRunNO == "") // and it should be
                        {
                            LastRunNO = DataRXd; //we're just starting, set them equal.
                        }
                        AskedForCurrentRunNO = false;
                    }
                }
                else if (!AskedForCurrentRunNO && !RecordData)//this will usually be the case, rather than the 1st condition
                {
                    //get the current run #
                    SendMSG(cmd880.EXCT_rdsp + "," + cmd880.Param.RUNNO);
                    AskedForCurrentRunNO = true;
                }
                else if (AskedForCurrentRunNO)
                {
                    //store currentRunNO
                    CurrentRunNO = DataRXd;
                    try
                    {
                        if (Convert.ToDouble(CurrentRunNO) > Convert.ToDouble(LastRunNO))
                        {
                            RecordData = true; // a new run as begun 
                            SendMSG(cmd880.EXCT_prac + "," + cmd880.Param.PA_rnno); //job accounting runno
                            AskedForLastRunNO = true;
                            LastRunNO = DataRXd;
                        }
                        else
                        {
                            RecordData = false;
                        }
                    }
                    catch
                    {
                        //exceptions!!!
                        DispMsg("AHHH!!!");
                    }
                    AskedForCurrentRunNO = false;
                }
                else if (RecordData) //then we can save the last run
                {
                    if (AskedForLastRunNO)
                    {
                        //store Run #
                        StoreRunNO = DataRXd;
                        //ask for Density
                        SendMSG(cmd880.EXCT_rdfp + "," + cmd880.Param.DENS);
                        AskedForDensity = true;
                        AskedForLastRunNO = false;
                    }
                    else if (AskedForDensity)
                    {
                        //store Density
                        CurrentDensity = DataRXd;
                        //ask for Zfactor
                        SendMSG(cmd880.EXCT_rdfp + "," + cmd880.Param.ZRAT);
                        AskedForDensity = false;
                        AskedForZfactor = true;
                    }
                    else if (AskedForZfactor)
                    {
                        //store Zfactor
                        CurrentZfactor = DataRXd;
                        //ask for thickness (if run ended)
                        SendMSG(cmd880.EXCT_prac + "," + cmd880.Param.PA_ethk); //ending thickness
                        AskedForZfactor = false;
                        AskedForThickness = true;
                    }
                    else if (AskedForThickness)
                    {
                        //store thickness
                        CurrentThickness = DataRXd;
                        //ask for DepRate
                        SendMSG(cmd880.EXCT_prac + "," + cmd880.Param.PA_erate); //ending rate
                        AskedForThickness = false;
                        AskedForDepRate = true;
                    }
                    else if (AskedForDepRate)
                    {
                        //store deprate
                        CurrentDepRate = DataRXd;
                        //ask for RunTime
                        SendMSG(cmd880.EXCT_prac + "," + cmd880.Param.PA_tproc);//time to idle or stop
                        AskedForDepRate = false;
                        AskedForRunTime = true;
                    }
                    else if (AskedForRunTime)
                    {
                        //store runtime
                        CurrentRunTime = DataRXd;
                        //build string to write to file
                        AskedForRunTime = false;
                        BuildString2Write = true;
                    }
                    else if (BuildString2Write)
                    {
                        tmpint = WhatMaterial(CurrentDensity, CurrentZfactor);
                        CurrentSymbol = ElementTable.Symbol[tmpint];
                        CurrentMaterial = ElementTable.Name[tmpint];
                        Str2Write = "";

                        Str2Write = StoreRunNO + "," + CurrentMaterial + "," + CurrentSymbol + "," + CurrentThickness + "," +
                                    CurrentDensity + "," + CurrentZfactor + "," + CurrentDepRate + "," + CurrentRunTime;
                        WriteToFile(Str2Write);
                        BuildString2Write = false;
                        RecordData = false;
                    }
                }
            }
            else
            {
                DispMsg("There were issues with this message, we " + Environment.NewLine +
                        "did not receive the AOK from the machine.");
            }

            //for testing
            //DispMsg("We received this: " + Environment.NewLine + DataRXd);
            if (chkbxDebug.Checked)
            {
                DisplaySerialData_Hex(DataRXd);
            }

        }

    }
}