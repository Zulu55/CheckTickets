using System;
using System.ComponentModel;
using System.Windows.Input;
using CheckTickets.Models;
using CheckTickets.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace CheckTickets.ViewModels
{
	public class CheckTicketsViewModel : INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;
		private string ticketCode;
        private Color colorMessage;
		private string message;
		private bool isRunning;
		private bool isEnabled;
        private User user;
        #endregion

        #region Properties
        public string TicketCode
		{
			set
			{
				if (ticketCode != value)
				{
					ticketCode = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TicketCode"));
				}
			}
			get
			{
				return ticketCode;
			}
		}

		public Color ColorMessage
		{
			set
			{
				if (colorMessage != value)
				{
					colorMessage = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorMessage"));
				}
			}
			get
			{
				return colorMessage;
			}
		}

		public string Message
		{
			set
			{
				if (message != value)
				{
					message = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
				}
			}
			get
			{
				return message;
			}
		}

		public bool IsRunning
		{
			set
			{
				if (isRunning != value)
				{
					isRunning = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
				}
			}
			get
			{
				return isRunning;
			}
		}

		public bool IsEnabled
		{
			set
			{
				if (isEnabled != value)
				{
					isEnabled = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
				}
			}
			get
			{
				return isEnabled;
			}
		}
		#endregion

		#region Constructor
        public CheckTicketsViewModel(User user)
        {
            this.user = user;
		
            apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

			Message = "Wait for read a ticket...";
            ColorMessage = Color.Gray;
			IsEnabled = true;
		}
        #endregion

        #region Commands
        public ICommand CheckTicketCommand { get { return new RelayCommand(CheckTicket); } }

		private async void CheckTicket()
		{
			if (string.IsNullOrEmpty(TicketCode))
			{
				await dialogService.ShowMessage("Error", "You must enter a valid ticket.");
				return;
			}

			if (TicketCode.Length != 4)
			{
				await dialogService.ShowMessage("Error", "The ticket code must be 4 characters length.");
				return;
			}

			IsRunning = true;
			IsEnabled = false;

            var response = await apiService.GetTicket(
				"http://checkticketsback.azurewebsites.net",
				"/api",
				"/Tickets",
				TicketCode);

			IsRunning = false;
			IsEnabled = true;

			if (response.IsSuccess)
			{
                Message = string.Format("{0}, TICKET READ BEFORE!",TicketCode);
                ColorMessage = Color.Red;
                TicketCode = null;
				return;
			}

            var ticket = new Ticket
            {
                DateTime = DateTime.Now,
                TicketCode = TicketCode,
                UserId = user.UserId,
            };

			IsRunning = true;
			IsEnabled = false;

			response = await apiService.Post(
				"http://checkticketsback.azurewebsites.net",
				"/api",
				"/Tickets",
				ticket);

			IsRunning = false;
			IsEnabled = true;

			if (!response.IsSuccess)
			{
				Message = string.Format("{0}, The ticket can't be update, " +
                                        "please try latter.", TicketCode);
				ColorMessage = Color.Orange;
				TicketCode = null;
				return;
			}
			
            Message = string.Format("{0}, ALLOW ACCESS!", TicketCode);
			ColorMessage = Color.Green;
			TicketCode = null;
		}
		#endregion
	}
}
