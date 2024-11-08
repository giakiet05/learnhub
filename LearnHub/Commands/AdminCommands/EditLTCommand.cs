﻿using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
    public class EditLTCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó không biết

            // sau đó mở popup EditCalenderViewModel
            // cần truyền tham trị
            ModelNavigationStore.Instance.NavigateCurrentModelViewModel(() => new EditCalendarViewModel());
        }
    }
}
