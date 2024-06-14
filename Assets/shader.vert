#version 330 core

in vec3 aPosition; // Vertex positions are input to the vertex shader...
in vec2 aUVCoord; // Texture coordinates are input to the vertex shader...

out vec2 uvCoord; // UV Coordinates are output from the vertex shader to the fragment shader...

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
    // Then, we further the input texture coordinate to the output one.
    // texCoord can now be used in the fragment shader.
    
    // We don't need to do anything with the UVs, so we just pass them through to the fragment shader.
    uvCoord = aUVCoord;

    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}