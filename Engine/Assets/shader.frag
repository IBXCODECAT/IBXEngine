#version 330 core

out vec4 outputColor;

in vec2 uvCoord;       // UV coordinates from the vertex shader
in vec3 Normal;       // Normals from the vertex shader
in vec3 FragPos;       // Fragment position from the vertex shader

// Samplers for the textures
uniform sampler2D texture0;
uniform sampler2D texture1;

// Lighting uniforms
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos; // Camera position for specular highlights

void main()
{
    // Ambient light color
    vec3 ambientLightColor = vec3(0.1, 0.1, 0.1);

    // Normalize the input normals
    vec3 norm = normalize(Normal);

    // Calculate the direction of the light
    vec3 lightDir = normalize(lightPos - FragPos);  

    // Calculate the diffuse component
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuseColor = diff * lightColor;

    // Calculate the specular component
    float shininess = 32.0;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
    vec3 specularColor = spec * lightColor;

    // Sample the textures and mix them
    vec4 textureColor0 = texture(texture0, uvCoord);
    vec4 textureColor1 = texture(texture1, uvCoord);
    vec4 textureColor = mix(textureColor0, textureColor1, 0.5);

    // Combine the lighting components
    vec4 ambient = vec4(ambientLightColor, 1.0);
    vec4 diffuse = vec4(diffuseColor, 1.0);
    vec4 specular = vec4(specularColor, 1.0);

    // Final output color
    outputColor = textureColor * (ambient + diffuse + specular);
}