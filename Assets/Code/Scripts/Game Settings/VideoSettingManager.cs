using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VideoSettingManager : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevel;

    public void ChangeQualityLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevel[value];
        Debug.Log("Quality level is " + value);
    }
}
