using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T RandomObject<T>(this IList<T> list) => list[list.Count.Random()];

    public static T LastObject<T>(this IList<T> list) => list[list.Count - 1];

    public static bool IsEmpty<T>(this IList<T> list) => list.Count == 0;
}
