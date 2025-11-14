using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Woof
{
    public partial class VeterinariosPage : ContentPage
    {
        private ObservableCollection<Veterinario> _veterinarios;
        private Veterinario _veterinarioSeleccionado;

        public VeterinariosPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarVeterinarios();
        }

        private async Task CargarVeterinarios()
        {
            var lista = await App.Database.Table<Veterinario>().ToListAsync();
            _veterinarios = new ObservableCollection<Veterinario>(lista);
            VeterinariosListView.ItemsSource = _veterinarios;
        }

        private async void OnAgregarVeterinarioClicked(object sender, EventArgs e)
        {
            var nombre = VetNombreEntry.Text?.Trim();
            var especialidad = VetEspecialidadEntry.Text?.Trim();
            var telefono = VetTelefonoEntry.Text?.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("Error", "El nombre es obligatorio.", "OK");
                return;
            }

            var nuevo = new Veterinario
            {
                Nombre = nombre,
                Especialidad = especialidad,
                Telefono = telefono
            };

            await App.Database.InsertAsync(nuevo);
            _veterinarios.Add(nuevo);

            VetNombreEntry.Text = VetEspecialidadEntry.Text = VetTelefonoEntry.Text = string.Empty;
        }

        private void OnVeterinarioSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _veterinarioSeleccionado = e.SelectedItem as Veterinario;
        }

        private async void OnEliminarVeterinarioClicked(object sender, EventArgs e)
        {
            if (_veterinarioSeleccionado == null)
            {
                await DisplayAlert("Aviso", "Seleccione un veterinario primero.", "OK");
                return;
            }

            var aceptar = await DisplayAlert(
                "Confirmar",
                $"¿Eliminar al veterinario {_veterinarioSeleccionado.Nombre}?",
                "Sí", "No");

            if (!aceptar) return;

            await App.Database.DeleteAsync(_veterinarioSeleccionado);
            _veterinarios.Remove(_veterinarioSeleccionado);
            _veterinarioSeleccionado = null;
            VeterinariosListView.SelectedItem = null;
        }

        private async void OnRegresarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
