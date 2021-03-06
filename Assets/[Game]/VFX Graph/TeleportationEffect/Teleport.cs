using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    VisualEffect start, p, finish;

    [SerializeField]
    Transform[] points;

    int pointIndex = 0;
    bool isMoving = false;

    [SerializeField]
    SkinnedMeshRenderer[] renderer;

    void Start()
    {
        if (points.Length == 0)
        {
            Debug.Log("Boþ");
            return;
        }
        start.Stop();
        p.Stop();
        finish.Stop();
        transform.position = points[0].position;
    }

    void Update()
    {
        if(!isMoving && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        start.Stop();
        p.Stop();
        finish.Stop();

        isMoving = true;
        int lastPoint = pointIndex;
        pointIndex = (pointIndex + 1) % points.Length;
        float dissolveTime = 0;

        //
        start.Play();
        while (dissolveTime <= 1f)
        {
            for(int i = 0; i < renderer.Length; i++)
            {
                renderer[i].sharedMaterial.SetFloat("DissolveTime", dissolveTime);
            }
            start.SetFloat("DissolveTime", dissolveTime);
            dissolveTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        start.Stop();
        //

        //
        float path = 0;
        p.Play();
        while (path <= 1f)
        {
            transform.position = Vector3.Lerp(points[lastPoint].position, points[pointIndex].position, path);
            path += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        p.Stop();
        //

        //
        finish.Play();
        while (dissolveTime >= 0f)
        {
            for(int i = 0; i < renderer.Length; i++)
            {
                renderer[i].sharedMaterial.SetFloat("DissolveTime", dissolveTime);
            }
            finish.SetFloat("DissolveTime", dissolveTime);
            dissolveTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        finish.Stop();
        isMoving = false;
        //
    }
}
