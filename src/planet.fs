// Generate a procedural planet and orbiting moon. Use layers of (improved)
// Perlin noise to generate planetary features such as vegetation, gaseous
// clouds, mountains, valleys, ice caps, rivers, oceans. Don't forget about the
// moon. Use `animation_seconds` in your noise input to create (periodic)
// temporal effects.
//
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
in vec3 sphere_fs_in; // 3D position _before_ applying model, view or projection  
                      // transformations (e.g., point on unit sphere)
in vec3 normal_fs_in; // view and model transformed 3D normal
in vec4 pos_fs_in;  //  projected, view, and model transformed 3D position
in vec4 view_pos_fs_in; // view and model transformed 3D position
// Outputs:
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent

float compute_custom_bump_height(bool is_moon, vec3 s){
  float height;
  if (is_moon){
    height = bump_height(is_moon, 10*s);
  } else {
    height = bump_height(is_moon, 10*s);
  }
  return height;
}

vec3 compute_custom_bump_position(bool is_moon, vec3 s){
  float height;
  if (is_moon){
    height = 3*bump_height(is_moon, s*vec3(2, 2, 2));
    height += 0.5*bump_height(is_moon, s*vec3(8, 8 ,8));
  } else {
    height = 1*bump_height(is_moon, s*vec3(1, 4 ,1));
    height += 0.5*bump_height(is_moon, s*vec3(4, 4 ,4));
    height += 0.25*bump_height(is_moon, s*vec3(8, 8 ,8));
  }
  return (1 + height)*s;
}

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
  float top_level_noise, height;
  vec3 base_color, spec_color, new_sphere_pos, new_view_pos_fs_in, new_normal_fs_in;

  vec3 T, B;
  vec3 unit_sphere_fs_in = normalize(sphere_fs_in);
  tangent(normalize(sphere_fs_in), T, B);
  float epsilon = 0.0001;
  new_sphere_pos = compute_custom_bump_position(is_moon, unit_sphere_fs_in);
  vec3 dh_dT = (compute_custom_bump_position(is_moon, unit_sphere_fs_in + epsilon*T) - new_sphere_pos) / epsilon;
  vec3 dh_dB = (compute_custom_bump_position(is_moon, unit_sphere_fs_in + epsilon*B) - new_sphere_pos) / epsilon;
  vec3 new_normal = normalize(cross(dh_dT, dh_dB));

  new_view_pos_fs_in = (view*model(is_moon, animation_seconds)*vec4(new_sphere_pos, 1)).xyz;
  new_normal_fs_in = (view*model(is_moon, animation_seconds)*vec4(new_normal, 0)).xyz;

  if (is_moon){
    // base_color = vec3(0.5,0.5,0.5);
    base_color = vec3(0.8, 0.8, 0.8);
  } else {
    // Try to replicate the banding pattern on Jupiter
    // Also have it swwirl with time like the storms
    if (length(new_sphere_pos) <= 0.98){
      base_color = vec3(0, 0, 1);
    } else if (length(new_sphere_pos)<= 1.02) {
      base_color = vec3(0, 1, 0);
    } else {
      base_color = vec3(1, 0, 0);
    }
  }

  color = blinn_phong(base_color, base_color, vec3(0.8, 0.8, 0.8), 
    /*phong*/ phong, 
    normalize(new_normal_fs_in), 
    normalize(-new_view_pos_fs_in.xyz), 
    normalize(view_space_light_direction));
  /////////////////////////////////////////////////////////////////////////////
}
