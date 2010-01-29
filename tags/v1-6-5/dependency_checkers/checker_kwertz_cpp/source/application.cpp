//#define ANFP
#define _CRT_SECURE_NO_WARNINGS
//#define FCLOSE_WORKAROUND

#include "stdinc.h"

//
// Helper function to determine whether a file exists
//
bool FileExists(const char* filename) 
{
	FILE* file;

	file = fopen(filename, "r");
	bool exists = !(file == NULL);

	// BUG: On some systems, closing a file using tlibc causes an access violation
	// WORKAROUND: Don't do it, simply! The file is closed on ExitProcess anyways.
#ifndef FCLOSE_WORKAROUND
	fclose(file);
#endif

	return exists;
}

/**************************************************************************************************
*
* Function to check for the presence of a given version of the .NET Framework on the machine.
*
* Parameters:
* - major version (e.g. 1)
* - minor version (e.g. 1)
* - build number (e.g. 4322)
*
* Returns:
* - 0 when not found
* - 1 when found
*
**************************************************************************************************/

int check(int major, int minor/*, int build*/)
{
   //
   // Variables for registry functions parameter construction
   //
   char root[1024];
#ifdef ANFP
   char bld[1024];
#endif

   //
   // Registry key
   //
   HKEY hkey;

   //
   // Registry type, value, result buffer
   //
   DWORD dwType = REG_SZ;
#ifdef ANFP
   DWORD dwSize;
   char buffer[1024];
#endif

   //
   // Found flag
   //
   int found = FALSE;

   //
   // Registry functions parameter construction
   //
#ifdef ANFP
   wsprintf(root, "SOFTWARE\\Microsoft\\.NETFramework\\policy\\v%d.%d", major, minor);
   wsprintf(bld, "%d", build);
#else
   wsprintf(root, "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v%d.%d" /*".%d"*/, major, minor /*, build*/);
#endif

   //
   // Open registry key
   //
   if (ERROR_SUCCESS == RegOpenKeyEx(HKEY_LOCAL_MACHINE, root, 0, KEY_QUERY_VALUE, &hkey))
   {
#ifdef ANFP
      //
      // Read registry value
      //
      if (ERROR_SUCCESS == RegQueryValueEx(hkey, bld, NULL, &dwType, (unsigned char*) &buffer, &dwSize))
         found = TRUE;
#else
      found = TRUE;
#endif

      //
      // Close registry key
      //
      RegCloseKey(hkey);
   }

   return found;
}

/**************************************************************************************************
*
* Main entry point.
*
* Parameters:
* - number of command-line arguments
* - array with command-line arguments
*
* Returns:
* - -1 on invalid syntax
* - 1 if version was found
* - 0 if version wasn't found
*
**************************************************************************************************/
int main(/*int argc, char **argv*/)
{
	//
	// Variable declaration for check status
	//
	bool dotNet, directX, msvcrt, checksPassed;

	// TODO: Implement the other checks

	//
	// Check for .NET Framework 3.5
	//
	int major = 3;
	int minor = 5;
	//int build = 30729;

perform:

	dotNet = check(major, minor/*, build*/) == 1;

	if (!dotNet)
		if (
			MessageBox(NULL, "MOGRE needs at least the .NET Framework 2.0.\n" 
						   + "But for automatically build of the MOGRE samples you need .NET 3.5.\n" 
						   + "Do you want to download the .NET Framework 3.5 now?", 
			"Dependency missing", MB_YESNO | MB_ICONQUESTION)
		   == IDYES
		   )
			ShellExecute(NULL, "open", "http://www.microsoft.com/downloads/details.aspx?FamilyId=333325FD-AE52-4E35-B531-508D977D32A6&displaylang=en", NULL, NULL, SW_SHOWNORMAL);

	//
	// Get Windows directory
	//

	char winDir[MAX_PATH];
	GetWindowsDirectory(winDir, MAX_PATH);

	//
	// Check for DirectX 9.0c
	//
	char pathStr[MAX_PATH];
	strcpy(pathStr, winDir);
	strcat(pathStr, "\\system32\\D3DX9_40.dll");
	directX = FileExists(pathStr);

	if (!directX)
		if (
			MessageBox(NULL, "DirectX is outdated or not installed.\n"
			               + "For MOGRE you need at least DirectX 9.0c of March 2009.\n"
			               + "Do you want to download the latest revision of DirectX 9.0c?", 
			"Dependency missing", MB_YESNO | MB_ICONQUESTION)
			== IDYES
		)
		 ShellExecute(NULL, "open", "http://www.microsoft.com/downloads/details.aspx?FamilyId=2da43d38-db71-4c1b-bc6a-9b6652cd92a3&displaylang=en", NULL, NULL, SW_SHOWNORMAL);

	//
	// Check for Microsoft Visual C++ 2008 SP1 runtime
	// NOTE: We can do this here, because tlibc does not depend on it
	//
	pathStr[MAX_PATH];
	strcpy(pathStr, winDir);
	strcat(pathStr, "\\WinSxS\\x86_Microsoft.VC90.CRT_1fc8b3b9a1e18e3b_9.0.30729.4148_x-ww_d495ac4e\\msvcr90.dll");
	msvcrt = FileExists(pathStr);

	if (!msvcrt)
		if (
			MessageBox(NULL, "It's recommended to install the \"Microsoft Visual C++ 2008 SP1 Redistributable Package\"."
			               + "Do you want to download it?", 
			"Dependency missing", MB_YESNO | MB_ICONQUESTION)
			== IDYES
		)
		 ShellExecute(NULL, "open", "http://www.microsoft.com/downloads/details.aspx?FamilyId=A5C84275-3B97-4AB7-A40D-3802B2AF5FC2&displaylang=en", NULL, NULL, SW_SHOWNORMAL);

	//
	//
	//
	checksPassed = dotNet && directX && msvcrt;

	if (checksPassed)
	{
		int dialogResult = MessageBox(NULL, "Your system configuration passed all checks.\n"
			                              + "Do you want to build the samples now?", 
			"Checks complete", MB_YESNO | MB_ICONQUESTION);
		if (dialogResult == IDYES)
			ShellExecute(NULL, "open", "BuildSamples.cmd", NULL, ".", SW_SHOWNORMAL);
	}
	else
	{
		if (
			MessageBox(NULL, "There were one or more dependencies missing. \n"
			               + "Do you want to perform the checks again?",
			"One or more checks failed", MB_YESNO | MB_ICONWARNING)
			== IDYES
			)
			 goto perform;
		else
			 MessageBox(NULL, "You choose to skip the depencency checks.\n"
			                + "Please note that no MOGRE-based application will run until all dependencies are installed.\n" 
							+ "To perform the checks again, please re-run this checker application manually. \n"
							+ "(Use file \"dependencycheck.exe\" in the SDK root folder).",
			 "Dependency checks skipped", MB_ICONWARNING);
	}

	//
	// Return status
	//
	return checksPassed ? 0 : 1;
}