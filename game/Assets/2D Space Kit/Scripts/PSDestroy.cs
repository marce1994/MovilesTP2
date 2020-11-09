using System.Collections;
using UnityEngine;

public class PSDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Recicle());
    }

    private void Start()
    {
        StartCoroutine(Recicle());
    }

    IEnumerator Recicle()
	{
		yield return new WaitForSeconds(1f);
		ObjectPooler.Instance.Recicle("Explosion", this.gameObject);
	}
}
