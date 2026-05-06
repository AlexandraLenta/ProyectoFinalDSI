using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

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
    Dictionary<string, string> _imageMap = new Dictionary<string, string>(); // key - value : nombre de imagen, path de imagen

    GameScreen _gameScreen;
    HelpMenu _helpMenu;

    // Variables por defecto
    const string DEFAULT_NAME = "Unnamed";
    const int DEFAULT_HP = 100;
    const int DEFAULT_ATTACK = 10;
    const string DEFAULT_IMAGE = "default_char";

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
        _helpButton.RegisterCallback<ClickEvent>(OpenHelp);


        // crear lista de personajes
        _characterList = new List<Character>();

        // cargar las imagenes
        LoadImageDatabase();
    }

    void LoadImageDatabase()
    {
        // archivo
        TextAsset jsonFile = Resources.Load<TextAsset>("Characters/characterImages");

        Debug.Log(jsonFile.text);
        // coger el contenido del json
        ImageDatabase db = JsonUtility.FromJson<ImageDatabase>(jsonFile.text);

        List<string> imgOptions = new List<string>();

        foreach (var img in db.images)
        {
            imgOptions.Add(img.displayName);
            _imageMap[img.displayName] = img.path;
        }

        _imageDropdown.choices = imgOptions;
    }
    
    // agregar personaje
    void AddCharacter(ClickEvent ev, CharacterType type)
    {
        if (_characterList.Count >= 4)
        {
            Debug.Log("Max 4 characters reached!");
            return;
        }

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        TextField nameField = root.Q<TextField>("NameField");
        IntegerField hpField = root.Q<IntegerField>("HpField");
        SliderInt attackField = root.Q<SliderInt>("AttackSlider");

        string nameValue = string.IsNullOrWhiteSpace(nameField.value) 
            ? DEFAULT_NAME 
            : nameField.value;

        int hpValue = hpField.value <= 0 
            ? DEFAULT_HP 
            : hpField.value;

        int attackValue = attackField.value <= 0 
            ? DEFAULT_ATTACK 
            : attackField.value;

        string imageName = string.IsNullOrEmpty(_currImageName) 
            ? DEFAULT_IMAGE 
            : _currImageName;

        Texture2D testImage = Resources.Load<Texture2D>(imageName);
        if (testImage == null)
        {
            Debug.LogWarning("Image not found, using default.");
            imageName = DEFAULT_IMAGE;
        }

        Character character = new Character(nameField.value, hpField.value, attackField.value, type, _currImageName);

        _characterList.Add(character);

        // Resetear despues de aniadir 
        nameField.value = "";
        hpField.value = DEFAULT_HP;
        attackField.value = DEFAULT_ATTACK;
    }
    
    // cambio de imagen en dropdown: coger nombre de imagen, cargar imagen
    void ChangeImage(ChangeEvent<string> ev)
    {
        string selected = ev.newValue;

        if (!_imageMap.ContainsKey(selected))
            return;

        _currImageName = _imageMap[selected];

        _currImage = Resources.Load<Texture2D>(_currImageName);

        _currImageElement.style.backgroundImage = new StyleBackground(_currImage);
    }

    void StartGame(ClickEvent ev)
    {
        SaveCharactersToJson();

        _gameScreen.enabled = true;

        this.enabled = false;
    }

    void SaveCharactersToJson()
    {
        string json = JsonHelperCharacter.ToJson(_characterList, true);

        string path = Path.Combine(Application.persistentDataPath, "characters.json");

        File.WriteAllText(path, json);

        Debug.Log("Saved JSON to: " + path);
    }

    void OpenHelp(ClickEvent ev)
    {
        _helpMenu.enabled = true;
        _helpMenu.PreviousScreen = this;

        this.enabled = false;
    }

    void OnDisable()
    {
        // desactivar setup screen
        _root.Q("SetupScreen").style.display = DisplayStyle.None;
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

    //     button.RemoveFromClassList("classic-button-pressed");
    // }
}