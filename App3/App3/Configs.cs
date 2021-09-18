using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace App3
{
    class Configs
    {

        //FUNCTIONS

        public static SqlConnection cnx = new SqlConnection("Server = 192.168.0.38; Database = HOUSSAM; User Id = sa; Password = 123456; ");


        public static void checkOpencnx()
        {

            if (cnx.State == ConnectionState.Open)
                cnx.Close();
         

        }

        public static void Execute(String Requet)
        {
            checkOpencnx();
            cnx.Open();
            SqlCommand cmd = new SqlCommand(Requet, cnx);
            cmd.ExecuteNonQuery();


        }


        public static bool Existe(String Requet)
        {

            checkOpencnx();
            cnx.Open();
            SqlCommand cmd = new SqlCommand(Requet, cnx);
            var dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
                return true;
            else
                return false;

        }









    }
}