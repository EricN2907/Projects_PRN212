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
    using System.Windows.Shapes;

    namespace InfertilityTreatment.Customer
    {
        /// <summary>
        /// Interaction logic for CustomerConfirmCancelDialog.xaml
        /// </summary>
        public partial class CustomerConfirmCancelDialog : Window
        {
            public string CancelReason { get; private set; }

            public CustomerConfirmCancelDialog()
            {
                InitializeComponent();
            }

            private void ConfirmCancelAppointment_Click(object sender, RoutedEventArgs e)
            {
            CancelReason = CancelReasonTextBox.Text;

            if (string.IsNullOrEmpty(CancelReason))
            {
                // Show error message if no reason is provided
                MessageBox.Show("Please provide a reason for cancellation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Display success message and close the dialog
                MessageBox.Show("Cancel Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;  // Close the dialog and return "true"
            }
        }

        private void CancelCancelAction_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog without taking action
            this.Close();
        }
    }
}

