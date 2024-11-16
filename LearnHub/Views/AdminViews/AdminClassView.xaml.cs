using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearnHub.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for AdminClassView.xaml
    /// </summary>
    public partial class AdminClassView : UserControl
    {
        public AdminClassView()
        {
            InitializeComponent();
            DataContext = new AdminClassViewModel();
        }

        private void LopDG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            // Lấy đối tượng nơi nhấn chuột
            var hit = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));

            if (hit != null)
            {
                // Xác định dòng được nhấn (nếu có)
                var row = ItemsControl.ContainerFromElement(dataGrid, hit.VisualHit) as DataGridRow;

                if (row != null)
                {
                    // Chọn dòng
                    dataGrid.SelectedItem = row.Item;
                }
                else
                {
                    // Nếu nhấn vào khoảng trống, không thay đổi lựa chọn
                    e.Handled = true;
                }
            }
        }
    }
}
