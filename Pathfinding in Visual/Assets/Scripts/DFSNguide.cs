﻿// Copyright (C) 2018 Memo游戏工作室 版权所有
// 创建标识： #DEVELOPER_NAME# #CREATE_DATE#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class DFSNguide : AlgoVisual {

	//const int Height = 30;
	//const int Width = 30;
	//bfs表
	int[,] bfs = new int[Height,Width];
	// 地图实例
	//public Map map;
	// 任务队列
	Stack<Pos> stack = new Stack<Pos>();
	public int curdepth;

	string allbfs; 


	IEnumerator InitBFS()
	{
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				//bfs[i,j] = short.MaxValue;
				bfs[i,j] = short.MaxValue;
			}
		}
		// 初始点为0
		bfs[mapInstance.startPos.y,mapInstance.startPos.x] = 0;
		//quene.Enqueue(mapInstance.startPos);
		stack.Push(mapInstance.startPos);
		while(stack.Count>0)
		{	
			//Debug.Log("进入循环体成功");
			Pos cur = stack.Pop();
			//Debug.Log("queneCount:"+quene.Count.ToString());
			//if(cur.Equals(mapInstance.endPos)) break;
			if(cur.y>0)
			{
				if(checkPos(cur,0,-1)) { break; }
			}
			if(cur.y<Height-1)
			{
				if(checkPos(cur,0,1)) { break; }
			}
			if(cur.x > 0)
			{
				if(checkPos(cur,-1,0)) { break; }
			}
			if(cur.x < Width - 1)
			{
				if(checkPos(cur,1,0)) { break; }
			}
			if(bfs[cur.y,cur.x]>curdepth)
			{
				curdepth = bfs[cur.y,cur.x];
				// refresh
				Refresh();
				yield return new WaitForSeconds(0.05f);
			}

			yield return null;
		}

		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				allbfs+=bfs[i,j].ToString()+' ';
			}
			allbfs+='\n';
		} 
		// Refresh();
		System.IO.File.WriteAllText(Application.dataPath + "//" + "dfs1.txt", allbfs, Encoding.UTF8);
		StartCoroutine(visualPath());
	}

	bool checkPos(Pos cur,int dx,int dy)
	{// 判断是否寻路完成同时对bfs表进行更新）
		if(mapInstance.map[cur.y + dy,cur.x + dx] == 8 )
		{
			bfs[cur.y + dy,cur.x + dx] = (short)(bfs[cur.y,cur.x] + 1);
			Debug.Log("寻路完成。");
			return true;
		}
		if(mapInstance.map[cur.y + dy,cur.x + dx] == 0)
		{
			if(bfs[cur.y + dy,cur.x + dx]>(short)(bfs[cur.y,cur.x] + 1))
			{
				bfs[cur.y + dy,cur.x + dx] = (short)(bfs[cur.y,cur.x] + 1);
				//quene.Enqueue(new Pos(cur.x + dx,cur.y + dy));
				stack.Push(new Pos(cur.x + dx,cur.y + dy));
			}

		}
		return false;
	}

	void Refresh()
	{
		GameObject[] all_go = GameObject.FindGameObjectsWithTag("Path");
		foreach (var go in all_go)
		{
			Destroy(go);
		}
		for(int i = 0;i<Height;i++)
		{
			for(int j = 0;j<Width;j++)
			{
				if (mapInstance.map[i,j]==0 && bfs[i, j] != short.MaxValue)
                {
                    var go = Instantiate(mapInstance.prefabs[3], new Vector3(j * 1, 0.1f, i * 1), Quaternion.identity,mapInstance.GetWalls().transform);
                    go.tag = "Path";
                }

			}
		}
	}

	public override IEnumerator visualPath()
	{
		Pos p = mapInstance.endPos;

		while(true)
		{
			if(mapInstance.endPos == null)
			{
				Debug.Log("无对应路径存在！");
				break;
			}
			int cur_step = bfs[p.y,p.x];
			if(cur_step == 0)
				break;
			if (p.y>0 && bfs[p.y-1, p.x] == cur_step - 1)
            {
                p.y -= 1;
            }
            else if (p.y<bfs.GetLength(0) && bfs[p.y+1, p.x] == cur_step - 1)
            {
                p.y += 1;
            }
            else if (p.x>0 && bfs[p.y, p.x-1] == cur_step - 1)
            {
                p.x -= 1;
            }
            else if (p.x<Width && bfs[p.y, p.x+1] == cur_step - 1)
            {
                p.x += 1;
            }
			if (!p.Equals(mapInstance.startPos))
            {
                var go = Instantiate(mapInstance.prefabs[4], new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, mapInstance.GetWalls().transform);
                yield return new WaitForSeconds(0.1f);
            }
		}
		yield return null;
	}

	public override void startSearch()
	{
		StartCoroutine(InitBFS());
	}
}

