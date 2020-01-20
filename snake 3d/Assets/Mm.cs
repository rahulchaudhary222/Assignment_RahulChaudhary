using UnityEngine.UI;
using UnityEngine;


public class Mm : MonoBehaviour
{
    public Text highscore;
    void Start()
    {
        highscore.text = "High Score " + PlayerPrefs.GetInt("highscore").ToString();
    }
    
}
