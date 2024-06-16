#version 330 core

out vec3 OUTPUT_COLOR;

uniform vec3 lightColor;

void main()
{
	OUTPUT_COLOR = lightColor;
}