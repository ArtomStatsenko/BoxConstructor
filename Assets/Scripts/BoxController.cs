using UnityEngine;

public sealed class BoxController
{
    private LayerMask _layer;
    private Camera _camera;

    public BoxController(LayerMask layer)
    {
        _layer = layer;
        _camera = Camera.main;
    }

    //private void Update()
    //{
    //    if (_isDrag)
    //    {
    //        var mousePosition = Input.mousePosition;
    //        var screenMousePosition = _camera.ScreenPointToRay(mousePosition);

    //        if (Physics.Raycast(screenMousePosition, out RaycastHit hit, 1000f, _layer.value))
    //        {
    //            transform.position = transform.position.Change(x: hit.point.x, z: hit.point.z);
    //        }
    //    }
    //}

    //private void OnMouseDown()
    //{
    //    _isDrag = true;
    //}

    //private void OnMouseUp()
    //{
    //    _isDrag = false;
    //}
}
