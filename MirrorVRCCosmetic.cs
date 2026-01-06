using UnityEngine;
using Mirror.VR;
using System.Collections;

public enum CosmeticTypeEnum
{
    Head,
    Face,
    Body,
    Left,
    Right
}

public class MirrorVRCCosmetic : MonoBehaviour
{
    [Header("Script by GGameralt")]
    [Header("Cosmetic Settings")]
    public CosmeticTypeEnum CosmeticType = CosmeticTypeEnum.Head;
    public string CosmeticName = "";
    public bool RemoveCosmetic = false;

    [Header("Hand Tag")]
    public string HandTag = "HandTag";

    [Header("EOS Safety Settings")]
    public float SaveDelay = 0.5f;

    private bool isProcessing = false;
    private Coroutine saveCoroutine;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag))
        {
            SetCosmeticSafe();
        }
    }

    public void SetCosmeticSafe()
    {
        if (isProcessing)
        {
            return;
        }

        if (saveCoroutine != null)
        {
            StopCoroutine(saveCoroutine);
        }

        saveCoroutine = StartCoroutine(SetCosmeticWithDelay());
    }

    private IEnumerator SetCosmeticWithDelay()
    {
        isProcessing = true;

        yield return new WaitForSeconds(SaveDelay);

        try
        {
            if (RemoveCosmetic)
            {
                MirrorVRManager.SetCosmetic(CosmeticType.ToString(), "");
            }
            else
            {
                if (!string.IsNullOrEmpty(CosmeticName))
                {
                    MirrorVRManager.SetCosmetic(CosmeticType.ToString(), CosmeticName);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error setting cosmetic: {e.Message}");
        }

        yield return new WaitForSeconds(SaveDelay);
        isProcessing = false;
    }

    public void SetCosmeticImmediate()
    {
        if (RemoveCosmetic)
        {
            MirrorVRManager.SetCosmetic(CosmeticType.ToString(), "");
        }
        else
        {
            if (!string.IsNullOrEmpty(CosmeticName))
            {
                MirrorVRManager.SetCosmetic(CosmeticType.ToString(), CosmeticName);
            }
        }
    }

    private void OnDestroy()
    {
        if (saveCoroutine != null)
        {
            StopCoroutine(saveCoroutine);
        }
    }
}
