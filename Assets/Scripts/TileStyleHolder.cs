using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileStyle
{
    public int Number;
    public Color TileColor;
    public Color NumberColor;
}

public class TileStyleHolder : MonoBehaviour
{
    public static TileStyleHolder Instance;
    public TileStyle[] TileStyles;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
