using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class UI : MonoBehaviour
{
    public Action OnCreateButtonClick;
    public Action OnRotateButtonClick; 
    public Action OnDeleteButtonClick;

    [SerializeField]
    private Button _createButton;
    [SerializeField]
    private Button _rotateButton;
    [SerializeField]
    private Button _deleteButton;

    private void OnEnable()
    {
        _createButton.onClick.AddListener(CreateBox);
        _rotateButton.onClick.AddListener(RotateBox);
        _deleteButton.onClick.AddListener(DeleteBox);
    }

    private void OnDisable()
    {
        _createButton.onClick.RemoveAllListeners();
        _rotateButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.RemoveAllListeners();
    }

    private void CreateBox()
    {
        OnCreateButtonClick?.Invoke();
    }

    private void RotateBox()
    {
        OnRotateButtonClick?.Invoke();
    }

    private void DeleteBox()
    {
        OnDeleteButtonClick?.Invoke();
    }
}