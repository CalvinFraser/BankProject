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
       
        Regex rx = new Regex("[^A-Za-z]");
        public MainWindow()
        {
            InitializeComponent();
            string URL = "http://localhost:52364/";
            RS = new RestClient(URL);
            RestRequest RR = new RestRequest("api/GetTotalValues");
            RestResponse restResponse = RS.Get(RR);

            TotalItemsVal.Content = restResponse.Content;
            totalValues = int.Parse(TotalItemsVal.Content.ToString());

            DataIntermed data = new DataIntermed();

            RR = new RestRequest("api/GetAccount/" + index.ToString());
            restResponse = RS.Get(RR);
            data = JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);
            FNameBox.Text = data.firstName;
            LNameBox.Text = data.lastName;
            BalBox.Text = data.balance.ToString("C");
            PinBox.Text = data.pin.ToString("D4");
            AccBox.Text = data.acctNo.ToString();

            indexEntry.Text = index.ToString();

            searchData = new SearchData();

           // PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(data.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        private async void HandleClickIndex(object sender, EventArgs e)
        {


            if (int.TryParse(indexEntry.Text, out index))
            {
                if (index >= 0 && index < totalValues)
                {
                    index = int.Parse(indexEntry.Text);


                    Task<DataIntermed> task = new Task<DataIntermed>(getIndex);
                    task.Start();

                    GoButton.IsEnabled = false;
                    SearchButton.IsEnabled = false;
                    indexEntry.IsReadOnly = true;
                    searchEntry.IsReadOnly = true;

                    DataIntermed data = await task;



                    FNameBox.Text = data.firstName;
                    LNameBox.Text = data.lastName;
                    BalBox.Text = data.balance.ToString("C");
                    PinBox.Text = data.pin.ToString("D4");
                    AccBox.Text = data.acctNo.ToString();


                    GoButton.IsEnabled = true;
                    SearchButton.IsEnabled = true;
                    indexEntry.IsReadOnly = false;
                    searchEntry.IsReadOnly = false;

                }
            }
        }

        private DataIntermed getIndex()
        {
            RestRequest RR = new RestRequest("api/GetAccount/" + index.ToString());
            RestResponse restResponse = RS.Get(RR);

            return JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);

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




            Task<DataIntermed> task = new Task<DataIntermed>(getSearch);
            task.Start();

            GoButton.IsEnabled = false;
            SearchButton.IsEnabled = false;
            indexEntry.IsReadOnly = true;
            searchEntry.IsReadOnly = true;
            DataIntermed data = await task;

          

            FNameBox.Text = data.firstName;
            LNameBox.Text = data.lastName;
            BalBox.Text = data.balance.ToString("C");
            PinBox.Text = data.pin.ToString("D4");
            AccBox.Text = data.acctNo.ToString();

            GoButton.IsEnabled = true;
            SearchButton.IsEnabled = true;
            indexEntry.IsReadOnly = false;
            searchEntry.IsReadOnly = false;


        }

        private DataIntermed getSearch()
        {
            RestRequest RR = new RestRequest("api/search/");
            RR.AddJsonBody(JsonConvert.SerializeObject(searchData));
            RestResponse restResponse = RS.Post(RR);

            SearchData SD = JsonConvert.DeserializeObject<SearchData>(restResponse.Content);

            //return new DataIntermed();
            return JsonConvert.DeserializeObject<DataIntermed>(restResponse.Content);

        }

    }
}
