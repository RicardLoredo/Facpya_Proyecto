using AppNotas.modelo;
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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Label_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new view.Recuperar());
        }


        private async void registroButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new view.Registro());
        }

        private async void ingresarButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPassword.Text) || string.IsNullOrWhiteSpace(TxtCorreo.Text))
            {
                await DisplayAlert("Oops", "Ingrese todos los datos", "Aceptar");
                return;
            }

            UserAuthentication oUsuario = new UserAuthentication()
            {
                email = TxtCorreo.Text,
                password = TxtPassword.Text,
                returnSecureToken = true
            };

            bool respuesta = await ApiServiceAuthentication.Login(oUsuario);
            if (respuesta)
            {
                Usuario usuario = await ApiServiceFirebase.ObtenerUsuario();
                string nombreUsuario = usuario.Nombres;
                string rolUsuario = usuario.Rol;
                Application.Current.MainPage = new NavigationPage(new Inicio(nombreUsuario, rolUsuario));
            }
            else
            {
                await DisplayAlert("Oops", "Usuario no encontrado", "OK");
            }
        }
    }
}