using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;


/// <summary>
/// SplitTimelinePostprocessor
/// Mainly used for splitting a model's timeline at import. 
/// Reads from an XML file that follows the format in Template.xml
/// Some other settings are set on the model. These just happen to be the settings I always use on importing.
/// Adding/removing some setting is straight forward.
/// </summary>
public class SplitTimelinePostprocessor : AssetPostprocessor {

	void OnPreprocessModel()
	{
		ModelImporter modelImporter = assetImporter as ModelImporter;

		//Some personal settings I always make
		modelImporter.bakeIK = true;
		modelImporter.importMaterials = true;


		//Splitting the timeline based on the xml file provided by the artist

		//Find the xml file and parse it
		//We expect to find the xml file in the same folder as the model and to share the same name
		string filePath = Path.GetDirectoryName(assetPath) + "/Timelines/" + Path.GetFileNameWithoutExtension(assetPath);
		ClipXMLParser parser = new ClipXMLParser(filePath + ".xml");

		//Get the list od clips we pulled from the xml data
		XmlNodeList clipNodes = parser.GetClips();
		ModelImporterClipAnimation[] clips = new ModelImporterClipAnimation[clipNodes.Count];

		//Create a ModelImporterClipAnimation for each clip and populate the array
		for(int i=0; i<clipNodes.Count; i++)
		{		
			ModelImporterClipAnimation mica = new ModelImporterClipAnimation();
			mica.name 		= parser.GetClipAttribute("name", clipNodes[i]);
			mica.firstFrame = Int32.Parse( parser.GetClipAttribute("firstFrame", clipNodes[i]) );
			mica.lastFrame 	= Int32.Parse( parser.GetClipAttribute("lastFrame", clipNodes[i]) );
			mica.loop 		= bool.Parse( parser.GetClipAttribute("loop", clipNodes[i]) );
			mica.wrapMode 	= mica.loop ? WrapMode.Loop : WrapMode.Default;

			clips[i] = mica;
		}
	
		modelImporter.clipAnimations = clips;

	}


}
