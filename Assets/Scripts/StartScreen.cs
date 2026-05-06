using UnityEngine;
using UnityEngine.UIElements;

public class StartScreen : MonoBehaviour
{
    SetupScreen _setupScreen;
    VisualElement _root;
    Button _startButton;

    void OnEnable()
    {
        _setupScreen = GetComponent<SetupScreen>();

        _root = GetComponent<UIDocument>().rootVisualElement;

        // activar el start screen
        _root.Q("StartScreen").style.display = DisplayStyle.Flex;
        // coger boton
        _startButton = _root.Q<Button>("StartButton");

        _startButton.RegisterCallback<ClickEvent>(OnStart);

    
        _startButton.RegisterCallback<MouseEnterEvent>(OnHoverEnter);

 
        _startButton.RegisterCallback<MouseLeaveEvent>(OnHoverExit);


        _startButton.RegisterCallback<MouseDownEvent>(OnPressed);

        _startButton.RegisterCallback<MouseUpEvent>(OnReleased);
    }

    // cambiar al setup screen cuando empieza el juego
    void OnStart(ClickEvent ev)
    {
        _setupScreen.enabled = true;
        this.enabled = false;
    }

    void OnHoverEnter(MouseEnterEvent ev)
    {
        _startButton.AddToClassList("classic-button-hover");
    }

    void OnHoverExit(MouseLeaveEvent ev)
    {
        _startButton.RemoveFromClassList("classic-button-hover");
    }
    void OnPressed(MouseDownEvent ev)
    {
        _startButton.AddToClassList("classic-button-pressed");
    }

    void OnReleased(MouseUpEvent ev)
    {
        _startButton.RemoveFromClassList("classic-button-pressed");
    }

    void OnDisable()
    {
        // desactivar start screen
        _root.Q("StartScreen").style.display = DisplayStyle.None;
    }
}
