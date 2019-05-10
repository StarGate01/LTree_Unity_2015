using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Tree.Scripts.Geometry;
using Assets.Tree.Scripts.Geometry.Meshes;
using Assets.Tree.Scripts.Abstract;
using Assets.Tree.Scripts.Abstract.Lindenmayer;
using System.Reflection;

public class LTree : MonoBehaviour
{

    [Space(10)]
    public string TreeClassName = "Default";
    [Space(10)]
    public int Seed = 41;
    [Range(1, 50)]
    public int Iterations = 28;
    [Range(5, 26)]
    public int Quality = 8;
    [Space(10)]
    public NamedFloat[] SpecificSettings = new NamedFloat[0];

    private Generator generator = new Generator();
    private ILindenmayerConfigurationProvider provider;

    [ContextMenu("Load config")]
    void LoadConfig()
    {
        object handle = Activator.CreateInstance(Type.GetType("Assets.Tree.Scripts.LindenmayerConfigurationProviders." + TreeClassName));
        provider = (ILindenmayerConfigurationProvider)handle;
        SpecificSettings = provider.PreConfig();
        Debug.Log("LTree: Config '" + TreeClassName + "' loaded");
    }

    [ContextMenu("Regenerate Mesh")]
    void RegenerateMesh()
    {
        if (provider != null && SpecificSettings != null)
        {
            GetComponent<MeshFilter>().mesh = generator.LTree(provider.Get(Quality, Seed, Iterations, NamedFloatCoverter.ToDictionary(SpecificSettings)));
        }
        else
        {
            Debug.LogError("LTree: No config was loaded!");
        }
        //GetComponent<MeshFilter>().mesh = generator.DoubleQuad(20, 20);
    }

    //public void Start()
    //{
    //    GetComponent<LMaterialColor>().Time = 0;
    //}

    //public void FixedUpdate()
    //{
    //    GetComponent<LMaterialColor>().Time += 0.0005f;
    //    if (GetComponent<LMaterialColor>().Time > 1) GetComponent<LMaterialColor>().Time = 0;
    //}

}
