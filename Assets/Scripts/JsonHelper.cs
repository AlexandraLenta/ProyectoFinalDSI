using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelperCharacter
{
    public static List<Character> FromJson<Character>(string json)
    {
        ListCharacter<Character> listCharacter = JsonUtility.FromJson<ListCharacter<Character>>(json);
        return listCharacter.Characters;
    }

    public static string ToJson<Character>(List<Character> list)
    {
        ListCharacter<Character> listCharacter = new ListCharacter<Character>();
        listCharacter.Characters = list;

        return JsonUtility.ToJson(listCharacter);
    }

    public static string ToJson<Character>(List<Character> list, bool prettyPrint)
    {
        ListCharacter<Character> listCharacter = new ListCharacter<Character>();
        listCharacter.Characters = list;

        return JsonUtility.ToJson(listCharacter, prettyPrint);
    }

    [Serializable]
    private class ListCharacter<Character>
    {
        public List<Character> Characters;
    }
}