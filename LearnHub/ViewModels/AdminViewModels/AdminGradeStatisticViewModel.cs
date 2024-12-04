﻿using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminGradeStatisticViewModel : BaseViewModel
    {
      public  ICommand SwitchToYearCommand { get; }
        public AdminGradeStatisticViewModel()
        {
            SwitchToYearCommand = new NavigateLayoutCommand(() => new  AdminYearStatisticViewModel());
        }
    }
}