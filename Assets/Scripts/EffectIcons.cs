using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectIcons : MonoBehaviour
{
    public Sprite NullSprite;
    public Sprite BackSprite;
    public Sprite StopSprite;
    public List<Sprite> Sprites;
    // Start is called before the first frame update
    void Start()
    {
        Sprites = new List<Sprite> { NullSprite, BackSprite,StopSprite };
    }
    public Sprite GetSpriteById(int id)
    {
        return Sprites[id];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
