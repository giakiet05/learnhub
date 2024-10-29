using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyHoatDongDayHoc.ViewModel.LoginVM.SignUpVM
{
    public class SignUpViewModel : BaseViewModel
    {
        public ICommand NextCommand { get; set; }

        public SignUpViewModel()
        {
            NextCommand = new RelayCommand<Border>((p) => { return true; }, (p) =>
            {
                
                p.Visibility = Visibility.Visible;
            });
        }
    }
}
