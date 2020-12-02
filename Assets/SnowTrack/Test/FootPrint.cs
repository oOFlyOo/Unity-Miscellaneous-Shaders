using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FootPrint : MonoBehaviour
{
    [SerializeField] private float _footPrintSize = 1;
    [SerializeField] private int _worldSize = 20;
    [SerializeField] private int _textureSize = 256;

    [SerializeField] private Material _footPrintMaterial;
    [SerializeField] private float _atten = 0.1f;
    [SerializeField] private int _duration = 3;
    private int _curDuration = 0;

    [SerializeField] private RenderTexture _srcRenderTexture;
    [SerializeField] private RenderTexture _dstRenderTexture;

    private Vector3 _lastWorldPosition;

    private void OnEnable()
    {
        _srcRenderTexture = RenderTexture.GetTemporary(_textureSize, _textureSize, 0, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear, 1);
        _srcRenderTexture.wrapMode = TextureWrapMode.Clamp;
        _srcRenderTexture.filterMode = FilterMode.Point;
        _srcRenderTexture.Release();
        
        _dstRenderTexture = RenderTexture.GetTemporary(_textureSize, _textureSize, 0, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear, 1);
        _dstRenderTexture.wrapMode = TextureWrapMode.Clamp;
        _srcRenderTexture.filterMode = FilterMode.Point;

        _lastWorldPosition = transform.position;
    }

    private void OnDisable()
    {
        RenderTexture.ReleaseTemporary(_srcRenderTexture);
        _srcRenderTexture = null;
        
        RenderTexture.ReleaseTemporary(_dstRenderTexture);
        _dstRenderTexture = null;
    }

    private void OnWillRenderObject()
    {
        if (_footPrintMaterial == null)
        {
            return;
        }

        _curDuration++;
        if (_curDuration % _duration != 0)
        {
            return;
        }

        Shader.SetGlobalFloat("_FootPrintSize", _footPrintSize);
        Shader.SetGlobalInt("_WorldSize", _worldSize);
        Shader.SetGlobalFloat("_Atten", _atten);
        Shader.SetGlobalVector("_WorldPosition", transform.position);
        Shader.SetGlobalVector("_LastWorldPosition", _lastWorldPosition);
        Shader.SetGlobalTexture("_FootPrintTex", _dstRenderTexture);

        // Debug.Log(_lastWorldPosition - transform.position);

        // _footPrintMaterial.mainTexture = _renderTexture;

        Graphics.Blit(_srcRenderTexture, _dstRenderTexture, _footPrintMaterial);
        Graphics.Blit(_dstRenderTexture, _srcRenderTexture);
        
        Shader.SetGlobalTexture("_SnowTrackTex", _dstRenderTexture);

        _lastWorldPosition = transform.position;
    }
}