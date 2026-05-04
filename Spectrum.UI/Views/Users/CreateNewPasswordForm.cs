using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Microsoft.AspNet.Identity;
using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Users;
using Spectrum.Models.Users;
using Spectrum.Utilities.Common;
using System;
using System.Text;
using System.Windows.Forms;

namespace Spectrum.Views.Users
{
	public partial class CreateNewPasswordForm : XtraForm
	{
		private UserModel _userModel = new UserModel();
		private readonly UserRepository _userRepository = new UserRepository(DatabaseFactory.ProfilePrimary);

		public event EventHandler SendChangedPassword;

		public CreateNewPasswordForm(UserModel model)
		{
			InitializeComponent();

			_userModel = model;

			InitializeBindings();
			WireUpBindings();
			ApplyDefaults();
			ApplyPermissions();
		}

		private void InitializeBindings()
		{

		}

		private void WireUpBindings()
		{

		}

		private void ApplyDefaults()
		{
			txtUsername.Text = _userModel.Username;
		}

		private void ApplyPermissions()
		{

		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (!ValidateForm()) return;

			try
			{
				SystemUtilities.PasswordHasher = new PasswordHasher();

				_userModel.PasswordHash = SystemUtilities.PasswordHasher.HashPassword(txtNewPassword.Text);
				_userModel.ChangePasswordNextLogon = false;
				_userModel.FirstTimeAccess = false;

				var updateResult = await _userRepository.UpdateUserAsync(_userModel);

				if (!updateResult)
				{
					throw new Exception("Error while changing password\n- Contact your system administrator.");
				}

				XtraMessageBox.Show("Password changed!",
					"Password notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

				SendChangedPassword(_userModel, EventArgs.Empty);
				DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception exception)
			{
				XtraMessageBox.Show(exception.Message, "Change Password",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private bool ValidateForm()
		{
			var validateReturnValue = true;
			var messageNumber = 0;
			var validateMessage = new StringBuilder();

			if (txtConfirmPassword.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Confirm Password cannot be empty.");
				validateReturnValue = false;
				txtConfirmPassword.Focus();
			}

			if (txtNewPassword.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- New Password cannot be empty.");
				validateReturnValue = false;
				txtNewPassword.Focus();
			}


			if (txtUsername.Text == "")
			{
				messageNumber += 1;
				validateMessage.Append("\n- Username cannot be empty.");
				validateReturnValue = false;
				txtUsername.Focus();
			}

			if (txtNewPassword.Text != txtConfirmPassword.Text)
			{
				messageNumber += 1;
				validateMessage.Append("\n- New Password and Confirm Password do not match.");
				validateReturnValue = false;
				txtConfirmPassword.Focus();
			}


			if (!validateReturnValue)
			{
				validateMessage.Insert(0, "The following need your attention:");
				if (messageNumber > 1) validateMessage.Replace("following", "followings");
				XtraMessageBox.Show(validateMessage + " \nPlease try again.",
					"Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return validateReturnValue;
		}

		private void txtNewPassword_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			ButtonEdit edit = sender as ButtonEdit;
			if (edit != null) edit.Properties.PasswordChar = (edit.Properties.PasswordChar == '*') ? '\0' : '*';
		}

		private void txtNewPassword_MouseLeave(object sender, EventArgs e)
		{
			ButtonEdit edit = sender as ButtonEdit;
			if (edit != null) edit.Properties.PasswordChar = edit.Properties.PasswordChar = '*';
		}

		private void txtConfirmPassword_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			ButtonEdit edit = sender as ButtonEdit;
			if (edit != null) edit.Properties.PasswordChar = (edit.Properties.PasswordChar == '*') ? '\0' : '*';
		}

		private void txtConfirmPassword_MouseLeave(object sender, EventArgs e)
		{
			ButtonEdit edit = sender as ButtonEdit;
			if (edit != null) edit.Properties.PasswordChar = edit.Properties.PasswordChar = '*';
		}
	}
}