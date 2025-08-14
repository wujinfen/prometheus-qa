using JsonPlaceholderService.Models;
using System.Text;
using System.Text.Json;

namespace JsonPlaceholderService
{
    public class JsonPlaceholderApiService
    {
        private readonly HttpClient _httpClient; //recieve an HTTP Client via dependency injection so that it doesn't create a new client for every request
        private readonly JsonSerializerOptions _jsonOptions; //serializer because the expected cases don't match

        public JsonPlaceholderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            //set base addr to JSON Placeholder API in case caller doesn't set it
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            }

            _httpClient.Timeout = TimeSpan.FromSeconds(12);
        }

        /** 
          * /posts HTTP methods (note: all methods are async)
          * 
          * FOR REFERENCE: GetAsync, PostAsync, PutAsync, DeleteAsync
          * Get - all posts
          * Get - single post by id
          * Post - create new post
          * Put - update post by id
          * Delete - delete post by id
          */

        public async Task<List<Post>> GetPosts()
        {
            var response = await _httpClient.GetAsync("posts");
            response.EnsureSuccessStatusCode(); 
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Post>>(content, _jsonOptions) ?? new List<Post>(); //convert JSON to Post obj, return empty list if null
        }

        public async Task<Post?> GetPost(int id)
        {
            var response = await _httpClient.GetAsync($"posts/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Post>(content, _jsonOptions) ?? null;
        }

        //might be a bit strange since JSON Placeholder doesn't create, update, or delete posts
        public async Task<Post?> CreatePost(Post post)
        {
            //convert post to JSON and send it in request body
            var json = JsonSerializer.Serialize(post, _jsonOptions);
            //create body and set header fields
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //send post request to API
            var response = await _httpClient.PostAsync("posts", content);
            response.EnsureSuccessStatusCode();

            //convert response to Post object
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Post>(responseJson, _jsonOptions);
        }

        public async Task<Post?> UpdatePost(int id, Post post)
        {
            var json = JsonSerializer.Serialize(post, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"posts/{id}", content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Post>(responseJson, _jsonOptions);
        }

        public async Task<bool> DeletePost(int id)
        {
            var response = await _httpClient.DeleteAsync($"posts/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

    }
}