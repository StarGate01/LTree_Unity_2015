using UnityEngine;
using System.Collections;

public class LMaterialColor : MonoBehaviour
{

    [Space(10)]
    public Material Material;
    public Gradient Colors = new Gradient();
    [Range(0, 1)]
    public float Time = 0.5f;

    [ContextMenu("Apply")]
    public void Apply()
    {
        Material.color = Colors.Evaluate(Time);
    }

}
