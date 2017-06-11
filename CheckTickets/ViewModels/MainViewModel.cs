using System;
namespace CheckTickets.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public LoginViewModel Login
        {
            get;
            set;
        }

        public CheckTicketsViewModel CheckTickets
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

		public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
				instance = new MainViewModel();
			}

			return instance;
		}
		#endregion
	}
}
