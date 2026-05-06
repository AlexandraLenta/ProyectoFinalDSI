using UnityEngine;
using UnityEngine.UIElements;

public class HelpMenu : MonoBehaviour
{
    GameScreen _gameScreen;

    VisualElement _root;

    VisualElement contenidoCharacter;
    VisualElement contenidoMovement;
    VisualElement contenidoAttack;

    Button pestaniaCharacter;
    Button pestaniaMovement;
    Button pestaniaAttack;

    Button backButton;

    void OnEnable()
    {
        _gameScreen = GetComponent<GameScreen>();

        _root = GetComponent<UIDocument>().rootVisualElement;
        _root.Q("HelpScreen").style.display = DisplayStyle.Flex;

        pestaniaCharacter = _root.Q<Button>("CharacterTab");
        pestaniaMovement = _root.Q<Button>("MovementTab");
        pestaniaAttack = _root.Q<Button>("AttackTab");

        contenidoCharacter = _root.Q<VisualElement>("CharacterPage");
        contenidoMovement = _root.Q<VisualElement>("MovementPage");
        contenidoAttack = _root.Q<VisualElement>("AttackPage");

        pestaniaCharacter.RegisterCallback<ClickEvent>(ShowCharacter);
        pestaniaMovement.RegisterCallback<ClickEvent>(ShowMovement);
        pestaniaAttack.RegisterCallback<ClickEvent>(ShowAttack);

        RegisterButtonEffects(pestaniaCharacter);

        RegisterButtonEffects(pestaniaMovement);

        RegisterButtonEffects(pestaniaAttack);


        ShowCharacter(null);
    }


    void NoContenido()
    {
        contenidoCharacter.style.display = DisplayStyle.None;

        contenidoMovement.style.display = DisplayStyle.None;

        contenidoAttack.style.display = DisplayStyle.None;
    }

    void ShowCharacter(ClickEvent evt)
    {
        NoContenido();

        contenidoCharacter.style.display = DisplayStyle.Flex;
    }

    void ShowMovement(ClickEvent evt)
    {
        NoContenido();

        contenidoMovement.style.display = DisplayStyle.Flex;
    }

    void ShowAttack(ClickEvent evt)
    {
        NoContenido();

        contenidoAttack.style.display = DisplayStyle.Flex;
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

        button.RemoveFromClassList("menu-button-pressed");
    }

    void OnDisable()
    {
        _root.Q("HelpScreen").style.display = DisplayStyle.None;
    }
}