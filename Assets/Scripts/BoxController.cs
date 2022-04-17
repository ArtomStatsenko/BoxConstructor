using UnityEngine;

public sealed class BoxController
{
    private BoxModel _model;
    private BoxView _selectedBox;
    private BoxView _prefab;
    private Camera _camera;
    private LayerMask _layer;
    private Vector3 _size = Vector3.one;
    private Vector3 _gridSize;
    private Color _ghostColor;
    private Color _defaultColor;
    private float _xBorder;
    private float _zBorder;
    private float _step;
    private float _gap = 0.05f;
    //private bool _isDrag;

    public BoxController(BoxModel model, BoxView prefab, float step, Vector3 gridSize, Camera camera)
    {
        _model = model;
        _prefab = prefab;
        _step = step;
        _gridSize = gridSize;
        _camera = camera;
        //_isDrag = false;
    }

    public void StartPlacingBox()
    {
        if (_selectedBox != null)
        {
            Object.Destroy(_selectedBox.gameObject);
        }

        CreateBox();

        SetTransparent(true);
    }

    private void CreateBox()
    {
        BoxView box = Object.Instantiate(_prefab);
        _selectedBox = box;

        _ghostColor = _selectedBox.GhostColor;
        _defaultColor = _selectedBox.Renderer.material.color;
        _layer = box.Layer;

        _size = _model.Size;
        _selectedBox.Size = _size;
        box.transform.localScale = _size - Vector3.one * _gap;
        box.transform.position = box.transform.position.Change(y: _size.y * 0.5f);
        _xBorder = (_gridSize.x - _size.x) * 0.5f;
        _zBorder = (_gridSize.z - _size.z) * 0.5f;
    }

    public void SetTransparent(bool isSelected)
    {
        if (isSelected)
        {
            _selectedBox.Renderer.material.color = _ghostColor;
        }
        else
        {
            _selectedBox.Renderer.material.color = _defaultColor;
        }
    }

    public void Update()
    {
        if (_selectedBox != null)
        {
            MoveBox();
        }
        else
        {
            SelectBox();
        }
    }

    private void MoveBox()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            float x = Mathf.Round(worldPosition.x / _step) * _step;
            float z = Mathf.Round(worldPosition.z / _step) * _step;
            _selectedBox.transform.position = _selectedBox.transform.position.Change(x: x, z: z);

            bool available = true;
            if (x < -_xBorder || x > _xBorder || z < -_zBorder || z > _zBorder)
            {
                available = false;
            }
            if (_selectedBox.IsCollised)
            {
                available = false;
            }

            if (available && Input.GetMouseButtonDown(0))
            {
                SetTransparent(false);
                _selectedBox = null;
            }
        }
    }

    private void SelectBox()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layer.value))
        {
            GameObject selectedBoxObject = hit.collider.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                _selectedBox = selectedBoxObject.transform.GetComponent<BoxView>();
                _size = _selectedBox.Size;
                _xBorder = (_gridSize.x - _size.x) * 0.5f;
                _zBorder = (_gridSize.z - _size.z) * 0.5f;
                SetTransparent(true);
            }
        }
    }

    //private void Update()
    //{
    //    if (_isDrag)
    //    {
    //        var mousePosition = Input.mousePosition;
    //        var screenMousePosition = _camera.ScreenPointToRay(mousePosition);

    //        if (Physics.Raycast(screenMousePosition, out RaycastHit hit, 1000f, layer.value))
    //        {
    //            _selectedBox.transform.position = _selectedBox.transform.position.Change(x: hit.point.x, z: hit.point.z);
    //        }
    //    }
    //}

    //private void OnMouseDown()
    //{
    //    _isDrag = true;
    //    SetTransparent(true);
    //}

    //private void OnMouseUp()
    //{
    //    _isDrag = false;
    //    SetTransparent(true);
    //}
}
