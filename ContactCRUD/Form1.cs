using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ContactCRUD {
    public partial class Form1 : Form {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Visual Studio\ContactCRUD\ContactCRUD\Data\ContactDB.mdf;Integrated Security=True;Connect Timeout=30");
        int ContactId = 0;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            FillDataGridView();
            Reset();
        }

        void FillDataGridView() {
            if (con.State == ConnectionState.Closed) {
                con.Open();
            }
            //When retrieving data: Use SqlDataAdapter object
            SqlDataAdapter sda = new SqlDataAdapter("ContactViewOrSearch", con);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.AddWithValue("@ContactName", txtSearch.Text.Trim());
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContacts.DataSource = dt;
            dgvContacts.Columns[0].Visible = false;
            con.Close();
        }

        void Reset() {
            txtName.Text = txtMobileNumber.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            ContactId = 0;
            btnDelete.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            try {
                if (con.State == ConnectionState.Closed) {
                    con.Open();
                }
                if (btnSave.Text == "Save") {
                    //When inserting data: Use SqlCommand object
                    SqlCommand cmd = new SqlCommand("ContactAddOrEdit", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Add");
                    cmd.Parameters.AddWithValue("@ContactId", 0);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Saved Successfully!", "Save");
                }
                else {
                    SqlCommand cmd = new SqlCommand("ContactAddOrEdit", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Edit");
                    cmd.Parameters.AddWithValue("@ContactId", ContactId);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@MobileNumber", txtMobileNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully!", "Save");
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error Message!");
            }
            finally {
                con.Close();
                FillDataGridView();
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e) {
            try {
                FillDataGridView();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error Message!");
            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e) {
            if (dgvContacts.CurrentRow.Index > -1) {
                ContactId = Convert.ToInt32(dgvContacts.CurrentRow.Cells[0].Value.ToString());
                txtName.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                txtMobileNumber.Text = dgvContacts.CurrentRow.Cells[2].Value.ToString();
                txtAddress.Text = dgvContacts.CurrentRow.Cells[3].Value.ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            try {
                if (con.State == ConnectionState.Closed) {
                    con.Open();
                }
                //When inserting data: Use SqlCommand object
                SqlCommand cmd = new SqlCommand("ContactDeletion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ContactId", ContactId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully!", "Save");
                Reset();
                FillDataGridView();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error Message!");
            }
        }

        private void btnReset_Click(object sender, EventArgs e) {
            Reset();
        }

    }
}
