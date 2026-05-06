using System.Collections.Generic;

[System.Serializable]
public class ImageEntry
{
    public string displayName;
    public string path;
}

[System.Serializable]
public class ImageDatabase
{
    public List<ImageEntry> images;
}