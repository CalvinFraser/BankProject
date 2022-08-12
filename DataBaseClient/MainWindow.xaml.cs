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

namespace DataBaseClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface.BusinessServerInterface channel;
        private int index = 0;
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
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void getDataSearch(string searchTerm)
        {
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            Bitmap icon = null;


            channel.getAccountByLastName(searchTerm, out acct, out pin, out bal, out fName, out lName, out icon); //RPC STUFF
            FNameBox.Text = fName;
            LNameBox.Text = lName;
            BalBox.Text = bal.ToString("C");
            AccBox.Text = acct.ToString();
            PinBox.Text = pin.ToString("D4");
            //Taken from: https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource
            PFP.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            icon.Dispose();

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
            catch(FaultException<AccountNotFoundFault> exception)
            {
                MessageBox.Show(exception.Detail.Issue);
            }
        }

        private void HandleClickSearch(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchEntry.Text;
            getDataSearch(searchTerm);
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

    }
}
