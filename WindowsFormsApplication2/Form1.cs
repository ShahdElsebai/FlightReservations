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
using System.Configuration;
using Tulpep.NotificationWindow;

namespace WindowsFormsApplication2
{

    public partial class Form1 : Form
    {
        int num_seats;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-8PKNO1H\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True ");
        public Form1()
        {

            InitializeComponent();
            p_cancle.Visible = false;
            p_result.Visible = false;
            p_search.Visible = false;
            p_signup.Visible = false;
            p_functions.Visible = false;
            p_update.Visible = false;
            p_all_flights.Visible = false;
        }
       
        public void search()
        {
            num_seats = Convert.ToInt32(t_num_seats.Text);
            bool check = false;

            t_from.CharacterCasing = CharacterCasing.Lower;
            t_to.CharacterCasing = CharacterCasing.Lower;
            con.Open();

            SqlCommand cmd = new SqlCommand("Select Flight_id, Departure,Distnation,Price,Airline_logo_path,Number_Seat_Avaliable,Departure_Time,Flight_time,direct from Flight", con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable tbl_Flight = new DataTable();
            tbl_Flight.Columns.Add("Airline_logo_path");
            tbl_Flight.Columns.Add("Departure");
            tbl_Flight.Columns.Add("Distnation");
            tbl_Flight.Columns.Add("Price");
            tbl_Flight.Columns.Add("Flight_id");
            tbl_Flight.Columns.Add("Flight_time");
            DataRow row;
            if (rb_direct.Checked == true)
            {
                while (reader.Read())
                {

                 
                    if (t_from.Text == (string)reader["Departure"] && t_to.Text == (string)reader["Distnation"] && Convert.ToInt32(t_num_seats.Text) <= (int)reader["Number_Seat_Avaliable"] && Convert.ToDateTime(dtp_Distnation.Text) == (DateTime)reader["Departure_Time"]&&(bool)reader["direct"]==true)
                    {

                        row = tbl_Flight.NewRow();
                        row["Flight_id"] = reader["Flight_id"];
                        row["Departure"] = reader["Departure"];
                        row["Distnation"] = reader["Distnation"];
                        row["Price"] = reader["Price"];
                        row["Airline_logo_path"] = reader["Airline_logo_path"];
                        row["Flight_time"] = reader["Flight_time"];
                        tbl_Flight.Rows.Add(row);
                        check = true;
                    }
                }
            }
            else if (rb_indirect.Checked == true)
            {
                while (reader.Read())
                {
                    t_from.CharacterCasing = CharacterCasing.Lower;
                    t_to.CharacterCasing = CharacterCasing.Lower;


                    if (t_from.Text == (string)reader["Departure"] && t_to.Text == (string)reader["Distnation"] && Convert.ToInt32(t_num_seats.Text) <= (int)reader["Number_Seat_Avaliable"] && Convert.ToDateTime(dtp_Distnation.Text) == (DateTime)reader["Departure_Time"] && (bool)reader["direct"] == false)
                    {

                        row = tbl_Flight.NewRow();
                        row["Flight_id"] = reader["Flight_id"];
                        row["Departure"] = reader["Departure"];
                        row["Distnation"] = reader["Distnation"];
                        row["Price"] = reader["Price"];
                        row["Airline_logo_path"] = reader["Airline_logo_path"];
                        row["Flight_time"] = reader["Flight_time"];
                        tbl_Flight.Rows.Add(row);
                        check = true;
                    }
                }
            }

            reader.Close();
            con.Close();

            dgv.DataSource = tbl_Flight;


            if (check)
            {
                p_search.Visible = false;
                p_result.Visible = true;
                t_from.Text = " ";
                t_to.Text = " ";
                t_num_seats.Text = " ";

            }
            else
            {
                PopupNotifier notification = new PopupNotifier();
                notification.Image = Properties.Resources.info;
                notification.TitleText = "SkyScanner";

                notification.ContentText = "No Avialable Flight";
                notification.Popup();
            }
        }
        public void reserve()
        {

            SqlConnection con = new SqlConnection("Data Source=DESKTOP-8PKNO1H\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True ");
            con.Open();
            // int nos=Convert.ToInt32(t_num_seats.Text);
            bool Direct = false;
            if (rb_direct.Checked == true)
            {


                Direct = true;}
            else
            {
                Direct = false;
            }

            string insertstring = @"insert into flight_reservation ([flight_id],[customer_id],[numofseats],[direct]) values 
((select flight_id from flight where flight_id='" + t_flightId.Text + "'),(select customer_id from customer where User_Name=@un),@nos,@Direct)";
         /*   if (t_un.Text != t_name.Text && t_un.Text != t_username.Text)
            {
                MessageBox.Show("Incorrect UserName");
            }
          3ayzen na4ofe ane username hwa ely da5alo f login
            else
            {*/
                SqlCommand d = new SqlCommand(insertstring, con);
                SqlParameter userName = new SqlParameter("@un", t_un.Text);
                SqlParameter numofSeat = new SqlParameter("@nos", Convert.ToString(num_seats));
                SqlParameter directFlight = new SqlParameter("@Direct", Direct);
                d.Parameters.Add(userName);
                d.Parameters.Add(directFlight);
                d.Parameters.Add( numofSeat);
               
               
                d.ExecuteNonQuery();
           // }
            con.Close();


            //task israa haysagal el baynat f el table w yan2sa el count 1
            PopupNotifier notification = new PopupNotifier();
            notification.Image = Properties.Resources.info;
            notification.TitleText = "SkyScanner";
            //if (ane el data atsagalt f el table w el 7agz tam bangaa7)
            notification.ContentText = "Accepted";
            notification.Popup();

        }
         public void dispalay_ALL()
        {
           
            con.Open();
            SqlCommand cmd = new SqlCommand("Select Flight_id, Departure,Distnation,Price,Airline_logo_path,Number_Seat_Avaliable,Departure_Time,Flight_time from Flight", con);
          cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable tbl_All_Flight = new DataTable();
            tbl_All_Flight.Columns.Add("Airline_logo_path");
            tbl_All_Flight.Columns.Add("Departure");
            tbl_All_Flight.Columns.Add("Distnation");
            tbl_All_Flight.Columns.Add("Price");
            tbl_All_Flight.Columns.Add("Flight_id");
            tbl_All_Flight.Columns.Add("Flight_time");

            DataRow row;
            while (reader.Read())
            {
                
                    row = tbl_All_Flight.NewRow();
                    row["Flight_id"] = reader["Flight_id"];
                    row["Departure"] = reader["Departure"];
                    row["Distnation"] = reader["Distnation"];
                    row["Price"] = reader["Price"];
                    row["Airline_logo_path"] = reader["Airline_logo_path"];
                    row["Flight_time"] = reader["Flight_time"];
                    tbl_All_Flight.Rows.Add(row);
                   
            }

            reader.Close();
            con.Close();

            dgv_all_flights.DataSource = tbl_All_Flight;


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
            rb_female.Checked = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void b_search_Click(object sender, EventArgs e)
        {
           search();

           
        }

        private void b_back_Click(object sender, EventArgs e)
        {
            p_result.Visible = false;
            p_search.Visible = true;
        }

        private void dtp_Distnation_ValueChanged(object sender, EventArgs e)
        {
            if (dtp_Distnation.Value == DateTime.Now || dtp_Distnation.Value > DateTime.Now)
            {
                dtp_Distnation.CustomFormat = "dd-MM-yyyy";
            }
            else
            {
                MessageBox.Show("The date you enterd does not exist", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtp_Distnation.Value = DateTime.Now;
            }
        }

        private void p_search_Paint(object sender, PaintEventArgs e)
        {

        }

        private void b_required_Click(object sender, EventArgs e)
        {
            reserve();
        }
        //3al4an alzam user yad5al number f textbox
        private void t_num_seats_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            //ch!=8 mean ch!=backspace
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void t_num_seats_TextChanged(object sender, EventArgs e)
        {

        }

        private void p_login_signup_Paint(object sender, PaintEventArgs e)
        {

        }

        private void p_signup_Paint(object sender, PaintEventArgs e)
        {

        }

        private void p_signup_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void p_login_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void b_login_Click(object sender, EventArgs e)
        {
           
            string confg = "Data Source=DESKTOP-8PKNO1H\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(confg))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT  dbo.Log_In(@User_UserName,@User_Password)", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@User_UserName", t_name.Text);
                    cmd.Parameters.AddWithValue("@User_Password", t_pass.Text);
                    string f = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                   if(f=="EXIST")
                   {
                       p_login.Visible = false;
                       p_functions.Visible = true;
                       t_pass.Text = " ";
                       t_name.Text = " ";


                   }
                   else
                   {
                       MessageBox.Show("Incorrect Data");
                   }

                   
                }
            }
        }

        private void b_c_reserve_Click(object sender, EventArgs e)
        {
            p_cancle.Visible = false;
            p_all_flights.Visible = false;
            p_result.Visible = false;
            p_update.Visible = false;
            p_search.Visible=true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            p_update.Visible = true;
            p_all_flights.Visible = false;
            p_result.Visible = false;
            p_cancle.Visible = false;
            p_search.Visible = false;
            
        }

        private void b_logout_Click(object sender, EventArgs e)
        {
            p_functions.Visible = false;
            p_result.Visible = false;
            p_search.Visible = false;
            p_login.Visible = true;
           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void p_all_flights_Paint(object sender, PaintEventArgs e)
        {

        }

        private void b_all_flights_Click(object sender, EventArgs e)
        {
           
            dispalay_ALL();
            p_all_flights.Visible = true;
            p_result.Visible = false;
            p_search.Visible = false;
            p_update.Visible = false;
            p_cancle.Visible = false;
        }

        private void b_bback_Click(object sender, EventArgs e)
        {
            p_all_flights.Visible = false;
            p_functions.Visible = true;
        }

        private void b_c_login_Click(object sender, EventArgs e)
        {
            p_login.Visible = true;
           
        }

        private void b_c_signup_Click(object sender, EventArgs e)
        {
            p_signup.Visible = true;
           
        }

        private void b_back_s_f_Click(object sender, EventArgs e)
        {
            p_search.Visible = false;
            p_functions.Visible = true;
        }

        private void b_back_l_w_Click(object sender, EventArgs e)
        {
            p_login.Visible = false;
           
            t_pass.Text = " ";
            t_name.Text = " ";

        }

        private void b_back_s_w_Click(object sender, EventArgs e)
        {
           p_signup.Visible = false ;
           p_login.Visible = true;
            t_email.Text = " ";
            t_firstname.Text =" ";
            t_lastname.Text = " ";
            t_middlename.Text = " ";
            t_nationality.Text = " ";
           t_password.Text = " ";
           t_passport_num.Text = " ";
           t_username.Text = " ";
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void p_functions_Paint(object sender, PaintEventArgs e)
        {

        }

        private void b_exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void b_cancle_Click(object sender, EventArgs e)
        {
            p_update.Visible = false;
            p_cancle.Visible = true;
            p_login.Visible = false;
            p_result.Visible = false;
            p_search.Visible = false;
            p_signup.Visible = false;
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-8PKNO1H\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True ");
            con.Open();
            SqlCommand cmd2 = new SqlCommand("Select customer_id,flight_id,numofseats,direct from flight_reservation", con);


            SqlDataReader reader = cmd2.ExecuteReader();
            DataTable tbl_history_reservation = new DataTable();
            tbl_history_reservation.Columns.Add("customer_id");
            tbl_history_reservation.Columns.Add("flight_id");
            tbl_history_reservation.Columns.Add("Num_Of_Seat");
            tbl_history_reservation.Columns.Add("direct");

            DataRow row;
            SqlCommand m = new SqlCommand(@"select customer_id from Customer where User_Name=t_name.Text", con);
            int x = 4;// Convert.ToInt32(m.ExecuteScalar());

            while (reader.Read())
            {



                if (x == (int)reader["Customer_id"])
                {

                    row = tbl_history_reservation.NewRow();
                    row["customer_id"] = reader["customer_id"];
                    row["flight_id"] = reader["flight_id"];
                    row["Num_Of_Seat"] = reader["numofseats"];
                    row["direct"] = reader["direct"];

                    tbl_history_reservation.Rows.Add(row);

                }

            }
            reader.Close();


            dgv_cancle.DataSource = tbl_history_reservation;
            con.Close();

        }

        private void p_signup_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_signup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            p_signup.Visible = true;
            p_login.Visible = false;
            p_cancle.Visible = false;
        }

        private void rb_direct_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             int index = e.RowIndex;
            DataGridViewRow SELECTEDROW = dgv.Rows[index];
            t_flightId.Text = SELECTEDROW.Cells[4].Value.ToString();
        
        }

        private void b_cancle_reserve_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-8PKNO1H\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True ");
            con.Open();
            SqlCommand cmd = new SqlCommand("CancaleReservation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Num_Of_Seat",t_n.Text);
            cmd.Parameters.AddWithValue("@customer_id", t_c_id.Text);
            cmd.Parameters.AddWithValue("@flight_id", t_f_id.Text);
            cmd.Parameters.AddWithValue("@direct", t_direct.Text);
             cmd.ExecuteNonQuery();
            
             MessageBox.Show("CANCELATION IS COMPLETE");
             con.Close();

        }

