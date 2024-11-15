﻿using LearnHub.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class AdminGradeViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Delete { get; }
        public ICommand Edit { get; }
        public ICommand Class { get; }
        public AdminGradeViewModel()
        {
           
            Class = new NavigateLayoutCommand(() => new AdminClassViewModel());
        }
    }
}
