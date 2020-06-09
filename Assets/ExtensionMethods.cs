using System.Collections.Generic;

namespace DefaultNamespace
{
    public static class ExtensionMethods
    {
        /// <summary>
        ///  Extension method that copies all members of array into the list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="array"> Array of the same type </param>
        /// <typeparam name="T"></typeparam>
        public static void AddMembersFromArray<T>(this List<T> list, T[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
        }
    }
}