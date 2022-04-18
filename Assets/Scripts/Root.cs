using UnityEngine;

public sealed class Root : MonoBehaviour
{
    [SerializeField]
    private UI _ui;
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float _step;
    [SerializeField]
    private BoxModel _boxModel;
    [SerializeField]
    private BoxView _boxPrefab;
    [SerializeField]
    private GridModel _gridModel;

    private BoxController _boxController;

    private Camera _camera;
    private Vector3 _gridSize = Vector3.one;

    private void Awake()
    {
        _camera = Camera.main;
        _gridSize = transform.localScale.Change(x: _gridModel.Size.x, z: _gridModel.Size.y);
        transform.localScale = _gridSize;

        _boxController = new BoxController(_boxModel, _boxPrefab, _step, _gridSize, _camera);
    }

    private void Update()
    {
        _boxController.Update();
    }

    private void OnEnable()
    {
        _ui.OnCreateButtonClick += _boxController.StartPlacingBox;
        _ui.OnRotateButtonClick += _boxController.RotateSelectedBox;
        _ui.OnDeleteButtonClick += _boxController.DeleteSelectedBox;
    }

    private void OnDisable()
    {
        _ui.OnCreateButtonClick -= _boxController.StartPlacingBox;
        _ui.OnRotateButtonClick -= _boxController.RotateSelectedBox;
        _ui.OnDeleteButtonClick -= _boxController.DeleteSelectedBox;
    }
}