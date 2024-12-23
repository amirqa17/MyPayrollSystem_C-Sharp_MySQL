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
    public partial class Form1 : Form
    {
        static string connection = "datasource=127.0.0.1; port=3306; username=root; password=; database=mypayroll; SslMode=none";
        MySqlConnection con = new MySqlConnection(connection);
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt = new DataTable();
        DataTable dt_2 = new DataTable();
        string tax1, tax2;
        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "First Name";
            dataGridView1.Columns[2].Name = "Last Name";
            dataGridView1.Columns[3].Name = "Age";
            dataGridView1.Columns[4].Name = "Gender";
            dataGridView1.Columns[5].Name = "Address";
            dataGridView1.Columns[6].Name = "Email";            
            dataGridView1.Columns[7].Name = "Phone";
            dataGridView1.Columns[8].Name = "Designation";

            dataGridView2.ColumnCount = 9;
            dataGridView2.Columns[0].Name = "ID";
            dataGridView2.Columns[1].Name = "First Name";
            dataGridView2.Columns[2].Name = "Last Name";
            dataGridView2.Columns[3].Name = "Age";
            dataGridView2.Columns[4].Name = "Gender";
            dataGridView2.Columns[5].Name = "Address";
            dataGridView2.Columns[6].Name = "Email";
            dataGridView2.Columns[7].Name = "Phone";
            dataGridView2.Columns[8].Name = "Designation";

            dataGridView3.ColumnCount = 7;
            dataGridView3.Columns[0].Name = "ID";
            dataGridView3.Columns[1].Name = "Employee ID";
            dataGridView3.Columns[2].Name = "Basic Salary";
            dataGridView3.Columns[3].Name = "Total Tax";
            dataGridView3.Columns[4].Name = "Total Bonus";
            dataGridView3.Columns[5].Name = "Salary(USD)";
            dataGridView3.Columns[6].Name = "Salary(RMB)";


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.MultiSelect = false;
            viewEmployee();
            viewSalary();
        }

        public void employeeList(string id, string name, string lname, string age, string gender, string address, string mail, string phone, string des)
        {
            dataGridView1.Rows.Add(id, name, lname, age, gender, address, mail, phone, des);
            dataGridView2.Rows.Add(id, name, lname, age, gender, address, mail, phone, des);
        }
        public void salaryList(string id, string employeeId, string bas, string tax, string bonus, string usd, string rmb)
        {
            dataGridView3.Rows.Add(id, employeeId, bas, tax, bonus, usd, rmb);
          
        }

        private void viewSalary()
        {
            dataGridView3.Rows.Clear();

            string sql = "SELECT * FROM salary";
            cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt_2);

                foreach (DataRow row in dt_2.Rows)
                {
                    salaryList(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString());
                }
                con.Close();
                dt_2.Rows.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }
        private void viewEmployee()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            string sql = "SELECT * FROM employee";
            cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    employeeList(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString());
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

        private void insert(string fname, string lname, string age, string gender, string address, string mail, string phone, string des)
        {
            string sql = "INSERT INTO employee(fname, lname, age, gender, address, mail, phone, designation) values (@FNAME, @LNAME, @AGE, @GENDER, @ADDRESS, @MAIL, @PHONE, @DES)";
            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@FNAME", fname);
            cmd.Parameters.AddWithValue("@LNAME", lname);
            cmd.Parameters.AddWithValue("@AGE", age);
            cmd.Parameters.AddWithValue("@GENDER", gender);
            cmd.Parameters.AddWithValue("@ADDRESS", address);
            cmd.Parameters.AddWithValue("@MAIL", mail);
            cmd.Parameters.AddWithValue("@PHONE", phone);
            cmd.Parameters.AddWithValue("@DES", des);
          
            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Added");
                }
                con.Close();
                viewEmployee();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }
        private void update(int id, string fname, string lname, string age, string gender, string address, string mail, string phone, string des)
        {
            string sql = "UPDATE employee SET fname='" + fname + "',lname='" + lname + "',age='" + age + "',gender='" + gender + "',address='" + address + "',mail='" + mail + "',phone='" + phone + "',designation='" + des + "' WHERE id=" + id + "";
            cmd = new MySqlCommand(sql, con);

            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    
                    MessageBox.Show("Updated");
                }
                con.Close();
                viewEmployee();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }

        private void delete(int id)
        {
            string sql = "DELETE FROM employee WHERE id=" + id + "";
            cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                if (MessageBox.Show("Do you want to delete this data?", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        
                        MessageBox.Show("Deleted");
                    }
                }
                con.Close();
                viewEmployee();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {
            insert(tfname.Text, tlname.Text, tage.Text, tgender.Text, taddress.Text, tmail.Text, tphone.Text, tdesignation.Text);
        }

        private void retreiveTax()
        {
            string sql1 = "SELECT tax_rate FROM tax_type where id =1";
            cmd = new MySqlCommand(sql1, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tax1 = reader[0].ToString();

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
            string sql2 = "SELECT tax_rate FROM tax_type where id=2";
            cmd = new MySqlCommand(sql2, con);

            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tax2 = reader[0].ToString();

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
         }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            tfname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            tlname.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            tage.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            tgender.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            taddress.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            tmail.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            tphone.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            tdesignation.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);            
            update(id, tfname.Text, tlname.Text, tage.Text, tgender.Text, taddress.Text, tmail.Text, tphone.Text, tdesignation.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            String selected = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            delete(id);
        }

        private void btnclc_Click(object sender, EventArgs e)
        {
            Calc();
        }

        private void btnsv_Click(object sender, EventArgs e)
        {
            insertPayroll(tid.Text, tbs.Text, ttax.Text, tbonus.Text, ttotalusd.Text, ttotalrmb.Text);
        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            tid.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void Calc()
        {
            retreiveTax();
            double tax = System.Convert.ToDouble(tax1) + System.Convert.ToDouble(tax2);
            double totalTax;
            int basic = System.Convert.ToInt32(tbs.Text);
            double bonus, totalusd, totalrmb;
            if (cbonus.Text.Equals("A"))
            {
                totalTax = basic * tax / 100;
                ttax.Text = totalTax.ToString();
                bonus = 500;
                tbonus.Text = bonus.ToString();
                totalusd = basic - totalTax + bonus;
                totalrmb = totalusd * 6.2;
                ttotalusd.Text = totalusd.ToString();
                ttotalrmb.Text = totalrmb.ToString();
            }
            else if (cbonus.Text.Equals("B"))
            {
                totalTax = basic * tax / 100;
                ttax.Text = totalTax.ToString();
                bonus = 300;
                tbonus.Text = bonus.ToString();
                totalusd = basic - totalTax + bonus;
                totalrmb = totalusd * 6.2;
                ttotalusd.Text = totalusd.ToString();
                ttotalrmb.Text = totalrmb.ToString();
            } else if (cbonus.Text.Equals("C"))
            {
                totalTax = basic * tax / 100;
                ttax.Text = totalTax.ToString();
                bonus = 100;
                tbonus.Text = bonus.ToString();
                totalusd = basic - totalTax + bonus;
                totalrmb = totalusd * 6.2;
                ttotalusd.Text = totalusd.ToString();
                ttotalrmb.Text = totalrmb.ToString();
            }
            if (cbonus.Text.Equals("D"))
            {
                totalTax = basic * tax / 100;
                ttax.Text = totalTax.ToString();
                bonus = 0;
                tbonus.Text = bonus.ToString();
                totalusd = basic - totalTax + bonus;
                totalrmb = totalusd * 6.2;
                ttotalusd.Text = totalusd.ToString();
                ttotalrmb.Text = totalrmb.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "Employee Management";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            login bbb = new login();
            bbb.Show();
        }

        private void insertPayroll(string id, string basic, string taxes, string bonus, string usd, string rmb)
        {
            string sql = "INSERT INTO salary(employee_id, basic_salary, taxes_amount, bonus_amount, total_usd, total_rmb) values (@ID, @BASIC, @TAXESAMOUNT, @BONUSAMOUNT, @TOTALUSD, @TOTALRMB)";
            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@BASIC", basic);
            cmd.Parameters.AddWithValue("@TAXESAMOUNT", taxes);
            cmd.Parameters.AddWithValue("@BONUSAMOUNT", bonus);
            cmd.Parameters.AddWithValue("@TOTALUSD", usd);
            cmd.Parameters.AddWithValue("@TOTALRMB", rmb);

            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Successful");
                }
                con.Close();
                viewSalary();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                con.Close();
            }
        }


    }
}
