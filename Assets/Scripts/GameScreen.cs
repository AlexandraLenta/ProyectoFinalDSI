using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    EndScreen _endScreen;

    VisualElement _root;

    Button _helpButton;

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _root.Q("GameScreen").style.display = DisplayStyle.Flex;
    }


    void OnDisable()
    {
        _root.Q("GameScreen").style.display = DisplayStyle.None;

    }
}