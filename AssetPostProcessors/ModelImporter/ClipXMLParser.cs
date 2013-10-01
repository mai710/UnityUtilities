using UnityEngine;
using System.Xml;
using System.Collections;


/// <summary>
/// Clip XML parser.
/// Parses an XML file for clip attributes that get set when splitting a timeline. 
/// </summary>
public class ClipXMLParser {

	private XmlDocument docXml;
	private int _clipCount;


	public ClipXMLParser(string filePath){
		docXml = new XmlDocument();
		docXml.Load(filePath);

		if(docXml == null){
			Debug.LogError("Your XML file needs to share the same path and file name as the model it references.");
		}
	}

	public XmlNodeList GetClips(){
		XmlNode rootNode = docXml.SelectSingleNode("timeline");
		return rootNode.SelectNodes("clip");
	}

	public string GetClipAttribute(string attribute, XmlNode clipNode){
		return clipNode.SelectSingleNode(attribute).InnerText;
	}

}
