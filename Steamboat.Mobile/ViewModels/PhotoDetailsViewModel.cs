using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Steamboat.Mobile.ViewModels.Modals;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class PhotoDetailsViewModel : ViewModelBase
    {
        #region Properties

        private PhotojournalingCommentEmoji _userComment;
        private PhotojournalingCommentEmoji _coachComment;
        private Photograph _userPhotograph;
        private bool _showUserComment;
        private bool _showCoachComment;
        private bool _isCoachAcknowledged;
        private bool _replyCoach;

        public PhotojournalingCommentEmoji UserComment { get { return _userComment; } set { SetPropertyValue(ref _userComment, value); } }
        public PhotojournalingCommentEmoji CoachComment { get { return _coachComment; } set { SetPropertyValue(ref _coachComment, value); } }
        public Photograph UserPhotograph { get { return _userPhotograph; } set { SetPropertyValue(ref _userPhotograph, value); } }
        public bool ShowUserComment { get { return _showUserComment; } set { SetPropertyValue(ref _showUserComment, value); } }
        public bool ShowCoachComment { get { return _showCoachComment; } set { SetPropertyValue(ref _showCoachComment, value); } }
        public bool IsCoachAcknowledged { get { return _isCoachAcknowledged; } set { SetPropertyValue(ref _isCoachAcknowledged, value); } }
        public bool ReplyCoach { get { return _replyCoach; } set { SetPropertyValue(ref _replyCoach, value); } }
        public ICommand NavigateToMessagesCommand { get; set; }
        public ICommand OpenImageCommand { get; set; }

        #endregion

        public PhotoDetailsViewModel()
        {
            NavigateToMessagesCommand = new Command(async () => await this.OpenMessages());
            OpenImageCommand = new Command(async () => await this.OpenImage());
        }

        public async override Task InitializeAsync(object parameter)
        {
            if (parameter is Photograph photo)
            {
                UserPhotograph = photo;
                ShowUserComment = !string.IsNullOrEmpty(photo.ParticipantComment) || !string.IsNullOrEmpty(photo.ParticipantOpinionRatingName);
                if (ShowUserComment)
                {
                    UserComment = new PhotojournalingCommentEmoji()
                    {
                        AvatarUrl = App.CurrentUser.AvatarUrl,
                        Emoji = photo.ParticipantOpinionRatingName
                    };
                    UserComment.Text = HandlePhotoText(photo.ParticipantComment, photo.ParticipantOpinionRatingName);
                }

                IsCoachAcknowledged = photo.ReviewedTimestamp != DateTime.MinValue && string.IsNullOrEmpty(photo.ReviewerComment);
                if (!IsCoachAcknowledged)
                {
                    ShowCoachComment = !string.IsNullOrEmpty(photo.ReviewerComment) || !string.IsNullOrEmpty(photo.ReviewerOpinionRatingName);
                    if (ShowCoachComment)
                    {
                        CoachComment = new PhotojournalingCommentEmoji()
                        {
                            CreatedTimestamp = photo.ReviewedTimestamp,
                            Emoji = photo.ReviewerOpinionRatingName
                        };
                        CoachComment.Text = HandlePhotoText(photo.ReviewerComment, photo.ReviewerOpinionRatingName);
                    }
                }
                else
                {
                    CoachComment = new PhotojournalingCommentEmoji()
                    {
                        CreatedTimestamp = photo.ReviewedTimestamp
                    };
                }
                ReplyCoach = IsCoachAcknowledged || ShowCoachComment;
            }

            await base.InitializeAsync(parameter);
        }

        private string HandlePhotoText(string comment, string emoji)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                return emoji != null ? comment.Insert(0, "        ") : comment;
            }
            else
                return string.Empty;
        }

        private async Task OpenMessages()
        {
            await NavigationService.NavigateToAsync<MessagingViewModel>(mainPage: false);
        }

        private async Task OpenImage()
        {
            await ModalService.PushAsync<PhotoDetailsModalViewModel>(UserPhotograph.Url);
        }

        //cambiar svg x png
    }
}
