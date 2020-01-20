using UnityEngine.SceneManagement;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public void Loadscene(int x)
    {
        SceneManager.LoadScene(x);
    }
}
