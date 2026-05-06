using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;



public class GameScreen : MonoBehaviour
{
    List<Character> _characterList;
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

        _helpButton.RegisterCallback<ClickEvent>(OpenHelp);
        
        LoadCharactersFromJson(); 
    }

    void LoadCharactersFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "characters.json");

        if (!File.Exists(path))
        {
            Debug.LogError("JSON file not found!");
            return;
        }

        string json = File.ReadAllText(path);

        _characterList = JsonHelperCharacter.FromJson<Character>(json);

        Debug.Log("Loaded " + _characterList.Count + " characters");

        // Optional: recreate visuals or logic
        InitializeCharacters();
    }

    void InitializeCharacters()
    {
        foreach (Character character in _characterList)
        {
            // Load image again from Resources
            Texture2D texture = Resources.Load<Texture2D>(character.ImageName);

            Debug.Log($"Character: {character.Name}, HP: {character.HP}, ATK: {character.Attack}");
        }
    }

    void OpenHelp(ClickEvent ev)
    {
        _helpMenu.enabled = true;
        _helpMenu.PreviousScreen = this;

        this.enabled = false;
    }


    void OnDisable()
    {
        _root.Q("GameScreen").style.display = DisplayStyle.None;

    }

    //    void RegisterButtonEffects(Button button)
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