using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;

public class GameScreen : MonoBehaviour
{
    enum Action
    {
        NONE,
        MOVE,
        ATTACK
    }

    Action _currentMode = Action.NONE; // la accion que se ha seleccionado

    Character _selectedCharacter; // el personaje seleccionado
    VisualElement _selectedSlot; // la casilla del grid seleccionada

    // casilla ocupada -- personaje
    Dictionary<string, Character> _slotCharacters = new Dictionary<string, Character>();

    // casillas -- el elemento de la casilla
    Dictionary<string, VisualElement> _gridSlots = new Dictionary<string, VisualElement>();

    // casillas que se han coloreado (clickeables)
    List<VisualElement> _highlightedSlots = new List<VisualElement>();

    List<Character> _characterList;
    EndScreen _endScreen;
    HelpMenu _helpMenu;
    VisualElement _root;

    Button _helpButton;
    Button _moveButton;
    Button _attackButton;

    Label _nameLabel;
    Label _hpLabel;
    Label _attackLabel;

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

        _nameLabel = _root.Q<Label>("Name");
        _hpLabel = _root.Q<Label>("hp");
        _attackLabel = _root.Q<Label>("Attack");

        RegisterGridSlots();
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

        slot.Add(image);
        slot.Add(label);

        // click 
        slot.RegisterCallback<ClickEvent>((evt) =>
        {
            ShowCharacterStats(character);
        });
    }

    void ShowCharacterStats(Character character)
    {
        _nameLabel.text = "Name : " + character.Name;
        _hpLabel.text = "Hp : " + character.HP;
        _attackLabel.text = "Attack : " + character.Attack;
    }

    void RegisterGridSlots()
    {
        // los nombres de cada casilla
        string[] slotNames =
        {
            "1stRow1","1stRow3","1stRow5","1stRow7",
            "2ndRow1","2ndRow3","2ndRow5","2ndRow7",
            "3rdRow1","3rdRow3","3rdRow5","3rdRow7",
            "4thRow2","4thRow4","4thRow6","4thRow8",
            "5thRow2","5thRow4","5thRow6","5thRow8"
        };

        foreach (string slotName in slotNames)
        {
            VisualElement slot = _root.Q<VisualElement>(slotName);

            _gridSlots.Add(slotName, slot);

            slot.RegisterCallback<ClickEvent>((evt) =>
            {
                OnGridSlotClicked(slotName);
            });
        }
    }

    void OnGridSlotClicked(string slotName)
    {
        // si no se ha seleccionado personaje no hacemos nada
        if (_selectedCharacter == null)
            return;

        switch (_currentMode)
        {
            case Action.MOVE:
                MoveCharacter(slotName);
                break;

            case Action.ATTACK:
                AttackCharacter(slotName);
                break;
        }

        // ClearHighlights(); // para resetear los colores

        // resetear accion
        _currentMode = Action.NONE;
    }

    void MoveCharacter(string slotName)
    {
        // si esta ocupada la casilla, no nos podemos mover alli
        if (_slotCharacters.ContainsKey(slotName))
            return;

        // visual element de la casilla
        VisualElement targetSlot = _gridSlots[slotName];

        // vaciar la casilla destino
        targetSlot.Clear();

        // cogemos *todo* lo que esta en la casilla seleccionada por si acaso
        foreach (VisualElement child in _selectedSlot.Children())
        {
            // aniadimos todo a la casilla destino
            targetSlot.Add(child);
        }

        // vaciar la casilla source
        _selectedSlot.Clear();

        
        string oldSlot = "";

        foreach (var pair in _gridSlots) // buscamos en las casillas hasta encontrar la seleccionada
        {
            if (pair.Value == _selectedSlot)
            {
                oldSlot = pair.Key;
                break;
            }
        }

        // cambiamos el personaje a la nueva casilla
        _slotCharacters.Remove(oldSlot);
        _slotCharacters[slotName] = _selectedCharacter;

        // seleccionamos la nueva casilla
        _selectedSlot = targetSlot;
    }

    void AttackCharacter(string slotName)
    {
        
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