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
    public partial class FlügeVerwalten : Form
    {
        public static int FlugID
        {
            get; set;
        }
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
            dataGridView1.Columns.Remove("F_ID");
            DataGridViewTextBoxColumn tb0 = new DataGridViewTextBoxColumn();
            tb0.DataPropertyName = "F_ID";
            tb0.HeaderText = "ID";
            tb0.DisplayIndex = 0;
            dataGridView1.Columns.Add(tb0);

            dataGridView1.Columns.Remove("F_Nr");
            DataGridViewTextBoxColumn tb1 = new DataGridViewTextBoxColumn();
            tb1.DataPropertyName = "F_Nr";
            tb1.HeaderText = "Flugnummer";
            tb1.DisplayIndex = 1;
            dataGridView1.Columns.Add(tb1);

            dataGridView1.Columns.Remove("F_Abflug");
            DataGridViewTextBoxColumn tb2 = new DataGridViewTextBoxColumn();
            tb2.DataPropertyName = "F_Abflug";
            tb2.HeaderText = "Abflug";
            tb2.DisplayIndex = 2;
            dataGridView1.Columns.Add(tb2);

            dataGridView1.Columns.Remove("F_Ankunft");
            DataGridViewTextBoxColumn tb3 = new DataGridViewTextBoxColumn();
            tb3.DataPropertyName = "F_Ankunft";
            tb3.HeaderText = "Ankunft";
            tb3.DisplayIndex = 3;
            dataGridView1.Columns.Add(tb3);

            dataGridView1.Columns.Remove("F_Datum");
            DataGridViewTextBoxColumn tb4 = new DataGridViewTextBoxColumn();
            tb4.DataPropertyName = "F_Datum";
            tb4.HeaderText = "Datum";
            tb4.DisplayIndex = 4;
            dataGridView1.Columns.Add(tb4);


            dataGridView1.Columns.Remove("F_Maxgewichtproperson");
            DataGridViewTextBoxColumn tb5 = new DataGridViewTextBoxColumn();
            tb5.DataPropertyName = "F_Maxgewichtproperson";
            tb5.HeaderText = "Max. Gewicht pro Person (KG)";
            tb5.DisplayIndex = 5;
            dataGridView1.Columns.Add(tb5);


            dataGridView1.Columns.Remove("K_ID");
            DataGridViewTextBoxColumn tb6 = new DataGridViewTextBoxColumn();
            tb6.DataPropertyName = "K_ID";
            tb6.HeaderText = "Kapitan_ID";
            tb6.DisplayIndex = 6;
            dataGridView1.Columns.Add(tb6);

            dataGridView1.Columns.Remove("FZ_ID");
            DataGridViewTextBoxColumn tb7 = new DataGridViewTextBoxColumn();
            tb7.DataPropertyName = "FZ_ID";
            tb7.HeaderText = "Flug Nummer";
            tb7.DisplayIndex = 7;
            dataGridView1.Columns.Add(tb7);

            dataGridView1.Columns.Remove("F_K_ID");
            dataGridView1.Columns.Remove("F_FZ_ID");
            dataGridView1.Columns.Remove("K_Name");
            dataGridView1.Columns.Remove("FZ_Art");

            /*DataGridViewComboBoxColumn cb1 = new DataGridViewComboBoxColumn();
            cb1.DataSource = griddatatable();
            cb1.DataPropertyName = "fpilot";
            cb1.ValueMember = "K_ID";
            cb1.DisplayMember = "K_Name";
            cb1.HeaderText = "Piloten";
            cb1.DisplayIndex = 6;
            cb1.Width = 50;
            dataGridView1.Columns.Add(cb1);*/

            dataGridView1.ForeColor = Color.Black;
        }

        /*private DataTable griddatatable()
        {
            OleDbDataAdapter dapilot = new OleDbDataAdapter("Select F_K_ID, K_ID, K_Name from Kapitan, Flüge where F_K_ID = K_ID", con);
            DataTable griddt = new DataTable("fpilot");
            griddt.Columns.Add("F_K_ID", Type.GetType("System.String"));
            griddt.Columns.Add("K_ID", Type.GetType("System.String"));
            griddt.Columns.Add("K_Name", Type.GetType("System.String"));
            DataSet gridds = new DataSet();
            dapilot.Fill(gridds, "Kundenplz");
            griddt = gridds.Tables["Kundenplz"];
            return griddt;
        }*/
        void datagridwiew()
        {
            cmd.CommandText = "select F_ID, F_Nr, F_Abflug, F_Ankunft, F_Datum, F_Maxgewichtproperson, K_ID, F_K_ID, FZ_ID ,F_FZ_ID, K_Name, FZ_Art from Flüge, Kapitan, Flugzeuge where F_K_ID = K_ID AND F_FZ_ID = FZ_ID";
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
                MessageBox.Show("Datenbankfehler:\n" + a);
                this.Close();
            }
        }

        public FlügeVerwalten()
        {
            InitializeComponent();
        }

        private void Flüge_Verwalten_Load(object sender, EventArgs e)
        {
            try
            {
                con.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source = FlughafenDB.accdb;";
                con.Open();
            }
            catch (Exception a)
            {
                MessageBox.Show("Updatefehler:\n" + a);
                this.Close();
            }
            AdminHomepage.LoadAccount(cmd, dr, con, Account, label2, label3);
            datagridwiew();
            dataGridView1.Columns[0].ReadOnly = true;
            odcb = new OleDbCommandBuilder(da);

            OleDbDataAdapter dapilot = new OleDbDataAdapter("select * from Kapitan", con);
            DataSet dataSet = new DataSet();
            dapilot.Fill(dataSet, "Flugdaten");
            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string item = row["K_Name"].ToString();
                comboBox1.Items.Add(item);
            }
            OleDbDataAdapter dapilott = new OleDbDataAdapter("select * from Flugzeuge", con);
            DataSet dataSett = new DataSet();
            dapilott.Fill(dataSett, "Flugdaten");
            DataTable tablet = dataSett.Tables[0];
            foreach (DataRow rows in tablet.Rows)
            {

                string item = rows["FZ_Art"].ToString();
                comboBox2.Items.Add(item);
            }
           
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            var AdminHomepage = new AdminHomepage();
            AdminHomepage.Location = this.Location;
            AdminHomepage.StartPosition = FormStartPosition.Manual;
            AdminHomepage.FormClosing += delegate { this.Show(); };
            AdminHomepage.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Anmelden = new Anmelden();
            Anmelden.Location = this.Location;
            Anmelden.StartPosition = FormStartPosition.Manual;
            Anmelden.FormClosing += delegate { this.Show(); };
            Anmelden.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            da.Update(ds, "Accounts");          

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Ticktverwalten = new Ticktverwalten();
            this.Hide();
            Ticktverwalten.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataView dv = new DataView(ds.Tables["Accounts"]);
                DataRowView drv = dv[dataGridView1.CurrentRow.Index];
                Zugriffsindex = drv["F_ID"].ToString();
                FlugID = Convert.ToInt32(Zugriffsindex);
                button2.Enabled = true;
            }
            catch (Exception a)
            {
                MessageBox.Show("Tabellen-Zugriffsfehler(Direktes Suchen)" + a);
            }

