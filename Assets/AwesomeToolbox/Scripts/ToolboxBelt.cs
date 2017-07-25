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

	private int animationDirection = 0;
	private float animationProgress = 1f;

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
		if (animationDirection != 0) {
			animationProgress += Time.deltaTime / AnimationDuration * animationDirection;
			float progress = Mathf.Clamp01(animationProgress);
			progress = animationCurve.Evaluate (progress);
			Animate (progress);

			if ((animationProgress > 1f && animationDirection == 1) || (animationProgress < 0f  && animationDirection == -1)) {
				animationDirection = 0;
				StartCoroutine (RebuildLayoutInNextFrame ());
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
		animationDirection = 1;
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
		animationDirection = -1;
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

	public T[] GetComponentsInChildrenFirstLevel<T> (bool includeInactive = true)
	{
		List<T> res = new List<T> ();
		T[] childs =  GetComponentsInChildren<T> (includeInactive);
		for (int i = 0; i < childs.Length; i++) {
			if(((Component)(object)childs[i]).transform.parent == transform) {
				res.Add (childs [i]);
			}
		}
		return res.ToArray();
	}

}
