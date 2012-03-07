-------------------------------------
WARNING
-------------------------------------
This is a preliminary SDK distribution. Please do not use this SDK for production purposes.

-------------------------------------
What's this
-------------------------------------
MOGRE (Managed OGRE) is a C++/CLI wrapper for the Ogre 3D engine (www.ogre3d.org). You can use
Mogre with all .NET languages.

Mogre goes beyond a simple wrapper; By modifying Ogre's source the Ogre classes are integrated
into the .NET framework seamlessly.

If you want to use a plugin (like the Paging Landscape Scene Manager) with Mogre, you have to
recompile it using the modified Ogre's include files.


-------------------------------------
Home page
-------------------------------------
http://www.ogre3d.org/wiki/index.php/MOGRE


-------------------------------------
How to compile MOGRE
-------------------------------------
The most easy way is our "MogreBuilder" tool:
https://bitbucket.org/mogre/mogrebuilder

Instructions step by step:
http://www.ogre3d.org/tikiwiki/Building+MOGRE+from+source


-------------------------------------
What's the Ogre version
-------------------------------------
It's the 1.7.x (Cthugha) release.


-------------------------------------
What's the license
-------------------------------------
MIT (see license_MIT.txt)


-------------------------------------
Who is responsible
-------------------------------------
Well, there is no specific maintainer. 
There always active people.

Just visit our Mogre forum:
http://www.ogre3d.org/addonforums/viewforum.php?f=8

Before you ask questions, please use the forum search 
and look to our Mogre main page in the wiki:
http://www.ogre3d.org/tikiwiki/MOGRE

Special thanks to the father of Mogre:
Argiris Kirtzidis (Bekas) - He created the Mogre autowrapper in 2006-2007


-------------------------------------
Acknowledgements
-------------------------------------

--Ogre4J team
They provided an utility that gets the analysis that doxygen produces from C++ include files,
and transforms it into a language independent class structure definition file. I modified the
utility a bit to add more things into the definition file and I created a C# utility that
translates the definition file into C++/CLI wrapping code.
Thanks a lot guys!

--Ogre team
They provided the amazing Object-oriented Graphics Rendering Engine (OGRE).

--Mogre community
It's only a small sub-community of Ogre, but continues the Mogre development and gives support. 
Thanks to all Mogre members (-:
