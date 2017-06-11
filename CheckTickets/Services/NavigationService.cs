using System.Threading.Tasks;
using CheckTickets.Views;

namespace CheckTickets.Services
{
    public class NavigationService
    {
		public async Task Navigate(string pageName)
		{   
			switch (pageName)
			{
				case "CheckTicketsPage":
					await App.Current.MainPage.Navigation.PushAsync(new CheckTicketsPage());
					break;
				default:
					break;
			}
		}

		public async Task Back()
		{
			await App.Current.MainPage.Navigation.PopAsync();
		}
	}
}
