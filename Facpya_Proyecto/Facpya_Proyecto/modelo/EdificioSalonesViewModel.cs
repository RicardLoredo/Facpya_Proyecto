using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Facpya_Proyecto.modelo
{
    public class EdificioSalonesViewModel : BindableObject
    {
        private Edificio _edificio;

        public Edificio Edificio
        {
            get => _edificio;
            set
            {
                _edificio = value;
                OnPropertyChanged();
            }
        }

        private bool _isPopupVisible;
        public bool IsPopupVisible
        {
            get => _isPopupVisible;
            set
            {
                _isPopupVisible = value;
                OnPropertyChanged();
            }
        
        }

        private bool _isVideoPopupVisible;

        public string SelectedVideoUrl { get; private set; }

        public bool IsVideoPopupVisible
        {
            get => _isVideoPopupVisible;
            set { _isVideoPopupVisible = value; 
                OnPropertyChanged(nameof(IsVideoPopupVisible)); }
        }

        private bool _isSketchPopupVisible;

        public bool IsSketchPopupVisible
        {
            get => _isSketchPopupVisible;
            set
            {
                _isSketchPopupVisible = value;
                OnPropertyChanged(nameof(IsSketchPopupVisible));
            }
        }

        public ICommand OpenSketchPopupCommand { get; }
        public ICommand CloseSketchPopupCommand { get; }


        public ICommand OpenPopupCommand { get; }
        public ICommand ClosePopupCommand { get; }

        public ICommand OpenVideoPopupCommand { get; }
        public ICommand CloseVideoPopupCommand { get; }


        public string SelectedImageUrl { get; set; }
        public string SelectedSketchImageUrl { get; set; }

        public EdificioSalonesViewModel(Edificio edificio)
        {
            Edificio = edificio;
            OpenPopupCommand = new Command<string>(url =>
            {
                SelectedImageUrl = url;
                OnPropertyChanged(nameof(SelectedImageUrl));
                IsPopupVisible = true;
                IsVideoPopupVisible = false;
                IsSketchPopupVisible = false;
            });
            ClosePopupCommand = new Command(() => IsPopupVisible = false);

            OpenVideoPopupCommand = new Command<string>(url =>
            {
                SelectedVideoUrl = url;
                OnPropertyChanged(nameof(SelectedVideoUrl));
                IsVideoPopupVisible = true;
                IsPopupVisible = false;
                IsSketchPopupVisible = false;
            });
            CloseVideoPopupCommand = new Command(() => IsVideoPopupVisible = false);

            OpenSketchPopupCommand = new Command<string>(url =>
            {
                SelectedSketchImageUrl = url;
                OnPropertyChanged(nameof(SelectedSketchImageUrl));
                IsSketchPopupVisible = true;
                IsPopupVisible = false;
                IsVideoPopupVisible = false;
            });
            CloseSketchPopupCommand = new Command(() => IsSketchPopupVisible = false);


        }
    }
}
