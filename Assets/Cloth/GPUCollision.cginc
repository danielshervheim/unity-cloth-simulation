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
// COLLISIONS
//

/* Ray Sphere Intersection from "Real-Time Collision Detection" by Christer Ericson. */
Hit RaySphereCollision(Ray r, SphereCollider s) {
	Hit h;
	h.collision = false;

	float3 p;

	if (length(r.origin - s.center) < length(r.origin+r.direction-s.center)) {
		p = r.origin;
	}
	else {
		p = r.origin+r.direction;
	}

	if (length(p - s.center) <= s.radius) {
		h.collision = true;
	}

	h.hitPoint = s.center + normalize(p-s.center)*(s.radius+5.0*EPSILON);
	h.hitNormal = normalize(h.hitPoint - s.center);
	return h;

	/*
	h.collision = false;
	h.hitPoint = float3(0, 0, 0);
	h.hitNormal = float3(0, 1, 0);

	float b = dot(r.origin - s.center, normalize(r.direction));
	float c = dot(r.origin - s.center, r.origin - s.center) - pow(s.radius, 2.0);

	// todo: handle case where ray starts inside sphere

	if (c > 0 && b > 0) {
		return h;
	}

	float discriminant = pow(b, 2.0) - c;

	if (discriminant < 0.0) {
		return h;
	}
	
	float t = -b - sqrt(discriminant);

	if (t < 0.0 || t > length(r.direction)) {
		return h;
	}

	h.collision = true;
	float3 hp = r.origin + normalize(r.direction)*t;
	h.hitPoint = s.center + normalize(hp-s.center)*(s.radius + 5.0*EPSILON);
	h.hitNormal = normalize(h.hitPoint - s.center);
	return h;
	*/
}

Hit RayBoxCollision(Ray r, BoxCollider b) {
	Hit h;
	h.collision = false;
	h.hitPoint = float3(0, 0, 0);
	h.hitNormal = h.hitPoint;




	return h;
}
