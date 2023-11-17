using AppNotas.modelo;
using Facpya_Proyecto;
using Facpya_Proyecto.modelo;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Facpya_Proyecto.Service
{
    public class ApiServiceFirebase
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("prueba-8060c.appspot.com");

        FirebaseClient firebase = new FirebaseClient("https://prueba-8060c-default-rtdb.firebaseio.com/");

        public async Task<List<Edificio>> GetEdificiosAsync()
        {
            var edificios = await firebase
                .Child("edificios")
                .OnceAsync<Edificio>();

            return edificios.Select(item => new Edificio
            {
                Id = item.Object.Id,
                Name = item.Object.Name,
                ImageUrl = item.Object.ImageUrl,
                SketchImageUrl = item.Object.SketchImageUrl,
                VideoUrl = item.Object.VideoUrl
            }).ToList();
        }

        public async Task UpdateEdificio(Edificio edificio)
        {
            // Aquí asumimos que tu objeto Edificio tiene una propiedad Id que es la clave en Firebase
            if (!string.IsNullOrEmpty(edificio.Id))
            {
                await firebase
                    .Child("edificios")
                    .Child(edificio.Id)
                    .PutAsync(edificio);
            }
            else
            {
                throw new Exception("El edificio no tiene una ID válida.");
            }
        }


        public async Task AddEdificio(Edificio edificio)
        {
            var databaseReference = firebase.Child("edificios");

            if (string.IsNullOrEmpty(edificio.Id))
            {
                var newEdificio = await databaseReference.PostAsync(edificio);
                edificio.Id = newEdificio.Key;
                await databaseReference.Child(edificio.Id).PutAsync(edificio);

            }
            else
            {
                // Actualiza un edificio existente
                await databaseReference
                    .Child(edificio.Id)
                    .PutAsync(edificio);
            }
        }

        public async Task DeleteEdificio(string edificioId)
        {
            await firebase.Child("edificios").Child(edificioId).DeleteAsync();
        }



        /*public async Task AddSalonToEdificio(string edificioKey, Salon salon)
        {
            // Agrega un salón a un edificio específico en la base de datos
            await firebase
              .Child("Edificios")
              .Child(edificioKey)
              .Child("Salones")
              .PostAsync(salon);
        }*/

        public async Task<string> UploadFile(Stream fileStream, string folderName, string fileName)
        {
            var storageReference = firebaseStorage.Child(folderName).Child(fileName);
            var uploadTask = storageReference.PutAsync(fileStream);
            await uploadTask;
            return await storageReference.GetDownloadUrlAsync();
        }

        public async Task SaveEdificio(Edificio edificio, Stream imageStream, Stream sketchStream, Stream videoStream)
        {

            if (imageStream != null)
            {
                edificio.ImageUrl = await UploadFile(imageStream, "edificios/images", $"image_{Guid.NewGuid()}.jpg");
            }
            if (sketchStream != null)
            {
                edificio.SketchImageUrl = await UploadFile(sketchStream, "edificios/sketches", $"sketch_{Guid.NewGuid()}.jpg");
            }
            if (videoStream != null)
            {
                edificio.VideoUrl = await UploadFile(videoStream, "edificios/videos", $"video_{Guid.NewGuid()}.mp4");
            }

            // Después de subir los archivos y obtener las URLs, guarda el objeto Edificio en la base de datos
            // Asumiendo que tienes un método AddEdificio que hace esto:
            await AddEdificio(edificio);
        }

        public static async Task<bool> RegistrarUsuario(Usuario oUsuario, ResponseAuthentication oResponse)
        {
            try
            {

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var formatoapi = string.Concat(AppSettings.ApiFirebase, "{0}/{1}.json?auth={2}");

                var response = await client.PutAsync(
                    string.Format(formatoapi, "usuarios", oResponse.LocalId, oResponse.IdToken),
                    content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }



        public static async Task<Usuario> ObtenerUsuario()
        {
            Usuario oObject = new Usuario();
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/{0}.json?auth={1}");
                var response = await client.GetAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        oObject = JsonConvert.DeserializeObject<Usuario>(jsonstring);
                    }
                    return oObject;
                }
                else
                {
                    return oObject;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return oObject;
            }

        }

        public static async Task<bool> GuardarCambiosUsuario(Usuario oUsuario)
        {
            Usuario oObject = new Usuario();
            try
            {
                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");


                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/{0}.json?auth={1}");
                var response = await client.PutAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        public static async Task<bool> VerificarCorreo(string email)
        {
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios.json?orderBy=\"Email\"&equalTo=\"", Uri.EscapeDataString(email), "\"");
                var response = await client.GetAsync(apiformat);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Dictionary<string, Usuario>>(jsonstring);
                    return result.Any();
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                // Manejo de excepciones
            }

            return false;
        }

        public static async Task<bool> CambiarContraseña(string email, string nuevaContraseña)
        {
            try
            {
                // Descargar todos los usuarios
                HttpClient client = new HttpClient();
                string urlUsuarios = string.Concat(AppSettings.ApiFirebase, "usuarios.json");
                var responseUsuarios = await client.GetAsync(urlUsuarios);

                if (!responseUsuarios.IsSuccessStatusCode)
                {
                    return false;
                }

                var jsonUsuarios = await responseUsuarios.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<Dictionary<string, Usuario>>(jsonUsuarios);

                // Buscar el usuario que coincide con el correo electrónico
                var usuarioId = usuarios.FirstOrDefault(u => u.Value.Email == email).Key;
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return false; // Usuario no encontrado
                }

                // Actualizar la contraseña del usuario encontrado
                string nuevaClaveJson = JsonConvert.SerializeObject(nuevaContraseña);
                var content = new StringContent(nuevaClaveJson, Encoding.UTF8, "application/json");
                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/", usuarioId, "/Clave.json");
                var response = await client.PutAsync(apiformat, content);

                return response.StatusCode.Equals(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }
        }
    }
}
