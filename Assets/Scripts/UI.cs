using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class UI : MonoBehaviour
{
    public Action OnCreateButtonClick;

    [SerializeField]
    private Button _createButton;

    private void OnEnable()
    {
        _createButton.onClick.AddListener(CreateBox);
    }

    private void OnDisable()
    {
        _createButton.onClick.RemoveAllListeners();
    }

    public void CreateBox()
    {
        OnCreateButtonClick?.Invoke();
    }
}