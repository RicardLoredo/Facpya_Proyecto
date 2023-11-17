using Facpya_Proyecto.Service;
using Facpya_Proyecto.view;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Facpya_Proyecto.modelo
{
    public class AddEdificioViewModel : BindableObject
    {

        private ApiServiceFirebase _firebaseService;

        private FileResult _buildingImage;
        private FileResult _sketchImage;
        private FileResult _videoFile;

        private ImageSource _buildingImageSource;
        public ImageSource BuildingImageSource
        {
            get => _buildingImageSource;
            set
            {
                _buildingImageSource = value;
                OnPropertyChanged(nameof(BuildingImageSource));
                OnPropertyChanged(nameof(IsBuildingImageVisible));
            }
        }
        public bool IsBuildingImageVisible => _buildingImageSource != null;

        private ImageSource _sketchImageSource;
        public ImageSource SketchImageSource
        {
            get => _sketchImageSource;
            set
            {
                _sketchImageSource = value;
                OnPropertyChanged(nameof(SketchImageSource));
                OnPropertyChanged(nameof(IsSketchImageVisible));
            }
        }
        public bool IsSketchImageVisible => _sketchImageSource != null;

        private Uri _videoSource;
        public Uri VideoSource
        {
            get => _videoSource;
            set
            {
                if (_videoSource != value)
                {
                    _videoSource = value;
                    OnPropertyChanged(nameof(VideoSource));
                    OnPropertyChanged(nameof(IsVideoVisible));
                    OnPropertyChanged(nameof(IsVideoNotVisible));
                }
            }
        }

        public bool IsVideoVisible => _videoSource != null;
        public bool IsVideoNotVisible => _videoSource == null;

        public Edificio Edificio { get; set; } = new Edificio();

        public ICommand PickImageCommand { get; }
        public ICommand PickSketchCommand { get; }
        public ICommand PickVideoCommand { get; }
        public ICommand SaveEdificioCommand { get; }
        
        public AddEdificioViewModel()
        {
            _firebaseService = new ApiServiceFirebase();

            PickImageCommand = new Command(async () => await PickImageAsync());
            PickSketchCommand = new Command(async () => await PickSketchAsync());
            PickVideoCommand = new Command(async () => await PickVideoAsync());

            SaveEdificioCommand = new Command(async () => await SaveEdificio(Edificio));
        }

        private async Task PickImageAsync()
        {
            try
            {
                var fileResult = _buildingImage = await MediaPicker.PickPhotoAsync();
                if (fileResult != null)
                {
                    var stream = await fileResult.OpenReadAsync();
                    BuildingImageSource = ImageSource.FromStream(() => stream);
                    OnPropertyChanged(nameof(BuildingImageSource));
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"No se pudo seleccionar la imagen: {ex.Message}");
            }
        }

        private async Task PickSketchAsync()
        {
            try
            {
                var fileResult = _sketchImage = await MediaPicker.PickPhotoAsync();
                if (fileResult != null)
                {
                    // Usar OpenReadAsync para obtener un stream de la imagen seleccionada
                    var stream = await fileResult.OpenReadAsync();
                    SketchImageSource = ImageSource.FromStream(() => stream);
                    OnPropertyChanged(nameof(SketchImageSource));
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, por ejemplo, si el usuario cancela el selector de medios
                Debug.WriteLine($"No se pudo seleccionar la imagen: {ex.Message}");
            }
        }

        private async Task PickVideoAsync()
        {
            try
            {
                var fileResult = _videoFile = await MediaPicker.PickVideoAsync();
                if (fileResult != null)
                {
                    var filePath = fileResult.FullPath;
                    VideoSource = new Uri(filePath);
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                Debug.WriteLine($"No se pudo seleccionar el video: {ex.Message}");
            }
        }

        private async Task SaveEdificio(Edificio edificio)
        {

            if (string.IsNullOrEmpty(edificio.Name) || _buildingImage == null || _sketchImage == null || _videoFile == null)
            {
                // No todos los campos están llenos, muestra un mensaje de error
                await App.Current.MainPage.DisplayAlert("Error", "Debes llenar todos los campos y seleccionar todas las imágenes y el video antes de continuar.", "OK");
                return;
            }

            try
            {
                if (_buildingImage != null)
                {
                    using (var stream = await _buildingImage.OpenReadAsync())
                    {
                        // Asegúrate de especificar la carpeta y el nombre del archivo aquí
                        edificio.ImageUrl = await _firebaseService.UploadFile(stream, "edificios/images", $"building_{Guid.NewGuid()}.jpg");
                    }
                }

                if (_sketchImage != null)
                {
                    using (var stream = await _sketchImage.OpenReadAsync())
                    {
                        // Asegúrate de especificar la carpeta y el nombre del archivo aquí
                        edificio.SketchImageUrl = await _firebaseService.UploadFile(stream, "edificios/sketches", $"sketch_{Guid.NewGuid()}.jpg");
                    }
                }

                if (_videoFile != null)
                {
                    using (var stream = await _videoFile.OpenReadAsync())
                    {
                        // Asegúrate de especificar la carpeta y el nombre del archivo aquí
                        edificio.VideoUrl = await _firebaseService.UploadFile(stream, "edificios/videos", $"video_{Guid.NewGuid()}.mp4");
                    }
                }

                await _firebaseService.AddEdificio(edificio);

                // Confirmación de éxito
                await App.Current.MainPage.DisplayAlert("Éxito", "Edificio agregado correctamente", "OK");

                await App.Current.MainPage.Navigation.PopAsync();

                ResetViewModelState();
            }
            catch (Exception ex)
            {
    
                await App.Current.MainPage.DisplayAlert("Error", $"Algo mal ocurrió: {ex.Message}", "OK");
            }
        }

        private void ResetViewModelState()
        {
            Edificio = new Edificio();
            _buildingImage = null;
            _sketchImage = null;
            _videoFile = null;
            BuildingImageSource = null;
            SketchImageSource = null;
            VideoSource = null;
            OnPropertyChanged(nameof(Edificio));
            OnPropertyChanged(nameof(BuildingImageSource));
            OnPropertyChanged(nameof(SketchImageSource));
            OnPropertyChanged(nameof(VideoSource));
        }
    }
}
