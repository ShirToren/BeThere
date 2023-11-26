using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Services
{
    public class CreditsService : BaseService
    {
        public async Task<ResultUnit<string>> TryUpdateCredits(int i_Credits)
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string endPointQueryUri = $"api/Credits?UserName={ConnectedUser.Username}&Credits={i_Credits}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);

            if (!response.IsSuccessStatusCode)
            {
                result.IsSuccess = true;
            }

            return result;
        }
    }
}
