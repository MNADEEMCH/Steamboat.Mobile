using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Steamboat.Mobile.ViewModels.Modals;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class PhotojournalingViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;
        private ObservableCollection<Photograph> _photoCollection;
        private string _photosTaken;
        private bool _showPhotos;

        public ICommand OpenCameraCommand { get; set; }
        public ICommand MoreInfoCommand { get; set; }
        public ObservableCollection<Photograph> PhotoCollection { get { return _photoCollection; } set { SetPropertyValue(ref _photoCollection, value); } }
        public string PhotosTaken { get { return _photosTaken; } set { SetPropertyValue(ref _photosTaken, value); } }
        public bool ShowPhotos { get { return _showPhotos; } set { SetPropertyValue(ref _showPhotos, value); } }

        #endregion

        public PhotojournalingViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            OpenCameraCommand = new Command(async () => await OpenCamera());
            MoreInfoCommand = new Command(async () => await MoreInfo());

            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
        }

        public async override Task InitializeAsync(object parameter)
        {
            var photos = await _participantManager.GetPhotographs();
            PreparePhotos(ref photos);
            PhotoCollection = photos.ToObservableCollection();
            var photosCount = PhotoCollection.Count;
            PhotosTaken = string.Format("{0}/{1}", photosCount, "20");
            ShowPhotos = photosCount > 0;

            IsLoading = false;
        }

        private async Task OpenCamera()
        {
            await TryExecute(async () =>
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Camera))
                        status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    await NavigationService.NavigateToAsync<CameraViewModel>(animate: false);
                }
                else if (status != PermissionStatus.Unknown)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ModalService.PushAsync<CameraAccessModalViewModel>();
                    });
                }
            }, null, null);
        }

        private async Task MoreInfo()
        {
            await ModalService.PushAsync<PhotojournalingMoreInfoModalViewModel>();
        }

        private void PreparePhotos(ref List<Photograph> photos)
        {
            foreach (var photo in photos)
            {
                photo.ReviewPending = photo.ReviewerComment == null && photo.ReviewerOpinionRating == null;
                photo.Acknowledged = photo.ReviewPending && photo.ReviewedTimestamp != DateTime.MinValue;
            }
        }
    }
}
