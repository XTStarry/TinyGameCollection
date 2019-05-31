using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Tile[] allTiles;
    private List<Tile> emptyTiles;
    private Tile[,] xyTiles;

    public List<Tile[]> rowTiles;
    public List<Tile[]> columnTiles;
    // Start is called before the first frame update
    void Start()
    {
        xyTiles = new Tile[4, 4];
        allTiles = GameObject.FindObjectsOfType<Tile>();
        emptyTiles = new List<Tile>();
        rowTiles = new List<Tile[]>();
        columnTiles = new List<Tile[]>();
        foreach (var item in allTiles)
        {
            item.Number = 0;
            emptyTiles.Add(item);
            xyTiles[item.row, item.column] = item;
        }
        // 添加行数据
        rowTiles.Add(new Tile[] { xyTiles[0, 0], xyTiles[0, 1], xyTiles[0, 2], xyTiles[0, 3] });
        rowTiles.Add(new Tile[] { xyTiles[1, 0], xyTiles[1, 1], xyTiles[1, 2], xyTiles[1, 3] });
        rowTiles.Add(new Tile[] { xyTiles[2, 0], xyTiles[2, 1], xyTiles[2, 2], xyTiles[2, 3] });
        rowTiles.Add(new Tile[] { xyTiles[3, 0], xyTiles[3, 1], xyTiles[3, 2], xyTiles[3, 3] });
        // 添加列数据
        columnTiles.Add(new Tile[] { xyTiles[0, 0], xyTiles[1, 0], xyTiles[2, 0], xyTiles[3, 0] });
        columnTiles.Add(new Tile[] { xyTiles[0, 1], xyTiles[1, 1], xyTiles[2, 1], xyTiles[3, 1] });
        columnTiles.Add(new Tile[] { xyTiles[0, 2], xyTiles[1, 2], xyTiles[2, 2], xyTiles[3, 2] });
        columnTiles.Add(new Tile[] { xyTiles[0, 3], xyTiles[1, 3], xyTiles[2, 3], xyTiles[3, 3] });

        Generate();
        Generate();
    }

    /// <summary>
    /// 从小索引移动的逻辑,返回值为是否发生移动
    /// </summary>
    /// <param name="lineOfTiles">待移动块列</param>
    bool MakeOneMoveDownIndex(Tile[] lineOfTiles)
    {
        for (int i = 0; i < lineOfTiles.Length-1; i++)
        {
            if (lineOfTiles[i].Number == 0 && lineOfTiles[i + 1].Number != 0)
            {
                lineOfTiles[i].Number = lineOfTiles[i + 1].Number;
                lineOfTiles[i + 1].Number = 0;
                return true;
            }
            else if (lineOfTiles[i].Number != 0 && lineOfTiles[i].Number == lineOfTiles[i + 1].Number
                && lineOfTiles[i].isMerged == false && lineOfTiles[i + 1].isMerged == false)
            {
                lineOfTiles[i].Number *= 2;
                lineOfTiles[i + 1].Number = 0;
                lineOfTiles[i].isMerged = true;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 从大索引移动的逻辑,返回值为是否发生移动
    /// </summary>
    /// <param name="lineOfTiles">待移动块列</param>
    /// <returns></returns>
    bool MakeOneMoveUpIndex(Tile[] lineOfTiles)
    {
        for (int i = lineOfTiles.Length - 1; i > 0; i--)
        {
            if (lineOfTiles[i].Number == 0 && lineOfTiles[i - 1].Number != 0)
            {
                lineOfTiles[i].Number = lineOfTiles[i - 1].Number;
                lineOfTiles[i - 1].Number = 0;
                return true;
            }
            else if (lineOfTiles[i].Number != 0 && lineOfTiles[i].Number == lineOfTiles[i - 1].Number
                && lineOfTiles[i].isMerged == false && lineOfTiles[i - 1].isMerged == false)
            {
                lineOfTiles[i].Number *= 2;
                lineOfTiles[i - 1].Number = 0;
                lineOfTiles[i].isMerged = true;
                return true;
            }
        }
        return false;
    }

    void Generate()
    {
        if (emptyTiles.Count>0)
        {
            int randomIndex = Random.Range(0, emptyTiles.Count);
            int randomNumber = Random.Range(0, 10);
            if (randomNumber==0)
            {
                emptyTiles[randomIndex].Number = 4;
            }
            else
            {
                emptyTiles[randomIndex].Number = 2;
            }
            
            emptyTiles.RemoveAt(randomIndex);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(MoveDirection md)
    {
        ResetAllMergeFlags();
        bool makeMove = false;
        switch (md)
        {
            case MoveDirection.left:
                foreach (var item in rowTiles)
                {
                    while (MakeOneMoveDownIndex(item)) { makeMove = true; }
                }
                break;
            case MoveDirection.right:
                foreach (var item in rowTiles)
                {
                    while (MakeOneMoveUpIndex(item)) { makeMove = true; }
                }
                break;
            case MoveDirection.up:
                foreach (var item in columnTiles)
                {
                    while (MakeOneMoveDownIndex(item)) { makeMove = true; }
                }
                break;
            case MoveDirection.down:
                foreach (var item in columnTiles)
                { 
                    while (MakeOneMoveUpIndex(item)) { makeMove = true; }
                }
                break;
            default:
                break;
        }
        // 如果发生移动，更新空格子,生成一个数字格子
        if(makeMove)
        {
            UpdateEmptyTiles();
            Generate();
        }
    }

    /// <summary>
    /// 充值所有合并标记
    /// </summary>
    private void ResetAllMergeFlags()
    {
        foreach (var item in allTiles)
        {
            item.isMerged = false;
        }
    }

    /// <summary>
    /// 更新空格子
    /// </summary>
    private void UpdateEmptyTiles()
    {
        emptyTiles.Clear();
        foreach (var item in allTiles)
        {
            if (item.Number == 0)
                emptyTiles.Add(item);
        }
    }
}
