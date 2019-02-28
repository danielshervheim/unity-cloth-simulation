using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUCollision {
	[System.Serializable]
	public struct SphereCollider {
		public Vector3 center;
		public float radius;
	}

	public static int SphereColliderSize() {
		return 16;
	}



	[System.Serializable]
	public struct BoxCollider {
		public Vector3 center, extents;
	}

	public static int BoxColliderSize() {
		return 24;
	}
}
