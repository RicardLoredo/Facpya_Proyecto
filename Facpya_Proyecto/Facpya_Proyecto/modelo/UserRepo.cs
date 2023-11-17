using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace Facpya_Proyecto.modelo
{
    public class Usuario
    {
        public string Rol { get; set; }
        public string Nombres { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
    }
}
