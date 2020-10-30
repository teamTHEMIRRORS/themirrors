using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BloodEffectObject : MonoBehaviour
{
    private Material BloodEffectmaterial;
    [Tooltip("ダメージのエフェクトシェーダー")] public Shader damageEffectSahder;
    private Material ProjectionTransformEffectmaterial;
    [Tooltip("血痕をProjection空間に投影するエフェクトシェーダー")] public Shader ProjectionTransformEffectSahder;
    public List<Sprite> BloodSpr;
    [Range(0, 1), Tooltip("ダメージの量")] public float damage;
    private float inpactnum;
    [Tooltip("血痕の量")] public int bloodEffectnum = 10;

    private Stack<GameObject> BloodObjSta = new Stack<GameObject>();

    [Tooltip("PostEffectのVolumeProfileを入れる")]
    public VolumeProfile _volumeProfile;
    private Vignette _vignette;

    private int TempTargetId = Shader.PropertyToID("_tempTargetId");

    // Start is called before the first frame update
    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += ExecutePlanarReflections;
        BloodEffectmaterial = new Material(damageEffectSahder);
        ProjectionTransformEffectmaterial = new Material(ProjectionTransformEffectSahder);
    }

    void Start()
    {
        if (_volumeProfile)
        {
            _volumeProfile.TryGet<Vignette>(out _vignette);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int SprListnum = (int) (damage * (float) bloodEffectnum);
        inpactnum += Mathf.Max((SprListnum - BloodObjSta.Count) / (float) bloodEffectnum, 0f);
        //血痕をダメージ量分増やすよ
        for (int i = 0; i < SprListnum - BloodObjSta.Count; i++)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SpriteRenderer>().sprite = BloodSpr[Random.Range(0, BloodSpr.Count)];
            obj.GetComponent<Renderer>().material = ProjectionTransformEffectmaterial;
            var position = Random.insideUnitCircle.normalized;
            position *= Random.Range(6.5f, 10f);
            obj.transform.Translate(position.x, position.y, -1);
            obj.transform.Rotate(0, 0, Random.Range(0, 360));
            BloodObjSta.Push(obj);
        }

        //血痕をダメージ量分減らす
        for (int i = 0; i < BloodObjSta.Count - SprListnum; i++)
        {
            var obj = BloodObjSta.Pop();
            GameObject.Destroy(obj);
        }

        BloodEffectmaterial.SetFloat("_effectVolume", inpactnum / 4f);
        inpactnum = Mathf.Max(inpactnum - Mathf.Max(inpactnum * 0.005f, 0.0005f), 0);
        if (_vignette == null)
        {
            return;
        }
        _vignette.smoothness.value = Mathf.Cos(Time.time * 5f) / 10f + damage - 0.16f;
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
        if (camera.name == "SceneCamera")
        {
            return;
        }
        Rendering(ref context, ref camera);
    }


    private void Rendering(ref ScriptableRenderContext context, ref Camera camera)
    {
        var cmd = CommandBufferPool.Get("RenderBlood"); // 適当なコマンドバッファをとってくる
        //cmd.Blit(Texture2D.whiteTexture, camera.activeTexture, material); // カメラにマテリアルを適用
        cmd.GetTemporaryRT(TempTargetId, camera.scaledPixelWidth, camera.scaledPixelHeight, 0,
            FilterMode.Point); //一時テクスチャーの生成
        cmd.Blit(camera.targetTexture, TempTargetId); // 一時テクスチャーに現在レンダリングしているカメラのテクスチャーをコピーする。
        cmd.Blit(TempTargetId, camera.targetTexture, BloodEffectmaterial); // カメラにマテリアルを適用
        cmd.ReleaseTemporaryRT(TempTargetId);
        
        context.ExecuteCommandBuffer(cmd); // コマンドバッファ実行
        context.Submit();
        CommandBufferPool.Release(cmd);
    }
}
