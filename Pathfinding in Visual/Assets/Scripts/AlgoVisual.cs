using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoVisual : MonoBehaviour {

	public const int Height = 30;
	public const int Width = 30;

	protected Map mapInstance;

	public virtual IEnumerator visualPath()
	{
		yield return null;
	}

	public virtual void startSearch()
	{

	}

	public void setMap()
	{
		mapInstance = GetComponent<Map>();
	}

	public Map GetMap()
	{
		return mapInstance;
	}
}
