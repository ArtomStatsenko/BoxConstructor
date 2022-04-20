using System;
using UnityEngine;

public sealed class GeneratorController : IUpdatable
{
    public Action<Vector3> OnBoxClick;

    private BoxModel _model;
    private GeneratorView _view;
    private LayerMask _layer;
    private Camera _camera;

    public GeneratorController(BoxModel model, GeneratorView view, Camera camera)
    {
        _model = model;
        _view = view;
        _camera = camera;
        _layer = _view.Layer;
        _view.transform.localScale = _model.Size;
    }

    public void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (Input.GetButtonDown(InputManager.BUTTON))
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layer.value))
            {
                if (hit.collider.transform.TryGetComponent(out GeneratorView view))
                {
                    OnBoxClick?.Invoke(mousePosition);
                }
            }
        }
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

        _view.transform.localScale = _model.Size;
    }
}