using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    Playing,
    GameOver,
    WaitForMoveEnd
}
public class GameManager : MonoBehaviour
{

    private Tile[] allTiles;
    private List<Tile> emptyTiles;
    private Tile[,] xyTiles;
    private bool makeMove;
    private bool[] moveComplete = new bool[] { true, true, true, true };

    public List<Tile[]> rowTiles;
    public List<Tile[]> columnTiles;
    public GameObject GameOverPanel;
    public GameObject WinPanel;
    public GameState state = GameState.Playing;
    public bool IsWin;

    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        IsWin = false;
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
                // 得分
                ScoreTracker.Instance.Score += lineOfTiles[i].Number;
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
                // 得分
                ScoreTracker.Instance.Score += lineOfTiles[i].Number;
                lineOfTiles[i].isMerged = true;
                if (lineOfTiles[i].Number==2048&&IsWin==false)
                {
                    GameWin();
                }
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

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="md">移动方向</param>
    public void Move(MoveDirection md)
    {
        ResetAllMergeFlags();
        if (state!=GameState.Playing)
        {
            return;
        }

        if (delay > 0)
        {
            StartCoroutine(MoveCoroutine(md));
        }
        else
        {
            

        }
        ResetAllMergeFlags();
        
    }

    IEnumerator MoveCoroutine(MoveDirection md)
    {
        yield return null;
        state = GameState.WaitForMoveEnd;
        switch (md)
        {
            case MoveDirection.left:
                for (int i = 0; i < rowTiles.Count; i++)
                {
                    StartCoroutine(MakeOneMoveDownIndexCort(rowTiles[i],i));
                }
                break;
            case MoveDirection.right:
                for (int i = 0; i < rowTiles.Count; i++)
                {
                    StartCoroutine(MakeOneMoveUpIndexCort(rowTiles[i], i));
                }
                break;
            case MoveDirection.up:
                for (int i = 0; i < columnTiles.Count; i++)
                {
                    StartCoroutine(MakeOneMoveDownIndexCort(columnTiles[i], i));
                }
                break;
            case MoveDirection.down:
                for (int i = 0; i < columnTiles.Count; i++)
                {
                    StartCoroutine(MakeOneMoveUpIndexCort(columnTiles[i], i));
                }
                break;
            default:
                break;
        }
        while (moveComplete[0]==false|| moveComplete[1] == false || moveComplete[2] == false || moveComplete[3] == false)
        {
            yield return null;
        }
        // 如果发生移动，更新空格子,生成一个数字格子
        if (makeMove)
        {
            UpdateEmptyTiles();
            Generate();
            if (CanMove() == false)
            {
                GameOver();
            }
        }
        state = GameState.Playing;


    }

    IEnumerator MakeOneMoveDownIndexCort(Tile[] lineOfTiles, int index)
    {
        moveComplete[index] = false;
        while (MakeOneMoveDownIndex(lineOfTiles))
        {
            makeMove = true;
            yield return new WaitForSeconds(delay);
        }
        moveComplete[index] = true;
    }
    IEnumerator MakeOneMoveUpIndexCort(Tile[] lineOfTiles, int index)
    {
        moveComplete[index] = false;
        while (MakeOneMoveUpIndex(lineOfTiles))
        {
            makeMove = true;
            yield return new WaitForSeconds(delay);
        }
        moveComplete[index] = true;
    }
    /// <summary>
    /// 重置所有合并标记
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

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void NewGame() => SceneManager.LoadScene(0);

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        state = GameState.GameOver;
        GameOverPanel.SetActive(true);
    }

    /// <summary>
    /// 胜利
    /// </summary>
    void GameWin()
    {
        WinPanel.SetActive(true);
        IsWin = true;
    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    public void KeepGoing()
    {
        WinPanel.SetActive(false);
        state = GameState.Playing;
    }

    /// <summary>
    /// 是否可以移动
    /// </summary>
    /// <returns></returns>
    bool CanMove()
    {
        if (emptyTiles.Count>0)
        {
            return true;
        }
        foreach (var item in rowTiles)
        {
            for (int i = 0; i < item.Length-1; i++)
            {
                if (item[i].Number==item[i+1].Number)
                {
                    return true;
                }
            }
        }
        foreach (var item in columnTiles)
        {
            for (int i = 0; i < item.Length - 1; i++)
            {
                if (item[i].Number == item[i + 1].Number)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
