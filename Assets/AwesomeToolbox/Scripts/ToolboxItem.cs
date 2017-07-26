using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolboxItem : MonoBehaviour {

	public bool AlwaysVisibleInParent = false;
	public bool AlwaysVisible = false;
	protected ToolboxBelt parentToolbox;

	[System.NonSerialized]
	public RectTransform rectTransform;
	[System.NonSerialized]
	public Vector2 startSize;
	protected virtual void Start () {
		rectTransform = GetComponent<RectTransform> ();
		startSize = rectTransform.sizeDelta;
		parentToolbox = transform.parent.GetComponent<ToolboxBelt> ();
	}

	public virtual bool Animate(float Progress, bool forceHide = false) {
		rectTransform.sizeDelta = startSize * Progress;
		if ((!AlwaysVisibleInParent || forceHide) && !AlwaysVisible) {
			rectTransform.sizeDelta = startSize * Progress;
			if (Progress == 0 && gameObject.activeSelf) {
				gameObject.SetActive (false);
			} else if(Progress > 0 && !gameObject.activeSelf) {
				gameObject.SetActive (true);
			}
		} else {
			rectTransform.sizeDelta = startSize * Mathf.Max(1f, Progress);
		}
		return gameObject.activeSelf;
	}

	public IEnumerator RebuildLayoutInNextFrame() {
		yield return new WaitForEndOfFrame ();
		ToolboxBelt toolbox = null;
		if (parentToolbox != null) {
			toolbox = parentToolbox;
		} else if (GetType () == typeof(ToolboxBelt)) {
			toolbox = (ToolboxBelt)this;
		} else {
			yield return null;
		}
		while (toolbox.parentToolbox != null) {
			toolbox = toolbox.parentToolbox;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate (toolbox.rectTransform);
	}
}
