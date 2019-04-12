using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSearchSystem : MonoBehaviour {
// 寻路系统，主要与Unity内置函数产生交集的位置
	enum SearchMethod
	{// 寻路方法枚举，用于有限状态机状态识别
		BreadthFirstPaths,
		DepthFirstPaths,
		Dijkstra,
		Astar,
		ThetaStar,
		Dstar,
		Genetic,
		GreedyBFS,
		DFSNguide,
	}

	[SerializeField]
	SearchMethod currentMethod;
	BFS bfsSystem;
	DFS dfsSystem;
	Dijkstra dijkstraSystem;
	Astar astarSystem;
	GreedyBFS greedyBFSSystem;
	DFSNguide dFSNguideSystem;
	void Awake()
	{
		bfsSystem = this.GetComponent<BFS>();
		dfsSystem = this.GetComponent<DFS>();
		dijkstraSystem = this.GetComponent<Dijkstra>();
		astarSystem = this.GetComponent<Astar>();
		greedyBFSSystem = this.GetComponent<GreedyBFS>();
		dFSNguideSystem = this.GetComponent<DFSNguide>();
	}
	// Use this for initialization
	void Start () {
		switch(currentMethod)
		{
			case SearchMethod.BreadthFirstPaths:
				bfsSystem.setMap();
				bfsSystem.startSearch();
				break;
			case SearchMethod.DepthFirstPaths:
				dfsSystem.setMap();
				dfsSystem.startSearch();
				break;
			case SearchMethod.Dijkstra: break;
			case SearchMethod.Astar: 
				astarSystem.setMap();
				astarSystem.startSearch();
				break;
			case SearchMethod.ThetaStar: break;
			case SearchMethod.Dstar: break;
			case SearchMethod.Genetic: break;
			case SearchMethod.GreedyBFS:
				greedyBFSSystem.setMap();
				greedyBFSSystem.startSearch();
				break;
			case SearchMethod.DFSNguide:
				dFSNguideSystem.setMap();
				dFSNguideSystem.startSearch();
				break;
		}
	}
	
/* 	// Update is called once per frame
	void Update () {
		
	} */
}
