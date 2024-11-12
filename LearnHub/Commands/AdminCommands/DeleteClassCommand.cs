using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
    public class DeleteClassCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó ko biết

            //sau đó mở xác nhận xóa
            ModalNavigationStore.Instance.NavigateCurrentModelViewModel(() => new DeleteConfirmViewModel());
        }
    }
   
}
