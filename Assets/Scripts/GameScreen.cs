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
        //foreach (Character character in _characterList)
        //{
        //    // Load image again from Resources
        //    Texture2D texture = Resources.Load<Texture2D>(character.ImageName);

        //    Debug.Log($"Character: {character.Name}, HP: {character.HP}, ATK: {character.Attack}");
        //}

        List<string> enemySlots = new List<string>()
    {
        "1stRow1",
        "1stRow3",
        "1stRow5",
        "1stRow7"
    };

        List<string> playerSlots = new List<string>()
    {
        "5thRow2",
        "5thRow4",
        "5thRow6",
        "5thRow8"
    };

        int enemyIndex = 0;
        int playerIndex = 0;

        foreach (Character character in _characterList)
        {
            Texture2D texture = Resources.Load<Texture2D>(character.ImageName);

            Debug.Log($"Character: {character.Name}, HP: {character.HP}, ATK: {character.Attack}");

            // enemigos
            if (character.CharacterType == SetupScreen.CharacterType.ENEMY)
            {
                if (enemyIndex >= enemySlots.Count)
                    continue;

                VisualElement slot = _root.Q<VisualElement>(enemySlots[enemyIndex]);

                CreateCharacterVisual(slot, character, texture);

                enemyIndex++;
            }

            // player
            else
            {
                if (playerIndex >= playerSlots.Count)
                    continue;

                VisualElement slot = _root.Q<VisualElement>(playerSlots[playerIndex]);

                CreateCharacterVisual(slot, character, texture);

                playerIndex++;
            }
        }
    }

    void CreateCharacterVisual(VisualElement slot, Character character, Texture2D texture)
    {
        slot.Clear();

        // Imagen
        VisualElement image = new VisualElement();

        image.style.flexGrow = 1;
        image.style.backgroundImage = new StyleBackground(texture);
        

        // Nombre
        Label label = new Label(character.Name);

        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.fontSize = 18;
        label.style.color = Color.black;

        // Aniadir
        slot.Add(image);
        slot.Add(label);
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