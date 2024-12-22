using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;
using RegistroEstudiantes.Modelos.Helpers;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class CrearEstudiante : ContentPage
{
    FirebaseClient client = new FirebaseClient(FireBaseConfig.DatabaseUrl);
    public List<Curso> Cursos { get; set; }
    public CrearEstudiante()
	{
		InitializeComponent();
		ListarCursos();
        BindingContext = this;
    }

    private void ListarCursos()
    {
        var cursos = client.Child("Cursos").OnceAsync<Curso>();
        Cursos=cursos.Result.Select(x=>x.Object).ToList();
    }

    private async void guardarBtn_Clicked(object sender, EventArgs e)
    {
        Curso curso = cursoPicker.SelectedItem as Curso;

        var estudiante = new Estudiante
        {
            PNombre = primerNombreEntry.Text,
            SNombre = segundoNombreEntry.Text,
            PApellido = primerApellidoEntry.Text,
            SApellido = segundoApellidoEntry.Text,
            Edad = int.Parse(edadEntry.Text),
            CElectronico = correoEntry.Text,
            Curso = curso,
            Estado = true
        };

        try
        {
            await client.Child("Estudiantes").PostAsync(estudiante);

            await DisplayAlert("Guardado", $"El Estudiante {estudiante.PNombre} {estudiante.PApellido} se ingreso correctamente al sistema","OK");

            await Navigation.PopAsync();

            
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}