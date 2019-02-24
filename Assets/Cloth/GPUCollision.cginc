//
// DEFINES
//

#define EPSILON 0.001



//
// TYPES
//

struct Ray {
	float3 origin, direction;
};

struct SphereCollider {
	float3 center;
	float radius;
};

struct BoxCollider {
	float3 center;
	float3 extents;
};

struct Hit {
	bool collision;
	float3 hitPoint,  hitNormal;
};


//
// UTILITY METHODS
//

float3 NearestPointOnRay(Ray r, float3 p) {
	// If the ray is sufficiently short, just return the midpoint.
	if (length(r.direction) < EPSILON) {
		return r.origin +r.direction/2.0;
	}

	float t = dot(p - r.origin, r.direction)/dot(r.direction, r.direction);
	return r.origin + t*normalize(r.direction);
}

float3 Reflect(float3 original, float3 normal) {
	return original - 2.0*dot(original, normal)*normal;
}



//
// COLLISIONS
//

/* Ray Sphere Intersection. */
Hit RaySphereCollision(Ray r, SphereCollider s, float padding) {
	Hit h;
	h.collision = false;
	h.hitPoint = float3(0,0,0);
	h.hitNormal = float3(0,1,0);

	// Get the nearest point along the line to the sphere center.
	float3 nearestPoint = NearestPointOnRay(r, s.center);

	/* If the nearest point on the ray is less than the radius of the sphere,
	then the ray must have passed through the sphere. */
	if (length(nearestPoint - s.center) <= s.radius+padding) {
		h.collision = true;
		h.hitNormal = normalize(nearestPoint - s.center);
		h.hitPoint = nearestPoint + h.hitNormal*EPSILON;
	}

	return h;
}

Hit RayBoxCollision(Ray r, BoxCollider b) {
	Hit h;
	h.collision = false;
	h.hitPoint = float3(0, 0, 0);
	h.hitNormal = h.hitPoint;




	return h;
}
