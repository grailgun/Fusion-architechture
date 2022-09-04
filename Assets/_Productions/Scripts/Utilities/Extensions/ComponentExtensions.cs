using UnityEngine;

public static partial class ComponentExtensions
{
	// PUBLIC METHODS

	public static void SetActive(this Component component, bool value)
	{
		if (component == null)
			return;

		if (component.gameObject.activeSelf == value)
			return;

		component.gameObject.SetActive(value);
	}
}
