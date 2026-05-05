using UnityEngine;
using UnityEngine.UIElements;

public class StartScreen : MonoBehaviour
{
    SetupScreen _setupScreen;
    VisualElement _root;
    
    void OnEnable()
    {
        _setupScreen = GetComponent<SetupScreen>();

        _root = GetComponent<UIDocument>().rootVisualElement;

        // activar el start screen
        _root.Q("StartScreen").style.display = DisplayStyle.Flex;
        // coger boton
        VisualElement startButton = _root.Q<Button>();
        startButton.RegisterCallback<ClickEvent>(OnStart);
    }

    // cambiar al setup screen cuando empieza el juego
    void OnStart(ClickEvent ev)
    {
        _setupScreen.enabled = true;
        this.enabled = false;
    }

    void OnDisable()
    {
        // desactivar start screen
        _root.Q("StartScreen").style.display = DisplayStyle.None;
    }
}
