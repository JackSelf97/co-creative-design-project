using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour
{
    #region Sprite Variables
    public Color baseColour, offsetColour;
    public SpriteRenderer spriteRenderer;
    public bool isOccupied = false;
    [SerializeField] private GameObject highlight;
    public bool shrink;
    public float xScale, yScale;
    #endregion

    public void Start()
    {
        xScale = 1f; // size of the objects
        yScale = 1f;
    }

    public void Update()
    {
        if (shrink)
        {
            xScale -= Time.deltaTime;
            yScale -= Time.deltaTime;
            transform.localScale = new Vector3(xScale, yScale);

            if (yScale <= 0f)
            {
                xScale = 0f;
                yScale = 0f;

                shrink = false;
            }
        }
    }

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
