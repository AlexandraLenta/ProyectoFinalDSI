using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetupScreen : MonoBehaviour
{
    public enum CharacterType
    {
        PLAYER,
        ENEMY
    }
    VisualElement _root;
    Button _playerButton;
    Button _enemyButton;
    Button _startButton;
    Button _helpButton;

    string _currImageName;
    DropdownField _imageDropdown;
    VisualElement _currImageElement;
    Texture2D _currImage;
    List<Character> _characterList;

    GameScreen _gameScreen;
    HelpMenu _helpMenu;
    void OnEnable()
    {
        //coger game screen
        _gameScreen = GetComponent<GameScreen>();
        _helpMenu = GetComponent<HelpMenu>();

        // coger todos los elementos necesarios
        _root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement setupScreen = _root.Q("SetupScreen");
        // activar el setup screen
        _root.Q("SetupScreen").style.display = DisplayStyle.Flex;

        _playerButton = _root.Q<Button>("PlayerButton");
        _enemyButton = _root.Q<Button>("EnemyButton");
        _startButton = _root.Q<Button>("StartGameButton");
        _helpButton = setupScreen.Q<Button>("HelpButton");
        _imageDropdown = _root.Q<DropdownField>("ImageField");
        _currImageElement = _root.Q("CharacterImage");

        // registrar callbacks
        _playerButton.RegisterCallback<ClickEvent, CharacterType>(AddCharacter, CharacterType.PLAYER);
        _enemyButton.RegisterCallback<ClickEvent, CharacterType>(AddCharacter, CharacterType.ENEMY);
        _startButton.RegisterCallback<ClickEvent>(StartGame);
        _imageDropdown.RegisterCallback<ChangeEvent<string>>(ChangeImage);

        RegisterButtonEffects(_playerButton);

        RegisterButtonEffects(_enemyButton);

        RegisterButtonEffects(_startButton);
        _helpButton.RegisterCallback<ClickEvent>(OpenHelp);


        // crear lista de personajes
        _characterList = new List<Character>();
    }
    
    // agregar personaje
    void AddCharacter(ClickEvent ev, CharacterType type)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        TextField nameField = root.Q<TextField>("NameField");
        IntegerField hpField = root.Q<IntegerField>("HpField");
        SliderInt attackField = root.Q<SliderInt>("AttackSlider");

        Character character = new Character(nameField.value, hpField.value, attackField.value, type, _currImage);

        _characterList.Add(character);
    }
    
    // cambio de imagen en dropdown: coger nombre de imagen, cargar imagen
    void ChangeImage(ChangeEvent<string> ev)
    {
        _currImageName = _imageDropdown.value;

        _currImage = Resources.Load<Texture2D>(_currImageName);

        _currImageElement.style.backgroundImage = new StyleBackground(_currImage);
    }

    void StartGame(ClickEvent ev)
    {
        _gameScreen.enabled = true;

        this.enabled = false;
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
        Debug.Log("OPEN HELP");
        _helpMenu.enabled = true;

        this.enabled = false;
    }

    void OnDisable()
    {
        // desactivar setup screen
        _root.Q("SetupScreen").style.display = DisplayStyle.None;
    }
}