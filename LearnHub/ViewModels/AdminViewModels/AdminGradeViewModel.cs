using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class AdminGradeViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Grade> _grades;
        public IEnumerable<Grade> Grades => _grades; //dùng binding vào view

        private Grade _selectedGrade;
        public Grade SelectedGrade //dùng để binding vào view
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                GradeStore.Instance.SelectedGrade = value; // Sync with GradeStore

            }
        }
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }

        public ICommand SwitchToClassCommand { get; }
        public AdminGradeViewModel()
        {

            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteGrade), () => _selectedGrade != null, "Chưa chọn khối để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddGradeViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditGradeViewModel(), () => _selectedGrade != null, "Chưa chọn khối để sửa");
            SwitchToClassCommand = new NavigateLayoutCommand(() => new AdminClassViewModel());

            _grades = GradeStore.Instance.Grades; //lấy grades từ store để binding cho view


            LoadGradesAsync();
        }

        private async void LoadGradesAsync()
        {
            var grades = await GenericDataService<Grade>.Instance.GetAll();
            GradeStore.Instance.LoadGrades(grades);
        }

        private async void DeleteGrade()
        {
            var selectedGrade = GradeStore.Instance.SelectedGrade;

            try
            {
                await GenericDataService<Grade>.Instance.DeleteById(selectedGrade.Id);

                GradeStore.Instance.DeleteGrade(selectedGrade.Id);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Xóa thất bại");
            }
        }
    }
}
