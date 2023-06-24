using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [SerializeField] Text scoreText;

    private int currentScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        //StartCounter.OnStartGame += OnStartGame();
        this.RefreshScores();

    }

    private void RefreshScores()
    {
        this.scoreText.text = currentScore.ToString();


    }

    public void AddScores(int amount)
    {
        currentScore += amount;
        this.RefreshScores();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartGame()
    {

    }
}
