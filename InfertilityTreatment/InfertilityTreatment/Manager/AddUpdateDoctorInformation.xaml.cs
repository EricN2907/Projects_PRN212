
using BusinessObject.Models;
using Service.Implements;
using Service.Interface;
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

namespace InfertilityTreatment.Manager
{
    /// <summary>
    /// Interaction logic for AddUpdateDoctorInformation.xaml
    /// </summary>
    public partial class AddUpdateDoctorInformation : Window
    {
        private readonly IUserService _userService;
        private readonly User _doctorToUpdate;
        private readonly bool _isEditMode;
        public AddUpdateDoctorInformation()
        {
            InitializeComponent();
            _userService = new UserService();
            _userService = new UserService();
            _isEditMode = false;
        }
        public AddUpdateDoctorInformation(User doctorToEdit)
        {
            InitializeComponent();
            _userService = new UserService();
            _doctorToUpdate = doctorToEdit;
            _isEditMode = true;

            // Populate existing values
            if (_doctorToUpdate != null)
            {
                NameTextBox.Text = _doctorToUpdate.UserName;
                PhoneTextBox.Text = _doctorToUpdate.PhoneNumber;
                FullNameTextBox.Text = _doctorToUpdate.FullName;
                AgeTextBox.Text = _doctorToUpdate.Age.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string doctorName = NameTextBox.Text.Trim();
                string phoneNumber = PhoneTextBox.Text.Trim();
                string fullName  = FullNameTextBox.Text.Trim();
                string ageText = AgeTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(doctorName) || string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("Doctor name and phone cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(ageText, out int age))
                {
                    MessageBox.Show("Invalid age format.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_isEditMode)
                {
                    // Update existing doctor
                    _doctorToUpdate.FullName = fullName;
                    _doctorToUpdate.PhoneNumber = phoneNumber;
                    _doctorToUpdate.UserName = doctorName; // Assuming UserName should be the same as FullName
                    _doctorToUpdate.Age = age;

                    _userService.UpdateUser(_doctorToUpdate, _doctorToUpdate.UserId);
                    MessageBox.Show("Doctor updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Add new doctor
                    User newDoctor = new User
                    {
                        UserName = doctorName,
                        FullName = fullName,
                        PhoneNumber = phoneNumber,
                        Age = age,
                        Password = "Doctor",
                        RoleId = 3,
                        IsActive = true
                    };
                    _userService.AddUsere(newDoctor);
                    MessageBox.Show("Doctor added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
