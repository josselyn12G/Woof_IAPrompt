using Microsoft.Maui.Controls;
using System;

namespace Woof
{
    public partial class MainPage : ContentPage
    {
        private readonly Usuario _usuario;

        public MainPage(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;

            SaludoLabel.Text = $"Hola, {_usuario.Username} 👋";
        }

        private async void OnCitasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CitasPage());
        }

        private async void OnTiendaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TiendaPage());
        }

        private async void OnMascotasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MascotasPage());
        }

        private async void OnVeterinariosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VeterinariosPage());
        }
        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Confirmar",
                "¿Está seguro de que desea cerrar sesión?",
                "Sí", "No");

            if (!confirmar)
                return;

            await Navigation.PopToRootAsync();
        }


    }
}
