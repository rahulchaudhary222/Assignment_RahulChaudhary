using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Move : MonoBehaviour
{
    
    public float speed = 10f;
    public float turnspeed = 10f;
    public GameObject bodypart, body, foodred, foodblue , ovrtxt, head;
    int lastfood = 0;
    public Text scr, total;
    List<Transform> taillist = new List<Transform>();
    private Vector3 pos;
    private bool ate = false;
    Food redfood = JsonUtility.FromJson<Food>(File.ReadAllText(Application.streamingAssetsPath + "/foodred.json"));
    Food bluefood = JsonUtility.FromJson<Food>(File.ReadAllText(Application.streamingAssetsPath + "/foodblue.json"));


    void Start()
    {
        PlayerPrefs.SetInt("crntscr", 0);
        Time.timeScale = 1;
        InvokeRepeating("Go", 1/speed, 1/speed);
        PlayerPrefs.SetInt("crntscr", 0);
        scr.text = "score " + PlayerPrefs.GetInt("crntscr").ToString();
        
    }
    [System.Serializable]
    public class Food
    {
        public string color;
        public int streak;
        public int value;
        public bool eat;
    }


    void Update()
    {
        
        head.transform.eulerAngles += new Vector3(0, Input.GetAxis("Horizontal") * turnspeed, 0);
        
        //cheat code
        if (Input.GetKeyDown("a"))
        {
            ate = true;
        }


    }
    void Go()
    {
        Vector3 direction = Vector3.forward;
        pos = head.transform.position;
        head.transform.Translate(direction);
        if (ate)
        {
            GameObject ins = Instantiate(bodypart, pos, new Quaternion(90, 0f, 0f, 0f), body.transform);
            taillist.Insert(0, ins.transform);
            ate = false;
        }
        else if (taillist.Count > 0)
        {
            taillist.Last().position = pos;
            taillist.Insert(0, taillist.Last());
            taillist.RemoveAt(taillist.Count - 1);
        }
        if (redfood.eat)
        {
            Instantiate(foodred, new Vector3(Random.Range(-23f, 23f), 0.7f, Random.Range(-23f, 23f)), new Quaternion(0f, 0f, 0f, 0f));
            
            if (lastfood == 0)
            {
                PlayerPrefs.SetInt("crntscr", PlayerPrefs.GetInt("crntscr") + redfood.value * redfood.streak);
                redfood.streak++;
            }
            else
            {
                PlayerPrefs.SetInt("crntscr", PlayerPrefs.GetInt("crntscr") + redfood.value);
            }

            bluefood.streak = 1;
            lastfood = 0;
            scr.text = "score " + PlayerPrefs.GetInt("crntscr").ToString();
            redfood.eat = false;
        }

        if (bluefood.eat)
        {
            Instantiate(foodblue, new Vector3(Random.Range(-23f, 23f), 0.7f, Random.Range(-23f, 23f)), new Quaternion(0f, 0f, 0f, 0f)); ;
            if (lastfood == 1)
            {
                PlayerPrefs.SetInt("crntscr", PlayerPrefs.GetInt("crntscr") + bluefood.value * bluefood.streak);
                bluefood.streak++;
            }
            else
            {
                PlayerPrefs.SetInt("crntscr", PlayerPrefs.GetInt("crntscr") + bluefood.value);
            }
            
            redfood.streak = 1;
            lastfood = 1;
            scr.text = "score " + PlayerPrefs.GetInt("crntscr").ToString();
            bluefood.eat = false;

        }


    }
    

    
    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("food blue"))
        {
            bluefood.eat = true;
            Destroy(collision.gameObject);
            Grow();
        }
        if (collision.gameObject.CompareTag("food red"))
        {
            redfood.eat = true;
            Destroy(collision.gameObject);
            Grow();
        }
        
        if (collision.gameObject.CompareTag("bodyparts"))
        {
            Over();
        }
        if (collision.gameObject.CompareTag("boundry"))
        {
            Over();
        }
        
    }

    

    public void  Grow()
    {
        ate = true;
    }
    public void Over()
    {
        ovrtxt.SetActive(true);
        total.text = "score " + PlayerPrefs.GetInt("crntscr").ToString();
        if(PlayerPrefs.GetInt("crntscr") > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("crntscr"));
        }
        Time.timeScale = 0;
    }
}
