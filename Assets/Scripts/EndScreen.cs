using UnityEngine;
using UnityEngine.UIElements;

public class EndScreen : MonoBehaviour
{
    StartScreen _startScreen;
    SetupScreen _setupScreen;
    VisualElement _root;

    Button _retryButton;
    Button _newGameButton;

    void OnEnable()
    {
        _startScreen = GetComponent<StartScreen>();

        _root = GetComponent<UIDocument>().rootVisualElement;

        // activar endscreen
        _root.Q("EndScreen").style.display = DisplayStyle.Flex;

        // botones
        _retryButton = _root.Q<Button>("RetryButton");
        _newGameButton = _root.Q<Button>("NewGameButton");

        _retryButton.RegisterCallback<ClickEvent>(OnRetry);

        _newGameButton.RegisterCallback<ClickEvent>(OnNewGame);
    }

    void OnRetry(ClickEvent ev)
    {
        _startScreen.enabled = true;

        this.enabled = false;
    }

    void OnNewGame(ClickEvent ev)
    {
        _setupScreen.enabled = true;

        this.enabled = false;

    }

    void OnDisable()
    {
        _root.Q("EndScreen").style.display = DisplayStyle.None;

    }
}