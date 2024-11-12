using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.Views.AdminViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
    public class DeleteNotificationCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó ko biết

            //sau đó mở xác nhận xóa
            ModalNavigationStore.Instance.NavigateCurrentModelViewModel(() => new DeleteConfirmViewModel());
        }
    }
}
