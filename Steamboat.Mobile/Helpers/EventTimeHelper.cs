using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Steamboat.Mobile.Models.Participant;

namespace Steamboat.Mobile.Helpers
{
    public static class EventTimeHelper
    {
        /// <summary>
        /// Gets list of groups of consecutive events.
        /// </summary>
        /// <returns>Matrix of events</returns>
        /// <param name="eventList">Event time list</param>
        public static ObservableCollection<ObservableCollection<EventTime>> GetList(List<EventTime> eventList)
        {
            var retList = new ObservableCollection<ObservableCollection<EventTime>>();

            if(eventList.Count > 1)
            {
                var count = -1;
                var auxList = new ObservableCollection<EventTime>();
                foreach (var item in eventList)
                {
                    if (count < 0)
                    {
                        auxList.Add(item);
                        count++;
                    }
                    else
                    {
                        var prevItem = eventList[count];
                        if (AreConsecutiveEvents(item, prevItem))
                        {
                            auxList.Add(item);
                            count++;
                        }
                        else
                        {
                            retList.Add(auxList);
                            count++;
                            auxList = new ObservableCollection<EventTime>();
                            auxList.Add(item);
                        }
                        if (count.Equals(eventList.Count - 1))
                        {
                            retList.Add(auxList);
                        }
                    }
                }
            }
            else
            {
                retList.Add(eventList.ToObservableCollection());
            }

            return retList;
        }

        private static bool AreConsecutiveEvents(EventTime item, EventTime prevItem)
        {
            return prevItem.Finish.AddSeconds(1).Equals(item.Start);
        }
    }
}
