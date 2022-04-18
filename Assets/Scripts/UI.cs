using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class UI : MonoBehaviour
{
    public Action OnCreateButtonClick;
    public Action OnRotateButtonClick; 
    public Action OnDeleteButtonClick;
    public Action OnAcceptButtonClick;

    [SerializeField]
    private Button _createButton;
    [SerializeField]
    private Button _rotateButton;
    [SerializeField]
    private Button _deleteButton;
    [SerializeField]
    private Button _acceptButton;

    private void OnEnable()
    {
        _createButton.onClick.AddListener(CreateBox);
        _rotateButton.onClick.AddListener(RotateBox);
        _deleteButton.onClick.AddListener(DeleteBox);
        _acceptButton.onClick.AddListener(Accept);
    }

    private void OnDisable()
    {
        _createButton.onClick.RemoveAllListeners();
        _rotateButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.RemoveAllListeners();
        _acceptButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        SetEditMode(false);
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
    
    private void Accept()
    {
        OnAcceptButtonClick?.Invoke();
    }

    public void SetEditMode(bool enable)
    {
        _createButton.gameObject.SetActive(!enable);
        _rotateButton.gameObject.SetActive(enable);
        _deleteButton.gameObject.SetActive(enable);
        _acceptButton.gameObject.SetActive(enable);
    }
}