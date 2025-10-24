// Given a 3d position as a seed, compute a smooth procedural noise
// value: "Perlin Noise", also known as "Gradient noise".
//
// Inputs:
//   st  3D seed
// Returns a smooth value between (-1,1)
//
// expects: random_direction, smooth_step
float perlin_noise( vec3 st) 
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 

  // Get corners of unit grid that st lies in
  float min_x = floor(st.x);
  float max_x = min_x + 1.0;
  float min_y = floor(st.y);
  float max_y = min_y + 1.0;
  float min_z = floor(st.z);
  float max_z = min_z + 1.0;

  vec3 corner_000 = vec3(min_x, min_y, min_z);
  vec3 corner_001 = vec3(min_x, min_y, max_z);
  vec3 corner_010 = vec3(min_x, max_y, min_z);
  vec3 corner_100 = vec3(max_x, min_y, min_z);
  vec3 corner_011 = vec3(min_x, max_y, max_z);
  vec3 corner_101 = vec3(max_x, min_y, max_z);
  vec3 corner_110 = vec3(max_x, max_y, min_z);
  vec3 corner_111 = vec3(max_x, max_y, max_z);

  vec3 gradient_000 = random_direction(corner_000);
  vec3 gradient_001 = random_direction(corner_001);
  vec3 gradient_010 = random_direction(corner_010);
  vec3 gradient_100 = random_direction(corner_100);
  vec3 gradient_011 = random_direction(corner_011);
  vec3 gradient_101 = random_direction(corner_101);
  vec3 gradient_110 = random_direction(corner_110);
  vec3 gradient_111 = random_direction(corner_111);

  float dot_prod_000 = dot(gradient_000, corner_000 - st);
  float dot_prod_001 = dot(gradient_001, corner_001 - st);
  float dot_prod_010 = dot(gradient_010, corner_010 - st);
  float dot_prod_100 = dot(gradient_100, corner_100 - st);
  float dot_prod_011 = dot(gradient_011, corner_011 - st);
  float dot_prod_101 = dot(gradient_101, corner_101 - st);
  float dot_prod_110 = dot(gradient_110, corner_110 - st);
  float dot_prod_111 = dot(gradient_111, corner_111 - st);

  float tx = smooth_step(st.x - min_x);
  float ty = smooth_step(st.y - min_y);
  float tz = smooth_step(st.z - min_z);

  // lerp x axis using tx
  float interp_x00 = mix(dot_prod_000, dot_prod_100, tx);
  float interp_x10 = mix(dot_prod_010, dot_prod_110, tx);
  float interp_x01 = mix(dot_prod_001, dot_prod_101, tx);
  float interp_x11 = mix(dot_prod_011, dot_prod_111, tx);

  // lerp y axis using ty
  float interp_xy0 = mix(interp_x00, interp_x10, ty);
  float interp_xy1 = mix(interp_x01, interp_x11, ty);


  return mix(interp_xy0, interp_xy1, tz);
  /////////////////////////////////////////////////////////////////////////////
}

