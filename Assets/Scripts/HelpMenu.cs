using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelpMenu : MonoBehaviour
{
    VisualElement contenidoCharacter;
    VisualElement contenidoMovement;
    VisualElement contenidoAttack;

    VisualElement pestaniaCharacter;
    VisualElement pestaniaMovement;
    VisualElement pestaniaAttack;

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

        VisualElement pestanias = root.Q<VisualElement>("Pestanias");
        VisualElement contenido = root.Q<VisualElement>("Contenido");

        pestaniaCharacter = root.Q<Button>("CharacterTab");
        pestaniaMovement = root.Q<Button>("MovementTab");
        pestaniaAttack = root.Q<Button>("AttackTab");

        contenidoCharacter = root.Q<VisualElement>("CharacterPage");
        contenidoMovement = root.Q<VisualElement>("MovementPage");
        contenidoAttack = root.Q<VisualElement>("AttackPage");

        pestaniaCharacter.RegisterCallback<ClickEvent>((evt) =>
        {
            Debug.Log("Character");
            NoContenido();
            contenidoCharacter.style.display = DisplayStyle.Flex;
        });
        pestaniaMovement.RegisterCallback<ClickEvent>((evt) =>
        {
            Debug.Log("Movement");
            NoContenido();
            contenidoMovement.style.display = DisplayStyle.Flex;
        });
        pestaniaAttack.RegisterCallback<ClickEvent>((evt) =>
        {
            Debug.Log("Attack");
            NoContenido();
            contenidoAttack.style.display = DisplayStyle.Flex;
        });

    }
}
