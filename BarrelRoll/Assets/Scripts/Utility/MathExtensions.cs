//////////////////////////////////////////
//	Create by Leonard Marineau-Quintal  //
//		www.leoquintgames.com			//
//////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtensions {

    public static Vector3 RotatePoint(this Vector3 toRotate, Vector3 rotateAround, float angleInRad)
    {

        float sin = Mathf.Sin(angleInRad);
        float cos = Mathf.Cos(angleInRad);

        // Translate point back to origin
        toRotate.x -= rotateAround.x;
        toRotate.y -= rotateAround.y;

        // Rotate point
        float xnew = toRotate.x * cos - toRotate.y * sin;
        float ynew = toRotate.x * sin + toRotate.y * cos;

        // Translate point back
        Vector3 newPoint = new Vector3(xnew + rotateAround.x, ynew + rotateAround.y);
        return newPoint;
    }

    public static Stack<T> Shuffle<T>(this Stack<T> stack)
    {
        List<T> toShuffle = new List<T>(stack);
        for (int i = toShuffle.Count - 1; i > 0; i--)
        {
            T temp = toShuffle[i];
            int index = UnityEngine.Random.Range(0, toShuffle.Count);
            toShuffle[i] = toShuffle[index];
            toShuffle[index] = temp;
        }

        stack.Clear();

        for (int i = 0; i < toShuffle.Count; ++i)
        {
            stack.Push(toShuffle[i]);
        }
        toShuffle = null;
        return stack;
    }
}
