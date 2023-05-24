using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokedexInfos : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pokemonName;
    [SerializeField] Text description;
    [SerializeField] RawImage sprite;

    public void NameDefine(string _pokemonName) => this.pokemonName.text = _pokemonName;
    public void DescriptionDefine(string _description) => this.description.text = _description;
    public void SpriteDefine(Texture2D _sprite) => this.sprite.texture = _sprite;
}
