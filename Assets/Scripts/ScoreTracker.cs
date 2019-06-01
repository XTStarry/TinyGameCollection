using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker Instance;
    public Text BestText;
    public Text NumberText;
    private int score;
    private int bestScore;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        score = 0;
        bestScore = 0;

        // 读取最高分
        if (PlayerPrefs.HasKey("Best"))
        {
            bestScore = PlayerPrefs.GetInt("Best");
        }
        BestText.text = bestScore.ToString();
        NumberText.text = score.ToString();
    }

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            // 更新得分文本
            NumberText.text = score.ToString();
            // 更新最高分
            if(score>bestScore)
            {
                bestScore = score;
                // 存储到本地
                PlayerPrefs.SetInt("Best", bestScore);
                BestText.text = bestScore.ToString();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
