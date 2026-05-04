using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelpMenu : MonoBehaviour
{
    VisualElement contenidoCharacter;
    VisualElement contenidoMovement;
    VisualElement contenidoAttack;

    VisualElement navCharacter;
    VisualElement navMovement;
    VisualElement navAttack;

    private void NoContenido()
    {
        contenidoCharacter.style.display = DisplayStyle.None;
        contenidoMovement.style.display = DisplayStyle.None;
        contenidoAttack.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        UIDocument uiDoc = GetComponent<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;

        VisualElement nav = root.Q<VisualElement>("Pestanias");
        VisualElement body = root.Q<VisualElement>("Contenido");

        contenidoCharacter = root.Q<VisualElement>("CharacterTab");
        contenidoMovement = root.Q<VisualElement>("MovementTab");
        contenidoAttack = root.Q<VisualElement>("AttackTab");

        navCharacter = root.Q<VisualElement>("CharacterPage");
        navMovement = root.Q<VisualElement>("MovementPage");
        navAttack = root.Q<VisualElement>("AttackPage");

        navCharacter.RegisterCallback<ClickEvent>((evt) =>
        {
            Debug.Log("CharacterPage");
            NoContenido();
            contenidoCharacter.style.display = DisplayStyle.Flex;
        });
        navMovement.RegisterCallback<ClickEvent>((evt) =>
        {
            Debug.Log("MovementPage");
            NoContenido();
            contenidoMovement.style.display = DisplayStyle.Flex;
        });
        navAttack.RegisterCallback<ClickEvent>((evt) =>
        {
            Debug.Log("AttackPage");
            NoContenido();
            contenidoAttack.style.display = DisplayStyle.Flex;
        });

    }
}
