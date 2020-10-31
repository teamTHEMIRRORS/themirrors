using System;
using UnityEngine;
using UnityEngine.Rendering;

public class KillerEffectObject : MonoBehaviour
{
    private Material _killerEffectmaterial;
    [Tooltip("ダメージのエフェクトシェーダー")] public Shader _killerEffectSahder;


    private int _tempTargetId = Shader.PropertyToID("_tempTargetId");
    [Range(0, 1), Tooltip("エフェクトの量")] public float _effectVolume;

    public Boolean InnerMirror = false;

    // Start is called before the first frame update
    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += ExecutePlanarReflections;
        _killerEffectmaterial = new Material(_killerEffectSahder);
    }

    void Start()
    {
    }

    // Update is called once per frame
    /// <summary>
    /// 鏡の中に入ると世界が灰色になるよ
    /// 外に出ると色が戻る。
    /// _effectVolumeが１に近づくと灰色度があがるよ
    /// </summary>
    void Update()
    {
        _effectVolume = Mathf.Clamp(InnerMirror ? _effectVolume + 0.01f : _effectVolume - 0.01f, 0, 1);
        _killerEffectmaterial.SetFloat("_effectVolume", _effectVolume);
    }

    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= ExecutePlanarReflections;
    }

    /// <summary>
    /// ポストエフェクトをレンダリングする関数
    /// RenderPipelineManager.endCameraRendering に追加する。
    /// </summary>
    /// <param name="context"></param>
    /// <param name="camera"></param>
    public void ExecutePlanarReflections(ScriptableRenderContext context, Camera camera)
    {
        //レンダリングコード...
        //Debug.Log(camera.name);
        if (camera.name == "SceneCamera")
        {
            return;
        }

        Rendering(ref context, ref camera);
    }

    /// <summary>
    /// killerのエフェクトをレンダリングするよ
    /// </summary>
    /// <param name="context"></param>
    /// <param name="camera"></param>
    private void Rendering(ref ScriptableRenderContext context, ref Camera camera)
    {
        var cmd = CommandBufferPool.Get("RenderBlood"); // 適当なコマンドバッファをとってくる
        //cmd.Blit(Texture2D.whiteTexture, camera.activeTexture, material); // カメラにマテリアルを適用
        cmd.GetTemporaryRT(_tempTargetId, camera.scaledPixelWidth, camera.scaledPixelHeight, 0,
            FilterMode.Point); //一時テクスチャーの生成
        cmd.Blit(camera.targetTexture, _tempTargetId); // 一時テクスチャーに現在レンダリングしているカメラのテクスチャーをコピーする。
        cmd.Blit(_tempTargetId, camera.targetTexture, _killerEffectmaterial); // カメラにマテリアルを適用
        cmd.ReleaseTemporaryRT(_tempTargetId);
        context.ExecuteCommandBuffer(cmd); // コマンドバッファ実行
        context.Submit();
        CommandBufferPool.Release(cmd);
    }
}