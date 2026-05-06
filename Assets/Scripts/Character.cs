using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Character
{
    string _name;
    int _hp;    
    int _attack;
    string _imgName;
    Texture2D _img;
    SetupScreen.CharacterType _characterType;

    public string Name
    {
        get {return _name;}
    }
    public int HP
    {
        get {return _hp;}
    }

    public int Attack
    {
        get{return _attack;}
    }

    public string ImageName
    {
        get {return _imgName;}
    }

    public SetupScreen.CharacterType CharacterType
    {
        get {return _characterType;}
    }

    public Character(string n, int hp, int atk, SetupScreen.CharacterType type, string img)
    {
        _name = n;
        _hp = hp;
        _attack = atk;
        _characterType = type;
        _imgName = img;
        _img = Resources.Load<Texture2D>(img);
    }
}
