using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedVerticalLayoutGroup : AnimatedHorizontalOrVerticalLayoutGroup {
	//
	// Constructors
	//
	protected AnimatedVerticalLayoutGroup ()
	{
		
	}

	//
	// Methods
	//
	public override void CalculateLayoutInputHorizontal ()
	{
		base.CalculateLayoutInputHorizontal ();
		base.CalcAlongAxis (0, true);
	}

	public override void CalculateLayoutInputVertical ()
	{
		base.CalcAlongAxis (1, true);
	}

	public override void SetLayoutHorizontal ()
	{
		base.SetChildrenAlongAxis (0, true);
		if (ResizeParentToFit) {
			GetComponent<RectTransform> ().sizeDelta = childCount > 0 ? new Vector2 (preferredWidth, preferredHeight) : Vector2.zero;
		}
	}

	public override void SetLayoutVertical ()
	{
		base.SetChildrenAlongAxis (1, true);
		if (ResizeParentToFit) {
			GetComponent<RectTransform> ().sizeDelta = childCount > 0 ? new Vector2 (preferredWidth, preferredHeight) : Vector2.zero;
		}
	}

}
