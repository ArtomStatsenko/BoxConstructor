using UnityEngine;

public sealed class BoxView : MonoBehaviour
{
    [SerializeField]
    private LayerMask layer;

    private bool _isDrag;
    private Camera _camera;

    private void Start()
    {
        _isDrag = false;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isDrag)
        {
            var mousePosition = Input.mousePosition;
            var screenMousePosition = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(screenMousePosition, out RaycastHit hit, 1000f, layer.value))
            {
                transform.position = transform.position.Change(x: hit.point.x, z: hit.point.z);
            }
        }
    }
    
    private void OnMouseDown()
    {
        _isDrag = true;
    }

    private void OnMouseUp()
    {
        _isDrag = false;
    }
}
