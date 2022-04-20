using UnityEngine;

public class GeneratorView : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layer;

    public LayerMask Layer => _layer;
}