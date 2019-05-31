using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int row;
    public int column;
    public bool isMerged;
    private Image tile;
    private Text numberText;
    private int number;
    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number == 0)
            {
                SetEpmty();
            }
            else
            {
                SetVisible();
                ApplyStyleByNumber(value);
            }
        }
    }

    private void Awake()
    {
        numberText = GetComponentInChildren<Text>();
        tile = transform.Find("TileBg").GetComponent<Image>();
    }
    void Start()
    {
        
    }
    private void SetVisible()
    {
        tile.gameObject.SetActive(true);
        numberText.gameObject.SetActive(true);
    }

    private void SetEpmty()
    {
        tile.gameObject.SetActive(false);
        numberText.gameObject.SetActive(false);
    }

    void AppStyleFromHolder(int index)
    {
        numberText.text = TileStyleHolder.Instance.TileStyles[index].Number.ToString();
        numberText.color = TileStyleHolder.Instance.TileStyles[index].NumberColor;
        tile.color = TileStyleHolder.Instance.TileStyles[index].TileColor;
    }
    void ApplyStyleByNumber(int num)
    {
        switch (num)
        {
            case 2:
                AppStyleFromHolder(0);
                break;
            case 4:
                AppStyleFromHolder(1);
                break;
            case 8:
                AppStyleFromHolder(2);
                break;
            case 16:
                AppStyleFromHolder(3);
                break;
            case 32:
                AppStyleFromHolder(4);
                break;
            case 64:
                AppStyleFromHolder(5);
                break;
            case 128:
                AppStyleFromHolder(6);
                break;
            case 256:
                AppStyleFromHolder(7);
                break;
            case 512:
                AppStyleFromHolder(8);
                break;
            case 1024:
                AppStyleFromHolder(9);
                break;
            case 2048:
                AppStyleFromHolder(10);
                break;
            default:
                AppStyleFromHolder(10);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
