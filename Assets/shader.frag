#version 330

out vec4 outputColor;

in vec2 uvCoord; // UV coordinates from the vertex shader.
in vec3 Normals; // Normals from the vertex shader.
in vec3 FragPos; // Fragment position from the vertex shader.

// A sampler2d is the representation of a texture in a shader.
// Each sampler is bound to a texture unit (texture units are described in Texture.cs on the Use function).
// By default, the unit is 0, so no code-related setup is actually needed.
// Multiple samplers will be demonstrated in section 1.5.

uniform sampler2D texture0;
uniform sampler2D texture1;

uniform vec3 lightPos;
uniform vec3 lightColor;

void main()
{
    vec3 ambientLightColor = vec3(0.1, 0.1, 0.1);

    vec3 norm = normalize(Normals);
    vec3 lightDir = normalize(lightPos - FragPos);  

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuseColor = diff * lightColor;

    vec4 textureColor = mix(texture(texture0, uvCoord), texture(texture1, uvCoord), 0.5);

    vec4 diffuse = vec4(diffuseColor, 1.0);
    vec4 ambient = vec4(ambientLightColor, 1.0);

    // Apply ambient lighting
    outputColor = textureColor * (ambient + diffuse);

    // To use a texture, you call the texture() function.
    // It takes two parameters: the sampler to use, and a vec2, used as texture coordinates.
    // outputColor = texture(texture0, uvCoord);
}