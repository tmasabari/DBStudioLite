using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace DBStudioLite
{
    public partial class frmConnections : Form
    {
        //public event EventHandler ConnectionChanged;

        public string ConnectionItems = "";
        string[] sItems;
        public ArrayList sConnectionCaptions = new ArrayList();
        public ArrayList sConnectionData = new ArrayList();
        private int iOldIndex = -1;
        private bool bFirstTime = false;

        public frmConnections()
        {
            bFirstTime = true;
            InitializeComponent();
            ConnectionItems = DataSecure.ReadFile(Application.StartupPath + "\\connections.txt");
            ConnectionItems = ConnectionItems.Replace("\r\n", "");

            string[] sSplits = { "**" };
            sItems = ConnectionItems.Split(sSplits, StringSplitOptions.None);
            for (int i = 0; i < sItems.Length; i++)
            {
                string sCaption = "";
                if (sItems[i].IndexOf("||") >= 0)
                {
                    sCaption = sItems[i].Substring(0, sItems[i].IndexOf("||")).Trim();
                    if (sCaption.Length > 0)
                    {
                        ConnectionsList.Items.Add(sCaption);
                        sConnectionCaptions.Add(sCaption);
                        sConnectionData.Add(sItems[i].Substring(sItems[i].IndexOf("||") + 2));
                    }
                }
            }
        }
        private void frmConnections_Activated(object sender, EventArgs e)
        {
            if (bFirstTime)
            {
                bFirstTime = false;
                if (ConnectionsList.Items.Count == 0)
                {
                    btnAdd_Click(this, null);
                }
                this.Show();
            }

        }

        private string _selectedConnectionName;
        public string GetConnectionName()
        {
            return _selectedConnectionName;
        }
        public bool SetConnectionName(string value)
        {
            if (sConnectionCaptions.Contains(value))
            {
                _selectedConnectionName = value;
                return true;
            }
            else
            {
                _selectedConnectionName = "";
                return false;
            }
        }
        public int GetSelectedIndex()
        {
            int index = sConnectionCaptions.IndexOf(_selectedConnectionName);
            return index;
        }
        public string GetConnectionString()
        {
            int index = sConnectionCaptions.IndexOf(_selectedConnectionName);
            if (index == -1) return String.Empty;
            return (string) sConnectionData[index];
        }

        private void ConnectionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!this.Visible && ConnectionChanged != null)
            //    ConnectionChanged.Invoke(this, EventArgs.Empty);

            if (ConnectionsList.SelectedIndex < 0)
            {
                iOldIndex = ConnectionsList.SelectedIndex;
                return;
            }
            if (iOldIndex >= 0) sConnectionData[iOldIndex] = txtConnectionString.Text;
            iOldIndex = ConnectionsList.SelectedIndex;
            txtConnectionString.Text = (string)sConnectionData[ConnectionsList.SelectedIndex];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string sConnectionName = Microsoft.VisualBasic.Interaction.InputBox
                ("Enter New Connection Name", "New Connection", "", 100, 100);
            if (sConnectionName == "") return;

            ConnectionsList.Items.Add(sConnectionName);
            sConnectionCaptions.Add(sConnectionName);
            sConnectionData.Add("data source=<SERVERNAME\\INSTANCENAME>;user id=<UserAccountID>;password=<PASSWORD>");
            ConnectionsList.SelectedIndex = sConnectionData.Count - 1;
            //txtConnectionString.Text = "data source=<SERVERNAME\\INSTANCENAME>;user id=<UserAccountID>;password=<PASSWORD>";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConnections();
        }
        public void SaveConnections()
        {
            if (iOldIndex >= 0) sConnectionData[iOldIndex] = txtConnectionString.Text;
            string sData = "";
            for (int i = 0; i < sConnectionCaptions.Count; i++)
            {
                sData = sData + (string)sConnectionCaptions[i] + "||" +
                        sConnectionData[i] + "**" + Environment.NewLine;
            }
            DataSecure.WriteFile(Application.StartupPath + "\\connections.txt", sData);
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (ConnectionsList.SelectedIndex >= 0)
            {
                txtConnectionString.Text = "";
                sConnectionCaptions.RemoveAt(ConnectionsList.SelectedIndex);
                sConnectionData.RemoveAt(ConnectionsList.SelectedIndex);
                ConnectionsList.Items.RemoveAt(ConnectionsList.SelectedIndex);
            }
        }

        private void frmConnections_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConnections();
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
