using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SMS_Popup
{
    public class SMS
    {
        public long id;
        public string _from;
        public string text;
        public string sentStamp;
        public string receivedStamp;
        public string sim;
    }

    class SMSRestClient
    {
        static HttpClient client = CreateClient();
        const string WEBSERVICE_URL = "";

        string lastMessage = "";
        public string LastMessage
        {
            get { return lastMessage; }
            set { lastMessage = value; }
        }

        public SMSRestClient()
        {
        }

        private static HttpClient CreateClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            return new HttpClient(handler);
        }

        public void StartListening()
        {
            var timer = new System.Threading.Timer((e) =>
            {
                Application.Current.Dispatcher.Invoke((Action)async delegate
                {
                    try
                    {
                        SMS lastSMS = await GetLastSMS(WEBSERVICE_URL + "/last");
                        if (lastSMS != null && lastSMS.text != LastMessage)
                        {
                            new Popup(lastSMS.text);
                            LastMessage = lastSMS.text;
                        };
                    } 
                    catch (Exception e)
                    {

                    }
                });
            }, null, 0, 2500);
        }

        static async Task<SMS> GetLastSMS(string path)
        {
            SMS product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<SMS>();
            }
            return product;
        }

    }
}
