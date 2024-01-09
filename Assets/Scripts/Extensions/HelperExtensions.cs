using System;
using DG.Tweening;
using UnityEngine;

public static class HelperExtensions
{
    /// <summary>
    /// Returns random value between <b>a</b> and <b>b</b>.
    /// Order doesn't matter.
    /// </summary>
    public static int Random(this int a, int b = 0)
    {
        if (a == b) return a;

        return a > b ? UnityEngine.Random.Range(b, a) : UnityEngine.Random.Range(a, b);
    }

    /// <summary>
    /// Returns random value between <b>a</b> and <b>b</b>.
    /// Order doesn't matter.
    /// </summary>
    public static float Random(this float a, float b = 0f) => a > b ? UnityEngine.Random.Range(b, a) : UnityEngine.Random.Range(a, b);

    /// <summary>
    /// Returns new Vector3 with random x, y, z values, between <b>a</b> and <b>b</b>.
    /// Order doesn't matter.
    /// </summary>
    public static Vector3 Random(this Vector3 a, Vector3 b) => new Vector3(a.x.Random(b.x), a.y.Random(b.y), a.z.Random(b.z));

    /// <summary>
    /// Returns new Vector3 with x, y, z values equal to value.    
    /// </summary>
    public static Vector3 ToVector3(this float value) => new Vector3(value, value, value);

    static int id = 0;

    /// <summary>
    /// Delayed invoke of an action. Can be stopped with Dotween.Kill(id).
    /// </summary>
    public static int Timer(this Action action, float delay)
    {
        id++;
        DOTween.Sequence()
            .AppendCallback(() => action?.Invoke())
            .SetDelay(delay)
            .SetId(id);
        return id;
    }

    /// <summary>
    /// Returns the direction (left, right, up, down) of Vector2
    /// </summary>
    public static SwipeHandler.Direction GetDirection(this Vector2 delta)
    {
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle > -45f && angle <= 45f)
        {
            return SwipeHandler.Direction.right;
        }
        else if (angle > 45f && angle <= 135f)
        {
            return SwipeHandler.Direction.up;
        }
        else if (angle > -135f && angle <= -45f)
        {
            return SwipeHandler.Direction.down;
        }
        else
        {
            return SwipeHandler.Direction.left;
        }
    }
}
