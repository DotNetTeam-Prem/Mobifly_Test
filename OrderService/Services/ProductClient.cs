using OrderService.DTOs;
using OrderService.IServices;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductResponse?> GetProductAsync(
            int productId)
        {
            var token =
                _httpContextAccessor.HttpContext?
                    .Request.Headers.Authorization
                    .ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse(token!);

            var response = await _httpClient.GetAsync(
                $"api/products/{productId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<ProductResponse>();
        }

        public async Task<bool> ReduceStockAsync(
            int productId,
            int quantity)
        {
            var token =
                _httpContextAccessor.HttpContext?
                    .Request.Headers.Authorization
                    .ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse(token!);

            var response = await _httpClient.PostAsync(
                $"api/products/{productId}/reduce_stock?quantity={quantity}",
                null);

            return response.IsSuccessStatusCode;
        }
    }
}