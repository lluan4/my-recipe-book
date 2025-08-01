﻿using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.test
{
    public class MyRecipeBookClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient = factory.CreateClient();


        protected async Task<HttpResponseMessage> DoPost(
           string method,
           object request,
           string token = "",
           string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.PostAsJsonAsync(method, request);
        }

        protected async Task<HttpResponseMessage> DoGet(string method, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.GetAsync(method);
        }

        protected async Task<HttpResponseMessage> DoPut(string method, object request, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _httpClient.PutAsJsonAsync(method, request);
        }


        private void ChangeRequestCulture(string culture)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

            _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

        private void AuthorizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
