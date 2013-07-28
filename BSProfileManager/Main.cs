using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Reflection;

namespace BSProfileManager
{
    public partial class Main : Form
    {
        private string versionNumber = "v0.9";
        private SettingHelper settingHelper;
        private BSHelper bsHelper;
        private SqliteHelper sqliteHelper;

        public Main()
        {   
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = string.Format(this.Text, versionNumber);

            //init helper
            settingHelper = new SettingHelper();
            bsHelper = new BSHelper();
            sqliteHelper = new SqliteHelper();
            
            //check bs program
            checkBSProgram(settingHelper.getStartExePath());

            //import old record
            importLog();

            //build grid
            buildGrid();
        }

        private void importLog()
        {
            if (File.Exists(Global.LOG_FILE_PATH))
            {
                if (MessageBox.Show("log.txt is found, import it?", "Importing...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (StreamReader sr = new StreamReader(Global.LOG_FILE_PATH))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != string.Empty)
                                sqliteHelper.insertRecord(line);
                        }
                    }

                    if (MessageBox.Show("import finish, delete it?", "Importing...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        File.Delete(Global.LOG_FILE_PATH);
                    }
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Global.DEFAULT_PROGRAM_FILE_PATH;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string bsFolderPath = folderBrowserDialog1.SelectedPath + "\\";
                string bsExePath = bsFolderPath + Global.BS_START;

                if (checkBSProgram(bsExePath))
                    settingHelper.setBSFolderPath(bsFolderPath);

            }
        }

        private bool checkBSProgram(string path) 
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Cannot detect BS program");
                cbRestart.Checked = false;
                cbRestart.Enabled = false;
                return false;
            }
            else
            {
                cbRestart.Checked = true;
                cbRestart.Enabled = true;
                tbBSPath.Text = settingHelper.getBSFolderPath();
                return true;
            }
        }

        private void btnCreateNewGuid_Click(object sender, EventArgs e)
        {
            Guid guid = Guid.NewGuid();

            //modify
            modifyBSRegistry(btnCreateNewGuid, guid.ToString(), cbRestart.Checked);

            //write to db
            sqliteHelper.insertRecord(guid.ToString());
            
            //rebuild guid
            buildGrid();
            selectLastRow();
        }

        private void btnSetSelectedGuid_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string guid = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                //modify
                //modifyBSRegistry(btnSetSelectedGuid, guid, cbRestart.Checked);
                MessageBox.Show(guid);
            }
        }

        private void modifyBSRegistry(Button target, string guid, bool restartBS)
        {
            target.Enabled = false;
            string originText = target.Text;

            if (restartBS)
            {
                target.Text = "Closing BS...";
                target.Refresh();
                bsHelper.closeBS();
            }

            target.Text = "Modifiying GUID...";
            target.Refresh();
            bsHelper.modifyBSRegister(guid);

            if (restartBS)
            {
                target.Text = "Starting BS...";
                target.Refresh();
                bsHelper.startBS();
            }

            if (!restartBS)
            {
                MessageBox.Show("Please restart BS to take effect");
            }
            
            target.Text = originText;
            target.Enabled = true;
        }

        //rebuild the grid
        private void buildGrid()
        {
            DataSet ds = sqliteHelper.getDataSet();
            
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
        }

        private void selectLastRow()
        {
            int lastRowIndex = dataGridView1.Rows.Count - 1;
            dataGridView1.Rows[lastRowIndex].Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }

        //update row
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                string id = dataGridView1[0, e.RowIndex].Value.ToString();
                //string guid = dataGridView1[1, e.RowIndex].Value.ToString();
                string remark = dataGridView1[2, e.RowIndex].Value.ToString();

                sqliteHelper.updateRecord(id, remark);
            }
        }

        //delete row
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete?", "Deleting...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                sqliteHelper.deleteRecord(id);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string inputGuid = tbInputGuid.Text.Trim();
                Guid testGuid = new Guid(inputGuid);

                //write to db
                sqliteHelper.insertRecord(inputGuid);

                //rebuild guid
                buildGrid();
                selectLastRow();

                tbInputGuid.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("this is not a valid guid, please check again");
            }
        }
    }
}
