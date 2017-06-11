using CheckTickets.ViewModels;

namespace CheckTickets.Infrastructure
{
    public class InstanceLocator
	{
		public MainViewModel Main { get; set; }

		public InstanceLocator()
		{
			Main = new MainViewModel();
		}
	}
}
