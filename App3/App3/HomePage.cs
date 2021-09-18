using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Linq;
using Android.Telephony;
using Android.Content;
using Xamarin.Essentials;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
namespace App3
{
    [Activity(Label = "Duroc Notifier :: DATA")]
    public class HomePage : AppCompatActivity
    {
        private bool Aborted;

        async Task RequestPermissionAsync()
        {

            var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.Sms>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            var res = grantResults.FirstOrDefault(val => val == Android.Content.PM.Permission.Denied);
            if (res == Android.Content.PM.Permission.Denied)
            {
                Intent _intent = new Intent(this, typeof(MainActivity));
                StartActivity(_intent);
                Toast.MakeText(this, "Permission not granted!\ntry again.", ToastLength.Long).Show();

            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.home);







            FindViewById<Button>(Resource.Id.btnSend).Click += HomePage_Click;

            /*

             var button2 = FindViewById<Button>(Resource.Id.button2);
             button2.Click += async (sender, e) =>
             {



                 /* SmsManager.Default.SendTextMessage("+212663087563", null, "Hello World", null, null);
                  var smsUri = Android.Net.Uri.Parse("smsto:3030");
                  var smsIntent = new Intent(Intent.ActionSendto, smsUri);
                  smsIntent.PutExtra("sms_body", "1");

            await RequestPermissionAsync();
                var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
                if (status != PermissionStatus.Granted)
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    return;
                }

                try
                {
                    Android.Telephony.SmsManager mSmsManager = Android.Telephony.SmsManager.Default;
                    mSmsManager.SendTextMessage("555", "555" +
                        "", "DATA SENT", null, null);
                }
                catch (Exception exx) { }
            };
            */
        }


        void DataSender()
        {


            SqlCommand getdata = new SqlCommand("SELECT ID, Numero, Date, Camion, Client   FROM tempExpedition", Configs.cnx);
            DataTable dt = new DataTable();
            if (Configs.cnx.State == ConnectionState.Open)
                Configs.cnx.Close();
            Configs.cnx.Open();
            dt.Load(getdata.ExecuteReader());
            Configs.cnx.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Android.Telephony.SmsManager mSmsManager = Android.Telephony.SmsManager.Default;
                mSmsManager.SendTextMessage("0677743095", "0677743095" +
                    "", "Expediteur:\n\n-Numero:'" + dt.Rows[i]["ID"].ToString() + "'\n-Date:'" + dt.Rows[i]["Date"].ToString() + "'\n-Camion:'" + dt.Rows[i]["Camion"].ToString() + "'\n-Client:'" + dt.Rows[i]["Client"].ToString() + "'    ", null, null);
                System.Threading.Thread.Sleep(2000);
            }




        }

        void DataSenderDefault()
        {


            Android.Telephony.SmsManager mSmsManager = Android.Telephony.SmsManager.Default;
            mSmsManager.SendTextMessage("555", "555" + "", "DATA SENT", null, null);
        }

        private async void HomePage_Click (object sender, EventArgs e)
        {
            await RequestPermissionAsync();
            var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
            if (status != PermissionStatus.Granted)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                return;
            }
            TextView ServerState = FindViewById<TextView>(Resource.Id.serverstate);

            Thread th = new Thread(new ThreadStart(nobugplz));
            th.Start();

            

            void nobugplz()
             {
                while (true)
                {
                    Thread.Sleep(5000);

                    
                    using (var sqlCommand = new SqlCommand("SELECT COUNT (*) as 'count' FROM users", Configs.cnx))
                    {
                        try
                        {

                            ServerState.Text = "Checking....";

                            if (Configs.cnx.State == ConnectionState.Open)
                                Configs.cnx.Close();
                            Configs.cnx.Open();

                            object reader = sqlCommand.ExecuteScalar();
                            Configs.cnx.Close();
                            if (int.Parse(reader.ToString()) > 0)
                            {

                                ;
                                ServerState.Text = "Sending DATA....";

                                // DataSenderDefault();
                                DataSender();


                                ServerState.Text = "DATA Succefully Sent";


                            }
                            else
                            {
                                ServerState.Text = "NO DATA!";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (Configs.cnx.State == ConnectionState.Open)
                                Configs.cnx.Close();
                            Thread.Sleep(5000);
                        }
                        if (Aborted) break;
                    }

                }

            }


            FindViewById<Button>(Resource.Id.btnStop).Click += btnStop_Click;
            void btnStop_Click(object sender, EventArgs e)
            {
                ServerState.Text = "DISCONNECTED";
                Aborted = true;
                
            }
        }
    }
}