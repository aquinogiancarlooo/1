﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace LIBRARY
{
    class myconn
    {
        public MySqlConnection con;
        public MySqlDataReader dr;
        public MySqlCommand cmd;
        public DataTable dt;

        public void connect()
        {
            con = new MySqlConnection("datasource=localhost;Database=dblibrary;username=root");
            con.Open();
        }

        public void Disconnect()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
    }
}
