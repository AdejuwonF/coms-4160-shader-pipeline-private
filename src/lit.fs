// Add (hard code) an orbiting (point or directional) light to the scene. Light
// the scene using the Blinn-Phong Lighting Model.
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
// expects: PI, blinn_phong
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  // Copied from rotate_about_y which isn't be imported?
  float theta = mod(animation_seconds*(0.25) * M_PI, 2*M_PI);
  mat4 rotate_about_y = mat4(
    cos(theta),0,-sin(theta),0,
    0,         1,   0,       0,
    sin(theta),0,cos(theta), 0,
    0,         0,    0,      1);
  vec3 world_space_light_direction = (rotate_about_y * vec4(1, 1, 0, 0)).xyz;
    // (rotate_about_y(mod(animation_seconds*(1.0) * M_PI, 2*M_PI)) * vec4(0, 1, 1, 0)).xyz;

  vec3 view_space_light_direction = (view*vec4(world_space_light_direction, 0)).xyz;
  vec3 base_color = vec3(0, 0, 1);
  if (is_moon){
    base_color = vec3(0.5,0.5,0.5);
  }
  color = blinn_phong(base_color, base_color, vec3(0.8, 0.8, 0.8), 
    /*phong*/ 1000.0, 
    normalize(normal_fs_in), 
    normalize(-view_pos_fs_in.xyz), 
    normalize(view_space_light_direction));

  // Is Moon
  // color = vec3(float(is_moon),1,0)

  // Depth
  // color = (1+(view_pos_fs_in.z - -3)/5)*vec3(1,1,1);
  
  // Normals
  // color = 0.5+0.5*normalize(normal_fs_in);

  // Position
  // color = vec3(0.5,0.5,0)+vec3(0.5,0.5,0)*view_pos_fs_in.xyz
  /////////////////////////////////////////////////////////////////////////////
}
