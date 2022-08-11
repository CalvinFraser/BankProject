/* CALVIN FRASER
 * CURTIN UNIVERSITY
 * 19921792
 */


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    internal class DatabaseGen
    {
        private readonly  Random _random = new Random();        
        private readonly string[] firstNames = { "James", "Katheryn", "Calvin", "Ethan", "Sarah", "Chris" };
        private readonly string[] lastNames = { "Fraser", "Smith", "Douglas", "Janson", "Wilson", "Anderson" };
        private readonly List<Bitmap> icons = new List<Bitmap>();
        public DatabaseGen() 
        {
            int width = 64;
            int height = 64;

            Bitmap bmp;

            for (int i = 0; i < 32; i++)
            {
                bmp = new Bitmap(width, height);
                for (int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        int a = _random.Next(256);
                        int r = _random.Next(256);
                        int g = _random.Next(256);
                        int b = _random.Next(256);

                        bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    }
                }
                icons.Add(bmp);
            }
        }

        private Bitmap getIcon() { return icons[_random.Next(icons.Count)]; }
        private string getFirstName() { return firstNames[_random.Next(firstNames.Length)]; }
        private string getLastName() { return lastNames[_random.Next(lastNames.Length)]; }

        private uint getPin() { return (uint)_random.Next(9999); }

        //Min account number is 10000000. Max is 99999999. Random 8 digit number
        private uint getAccNo() { return (uint)_random.Next(10000000, 99999999); }

        private int getBalance() { return _random.Next(-100000, 100000); }
          
        public int numberOfAccounts() { return _random.Next(10000, 30000); }
        public void getNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out Bitmap icon)
        {
            pin = getPin(); 
            acctNo = getAccNo();    
            firstName = getFirstName(); 
            lastName = getLastName();   
            balance = getBalance(); 
            icon = getIcon();  
        }



    }
}

