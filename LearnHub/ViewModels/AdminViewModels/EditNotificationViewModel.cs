﻿using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class EditNotificationViewModel : BaseViewModel
    {
        public ICommand Edit{ get; }
        public ICommand Cancel { get; }
        public EditNotificationViewModel()
        {
            Edit = new Confirm_EditNotificationCommand();
            Cancel = new Cancel_EditNotificationCommand();
        }
    }
}