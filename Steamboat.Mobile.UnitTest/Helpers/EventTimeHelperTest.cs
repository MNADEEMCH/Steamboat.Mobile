using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Models.Participant;

namespace Steamboat.Mobile.UnitTest.Helpers
{
    [TestFixture]
    public class EventTimeHelperTest
    {
        [Test]
        public void EventTimeHelperHelper_GetList_OnlyOneGroup()
        {
            var oneGroupList = new List<EventTime>();
            var startDate = GetStartTime();
            var elementCount = 8;

            for (int i = 0; i < elementCount; i++)
            {
                oneGroupList.Add(Get15MinEvent(startDate));
                startDate = startDate.AddMinutes(15);
            }

            var resultList = EventTimeHelper.GetList(oneGroupList);

            Assert.AreEqual(resultList.Count, 1);
            Assert.AreEqual(resultList[0].Count, elementCount);
        }

        [Test]
        public void EventTimeHelperHelper_GetList_OnlyOneGroupDifferentTimespan()
        {
            var oneGroupList = new List<EventTime>();
            var startDate = GetStartTime();
            var elementCount = 10;

            for (int i = 0; i < elementCount; i++)
            {
                oneGroupList.Add(Get10MinEvent(startDate));
                startDate = startDate.AddMinutes(10);
            }

            var resultList = EventTimeHelper.GetList(oneGroupList);

            Assert.AreEqual(resultList.Count, 1);
            Assert.AreEqual(resultList[0].Count, elementCount);
        }

        [Test]
        public void EventTimeHelperHelper_GetList_TwoGroups()
        {
            var oneGroupList = new List<EventTime>();
            var startDate = GetStartTime();
            var elementCount = 10;

            for (int i = 0; i < elementCount; i++)
            {
                oneGroupList.Add(Get15MinEvent(startDate));
                startDate = i.Equals(5) ? startDate.AddMinutes(20) : startDate.AddMinutes(15);
            }

            var resultList = EventTimeHelper.GetList(oneGroupList);

            Assert.AreEqual(resultList.Count, 2);
            var lastOfFirst = resultList[0].Last().Finish.AddSeconds(1);
            var firstOfLast = resultList[1].First().Start;
            Assert.AreNotEqual(lastOfFirst, firstOfLast);
        }

        [Test]
        public void EventTimeHelperHelper_GetList_AllGroups()
        {
            var oneGroupList = new List<EventTime>();
            var startDate = GetStartTime();
            var elementCount = 10;

            for (int i = 0; i < elementCount; i++)
            {
                oneGroupList.Add(Get15MinEvent(startDate));
                startDate = startDate.AddMinutes(20);
            }

            var resultList = EventTimeHelper.GetList(oneGroupList);

            Assert.AreEqual(resultList.Count, elementCount);
            Assert.AreEqual(resultList[0].Count, resultList.Last().Count());
        }

        [Test]
        public void EventTimeHelperHelper_GetList_TwoItemsGroups()
        {
            var oneGroupList = new List<EventTime>();
            var startDate = GetStartTime();
            var elementCount = 10;

            for (int i = 0; i < elementCount; i++)
            {
                oneGroupList.Add(Get15MinEvent(startDate));
                //Add pair of elements when index is odd
                startDate = i % 2 != 0 ? startDate.AddMinutes(20) : startDate.AddMinutes(15);
            }

            var resultList = EventTimeHelper.GetList(oneGroupList);

            Assert.AreEqual(resultList.Count, elementCount / 2);
            Assert.AreEqual(resultList[0].Count, 2);
            Assert.AreEqual(resultList[0].Count, resultList.Last().Count());
        }

        [Test]
        public void EventTimeHelperHelper_GetList_EmptyList()
        {
            var oneGroupList = new List<EventTime>();

            var resultList = EventTimeHelper.GetList(oneGroupList);

            Assert.AreEqual(resultList.Count, 1);
            Assert.AreEqual(resultList[0].Count(), 0);
        }

        #region TestHelpers

        private EventTime Get15MinEvent(DateTime startTime)
        {
            var newEvent = new EventTime();
            newEvent.ID = 1;
            newEvent.IsActive = true;
            newEvent.Start = startTime;
            newEvent.Finish = startTime.AddMinutes(14).AddSeconds(59);
            return newEvent;
        }

        private EventTime Get10MinEvent(DateTime startTime)
        {
            var newEvent = new EventTime();
            newEvent.ID = 1;
            newEvent.IsActive = true;
            newEvent.Start = startTime;
            newEvent.Finish = startTime.AddMinutes(9).AddSeconds(59);
            return newEvent;
        }

        private DateTime GetStartTime()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
        }

        #endregion
    }
}
