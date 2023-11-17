using AppNotas.modelo;
using Facpya_Proyecto.modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Facpya_Proyecto
{
    public class AppSettings
    {
        public static string ApiFirebase = "https://prueba-8060c-default-rtdb.firebaseio.com/";
        private static string KeyAplication = "AIzaSyDxnHygsG4-E17KQkoUZk55xjSbJhZlTkE";

        public static ResponseAuthentication oAuthentication { get; set; }
        private static string ApiUrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";

        public static string ApiAuthentication(string tipo)
        {
            if (tipo == "LOGIN")
            {

                return ApiUrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;
            }
            else if (tipo == "SIGNIN")
            {

                return ApiUrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
            }
            else if (tipo == "CHANGE_PASSWORD")
            {

                return ApiUrlGoogleApis + "accounts:update?key=" + KeyAplication;
            }
            else if (tipo == "RESET_PASSWORD")
            {

                return ApiUrlGoogleApis + "accounts:sendOobCode?key=" + KeyAplication;
            }
            else
            {
                return "";
            }
        }

    }
}
