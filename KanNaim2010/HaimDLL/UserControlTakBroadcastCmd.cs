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

        public bool SaveToDatabase(int takId, int takTypeId)
        {
            int days;
            if (!int.TryParse(comboBoxTakRecDurationDays.Text, out days))
                days = 0;

            int hours;
            if (!int.TryParse(comboBoxTakRecDurationHours.Text, out hours))
                hours = 0;

            hours += 24 * days;
            DateTime startDateTime = datePickerTakStart.Value;

            var newBroadcast = new Table_Broadcast
            {
                isRecursive = checkBoxTakRecursive.Checked,
                isDaily = radioButtonTakRepeatDaily.Checked,
                isWeekly = radioButtonTakRepeatWeekly.Checked,
                isMonthly = radioButtonTakRepeatWeekly.Checked,
                isYearly = radioButtonTakRepeatYearly.Checked,
                TakId = takId,
                TakTypeId = takTypeId,
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(hours)
            };

            foreach (TreeNode node in treeViewTakBroadcastCatSelector.Nodes)
            {
                if (node.IsSelected)
                {
                    int catId = Lookup.GetLookupCategoryIdFromName(node.Name);
                    newBroadcast.CategoryId = catId;
                    
                    Table_Broadcast broadcast = Filter.GetBroadcastByTakIdCatId(takId, catId);
                    
                    if (broadcast == null)
                    {
                        newBroadcast = Insert.TableBroadcast(newBroadcast);
                    }
                    else
                    {
                        broadcast.TakTypeId = (newBroadcast.TakTypeId > broadcast.TakTypeId)
                                               ? newBroadcast.TakTypeId
                                               : broadcast.TakTypeId;

                        newBroadcast = Insert.TableBroadcast(broadcast);
                    }
                }
            }
            return (newBroadcast != null);
        }
    }
}
