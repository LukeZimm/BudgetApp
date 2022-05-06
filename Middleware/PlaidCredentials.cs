using Going.Plaid;

namespace BudgetApp.Middleware
{
	public class PlaidCredentials : PlaidOptions
	{
		public string LinkToken { get; set; }
		public string AccessToken { get; set; }
	}
}
