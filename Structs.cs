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

namespace MetalDep
{
    public partial class frmMetalDep : Form
    {
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
            /** Film Parameters **/
            public string DENS;
            public string ZRAT;
            public string STHK;
            public string SPTM; //8
            public string RATE; //17

            /** System Parameters **/
            public string RUNNO; //3

            /** Run Time Data Parameters **/
            public string RATEVL; //2
            public string THKVL; //3
            public string RUNTMR; //5
            public string RDEVTN; //6

            /** Status Info Parameters **/
            public string RNMD; //7 - tells if a process is running (0 is false)

            /** Process Accounting Parameters **/
            public string PA_rnno; //2
            public string PA_ethk; //10
            public string PA_erate; //11
            public string PA_tproc; //22

            public Parameter(string dens, string zrat, string sthk, string sptm, string rate,
                        string runno, string ratevl, string thkvl, string runtmr, string rdevtn,
                        string rnmd, string parnno, string paethk, string paerate, string patproc)
            {
                DENS = dens;
                ZRAT = zrat;
                STHK = sthk;
                SPTM = sptm;
                RATE = rate;
                RUNNO = runno;
                RATEVL = ratevl;
                THKVL = thkvl;
                RUNTMR = runtmr;
                RDEVTN = rdevtn;
                RNMD = rnmd;
                PA_rnno = parnno;
                PA_ethk = paethk;
                PA_erate = paerate;
                PA_tproc = patproc;
            }
        }

        public struct Commands
        {
            public Parameter Param;
            /** What Version ? **/
            public string EXCT_whatv; //@

            /** R/W Film Parameters **/
            public string EXCT_rdfp; //A
            public string EXCT_wrfp; //B

            /** Read or Write System Parameters **/
            public string EXCT_rdsp; //C
            public string EXCT_wrsp; //D

            /** Read Runtime Data **/
            public string EXCT_rdata; //E

            /** Read or Set Status Info **/
            public string EXCT_rstat; //F
            public string EXCT_wstat; //G

            /** Read Process Accounting **/
            public string EXCT_prac; //L

            public Commands(Parameter param, string whatv, string rdfp, string wrfp, string rdsp,
                        string wrsp, string rdata, string rstat, string wstat, string prac)
            {
                Param = param;
                EXCT_whatv = whatv;
                EXCT_rdfp = rdfp;
                EXCT_wrfp = wrfp;
                EXCT_rdsp = rdsp;
                EXCT_wrsp = wrsp;
                EXCT_rdata = rdata;
                EXCT_rstat = rstat;
                EXCT_wstat = wstat;
                EXCT_prac = prac;
            }

        }

        private Commands cmd880 = new Commands();
        #endregion

        #region ReturnValues Defined
        public struct ReturnVal
        {   
                               //Following ACK/RESET
            public char AOK; // A
            public char AOKR; //B
            public char ILCD; // F
            public char ILCDR; //G
            public char ILDV; // H
            public char ILDVR; //I
            public char ILSN; // J
            public char ILSNR; // K
            public char INHB; // L
            public char INHBR; //M
            public char OBSOI; // R
            public char OBSOIR; //S

            public ReturnVal(char aok, char ilcd, char ildv, char ilsn, char inhb,
                            char obsoi, char aokr, char ilcdr, char ildvr, char ilsnr,
                            char inhbr, char obsoir)
            {
                AOK = aok;
                AOKR = aokr;
                ILCD = ilcd;
                ILCDR = ilcdr;
                ILDV = ildv;
                ILDVR = ildvr;
                ILSN = ilsn;
                ILSNR = ilsnr;
                INHB = inhb;
                INHBR = inhbr;
                OBSOI = obsoi;
                OBSOIR = obsoir;
            }

        }
        private ReturnVal ReturnValue = new ReturnVal();
        #endregion

        private void InitConstants()
        {
            /***Load the Table***/
            ElementTable.Name = materials;
            ElementTable.Symbol = symbols;
            ElementTable.Density = densities;
            ElementTable.Zfactor = zfactors;

            /*** Return Values ***/
            ReturnValue.AOK = 'A'; //all values assume normal start, unless ending in r for reset
            ReturnValue.AOKR = 'B';
            ReturnValue.ILCD = 'F';
            ReturnValue.ILCDR = 'G';
            ReturnValue.ILDV = 'H';
            ReturnValue.ILDVR = 'I';
            ReturnValue.ILSN = 'J';
            ReturnValue.ILSNR = 'K';
            ReturnValue.INHB = 'L';
            ReturnValue.INHBR = 'M';
            ReturnValue.OBSOI = 'R';
            ReturnValue.OBSOIR = 'S';

            /***Commands***/

            cmd880.EXCT_whatv = "@";
            cmd880.EXCT_rdfp = "A";
            cmd880.EXCT_wrfp = "B";
            cmd880.EXCT_rdsp = "C";
            cmd880.EXCT_wrsp = "D";
            cmd880.EXCT_rdata = "E";
            cmd880.EXCT_rstat = "F";
            cmd880.EXCT_wstat = "G";
            cmd880.EXCT_prac = "L";

            /***Params***/
            cmd880.Param.DENS = "1";
            cmd880.Param.ZRAT = "2";
            cmd880.Param.STHK = "6";
            cmd880.Param.SPTM = "8";
            cmd880.Param.RATE = "17";
            cmd880.Param.RUNNO = "3";
            cmd880.Param.RATEVL = "2";
            cmd880.Param.THKVL = "3";
            cmd880.Param.RUNTMR = "5";
            cmd880.Param.RDEVTN = "6";
            cmd880.Param.RNMD = "7";
            cmd880.Param.PA_rnno = "0 2"; //0 indicates the most recently finished process
            cmd880.Param.PA_ethk = "0 10";
            cmd880.Param.PA_erate = "0 11";
            cmd880.Param.PA_tproc = "0 22";

        }

    }

}