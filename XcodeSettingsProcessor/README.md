PostProcessBuildPlayer
======================

This is a Python script that will parse the project.pbxproj file and edit some Xcode build settings.

Note: this particular implementation edits the Debug Information Format setting. 

A Quick Tutorial
----------------

It’s great that Unity can spit out a ready-to-build Xcode project for you, but sometimes you will need to customize some of the build settings based on your needs. You can do it after Unity creates the project of course, but if you have to re-build a fresh project from Unity you will lose your changes. What we can do instead is add a step to the Unity build process that makes our custom changes. 

This step can be added through a PostProcessBuildPlayer (if you’re not familiar with these, read about them here http://docs.unity3d.com/Documentation/Manual/BuildPlayerPipeline.html). The gist of it is, Unity automatically executes this commandline script after building the player. You can do whatever actions you like in this script. We’re going to use it to process our Xcode project. 

The file we’re after is project.pbxproj. This file is not in plain view,  but what you will see is a file called You_Project_Name.xcodeproj. This is actually a folder. You can see its contents by right-clicking on the file and selecting Show Package Contents… You can now open project.pbxproj in any text editor and inspect it. You’ll probably find it a little overwhelming at first, but eventually you will find the section you’re interested it. In my case, I was after the Build Settings. More specifically, I wanted to set the Debug Information Format setting. 

The first thing to do is, in Xcode, go to your Build Settings tab. Then from the Xcode menu, toggle Editor > Show Settings Names. You’ll notice the build settings are now displayed in a slightly different format. This is the format they are actually saved as and are referenced by in config files. My setting name now shows as DEBUG_INFORMATION_FORMAT. Next, toggle Editor > Show Definitions. This will do the same for the values of each setting.

Now here’s what I wanted to achieve: 
- for a Debug build, set DEBUG_INFORMATION_FORMAT = dwarf
- for a Release build, set DEBUG_INFORMATION_FORMAT = dwarf-with-dsym

If you’ve a regular Unity user, you’ve probably noticed the amount of time generating the DSYM file adds to your build. I wanted to turn this off when generating Debug builds. (Note: the DSYM file is important for symbolication of crash reports and should be generated with any builds you plan on releasing. You can read about it here https://developer.apple.com/library/ios/technotes/tn2151/_index.html#//apple_ref/doc/uid/DTS40008184-CH1-ANALYZING_CRASH_REPORTS-SYMBOLICATION)

As this was my first time inspecting the project.pbxproj file, I wasn’t exactly sure where to look. I started by searching for the name of my settings, but no results were found. So I searched for Build Settings and got a few hits. I found a few occurrences under the XCBuildConfiguration section. If you go to this section, you will see blocks of code like this:

	1D6058940D05DD3E006BFB54 /* Debug */ = {
		isa = XCBuildConfiguration;
		buildSettings = {
			ALWAYS_SEARCH_USER_PATHS = NO;
			ARCHS = armv7;
			…
		};
	};


And similar block for /* Release */.

Since my setting wasn’t listed in these blocks, I tried adding it. Sure enough, I saw the changes reflected in my Xcode Build Settings tab. So now I just needed to automate this. This is the easy or the hard part, depending on how you feel about string parsing!

After a bit of time dealing with some seemingly random whitespace, I threw together a python script that read through the project.pbxproj file and inserted the lines I wanted into the right spots. If the line already exists, we’ll replace it with our own.

After you’re happy with your script and you’ve checked that it does what you need, place it under Assets/Editor in you Unity project and make sure its named PostProcessBuildPlayer (no extension). As I said earlier, Unity will look for this file and execute it on its own. On thing to note here is that the script is executed from your Unity project’s root directory (the directory where the Assets folder sits). So any paths you reference in your PostProcessBuildPlayer must be relative to that directory.

And we’re done! 

