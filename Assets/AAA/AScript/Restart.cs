using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void GameEnd()
    {
        SceneManager.LoadScene(0);
    }
}
