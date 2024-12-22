using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroEstudiantes.Modelos.Helpers;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();

#endif
            ActualizarCursos();
            ActualizarEstudiantes();
            return builder.Build();
        }

        public static async Task ActualizarCursos()
        {
            FirebaseClient client = new FirebaseClient(FireBaseConfig.DatabaseUrl);

            var cursos = await client.Child("Cursos").OnceAsync<Curso>();

            if (cursos.Count == 0)
            {
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Primero Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Segundo Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Tercero Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Cuarto Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Quinto Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Sexto Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Septimo Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Octavo Basico" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Primero Medio" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Segundo Medio" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Tercero Medio" });
                await client.Child("Cursos").PostAsync(new Curso { Nombre = "Cuarto Medio" });
            }
            else
            {
                foreach (var curso in cursos)
                {
                    if (curso.Object.Estado == null)
                    {
                        var cursoActualizado = curso.Object;
                        cursoActualizado.Estado = true;

                        await client.Child("Cursos").Child(curso.Key).PutAsync(cursoActualizado);
                    }
                }
            }
        }

        public static async Task ActualizarEstudiantes()
        {
            FirebaseClient client = new FirebaseClient(FireBaseConfig.DatabaseUrl);

            var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();
            foreach (var estudiante in estudiantes)
            {
                if (estudiante.Object.Estado == null)
                {
                    var estudianteActualizado = estudiante.Object;
                    estudianteActualizado.Estado = true;

                    await client.Child("Estudiantes").Child(estudiante.Key).PutAsync(estudianteActualizado);
                }
            }

        }
    }
}
