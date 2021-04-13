using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreValue = 0;
    TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
        scoreValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "SCORE: " + scoreValue;
    }
}
