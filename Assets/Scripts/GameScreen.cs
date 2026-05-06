using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    EndScreen _endScreen;

    VisualElement _root;

    Button _helpButton;
    Button _moveButton;
    Button _attackButton;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _root.Q("GameScreen").style.display = DisplayStyle.Flex;

        _moveButton = _root.Q<Button>("MoveButton");
        _attackButton = _root.Q<Button>("AttackButton");
    }


    void OnDisable()
    {
        _root.Q("GameScreen").style.display = DisplayStyle.None;

    }
}