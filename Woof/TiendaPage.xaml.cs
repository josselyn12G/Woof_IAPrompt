using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Woof
{
    public partial class TiendaPage : ContentPage
    {
        private ObservableCollection<ProductoTienda> _productos;
        private ProductoTienda _productoSeleccionado;

        public TiendaPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarProductos();
        }

        private async Task CargarProductos()
        {
            var lista = await App.Database.Table<ProductoTienda>().ToListAsync();
            _productos = new ObservableCollection<ProductoTienda>(lista);
            ProductosListView.ItemsSource = _productos;
        }

        // ====================================================
        // MÉTODO PARA AGREGAR PRODUCTO 
        // ====================================================
        private async void OnAgregarProductoClicked(object sender, EventArgs e)
        {
            var nombre = ProductoNombreEntry.Text?.Trim();
            var precioTexto = ProductoPrecioEntry.Text?.Trim();
            var stockTexto = ProductoStockEntry.Text?.Trim();

            if (string.IsNullOrEmpty(nombre) ||
                string.IsNullOrEmpty(precioTexto) ||
                string.IsNullOrEmpty(stockTexto))
            {
                await DisplayAlert("Error", "Complete todos los campos.", "OK");
                return;
            }

            if (!decimal.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
            {
                await DisplayAlert("Error", "Precio inválido.", "OK");
                return;
            }

            if (!int.TryParse(stockTexto, out var stock))
            {
                await DisplayAlert("Error", "Stock inválido.", "OK");
                return;
            }

            var nuevo = new ProductoTienda
            {
                Nombre = nombre,
                Precio = precio,
                Stock = stock
            };

            await App.Database.InsertAsync(nuevo);
            _productos.Add(nuevo);

            // limpiar campos
            ProductoNombreEntry.Text = "";
            ProductoPrecioEntry.Text = "";
            ProductoStockEntry.Text = "";
        }


        // ====================================================
        // SELECCIONAR PRODUCTO
        // ====================================================
        private void OnProductoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _productoSeleccionado = e.SelectedItem as ProductoTienda;
        }


        // ====================================================
        // ELIMINAR PRODUCTO
        // ====================================================
        private async void OnEliminarProductoClicked(object sender, EventArgs e)
        {
            if (_productoSeleccionado == null)
            {
                await DisplayAlert("Aviso", "Seleccione un producto primero.", "OK");
                return;
            }

            var aceptar = await DisplayAlert(
                "Confirmar",
                $"¿Eliminar el producto {_productoSeleccionado.Nombre}?",
                "Sí", "No");

            if (!aceptar) return;

            await App.Database.DeleteAsync(_productoSeleccionado);
            _productos.Remove(_productoSeleccionado);

            _productoSeleccionado = null;
            ProductosListView.SelectedItem = null;
        }


        // ====================================================
        // BOTÓN REGRESAR
        // ====================================================
        private async void OnRegresarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
