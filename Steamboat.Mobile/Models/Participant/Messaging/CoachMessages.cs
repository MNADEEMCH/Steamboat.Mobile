using System;
using System.Collections.Generic;

namespace Steamboat.Mobile.Models.Participant.Messaging
{
	public class CoachMessages
	{
		public List<Message> Messages { get; set; }

		public class From
		{
			public int AccountID { get; set; }
			public string EmailAddress { get; set; }
			public string Role { get; set; }
		}

		public class To
		{
			public int AccountID { get; set; }
			public string EmailAddress { get; set; }
			public string Role { get; set; }
		}

		public class Message
		{
			public int ID { get; set; }
			public string Text { get; set; }
			public DateTime CreatedTimestamp { get; set; }
			public bool IsSender { get; set; }
			public From From { get; set; }
			public To To { get; set; }
			public bool ShowImage { get; set; }
			public bool ShowDate { get; set; }
			public string AvatarUrl { get; set; }

            public Message()
			{
				ShowDate = true;
			}
		}
	}
}
