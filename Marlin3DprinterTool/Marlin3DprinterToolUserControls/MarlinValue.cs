﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Marlin3DprinterToolUserControls
{
    public partial class MarlinValue : UserControl
    {
        #region Properties
       
        private string _controlText;
        private int _spliterDistance;
        



        public FirmwareHelper currentFirmwareHelper { get; set; }
        public FirmwareHelper NewFirmwareHelper { get; set; }


        

        public string ControlText
        {
            set
            {
                _controlText = value;
                UpdateText(_controlText);
                
            }
            get { return _controlText; }
        }

        public WebBrowser HelpWebBrowser { set; get; }

        public string HelpUrl { set; get; }

        public int SpliterDistance
        {
            set
            {
                _spliterDistance = value;
                splitContainer1.SplitterDistance = _spliterDistance;
            }
            get
            {
                _spliterDistance = splitContainer1.SplitterDistance;
                return _spliterDistance;
            }
        }


        public string Feature { set; get; }

        #endregion



        public MarlinValue()
        {
            InitializeComponent();
        }




        public void UpdateText(string text)
        {
            //Control to get Text/Label 
            lblControlText.Text = text;
        }
        
        public  void UpdateStatus()
        {
            ledBulbEqualcurrentFirmware.On = true;
            if (currentFirmwareHelper != null && NewFirmwareHelper != null)
            {
                if (currentFirmwareHelper.GetFeatureValue(Feature) == null && NewFirmwareHelper.GetFeatureValue(Feature) != null)
                {
                    ledBulbEqualcurrentFirmware.Color = Color.Yellow;
                    
                }
                else if (NewFirmwareHelper.GetFeatureValue(Feature) == null)
                {
                    ledBulbEqualcurrentFirmware.Color = Color.Blue;
                    this.Enabled = false;
                }
                else
                {
                    ledBulbEqualcurrentFirmware.Color = currentFirmwareHelper.GetFeatureValue(Feature) == NewFirmwareHelper.GetFeatureValue(Feature)
                        ? Color.Green
                        : Color.Red;
                }
            }
            string value = NewFirmwareHelper?.GetFeatureValue(Feature);
            if (value != null)
            {
                txtBxValue.Text = NewFirmwareHelper.GetFeatureValue(Feature);
            }
        }

        public  void TransfercurrentFirmwareToNewFirmware()
        {

            if (currentFirmwareHelper.GetFeatureValue(Feature) == null && NewFirmwareHelper.GetFeatureValue(Feature) != null)
            {
                MessageBox.Show(@"The feature " + Feature + @" is not available in the old Marlin Firmware." +
                                Environment.NewLine +
                                @"This might be a new feature." + Environment.NewLine +
                                @"Read the documentation and set this value manually.", @"New feature",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (NewFirmwareHelper.GetFeatureValue(Feature) == null)
            {
                MessageBox.Show(@"The feature " + Feature + @" is not available in the new Marlin Firmware." +
                                Environment.NewLine +
                                @"This might be a obsolite feature." + Environment.NewLine +
                                @"Read the documentation and set this value manually.", @"Obsolite feature in new Firmware",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(@"Do you want to transfer value from Current Firmware to the New Firmware?",
                @"Transfer value", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                Migrate();
                
            }
        }

        public void Migrate()
        {
            if (ledBulbEqualcurrentFirmware.Color == Color.Red || ledBulbEqualcurrentFirmware.Color == Color.Green)
            {
                NewFirmwareHelper.SetFeatureValue(Feature, currentFirmwareHelper.GetFeatureValue(Feature));
                UpdateStatus();
            }
            
        }

        public  void DataChanged()
        {
            NewFirmwareHelper.SetFeatureValue(Feature,txtBxValue.Text);
            ToolTip();
        }

        private void ToolTip()
        {
            toolTipControl.SetToolTip(lblControlText, "Activate and deactivate Marlin FW feature " + Feature);
            toolTipControl.SetToolTip(txtBxValue, "Enter value for Marlin FW feature " + Feature);

            switch (ledBulbEqualcurrentFirmware.Color.ToKnownColor().ToString())
            {
                case @"Red":
                    toolTipControl.SetToolTip(ledBulbEqualcurrentFirmware, $"Old FW feature {Feature} is different!" + Environment.NewLine +
                       @"Value in current Firmware is " + currentFirmwareHelper.GetFeatureValue(Feature));
                    break;
                case @"Green":
                    toolTipControl.SetToolTip(ledBulbEqualcurrentFirmware, $"New and Old FW feature {Feature} is equal.");
                    break;
                case @"Yellow":
                    toolTipControl.SetToolTip(ledBulbEqualcurrentFirmware, $"Feature {Feature} is not available in Old Marlin Firmware.");
                    break;
                case @"Blue":
                    toolTipControl.SetToolTip(ledBulbEqualcurrentFirmware, $"Feature {Feature} might be obsolite in new Marlin Firmware.");
                    break;

            }


            if (HelpWebBrowser != null && !string.IsNullOrEmpty(HelpUrl) && HelpWebBrowser.Visible)
            {
                HelpWebBrowser.Url = new Uri(HelpUrl);
            }
        }



        //=====================================================================================================
        //
        // Events
        //
        //=====================================================================================================
        

        private void MarlinValue_Enter(object sender, System.EventArgs e)
        {
            if (HelpWebBrowser != null && !string.IsNullOrEmpty(HelpUrl) && HelpWebBrowser.Visible)
            {
                HelpWebBrowser.Url = new Uri(HelpUrl);
            }
            UpdateStatus();
        }

        private void ledBulbEqualcurrentFirmware_Click(object sender, EventArgs e)
        {
            TransfercurrentFirmwareToNewFirmware();
        }

        private void MarlinValue_MouseHover(object sender, EventArgs e)
        {
            ToolTip();
        }

        private void chkBxProperty_MouseHover(object sender, EventArgs e)
        {
            ToolTip();
        }

        private void ledBulbEqualcurrentFirmware_MouseHover(object sender, EventArgs e)
        {
            ToolTip();
        }

        private void txtBxValue_TextChanged(object sender, EventArgs e)
        {
            DataChanged();
        }
    }
}
