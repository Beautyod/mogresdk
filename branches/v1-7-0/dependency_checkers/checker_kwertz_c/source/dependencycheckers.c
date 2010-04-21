#include <stdio.h>
#include <windows.h>

#include "dependencycheckers.h"

int FileExists(const char* filename)
{
    FILE* file;

    file = fopen(filename, "r");
    int exists = !(file == NULL);

    fclose(file);
}

int CheckDotNet(int major, int minor, int build)
{
    char root[1024];
#ifdef ANFP
    char bld[1024];
#endif

    HKEY hKey;

    DWORD dwType = REG_SZ;
#ifdef ANFP
    DWORD dwSize;
    char buffer[1024];
#endif

    int found = FALSE;

#ifdef ANFP
    wsprintf(root, "SOFTWARE\\Microsoft\\.NETFramework\\policy\\v%d.%d", major, minor);
    wsprintf(bld, "%d", build);
#else
    wsprintf(root, "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v%d.%d", major, minor);
#endif

    if (ERROR_SUCCESS == RegOpenKeyEx(HKEY_LOCAL_MACHINE, root, 0, KEY_QUERY_VALUE, &hKey))
    {
#ifdef ANFP
        if (ERROR_SUCCESS == RegQueryValueEx(hKey, bld, NULL, &dwType, (unsigned char*) &buffer, &dwSize))
            found = TRUE;
#else
        found = TRUE;
#endif

        RegCloseKey(hKey);
    }

    return found;
}

int CheckDirectX()
{
    char winDir[MAX_PATH];
    GetWindowsDirectory(winDir, MAX_PATH);

    // DirectX 9.0c
    char pathStr[MAX_PATH];
    wsprintf(pathStr, "%d%d", winDir, "\\system32\\D3DX9_40.dll");

    return FileExists(pathStr);
}

int CheckMSVCR90()
{
    char winDir[MAX_PATH];
    GetWindowsDirectory(winDir, MAX_PATH);

    // Microsoft Visual C++ 2008 Runtime
    char pathStr[MAX_PATH];
    wsprintf(pathStr, "%d%d", winDir, "\\WinSxS\\x86_Microsoft.VC90.CRT_1fc8b3b9a1e18e3b_9.0.30729.4148_x-ww_d495ac4e\\msvcr90.dll");

    return FileExists(pathStr);
}
