// Set the pixel color using Blinn-Phong shading (e.g., with constant blue and
// gray material color) with a bumpy texture.
// 
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
//                     linearly interpolated from tessellation evaluation shader
//                     output
in vec3 sphere_fs_in; // 3D position _before_ applying model, view or projection  
                      // transformations (e.g., point on unit sphere)
in vec3 normal_fs_in; // view and model transformed 3D normal
in vec4 pos_fs_in;  //  projected, view, and model transformed 3D position
in vec4 view_pos_fs_in; // view and model transformed 3D position
// Outputs:
//               rgb color of this pixel
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  float theta = mod(-animation_seconds*(0.25) * M_PI, 2*M_PI);
  float phong = 1000.0;
  mat4 rotate_about_y = mat4(
    cos(theta),0,-sin(theta),0,
    0,         1,   0,       0,
    sin(theta),0,cos(theta), 0,
    0,         0,    0,      1);
  vec3 world_space_light_direction = (rotate_about_y * vec4(1, 1, 0, 0)).xyz;
  vec3 view_space_light_direction = (view*vec4(world_space_light_direction, 0)).xyz;
  float top_level_noise;
  vec3 base_color, spec_color;
  if (is_moon){
    // base_color = vec3(0.5,0.5,0.5);
    base_color = vec3(0.8, 0.8, 0.8);
  } else {
    // Try to replicate the banding pattern on Jupiter
    // Also have it swwirl with time like the storms
    base_color = vec3(0, 0, 1);
  }

  vec3 T, B;
  vec3 unit_sphere_fs_in = normalize(sphere_fs_in);
  tangent(unit_sphere_fs_in, T, B);
  float epsilon = 0.0001;
  vec3 new_sphere_pos = bump_position(is_moon, unit_sphere_fs_in);
  vec3 dh_dT = (bump_position(is_moon, unit_sphere_fs_in + epsilon*T) - new_sphere_pos) / epsilon;
  vec3 dh_dB = (bump_position(is_moon, unit_sphere_fs_in + epsilon*B) - new_sphere_pos) / epsilon;
  vec3 new_normal = normalize(cross(dh_dT, dh_dB));

  vec3 new_view_pos_fs_in = (view*model(is_moon, animation_seconds)*vec4(new_sphere_pos, 1)).xyz;
  vec3 new_normal_fs_in = (view*model(is_moon, animation_seconds)*vec4(new_normal, 0)).xyz;

  color = blinn_phong(base_color, base_color, vec3(0.8, 0.8, 0.8), 
    /*phong*/ phong, 
    normalize(new_normal_fs_in), 
    normalize(-new_view_pos_fs_in.xyz), 
    normalize(view_space_light_direction));
  /////////////////////////////////////////////////////////////////////////////
}
