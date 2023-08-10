using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeThere.Services;
using Mopups.Services;

namespace BeThere.ViewModels.PopupViewModel
{
    public partial class PopupViewModel : BaseViewModels
    {
        private readonly NotificationsService r_NotificationsService;
        public Command CloseCommand { get; }
        public Command AnswerCommand { get; }
        public PopupViewModel(NotificationsService i_NotificationsService)
        {
            r_NotificationsService = i_NotificationsService;
            Title = "Popup";
            CloseCommand = new Command(async () => await closeClicked());
            AnswerCommand = new Command(async () => await answerClicked());
        }

        private async Task closeClicked()
        {
            await MopupService.Instance.PopAsync();
        }
        private async Task answerClicked()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}
