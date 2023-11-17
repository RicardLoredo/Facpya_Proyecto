using Facpya_Proyecto.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Facpya_Proyecto.modelo
{
    public class AddOrUpdateEdificioViewModel : INotifyPropertyChanged
    {
        private ApiServiceFirebase _firebaseService;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBuildingImageVisible => BuildingImagePreview != null || !string.IsNullOrEmpty(Edificio?.ImageUrl);
        public bool IsBuildingImageNotVisible => !IsBuildingImageVisible;

        private Edificio _edificio;
        public Edificio Edificio
        {
            get => _edificio;
            set
            {
                if (_edificio != value)
                {
                    _edificio = value;
                    OnPropertyChanged(nameof(Edificio));
                    OnPropertyChanged(nameof(CurrentBuildingImageSource)); 
                }
            }
        }

        public bool IsSketchImageVisible => SketchImagePreview != null || !string.IsNullOrEmpty(Edificio?.SketchImageUrl);
        public bool IsSketchImageNotVisible => !IsSketchImageVisible;

        private Edificio _sketch;
        public Edificio Sketch

        {
            get => _sketch;
            set
            {
                if (_sketch != value)
                {
                    _sketch = value;
                    OnPropertyChanged(nameof(Sketch));
                    OnPropertyChanged(nameof(CurrentSketchImageSource));
                }
            }
        }

        public bool IsVideoVisible => VideoPreview != null || !string.IsNullOrEmpty(Edificio?.VideoUrl);
        public bool IsVideoNotVisible => !IsVideoVisible;

        private Edificio _video;
        public Edificio Video
        {
            get => _video;
            set
            {
                if (_video != value)
                {
                    _video = value;
                    OnPropertyChanged(nameof(Video));
                    OnPropertyChanged(nameof(CurrentVideoSource));
                }
            }
        }

        public ICommand PickImageCommand { get; }
        public ICommand PickSketchCommand { get; }
        public ICommand PickVideoCommand { get; }
        public ICommand UpdateEdificioCommand { get; }

        public AddOrUpdateEdificioViewModel(Edificio edificio)
        {
            _firebaseService = new ApiServiceFirebase();

            Edificio = edificio ?? new Edificio();

            PickImageCommand = new Command(async () => await PickImageAsync());

            PickSketchCommand = new Command(async () => await PickSketchAsync());

            PickVideoCommand = new Command(async () => await PickVideoAsync());

            UpdateEdificioCommand = new Command(async () => await UpdateEdificio());
        }

        private FileResult _buildingImage;
        private FileResult _sketchImage;
        private FileResult _videoFile;

        public ImageSource CurrentSketchImageSource => SketchImagePreview ?? Edificio?.SketchImageUrl;

        private ImageSource _sketchImagePreview;
        public ImageSource SketchImagePreview
        {
            get => _sketchImagePreview;
            set
            {
                if (_sketchImagePreview != value)
                {
                    _sketchImagePreview = value;
                    OnPropertyChanged(nameof(SketchImagePreview));
                    OnPropertyChanged(nameof(CurrentSketchImageSource));
                }
            }
        }

        private async Task PickSketchAsync()
        {
            try
            {
                var fileResult = _sketchImage = await MediaPicker.PickPhotoAsync();
                if (fileResult != null)
                {
                    var stream = await fileResult.OpenReadAsync();
                    SketchImagePreview = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"No se pudo seleccionar la imagen: {ex.Message}");
            }
        }

        public ImageSource CurrentBuildingImageSource => BuildingImagePreview ?? Edificio?.ImageUrl;

        private ImageSource _buildingImagePreview;
        public ImageSource BuildingImagePreview
        {
            get => _buildingImagePreview;
            set
            {
                if (_buildingImagePreview != value)
                {
                    _buildingImagePreview = value;
                    OnPropertyChanged(nameof(BuildingImagePreview));
                    OnPropertyChanged(nameof(CurrentBuildingImageSource));
                }
            }
        }

        private async Task PickImageAsync()
        {
            try
            {
                var fileResult = _buildingImage = await MediaPicker.PickPhotoAsync();
                if (fileResult != null)
                {
                    var stream = await fileResult.OpenReadAsync();
                    BuildingImagePreview = ImageSource.FromStream(() => stream);
                    OnPropertyChanged(nameof(BuildingImagePreview));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"No se pudo seleccionar la imagen: {ex.Message}");
            }
        }

        public Uri CurrentVideoSource => VideoPreview ?? (!string.IsNullOrEmpty(Edificio?.VideoUrl) ? new Uri(Edificio.VideoUrl) : null);

        private Uri _videoPreview;
        public Uri VideoPreview
        {
            get => _videoPreview;
            set
            {
                if (_videoPreview != value)
                {
                    _videoPreview = value;
                    OnPropertyChanged(nameof(VideoPreview));
                    OnPropertyChanged(nameof(CurrentVideoSource));
                }
            }
        }
        private async Task PickVideoAsync()
        {
            try
            {
                var fileResult = _videoFile = await MediaPicker.PickVideoAsync();
                if (fileResult != null)
                {
                    VideoPreview = new Uri(fileResult.FullPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"No se pudo seleccionar el video: {ex.Message}");
            }
        }

        private async Task UpdateEdificio()
        {
            try
            {
                if (_buildingImage != null)
                {
                    using (var stream = await _buildingImage.OpenReadAsync())
                    {
                        Edificio.ImageUrl = await _firebaseService.UploadFile(stream, "edificios/images", $"building_{Edificio.Id}.jpg");
                    }
                }

                if (_sketchImage != null)
                {
                    using (var stream = await _sketchImage.OpenReadAsync())
                    {
                        Edificio.SketchImageUrl = await _firebaseService.UploadFile(stream, "edificios/sketches", $"sketch_{Edificio.Id}.jpg");
                    }
                }

                if (_videoFile != null)
                {
                    using (var stream = await _videoFile.OpenReadAsync())
                    {
                        Edificio.VideoUrl = await _firebaseService.UploadFile(stream, "edificios/videos", $"video_{Edificio.Id}.mp4");
                    }
                }
                await _firebaseService.UpdateEdificio(Edificio);
                await App.Current.MainPage.DisplayAlert("Éxito", "Edificio actualizado correctamente", "OK");

                if (App.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Algo mal ocurrió: {ex.Message}", "OK");
            }
        }


        private bool CanPickVideo()
        {
            return string.IsNullOrEmpty(Edificio?.VideoUrl);
        }

        private bool CanPickSketch()
        {
            return string.IsNullOrEmpty(Edificio?.SketchImageUrl);
        }

        private bool CanPickImage()
        {
            return string.IsNullOrEmpty(Edificio?.ImageUrl);
        }

        private void RefreshCanExecuteCommands()
        {
            (PickImageCommand as Command)?.ChangeCanExecute();
        }


    }
}
