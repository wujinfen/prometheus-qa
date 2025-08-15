using NUnit.Framework;
using JsonPlaceholderService;
using JsonPlaceholderService.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using Moq.Contrib.HttpClient;
using System.Threading.Tasks; //makes it easier to mock HttpClient responses

/**
 * The idea is that we mock the HttpClient to avoid making real HTTP requests for the unit tests.
 * This allows us to test how the service handles *fake* HTTP responses without relying on the actual API.
 * Mocking HttpClient isn't trivial since it's a concrete class, so I used Moq.Contrib.HttpClient to simplify it (via SetupRequest(...)).
 * It also provides a .VerifyRequest(...), which allows us to check if the mocked handler was actually called
 * 
 * see: https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests
 */

namespace JsonPlaceholderService.UnitTests
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
        public async Task GetAllPosts_Unit()
        {
            //did not create all 100 fake entries
            var mockJsonResponse =
                """
                [
                    {"userId": 1, "id": 1, "title": "test post 1", "body": "test post body 1."},
                    {"userId": 2, "id": 2, "title": "test post 2", "body": "test post body 2."},
                    {"userId": 3, "id": 3, "title": "test post 3", "body": "test post body 3."}
                ]
                """;

            _httpMessageHandler.SetupRequest(HttpMethod.Get, "https://fakejsonplaceholder.com/posts")
                .ReturnsResponse(System.Net.HttpStatusCode.OK, mockJsonResponse, "application/json");

            var response = await _service.GetPosts();
            var postIds = new List<int>();
           
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(3)); //expects 3 posts
            //Assert that all posts have required fields
            Assert.That(response.All(p => p.Id > 0), Is.True, "all posts should have IDs");
            Assert.That(response.All(p => p.UserId > 0), Is.True, "all posts should have UserIds");
            Assert.That(response.All(p => !string.IsNullOrEmpty(p.Title)), Is.True, "all posts should have titles");
            Assert.That(response.All(p => !string.IsNullOrEmpty(p.Body)), Is.True, "all posts should have bodies");

            _httpMessageHandler.VerifyRequest(HttpMethod.Get, "https://fakejsonplaceholder.com/posts", Times.Once());
        }

        [Test]
        public async Task GetPost_Unit()
        {
            var mockJsonResponse =
                """
                {"userId": 1, "id": 1, "title": "test post 1", "body": "test post body 1."}
                """;

            _httpMessageHandler.SetupRequest(HttpMethod.Get, "https://fakejsonplaceholder.com/posts/1")
                .ReturnsResponse(HttpStatusCode.OK, mockJsonResponse, "application/json");

            var response = await _service.GetPost(1);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.UserId, Is.EqualTo(1));
            Assert.That(response.Title, Is.EqualTo("test post 1"));
            Assert.That(response.Body, Is.EqualTo("test post body 1."));

            _httpMessageHandler.VerifyRequest(HttpMethod.Get, "https://fakejsonplaceholder.com/posts/1", Times.Once());
        }

        [Test]
        public async Task CreatePost_Unit()
        {
            var newPost = new Post
            {
                //server assigns ID
                UserId = 1,
                Title = "new post test",
                Body = "this is a test post targeting the POST /posts endpoint"
            };

            var mockJsonResponse =
                """
                {"userId": 1, "id": 1, "title": "new post test", "body": "this is a test post targeting the POST /posts endpoint"}
                """;

            _httpMessageHandler.SetupRequest(HttpMethod.Post, "https://fakejsonplaceholder.com/posts")
                .ReturnsResponse(HttpStatusCode.Created, mockJsonResponse, "application/json");

            var response = await _service.CreatePost(newPost);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.GreaterThan(0));
            Assert.That(response.UserId, Is.EqualTo(1));
            Assert.That(response.Title, Is.EqualTo("new post test"));
            Assert.That(response.Body, Is.EqualTo("this is a test post targeting the POST /posts endpoint"));

            _httpMessageHandler.VerifyRequest(HttpMethod.Post, "https://fakejsonplaceholder.com/posts", Times.Once());
        }

        [Test]
        public async Task UpdatePost_Unit()
        {
            var updatedPost = new Post
            {
                UserId = 1,
                Title = "updated post title",
                Body = "this is a test put targeting the PUT /posts/{id} endpoint",
            };

            var mockJsonResponse =
                """
                {"userId": 1, "id": 1, "title": "updated post title", "body": "this is a test put targeting the PUT /posts/{id} endpoint"}
                """;

            _httpMessageHandler.SetupRequest(HttpMethod.Put, "https://fakejsonplaceholder.com/posts/1")
                .ReturnsResponse(HttpStatusCode.OK, mockJsonResponse, "application/json");

            var response = await _service.UpdatePost(1, updatedPost);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.UserId, Is.EqualTo(1));
            Assert.That(response.Title, Is.EqualTo("updated post title"));
            Assert.That(response.Body, Is.EqualTo("this is a test put targeting the PUT /posts/{id} endpoint"));

            _httpMessageHandler.VerifyRequest(HttpMethod.Put, "https://fakejsonplaceholder.com/posts/1", Times.Once());
        }

        [Test]
        public async Task DeletePost_Unit()
        {
            _httpMessageHandler.SetupRequest(HttpMethod.Delete, "https://fakejsonplaceholder.com/posts/1")
                .ReturnsResponse(HttpStatusCode.OK);

            var response = await _service.DeletePost(1);

            Assert.That(response, Is.True);

            _httpMessageHandler.VerifyRequest(HttpMethod.Delete, "https://fakejsonplaceholder.com/posts/1", Times.Once());
        }

        [Test]
        public async Task GetInvalidPost_Unit()
        {
            _httpMessageHandler.SetupRequest(HttpMethod.Get, "https://fakejsonplaceholder.com/posts/101")
                .ReturnsResponse(HttpStatusCode.NotFound);

            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var response = await _service.GetPost(101);
            });

            _httpMessageHandler.VerifyRequest(HttpMethod.Get, "https://fakejsonplaceholder.com/posts/101", Times.Once());
        }

        [Test]
        public async Task DeleteInvalidEndpoint_Unit()
        {
            _httpMessageHandler.SetupRequest(HttpMethod.Delete, "https://fakejsonplaceholder.com/non-existent")
                   .ReturnsResponse(HttpStatusCode.NotFound);

            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                //the Delete /post for the endpoint will always return status code 200, so we do a negative test on nonexistent endpoint to induce an exception 
                var response = await _httpClient.DeleteAsync("/non-existent");
                response.EnsureSuccessStatusCode();
            });

            _httpMessageHandler.VerifyRequest(HttpMethod.Delete, "https://fakejsonplaceholder.com/non-existent", Times.Once());
        }
    }
}