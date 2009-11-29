IncludeFile "registry.pbi"

If CountProgramParameters() = 0
  MessageBox_(#Null, "Syntax: depchecker.exe <MOGRE sample GUI executable>", "Wrong parameter count", #MB_ICONWARNING)
  End
EndIf

; Check for .NET Framework 2.0
dotnet = KeyExists(#HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\.NetFramework\policy\v2.0", "")
If Not dotnet
  If MessageBox_(#Null, "The .NET Framework 2.0 required to run MOGRE could not be found." + Chr(13) + "Do you want to download it?", "Dependency missing", #MB_YESNO + #MB_ICONQUESTION) = #IDYES
    ShellExecute_(#Null, "open", "http://www.microsoft.com/downloads/details.aspx?FamilyId=333325FD-AE52-4E35-B531-508D977D32A6&displaylang=en", #Null, #Null, #SW_SHOWNORMAL)
  EndIf
  
  End
EndIf

; Get Windows directory
winDir$ = Space(#MAX_PATH)
GetWindowsDirectory_(winDir$, #MAX_PATH)
CreateRegularExpression(0, "\\")
winDir$ = ReplaceRegularExpression(0, winDir$, "/")

; Check for DirectX 9.0c
dx = (Not FileSize(winDir$ + "/system32/D3DX9_40.dll") = -1)
If Not dx
  If MessageBox_(#Null, "DirectX 9.0c required to run MOGRE could not be found." + Chr(13) + "Do you want to download it?", "Dependency missing", #MB_YESNO + #MB_ICONQUESTION) = #IDYES
      ShellExecute_(#Null, "open", "http://www.microsoft.com/downloads/details.aspx?FamilyId=2da43d38-db71-4c1b-bc6a-9b6652cd92a3&displaylang=en", #Null, #Null, #SW_SHOWNORMAL)
  EndIf
  
  End
EndIf

; Check for Microsoft Visual C++ 2008 SP1 runtime
vcrt = (Not FileSize(winDir$ + "/WinSxS/x86_Microsoft.VC90.CRT_1fc8b3b9a1e18e3b_9.0.30729.4148_x-ww_d495ac4e/msvcr90.dll") = -1)
If Not vcrt
  If MessageBox_(#Null, "The Microsoft Visual C++ 2008 SP1 runtime required to run MOGRE could not be found." + Chr(13) + "Do you want to download it?", "Dependency missing", #MB_YESNO + #MB_ICONQUESTION) = #IDYES
      ShellExecute_(#Null, "open", "http://www.microsoft.com/downloads/details.aspx?FamilyId=A5C84275-3B97-4AB7-A40D-3802B2AF5FC2&displaylang=en", #Null, #Null, #SW_SHOWNORMAL)
  EndIf
  
  ;End
EndIf

RunProgram(ProgramParameter(0))
; IDE Options = PureBasic 4.30 (Windows - x86)
; CursorPosition = 37
; FirstLine = 12
; Folding = -
; EnableXP
; UseIcon = resources\favicon.ico
; Executable = bin\depchecker.exe
; CommandLine = none.exe