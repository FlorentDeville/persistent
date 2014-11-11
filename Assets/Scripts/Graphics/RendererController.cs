using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RendererController : MonoBehaviour 
{
    private List<Color> m_OriginalColor;
    private List<Material> m_Materials;

    private bool m_ApplyAlpha;

    private bool m_PreviousFrameApplyAlpha;

	// Use this for initialization
	void Start () 
    {
        m_OriginalColor = new List<Color>();
        m_Materials = new List<Material>();
        //foreach (Material mat in gameObject.renderer.materials)
        //    m_OriginalColor.Add(mat.color);
        StoreMaterialsReferenceAndColor(gameObject);

        m_ApplyAlpha = false;
        m_PreviousFrameApplyAlpha = false;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if (!m_ApplyAlpha && m_PreviousFrameApplyAlpha)
        {
            ResetAlphaColor();
            //for (int i = 0; i < gameObject.renderer.materials.Length; ++i)
            //{
            //    gameObject.renderer.materials[i].color = m_OriginalColor[i];
            //}
        }

        m_PreviousFrameApplyAlpha = m_ApplyAlpha;
        m_ApplyAlpha = false;
	}

    public void ApplyAlpha(float _alpha)
    {
        m_ApplyAlpha = true;
        if (m_PreviousFrameApplyAlpha)
            return;

        //for(int i = 0; i < gameObject.renderer.materials.Length; ++i)
        //{
        //    Color newColor = new Color(m_OriginalColor[i].r, m_OriginalColor[i].g, m_OriginalColor[i].b, _alpha);
        //    gameObject.renderer.materials[i].color = newColor;
        //}
        Internal_ApplyAlpha(_alpha);
    }

    private void StoreMaterialsReferenceAndColor(GameObject _obj)
    {
        if (_obj.renderer != null)
        {
            foreach (Material mat in _obj.renderer.materials)
            {
                m_Materials.Add(mat);
                m_OriginalColor.Add(mat.color);
            }
        }

        for (int i = 0; i < _obj.transform.childCount; ++i)
            StoreMaterialsReferenceAndColor(_obj.transform.GetChild(i).gameObject);
    }

    private void Internal_ApplyAlpha(float _alpha)
    {
        for (int i = 0; i < m_Materials.Count; ++i)
        {
            Color newColor = new Color(m_OriginalColor[i].r, m_OriginalColor[i].g, m_OriginalColor[i].b, _alpha);
            m_Materials[i].color = newColor;
        }
    }

    private void ResetAlphaColor()
    {
        for (int i = 0; i < m_Materials.Count; ++i)
            m_Materials[i].color = m_OriginalColor[i];
    }
}
