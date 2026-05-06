using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    EndScreen _endScreen;
    HelpMenu _helpMenu;
    VisualElement _root;

    Button _helpButton;
    Button _moveButton;
    Button _attackButton;

    void OnEnable()
    {
        _helpMenu = GetComponent<HelpMenu>();
        _root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameScreen = _root.Q("GameScreen");
        _root.Q("GameScreen").style.display = DisplayStyle.Flex;

        _moveButton = _root.Q<Button>("MoveButton");
        _attackButton = _root.Q<Button>("AttackButton");
        _helpButton = gameScreen.Q<Button>("HelpButton");


        RegisterButtonEffects(_moveButton);

        RegisterButtonEffects(_attackButton);

        _helpButton.RegisterCallback<ClickEvent>(OpenHelp);

    }

    void RegisterButtonEffects(Button button)
    {
        button.RegisterCallback<MouseEnterEvent>(OnHoverEnter);

        button.RegisterCallback<MouseLeaveEvent>(OnHoverExit);

        button.RegisterCallback<MouseDownEvent>(OnPressed);

        button.RegisterCallback<MouseUpEvent>(OnReleased);
    }

    void OnHoverEnter(MouseEnterEvent ev)
    {
        VisualElement button = ev.currentTarget as VisualElement;

        button.AddToClassList("classic-button-hover");
    }

    void OnHoverExit(MouseLeaveEvent ev)
    {
        VisualElement button = ev.currentTarget as VisualElement;

        button.RemoveFromClassList("classic-button-hover");
    }

    void OnPressed(MouseDownEvent ev)
    {
        VisualElement button = ev.currentTarget as VisualElement;

        button.AddToClassList("classic-button-pressed");
    }

    void OnReleased(MouseUpEvent ev)
    {
        VisualElement button = ev.currentTarget as VisualElement;

        button.RemoveFromClassList("classic-button-pressed");
    }

    void OpenHelp(ClickEvent ev)
    {
        Debug.Log("open help menu");
        _helpMenu.enabled = true;

        this.enabled = false;
    }


    void OnDisable()
    {
        _root.Q("GameScreen").style.display = DisplayStyle.None;

    }
}