using System;
using System.Collections.Generic;
using System.Text;

namespace FOSDEM
{
    /// <summary>
    /// A keyed list of objects that provides additional info for presention in a JumpListBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JumpListGroup<T> : List<object>
    {
        /// <summary>
        /// Key that represents the identifier of group of objects.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Display value that represents the group and used as the group header.
        /// </summary>
        public string KeyDisplay { get; set; }

        /// <summary>
        /// Gets the default enumerator for this group of objects.
        /// </summary>
        /// <returns>Enumerator of the group's list of objects</returns>
        public new IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>)base.GetEnumerator();
        }
    }
}