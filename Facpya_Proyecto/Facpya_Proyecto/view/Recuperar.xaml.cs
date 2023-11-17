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
    public partial class Recuperar : ContentPage
    {
        public Recuperar()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var email = TxtEmailRegistrado.Text;


            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            bool emailExists = await ApiServiceFirebase.VerificarCorreo(email);
            if (!emailExists)
            {
                await DisplayAlert("Error", "Correo no existe.", "OK");
                return;
            }

            bool resetEmailSent = await ApiServiceAuthentication.ResetPassword(email);
            if (resetEmailSent)
            {
                await DisplayAlert("Éxito", "Se ha enviado un correo electrónico para restablecer la contraseña.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo enviar el correo electrónico de restablecimiento.", "OK");
            }
        }
    }
}