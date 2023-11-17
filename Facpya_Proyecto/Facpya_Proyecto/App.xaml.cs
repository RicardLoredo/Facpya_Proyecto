using Facpya_Proyecto.view;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facpya_Proyecto
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationStyle = new Style(typeof(NavigationPage))
            {
                Setters = {
                    new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.FromHex("#9D0924") },
                    new Setter { Property = NavigationPage.BarTextColorProperty, Value = Color.White }
                }
            };
            Resources = new ResourceDictionary();
            Resources.Add(navigationStyle);

            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
