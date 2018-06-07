using System;
using Steamboat.Mobile.Views.Messaging;
using Xamarin.Forms;
using static Steamboat.Mobile.Models.Participant.Messaging.CoachMessages;

namespace Steamboat.Mobile.CustomControls.Messaging
{
    public class MessagesTemplateSelector: DataTemplateSelector
    {
        private readonly DataTemplate _userMessageTemplate;
        private readonly DataTemplate _coachMessageTemplate;

        public MessagesTemplateSelector()
        {
            _userMessageTemplate = new DataTemplate(typeof(UserMessageView));
            _coachMessageTemplate = new DataTemplate(typeof(CoachMessageView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as Message;
            if (message == null)
                return null;

            if (message.IsSender)
            {
                return _userMessageTemplate;
            }
            else
            {
                return _coachMessageTemplate;
            }
        }
    }
}
