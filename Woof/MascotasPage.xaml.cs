using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Woof
{
    public partial class MascotasPage : ContentPage
    {
        private ObservableCollection<Mascota> _mascotas;
        private Mascota _mascotaSeleccionada;

        public MascotasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarMascotas();
        }

        private async Task CargarMascotas()
        {
            var lista = await App.Database.Table<Mascota>().ToListAsync();
            _mascotas = new ObservableCollection<Mascota>(lista);
            MascotasListView.ItemsSource = _mascotas;
        }

        private async void OnAgregarMascotaClicked(object sender, EventArgs e)
        {
            var nombre = MascotaNombreEntry.Text?.Trim();
            var especie = MascotaEspecieEntry.Text?.Trim();
            var raza = MascotaRazaEntry.Text?.Trim();

            if (string.IsNullOrEmpty(nombre) ||
                string.IsNullOrEmpty(especie))
            {
                await DisplayAlert("Error", "Nombre y especie son obligatorios.", "OK");
                return;
            }

            var nueva = new Mascota
            {
                Nombre = nombre,
                Especie = especie,
                Raza = raza
            };

            await App.Database.InsertAsync(nueva);
            _mascotas.Add(nueva);

            MascotaNombreEntry.Text = MascotaEspecieEntry.Text = MascotaRazaEntry.Text = string.Empty;
        }

        private void OnMascotaSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _mascotaSeleccionada = e.SelectedItem as Mascota;
        }

        private async void OnEliminarMascotaClicked(object sender, EventArgs e)
        {
            if (_mascotaSeleccionada == null)
            {
                await DisplayAlert("Aviso", "Seleccione una mascota primero.", "OK");
                return;
            }

            var aceptar = await DisplayAlert(
                "Confirmar",
                $"¿Eliminar a {_mascotaSeleccionada.Nombre}?",
                "Sí", "No");

            if (!aceptar) return;

            await App.Database.DeleteAsync(_mascotaSeleccionada);
            _mascotas.Remove(_mascotaSeleccionada);
            _mascotaSeleccionada = null;
            MascotasListView.SelectedItem = null;
        }

        private async void OnRegresarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
