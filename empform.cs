using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MyPayrollSystem
{
    public partial class empform : Form
    {
        static string connection = "datasource=127.0.0.1; port=3306; username=root; password=; database=mypayroll; SslMode=none";
        MySqlConnection con = new MySqlConnection(connection);
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();
        public empform()
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Employee ID";
            dataGridView1.Columns[2].Name = "Basic Salary";
            dataGridView1.Columns[3].Name = "Total Tax";
            dataGridView1.Columns[4].Name = "Total Bonus";
            dataGridView1.Columns[5].Name = "Salary(USD)";
            dataGridView1.Columns[6].Name = "Salary(RMB)";


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            viewSalary();
            profile();
        }
        public void salaryList(string id, string employeeId, string bas, string tax, string bonus, string usd, string rmb)
        {
            dataGridView1.Rows.Add(id, employeeId, bas, tax, bonus, usd, rmb);
        }
        private void viewSalary()
        {
            dataGridView1.Rows.Clear();
            string id = login.id;
            string sql = "SELECT * FROM salary where employee_id= "+id;
            cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    salaryList(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString());
                }
                con.Close();
                dt.Rows.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }
        private void profile()
        {
            string id = login.id;
            
            string eid = "";
            string fname = "";
            string lname = "";
            string age = "";
            string gender = ""; 
            string address = "";
            string mail = "";
            string phone = "";
            string des = "";
            string pass = "";

            try
            {
                cmd = new MySqlCommand("SELECT * FROM employee where id = " + id, con);
                con.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    eid = rd[0].ToString();
                    fname = rd[1].ToString();
                    lname = rd[2].ToString();
                    age = rd[3].ToString();
                    gender = rd[4].ToString();
                    address = rd[5].ToString();
                    mail = rd[6].ToString();
                    phone = rd[7].ToString();
                    des = rd[8].ToString();
                    pass = rd[9].ToString();
                }
                label1.Text = eid.ToString();
                label2.Text = fname.ToString();
                label3.Text = lname.ToString();
                label4.Text = age.ToString();
                label5.Text = gender.ToString();
                label6.Text = address.ToString();
                label7.Text = mail.ToString();
                label8.Text = phone.ToString();
                label9.Text = des.ToString();
                label10.Text = pass.ToString();
                con.Close();
                
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }

        private void empform_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            login aaa = new login();
            aaa.Show();
        }
    }
}