        private void dgv_cancle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow SELECTEDROW = dgv_cancle.Rows[index];
            t_c_id.Text = SELECTEDROW.Cells[0].Value.ToString();
            t_f_id.Text = SELECTEDROW.Cells[1].Value.ToString();
            t_n.Text = SELECTEDROW.Cells[2].Value.ToString();
            t_direct.Text = SELECTEDROW.Cells[3].Value.ToString();

        }

        private void b_update_done_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-8PKNO1H\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True");
            con.Open();
            string getId = @"select customer_id from customer where username='" + t_U_n.Text + "'";
            string updatestring = @"update flight_reservation set flight_id=@fid,numofseats=@nos where customer_id=@x ";
            string olds = @"update flight set Number_Seat_Avaliable+='" + t_onos.Text + "' where flight_id='" + t_ofi.Text + "'";
            string news = @"update flight set Number_Seat_Avaliable-='" + t_nnos.Text + "'where flight_id='" + t_nfi.Text + "'";
            SqlCommand cmd = new SqlCommand(updatestring, con);
            SqlParameter paramfid = new SqlParameter("@fid", t_nfi.Text);
            cmd.Parameters.Add(paramfid);
            SqlParameter paramseat = new SqlParameter("@nos", t_nnos.Text);
            cmd.Parameters.Add(paramseat);
            SqlCommand c = new SqlCommand(olds, con);

