using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors;
using Going.Plaid.Item;
using BudgetApp.Models;
using Going.Plaid.Accounts;

namespace BudgetApp.Controllers
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class PlaidController : ControllerBase
    {
        private readonly Middleware.PlaidCredentials _credentials;
        private readonly PlaidClient _client;
        private readonly DatabaseController _dbcontroller;

        public PlaidController(IOptions<Middleware.PlaidCredentials> credentials, PlaidClient client, DatabaseController dbcontroller)
        {
            _credentials = credentials.Value;
            _client = client;
            _dbcontroller = dbcontroller;
        }
        [HttpPost("api/plaid/link_token")]
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
        [HttpPost("api/plaid/access_token")]
        public async Task<Value> AccessToken(PlaidConnectRequest request)
        {
            var result = await _client.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest()
                {
                    PublicToken = request.publicToken,
                });
            //_credentials.AccessToken = result.AccessToken;
            var user = new User { AccessToken = result.AccessToken, Email = request.email };
            _ = _dbcontroller.CreateUser(user);
            return new Value { value = result.AccessToken };
        }
        [HttpPost("api/plaid/balances")]
        public async Task<AccountsGetResponse> Balances(PlaidBalanceRequest request)
        {
            var userid = await _dbcontroller.UserId(request.email);
            if (userid == null) return null;
            _client.AccessToken = await _dbcontroller.AccessToken(userid);
            var plaidrequest = new AccountsBalanceGetRequest()
            {
                Options = new AccountsBalanceGetRequestOptions()
            };
            var result = await _client.AccountsBalanceGetAsync(plaidrequest);
            return result;
        }
    }
}
