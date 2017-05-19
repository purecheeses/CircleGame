﻿using System;
using UnityEngine;

public class GameLogic
{
	private int[] current_sector_indexs;
	Plate _p;
	float _pointer = 0.0f;

	public int circleCount
	{
		get {
			return _p.layerNum;
		}
	}

	public int sectorCount
	{
		get {
			return _p.seperateNum;
		}
	}

	public float pointer
	{
		get {
			return _pointer;
		}

		set {
			_pointer = value;
		}
	}

	public int currentSector(int circleIndex)
	{
		return current_sector_indexs [circleIndex];
	}

	public void setCurrentSector(int circleIndex, int sectorIndex)
	{
		if (current_sector_indexs [circleIndex] != null) {
			current_sector_indexs [circleIndex] = sectorIndex;
		}
	}


	public GameLogic (Plate p)
	{
		_p = p;
		initGame ();
	}

	private void initGame()
	{
		current_sector_indexs = new int[circleCount];
	}

	private int calcPointedSectorIndex(int circleIndex)
	{
		int startIndex = circleIndex * sectorCount;
		for (int i = startIndex; i < startIndex + sectorCount; i++) {
			float base_angle = _p.getSectorRotation (i) % 360;
			float start_angle = base_angle - _p.config [i].angle / 2;
			float end_angle = base_angle + _p.config [i].angle / 2;

//			Debug.Log (string.Format("wenkan1111 startIndex {0}, base_angle {1}, start_angle {2}, end_angle {3}, pointer {4}",
//				startIndex, base_angle, start_angle, end_angle, pointer));

			if (start_angle >= 360 || end_angle >= 360) {
				start_angle -= 360;
				end_angle -= 360;
			} else if (start_angle <= -360 || end_angle <= -360) {
				start_angle += 360;
				end_angle += 360;
			}

//			Debug.Log (string.Format("wenkan2222 startIndex {0}, base_angle {1}, start_angle {2}, end_angle {3}, pointer {4}",
//				startIndex, base_angle, start_angle, end_angle, pointer));
			if (pointer > start_angle && pointer <= end_angle) {
				return i;
			}
		}
//		Debug.Log ("wenkan XXXXXXXXXXXXXXXXXXXXXXXXX");
		return -1;
	}


	public void onPlateRotate(int circleIndex)
	{
		int calcedIndex = calcPointedSectorIndex (circleIndex);
		int lastIndex = currentSector (circleIndex);

//		Debug.Log (string.Format("wenkan {0}, {1}", calcedIndex, lastIndex));
		if (calcedIndex != lastIndex) {
			setCurrentSector (circleIndex, calcedIndex);
			onEnterSector (calcedIndex);
		} else {
			onSlideThroughSector (calcedIndex);
		}
	}

	private void onEnterSector(int sectorIndex)
	{
		Debug.Log ("wenkan onEneterSector "+sectorIndex.ToString());
	}

	private void onSlideThroughSector(int sectorIndex)
	{
		Debug.Log ("wenkan onSlideThroughSector "+sectorIndex.ToString());
	}



}
