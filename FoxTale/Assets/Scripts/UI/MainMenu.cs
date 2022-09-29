using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.FadeAndLoadScene(GameManager.SceneName.GAMEPLAY, GameManager.SceneName.MAIN_MENU);
        SoundManager.instance.PlaySound("button_ok");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
