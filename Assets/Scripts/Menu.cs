using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        Global.persons.Clear();
        SceneManager.LoadScene("Main");
    }
}
