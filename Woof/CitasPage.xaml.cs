using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Woof
{
    public partial class CitasPage : ContentPage
    {
        private ObservableCollection<Cita> _citas;
        private Cita _citaSeleccionada;

        public CitasPage()
        {
            InitializeComponent();
            FechaPicker.Date = DateTime.Now;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarCitas();
        }

        private async Task CargarCitas()
        {
            var lista = await App.Database.Table<Cita>().ToListAsync();
            _citas = new ObservableCollection<Cita>(lista);
            CitasListView.ItemsSource = _citas;
        }

        private async void OnAgregarCitaClicked(object sender, EventArgs e)
        {
            var mascota = MascotaNombreEntry.Text?.Trim();
            var motivo = MotivoEntry.Text?.Trim();
            var fecha = FechaPicker.Date.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(mascota) || string.IsNullOrEmpty(motivo))
            {
                await DisplayAlert("Error", "Complete todos los campos.", "OK");
                return;
            }

            var nueva = new Cita
            {
                MascotaNombre = mascota,
                Fecha = fecha,
                Motivo = motivo
            };

            await App.Database.InsertAsync(nueva);
            _citas.Add(nueva);

            MascotaNombreEntry.Text = string.Empty;
            MotivoEntry.Text = string.Empty;
            FechaPicker.Date = DateTime.Now;
        }

        // POPUP AL SELECCIONAR UNA CITA
        private async void OnCitaSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            _citaSeleccionada = e.SelectedItem as Cita;

            string action = await DisplayActionSheet(
                "Opciones",
                "Cancelar",
                null,
                "Ver información"
            );

            if (action == "Ver información")
            {
                await DisplayAlert(
                    "Detalles de la cita",
                    $"Mascota: {_citaSeleccionada.MascotaNombre}\n" +
                    $"Fecha: {_citaSeleccionada.Fecha}\n" +
                    $"Motivo: {_citaSeleccionada.Motivo}",
                    "OK");
            }

            CitasListView.SelectedItem = null;
        }

        private async void OnEliminarCitaClicked(object sender, EventArgs e)
        {
            if (_citaSeleccionada == null)
            {
                await DisplayAlert("Aviso", "Seleccione una cita.", "OK");
                return;
            }

            bool aceptar = await DisplayAlert(
                "Confirmar",
                $"¿Eliminar la cita de {_citaSeleccionada.MascotaNombre}?",
                "Sí", "No");

            if (!aceptar)
                return;

            await App.Database.DeleteAsync(_citaSeleccionada);
            _citas.Remove(_citaSeleccionada);

            _citaSeleccionada = null;
            CitasListView.SelectedItem = null;
        }

        // BOTÓN REGRESAR
        private async void OnRegresarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
