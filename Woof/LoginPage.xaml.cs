using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Woof
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            var username = UsernameEntry.Text?.Trim();
            var password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorLabel.Text = "Ingrese usuario y contraseña.";
                ErrorLabel.IsVisible = true;
                return;
            }

            var user = await App.Database.Table<Usuario>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                ErrorLabel.Text = "Usuario o contraseña incorrectos.";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Navegar a la página principal
            await Navigation.PushAsync(new MainPage(user));
        }
    }
}
