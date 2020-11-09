using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private Dictionary<int, List<GameObject>> parallaxLayers;

    void Start()
    {
        parallaxLayers = new Dictionary<int, List<GameObject>>();
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        var sprites = FindObjectsOfType<SpriteRenderer>();
        parallaxLayers.Add(0, sprites.Where(x => x.sortingOrder == -1).Select(x => x.gameObject).ToList());
        parallaxLayers.Add(1, sprites.Where(x => x.sortingOrder == -2).Select(x => x.gameObject).ToList());
        parallaxLayers.Add(2, sprites.Where(x => x.sortingOrder == -3).Select(x => x.gameObject).ToList());
        parallaxLayers.Add(3, sprites.Where(x => x.sortingOrder == -4).Select(x => x.gameObject).ToList());
        parallaxLayers.Add(4, sprites.Where(x => x.sortingOrder == -5).Select(x => x.gameObject).ToList());
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement;
        lastCameraPosition = cameraTransform.position;

        for (int i = 0; i < parallaxLayers.Count; i++)
        {
            foreach (var item in parallaxLayers[i])
            {
                item.transform.position += deltaMovement * 0.1f * i;
            }
        }
    }
}
