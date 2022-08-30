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

namespace DataBaseClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private BusinessServerInterface.BusinessServerInterface channel;
        private int index = 0;

        private delegate Person Search(String search);
        private Search search;
        public string searchTerm;

        Regex rx = new Regex("[^A-Za-z]");
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<BusinessServerInterface.BusinessServerInterface> channelFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8101/BusinessService";

            channelFactory = new ChannelFactory<BusinessServerInterface.BusinessServerInterface>(tcp, URL);
            channel = channelFactory.CreateChannel();


            TotalItemsVal.Content = channel.getNumEntires();
            getDataIndex(index);
            indexEntry.Text = "0";

            status.Visibility = Visibility.Collapsed;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void getDataIndex(int index)
        {
            try
            {

                string fName = "", lName = "";
                int bal = 0;
                uint acct = 0, pin = 0;
                Bitmap icon = null;


                channel.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out icon); //RPC STUFF
                FNameBox.Text = fName;
                LNameBox.Text = lName;
                BalBox.Text = bal.ToString("C");
                AccBox.Text = acct.ToString();
                PinBox.Text = pin.ToString("D4");
                //Taken from: https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource
                PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                icon.Dispose();

            }
            catch (FaultException<AccountNotFoundFault> exception)
            {
                MessageBox.Show(exception.Detail.Issue);
            }
        }

        private async void HandleClickSearch(object sender, RoutedEventArgs e)
        {
            

            searchTerm = searchEntry.Text;
            if(rx.IsMatch(searchTerm))
            {
                MessageBox.Show("Please check your last name search term. ");
                return;
            }
            
            Task<Person> task = new Task<Person>(searchDB);
            status.Text = "Starting database search for last name...";
            status.Visibility = Visibility.Visible;
            task.Start();
            Person person = await task;
            status.Text = "Finished database search for last name...";
            FNameBox.Text = person.firstName;
            LNameBox.Text = person.lastName;
            BalBox.Text = person.balance.ToString("C");
            AccBox.Text = person.acctNo.ToString();
            PinBox.Text = person.pin.ToString("D4");
            PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(person.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            indexEntry.Text = index.ToString();        
        }

        private Person searchDB()
        {
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            Bitmap icon = null;
         
            channel.getAccountByLastName(searchTerm, out acct, out pin, out bal, out fName, out lName, out icon, out index); //RPC STUFF
            Person person = new Person();

            person.acctNo = acct;
            person.icon = icon;
            person.pin = pin;
            person.balance = bal;
            person.firstName = fName;
            person.lastName = lName;

            return person;
        }

        private void HandleClickIndex(object sender, RoutedEventArgs e)
        {
            int newIndex = 0;
            if (int.TryParse(indexEntry.Text, out newIndex) && newIndex != index)
            {
                index = newIndex;
                getDataIndex(index);
            }
            else
            {

            }

        }

        private struct Person
        {
            public uint acctNo;
            public uint pin;
            public int balance;
            public string firstName;
            public string lastName;
            public Bitmap icon;

        }

    }
}
