using NUnit.Framework;

//see reference.txt for NUnit reference sheet
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
            //close client
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllPosts()
        {

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