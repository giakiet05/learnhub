using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AdminStudentAssignmentView.xaml
    /// </summary>
    public partial class AdminStudentAssignmentView : UserControl
    {
        public AdminStudentAssignmentView()
        {
            InitializeComponent();
        }
        private void LopDG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            // Kiểm tra nếu nhấn vào thanh cuộn hoặc khu vực không phải nội dung
            var hit = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));

            if (hit != null)
            {
                var visualHit = hit.VisualHit;

                // Kiểm tra xem nơi nhấn chuột có phải là thanh cuộn hay không
                while (visualHit != null)
                {
                    if (visualHit is ScrollBar)
                    {
                        // Nếu là thanh cuộn, không chặn sự kiện
                        return;
                    }

                    visualHit = VisualTreeHelper.GetParent(visualHit);
                }

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
                    return;
                }
            }
        }
    }
}
