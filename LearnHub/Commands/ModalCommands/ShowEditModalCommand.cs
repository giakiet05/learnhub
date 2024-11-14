﻿using LearnHub.Stores;
using LearnHub.ViewModels;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.ModalCommands
{
    public class ShowEditModalCommand : BaseCommand
    {
        private readonly Func<BaseViewModel> _createViewModel;

        public ShowEditModalCommand(Func<BaseViewModel> createViewModel)
        {
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            var viewModel = _createViewModel();
            ModalNavigationStore.Instance.NavigateCurrentModelViewModel(() => viewModel);
        }
    }
}
