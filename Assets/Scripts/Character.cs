using UnityEngine;
using UnityEngine.UIElements;

public class Character
{
    string _name;
    int _hp;    
    int _attack;
    Texture2D _img;
    SetupScreen.CharacterType _characterType;

    public Character(string n, int hp, int atk, SetupScreen.CharacterType type, Texture2D img)
    {
        _name = n;
        _hp = hp;
        _attack = atk;
        _characterType = type;
        _img = img;
    }
}
