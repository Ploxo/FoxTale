using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMenu : MonoBehaviour
{
    public void QuitGame()
    {
        GameManager.instance.QuitGame(GameManager.SceneName.GAMEPLAY);
    }
}
