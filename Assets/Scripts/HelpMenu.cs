using System.ComponentModel;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UIElements;

public class HelpMenu : MonoBehaviour
{
    MonoBehaviour _previousScreen;

    public MonoBehaviour PreviousScreen {
        get {return _previousScreen;}
        set { _previousScreen = value;}
    }

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

        backButton = _root.Q<Button>("Back");
        backButton.RegisterCallback<ClickEvent>(ReturnToPrevious);

        ShowCharacter(null);
    }


    void NoContenido()
    {
        contenidoCharacter.style.display = DisplayStyle.None;

        contenidoMovement.style.display = DisplayStyle.None;

        contenidoAttack.style.display = DisplayStyle.None;
        
        pestaniaAttack.RemoveFromClassList("help-tab-button-selected");
        pestaniaCharacter.RemoveFromClassList("help-tab-button-selected");
        pestaniaMovement.RemoveFromClassList("help-tab-button-selected");
    }

    void ShowCharacter(ClickEvent evt)
    {
        NoContenido();

        contenidoCharacter.style.display = DisplayStyle.Flex;
        pestaniaCharacter.AddToClassList("help-tab-button-selected");
    }

    void ShowMovement(ClickEvent evt)
    {
        NoContenido();

        contenidoMovement.style.display = DisplayStyle.Flex;
        pestaniaMovement.AddToClassList("help-tab-button-selected");
    }

    void ShowAttack(ClickEvent evt)
    {
        NoContenido();

        contenidoAttack.style.display = DisplayStyle.Flex;
        pestaniaAttack.AddToClassList("help-tab-button-selected");
    }

    void ReturnToPrevious(ClickEvent ev)
    {
        if (_previousScreen != null)
        {
            _previousScreen.enabled = true;
            this.enabled = false;
        }
    }

    void OnDisable()
    {
        _root.Q("HelpScreen").style.display = DisplayStyle.None;
    }
    // void RegisterButtonEffects(Button button)
    // {
    //     button.RegisterCallback<MouseEnterEvent>(OnHoverEnter);

    //     button.RegisterCallback<MouseLeaveEvent>(OnHoverExit);

    //     button.RegisterCallback<MouseDownEvent>(OnPressed);

    //     button.RegisterCallback<MouseUpEvent>(OnReleased);
    // }

    // void OnHoverEnter(MouseEnterEvent ev)
    // {
    //     VisualElement button = ev.currentTarget as VisualElement;

    //     button.AddToClassList("classic-button-hover");
    // }

    // void OnHoverExit(MouseLeaveEvent ev)
    // {
    //     VisualElement button = ev.currentTarget as VisualElement;

    //     button.RemoveFromClassList("classic-button-hover");
    // }

    // void OnPressed(MouseDownEvent ev)
    // {
    //     VisualElement button = ev.currentTarget as VisualElement;

    //     button.AddToClassList("classic-button-pressed");
    // }

    // void OnReleased(MouseUpEvent ev)
    // {
    //     VisualElement button = ev.currentTarget as VisualElement;

    //     button.RemoveFromClassList("menu-button-pressed");
    // }
}