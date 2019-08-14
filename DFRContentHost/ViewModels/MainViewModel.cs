﻿using System;
using System.IO;
using System.Collections.ObjectModel;
using Windows.Media.Control;
using WindowsInput.Native;
using DFRContentHost.Models;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System.Threading.Tasks;

namespace DFRContentHost.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private GlobalSystemMediaTransportControlsSession _glbSmtcSession;
        private GlobalSystemMediaTransportControlsSessionManager _glbSmtcMgr;

        public ObservableCollection<FunctionRowButtonModel> FnKeys { get; }

        private string _mediaTitle;
        public string MediaTitle
        {
            get => _mediaTitle;
            set => this.RaiseAndSetIfChanged(ref _mediaTitle, value);
        }

        private string _mediaArtist;
        public string MediaArtist
        {
            get => _mediaArtist;
            set => this.RaiseAndSetIfChanged(ref _mediaArtist, value);
        }

        private Bitmap _bitmap;
        public Bitmap MediaThumbnail
        {
            get => _bitmap;
            set => this.RaiseAndSetIfChanged(ref _bitmap, value);
        }

        public MainViewModel()
        {
            FnKeys = new ObservableCollection<FunctionRowButtonModel>
            {
                new FunctionRowButtonModel("F1", VirtualKeyCode.F1),
                new FunctionRowButtonModel("F2", VirtualKeyCode.F2),
                new FunctionRowButtonModel("F3", VirtualKeyCode.F3),
                new FunctionRowButtonModel("F4", VirtualKeyCode.F4),
                new FunctionRowButtonModel("F5", VirtualKeyCode.F5),
                new FunctionRowButtonModel("F6", VirtualKeyCode.F6),
                new FunctionRowButtonModel("F7", VirtualKeyCode.F7),
                new FunctionRowButtonModel("F8", VirtualKeyCode.F8),
                new FunctionRowButtonModel("F9", VirtualKeyCode.F9),
                new FunctionRowButtonModel("F10", VirtualKeyCode.F10),
                new FunctionRowButtonModel("F11", VirtualKeyCode.F11),
                new FunctionRowButtonModel("F12", VirtualKeyCode.F12)
            };

            InitializeSmtcAsync();
        }

        private async void InitializeSmtcAsync()
        {
            _glbSmtcMgr = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            _glbSmtcSession = _glbSmtcMgr.GetCurrentSession();
            _glbSmtcSession.MediaPropertiesChanged += OnMediaPropertiesChanged;
            _glbSmtcMgr.CurrentSessionChanged += OnSmtcSessionChanged;

            await LoadMediaPropertiesAsync();
        }

        private void OnSmtcSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, 
            CurrentSessionChangedEventArgs args)
        {
            _glbSmtcSession.MediaPropertiesChanged -= OnMediaPropertiesChanged;
            _glbSmtcSession = _glbSmtcMgr.GetCurrentSession();
            _glbSmtcSession.MediaPropertiesChanged += OnMediaPropertiesChanged;
        }

        private async void OnMediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            await LoadMediaPropertiesAsync();
        }

        private async Task LoadMediaPropertiesAsync()
        {
            try
            {
                var prop = await _glbSmtcSession.TryGetMediaPropertiesAsync();
                MediaTitle = prop.Title;
                MediaArtist = prop.Artist;

                if (prop.Thumbnail != null)
                {
                    using (var thumbnailRuntimeStream = await prop.Thumbnail.OpenReadAsync())
                    using (var thumbnailStream = thumbnailRuntimeStream.AsStreamForRead())
                    {
                        MediaThumbnail = new Bitmap(thumbnailStream);
                    }
                }
            }
            catch (Exception)
            {
                // Ignore
            }
        }
    }
}
