using Facpya_Proyecto.modelo;
using Facpya_Proyecto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facpya_Proyecto.view
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        public Registro()
        {
            InitializeComponent();
        }

        private async void ButtonRegistrar_Clicked(object sender, EventArgs e)
        {
            Usuario oUsuario = new Usuario()
            {
                Rol = "",
                Nombres = TxtName.Text,
                Email = TxtEmail.Text,
                Clave = TxtPassword.Text
            };

            bool respuesta = await ApiServiceAuthentication.RegistrarUsuario(oUsuario);

            if (respuesta)
            {
                await DisplayAlert("Correcto", "Usuario registrado", "Aceptar");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Oops", "No se pudo registrar", "Aceptar");
            }

        }
    }
}