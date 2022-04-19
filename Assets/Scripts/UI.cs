﻿using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class UI : MonoBehaviour
{
    public Action OnCreateButtonClick;
    public Action OnRotateButtonClick; 
    public Action OnDeleteButtonClick;
    public Action OnAcceptButtonClick;
    public Action<float> OnSliderValueChanged;
    public Action<Size, float> OnInputFieldValueChanged;

    [SerializeField]
    private Button _createButton;
    [SerializeField]
    private Button _rotateButton;
    [SerializeField]
    private Button _deleteButton;
    [SerializeField]
    private Button _acceptButton;
    [SerializeField]
    private Slider _sensitivitySlider;
    [SerializeField]
    private InputField _lengthInputField; 
    [SerializeField]
    private InputField _widthInputField;
    [SerializeField]
    private InputField _heightInputField;

    private float _sliderScale = 10f;

    private void OnEnable()
    {
        _createButton.onClick.AddListener(CreateBox);
        _rotateButton.onClick.AddListener(RotateBox);
        _deleteButton.onClick.AddListener(DeleteBox);
        _acceptButton.onClick.AddListener(Accept);
        _sensitivitySlider.onValueChanged.AddListener(SensitivityValueChanged);
        _lengthInputField.onValueChanged.AddListener(LengthValueChanged);
        _widthInputField.onValueChanged.AddListener(WidthValueChanged);
        _heightInputField.onValueChanged.AddListener(HeightValueChanged);
    }

    private void OnDisable()
    {
        _createButton.onClick.RemoveAllListeners();
        _rotateButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.RemoveAllListeners();
        _acceptButton.onClick.RemoveAllListeners();
        _sensitivitySlider.onValueChanged.RemoveAllListeners();
        _lengthInputField.onValueChanged.RemoveAllListeners();
        _widthInputField.onValueChanged.RemoveAllListeners();
        _heightInputField.onValueChanged.RemoveAllListeners();

    }

    private void Start()
    {
        SetEditMode(false);
    }

    public void SetEditMode(bool enable)
    {
        _createButton.gameObject.SetActive(!enable);
        _rotateButton.gameObject.SetActive(enable);
        _deleteButton.gameObject.SetActive(enable);
        _acceptButton.gameObject.SetActive(enable);
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

    private void SensitivityValueChanged(float value)
    {
        OnSliderValueChanged?.Invoke(value / _sliderScale);
    }

    private void LengthValueChanged(string input)
    {
        if (float.TryParse(input, out float value))
        {
            OnInputFieldValueChanged?.Invoke(Size.Lenght, value);
        }
    }
    
    private void WidthValueChanged(string input)
    {
        if (float.TryParse(input, out float value))
        {
            OnInputFieldValueChanged?.Invoke(Size.Width, value);
        }
    }    
    
    private void HeightValueChanged(string input)
    {
        if (float.TryParse(input, out float value))
        {
            OnInputFieldValueChanged?.Invoke(Size.Height, value);
        }
    }
}