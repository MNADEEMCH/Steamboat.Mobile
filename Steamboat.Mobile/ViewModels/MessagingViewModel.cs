﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Participant.Messaging;
using Steamboat.Mobile.ViewModels.Interfaces;
using Xamarin.Forms;
using static Steamboat.Mobile.Models.Participant.Messaging.CoachMessages;

namespace Steamboat.Mobile.ViewModels
{
	public class MessagingViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
	{
		#region Properties

		private IParticipantManager _participantManager;
		private ObservableCollection<Message> _allMessages;
		private string _imgSource;
		private DateTime _requestTime;
		private bool _firstRequest;
		private string _messageText;

		public ObservableCollection<Message> AllMessages { get { return _allMessages; } set { SetPropertyValue(ref _allMessages, value); } }
		public string MessageText { get { return _messageText; } set { SetPropertyValue(ref _messageText, value); } }
		public ICommand ScrollToBottomCommand { get; set; }
		public ICommand SendMessageCommand { get; set; }

		#endregion

		public MessagingViewModel(IParticipantManager participantManager = null)
		{
			IsLoading = true;
			_participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
			SendMessageCommand = new Command(async () => await SendMessage());
			MessageText = string.Empty;

			_imgSource = App.CurrentUser.AvatarUrl;
			_firstRequest = true;
		}

		public override async Task InitializeAsync(object parameter)
		{
			await TryExecute(async () =>
			{
				if (_firstRequest)
				{
					var allMessages = await _participantManager.GetAllMessages();
					_requestTime = GetCreatedTimestamp(allMessages.Messages.Last());
					AllMessages = ParseMessages(allMessages.Messages);
					Device.BeginInvokeOnMainThread(() => ScrollToBottomCommand.Execute(false));
					_firstRequest = false;
				}
				else
				{
					Device.BeginInvokeOnMainThread(async () =>
					{
						try
						{
							var newMessages = await _participantManager.GetNewMessages(_requestTime.ToString("yyyyMMddTHHmmssfff"));
							if(newMessages.Messages.Count > 0)
							{
								_requestTime = GetCreatedTimestamp(newMessages.Messages.Last());
								var observableMessages = ParseMessages(newMessages.Messages);
								AllMessages.AddRange(observableMessages);
								ScrollToBottomCommand.Execute(true);
							}
						}
						catch (Exception e)
						{
							await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
						}
					});
				}

			}, null, () => IsLoading = false);
		}

		private static DateTime GetCreatedTimestamp(Message message)
		{
			return message.CreatedTimestamp.AddMilliseconds(1);
		}

		public async Task OnViewAppearingAsync(VisualElement view)
		{
			if (!_firstRequest)
			{
				Device.BeginInvokeOnMainThread(() => ScrollToBottomCommand.Execute(false));
				await Task.Delay(500);
			}
		}

		public Task OnViewDisappearingAsync(VisualElement view)
		{
			return Task.FromResult(true);
		}

		private async Task SendMessage()
		{
			await TryExecute(async () =>
			{
				if (!string.IsNullOrEmpty(MessageText))
				{
					var newMessage = await _participantManager.SendMessage(MessageText);
					var auxList = new List<Message>() { newMessage };
					var observableMessages = ParseMessages(auxList);
					Device.BeginInvokeOnMainThread(async () =>
					{
						AllMessages.AddRange(observableMessages);
                        ScrollToBottomCommand.Execute(true);
						MessageText = string.Empty;
					});
					_requestTime = GetCreatedTimestamp(newMessage);
				}
			});
		}

		private ObservableCollection<Message> ParseMessages(List<Message> messages)
		{
			var retList = new ObservableCollection<Message>();
			foreach (var message in messages)
			{
				if (retList.Count > 0)
				{
					var lastItem = retList.Last();
					if (message.IsSender)
					{
						message.ShowImage = true;
						message.AvatarUrl = _imgSource;
						if (lastItem.IsSender)
						{
							lastItem.ShowImage = false;
						}
					}
					else
						message.ShowImage = lastItem.IsSender;

					lastItem.ShowDate = lastItem.IsSender != message.IsSender;
				}
				else
				{
					message.ShowImage = messages.Count == 1 ? true : !message.IsSender;
					message.ShowDate = true;
					message.AvatarUrl = _imgSource;
				}

				retList.Add(message);
			}

			return retList;
		}
	}
}
