using UnityEngine;
using System;
using Object = UnityEngine.Object;

public sealed class BoxController : IUpdatable
{
    public Action<bool> OnEditModeEvent;

    private const float GAP = 0.05f;
    private const float SENSITIVITY_SCALE = 10f;

    private BoxModel _model;
    private BoxView _selectedBox;
    private BoxView _prefab;
    private Camera _camera;
    private LayerMask _layer;
    private Vector3 _gridSize;
    private Color _ghostColor;
    private Color _defaultColor;
    private float _xBorder;
    private float _zBorder;
    private float _step = 0.5f;
    private bool _isMoving;

    public BoxController(BoxModel model, BoxView prefab, Vector3 gridSize, Camera camera)
    {
        _model = model;
        _prefab = prefab;
        _gridSize = gridSize;
        _camera = camera;
    }

    public void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (_selectedBox != null && _isMoving)
        {
            MoveBox(mousePosition);
        }
        if (Input.GetButtonDown(InputManager.BUTTON))
        {
            SelectBox(mousePosition);
        }
        if (Input.GetButtonUp(InputManager.BUTTON))
        {
            _isMoving = false;
        }
    }

    private void MoveBox(Vector3 mousePosition)
    {
        Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        float x = Mathf.Round(worldPosition.x / _step) * _step;
        float z = Mathf.Round(worldPosition.z / _step) * _step;
        _selectedBox.transform.position = _selectedBox.transform.position.Change(x: x, z: z);
    }

    private void SelectBox(Vector3 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _layer.value))
        {
            if (hit.collider.transform.TryGetComponent(out BoxView view))
            {
                CompleteEdit();
                _selectedBox = view;
                SetBorders();
                SetSelectedBoxGhost(true);
                _isMoving = true;

                OnEditModeEvent?.Invoke(true);
            }
        }        
    }

    public void CompleteEdit()
    {
        if (_selectedBox == null)
        {
            return;
        }

        float x = _selectedBox.transform.position.x;
        float z = _selectedBox.transform.position.z;

        if (IsValidPosition(x, z))
        {
            SetSelectedBoxGhost(false);
            _selectedBox = null;

            OnEditModeEvent?.Invoke(false);
        }
    }

    private bool IsValidPosition(float x, float z)
    {
        if (x < -_xBorder || x > _xBorder || z < -_zBorder || z > _zBorder)
        {
            return false;
        }

        if (_selectedBox.IsCollised)
        {
            return false;
        }

        return true;
    }

    private void SetBorders()
    {
        if (_selectedBox.IsRotated)
        {
            _xBorder = (_gridSize.x - _selectedBox.Size.z) * 0.5f;
            _zBorder = (_gridSize.z - _selectedBox.Size.x) * 0.5f;
        }
        else
        {
            _xBorder = (_gridSize.x - _selectedBox.Size.x) * 0.5f;
            _zBorder = (_gridSize.z - _selectedBox.Size.z) * 0.5f;
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

    public void CreateBox(Vector3 position)
    {
        DeleteSelectedBox();

        _selectedBox = Object.Instantiate(_prefab, position, Quaternion.identity);
        _ghostColor = _selectedBox.GhostColor;
        _defaultColor = _selectedBox.Renderer.material.color;
        _layer = _selectedBox.Layer;

        _selectedBox.Size = _model.Size;
        _selectedBox.transform.localScale = _selectedBox.Size - Vector3.one * GAP;
        _selectedBox.transform.position = _selectedBox.transform.position.Change(y: _selectedBox.Size.y * 0.5f);
        _selectedBox.IsRotated = false;
        SetBorders();

        SetSelectedBoxGhost(true);
        _isMoving = true;

        OnEditModeEvent?.Invoke(true);
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

    public void SetStepValue(float value)
    {
        _step = value / SENSITIVITY_SCALE;
    }
}
