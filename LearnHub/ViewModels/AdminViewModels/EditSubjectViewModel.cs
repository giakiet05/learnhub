﻿using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class EditSubjectViewModel : BaseViewModel
    {
        public ICommand Edit { get; }
        public ICommand Cancel { get; }
        public EditSubjectViewModel()
        {
            Edit = new Confirm_EditSubjectViewModel();
            Cancel = new Cancel_EditSubjectViewModel();
        }
    }
}