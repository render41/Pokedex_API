[System.Serializable]
public class PokedexDescriptionLanguage
{
    public string name;
}

[System.Serializable]
public class PokedexDescription
{
    public string flavor_text;
    public PokedexDescriptionLanguage language;
}

[System.Serializable]
public class PokedexDescriptionRoot
{
    public PokedexDescription[] flavor_text_entries;
}