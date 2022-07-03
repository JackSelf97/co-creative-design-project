using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour
{
    #region Sprite Variables
    public Color baseColour, offsetColour;
    public SpriteRenderer spriteRenderer;

    public GameObject slot = null;
    [SerializeField] private GameObject highlight;
    private const int unitTypeLayer = 8;
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (slot != null)
        {
            return;
        }

        if (other.gameObject.layer == unitTypeLayer)
        {
            other.gameObject.transform.position = GameManager.gMan.activeTile.transform.position - new Vector3(0, 0, 1); // unit collider infront of tile collider
            GameManager.gMan.activeTile.GetComponent<Tile_Script>().slot = other.gameObject;
            GameManager.gMan.interactableSlot = null;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (slot != null)
        {
            return;
        }
        if (other.gameObject.layer == unitTypeLayer)
        {
            slot = null;
        }
    }
}
