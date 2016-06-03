using UnityEngine;
using System.Collections;

//Script which will set a repeatable background for the ice level

public class backgroundScroll : MonoBehaviour 
{
	void Update ()
	{
		MeshRenderer mr = GetComponent<MeshRenderer> ();

		Material mat = mr.material;

		Vector2 offset = mat.mainTextureOffset;

		offset.x += Time.deltaTime / 10f;

		mat.mainTextureOffset = offset;
	}
}
