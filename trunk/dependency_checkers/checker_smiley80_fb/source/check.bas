#Include "windows.bi"
#Include "file.bi"

'check dotnet
Dim hKey As PHKEY
Dim result As Long
result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\.NetFramework\policy\v2.0", 0, KEY_READ, @hKey)

If result = ERROR_SUCCESS Then
   Print ".Net Framework 2.0 found."
Else
   Print ".Net Framework 2.0 not found."
EndIf

'check directx
If FileExists("c:/windows/system32/D3DX9_40.dll") Then
   Print "DirectX 9.0c (March 2009) found."
Else
   Print "DirectX 9.0c (March 2009) not found."
EndIf

'check vcredist
If FileExists("C:/windows/WinSxS/x86_Microsoft.VC90.CRT_1fc8b3b9a1e18e3b_9.0.30729.4148_x-ww_d495ac4e/msvcr90.dll") Then
   Print "VC++ 2008 SP1 Redist found."
Else
   Print "VC++ 2008 SP1 Redist not found."
EndIf

Dim A As String
Input "Press any key to continue.", A

