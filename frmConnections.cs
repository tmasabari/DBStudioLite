using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace DBStudioLite
{
    public partial class frmConnections : Form
    {
        public event EventHandler ConnectionChanged;

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
            ConnectionItems = ReadFile(Application.StartupPath + "\\connections.txt");
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

        private string ReadFile(string sFile)
        {
            string sData = "";
            try
            {
                using (StreamReader objfile = new StreamReader(sFile))
                {
                    sData = objfile.ReadToEnd();
                    sData = DataSecure.DecryptString(sData);
                }
            }
            catch
            {
            }
            return sData;
        }
        private void WriteFile(string sFile, string data)
        {
            using (StreamWriter objfile = new StreamWriter(sFile))
            {
                objfile.Write(DataSecure.EncryptString(data));
            }

        }

        public int SelectedIndex
        {
            get
            {
                return ConnectionsList.SelectedIndex;
            }
            set
            {
                if (value < ConnectionList.Items.Count) ConnectionsList.SelectedIndex = value;
            }
        }
        public string ConnectionName
        {
            get
            {
                if (ConnectionsList.SelectedIndex >= 0)
                    return (string)sConnectionCaptions[ConnectionsList.SelectedIndex];
                else
                    return "";
            }
        }
        public string ConnectionText
        {
            get
            {
                if (ConnectionsList.SelectedIndex >= 0)
                    return (string)sConnectionData[ConnectionsList.SelectedIndex];
                else
                    return "";
            }
            set
            {
                sConnectionData[ConnectionsList.SelectedIndex] = value;
            }
        }
        public ListBox ConnectionList
        {
            get
            {
                return ConnectionsList;
            }
        }

        private void ConnectionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.Visible && ConnectionChanged != null)
                ConnectionChanged.Invoke(this, EventArgs.Empty);

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
            ConnectionList.SelectedIndex = sConnectionData.Count - 1;
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
            WriteFile(Application.StartupPath + "\\connections.txt", sData);
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (ConnectionList.SelectedIndex >= 0)
            {
                txtConnectionString.Text = "";
                sConnectionCaptions.RemoveAt(ConnectionList.SelectedIndex);
                sConnectionData.RemoveAt(ConnectionList.SelectedIndex);
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
