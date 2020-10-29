using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class collectLight : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private CustomRenderTexture captureImage = null;
    private List<Light> _lights;

    private void OnEnable()
    {
        var rt = new RenderTexture(300, 200, 24, RenderTextureFormat.ARGB32);
        rt.name = "TestRenderTarget";
    }

    void Start()
    {
        _lights = FindObjectsOfType<Light>().ToList();
        _lights.ForEach(e=>Debug.Log(e.type));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPostRender()
    {
        if (captureImage == null)
        {
            return;
        }
        Graphics.Blit(null, captureImage);
    }
}
