using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
namespace LIBRARY
{
    public partial class library : Form
    {
        String mycon = "datasource=localhost;Database=dblibrary;username=root;convert zero datetime=true";
        myconn mc = new myconn();
        String imagepath = "";
        String imagePath = "";
        String filename = "";
        String pics = "";
      

        int qty = 0;

        public library()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
           
        }

         private void library_Load(object sender, EventArgs e)
        {
            DateTime deadline = DateTime.Today.AddDays(5);
            tbdatereturn.Value = deadline;
            databooks();
            borrowtable();

        }

         private void penaltyy()
         {
             int edad = trdatereturned.Value.Day - trdatereturn.Value.Day;
             if(edad > 0 )
             {
                 edad = edad * 10;
             }

             else if (edad < 0)
             {
                 edad = edad * 0;
             }
             
             trpenalty.Text = edad.ToString();

         }


        private void databooks()
        {
            try
            {
                datagridbooks.AutoResizeColumns();
                datagridbooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                datagridbooks.DefaultCellStyle.Font = new Font("Tahoma", 11);

                String Query = "select * from tbinven;";
                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);

                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                datagridbooks.DataSource = dTable;
                datagridbooks.Columns[0].HeaderText = "ISBN";
                datagridbooks.Columns[1].HeaderText = "TITLE";
                datagridbooks.Columns[2].HeaderText = "AUTHOR";
                datagridbooks.Columns[3].HeaderText = "DATE PUBLISHED";
                datagridbooks.Columns[4].HeaderText = "STOCKS";
                datagridbooks.Columns[5].HeaderText = "BOOK FILE PATH";
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void datagridbooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = null;
                foreach (DataGridViewCell selectedCell in datagridbooks.SelectedCells)
                {
                    cell = selectedCell;
                }
                if (cell != null)
                {
                    DataGridViewRow row = cell.OwningRow;
                    tisbn.Text = row.Cells["isbn"].Value.ToString();
                    ttitle.Text = row.Cells["title"].Value.ToString();
                    tauthor.Text = row.Cells["author"].Value.ToString();
                    tdatepub.Text = row.Cells["datepublished"].Value.ToString();
                    tstocks.Text = row.Cells["stock"].Value.ToString();
                    pic.Image = Image.FromFile(row.Cells["path"].Value.ToString());
                    pics = row.Cells["path"].Value.ToString();

                    openFileDialog1.FileName = pics;
                    openFileDialog1.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                    if (openFileDialog1.FileName.ToString() != "")
                    {
                        imagePath = openFileDialog1.FileName.ToString();
                        imagepath = imagePath.ToString();
                        imagepath = imagepath.Substring(imagepath.LastIndexOf("\\"));
                        imagepath = imagepath.Remove(0, 1);
                    }

                    pics = "C:\\\\POS\\\\images\\\\" + imagepath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                pic.Image = Image.FromFile(@"C:/POS/images/Not Available.jpg");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bookpic();
        }

        private void bookpic()
        {
            OpenFileDialog open = new OpenFileDialog();

            open.FileName = "";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            
            if (openFileDialog1.FileName.ToString() != "")
            {
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pic.Image = new Bitmap(open.FileName);

                    imagePath = open.FileName.ToString();
                    imagepath = imagePath.ToString();
                    imagepath = imagepath.Substring(imagepath.LastIndexOf("\\"));
                    imagepath = imagepath.Remove(0, 1);
                }
            }

            pics = "C:\\\\POS\\\\images\\\\" + imagepath;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            tisbn.Text = "";
            ttitle.Text = "";
            tauthor.Text = "";
            tdatepub.Text = DateTime.Now.ToShortDateString();
            tstocks.Text = "";
            pic.Image = Image.FromFile(@"C:/POS/images/Not Available.jpg");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                String Query = "insert into tbinven values('" +
                    this.tisbn.Text + "', '" +
                    this.ttitle.Text + "', '" +
                    this.tauthor.Text + "', '" +
                    this.tdatepub.Text + "', '" +
                    this.tstocks.Text + "', '" + pics + "');";

                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataReader MyReader2;
                MyConn.Open();
                MyReader2 = MyCommand.ExecuteReader(); MessageBox.Show("New Book Has Been Added");
                databooks();

                MyConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ISBN HAS ALREADY USED");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result1 = MessageBox.Show("Are you sure you want to remove this product?", "Important Question", MessageBoxButtons.YesNo);
                String Query = "";
                if (result1.Equals(DialogResult.Yes))
                {
                    Query = "delete from tbinven where isbn='" + tisbn.Text + "';";
                }
                else
                {
                    MessageBox.Show("Nothing Changed");
                }

                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataReader MyReader2;
                MyConn.Open();
                MyReader2 = MyCommand.ExecuteReader();
                MessageBox.Show("Data has been deleted");
                databooks();
                MyConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Records Deleted");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                String Query = "update tbinven set isbn = '" + this.tisbn.Text +
                    "', title = '" + this.ttitle.Text + "', stock = '" + this.tstocks.Text + "', author = '" + this.tauthor.Text +
                    "', datepublished = '" + this.tdatepub.Text + "', path = '" + pics + "' where isbn = '" + tisbn.Text + "';";
                MySqlConnection MyConn = new MySqlConnection(mycon);

                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataReader MyReader2;
                MyConn.Open();
                MyReader2 = MyCommand.ExecuteReader();
                MessageBox.Show("Product Info Has Been Updated");
                databooks();
                MyConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Products To Update");
            }
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str = cbsearch.SelectedItem.ToString();
                String Query = "";

