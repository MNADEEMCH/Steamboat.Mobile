using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Steamboat.Mobile.Helpers
{
	public static class ObservableExtension
	{
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
		{
			ObservableCollection<T> collection = new ObservableCollection<T>();

			foreach (T item in source)
			{
				collection.Add(item);
			}

			return collection;
		}

		public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				coll.Add(item);
			}
        }

		public static int LastIndexOf<T>(this ObservableCollection<T> source, T elem)
		{
			var lastIndexOf = -1;
			var indexCounter = 0;
			foreach (T item in source)
			{
				if (item.Equals(elem))
					lastIndexOf = indexCounter;

				indexCounter++;
			}

			return lastIndexOf;
		}
	}
}
