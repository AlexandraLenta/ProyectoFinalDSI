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
    string _currImageName;
    DropdownField _imageDropdown;
    VisualElement _currImageElement;
    Texture2D _currImage;
    List<Character> _characterList;

    void OnEnable()
    {
        // coger todos los elementos necesarios
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        // activar el setup screen
        _root.Q("SetupScreen").style.display = DisplayStyle.Flex;

        _playerButton = _root.Q<Button>("PlayerButton");
        _enemyButton = _root.Q<Button>("EnemyButton");
        _imageDropdown = _root.Q<DropdownField>("ImageField");
        _currImageElement = _root.Q("CharacterImage");

        // registrar callbacks
        _playerButton.RegisterCallback<ClickEvent, CharacterType>(AddCharacter, CharacterType.PLAYER);
        _enemyButton.RegisterCallback<ClickEvent, CharacterType>(AddCharacter, CharacterType.ENEMY);
        _imageDropdown.RegisterCallback<ChangeEvent<string>>(ChangeImage);

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

    void OnDisable()
    {
        // desactivar setup screen
        _root.Q("SetupScreen").style.display = DisplayStyle.None;
    }
}