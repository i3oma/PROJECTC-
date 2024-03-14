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

namespace PROJECT
{
    public partial class Ticktverwalten : Form
    {
        OleDbConnection con = new OleDbConnection();
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader dr = null;
        DataSet ds = new DataSet();
        OleDbDataAdapter da = new OleDbDataAdapter();
        OleDbCommandBuilder odcb = null;
        int Account = Anmelden.AccountID;
        string Zugriffsindex;
        public static string SharedString { get; set; }
        private void spaltenformatierung()
        {
            dataGridView1.Columns.Remove("T_ID");
            DataGridViewTextBoxColumn tb0 = new DataGridViewTextBoxColumn();
            tb0.DataPropertyName = "T_ID";
            tb0.HeaderText = "Tickt_ID";
            tb0.DisplayIndex = 0;
            dataGridView1.Columns.Add(tb0);

            dataGridView1.Columns.Remove("F_ID");
            DataGridViewTextBoxColumn tb1 = new DataGridViewTextBoxColumn();
            tb1.DataPropertyName = "F_ID";
            tb1.HeaderText = "Flug_ID";
            tb1.DisplayIndex = 1;
            dataGridView1.Columns.Add(tb1);

            dataGridView1.Columns.Remove("P_ID");
            DataGridViewTextBoxColumn tb2 = new DataGridViewTextBoxColumn();
            tb2.DataPropertyName = "P_ID";
            tb2.HeaderText = "Pilot_ID";
            tb2.DisplayIndex = 2;
            dataGridView1.Columns.Add(tb2);


        }

        void nachsteflugedatagridwiew()
        {
            SharedString = FlügeVerwalten.SharedString;
            cmd.CommandText = $"select * from Tickets where F_ID = {SharedString}";
            cmd.Connection = con;
            da.SelectCommand = cmd;
            ds.Clear();
            try
            {
                da.Fill(ds, "Accounts");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Accounts";
                spaltenformatierung();
            }
            catch (Exception a)
            {
                MessageBox.Show("Datenbanköffnungsfehler\n" + a);
                this.Close();
            }
        }
        public Ticktverwalten()
        {
            SharedString = FlügeVerwalten.SharedString;
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = FlughafenDB.accdb";
            con.Open();
            nachsteflugedatagridwiew();
            cmd.Connection = con;

            cmd.CommandText = $"SELECT * FROM Flüge WHERE F_ID = {SharedString}";

            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                label17.Text = reader["F_Nr"].ToString();
                label15.Text = reader["F_Abflug"].ToString();
                label13.Text = reader["F_Ankunft"].ToString();
                label23.Text = reader["F_Datum"].ToString();



            }

        }
    }
    }

