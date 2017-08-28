
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEditor;

public class MasterBuildScript : MonoBehaviour {
	static string[] masterSceneList = new string[] {"UdacityVR/Scenes/Carnival"};
	static string buildPath = Application.dataPath.Replace("Assets","") + "Builds";
	static string projectName = "UdacityCarnival";
	static string buildId = ".";

	static TraceSource _trace = new TraceSource("BuildScript");


	[UnityEditor.MenuItem ("Buildpack/Run Android")]
	public static void RunAndroid() {
		Console.WriteLine("Hello World {0}", "CONSOLE");

		UnityEngine.Debug.Log(string.Format("You can run this in a terminal: {0}", "example"));
		string path = GenerateFilePath("ANDROID") + ".apk";
		_trace.TraceInformation ("path set to " + buildPath);
		_trace.TraceEvent(TraceEventType.Information,0, "path set to " + buildPath);
		UnityEngine.Debug.Log(string.Format("Writing to {0}", path));
		//System.Diagnostics.Debug("SYSTEM: "+path);
		Build(masterSceneList,  path, BuildTarget.Android, BuildOptions.None);
	}

	void Build(string  path, BuildTarget target, BuildOptions options){
		BuildPipeline.BuildPlayer(masterSceneList, path, target, options);
	}

	// Helper function for getting the command line arguments
	// *** from https://effectiveunity.com/articles/making-most-of-unitys-command-line.html ***
	private static string GetArg(string name){
		var args = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < args.Length; i++){
			if (args[i] == name && args.Length > i + 1){
				return args[i + 1];
			}
		}
		return null;
	}


	//this abstracts the various build settings into a single function invoked for different taregts
	private static void Build(string[] scenes, string destination, BuildTarget target, BuildOptions options){
		//overwrite scene list if passad as arg
		string sceneArgs = GetArg ("-scenes");
		if (sceneArgs != null) {
			scenes = sceneArgs.Split (',');
		}

		List<string> fullPathScenes = new List<string>();
		foreach (string scene in scenes) {
			fullPathScenes.Add(string.Format("Assets/{0}.unity", scene));
		}
		UnityEngine.Debug.Log(string.Format("Building scenes: {0}, id: {1}, output: {2}",  scenes[0], buildId, buildPath));
		UnityEngine.Debug.Log (string.Format (Application.dataPath));
		BuildPipeline.BuildPlayer(fullPathScenes.ToArray(), destination, target, options);
		// Get filename.


		//to set a save file path via window
		//string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");


		// Build player.


		// Copy a file from the project folder to the build folder, alongside the built game.
		//FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

		// Run the game (Process class from System.Diagnostics).
		//Process proc = new Process();
		//proc.StartInfo.FileName = path + "BuiltGame.exe";
		//proc.Start();
	}

	private static string GenerateFilePath(string system){
		string filename = masterSceneList[0];

		//overwrite scene list if passad as arg
		string sceneArgs = GetArg ("-scenes") as string;
		if (sceneArgs != null) {
			filename = sceneArgs.Split (',') [0];
		}

		string outputArg = GetArg ("-o");
		if (outputArg != null) {
			buildPath = outputArg;
		}

		string buildIdArg = GetArg ("-buildId");
		if (buildIdArg != null) {
			buildId = buildIdArg.Substring (0, 8);
		} else {
			buildId = string.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMdd"), "manual");
		}
		//TODO use git commit number instead of datetime
		return string.Format("{0}/{3}/{1}_{2}_{3}", buildPath, projectName, system, buildId);
		//return string.Format("{0}/{1}/{3}/{1}_{2}_{3}", buildPath, projectName, system, buildId);
	}
}