                if (str.Equals("ISBN"))
                {
                    str = "isbn";
                }
                else if (str.Equals("TITLE"))
                {
                    str = "title";
                }
                else if (str.Equals("AUTHOR"))
                {
                    str = "author";
                }
                else if (str.Equals("STOCK"))
                {
                    str = "stock";
                }
                else
                {
                    str = "datepublished";
                }

                Query = "select * from tbinven where " + str + " like '" + "%" + tsearch.Text + "%" + "';";
                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                datagridbooks.DataSource = dTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select category and type the keyword you want to search");
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            try
            {

                mc.connect();
                mc.cmd = new MySqlCommand("select * from tbinven where isbn = @isbn", mc.con);

                mc.cmd.Parameters.Add(new MySqlParameter("isbn", tbisbn.Text));
                mc.dr = mc.cmd.ExecuteReader();
                if (mc.dr.Read())
                {
                    tbtitle.Text = mc.dr.GetValue(1).ToString();
                    availquant.Text = mc.dr.GetValue(4).ToString();
                    pic1.Image = Image.FromFile(mc.dr.GetValue(5).ToString());
                    
                }
                else
                {
                    MessageBox.Show("Product Not Found", "Invalid Search!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                mc.Disconnect();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void borrowtable()
        {
            try
            {
                datagridborrow.AutoResizeColumns();
                datagridborrow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                datagridborrow.DefaultCellStyle.Font = new Font("Tahoma", 11);

                String Query = "select * from tbborrow;";
                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);

                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                datagridborrow.DataSource = dTable;
                datagridborrow.Columns[0].HeaderText = "TRANSACTION NUMBER";
                datagridborrow.Columns[1].HeaderText = "ISBN";
                datagridborrow.Columns[2].HeaderText = "TITLE";
                datagridborrow.Columns[3].HeaderText = "NAME OF BORROWER";
                datagridborrow.Columns[4].HeaderText = "DATE BORROW";
                datagridborrow.Columns[5].HeaderText = "DATE OF RETURN";
                datagridborrow.Columns[6].HeaderText = "BOOK QUANTITY";
                datagridborrow.Columns[7].HeaderText = "DATE FILE PATH";
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbisbn.Text.Equals("") || tbborrowname.Text.Equals("") || tbquant.Text.Equals(""))
                {
                    MessageBox.Show("Do Not Leave Blank Fields");
                }
                else
                {
                    
                    String Query = "insert into tbborrow values('" +
                        this.transacnumber.Text + "', '" +
                        this.tbisbn.Text + "', '" +
                        this.tbtitle.Text + "', '" +
                        this.tbborrowname.Text + "', '" +
                         this.tbdateborrow.Text + "', '" +
                          this.tbdatereturn.Text + "', '" +
                        this.tbquant.Text + "', '" + "');";

                    MySqlConnection MyConn = new MySqlConnection(mycon);
                    MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                    MySqlDataReader MyReader2;
                    MyConn.Open();
                    MyReader2 = MyCommand.ExecuteReader();
                    MessageBox.Show("Product has been added");
                    borrowtable();
                    lesstoinventory();
                    MyConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void lesstoinventory()
        {
            try
            {
                qty = Convert.ToInt32(tbquant.Text);
                String Query = "update tbinven set stock = (stock - '" + qty + "') where isbn in (select isbn from (select isbn from tbinven where isbn = '" + tbisbn.Text + "') as t);";
                MySqlConnection MyConn = new MySqlConnection(mycon);

                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataReader MyReader2;
                MyConn.Open();
                MyReader2 = MyCommand.ExecuteReader();
                MessageBox.Show("Product Info Has Been Updated");
                borrowtable();
                databooks();
                MyConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell cell = null;
                foreach (DataGridViewCell selectedCell in datagridborrow.SelectedCells)
                {
                    cell = selectedCell;
                }
                if (cell != null)
                {
                    DataGridViewRow row = cell.OwningRow;
                    transacnumber.Text = row.Cells["transactionnumber"].Value.ToString();
                    trtransac.Text = row.Cells["transactionnumber"].Value.ToString();
                    trisbn.Text = row.Cells["brisbn"].Value.ToString();
                    trtitle.Text = row.Cells["brtitle"].Value.ToString();
                    trborrowname.Text = row.Cells["brnameofborrower"].Value.ToString();
                    trdateborrow.Text = row.Cells["brdateborrow"].Value.ToString();
                    trdatereturn.Text = row.Cells["brdateofreturn"].Value.ToString();

                   /* try
                    {
                        mc.connect();
                        mc.cmd = new MySqlCommand("select * from tbinven where isbn = @isbn", mc.con);

                        mc.cmd.Parameters.Add(new MySqlParameter("prodcode", txtprodcode2.Text));
                        mc.dr = mc.cmd.ExecuteReader();
                        if (mc.dr.Read())
                        {
                            txtprodname2.Text = mc.dr.GetValue(1).ToString();
                            pictureBox2.Image = Image.FromFile(mc.dr.GetValue(5).ToString());
                            price = Convert.ToDouble(mc.dr.GetValue(4).ToString());
                        }
                        else
                        {
                            MessageBox.Show("Product Not Found", "Invalid Search!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        mc.Disconnect();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }*/

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            penaltyy();
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            penaltyy();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result1 = MessageBox.Show("Are you sure you want to remove this product?", "Important Question", MessageBoxButtons.YesNo);
                String Query = "";
                if (result1.Equals(DialogResult.Yes))
                {
                    Query = "delete from tbborrow where transactionnumber = '" + transacnumber.Text + "';";
                }
                else
                {
                    MessageBox.Show("Nothing Changed");
                }

                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataReader MyReader2;
                MyConn.Open();
                MyReader2 = MyCommand.ExecuteReader();
                MessageBox.Show("Data has been deleted");
                borrowtable();
                    databooks();
                
                
                addtoinventory();
               
                MyConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

           private void addtoinventory()
        {
            try
            {
                qty = Convert.ToInt32(tbquant.Text);
                String Query = "update tbinven set stock = (stock + '" + qty + "') where isbn in (select isbn from (select isbn from tbinven where isbn = '" + tbisbn.Text + "') as t);";
                MySqlConnection MyConn = new MySqlConnection(mycon);

                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                MySqlDataReader MyReader2;
                MyConn.Open();
                MyReader2 = MyCommand.ExecuteReader();
                MessageBox.Show("Product Info Has Been Updated");
                databooks();
                MyConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

           private void pic1_Click(object sender, EventArgs e)
           {

           }

           private void pic_Click(object sender, EventArgs e)
           {

           }

           private void button7_Click(object sender, EventArgs e)
           {
               tbisbn.Text = "";
               tbtitle.Text = "";
               tbborrowname.Text = "";
               tbdateborrow.Text = DateTime.Now.ToShortDateString();
               tbdatereturn.Text = DateTime.Now.ToShortDateString();
               tbquant.Text = "";
               availquant.Text = "";
               pic1.Image = Image.FromFile(@"C:/POS/images/Not Available.jpg");
           }

           private void button11_Click(object sender, EventArgs e)
           {
               trtransac.Text = "";
               trisbn.Text = "";
               trtitle.Text = "";
               trborrowname.Text = "";
               trpenalty.Text = "";
               trdateborrow.Text = DateTime.Now.ToShortDateString();
               trdatereturn.Text = DateTime.Now.ToShortDateString();
               trdatereturned.Text = DateTime.Now.ToShortDateString();
               pic2.Image = Image.FromFile(@"C:/POS/images/Not Available.jpg");
           }

           private void button10_Click(object sender, EventArgs e)
           {
               try
               {
                   String Query = "insert into tbreturn values('" +
                       this.trtransac.Text + "', '" +
                        this.trisbn.Text + "', '" +
                       this.trtitle.Text + "', '" +
                       this.trborrowname.Text + "', '" +
                        this.trdateborrow.Text + "', '" +
                         this.trdatereturn.Text + "', '" +
                       this.trdatereturned.Text + "', '" +
                       this.trpenalty.Text + "', '"  + "');";

                   MySqlConnection MyConn = new MySqlConnection(mycon);
                   MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                   MySqlDataReader MyReader2;
                   MyConn.Open();
                   MyReader2 = MyCommand.ExecuteReader(); MessageBox.Show("New Book Has Been Added");
                   databooks();

                   MyConn.Close();



               }
               catch (Exception ex)
               {
                   MessageBox.Show("ISBN HAS ALREADY USED");
               }
           }
        
        private void tablereturn()
        {
            try
            {
                datagridbooks.AutoResizeColumns();
                datagridbooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                datagridbooks.DefaultCellStyle.Font = new Font("Tahoma", 11);

                String Query = "select * from tbreturn;";
                MySqlConnection MyConn = new MySqlConnection(mycon);
                MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);

                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                datagridbooks.DataSource = dTable;
                datagridbooks.Columns[0].HeaderText = "ISBN";
                datagridbooks.Columns[1].HeaderText = "TITLE";
                datagridbooks.Columns[2].HeaderText = "AUTHOR";
                datagridbooks.Columns[3].HeaderText = "DATE PUBLISHED";
                datagridbooks.Columns[4].HeaderText = "STOCKS";
                datagridbooks.Columns[5].HeaderText = "BOOK FILE PATH";
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       




















    }
}
