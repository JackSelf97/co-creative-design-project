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

    void OnMouseEnter() // hover over
    {
        highlight.SetActive(true); // turns highlight on
        GameManager.gMan.activeTile = gameObject; // the tile currently highlighted gets set to 'activeTile'
    }
    void OnMouseExit()
    {
        highlight.SetActive(false);
        //GameManager.gMan.activeTile = null; // the tile currently highlighted gets set to 'activeTile'
    }
}
