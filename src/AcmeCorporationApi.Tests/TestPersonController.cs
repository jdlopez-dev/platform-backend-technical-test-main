using AcmeCorporation.Core.Entities;
using AcmeCorporation.Services.Helpers;
using AcmeCorporationApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AcmeCorporationApi.Tests
{
    public class TestPersonController
    {
        private readonly string _endPoint = "/Persons";
        private readonly HttpClient _httpClient;
        private readonly TestServer _testServer;

        public TestPersonController()
        {
            var webHostBuilder = new WebHostBuilder()
                .ConfigureAppConfiguration(p => p.AddJsonFile("appsettings.json", true))
                .UseStartup<Startup>();
            _testServer = new TestServer(webHostBuilder);
            _httpClient = _testServer.CreateClient();
        }

        [Fact]
        public async Task WhenCallApi_Post_CreateNewPerson_and_retries_create_then_get_duplicate_name_finally_delete()
        {
            PersonSaveModel person = new("PERSON_NEW", 18, "00000000T");
            var response = await _httpClient.PostAsJsonAsync(_endPoint, person);

            if (response.IsSuccessStatusCode)
            {
                var resultCreate = await response.Content.ReadFromJsonAsync<PersonModel>();

                // try to recreate the same person to get duplicate name error
                var responseDuplicatePerson = await _httpClient.PostAsJsonAsync(_endPoint, person);
                var errorMessage = responseDuplicatePerson.Content.ReadAsStringAsync().Result.Split(";");

                // delete person for database integrity
                string endPointDeleteId = String.Format("{0}/{1}", _endPoint, resultCreate.Id);
                var responseDelete = await _httpClient.DeleteAsync(endPointDeleteId);
                var resultDelete = await responseDelete.Content.ReadFromJsonAsync<bool>();

                // verifications
                Assert.NotNull(resultCreate);
                Assert.True(resultCreate.Id > 0);
                Assert.Contains(ErrorMessages.NAME_EXISTS, errorMessage);
                Assert.True(resultDelete);
            }
        }

        [Fact]
        public async Task WhenCallApi_Post_CreateNewPerson_With_DocumentTypeDNI()
        {
            PersonSaveModel person = new("PERSON_NEW", 18, "00000000T");
            var response = await _httpClient.PostAsJsonAsync(_endPoint, person);

            if (response.IsSuccessStatusCode)
            {
                var resultCreate = await response.Content.ReadFromJsonAsync<PersonModel>(); ;

                // delete person for database integrity
                string endPointDeleteId = String.Format("{0}/{1}", _endPoint, resultCreate.Id);
                var responseDelete = await _httpClient.DeleteAsync(endPointDeleteId);
                var resultDelete = await responseDelete.Content.ReadFromJsonAsync<bool>();

                // verifications
                Assert.NotNull(resultCreate);
                Assert.True(resultCreate.Id > 0);
                Assert.Equal(DocumentType.DNI.ToString(), resultCreate.DocumentType);
                Assert.True(resultDelete);
            }
        }

        [Fact]
        public async Task WhenCallApi_Post_ThenReturnNewPersonId_And_Delete()
        {
            PersonSaveModel person = new("PERSON_NEW", 18, "00000000T");
            var response = await _httpClient.PostAsJsonAsync(_endPoint, person);

            if (response.IsSuccessStatusCode)
            {
                // result of create new person
                var newPerson = await response.Content.ReadFromJsonAsync<PersonModel>();

                // delete person for database integrity
                string endPointDeleteId = String.Format("{0}/{1}", _endPoint, newPerson.Id);
                var responseDelete = await _httpClient.DeleteAsync(endPointDeleteId);
                var resultDelete = await responseDelete.Content.ReadFromJsonAsync<bool>();

                Assert.NotNull(newPerson);
                Assert.True(newPerson.Id > 0);
                Assert.True(resultDelete);
            }
        }

        [Fact]
        public async Task WhenCallApi_Post_With_InvalidAge_ThenReturnAgeMessageError()
        {
            PersonSaveModel person = new("NAME_PERSON", 0, "00000000T");
            var response = await _httpClient.PostAsJsonAsync(_endPoint, person);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result.Split(";");
                Assert.Equal(2, errorMessage.Length);
                Assert.Contains(ErrorMessages.AGE_REQUIRED, errorMessage);
                Assert.Contains(ErrorMessages.AGE_VALID, errorMessage);
            }
        }

        [Fact]
        public async Task WhenCallApi_Post_With_InvalidData_ThenReturnMessageError()
        {
            PersonSaveModel person = new("", 0, "");
            var response = await _httpClient.PostAsJsonAsync(_endPoint, person);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result.Split(";");
                Assert.Equal(5, errorMessage.Length);
            }
        }

        [Fact]
        public async Task WhenCallApi_Post_With_InvalidDocument_ThenReturnDocumenttMessageError()
        {
            PersonSaveModel person = new("NAME_PERSON", 18, "00000");
            var response = await _httpClient.PostAsJsonAsync(_endPoint, person);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                Assert.Equal("The document is not valid", errorMessage);
            }
        }

        [Fact]
        public async Task WhenCallApi_ToGet_Then_thenResult_and_Find_GenesisPerson()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<PersonModel>>(_endPoint,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task WhenCallApi_ToGetWithId1_ThenResult()
        {
            string endPointId = String.Format("{0}/1", _endPoint);
            var result = await _httpClient.GetFromJsonAsync<PersonModel>(endPointId, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }
    }
}