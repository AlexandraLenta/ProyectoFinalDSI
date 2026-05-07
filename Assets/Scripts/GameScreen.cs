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

    // casilla -- posicion
    Dictionary<string, Vector2Int> _slotPositions = new Dictionary<string, Vector2Int>();

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

        _endScreen = GetComponent<EndScreen>();

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

        characterContainer.AddToClassList("character-container");

        characterContainer.style.flexDirection = FlexDirection.Column;

        // imagen
        VisualElement image = new VisualElement();

        image.AddToClassList("character-image");

        image.style.backgroundImage =
            new StyleBackground(texture);

        // nombre
        Label label = new Label(character.Name);
        
        label.AddToClassList("character-label");

        // agregar al contenedor principal
        characterContainer.Add(image);
        characterContainer.Add(label);

        slot.Add(characterContainer);

        // guardar
        _slotCharacters[slotName] = character;
        _characterVisuals[character] = characterContainer;
    }

    void ShowCharacterStats(Character character)
    {
        _nameLabel.text = "Name : " + character.Name;
        _hpLabel.text = "Hp : " + character.HP;
        _attackLabel.text = "Attack : " + character.Attack;
    }

    void RegisterGridSlots()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                string rowName = "";

                switch (y)
                {
                    case 0: rowName = "1st"; break;
                    case 1: rowName = "2nd"; break;
                    case 2: rowName = "3rd"; break;
                    case 3: rowName = "4th"; break;
                    case 4: rowName = "5th"; break;
                }

                string slotName =
                    rowName + "Row" + (x + 1);

                VisualElement slot =
                    _root.Q<VisualElement>(slotName);

                if (slot == null)
                {
                    Debug.LogError("Missing slot: " + slotName);
                    continue;
                }

                _gridSlots[slotName] = slot;

                _slotPositions[slotName] =
                    new Vector2Int(x, y);

                slot.RegisterCallback<ClickEvent>((evt) =>
                {
                    OnGridSlotClicked(slotName);
                });
            }
        }
    }

    void OnGridSlotClicked(string slotName)
    {
        // NORMAL SELECTION MODE
        if (_currentMode == Action.NONE)
        {
            if (_slotCharacters.ContainsKey(slotName))
            {
                Character clickedCharacter =
                    _slotCharacters[slotName];

                _selectedCharacter = clickedCharacter;

                _selectedSlot = _gridSlots[slotName];

                ShowCharacterStats(clickedCharacter);
            }

            return;
        }

        // No selected character
        if (_selectedCharacter == null)
            return;

        // ACTION MODES
        switch (_currentMode)
        {
            case Action.MOVE:
                MoveCharacter(slotName);
                break;

            case Action.ATTACK:
                AttackCharacter(slotName);
                break;
        }

        ClearHighlights();

        _currentMode = Action.NONE;
    }

    // pos -> nombre
    Vector2Int GetSlotPosition(string slotName)
    {
        string[] split = slotName.Split("Row");

        string rowPart = split[0];
        int column = int.Parse(split[1]) - 1;

        int row = 0;

        switch (rowPart)
        {
            case "1st":
                row = 0;
                break;

            case "2nd":
                row = 1;
                break;

            case "3rd":
                row = 2;
                break;

            case "4th":
                row = 3;
                break;

            case "5th":
                row = 4;
                break;
        }

        return new Vector2Int(column, row);
    }

    // nombre -> pos
    string GetSlotName(Vector2Int pos)
    {
        string rowName = "";

        switch (pos.y)
        {
            case 0:
                rowName = "1st";
                break;

            case 1:
                rowName = "2nd";
                break;

            case 2:
                rowName = "3rd";
                break;

            case 3:
                rowName = "4th";
                break;

            case 4:
                rowName = "5th";
                break;
        }

        return rowName + "Row" + (pos.x + 1);
    }
    string GetCharacterSlot(Character character)
    {
        foreach (var pair in _slotCharacters)
        {
            if (pair.Value == character)
                return pair.Key;
        }

        return "";
    }
    void ActivateMoveMode(ClickEvent ev)
    {
        if (_selectedCharacter == null)
            return;

        _currentMode = Action.MOVE;

        HighlightSlots(Color.aliceBlue);
    }

    void ActivateAttackMode(ClickEvent ev)
    {
        if (_selectedCharacter == null)
            return;

        _currentMode = Action.ATTACK;

        HighlightSlots(Color.softRed);
    }

    void HighlightSlots(Color color)
    {
        ClearHighlights();

        string currentSlotName =
            GetCharacterSlot(_selectedCharacter);

        if (currentSlotName == "")
            return;

        Vector2Int currentPos =
            _slotPositions[currentSlotName];

        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int targetPos =
                currentPos + dir;

            foreach (var pair in _slotPositions)
            {
                if (pair.Value == targetPos)
                {
                    VisualElement slot =
                        _gridSlots[pair.Key];

                    slot.style.backgroundColor =
                        color;

                    _highlightedSlots.Add(slot);

                    break;
                }
            }
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

            CheckGameEnd();
        }

        ShowCharacterStats(target);
    }
    void OpenHelp(ClickEvent ev)
    {
        _helpMenu.enabled = true;
        _helpMenu.PreviousScreen = this;

        this.enabled = false;
    }

    void CheckGameEnd()
    {
        bool anyPlayerAlive = false;
        bool anyEnemyAlive = false;

        foreach (var character in _slotCharacters.Values)
        {
            if (character.CharacterType == SetupScreen.CharacterType.ENEMY)
                anyEnemyAlive = true;
            else
                anyPlayerAlive = true;
        }

        if (!anyPlayerAlive)
        {
            Debug.Log("Enemies win!");
            _endScreen.enabled = true;
            _endScreen.Win = false;

            this.enabled = false;
        }
        else if (!anyEnemyAlive)
        {
            Debug.Log("Players win!");
            _endScreen.enabled = true;
            _endScreen.Win = true;

            this.enabled = false;
        }
    }

    void OnDisable()
    {
        _root.Q("GameScreen").style.display = DisplayStyle.None;

    }
}