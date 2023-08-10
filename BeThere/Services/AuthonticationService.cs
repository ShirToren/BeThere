using System;
using BeThere.Models;
using Newtonsoft.Json;

namespace BeThere.Services
{
	public class AuthonticationService : BaseService
	{
		private const string m_AuthStateKey = "AuthStatet";
        private const string m_AuthenticatedUserName = "AuthUser";

        public AuthonticationService()
		{
        }

        public async Task GetAuthLoginUser()
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string userName = GetStoredAuthenticatedUser();
            string endPointQueryUri = $"GetUser?UserName={userName}";

            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                UserData userData = JsonConvert.DeserializeObject<UserData>(jsonResponse);
                LogedInUser.SetLogedInUser(userData);
                ConnectedUser = userData;
            }
            else
            {
                //handle
            }
        }


        public async Task<bool> isAuthenticatedAsync()
		{
            await Task.Delay(200);
            bool isAuth = Preferences.Default.Get<bool>(m_AuthStateKey, false);

			return isAuth;
        }

		public void Login()
		{
            Preferences.Default.Set<bool>(m_AuthStateKey, true);
        }

		public void Logout()
		{
            Preferences.Default.Remove(m_AuthStateKey);

        }

		public void StoreAuthenticatedUser(string i_UserName)
		{
            Preferences.Default.Set<string>(m_AuthenticatedUserName, i_UserName);
        }

		public string GetStoredAuthenticatedUser()
		{
			return Preferences.Default.Get<string>(m_AuthenticatedUserName, string.Empty);
        }

        public void RemoveAuthenticatedUser()
        {
            Preferences.Default.Remove(m_AuthenticatedUserName);
        }


    }
}

