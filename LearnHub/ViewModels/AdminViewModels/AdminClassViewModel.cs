using LearnHub.Commands;
using LearnHub.Commands.AdminCommands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminClassViewModel : BaseViewModel
    {

        private readonly ObservableCollection<Classroom> _classrooms;
        public IEnumerable<Classroom> Classrooms => _classrooms;
        private readonly IDataService<Classroom> _classroomService = GenericDataService<Classroom>.Instance;
       
        public ICommand Add { get; }
        public ICommand Delete { get; }
        public ICommand Edit { get; }
        public ICommand Grade { get; }

    

        public AdminClassViewModel()
        {
            ShowAddModalCommand = new ShowAddModalCommand(new AddClassViewModel());
            ShowEditModalCommand = new ShowEditModalCommand(new EditClassViewModel());

            _classrooms = new ObservableCollection<Classroom>();
            LoadClassroomsAsync();
        }
        private async Task LoadClassroomsAsync()
        {
            var classrooms = await _classroomService.GetAll();
            foreach (var classroom in classrooms) {
                _classrooms.Add(classroom);
            
            }
        }
    }
}
