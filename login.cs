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
    public partial class login : Form
    {
        static string ConnectionString = "datasource=127.0.0.1; port=3306; username=root; password=; database=mypayroll; SslMode=none";
        MySqlConnection con = new MySqlConnection(ConnectionString);
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        public static string id;
        public static string pass;
        
        public login()
        {
            InitializeComponent();
        }
        private bool emplogin(string id, string pass)
        {
            string sql = "SELECT * FROM employee WHERE id = @ID and pass = @PASS";
            cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@PASS", pass);
            try
            {
                con.Open();
                MySqlDataReader lg = cmd.ExecuteReader();
                if (lg.Read())
                {
                    con.Close();
                    return true;
                }
                else
                {
                    con.Close();
                    return false;
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void lgnbtn_Click(object sender, EventArgs e)
        {
            id = textBox1.Text;
            pass = textBox2.Text;
            bool lg = emplogin(id, pass);
            if (lg)
            {
                empform emp = new empform();
                emp.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Login Fail");
            }
        }

        private void lgnbtn2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals("giveaccess"))
            {
                Form1 f = new Form1();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Access Denied");
            }
        }
    }
}
