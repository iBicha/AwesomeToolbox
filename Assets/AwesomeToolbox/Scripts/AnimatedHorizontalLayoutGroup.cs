using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedHorizontalLayoutGroup : AnimatedHorizontalOrVerticalLayoutGroup {
	//
	// Constructors
	//
	protected AnimatedHorizontalLayoutGroup ()
	{
		
	}

	//
	// Methods
	//
	public override void CalculateLayoutInputHorizontal ()
	{
		base.CalculateLayoutInputHorizontal ();
		base.CalcAlongAxis (0, false);
	}

	public override void CalculateLayoutInputVertical ()
	{
		base.CalcAlongAxis (1, false);
	}

	public override void SetLayoutHorizontal ()
	{
		base.SetChildrenAlongAxis (0, false);
		if (ResizeParentToFit) {
			GetComponent<RectTransform> ().sizeDelta = childCount > 0 ? new Vector2 (preferredWidth, preferredHeight) : Vector2.zero;
		}
	}

	public override void SetLayoutVertical ()
	{
		base.SetChildrenAlongAxis (1, false);
		if (ResizeParentToFit) {
			GetComponent<RectTransform> ().sizeDelta = childCount > 0 ? new Vector2 (preferredWidth, preferredHeight) : Vector2.zero;
		}
	}

}
