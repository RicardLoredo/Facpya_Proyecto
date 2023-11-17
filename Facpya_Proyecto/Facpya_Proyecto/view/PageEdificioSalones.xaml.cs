using Facpya_Proyecto.modelo;
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
	public partial class PageEdificioSalones : ContentPage
	{
        public PageEdificioSalones(Edificio edificio)
        {
            InitializeComponent();
            BindingContext = new EdificioSalonesViewModel(edificio);
        }
    }
}