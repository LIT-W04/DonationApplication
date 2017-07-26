using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationApplication.Data;

namespace DonationApplication.AdminCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the admin creator application");
            string firstName = Prompt("Enter a first name");
            string lastName = Prompt("Enter a last name");
            string email = Prompt("Enter an email address");
            string password = Prompt("Enter a password");
            var userDb = new UserRepository(Properties.Settings.Default.ConStr);
            userDb.AddUser(firstName, lastName, email, password, true);
            Console.WriteLine("Account created, press any key to exit");
            Console.ReadKey(true);
        }

        static string Prompt(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }
    }
}
