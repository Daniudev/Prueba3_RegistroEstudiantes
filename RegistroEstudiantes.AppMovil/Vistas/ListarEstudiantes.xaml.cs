using Firebase.Database;
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

    private void CargarLista()
    {
		client.Child("Estudiantes").AsObservable<Estudiante>().Subscribe((estudiante) =>
		{
			if (estudiante != null)
			{
				Lista.Add(estudiante.Object);
			}
		});
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
}