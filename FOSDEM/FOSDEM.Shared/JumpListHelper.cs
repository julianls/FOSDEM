using System;
using System.Collections.Generic;
using System.Linq;

namespace FOSDEM
{
    /// <summary>
    /// Provides a utility to help group and sort data into JumpList compatible data.
    /// </summary>
    public static class JumpListHelper
    {
        /// <summary>
        /// Groups and sorts into a list of group lists based on a selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <typeparam name="TSort">Type of value returned by sortSelector.</typeparam>
        /// <typeparam name="TGroup">Type of value returned by groupSelector.</typeparam>
        /// <param name="source">List to be grouped and sorted</param>
        /// <param name="sortSelector">A selector that provides the value that items will be sorted by.</param>
        /// <param name="groupSelector">A selector that provides the value that items will be grouped by.</param>
        /// <param name="groupDisplaySelector">A selector that will provide the value represent a group for display.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static List<JumpListGroup<TSource>> ToGroups<TSource, TSort, TGroup>(
            this IEnumerable<TSource> source, Func<TSource, TSort> sortSelector,
            Func<TSource, TGroup> groupSelector, Func<TGroup, String> groupDisplaySelector = null)
        {
            var groups = new List<JumpListGroup<TSource>>();

            // Group and sort items based on values returned from the selectors
            var query = from item in source
                        orderby groupSelector(item), sortSelector(item)
                        group item by groupSelector(item) into g
                        select new { GroupName = g.Key, Items = g };

            // For each group generated from the query, create a JumpListGroup
            // and fill it with its items
            foreach (var g in query)
            {
                JumpListGroup<TSource> group = new JumpListGroup<TSource>();
                group.Key = g.GroupName;
                group.KeyDisplay = groupDisplaySelector == null ? g.GroupName.ToString() : groupDisplaySelector(g.GroupName);
                foreach (var item in g.Items)
                    group.Add(item);

                groups.Add(group);
            }

            return groups;
        }
    }
}