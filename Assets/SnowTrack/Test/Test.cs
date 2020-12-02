using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    private Camera _camera;
    private RenderTexture _rt;
    

    private void OnEnable()
    {        
        _camera = GetComponent<Camera>();
        var useRT = _camera.targetTexture;

        _rt = RenderTexture.GetTemporary(useRT.width, useRT.height, useRT.depth, useRT.format, RenderTextureReadWrite.Default, useRT.antiAliasing);
    }

    private void OnDisable()
    {
        RenderTexture.ReleaseTemporary(_rt);
    }

    private void LateUpdate()
    {
        Shader.SetGlobalVector("_CameraWorldPos", _camera.transform.position);

        var height = _camera.orthographicSize * 2;
        var width = height * _camera.aspect;
        
        Shader.SetGlobalFloat("_CameraWidth", width);
        Shader.SetGlobalFloat("_CameraHeight", height);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        
    }
}
