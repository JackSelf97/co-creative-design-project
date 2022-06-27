using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour
{
    #region Sprite Variables
    public Color baseColour, offsetColour;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject highlight;
    #endregion

    public void SetColour(bool isOffset)
    {
        spriteRenderer.color = isOffset ? offsetColour : baseColour; // is it offset colour - otherwise set base colour
    }

    void OnMouseEnter() // works as 'tap'
    {
        highlight.SetActive(true);
        
    }
    void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
