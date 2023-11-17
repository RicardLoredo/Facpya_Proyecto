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
    public partial class PageActualizarEdificio : ContentPage

    {
        public PageActualizarEdificio(Edificio edificio)
        {
            InitializeComponent();

            var viewModel = new AddOrUpdateEdificioViewModel(edificio);
            this.BindingContext = viewModel;
        }

    }
}