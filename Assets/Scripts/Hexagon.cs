﻿using UnityEngine;
using System.Collections;

public class Hexagon : MonoBehaviour {
    public Vector2 fixedPosition { get; set; }
    private Color defaultColor;

	// Use this for initialization
	void Start () {
        GetDefaultMaterial();
	}
	   
    private void GetDefaultMaterial()
    {
        defaultColor = GetSpriteRenderer().color;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameCordinator gameCordinator = GameObject.Find("GameCordinator").GetComponent<GameCordinator>();
        gameCordinator.receivePlayerHexagonClick(this);
    }

    public void SetColor(Color color)
    {
        GetSpriteRenderer().color = color;
    }

    public void RestoreDefaultColor()
    {
        GetSpriteRenderer().color = defaultColor;
    }

    private SpriteRenderer GetSpriteRenderer()
    {
        return gameObject.GetComponent<SpriteRenderer>();
    }
}
