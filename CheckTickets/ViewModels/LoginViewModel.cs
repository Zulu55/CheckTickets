using System;
using System.ComponentModel;
using System.Windows.Input;
using CheckTickets.Models;
using CheckTickets.Services;
using GalaSoft.MvvmLight.Command;

namespace CheckTickets.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;
		private string email;
		private string password;
		private bool isRunning;
		private bool isEnabled;
		#endregion

		#region Properties
		public string Email
		{
			set
			{
				if (email != value)
				{
					email = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
				}
			}
			get
			{
				return email;
			}
		}

		public string Password
		{
			set
			{
				if (password != value)
				{
					password = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
				}
			}
			get
			{
				return password;
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
        public LoginViewModel()
        {
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

            Email = "jzuluaga55@gmail.com";
            Password = "1234";
			IsEnabled = true;
		}
		#endregion

		#region Commands
		public ICommand LoginCommand { get { return new RelayCommand(Login); } }

		private async void Login()
		{
			if (string.IsNullOrEmpty(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter the user email.");
				return;
			}

			if (string.IsNullOrEmpty(Password))
			{
				await dialogService.ShowMessage("Error", "You must enter a password.");
				return;
			}

			IsRunning = true;
			IsEnabled = false;

			var response = await apiService.Login(
				"http://checkticketsback.azurewebsites.net",
				"/api",
				"/Users/Login",
				Email,
				Password);

			IsRunning = false;
			IsEnabled = true;

            if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", "User or password wrong.");
				Password = null;
				return;
			}

            var user = (User)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CheckTickets = new CheckTicketsViewModel(user);
            await navigationService.Navigate("CheckTicketsPage");
			Email = null;
			Password = null;
		}
		#endregion
	}
}
