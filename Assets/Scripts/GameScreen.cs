using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    EndScreen _endScreen;

    VisualElement _root;

    Button _helpButton;
    Button _moveButton;
    Button _attackButton;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _root.Q("GameScreen").style.display = DisplayStyle.Flex;

        _moveButton = _root.Q<Button>("MoveButton");
        _attackButton = _root.Q<Button>("AttackButton");

        RegisterButtonEffects(_moveButton);

        RegisterButtonEffects(_attackButton);
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


    void OnDisable()
    {
        _root.Q("GameScreen").style.display = DisplayStyle.None;

    }
}