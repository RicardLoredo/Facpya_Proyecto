using Facpya_Proyecto.modelo;
using Facpya_Proyecto.Service;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class Inicio : ContentPage
    {
        private string _nombreUsuario;
        private bool _esAdmin;

        public string NombreUsuario
        {
            get => _nombreUsuario;
            set
            {
                _nombreUsuario = value;
                OnPropertyChanged(nameof(NombreUsuario)); 
            }
        }

        public bool EsAdmin
        {
            get => _esAdmin;
            set
            {
                _esAdmin = value;
                OnPropertyChanged(nameof(EsAdmin));
            }
        }

        MostrarEdificiosVM _viewModel;

        public Inicio(string nombreUsuario, string rolUsuario)
        {
            InitializeComponent();
            _nombreUsuario = nombreUsuario;
            _esAdmin = rolUsuario == "Admin";
            _viewModel = new MostrarEdificiosVM();
            BindingContext = _viewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            addButton.IsVisible = _esAdmin;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadEdificios();
        }

        public async void OnAddImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new view.PageRegistroEdificio());
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }



    }
}