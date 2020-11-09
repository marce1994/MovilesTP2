using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class ListExtension
{
    public static T PopAt<T>(this List<T> list, int index)
    {
        T r = list[index];
        list.RemoveAt(index);
        return r;
    }

    public static void RemoveListeners(this IEnumerable<Button> buttons)
    {
        foreach (var button in buttons)
            button.onClick.RemoveAllListeners();
    }

    public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }
}
