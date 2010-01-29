#define ANFP

#include "stdinc.h"

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

int check(int major, int minor, int build)
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
   wsprintf(root, "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v%d.%d.%d", major, minor, build);
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
int main(int argc, char **argv)
{
   //
   // Variables declaration for version numbers and found status
   //
   int major, minor, build, found;

   //
   // Syntax check
   //
   if (3 != argc)
   {
		printf(".NET Framework checker for MOGRE\n");
		printf("--------------------------------\n\n");
		printf("Usage: netchecker.exe <MOGRE sample GUI executable> <dotnetfx.exe|dotnetfx35setup.exe>\n\n");
		return -1;
   }

   //
   // Version info
   //
   major = 2;
   minor = 0;
   build = 50727;

   //
   // Perform the check
   //
   found = check(major, minor, build);

   //
   // Execute actions
   //
   if (found)
      system(argv[4]);
   else
   {
	   if (
	   MessageBox(0, "The .NET Framework 2.0 required to run MOGRE could not be found.\nDo you want to install it now?", 
		   "Dependency not found", MB_YESNO | MB_ICONQUESTION)
		   == IDYES
		   )
	   {
			system(argv[5]);
	   }
   }

   //
   // Return status
   //
   return found;
}