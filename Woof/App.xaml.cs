using Microsoft.Maui.Controls;
using SQLite;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Woof
{
    public partial class App : Application
    {
        public static SQLiteAsyncConnection Database { get; private set; }

        public App()
        {
            InitializeComponent();

            InitDatabase();

            MainPage = new NavigationPage(new LoginPage());
        }

        private void InitDatabase()
        {
            if (Database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "woof.db3");
            Database = new SQLiteAsyncConnection(dbPath);

            // Crear tablas y usuario demo
            Task.Run(async () =>
            {
                await Database.CreateTableAsync<Usuario>();
                await Database.CreateTableAsync<Cita>();
                await Database.CreateTableAsync<Mascota>();
                await Database.CreateTableAsync<ProductoTienda>();
                await Database.CreateTableAsync<Veterinario>();

                // Usuario por defecto si no existe ninguno
                var count = await Database.Table<Usuario>().CountAsync();
                if (count == 0)
                {
                    await Database.InsertAsync(new Usuario
                    {
                        Username = "admin",
                        Password = "1234"
                    });
                }
            }).Wait();
        }
    }

    // ======== MODELOS SQLITE ========

    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [SQLite.MaxLength(50), Unique]
        public string Username { get; set; }

        [SQLite.MaxLength(50)]
        public string Password { get; set; }
    }

    public class Cita
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string MascotaNombre { get; set; }
        public string Fecha { get; set; }   // Para simple demo; en real usar DateTime
        public string Motivo { get; set; }
    }

    public class Mascota
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
    }

    public class ProductoTienda
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class Veterinario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Telefono { get; set; }
    }
}
