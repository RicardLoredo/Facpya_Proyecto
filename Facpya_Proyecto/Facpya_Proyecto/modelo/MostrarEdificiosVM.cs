using Facpya_Proyecto.Service;
using Facpya_Proyecto.view;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Facpya_Proyecto.modelo
{
    public class MostrarEdificiosVM : BindableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ApiServiceFirebase _firebaseService;

        private ObservableCollection<Edificio> _listaDeEdificios;



        public ICommand ImageTappedCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }

        public ObservableCollection<Edificio> ListaDeEdificios
        {
            get => _listaDeEdificios;
            set
            {
                _listaDeEdificios = value;
                OnPropertyChanged(nameof(ListaDeEdificios));
            }
        }

        public Command AddEdificioCommand { get; set; }

        public MostrarEdificiosVM()
        {
            _firebaseService = new ApiServiceFirebase();

            UpdateCommand = new Command<Edificio>(async (edificio) => await UpdateEdificio(edificio));
            ListaDeEdificios = new ObservableCollection<Edificio>();
            AddEdificioCommand = new Command(async () => await NavigateToAddEdificio());
            ImageTappedCommand = new Command<Edificio>(async (edificio) => await NavigateToDetailsPage(edificio));
            DeleteCommand = new Command<Edificio>(async (edificio) => await DeleteEdificio(edificio));
            LoadEdificios();
        }

        private async Task UpdateEdificio(Edificio edificio)
        {
            try
            {
                var editPage = new PageActualizarEdificio(edificio);
                await App.Current.MainPage.Navigation.PushAsync(editPage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);

            }

        }

        private async Task DeleteEdificio(Edificio edificio)
        {
            bool isUserSure = await App.Current.MainPage.DisplayAlert(
                "Confirmar eliminación",
                $"¿Estás seguro de eliminar el edificio '{edificio.Name}'?",
                "Sí", "No");


            if (isUserSure)
            {
                await _firebaseService.DeleteEdificio(edificio.Id);

                ListaDeEdificios.Remove(edificio);
            }

        }

        private async Task NavigateToDetailsPage(Edificio edificio)
        {

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(new PageEdificioSalones(edificio));
            }
        }

        private async Task NavigateToAddEdificio()
        {

        }

        public async Task LoadEdificios()
        {
            try
            {
                var firebaseService = new ApiServiceFirebase();
                var edificiosDesdeFirebase = await firebaseService.GetEdificiosAsync();
                ListaDeEdificios.Clear();
                foreach (var edificio in edificiosDesdeFirebase)
                {
                    ListaDeEdificios.Add(edificio);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar los edificios: {ex.Message}");

            }
        }

    }
}
