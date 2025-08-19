using System.Windows;
using BusinessObject.Models;

namespace InfertilityTreatment.Doctor
{
    public partial class RejectReasonWindow : Window
    {
        private Appointment _appointment;

        public RejectReasonWindow(Appointment appointment)
        {
            InitializeComponent();
            _appointment = appointment;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RejectReasonTextBox.Text))
            {
                MessageBox.Show("Please provide a reason for rejection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Không đóng cửa sổ nếu không có lý do từ chối
            }

            _appointment.Status = "Rejected"; // Cập nhật trạng thái thành Rejected
            _appointment.RejectReason = RejectReasonTextBox.Text; // Lấy lý do từ chối từ TextBox
            MessageBox.Show("The appointment has been rejected successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Đóng cửa sổ nhập lý do từ chối
        }
    }
}
