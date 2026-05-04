using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelperIndividual
{
    public static List<Individual> FromJson<Individual>(string json)
    {
        ListIndividual<Individual> listIndividual = JsonUtility.FromJson<ListIndividual<Individual>>(json);
        return listIndividual.Individuals;
    }

    public static string ToJson<Individual>(List<Individual> list)
    {
        ListIndividual<Individual> listIndividual = new ListIndividual<Individual>();
        listIndividual.Individuals = list;

        return JsonUtility.ToJson(listIndividual);
    }

    public static string ToJson<Individual>(List<Individual> list, bool prettyPrint)
    {
        ListIndividual<Individual> listIndividual = new ListIndividual<Individual>();
        listIndividual.Individuals = list;

        return JsonUtility.ToJson(listIndividual, prettyPrint);
    }

    [Serializable]
    private class ListIndividual<Individual>
    {
        public List<Individual> Individuals;
    }
}