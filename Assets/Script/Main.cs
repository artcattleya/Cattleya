using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void OnClickButton ()
    {
        SceneManager.LoadScene("Gallery");
    }
    
}
