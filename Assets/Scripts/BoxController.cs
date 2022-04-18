using UnityEngine;

public sealed class BoxController
{
    private const string FIRE_1 = "Fire1";
    private const float GAP = 0.05f;

    private BoxModel _model;
    private BoxView _selectedBox;
    private BoxView _prefab;
    private Camera _camera;
    private LayerMask _layer;
    private Vector3 _gridSize;
    private Vector3 _size;
    private Color _ghostColor;
    private Color _defaultColor;
    private float _xBorder;
    private float _zBorder;
    private float _step;
    private bool _isValidPosition;

    public BoxController(BoxModel model, BoxView prefab, float step, Vector3 gridSize, Camera camera)
    {
        _model = model;
        _prefab = prefab;
        _step = step;
        _gridSize = gridSize;
        _camera = camera;
    }

    public void StartPlacingBox()
    {
        DeleteSelectedBox();
        CreateNewBox();
        SetSelectedBoxGhost(true);
    }

    private void CreateNewBox()
    {
        _selectedBox = Object.Instantiate(_prefab);
        _ghostColor = _selectedBox.GhostColor;
        _defaultColor = _selectedBox.Renderer.material.color;
        _layer = _selectedBox.Layer;
        _size = _model.Size;
        _selectedBox.Size = _size;
        _selectedBox.transform.localScale = _size - Vector3.one * GAP;
        _selectedBox.transform.position = _selectedBox.transform.position.Change(y: _size.y * 0.5f);
        _selectedBox.IsRotated = false;
        SetBorders();
    }

    private void SetBorders()
    {
        if (_selectedBox.IsRotated)
        {
            _xBorder = (_gridSize.x - _size.z) * 0.5f;
            _zBorder = (_gridSize.z - _size.x) * 0.5f;
        }
        else
        {
            _xBorder = (_gridSize.x - _size.x) * 0.5f;
            _zBorder = (_gridSize.z - _size.z) * 0.5f;
        }
    }

    public void SetSelectedBoxGhost(bool isSelected)
    {
        if (_selectedBox == null)
        {
            return;
        }

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
        if (Input.GetButtonDown(FIRE_1))
        {
            _selectedBox = SelectBox();

            if (_selectedBox == null)
            {
                CompleteEdit();
            }
        }

        if (_selectedBox != null)
        {
            if (Input.GetButton(FIRE_1))
            {
                MoveBox();
            }
            //if (_isValidPosition && Input.GetButtonUp(FIRE_1))
            //{
            //    SetGhost(false);
            //    _selectedBox = null;
            //}
        }
    }

    private void CompleteEdit()
    {
        if (_isValidPosition)
        {
            SetSelectedBoxGhost(false);
            _selectedBox = null;
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

            _isValidPosition = true;
            if (x < -_xBorder || x > _xBorder || z < -_zBorder || z > _zBorder)
            {
                _isValidPosition = false;
            }
            if (_selectedBox.IsCollised)
            {
                _isValidPosition = false;
            }
        }
    }

    private BoxView SelectBox()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layer.value))
        {
            GameObject selectedBoxObject = hit.collider.gameObject;
            _size = _selectedBox.Size;
            SetBorders();
            SetSelectedBoxGhost(true);
            return selectedBoxObject.transform.GetComponent<BoxView>();
        }

        return null;
    }

    public void RotateSelectedBox()
    {
        if (_selectedBox != null)
        {
            _selectedBox.transform.Rotate(0f, 90f, 0f);
            _selectedBox.IsRotated = !_selectedBox.IsRotated;
            SetBorders();
        }
    }

    public void DeleteSelectedBox()
    {
        if (_selectedBox != null)
        {
            Object.Destroy(_selectedBox.gameObject);
            _selectedBox = null;
        }
    }
}
