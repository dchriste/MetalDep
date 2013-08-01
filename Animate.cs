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
        private void ActionsMenuToggle(object sender, EventArgs e)
        {
            //show the action panel
            if (!PanelIsMoving)
            {
                if (ActionsClicked)
                {
                    ActionsReClicked = true;
                    if (SettingsPanelShowing)
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

        #region Mouse sensing
        private void lblTriggerActionPanel_MouseHover(object sender, EventArgs e)
        {
            ActionsMenuToggle(sender, e); //allow show on hover
        }

        private void pnlActions_MouseLeave(object sender, EventArgs e)
        {
            if (ActionPanelShowing && !MouseOnPanel)
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


    }

}