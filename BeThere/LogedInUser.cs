using System;
using BeThere.Services;
using BeThere.Models;

namespace BeThere
{
	public static class LogedInUser
	{

		private static UserData m_LogedInUser; 

		public static string LogedInUserName()
		{
			string username = "User";

			return username;
        }

        public static void SetLogedInUser(UserData i_LogedInUser)
        {
			m_LogedInUser = i_LogedInUser;
        }

    }
}

