using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrintEffectMain : MonoBehaviour
{
    public TMPro.TMP_Text tmpText = null;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            StartCoroutine(PlayPrinterEffect(tmpText, 10f));
    }



    // Update is called once per frame
    IEnumerator PlayPrinterEffect(TMPro.TMP_Text textComponent, float speed)
    {
        if (textComponent == null)
        {
            Debug.LogError("Input component is null!!!");
            yield break;
        }
        if (speed <= 0)
        {
            Debug.LogError("Input speed should be larger than 0!!!");
            speed = 1;
        }

        TMPro.TMP_TextInfo textInfo = textComponent.textInfo;

        // 开始播放打印机效果，首先清空所有文字网格
        for (int i = 0; i < textInfo.materialCount; ++i)
        {
            textInfo.meshInfo[i].Clear();
        }
        textComponent.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.Vertices);

        float timer = 0;
        float duration = 1 / speed;
        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            while (timer < duration)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            timer -= duration;

            TMPro.TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];

            int materialIndex = characterInfo.materialReferenceIndex;
            int verticeIndex = characterInfo.vertexIndex;
            if(characterInfo.elementType == TMPro.TMP_TextElementType.Sprite)
            {
                verticeIndex = characterInfo.spriteIndex;
            }
            if (characterInfo.isVisible)
            {
                textInfo.meshInfo[materialIndex].vertices[0 + verticeIndex] = characterInfo.vertex_BL.position;
                textInfo.meshInfo[materialIndex].vertices[1 + verticeIndex] = characterInfo.vertex_TL.position;
                textInfo.meshInfo[materialIndex].vertices[2 + verticeIndex] = characterInfo.vertex_TR.position;
                textInfo.meshInfo[materialIndex].vertices[3 + verticeIndex] = characterInfo.vertex_BR.position;
                textComponent.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.Vertices);
            }

        }
        textComponent.ForceMeshUpdate();
    }

}
