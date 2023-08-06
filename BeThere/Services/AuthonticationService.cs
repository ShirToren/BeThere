using System;
using BeThere.Models;

namespace BeThere.Services
{
	public class AuthonticationService
	{
		private const string AuthStateKey = "AuthStatet";
        private const string AuthenntticatedUser = "AuthUser";

        public AuthonticationService()
		{
		}

		public async Task<bool> isAuthenticatedAsync()
		{
            await Task.Delay(200);
            bool isAuth = Preferences.Default.Get<bool>(AuthStateKey, false);

			return isAuth;
        }

		public void Login()
		{
            Preferences.Default.Set<bool>(AuthStateKey, true);
        }

		public void Logout()
		{
            Preferences.Default.Remove(AuthStateKey);

        }

		public void SetAuthenticatedUser(string i_UserName)
		{
            Preferences.Default.Set<string>(AuthenntticatedUser, i_UserName);
        }

		public string GetAuthenticatedUser()
		{
			return Preferences.Default.Get<string>(AuthenntticatedUser, string.Empty);
        }

        public void RemoveAuthenticatedUser()
        {
            Preferences.Default.Remove(AuthenntticatedUser);
        }


    }
}

