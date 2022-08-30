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

            progress.Value = 0;
            progress.Visibility = Visibility.Hidden;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private Person getDataSearch(string searchTerm)
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
            /*
            FNameBox.Text = fName;
            LNameBox.Text = lName;
            BalBox.Text = bal.ToString("C");
            AccBox.Text = acct.ToString();
            PinBox.Text = pin.ToString("D4");
            //Taken from: https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource
            PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            icon.Dispose();
            */

        }
        private  void getDataIndex(int index)
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
            catch(FaultException<AccountNotFoundFault> exception)
            {
                MessageBox.Show(exception.Detail.Issue);
            }
        }

        private void HandleClickSearch(object sender, RoutedEventArgs e)
        {

            string searchTerm = searchEntry.Text;

            if(rx.IsMatch(searchTerm))
            {
                MessageBox.Show("Please check your last name search term.");
                return;
            }
            // progress.Visibility = Visibility.Visible;
            FNameBox.IsReadOnly = true;
            LNameBox.IsReadOnly = true;
            BalBox.IsReadOnly = true;
            AccBox.IsReadOnly = true;
            PinBox.IsReadOnly = true;
            indexEntry.IsReadOnly = true;
            searchEntry.IsReadOnly = true;
            SearchButton.IsEnabled = false;
            GoButton.IsEnabled = false;

            
            search = getDataSearch;
            AsyncCallback searchCallBack;
            searchCallBack = OnSearchComplete;
            IAsyncResult result = search.BeginInvoke(searchTerm, searchCallBack, null);
  


        }

        private void OnSearchComplete(IAsyncResult asyncResult)
        {
            Person person;
            Search search = null; 
            AsyncResult obj = (AsyncResult)asyncResult;
            if(obj.EndInvokeCalled == false)
            {
                search = (Search)obj.AsyncDelegate;
                person = search.EndInvoke(obj);
                updateGUI(person);
            }
            obj.AsyncWaitHandle.Close();

        }
        private void updateGUI(Person person)
        {
            FNameBox.Dispatcher.Invoke(new Action(() => { FNameBox.Text = person.firstName;FNameBox.IsReadOnly = false;}));
            LNameBox.Dispatcher.Invoke(new Action(() => { LNameBox.Text = person.lastName; LNameBox.IsReadOnly = false; }));
            BalBox.Dispatcher.Invoke(new Action(() => { BalBox.Text = person.balance.ToString("C"); BalBox.IsReadOnly = false; }));
            AccBox.Dispatcher.Invoke(new Action(() => { AccBox.Text = person.acctNo.ToString(); AccBox.IsReadOnly = false; }));
            PinBox.Dispatcher.Invoke(new Action(()=> { PinBox.Text = person.pin.ToString("D4"); PinBox.IsReadOnly = false; }));

            //Taken from: https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource
            PFP.Dispatcher.Invoke(new Action( () => PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(person.icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));

            indexEntry.Dispatcher.Invoke(new Action(() => { indexEntry.IsReadOnly = false; indexEntry.Text = index.ToString(); }));
            searchEntry.Dispatcher.Invoke(new Action(() => { searchEntry.IsReadOnly = false; }));
            SearchButton.Dispatcher.Invoke(new Action(() => { SearchButton.IsEnabled = true; }));
            GoButton.Dispatcher.Invoke(new Action(() => { GoButton.IsEnabled = true; }));


        }

        private void HandleClickIndex(object sender, RoutedEventArgs e)
        {
            int newIndex = 0; 
            if(int.TryParse(indexEntry.Text, out newIndex) && newIndex != index)
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
