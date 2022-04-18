using UnityEngine;
using System;
using Object = UnityEngine.Object;

public sealed class BoxController
{
    public Action<bool> OnEditModeEvent;

    private const string BUTTON = "Fire1";
    private const float GAP = 0.05f;
    private const float UI_PROPORTION = 0.125f;

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

        OnEditModeEvent?.Invoke(true);
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
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (mousePosition.y < Screen.height * UI_PROPORTION)
        {
            return;
        }

        if (Input.GetButtonDown(BUTTON))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layer.value))
            {
                CompleteEdit();

                if (hit.collider.transform.TryGetComponent(out BoxView view))
                {
                    SelectBox(view);
                }
            }
        }

        if (_selectedBox != null)
        {
            if (Input.GetButton(BUTTON))
            {
                MoveBox(mousePosition);
            }
        }
    }

    private void MoveBox(Vector3 mousePosition)
    {
        Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        float x = Mathf.Round(worldPosition.x / _step) * _step;
        float z = Mathf.Round(worldPosition.z / _step) * _step;
        _selectedBox.transform.position = _selectedBox.transform.position.Change(x: x, z: z);

        CheckValidPosition(x, z);
    }

    private void CheckValidPosition(float x, float z)
    {
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

    private void SelectBox(BoxView view)
    {
        _selectedBox = view;
        _size = _selectedBox.Size;
        SetBorders();
        SetSelectedBoxGhost(true);

        OnEditModeEvent?.Invoke(true);
    }

    public void CompleteEdit()
    {
        if (_selectedBox == null)
        {
            return;
        }

        float x = _selectedBox.transform.position.x;
        float z = _selectedBox.transform.position.z;
        CheckValidPosition(x, z);

        if (_isValidPosition)
        {
            SetSelectedBoxGhost(false);
            _selectedBox = null;

            OnEditModeEvent?.Invoke(false);
        }
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

            OnEditModeEvent?.Invoke(false);
        }
    }
}
