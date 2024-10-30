using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using RandomApi;
using RandomApi.Repositories;
using SpecFlow.RandomApi.Test.Helpers;
using SpecFlow.RandomApi.Test.Repositories;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace SpecFlow.RandomApi.Test.StepDefinitions
{
    [Binding]
    public class WeatherForecastApiStepDefinitions
    {
        private const string BaseAddress = "http://localhost:5177/";
        public WebApplicationFactory<Program> Factory { get; }
        public IWeatherRepository Repository { get; }
        public HttpClient Client { get; set; } = null!;
        private HttpResponseMessage Response { get; set; } = null!;
        public JsonFilesRepository JsonFilesRepo { get; }
        private WeatherForecast? Entity { get; set; }


        private JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true
        };

        public WeatherForecastApiStepDefinitions(
            WebApplicationFactory<Program> factory,
            IWeatherRepository repository,
            JsonFilesRepository jsonFilesRepo)
        {
            Factory = factory;
            Repository = repository;
            JsonFilesRepo = jsonFilesRepo;
        }

        [Given(@"I am a client")]
        public void GivenIAmAClient()
        {
            Client = Factory.CreateDefaultClient(new Uri(BaseAddress));
        }

        [Given(@"the repository has weather data")]
        public async Task GivenTheRepositoryHasWeatherData()
        {
            var weathersJson = JsonFilesRepo.Files["weathers.json"];
            var weathers = JsonSerializer.Deserialize<IList<WeatherForecast>>(weathersJson, JsonSerializerOptions);
            if (weathers != null)
            {
                foreach (var weather in weathers)
                {
                    await Repository.AddAsync(weather);
                }
            }
        }

        [When(@"I make a GET request to '(.*)'")]
        public async Task WhenIMakeAgentRequestTo(string endpoint)
        {
            Response = await Client.GetAsync(endpoint);
        }

        [When(@"I make a GET request with id '(.*)' to '(.*)'")]
        public async Task WhenIMakeAgetRequestWithIdTo(int id, string endpoint)
        {
            Response = await Client.GetAsync($"{endpoint}/{id}");
        }

        [When(@"I make a POST request with '(.*)' to '(.*)'")]
        public async Task WhenIMakeApostRequestWithTo(string file, string endpoint)
        {
            var json = JsonFilesRepo.Files[file];
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            Response = await Client.PostAsync(endpoint, content);
        }

        [When(@"I make a PUT request with '(.*)' to '(.*)'")]
        public async Task WhenIMakeAputRequestWithTo(string file, string endpoint)
        {
            var json = JsonFilesRepo.Files[file];
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            Response = await Client.PutAsync(endpoint, content);
        }

        [When(@"I make a DELETE request with id '(.*)' to '(.*)'")]
        public async Task WhenIMakeAdeleteRequestWithIdTo(int id, string endpoint)
        {
            Response = await Client.DeleteAsync($"{endpoint}/{id}");
        }

        [Then(@"the response status code is '(.*)'")]
        public void ThenTheResponseStatusCodeIs(int statusCode)
        {
            var expected = (HttpStatusCode)statusCode;
            Assert.AreEqual(expected, Response.StatusCode);
        }

        [Then(@"the location header is '(.*)'")]
        public void ThenTheLocationHeaderIs(Uri location)
        {
            Assert.AreEqual(location, Response.Headers.Location);
        }

        [Then(@"the response json should be '(.*)'")]
        public async Task ThenTheResponseDataShouldBe(string file)
        {
            var expected = JsonFilesRepo.Files[file];
            var response = await Response.Content.ReadAsStringAsync();
            var actual = response.JsonPrettify();
            Assert.AreEqual(expected, actual);
        }

        [Then(@"the response entity should be '(.*)'")]
        public async Task ThenTheResponseEntityShouldBe(string file)
        {
            var json = JsonFilesRepo.Files[file];
            var expected = JsonSerializer.Deserialize<WeatherForecast>(json, JsonSerializerOptions);
            var actual = await Response.Content.ReadFromJsonAsync<WeatherForecast>();
            Entity = actual;
            Assert.IsTrue(new WeatherForecastComparer().Equals(expected, actual), "The WeatherForecast objects are not equal.");
        }

        [Then(@"it should have a new ETag")]
        public void ThenItShouldHaveANewETag()
        {
            var etag = Guid.Empty;
            if (Entity?.ETag != null)
                etag = Guid.Parse(Entity.ETag);
            Assert.AreNotEqual(Guid.Empty, etag);
        }
    }
}
