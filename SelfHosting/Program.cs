using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SelfHosting
{
    [ServiceContract]
    //use interface to define contracts
    public interface myInterface
    {
        [OperationContract]
        string CheckNumber(int userNum, int SecretNum);

        [OperationContract]
        int SecretNumber(int lower, int upper);
    }

    //implementation of interface
    public class guessService : myInterface
    {
        public int SecretNumber(int lower, int upper)
        {
            DateTime currentDate = DateTime.Now;
            int seed = (int)currentDate.Ticks;
            Random random = new Random(seed);
            int sNumber = random.Next(lower, upper);
            return sNumber;
        }

        public string CheckNumber(int userNum, int SecretNum)
        {
            if (userNum == SecretNum)
                return "correct";
            else
            if (userNum > SecretNum)
                return "too big";
            else return "too small";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Create a URI instance to guessService as the base address
            Uri baseAddress = new Uri("http://localhost:8000/Service");

            //create a new ServiceHost instance to host the service
            ServiceHost selfHost = new ServiceHost(typeof(guessService), baseAddress);

            try
            {
                //add a service endpoint with contract and binding
                selfHost.AddServiceEndpoint(typeof(myInterface), new WSHttpBinding(), "guessService");

                //add metadata for platform-independent access
                System.ServiceModel.Description.ServiceMetadataBehavior smb = new System.ServiceModel.Description.ServiceMetadataBehavior();
                //enable the metadata
                smb.HttpGetEnabled = true;
                //add here
                selfHost.Description.Behaviors.Add(smb);

                //start the service and waiting for request
                selfHost.Open();
                Console.WriteLine("Running on port 8000, ready to take requests");
                Console.WriteLine("Call SecretNumber(int lower, int upper) to generate a Secret Number");
                Console.WriteLine("Call CheckNumber(int userNum, int SecretNum) to guess the secret number");
                Console.WriteLine("Press <Enter>.\n to quit");
                Console.ReadLine();
                //close the ServiceHostBase to shutdown service
                selfHost.Close();
            }
            catch(CommunicationException ce)
            {
                Console.WriteLine("An Exception occured: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
