using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Helpers;
using RegistroEstudiantes.Modelos.Modelos;
using System.Collections.ObjectModel;


namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class ListarEstudiantes : ContentPage
{
	FirebaseClient client = new FirebaseClient(FireBaseConfig.DatabaseUrl);
	public ObservableCollection<Estudiante>Lista { get; set; } = new ObservableCollection<Estudiante>();
	public ListarEstudiantes()
	{
		InitializeComponent();
		BindingContext = this;
		CargarLista();
	}

    private async void CargarLista()
    {
        Lista.Clear();
        var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();

        var estudiantesActivos = estudiantes.Where(e => e.Object.Estado == true).ToList();

        foreach (var estudiante in estudiantesActivos)
        {
            Lista.Add(new Estudiante
            {
                Id = estudiante.Key,
                PNombre = estudiante.Object.PNombre,
                SNombre = estudiante.Object.SNombre,
                PApellido = estudiante.Object.PApellido,
                SApellido = estudiante.Object.SApellido,
                Edad = estudiante.Object.Edad,
                CElectronico = estudiante.Object.CElectronico,
                Estado = estudiante.Object.Estado,
                Curso = estudiante.Object.Curso,
            });
        }
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
		string filtro = filtroSearchBar.Text.ToLower();
		if(filtro.Length > 0)
		{
			ListaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
		else
		{
            ListaCollection.ItemsSource = Lista;
        }
    }

    private async void CrearEstudianteBtn_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new CrearEstudiante());
    }

    private async void editarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante != null && !string.IsNullOrEmpty(estudiante.Id))
        {
            await Navigation.PushAsync(new EditarEstudiante(estudiante.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la infromacion del estudiante", "OK");
        }
    }

    private async void deshabilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if(estudiante == null)
        {
            await DisplayAlert("Erroe", "No se pudo obtener la informacion del estudiante", "OK");
            return;
        }

        bool confirmacion = await DisplayAlert("Confirmacion", $"Seguro de que deseas deshabilitar al estudiante {estudiante.NombreCompleto}", "Si", "No");

        if (confirmacion)
        {
            try
            {
                estudiante.Estado = false;
                await client.Child("Estudiantes").Child(estudiante.Id).PutAsync(estudiante);
                await DisplayAlert("Exito", $"Se ha deshabilitado al estudiante {estudiante.NombreCompleto}", "OK");
                CargarLista();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}