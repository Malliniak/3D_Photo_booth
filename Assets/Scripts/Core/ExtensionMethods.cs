using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhotoBooth.Core
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

        public static float GetColorByEnum(this Color color, Rgb colorEnum)
        {
            switch (colorEnum)
            {
                case Rgb.RED:
                    return color.r;
                case Rgb.GREEN:
                    return color.g;
                case Rgb.BLUE:
                    return color.b;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorEnum), colorEnum, null);
            }
        }
        
        public static Vector3 Vector3FromValue(float value) => new Vector3(value, value, value);
        public static Vector3 Vector3FromValue(int value) => new Vector3(value, value, value);
    }
}