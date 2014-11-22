using AssemblyCSharp;
using UnityEngine;
using System.Collections;

public class FogBehavior : MonoBehaviour 
{
    private Texture2D m_texture;

	// Use this for initialization
	void Start () 
    {
        m_texture = new Texture2D(1024, 1024, TextureFormat.ARGB32, true, true);
        for(int i = 0; i < 1024; ++i)
        {
            for(int j = 0; j < 1024; ++j)
                m_texture.SetPixel(j, i, Color.black);
        }
        m_texture.Apply();

        renderer.material.mainTexture = m_texture;
        renderer.material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Get the camera frame
        GameObject camera = GameObject.Find("Main Camera");
        if (camera == null)
        {
            Debug.LogError("Can't find game object called Main Camera");
            Debug.DebugBreak();
        }

        GameObject player = GameObjectHelper.getPlayer();

        Vector3 rayDirection = -Vector3.up;//player.transform.position - camera.transform.position;
        Ray rayCameraToPlayer = new Ray(player.transform.position + Vector3.up * 20, rayDirection.normalized);
        Debug.DrawLine(player.transform.position, player.transform.position + rayDirection.normalized*20);
        RaycastHit hit;
        int mask = 1 << LayerMask.NameToLayer("Fog");
        if (Physics.Raycast(rayCameraToPlayer, out hit, mask))
        {
            Texture2D tex = hit.collider.renderer.material.mainTexture as Texture2D;
            if (tex == null)
                return;
            int row = (int)(tex.width * hit.textureCoord.x); //row
            int column = (int)(tex.height * hit.textureCoord.y);//col

            //calc pixel in via text coords
            //int pixIn = (column * tex.width) - (tex.width - row);
            //now alpha this pixel and an area around it

            int size = 10;
            int cStart = column - size >= 0 ? column - size : 0;
            int cEnd = column + size > tex.width ? column + size : tex.width - 1;

            int rStart = row - size >= 0 ? row - size : 0;
            int rEnd = row + size > tex.height ? row + size : tex.height - 1;
            for (int c = cStart; c < cEnd; ++c)
            {
                for (int r = rStart; r < rEnd; ++r)
                    tex.SetPixel(r, c, new Color(0, 0, 0, 0));
            }

            tex.Apply();
                
        }
	}
}
