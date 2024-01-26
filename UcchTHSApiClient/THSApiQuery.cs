using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace THSApiClient
{
    
    public class PatientQuery
    {
        public string patientId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string  gender { get; set; }
        public DateTimeOffset birthDate { get; set; }
        public string psnDomain { get; set; }
        public string psn { get; set; }
        public string QueryResponse { get; set; }
        public string QueryStatusCode { get; set; }
        public string QueryMessage { get; set; }
        public string QueryExeption { get; set; }
    };
    public partial class Query
    {
        private System.Net.Http.HttpClient _httpClient;
        private THSApiClient.Client _thsapiclient;
        private PatientRequestBody _patientRequestBody;

        public void ApiEndpoint(String url)
        {
            Uri uri = new Uri(url);
            _httpClient = new System.Net.Http.HttpClient();
           // _httpClient.BaseAddress = uri;
            _thsapiclient = new THSApiClient.Client(uri.ToString(),_httpClient);
        }
        public void ApiEndpoint(String url, string user, string pwd)
        {
            Uri uri = new Uri(url);
            _httpClient = new System.Net.Http.HttpClient();
            //_httpClient.BaseAddress = uri;
            var authenticationString = $"{user}:{pwd}";
            var base64String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",base64String);
            _thsapiclient = new THSApiClient.Client(uri.ToString(),_httpClient) ;
         }

        public void PatientQuery(string patientId)
        {
            _patientRequestBody.PatientId = patientId;
        }

        //Liefert nur die PatientenId als string zurück
        public string GetPseudonymByPatientId(string patiendId, string domain) 
            => _thsapiclient.GetPseudonymForPatientIdAsync(patiendId, domain).Result.Result.Psn;

        //
        public PatientQuery GetOrCreatePatient(PatientQuery patientquery)
        {
            PatientRequestBody body = new PatientRequestBody();
            
            body.PatientId = patientquery.patientId;
            body.FirstName = patientquery.firstName;
            body.LastName = patientquery.lastName;
            body.BirthDate = patientquery.birthDate;
            body.Gender = patientquery.gender;
            body.PsnDomain = patientquery.psnDomain;

            
            try
            {
                var ResInfo = _thsapiclient.GetOrCreatePatientAsync(body).Result;
                var ResPat = ResInfo.Result;
                if (ResPat.Psn.Length>0) {
                    patientquery.psn = ResPat.Psn;
                }
                patientquery.QueryStatusCode = ResInfo.StatusCode.ToString();
                return patientquery;
            }
            catch(Exception e) {
                ApiException a = (ApiException)e.InnerException;
                patientquery.QueryStatusCode = a.StatusCode.ToString() ?? String.Empty;
                patientquery.QueryMessage = a.Message?.ToString() ?? String.Empty;
                patientquery.QueryResponse = a.Response?.ToString() ?? String.Empty;
                patientquery.QueryExeption = a.InnerException?.ToString() ?? String.Empty;
                return patientquery; }

        }




    }
}
