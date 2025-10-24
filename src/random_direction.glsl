// Generate a pseudorandom unit 3D vector
// 
// Inputs:
//   seed  3D seed
// Returns psuedorandom, unit 3D vector drawn from uniform distribution over
// the unit sphere (assuming random2 is uniform over [0,1]²).
//
// expects: random2.glsl, PI.glsl
vec3 random_direction( vec3 seed)
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  // Treat samples as theta and phi in spherical coords then convert to Cartesian
  // https://mathworld.wolfram.com/SpherePointPicking.html. (requires acos)
  vec2 random = random2(seed);
  float phi = M_PI * 2 * random.x;
  float theta = acos(2*random.y - 1);

  return vec3(sin(theta)*cos(phi),sin(theta)*sin(phi),cos(theta));
  /////////////////////////////////////////////////////////////////////////////
}
