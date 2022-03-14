using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace AccessDBTest
{
    public partial class Form1 : Form
    {
        OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb");
        DataTable table;
        string tableName = "Tablo1";
        void Re(string tableName)
        {
            table = new DataTable();
            try { new OleDbDataAdapter("SELECT * FROM " + tableName, connection).Fill(table); dataGridView1.DataSource = table; }
            catch (Exception err) {}
        }
        void executeSql(string sqlString)
        {
            new OleDbCommand() { Connection = connection, CommandText = sqlString }.ExecuteNonQuery();

        }
        void addData(string tableName, Dictionary<string, string> data)
        {
            string sqlString = "INSERT INTO " + tableName + " (";
            foreach (var item in data)
            {
                sqlString += ("" + item.Key + ",");
            }
            sqlString = sqlString.TrimEnd(',') + ") VALUES (";
            foreach (var item in data)
            {
                sqlString += ("'" + item.Value + "',");
            }
            sqlString = sqlString.TrimEnd(',') + ")";
            executeSql(sqlString);
        }
        void deleteData(string tableName, Dictionary<string, string> condition)
        {
            if (condition.Keys.Count < 1) return;
            string sqlString = "DELETE FROM " + tableName + " ";
            if (condition[condition.Keys.ElementAt(0)] != "*"){
                sqlString += "WHERE ";
                foreach (var item in condition)
                {
                    sqlString += (item.Key + "='"+item.Value+"' and");
                }
                sqlString = sqlString.TrimEnd('d').TrimEnd('n').TrimEnd('a');
            }
            executeSql(sqlString);
        }
        public Form1()
        {
            InitializeComponent();
            Re(tableName);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var selected = dataGridView1.CurrentRow.Cells;
            txtId.Text = (string)selected[0].Value;
            txtAd.Text = (string)selected[1].Value;
            txtSoyad.Text = (string)selected[2].Value;
            txtNumara.Text = (string)selected[3].Value;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            addData(tableName, new Dictionary<string, string>() { { "Ad", txtAd.Text }, { "Soyad", txtSoyad.Text }, { "Numara", txtNumara.Text } });
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> condition = new Dictionary<string, string>() { { "Kimlik", txtId.Text } };
            Dictionary<string, string> data = new Dictionary<string, string>() { { "Ad", txtAd.Text }, { "Numara", txtNumara.Text }, { "Soyad", txtSoyad.Text } };
            foreach (var item in data)
            {
                if (item.Key != "Ad" && item.Value == "") throw new MissingPrimaryKeyException();
            }
            if (txtAd.Text == "*") condition["Kimlik"] = txtAd.Text;
            deleteData(tableName, condition);
        }
    }
}
