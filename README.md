## PG - Software QAE Assessment: 

Create two C# test projects, use ANY library reference/NuGet you like, upload to single GitHub repository and provide the URL upon completion. The projects must compile as we will clone the repository and make changes during the technical interview.
The first project will test some REST API. To showcase your development skills, you can consider using config, wrappers, API clients, DTO but balance between simplicity and scalability.

### Project 1 Requirements: 
Use the free REST API - https://jsonplaceholder.typicode.com/ [jsonplaceholder.typicode.com]

-The API call can be for any of the resources, /posts, /comments, /albums, /photos, /todos or /users.\
-Have at least 1 test of each HTTP method. GET, POST, PUT, DELETE.\
-Have validations for the responses\
-Have at least 1 negative test
 
### Project 2 Requirements:
The 2nd project will use Playwright. You can consider using config, page objects and whatever tricks you have up your sleeve.

-Using Playwright, go to http://www.google.com/ [google.com]\
-In the search box, type "Prometheus Group" and search\
-Verify in search result contains "Prometheus Group"\
-Click on the "Contact Us" link\
-On the ".../contact-us" page, enter the "First Name" and "Last Name" then click on "Submit"\
-Validate there are 4 additional required fields

****

### Project 1 Approach:

  -I made a simple API service `JsonPlaceholderApiService.cs` that handles HTTP requests for JsonPlaceholder API's `/posts` endpoint. It simply performs HTTP calls and represents the response as a C# Object.
  
  -Then, I created two test suites for the API service. I used NUnit and Moq for testing (nearly identical to JUnit and Mockito).
  
    -`PostsApiIntegrationTests.cs` holds E2E integration tests that validate functionality of the service's HTTP calls. It confirms that the service, HTTP Client, and API are all functional.
    -`PostsServiceUnitTests.cs` holds unit tests that validate functionality of the service's methods. It mocks the HTTP Client and HTTP response to validate that the service is functional without dependency on the client and API. It also verifies that the mocked endpoints were called via the service.
  
  -Finally, I created a .yml workflow that automatically builds and runs those test in GitHub Actions when anything is pushed to the repo.
