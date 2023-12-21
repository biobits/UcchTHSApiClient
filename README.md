# UcchTHSApiClient
 C# library to access UKE THS WebApi



## Installation
Add UcchTHSApiClient.dll to your project.

## Usage
```c#
using THSApiClient;

// Create a new Query Object and add the url of your api
THSApiClient.Query query = new THSApiClient.Query();
query.ApiEndpoint("https://my.specialapilink.net/treuhandstelle-iris_api/0.2.1/","ApiUser","ApiPassword");

// Create a new Patient Object that should be created or queried for psn
THSApiClient.Patient patient = new THSApiClient.Patient();
patient.patientId = "125125125";
patient.firstName = "Test";
patient.lastName = "Test";
patient.gender = "m";
patient.birthDate = DateTimeOffset.Parse("2001-01-01");
patient.psnDomain = "demo.ucch.mpi";

// Perform the Query
var pat = query.GetOrCreatePatient(patient);

// alternatively just perform a psn query for a given patientId. If the patientId is not known (the Patient does not exist in THS) a null-Value is returned
var psn=query.GetPseudonymByPatientId("23232323", "ucch.test");


            