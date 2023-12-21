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
    /// <summary>
    /// The Patient Object as represented in THS
    /// </summary>
    public class Patient
    {
        public string patientId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string  gender { get; set; }
        public DateTimeOffset birthDate { get; set; }
        public string psnDomain { get; set; }
        public string psn { get; set; }
    };
    /// <summary>
    /// THe Query Class - abstracts the Api Query
    /// </summary>
    public partial class Query
    {
        private System.Net.Http.HttpClient _httpClient;
        private THSApiClient.Client _thsapiclient;
        private PatientRequestBody _patientRequestBody;

        /// <summary>
        /// Creates a new ApiEndpoint without authentification
        /// </summary>
        /// <param name="url"></param>
        public void ApiEndpoint(String url)
        {
            Uri uri = new Uri(url);
            _httpClient = new System.Net.Http.HttpClient();
            _httpClient.BaseAddress = uri;
            _thsapiclient = new THSApiClient.Client(_httpClient);
        }
        /// <summary>
        /// Creates a new ApiEndpoint with authentification
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        public void ApiEndpoint(String url, string user, string pwd)
        {
            Uri uri = new Uri(url);
            _httpClient = new System.Net.Http.HttpClient();
            _httpClient.BaseAddress = uri;
            var authenticationString = $"{user}:{pwd}";
            var base64String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",base64String);
            _thsapiclient = new THSApiClient.Client(_httpClient);
         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientId"></param>
        public void Patient(string patientId)
        {
            _patientRequestBody.PatientId = patientId;
        }

        
        /// <summary>
        /// Gets the pseudonym of a patient by patientId
        /// </summary>
        /// <param name="patiendId"></param>
        /// <param name="domain"></param>
        /// <returns>String of pseudonym</returns>
        public string GetPseudonymByPatientId(string patiendId, string domain) => _thsapiclient.GetPseudonymForPatientIdAsync(patiendId, domain).Result.Psn;

        /// <summary>
        /// Gets or creates a Patient in the THS
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>A Patient Object with psn 
        /// </returns>
        public Patient GetOrCreatePatient(Patient patient)
        {
            PatientRequestBody body = new PatientRequestBody();
            
            body.PatientId = patient.patientId;
            body.FirstName = patient.firstName;
            body.LastName = patient.lastName;
            body.BirthDate = patient.birthDate;
            body.Gender = patient.gender;
            body.PsnDomain = patient.psnDomain;

            var ResPat = _thsapiclient.GetOrCreatePatientAsync(body).Result;
            patient.psn = ResPat.Psn;
            return patient;

        }




    }
}
