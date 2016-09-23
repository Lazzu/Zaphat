using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace Zaphat.Assets.Materials
{
	[StructLayout(LayoutKind.Sequential)]
	public struct DefaultMaterialProperties
	{
		public Vector3 AmbientColor;
		public Vector3 DiffuseColor;
		public Vector3 SpecularColor;
	}
}