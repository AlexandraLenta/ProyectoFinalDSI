using UnityEngine;

public class FullGame : MonoBehaviour
{
    StartScreen _startScreen;
    SetupScreen _setupScreen;
    GameScreen _gameScreen;
    HelpMenu _helpMenu;
    EndScreen _endScreen;
    void OnEnable()
    {
        // coger los scripts de las pantallas
        _startScreen = gameObject.GetComponent<StartScreen>();
        _setupScreen = gameObject.GetComponent<SetupScreen>();
        _gameScreen = gameObject.GetComponent<GameScreen>();
        _helpMenu = gameObject.GetComponent<HelpMenu>();
        _endScreen = gameObject.GetComponent<EndScreen>();

        // desactivar todos excepto el de principio
        _startScreen.enabled = true;
        _setupScreen.enabled = false;
        _gameScreen.enabled = false;
        _helpMenu.enabled = false;
        _endScreen.enabled = false;
    }
}
