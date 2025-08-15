using NUnit.Framework;
using JsonPlaceholderService;           
using JsonPlaceholderService.Models;
using System.Linq;

//see reference.txt for NUnit reference sheet
//PLACEHOLDER API: https://jsonplaceholder.typicode.com/
//NUnit Reference: https://www.automatetheplanet.com/nunit-cheat-sheet/
/*
 * Arrange
 * Act
 * Assert
 * Assert.That is (actual, expected)
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
            var postIds = new List<int>();
            foreach (var post in response)
            {
                postIds.Add(post.Id);
            }

            //Assert Response Status
            Assert.That(response, Is.Not.Null);

            //Assert all posts are returned
            Assert.That(response.Count, Is.GreaterThan(0));
            Assert.That(response.Count, Is.EqualTo(100)); //API has exactly 100 posts
            Assert.That(postIds, Is.Unique);

            //Assert that all posts have required fields
            Assert.That(response.All(p => p.Id > 0), Is.True, "all posts should have IDs");
            Assert.That(response.All(p => p.UserId > 0), Is.True, "all posts should have UserIds");
            Assert.That(response.All(p => !string.IsNullOrEmpty(p.Title)), Is.True, "all posts should have titles");
            Assert.That(response.All(p => !string.IsNullOrEmpty(p.Body)), Is.True, "all posts should have bodies");
        }

        [Test]
        public async Task GetPost()
        {
            var response = await _service.GetPost(1);

            Assert.That(response.UserId, Is.EqualTo(1));
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.Title, Is.EqualTo("sunt aut facere repellat provident occaecati excepturi optio reprehenderit"));
            Assert.That(response.Body, Is.EqualTo("quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"));
        }

        [Test]
        public async Task CreatePost()
        {
            var newPost = new Post
            {
                //server assigns ID
                UserId = 1,
                Title = "new post test",
                Body = "this is a test post targeting the POST /posts endpoint"
            };

            var response = await _service.CreatePost(newPost);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.GreaterThan(0));
            Assert.That(response.UserId, Is.EqualTo(1));    
            Assert.That(response.Title, Is.EqualTo("new post test"));
            Assert.That(response.Body, Is.EqualTo("this is a test post targeting the POST /posts endpoint"));
        }

        [Test]
        public async Task UpdatePost()
        {
            var updatedPost = new Post
            {
                UserId = 1,
                Title = "updated post title",
                Body = "this is a test put targeting the PUT /posts/{id} endpoint",
            };

            var response = await _service.UpdatePost(1, updatedPost);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.UserId, Is.EqualTo(1));
            Assert.That(response.Title, Is.EqualTo("updated post title"));
            Assert.That(response.Body, Is.EqualTo("this is a test put targeting the PUT /posts/{id} endpoint"));
        }

        [Test]
        public async Task DeletePost()
        {
            var response = await _service.DeletePost(1);
            Assert.That(response, Is.True);
        }

        [Test]
        public async Task GetInvalidPost()
        {
            int nonExistentId = 101;
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var response = await _service.GetPost(nonExistentId);
            });
        }

        [Test]
        public async Task DeleteInvalidEndpoint()
        {
            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                //the Delete /post for the endpoint will always return status code 200, so we do a negative test on nonexistent endpoint to induce an exception 
                var response = await _httpClient.DeleteAsync("/non-existent");
                response.EnsureSuccessStatusCode();
            });
        }

    }
}