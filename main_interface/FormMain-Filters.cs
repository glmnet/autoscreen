﻿namespace AutoScreenCapture
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using AutoScreenCapture.Properties;

    public partial class FormMain : Form
    {
        private void comboBoxFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxFilterType.Text))
            {
                comboBoxFilterValue.Enabled = true;
                SearchFilterValues();
            }
            else
            {
                SearchFilterValues();
                SearchDates();
                ShowScreenshots();

                if (comboBoxFilterValue.Items.Count > 1)
                {
                    comboBoxFilterValue.SelectedIndex = 0;
                }

                comboBoxFilterValue.Enabled = false;
            }
        }

        private void DoWork_runFilterSearchThread(object sender, DoWorkEventArgs e)
        {
            RunFilterSearch(e);
        }

        private void RunFilterSearch(DoWorkEventArgs e)
        {
            if (comboBoxFilterValue.InvokeRequired)
            {
                comboBoxFilterValue.Invoke(new RunTitleSearchDelegate(RunFilterSearch), new object[] { e });
            }
            else
            {
                if (comboBoxFilterType.SelectedItem != null && !string.IsNullOrEmpty(comboBoxFilterType.Text))
                {
                    List<string> filterValueList = _screenshotCollection.GetFilterValueList(comboBoxFilterType.Text);
                    filterValueList.Add(string.Empty);
                    filterValueList.Sort();

                    comboBoxFilterValue.DataSource = filterValueList;
                }
            }
        }

        private void SearchFilterValues()
        {
            comboBoxFilterValue.BeginUpdate();

            if (runFilterSearchThread != null && !runFilterSearchThread.IsBusy)
            {
                runFilterSearchThread.RunWorkerAsync();
            }

            comboBoxFilterValue.EndUpdate();
        }
    }
}