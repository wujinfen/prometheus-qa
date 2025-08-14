using JsonPlaceholderService.Models;
using System.Text.Json;

namespace JsonPlaceholderService
{
    public class JsonPlaceHolderApiService
    {
        private readonly HttpClient _httpClient; //recieve an HTTP Client via dependency injection so that it doesn't create a new client for every request
        private readonly JsonSerializationOptions _jsonOptions; //serializer because the expected cases don't match

        public JsonPlaceHolderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializationOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            //set base addr to JSON Placeholder API in case caller doesn't set it
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            }

            __httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        /** 
          * /posts HTTP methods (note: all methods are async)
          * 
          * Get - all posts
          * Get - single post by id
          * Post - create new post
          * Put - update post by id
          * Delete - delete post by id
          */
        public async Task<Post?> GetPost()
        {

        }

        public async Task<Post?> GetPost(int id)
        {

        }

        public async Task<Post?> CreatePost(Post post)
        {

        }

        public async Task<Post?> UpdatePost(int id, Post post)
        {

        }

        public async Task<bool> DeletePost(int id)
        {

        }

    }
}