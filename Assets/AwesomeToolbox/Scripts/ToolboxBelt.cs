using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(AnimatedHorizontalOrVerticalLayoutGroup))]
public class ToolboxBelt : ToolboxItem {
	public bool IsExpanded = true;

	public float AnimationDuration = 0.4f;
	public AnimationCurve animationCurve = AnimationCurve.EaseInOut (0f, 0f, 1f, 1f);

	private bool isExpanding = false;
	private bool isShrinking = false;
	private float animationStartTime = 0f;

	public ToolboxItem[] itemsCahche;
	AnimatedHorizontalOrVerticalLayoutGroup layout;
	protected override void Start ()
	{
		base.Start ();
		layout = GetComponent<AnimatedHorizontalLayoutGroup> ();
		if (layout == null) {
			layout = GetComponent<AnimatedVerticalLayoutGroup> ();
		}
		itemsCahche =  GetComponentsInChildrenFirstLevel<ToolboxItem> ();
	}
	 

	void Update() {
		if (isExpanding) {
			if (Time.time - animationStartTime > AnimationDuration) {
				isExpanding = false;
				Animate (1);
				StartCoroutine (RebuildLayoutInNextFrame ());
			} else {
				float animationProgress = Mathf.Clamp01((Time.time - animationStartTime) / AnimationDuration);
				animationProgress = animationCurve.Evaluate (animationProgress);
				Animate (animationProgress);
			}

		} else if (isShrinking) {
			if (Time.time - animationStartTime > AnimationDuration) {
				isShrinking = false;
				Animate (0);
				StartCoroutine (RebuildLayoutInNextFrame ());
			} else {
				float animationProgress = Mathf.Clamp01((Time.time - animationStartTime) / AnimationDuration);
				animationProgress = 1f - animationProgress;
				animationProgress = animationCurve.Evaluate (animationProgress);
				Animate (animationProgress);
			}
		}
	}
		
	public override bool Animate (float Progress, bool forceHide = false)
	{
		for (int i = 0; i < itemsCahche.Length; i++) {
			if(itemsCahche [i].GetType() == typeof(ToolboxBelt)) {
				ToolboxBelt item = (ToolboxBelt)itemsCahche [i];
				if (item.IsExpanded != IsExpanded) {
					item.Toggle ();
				}
			} else {
				itemsCahche [i].Animate (Progress, true);
			}
		}
		return layout.childCount > 0;
	}

	public void Toggle(bool recurrsive = false) {
		if(IsExpanded) {
			Shrink (recurrsive);
		} else {
			Expand (recurrsive);
		}
	}

	public void Expand(bool recurrsive = false) {
		if (IsExpanded)
			return;
		itemsCahche =  GetComponentsInChildrenFirstLevel<ToolboxItem> ();
		IsExpanded = true;
		isExpanding = true;
		isShrinking = false;
		animationStartTime = Time.time;
		if(recurrsive) {
			for (int i = 0; i < itemsCahche.Length; i++) {
				if(itemsCahche [i].GetType() == typeof(ToolboxBelt)) {
					ToolboxBelt item = (ToolboxBelt)itemsCahche [i];
					item.Expand (recurrsive);
				} 
			}
		}
	}

	public void Shrink(bool recurrsive = false) {
		if (!IsExpanded)
			return;
		itemsCahche =  GetComponentsInChildrenFirstLevel<ToolboxItem> ();
		IsExpanded = false;
		isExpanding = false;
		isShrinking = true;
		animationStartTime = Time.time;
		if(recurrsive) {
			for (int i = 0; i < itemsCahche.Length; i++) {
				if(itemsCahche [i].GetType() == typeof(ToolboxBelt)) {
					ToolboxBelt item = (ToolboxBelt)itemsCahche [i];
					item.Shrink (recurrsive);
				} 
			}
		}
	}

	public void OnItemClick(ToolboxButton item) {
		if (item.IsToggle) {
			item.IsToggled = !item.IsToggled;
		} else {
			itemsCahche =  GetComponentsInChildrenFirstLevel<ToolboxButton> ();
			for (int i = 0; i < itemsCahche.Length; i++) {
				if(itemsCahche [i].GetType() == typeof(ToolboxButton)) { 
					((ToolboxButton)itemsCahche [i]).IsSelected = false;
				}
			}
			item.IsSelected = true;
		}
	}

	public T[] GetComponentsInChildrenFirstLevel<T> ()
	{
		//return GetComponentsInChildren<T> ();
		List<T> res = new List<T> ();
		T[] childs =  GetComponentsInChildren<T> (true);
		for (int i = 0; i < childs.Length; i++) {
			if(((MonoBehaviour)(object)childs[i]).transform.parent == transform) {
				res.Add (childs [i]);
			}
		}
		return res.ToArray();
	}

}
