using Facpya_Proyecto.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Windows.Input;
using Facpya_Proyecto.Service;

namespace Facpya_Proyecto.view
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRegistroEdificio : ContentPage

    {
        private AddEdificioViewModel _viewModel;
        public PageRegistroEdificio()
        {

            InitializeComponent();
            _viewModel = new AddEdificioViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}