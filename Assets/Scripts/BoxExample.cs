using UnityEngine;
using System;

public sealed class BoxExample : MonoBehaviour
{
    public Action OnBoxClick;

    [SerializeField]
    private BoxModel _model;

    private void Start()
    {
        transform.localScale = _model.Size;
    }

    public void SetSize(Size size, float value)
    {
        switch (size)
        {
            case Size.Lenght:
                _model.Length = value;
                break;            
            case Size.Height:
                _model.Height = value;
                break;
            case Size.Width:
                _model.Width = value;
                break;
        }

        transform.localScale = _model.Size;
    }
}