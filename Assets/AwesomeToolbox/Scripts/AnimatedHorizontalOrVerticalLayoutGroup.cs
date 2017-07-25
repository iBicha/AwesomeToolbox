using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AnimatedHorizontalOrVerticalLayoutGroup : HorizontalOrVerticalLayoutGroup {
	public bool ResizeParentToFit = true;
	public int childCount {
		get {
			int count = 0;
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).gameObject.activeSelf) {
					count++;
				}
			}
			return count;
		}
	}

}
