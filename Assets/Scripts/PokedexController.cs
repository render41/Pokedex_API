using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PokedexController : MonoBehaviour
{
    private const string API = "https://pokeapi.co/api/v2/";
    private const string POKEMON_NAME = "pokemon/";
    private const string POKEMON_DESC = "pokemon-species/";
    private const string POKEMON_SPRITE = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/";
    private string id = "1";
    private UnityWebRequest request;
    private string jsonResponse;
    private bool isGetPokemon;

    [SerializeField] PokemonID pokemonID;
    [SerializeField] PokedexDescriptionRoot pokedexDescriptionRoot;
    [SerializeField] Texture2D sprite;
    [SerializeField] PokedexInfos pokedexInfos;
    [SerializeField] TMP_InputField inputField;

    private void Start() => StartCoroutine(GetDatas());

    private IEnumerator GetDatas()
    {
        isGetPokemon = false;
        yield return GetNameAndID();
        if (jsonResponse == "Not Found")
        {
            isGetPokemon = true;
            yield break;
        }
        else pokemonID = JsonUtility.FromJson<PokemonID>(jsonResponse);
        yield return GetDescription();
        yield return GetImage();


        InsertDatas();
        isGetPokemon = true;
    }

    private IEnumerator GetNameAndID()
    {
        request = UnityWebRequest.Get($"{API}{POKEMON_NAME}{id}");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            jsonResponse = request.downloadHandler.text;
        }
        else
        {
            Debug.LogError($"Failed to retrieve Pokemon ID. Error: {request.error}");
        }
    }

    private IEnumerator GetDescription()
    {
        request = UnityWebRequest.Get($"{API}{POKEMON_DESC}{id}");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            jsonResponse = request.downloadHandler.text;
            pokedexDescriptionRoot = JsonUtility.FromJson<PokedexDescriptionRoot>(jsonResponse);
        }
        else
        {
            Debug.LogError($"Failed to retrieve Pokemon Description. Error: {request.error}");
        }
    }

    private IEnumerator GetImage()
    {
        request = UnityWebRequestTexture.GetTexture($"{POKEMON_SPRITE}{id}.png");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            sprite = DownloadHandlerTexture.GetContent(request);
            sprite.filterMode = FilterMode.Point;
        }
        else
        {
            Debug.LogError($"Failed to retrieve Pokemon Image. Error: {request.error}");
        }
    }

    private void InsertDatas()
    {
        pokedexInfos.NameDefine(string.Format("0{0} - {1}", pokemonID.id, pokemonID.name.ToUpper()));
        pokedexInfos.DescriptionDefine(GetDescriptionEnglish());
        pokedexInfos.SpriteDefine(sprite);
    }

    private string GetDescriptionEnglish()
    {
        for (int i = 0; i <= pokedexDescriptionRoot.flavor_text_entries.Length; i++)
        {
            if (pokedexDescriptionRoot.flavor_text_entries[i].language.name == "en")
            {
                return pokedexDescriptionRoot.flavor_text_entries[i].flavor_text.Replace("\n", " ");
            }
        }
        return null;
    }

    #region Button Search Pokemon
    public void SearchPokemonButton()
    {
        if (isGetPokemon)
        {
            id = inputField.text;
            inputField.text = "";
            StartCoroutine(GetDatas());
        }
    }
    #endregion
}
