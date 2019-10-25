using System;
using UnityEngine;

public class WarSpriteRenderer
{
    readonly GameObject prefab = null;

    public WarSpriteRenderer(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public GameObject Draw(Vector2 position, string name)
    {
        try
        {
            Diagnostics.NotNull(prefab, "prefab in Draw");

            prefab.SetActive(true);
            GameObject go = UnityEngine.Object.Instantiate(prefab);

            Diagnostics.NotNull(go, "go in Draw");

            //go.GetComponent<Sprite>().
            go.SetActive(true);

            go.transform.position = position;
            go.name = name;


            return go;
        }
        catch (Exception e)
        {
            GameController.Log("Error rendering sprite " + e.ToString());
            throw e;
        }
    }

}
