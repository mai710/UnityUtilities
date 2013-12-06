using UnityEditor;
using UnityEngine;
using System;
using System.IO;


/// <summary>
/// GUITexturePostprocessor
/// Sets Texture Type = GUI and Format = Truecolor for all textures imported into the Atlases_Slices folder.
/// </summary>
public class GUITexturePostprocessor : AssetPostprocessor {


	void OnPreprocessTexture()
	{

		TextureImporter textureImporter = assetImporter as TextureImporter;

		DirectoryInfo path = new DirectoryInfo(assetPath);
		Debug.Log(path.Parent.Name);
		if(path.Parent.Name == "Atlas_Slices")
		{
			textureImporter.textureType = TextureImporterType.GUI;
			textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
		}

	}

}
