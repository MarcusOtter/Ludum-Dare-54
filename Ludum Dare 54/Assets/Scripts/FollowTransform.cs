using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private bool followPosition;
	[SerializeField] private bool followRotation;

	private void Update()
	{
		if (followPosition)
		{
			transform.position = target.position;
		}

		if (followRotation)
		{
			transform.rotation = target.rotation;
		}
	}
}
