using UnityEngine;
using UnityEditor;

public class scr_AssetImportSettings : AssetPostprocessor {

	void OnPreprocessModel () {
		ModelImporter modelImporter = (ModelImporter)assetImporter;
		modelImporter.importMaterials = false;
	}

	void OnPreprocessTexture () {
		TextureImporter textureImporter = (TextureImporter)assetImporter;
		textureImporter.anisoLevel = 0;
	}
}