/*OleDbDataAdapter dapilot = new OleDbDataAdapter("select K_Name, FZ_Art from Kapitan, Flugzeuge, Flüge where K_ID = F_K_ID AND F_FZ_ID = FZ_ID AND F_ID =" + FlugID, con);//, K_ID, F_K_ID, F_FZ_ID, F_ID
DataSet dataSet = new DataSet();
dapilot.Fill(dataSet, "Flugdaten");
DataTable table = dataSet.Tables[0];
string columnNamepilot = "K_Name";
string columnNamefz = "FZ_Art";
int rowIndex = 0;

object pilotname = table.Rows[rowIndex][columnNamepilot];
object fzart = table.Rows[rowIndex][columnNamefz];

string valueAsStringpilot = pilotname.ToString();
string valueAsStringfz = fzart.ToString();

comboBox1.SelectedItem = valueAsStringpilot;
comboBox2.SelectedItem = valueAsStringfz;*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
             dataGridView1.CurrentRow.Selected = true;
             DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            cmd.CommandText = $"SELECT * FROM Kapitan WHERE K_ID = {int.Parse(selectedRow.Cells[6].Value.ToString())}";
            OleDbDataReader reader = cmd.ExecuteReader();
             if (reader.Read())
              {
                    comboBox1.SelectedItem = reader["K_Name"].ToString();
              }
                SharedString = $"{int.Parse(selectedRow.Cells[0].Value.ToString())}";
             reader.Close();

              cmd.CommandText = $"SELECT * FROM Flugzeuge WHERE FZ_ID = {int.Parse(selectedRow.Cells[7].Value.ToString())}";
              reader = cmd.ExecuteReader();
             if (reader.Read())
              {
                    comboBox2.SelectedItem = reader["FZ_Art"].ToString();
             }

             reader.Close();
                button2.Enabled = true;

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
