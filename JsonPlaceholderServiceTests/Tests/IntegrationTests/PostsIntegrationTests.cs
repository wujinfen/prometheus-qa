using NUnit.Framework;
using JsonPlaceholderService;           
using JsonPlaceholderService.Models;   

//see reference.txt for NUnit reference sheet
/*
 * Arrange
 * Act
 * Assert
 * 
 */
namespace JsonPlaceholderService.IntegrationTests
{
    [TestFixture]
    public class JsonPlaceholderPostIntegrationTests
    {
        private JsonPlaceholderApiService _service;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            //create a http client and service before each test
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            _service = new JsonPlaceholderApiService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            //shutdown client
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllPosts()
        {
            var response = await _service.GetPosts();

            //Assert Response Status
            Assert.That(response, Is.Not.Null);
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo("application/json"));

            //Assert Content
            Assert.That(response.Count, Is.GreaterThan(0));
        
            
        }

        [Test]
        public async Task GetPost()
        {

        }

        [Test]
        public async Task CreatePost()
        {

        }

        [Test]
        public async Task UpdatePost()
        {

        }

        [Test]
        public async Task DeletePost()
        {

        }

    }
}