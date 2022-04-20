using System.Collections.Generic;
using UnityEngine;

public sealed class Root : MonoBehaviour
{
    [SerializeField]
    private UI _ui;
    [SerializeField]
    private BoxModel _boxModel;
    [SerializeField]
    private BoxView _boxPrefab;
    [SerializeField]
    private PalleteModel _gridModel;
    [SerializeField]
    private GeneratorView _generatorView;
    [SerializeField]
    private PalleteView _palleteView;

    private BoxController _boxController;
    private GeneratorController _boxGenerator;
    private Camera _camera;
    private Vector3 _gridSize = Vector3.one;
    private List<IUpdatable> _controllers = new List<IUpdatable>();

    private void Awake()
    {
        _camera = Camera.main;
        _gridSize = _palleteView.transform.localScale.Change(x: _gridModel.Size.x, z: _gridModel.Size.y);
        _palleteView.transform.localScale = _gridSize;

        _ui.SetEditMode(false);
        _ui.SetInputSizeValue(_boxModel.Size);
    }

    private void Update()
    {
        foreach (IUpdatable controller in _controllers)
        {
            controller.Update();
        }
    }

    private void OnEnable()
    {
        Init();
        Subscribe();
    }
    private void OnDisable()
    {
        Unsubscribe();
        _controllers.Clear();
    }

    private void Init()
    {
        _boxController = new BoxController(_boxModel, _boxPrefab, _gridSize, _camera);
        _boxGenerator = new GeneratorController(_boxModel, _generatorView, _camera);
        _controllers.Add(_boxController);
        _controllers.Add(_boxGenerator);
    }

    private void Subscribe()
    {
        _ui.OnRotateButtonClick += _boxController.RotateSelectedBox;
        _ui.OnDeleteButtonClick += _boxController.DeleteSelectedBox;
        _ui.OnAcceptButtonClick += _boxController.CompleteEdit;
        _ui.OnSliderValueChanged += _boxController.SetStepValue;
        _ui.OnInputFieldValueChanged += _boxGenerator.SetSize;
        _boxController.OnEditModeEvent += _ui.SetEditMode;
        _boxGenerator.OnBoxClick += _boxController.CreateBox;
    }

    private void Unsubscribe()
    {
        _ui.OnRotateButtonClick -= _boxController.RotateSelectedBox;
        _ui.OnDeleteButtonClick -= _boxController.DeleteSelectedBox;
        _ui.OnAcceptButtonClick -= _boxController.CompleteEdit;
        _ui.OnSliderValueChanged -= _boxController.SetStepValue;
        _ui.OnInputFieldValueChanged -= _boxGenerator.SetSize;
        _boxController.OnEditModeEvent -= _ui.SetEditMode;
        _boxGenerator.OnBoxClick -= _boxController.CreateBox;
    }
}