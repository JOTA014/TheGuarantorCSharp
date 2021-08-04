using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TheGuarantorsChallenge.Helpers;
using Xunit;

namespace TheGuarantorsChallenge
{
    public class FakeRestAPIUsersTest: IDisposable
    {
        readonly HttpClient Client;
        public FakeRestAPIUsersTest()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://fakerestapi.azurewebsites.net/api/v1/")
            };
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private UserType TestUserType => new UserType { Id = 1212, UserName = $"UserName 1212", Password = "password1212" };

        [Fact]
        public void GetAllUsers()
        {
            
            var response = Client.GetAsync("Users").Result;
            var content = JsonSerializer.Deserialize<IEnumerable<UserType>>(response.Content.ReadAsStringAsync().Result);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(content.Count() > 1);
        }

        [Fact]
        public void GetUserById()
        {
            var testId = 1;
            var response = Client.GetAsync($"Users/{testId}").Result;
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void GetByNotExistingId()
        {
            var testId = 125;
            var response = Client.GetAsync($"Users/{testId}").Result;
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void PostUser()
        {
            var response = Client.PostAsync("Users", StringContentHelper.GetJSONStringContent(TestUserType)).Result;
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void PostUserBadRequest()
        {
            var response = Client.PostAsync("Users", StringContentHelper.GetJSONStringContent("")).Result;
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public void PutUser()
        {
            var result = Client.PutAsync($"Users/{TestUserType.Id}", StringContentHelper.GetJSONStringContent(TestUserType)).Result;

            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void DeleteUser()
        {
            var testId = 1;
            var result = Client.DeleteAsync($"Users/{testId}").Result;

            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }

        public void Dispose()
        {
            //Here I would make some cleanup if neccesary, but since FakeAPI doesn't add or update anything, it is not needed
        }
    }
}