            SqlCommand m = new SqlCommand(news, con);

            SqlCommand p = new SqlCommand(getId, con);
            p.CommandType = CommandType.Text;
            int x = Convert.ToInt32(p.ExecuteScalar());
            cmd.Parameters.AddWithValue("@x", x);
            c.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            m.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("updated");
        }

        private void t_onos_TextChanged(object sender, EventArgs e)
        {

        }

        private void b_signup_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT dbo.User_Name_Exists(@User_UserName)", con);
            cmd.Parameters.AddWithValue("@User_UserName", t_username.Text);
            string name = Convert.ToString(cmd.ExecuteScalar());
            if (name == "EXIST")
            {
                MessageBox.Show("USERNAME ALREADY EXISTS");
            }
            else
            {
                SqlCommand REGESTIRE = new SqlCommand("regestire", con);
                REGESTIRE.CommandType = CommandType.StoredProcedure;
                REGESTIRE.Parameters.AddWithValue("@User_UserName", t_username.Text);
                REGESTIRE.Parameters.AddWithValue("@User_Password", t_password.Text);
                REGESTIRE.Parameters.AddWithValue("@User_FirstName", t_firstname.Text);
                REGESTIRE.Parameters.AddWithValue("@User_MiddleName", t_middlename.Text);
                REGESTIRE.Parameters.AddWithValue("@User_LastName", t_lastname.Text);
                REGESTIRE.Parameters.AddWithValue("@User_PassportNum", t_passport_num.Text);
                REGESTIRE.Parameters.AddWithValue("@User_Email", t_email.Text);
                REGESTIRE.Parameters.AddWithValue("@Nationality", t_nationality.Text);
                string value = "";
                bool isChecked = rb_female.Checked;
                if (isChecked)
                    value = rb_female.Text;
                else
                    value = rb_male.Text;
                REGESTIRE.Parameters.AddWithValue("@User_Gender", value);
                REGESTIRE.Parameters.AddWithValue("@NetworkCode", listBox_code.Text);
                REGESTIRE.Parameters.AddWithValue("@PhoneNum", t_phone.Text);
                REGESTIRE.ExecuteNonQuery();
                MessageBox.Show("REGESTERATION COMPLETE");

            }
            con.Close();

            
        }

        private void b_back_s_w_Click_1(object sender, EventArgs e)
        {
            p_signup.Visible = false;
            p_login.Visible = true;
        }

        private void b_signup_Click_1(object sender, EventArgs e)
        {

        }

    }
}