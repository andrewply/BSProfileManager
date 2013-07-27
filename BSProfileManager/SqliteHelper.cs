using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace BSProfileManager
{
    class SqliteHelper
    {
        private SQLiteConnection dbConnection;
        private SQLiteConnectionStringBuilder sb;
        private SQLiteCommandBuilder builder;
        private SQLiteCommand command;

        public SqliteHelper()
        {
            if (!File.Exists(Global.DB_FILE_PATH))
            {
                createDB();
            }
        }

        private void createDB()
        {
            //create file
            SQLiteConnection.CreateFile(Global.DB_FILE_PATH);
            
            //create table
            sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = Global.DB_FILE_PATH;

            openDB();

            string sql = "CREATE TABLE IF NOT EXISTS profile (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, guid VARCHAR(50), remark VARCHAR(200));";
            SQLiteCommand createCommand = new SQLiteCommand(sql, dbConnection);
            createCommand.ExecuteNonQuery();
            dbConnection.Close();

            closeDB();
        }

        private void openDB()
        {
            if (dbConnection != null)
                closeDB();

            sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = Global.DB_FILE_PATH;
            dbConnection = new SQLiteConnection(sb.ConnectionString);
            dbConnection.Open();
        }

        private void closeDB()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection = null;
            }
        }
        
        public DataSet getDataSet()
        {
            SQLiteDataAdapter dataAdaptor;
            DataSet ds = new DataSet();

            string sql = "select * from profile";

            openDB();
            dataAdaptor = new SQLiteDataAdapter(sql, dbConnection);
            dataAdaptor.Fill(ds);
            closeDB();

            return ds;
        }

        public void insertRecord(string guid)
        {
            string sql = "INSERT INTO profile (guid, remark) VALUES('{0}', '')";

            openDB();
            command = new SQLiteCommand(String.Format(sql, guid), dbConnection);
            command.ExecuteNonQuery();
            closeDB();
        }

        public void updateRecord(string id, string remark)
        {
            string sql = "UPDATE profile SET remark = '{0}' WHERE id = {1}";

            openDB();
            command = new SQLiteCommand(String.Format(sql, remark, id), dbConnection);
            command.ExecuteNonQuery();
            closeDB();
        }

        public void deleteRecord(string id)
        {
            string sql = "DELETE FROM profile WHERE id = {0}";
            
            openDB();
            command = new SQLiteCommand(String.Format(sql, id), dbConnection);
            command.ExecuteNonQuery();
            closeDB();
        }
    }
}
