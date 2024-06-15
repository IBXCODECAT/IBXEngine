#version 330 core

in vec3 aPosition; // Vertex positions are input to the vertex shader...
in vec3 aNormal; // Vertex normals are input to the vertex shader...
in vec2 aUVCoord; // Texture coordinates are input to the vertex shader...

out vec2 uvCoord; // UV Coordinates are output from the vertex shader to the fragment shader...
out vec3 Normals; // Normal is output from the vertex shader to the fragment shader...
out vec3 FragPos; // Fragment position is output from the vertex shader to the fragment shader...

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
    // Get the vertex positions so it renders correctly.
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;

    FragPos = vec3(model * vec4(aPosition, 1.0));

    // We don't need to do anything with the normals, so we just pass them through to the fragment shader.
    Normals = aNormal;

    // We don't need to do anything with the UVs, so we just pass them through to the fragment shader.
    uvCoord = aUVCoord;
}