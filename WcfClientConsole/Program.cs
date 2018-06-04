using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WcfClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a proxy to the WCF service
            int SecretNumber = 0;
            myInterfaceClient myPxy = new myInterfaceClient();

            //call the service operations through the proxy
            Console.WriteLine("Enter a low number: ");
            Int32 low = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a high number: ");
            Int32 high = Convert.ToInt32(Console.ReadLine());

            SecretNumber = myPxy.SecretNumber(low, high);
            //use to display secret number you cheater
            //Console.WriteLine(SecretNumber);

            Console.WriteLine();
            Int32 numAttempts = 0;
            bool isCorrect = false;
            Int32 guess = 0;
            string result = "";

            while(!isCorrect)
            {
                Console.WriteLine("Guess a Number!");
                guess = Convert.ToInt32(Console.ReadLine());
                result = myPxy.CheckNumber(guess, SecretNumber);

                if (result == "too big" || result == "too small")
                {
                    numAttempts++;
                    Console.WriteLine("Your Guess was " + result);
                    Console.WriteLine("Attempts: " + numAttempts);
                }
                else if(result == "correct")
                {
                    Console.WriteLine("Congrats, your Guess was " + result);
                    Console.WriteLine("Total Attempts: " + numAttempts);
                    isCorrect = true;
                    break;
                }
            }
            

            //Close the Proxy
            myPxy.Close();
            Console.WriteLine("/nPress <Enter> to terminate the client./n");
        }
    }
}
