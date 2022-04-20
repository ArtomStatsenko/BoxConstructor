using UnityEngine;

[CreateAssetMenu(fileName = "BoxModel" , menuName = "Models/Box", order = 1)]
public sealed class BoxModel : ScriptableObject
{
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _height; 
    [SerializeField]
    private float _width;

    public float Length
    {
        get => _length;
        set
        {
            if (value > 0)
            {
                _length = value;
            }
        }
    }

    public float Height
    {
        get => _height;
        set
        {
            if (value > 0)
            {
                _height = value;
            }
        }
    }
    public float Width
    {
        get => _width;
        set
        {
            if (value > 0)
            {
                _width = value;
            }
        }
    }

    public Vector3 Size => new Vector3(_length, _height, _width);
}