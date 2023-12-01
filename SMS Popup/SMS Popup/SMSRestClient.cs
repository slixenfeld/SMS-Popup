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
        const string WEBSERVICE_URL = WS.WS_URL;

        Popup window;

        public SMSRestClient(Popup window)
        {
            this.window = window;
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

        public void RequestLastSMS()
        {
            Application.Current.Dispatcher.Invoke((Action)async delegate
            {
                try
                {
                    window.SMSText.Text = "Requesting SMS...";
                    SMS lastSMS = await GetLastSMS(WEBSERVICE_URL + "/last");
                    if (lastSMS != null)
                        window.SMSText.Text = lastSMS.text;
                    else
                        window.SMSText.Text = "ERROR: SMS was NULL";
                } 
                catch (Exception e)
                {
                    window.SMSText.Text = "ERROR: " + e.ToString();
                }
            });
        }

        static async Task<SMS> GetLastSMS(string path)
        {
            SMS sms = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                sms = await response.Content.ReadAsAsync<SMS>();
            }
            return sms;
        }
    }
}
