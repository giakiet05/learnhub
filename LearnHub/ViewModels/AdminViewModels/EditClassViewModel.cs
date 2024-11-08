﻿using System;
using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   
    public class EditClassViewModel : BaseViewModel
    {
        public ICommand Edit { get; }
        public ICommand Cancel { get; }
        public EditClassViewModel()
        {
            Edit = new Confirm_EditClassViewModel();
            Cancel = new Cancel_EditClassViewModel();
        }
    }
}
