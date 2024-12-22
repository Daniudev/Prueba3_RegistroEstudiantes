using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;
using RegistroEstudiantes.Modelos.Helpers;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging.Abstractions;
namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class EditarEstudiante : ContentPage
{
	FirebaseClient client = new FirebaseClient(FireBaseConfig.DatabaseUrl);

    public List<Curso> Cursos { get; set; }

    public ObservableCollection<string> ListaCursos { get; set; } = new ObservableCollection<string>();

    private Estudiante estudianteActualizado = new Estudiante();

    private string estudianteId;
    public EditarEstudiante(string idEstudiante)
	{
		InitializeComponent();
        BindingContext = this;
        estudianteId = idEstudiante;
        CargarListaCursos();
        CargarEstudiante(estudianteId);
    }

    private async void CargarListaCursos()
    {
        try
        {
            var cursos = await client.Child("Cursos").OnceAsync<Curso>();
            ListaCursos.Clear();
            foreach (var curso in cursos)
            {
                ListaCursos.Add(curso.Object.Nombre);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private async void CargarEstudiante(string idEstudiante)
    {
        var estudiante = await client.Child("Estudiantes").Child(idEstudiante).OnceSingleAsync<Estudiante>();

        if (estudiante != null)
        {
            EditPNombreEntry.Text = estudiante.PNombre;
            EditSNombreEntry.Text = estudiante.SNombre;
            EditPApellidoEntry.Text = estudiante.PApellido;
            EditSApellidoEntry.Text = estudiante.SApellido;
            EditEdadEntry.Text = estudiante.Edad.ToString();
            EditCorreoEntry.Text = estudiante.CElectronico;
            EditCursoPicker.SelectedItem = estudiante.Curso?.Nombre;
        }
    }

    private async void ActualizarEstudianteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditPNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditSNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditPApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditSApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditEdadEntry.Text) ||
                string.IsNullOrWhiteSpace(EditCorreoEntry.Text) ||
                EditCursoPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Todos los campos son requeridos", "Ok");
                return;
            }

            if (!EditCorreoEntry.Text.Contains("@"))
            {
                await DisplayAlert("Error", "Correo electrónico inválido", "Ok");
                return;
            }

            if (!int.TryParse(EditEdadEntry.Text, out int edad))
            {
                await DisplayAlert("Error", "Edad inválida", "Ok");
                return;
            }

            if (edad < 5)
            {
                await DisplayAlert("Error", "El alumno no puede ser menor de 5 años.", "Ok");
                return;
            }

            estudianteActualizado.Id = estudianteId;
            estudianteActualizado.PNombre = EditPNombreEntry.Text.Trim();
            estudianteActualizado.SNombre = EditSNombreEntry.Text.Trim();
            estudianteActualizado.PApellido = EditPApellidoEntry.Text.Trim();
            estudianteActualizado.SApellido = EditSApellidoEntry.Text.Trim();
            estudianteActualizado.Edad = edad;
            estudianteActualizado.CElectronico = EditCorreoEntry.Text.Trim();
            estudianteActualizado.Curso = new Curso { Nombre = EditCursoPicker.SelectedItem.ToString() };
            estudianteActualizado.Estado = estadoSwitch.IsToggled;

            await client.Child("Estudiantes").Child(estudianteActualizado.Id).PutAsync(estudianteActualizado);

            await DisplayAlert("Éxito", "Estudiante actualizado correctamente", "Ok");
            await Navigation.PopAsync();

        }
        catch (Exception)
        {
            throw;
        }
    }
}