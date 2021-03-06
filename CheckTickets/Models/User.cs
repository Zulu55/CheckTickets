﻿namespace CheckTickets.Models
{
    public class User
    {
		public int UserId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

        public string FullName 
        { 
            get { return string.Format("{0} {1}", FirstName, LastName);  }  
        }
    }
}
