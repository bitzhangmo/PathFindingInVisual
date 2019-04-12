using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class Map : MonoBehaviour {

	const int Height = 30;
	const int Width = 30;
	public int[,] map = new int[Height,Width];
	public Pos startPos;
	public Pos endPos;
	const int start = 7;
	const int end = 8;
	const int wall = 1;
	public GameObject[] prefabs;
	// 地图物体的父物体
	private GameObject walls;
	// Use this for initialization
	void Awake () {
		ReadMapFile();
		InitMap0();
	}
	
	void InitMap0()
	{
		walls = new GameObject();
		walls.name = "walls";
		for(int i = 0;i<Height;i++)
		{
			for(int j = 0;j<Width;j++)
			{
				switch(map[i,j])
				{
					case wall :
						var go = Instantiate(prefabs[0],new Vector3(j*1,0.5f,i*1),Quaternion.identity,walls.transform);
						break;
					case start :
						var go1 = Instantiate(prefabs[1],new Vector3(j*1,0.5f,i*1),Quaternion.identity,walls.transform);
						startPos = new Pos(j,i);
						break;
					case end :
						var go2 = Instantiate(prefabs[2],new Vector3(j*1,0.5f,i*1),Quaternion.identity,walls.transform);
						endPos = new Pos(j,i);
						break;
				}
				/* Debug.Log(map[i,j]); */
			}
		}
	}

	public void ReadMapFile()
	{
		string path = Application.dataPath + "//" + "map.txt";
		if(!File.Exists(path))
		{
			Debug.Log("Read the map failed");
			return;
		}
		FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read);
		StreamReader read = new StreamReader(fs,Encoding.Default);

		string strReadLine = "";
		int y = 0;
		read.ReadLine();
		strReadLine = read.ReadLine();

		while(strReadLine!=null && y<Height)
		{
			for(int x = 0;x<Width&&x<strReadLine.Length;++x)
			{
				int t;
				switch(strReadLine[x])
				{
					case '1' :
						t = 1;
						break;
					case '7' :
						t = 7;
						break;
					case '8' :
						t = 8;
						break;
					default :
						t = 0;
						break;
				}
				map[y,x] = t;
			}
			y+=1;
			strReadLine = read.ReadLine();
		}
		read.Dispose();
		fs.Close();
		Debug.Log("Has read the map file!");
	}

	public GameObject GetWalls()
	{
		return walls;
	}
}
