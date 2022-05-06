using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;
using Going.Plaid.Item;
using BudgetApp.Models;
using System.Web.Http;

namespace BudgetApp.Controllers
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class PlaidController : ControllerBase
    {
        private readonly Middleware.PlaidCredentials _credentials;
        private readonly PlaidClient _client;

        public PlaidController(IOptions<Middleware.PlaidCredentials> credentials, PlaidClient client)
        {
            _credentials = credentials.Value;
            _client = client;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("api/plaid/link_token")]
        public async Task<Value> LinkToken()
        {
            var result = await _client.LinkTokenCreateAsync(
                new LinkTokenCreateRequest()
                {
                    User = new LinkTokenCreateRequestUser { ClientUserId = Guid.NewGuid().ToString(), },
                    ClientName = "Example Client",
                    Products = new[] { Products.Auth, Products.Transactions },
                    Language = Language.English,
                    CountryCodes = new[] { CountryCode.Us },
                });
            return new Value { value = result.LinkToken };
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("api/plaid/access_token")]
        public async Task<Value> AccessToken([FromUri] string publicToken)
        {
            var result = await _client.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest()
                {
                    PublicToken = publicToken,
                });
            _credentials.AccessToken = result.AccessToken;

            return new Value { value = result.AccessToken };
        }
    }
}
