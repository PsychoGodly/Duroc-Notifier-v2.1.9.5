using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Data;
using System.Data.SqlClient;


namespace App3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            FindViewById<Button>(Resource.Id.button1).Click += button1_Click;
            FindViewById<Button>(Resource.Id.button2).Click += button2_Click;

        }






        




        //LOG IN BUTTON





        private void button1_Click(object sender, EventArgs e)
        {
            /* Intent _intent = new Intent(this, typeof(HomePage));
             StartActivity(_intent);
             return;*/
            EditText txtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            string username = txtUserName.Text;

            EditText txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            string password = txtPassword.Text;
            //login(username, password);



           if( Configs.Existe("SELECT * FROM users WHERE username='"+username+"'AND password='"+password+"'")==true)
            {
                Intent _intent = new Intent(this, typeof(HomePage));
                StartActivity(_intent);

            }

           else
            {
                Toast.MakeText(this, "Invalid Enteries!", ToastLength.Long).Show();
            }






            




        }
        //void login(String txtUserName, String txtPassword)
        /*{
            SqlConnection signup = new SqlConnection("Server = 192.168.0.38; Database = HOUSSAM; User Id = sa; Password = 123456; ");
            try
            {

                signup.Open();
                SqlDataReader dr;



                SqlCommand cmd = new SqlCommand("select username,password from users where username@user and password@pass", signup);
                    dr = cmd.ExecuteReader();
                while (dr.Read());
                {


                    txtUserName = dr.GetString(0);
                    txtPassword = dr.GetString(0);


                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                signup.Close();
            }

           /* Intent _intent = new Intent(this, typeof(HomePage));
            StartActivity(_intent);*/

        //}*/






        //SIGN UP BUTTON


        private void button2_Click(object sender, EventArgs e)
        {
            EditText username = FindViewById<EditText>(Resource.Id.txtUserName);
            EditText password = FindViewById<EditText>(Resource.Id.txtPassword);
            InsertInfo(username.Text, password.Text);
        }
        void InsertInfo(String txtUserName, String txtPassword)
        {
            SqlConnection signup = new SqlConnection("Server = 192.168.0.38; Database = HOUSSAM; User Id = sa; Password = 123456; ");
            try
            {
                
                SqlCommand cmd = new SqlCommand(@$"Insert into users(username,password) values('{txtUserName}', '{txtPassword}')", signup);

                signup.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                
                    Toast.MakeText(this, "Succesfully Signed up!", ToastLength.Long).Show();

            
               
                signup.Close();
                

            }
           

        }








        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }

    }
}

