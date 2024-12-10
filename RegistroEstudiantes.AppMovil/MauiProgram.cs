using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;
using RegistroEstudiantes.Modelos.Helpers;
using Microsoft.Extensions.Logging;

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
            RegistrarEstudiante();
            return builder.Build();
        }

        public static void RegistrarEstudiante()
        {
            FirebaseClient client = new FirebaseClient(FireBaseConfig.DatabaseUrl);

            var cursos = client.Child("Cursos").OnceAsync<Curso>();

            if (cursos.Result.Count == 0)
            {
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Primero Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Segundo Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Tercero Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Cuarto Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Quinto Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Sexto Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Septimo Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Octavo Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Primero Medio" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Segundo Medio" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Tercero Medio" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "Cuarto Medio" });
            }
        }
    }
}
