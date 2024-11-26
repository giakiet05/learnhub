using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class YearScoreViewModel : BaseViewModel
    {
        public string Subject;
        public double Semester1;
        public double Semester2;
        public double AverageScore => (Semester1 + Semester2 * 2) / 3;

    }
}
