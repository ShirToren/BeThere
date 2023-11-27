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

			return m_LogedInUser.Username;
        }
        public static UserData LogedInUserObject()
        {

            return m_LogedInUser;
        }

        public static void SetLogedInUser(UserData i_LogedInUser)
        {
			m_LogedInUser = i_LogedInUser;
        }

    }
}

