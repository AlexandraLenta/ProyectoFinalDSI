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

    // personaje -- contenedor
    Dictionary<Character, VisualElement> _characterVisuals = new Dictionary<Character, VisualElement>();

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

        _moveButton.RegisterCallback<ClickEvent>(ActivateMoveMode);
        _attackButton.RegisterCallback<ClickEvent>(ActivateAttackMode);
        
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

                CreateCharacterVisual(slot, enemySlots[enemyIndex], character, texture);

                enemyIndex++;
            }

            // player
            else
            {
                if (playerIndex >= playerSlots.Count)
                    continue;

                VisualElement slot = _root.Q<VisualElement>(playerSlots[playerIndex]);

                CreateCharacterVisual(slot, playerSlots[playerIndex], character, texture);

                playerIndex++;
            }
        }
    }

    void CreateCharacterVisual(VisualElement slot, string slotName, Character character, Texture2D texture)
    {
        slot.Clear();

        // contenedor
        VisualElement characterContainer = new VisualElement();

        characterContainer.style.flexGrow = 1;
        characterContainer.style.flexDirection = FlexDirection.Column;

        // imagen
        VisualElement image = new VisualElement();

        image.style.flexGrow = 1;
        image.style.backgroundImage = new StyleBackground(texture);

        // nombre
        Label label = new Label(character.Name);

        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.fontSize = 18;

        // agregar al contenedor principal
        characterContainer.Add(image);
        characterContainer.Add(label);

        slot.Add(characterContainer);

        // guardar
        _slotCharacters[slotName] = character;
        _characterVisuals[character] = characterContainer;

        // click
        slot.RegisterCallback<ClickEvent>((evt) =>
        {
            _selectedCharacter = character;
            _selectedSlot = slot;

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

        ClearHighlights(); // para resetear los colores

        // resetear accion
        _currentMode = Action.NONE;
    }
    void ActivateMoveMode(ClickEvent ev)
    {
        if (_selectedCharacter == null)
            return;

        _currentMode = Action.MOVE;

        HighlightSlots(Color.blue);
    }

    void ActivateAttackMode(ClickEvent ev)
    {
        if (_selectedCharacter == null)
            return;

        _currentMode = Action.ATTACK;

        HighlightSlots(Color.red);
    }

    void HighlightSlots(Color color)
    {
        ClearHighlights();

        foreach (var pair in _gridSlots)
        {
            VisualElement slot = pair.Value;

            slot.style.backgroundColor = color;

            _highlightedSlots.Add(slot);
        }
    }

    void ClearHighlights()
    {
        foreach (VisualElement slot in _highlightedSlots)
        {
            slot.style.backgroundColor = Color.clear;
        }

        _highlightedSlots.Clear();
    }

    void MoveCharacter(string targetSlotName)
    {
        // si esta ocupada no hacemos nada
        if (_slotCharacters.ContainsKey(targetSlotName))
            return;

        VisualElement targetSlot = _gridSlots[targetSlotName];

        // encontrar el slot viejo de este personaje
        string oldSlotName = "";

        foreach (var pair in _slotCharacters)
        {
            if (pair.Value == _selectedCharacter)
            {
                oldSlotName = pair.Key;
                break;
            }
        }

        if (oldSlotName == "")
            return;

        VisualElement oldSlot = _gridSlots[oldSlotName];

        VisualElement characterVisual = _characterVisuals[_selectedCharacter];

        oldSlot.Remove(characterVisual);

        targetSlot.Add(characterVisual);

        // actualizar todo
        _slotCharacters.Remove(oldSlotName);

        _slotCharacters[targetSlotName] =
            _selectedCharacter;

        _selectedSlot = targetSlot;
    }

    void AttackCharacter(string slotName)
    {
        // si en la casilla destino no hay personaje, no hacemos nada
        if (!_slotCharacters.ContainsKey(slotName))
            return;

        // personaje a daniar
        Character target = _slotCharacters[slotName];

        // miramos si es del mismo equipo
        if (target.CharacterType == _selectedCharacter.CharacterType)
            return;

        // quitamos puntos de vida
        target.HP -= _selectedCharacter.Attack;

        Debug.Log(target.Name + " HP: " + target.HP);

        // Si ha muerto
        if (target.HP <= 0)
        {
            VisualElement targetSlot = _gridSlots[slotName];

            targetSlot.Clear();

            _slotCharacters.Remove(slotName);

            Debug.Log(target.Name + " dead");
        }

        ShowCharacterStats(target);
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