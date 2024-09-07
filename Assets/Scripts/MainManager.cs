using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    public static MainManager Instance; // The single instance of MainManager
    public string s_PLayerName; // the universal name
    public int highestScore = 0; // Highest Score
    public string s_BestPlayer; // string of Best Player
    public TextMeshProUGUI bestScore;


    // Start is called before the first frame update
    void Start()
    {
        s_PLayerName = MenuUIHandler.Instance.playerName; // It names the player based on the input of the user

        /*if (MenuUIHandler.Instance.b_HasLaunchedOnce == true)
        {
            MenuUIHandler.Instance.b_HasLaunchedOnce = true;
        }*/
        LoadData();

        bestScore.text = $"Best Score: {highestScore} by {s_BestPlayer}";
        ScoreText.text = $"{s_PLayerName}'s Score : {m_Points}"; // It displays the score of initital game
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{s_PLayerName}'s Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        CheckforHighScore(m_Points);
    }

    void CheckforHighScore(int points)
    {
        if (points > highestScore)
        {
            highestScore = points;
            saveData();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    public void saveData()
    {
        SaveData data = new SaveData();
        data.name = s_PLayerName;
        data.score = highestScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile_DataPersistence.js", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile_DataPersistence.js";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json); // It deserialize the data from Json to unity readable code
            if (data.name == null)
            {
                data.name = "";
            }
            if (s_BestPlayer != null)
            {
                s_BestPlayer = data.name;
            }

            highestScore = data.score;
            // update the best player to the current game
            // put the score of the data to the currennt score
            //MenuUIHandler.Instance.bestScore.text = $"Best Score: {highestScore} by {s_BestPlayer}";
        }
    }

}
