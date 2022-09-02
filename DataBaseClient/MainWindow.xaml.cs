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
        int totalValues = 0;
        DataIntermed data;
        string URL = "";
        Regex rx = new Regex("[^A-Za-z]");
        public MainWindow()
        {
            InitializeComponent();

            data = new DataIntermed();
            URL = URLentry.Text;




            RS = new RestClient(URL);

            RestRequest RR = new RestRequest("api/GetTotalValues");
            try
            {
                RestResponse restResponse = RS.Get(RR);


                TotalItemsVal.Content = restResponse.Content;
                totalValues = int.Parse(TotalItemsVal.Content.ToString());

                RR = new RestRequest("api/GetAccount/0");
                restResponse = RS.Get(RR);
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);
                    setGUI();
                }
                else
                {
                    MessageBox.Show("Please check server connection");
                }
                 

            }
            catch(System.Net.Http.HttpRequestException e)
            {
                MessageBox.Show("Please enter a valid URL\n" + e.Message);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
    

            indexEntry.Text = index.ToString();

            searchData = new SearchData();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void setGUI()
        {
            FNameBox.Text = data.firstName;
            LNameBox.Text = data.lastName;
            BalBox.Text = data.balance.ToString("C");
            PinBox.Text = data.pin.ToString("D4");
            AccBox.Text = data.acctNo.ToString();

            Bitmap bmp = Util.Base64StringToBitmap(data.icon64);


            PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        private void disableGUI()
        {
            GoButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            indexEntry.IsReadOnly = true;
            searchEntry.IsReadOnly = true;
        }

        private void enableGUI()
        {
            GoButton.IsEnabled = true;
            SearchButton.IsEnabled = true;
            indexEntry.IsReadOnly = false;
            searchEntry.IsReadOnly = false;
        }

        private async void HandleClickIndex(object sender, EventArgs e)
        {


                if (int.TryParse(indexEntry.Text, out index))
                {
                    if (index >= 0 && index < totalValues)
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
                    data = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);
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
                    data = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);
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

        private void searchEntryChanged(object sender, TextChangedEventArgs e)
        {
            URL = URLentry.Text;
            RS = new RestClient(URL);
        }
    }
}
