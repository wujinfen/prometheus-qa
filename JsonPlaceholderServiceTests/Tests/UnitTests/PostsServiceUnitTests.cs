using NUnit.Framework;
using JsonPlaceholderService;
using JsonPlaceholderService.Models;
using System.Linq;
using Moq;
using Moq.Contrib.HttpClient; //makes it easier to mock HttpClient responses

/**
 * The idea is that we mock the HttpClient to avoid making real HTTP requests for the unit tests.
 * This allows us to test how the service handles *fake* HTTP responses without relying on the actual API.
 * Mocking HttpClient isn't trivial since it's a concrete class, so I used Moq.Contrib.HttpClient to simplify it
 * 
 * see: https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests
 */

namespace JsonPlaceholderServiceTests
{
    [TestFixture]
    public class JsonPlaceholderServiceUnitTests
    {
        private JsonPlaceholderApiService _service;
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandler.Object);
            _httpClient.BaseAddress = new Uri("https://fakejsonplaceholder.com/");
            _service = new JsonPlaceholderApiService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public void GetAllPosts_Unit()
        {
            
        }

        [Test]
        public void GetPost_Unit()
        {

        }

        [Test]
        public void CreatePost_Unit()
        {

        }

        [Test]
        public void UpdatePost_Unit()
        {

        }

        [Test]
        public void DeletePost_Unit()
        {

        }

        [Test]
        public void GetInvalidPost_Unit()
        {

        }

        [Test]
        public void DeleteInvalidEndpoint_Unit()
        {

        }
    }
}