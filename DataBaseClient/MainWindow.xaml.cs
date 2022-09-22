/* CALVIN FRASER
 * CURTIN UNIVERSITY
 * 19921792
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using BusinessServerInterface;
using System.Drawing;
using System.Windows.Interop;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

using RestSharp;
using Newtonsoft.Json;
using Utils;
using DatatierWeb.Models; 

namespace DataBaseClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        private int index = 0;
        RestClient RS;
        SearchData searchData;
        Account data;
        string URL = "";
        Regex rx = new Regex("[^A-Za-z]");
        public MainWindow()
        {
            InitializeComponent();

            data = new Account();
            URL = URLentry.Text;




            RS = new RestClient(URL);
    

            indexEntry.Text = index.ToString();

            searchData = new SearchData();

        }

        public async void generateEntries(object sender, EventArgs e)
        {
            disableGUI();
            Task task = new Task(generate_entries);
            task.Start();
            await task;
            enableGUI();
            return;
        }


        private void generate_entries()
        {
                if (RS != null)
                {
                    try
                    {
                        RestRequest RR = new RestRequest("/api/generate");
                        RestResponse restResponse = RS.Post(RR);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Already generated entries. Please purge first.");
                        return;
                    }
                }
            MessageBox.Show("Generated database entries successfully");
            return;
            
        }

        public  async void purgeEntries(object sender, EventArgs e)
        {
            disableGUI();
            Task task = new Task(purge_entries);
            task.Start();
            await task;
            enableGUI();
            return;
            


        }

        private void purge_entries()
        {
            if (RS != null)
            {
                RestRequest RR = new RestRequest("/api/generate");
                RestResponse restResponse = RS.Delete(RR);

                if (restResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An error has occured. \n" + restResponse.Content);
                    return;
                }
            }
            MessageBox.Show("Emptied database successfully");
            return;
        }

        private void setGUI()
        {
            if (data.icon64 != null)
            {
                

                FNameBox.Text = data.firstName;
                LNameBox.Text = data.lastName;
                BalBox.Text = data.balance.ToString("C");
                PinBox.Text = data.pin.ToString("D4");
                AccBox.Text = data.accountNo.ToString();

                Bitmap bmp = Util.Base64StringToBitmap(data.icon64);


                PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

   
        private void disableGUI()
        {
            GoButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            indexEntry.IsReadOnly = true;
            searchEntry.IsReadOnly = true;
            insert.IsEnabled = false;
            delete.IsEnabled = false;
            generate.IsEnabled = false;
            purge.IsEnabled = false; 
        }

        private void enableGUI()
        {
            GoButton.IsEnabled = true;
            SearchButton.IsEnabled = true;
            indexEntry.IsReadOnly = false;
            searchEntry.IsReadOnly = false;
            insert.IsEnabled = true;
            delete.IsEnabled = true;
            generate.IsEnabled = true;
            purge.IsEnabled = true;

        }

        public void deleteEntry(object sender, EventArgs e)
        {
            RestRequest RR = new RestRequest("/api/getAccount/" +index);
            try
            {
                RestResponse RP = RS.Delete(RR);
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void updateEntry(object sender, EventArgs e)
        {
            RestRequest RR = new RestRequest("/api/getAccount/");
            try
            {
                Account temp = new Account();

                temp.Id = index;
                temp.accountNo = int.Parse(AccBox.Text);
                temp.firstName = FNameBox.Text;
                temp.lastName = LNameBox.Text;
                temp.balance = int.Parse(BalBox.Text, System.Globalization.NumberStyles.Currency);
                temp.pin = int.Parse(PinBox.Text);
                temp.icon64 = data.icon64;

                RR.AddBody(temp);
                RestResponse restResponse = RS.Put(RR);
                if (restResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    MessageBox.Show("Account does not  index already exists");
                }
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(restResponse.Content);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void insertEntry(object sender, EventArgs e)
        {
            RestRequest RR = new RestRequest("/api/getAccount/");
            try
            {
                Account temp = new Account();

                temp.Id = index;
                temp.accountNo = int.Parse(AccBox.Text);
                temp.firstName = FNameBox.Text;
                temp.lastName = LNameBox.Text;
                temp.balance = int.Parse(BalBox.Text, System.Globalization.NumberStyles.Currency);
                temp.pin = int.Parse(PinBox.Text);
                temp.icon64 = data.icon64; 

                RR.AddBody(temp);
                RestResponse restResponse = RS.Post(RR);
                if (restResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    MessageBox.Show("Account of that index already exists");
                }
                if(restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(restResponse.Content);
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private async void HandleClickIndex(object sender, EventArgs e)
        {


                if (int.TryParse(indexEntry.Text, out index))
                {
                    if (index >= 0)
                    {
                        index = int.Parse(indexEntry.Text);


                        Task task = new Task(getIndex);
                        task.Start();
                        disableGUI();
                        await task;
                        setGUI();
                        enableGUI();

                    }
                }
                else
            {
                MessageBox.Show("Please enter a valid number");
            }
        
        }

        private void  getIndex()
        {
 
                RestRequest RR = new RestRequest("api/GetAccount/" + index.ToString());
            try
            {
                RestResponse restResponse = RS.Get(RR);
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<Account>(restResponse.Content);
                }
                else if(restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Accout not found");
                }
                else
                {
                    MessageBox.Show("An unknown error has occured. " + restResponse.ErrorMessage + "\n" + restResponse.StatusDescription);
                }
            }catch(System.Net.Http.HttpRequestException e)
            {
                MessageBox.Show("Error accessing server. \n" + e.Message);
            }
            catch(Exception e)
            {

            }
    

        }

        private async void HandleClickSearch(object sender, EventArgs e)
        {

            string searchTerm = searchEntry.Text;
            if (rx.IsMatch(searchTerm))
            {
                MessageBox.Show("Please check your last name search term. ");
                return;
            }


            searchData.searchString = searchTerm;




            Task task = new Task(getSearch);
            task.Start();

            disableGUI();
            await task;

            setGUI();

            enableGUI();


        }

        private void  getSearch()
        {
            
            RestRequest RR = new RestRequest("api/search/");
            RR.AddJsonBody(JsonConvert.SerializeObject(searchData));
            try
            {
                RestResponse restResponse = RS.Post(RR);
                if (restResponse.IsSuccessful)
                {
                    data = JsonConvert.DeserializeObject<Account>(restResponse.Content);
                }
                else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Account not found.");

                }
                else
                {
                    MessageBox.Show("An unknown error has occured. " + restResponse.ErrorMessage + "\n" + restResponse.StatusDescription);
                }
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                MessageBox.Show(e.Message);
            }
            //return new DataIntermed();


        }

   

        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void searchEntryChanged(object sender, TextChangedEventArgs e)
        {
            URL = URLentry.Text;
            RS = new RestClient(URL);
        }

        private void indexChanged(object sender, TextChangedEventArgs e)
        {
            int tmp = index; 
            if(!int.TryParse(indexEntry.Text, out index))
            {
                MessageBox.Show("Please only enter integers into the index box.");
                index = tmp; 
            }

            
        }
    }
}
