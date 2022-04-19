using UnityEngine;
using System;

public sealed class BoxExample : MonoBehaviour
{
    public Action OnBoxClick;

    public void SetSize(Size size, float value)
    {
        switch (size)
        {
            case Size.Lenght:
                transform.localScale = transform.localScale.Change(x: value);
                break;
            case Size.Width:
                transform.localScale = transform.localScale.Change(z: value);
                break;
            case Size.Height:
                transform.localScale = transform.localScale.Change(y: value);
                break;
        }
    }
}