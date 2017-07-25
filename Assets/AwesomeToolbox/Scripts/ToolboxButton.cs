using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxButton : ToolboxItem {
	public bool IsToggle = false;
	public bool IsToggled = false;

	public bool IsSelected = false;


	public void OnClick() {
		parentToolbox.OnItemClick (this);
	}

}
