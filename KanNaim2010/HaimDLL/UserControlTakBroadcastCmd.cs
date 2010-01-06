using System;
using System.Windows.Forms;
using DbSql;

namespace HaimDLL
{
    public partial class UserControlTakBroadcastCmd : UserControl
    {
        public UserControlTakBroadcastCmd()
        {
            InitializeComponent();
        }

        private void checkBoxTakStart_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = checkBoxTakStart.Checked;
            
            datePickerTakStart.Enabled = isEnabled;
            timePickerTakStart.Enabled = isEnabled;

            isEnabled |= checkBoxTakRecursive.Checked;

            checkBoxTakBroadcastAllCatSelector.Enabled = isEnabled;
            treeViewTakBroadcastCatSelector.Enabled = isEnabled;
        }

        private void checkBoxTakRecursive_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = checkBoxTakRecursive.Checked;

            groupBoxRecursiveCmd.Enabled = isEnabled;

            isEnabled |= checkBoxTakStart.Checked;

            checkBoxTakBroadcastAllCatSelector.Enabled = isEnabled;
            treeViewTakBroadcastCatSelector.Enabled = isEnabled;
        }

        public int SaveToDatabase(int takId)
        {
            int days;
            if (!int.TryParse(comboBoxTakRecDurationDays.Text, out days))
                days = 0;
            
            int hours;
            if (!int.TryParse(comboBoxTakRecDurationHours.Text, out hours))
                hours = 0;
            
            hours += 24 * days;
            
            var broadcast = new Table_Broadcast
            {
                DurationHours = hours,
                isRecursive = checkBoxTakRecursive.Checked,
                isDaily = radioButtonTakRepeatDaily.Checked,
                isWeekly = radioButtonTakRepeatWeekly.Checked,
                isMonthly = radioButtonTakRepeatWeekly.Checked,
                isYearly = radioButtonTakRepeatYearly.Checked,
                TakId = takId,
                StartDateTime = datePickerTakStart.Value
            };

            broadcast = Insert.TableBroadcast(broadcast);

            return (broadcast == null) ? -1 : broadcast.Id;
        }
    }
}
