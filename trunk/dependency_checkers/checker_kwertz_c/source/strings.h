#ifndef MESSAGES_H_INCLUDED
#define MESSAGES_H_INCLUDED

#define DEPENDENCY_MISSING_TEXT "Dependency missing"

// @Beauty: Achte auf dein Englisch! ;)

#define MOGRE_MISSING_TEXT \
"MOGRE needs at least the .NET Framework 2.0\n" \
"But for the automatic build of MOGRE you need .NET Framework 3.5.\n" \
"Do you want to download it now?"

#define DIRECTX_90_MISSING_TEXT \
"DirectX is outdated or not installed.\n" \
"For MOGRE you need at least DirectX 9.0c of March 2009.\n" \
"Do you want to download the latest revision of DirectX 9.0c now?"

#define MSVCR_90_MISSING_TEXT \
"It is recommended to install the \"Microsoft Visual C++ 2008 SP1 Redistributable Package\".\n" \
"Do you want to download it?"

//+---------------------------------------------------------------------------

#define CHECKS_COMPLETE "Checks complete"
#define CHECKS_FAILED "One or more checks failed"

#define CHECKS_SUCCESSFUL_TEXT \
"Done. Your system configuration passed all checks.\n" \
"You may now build the samples using the \"Build\" button."

#define CHECKS_FAILED_TEXT \
"There were one or more dependencies missing.\n" \
"To go to the download page of the dependency in question, click on its check box.\n" \
"To perform the checks again, click on the \"Check\" button."

// Include skip message?

//+---------------------------------------------------------------------------

// Filenames and URLs

#define URL_DOTNET_INSTALL "http://www.microsoft.com/downloads/details.aspx?FamilyId=333325FD-AE52-4E35-B531-508D977D32A6&displaylang=en"
#define URL_DIRECTX_90_INSTALL "http://www.microsoft.com/downloads/details.aspx?FamilyId=2da43d38-db71-4c1b-bc6a-9b6652cd92a3&displaylang=en"
#define URL_MSVCR_90_INSTALL "http://www.microsoft.com/downloads/details.aspx?FamilyId=A5C84275-3B97-4AB7-A40D-3802B2AF5FC2&displaylang=en"

#define BUILD_SAMPLES_SCRIPT "BuildSamples.cmd"

#define SCRIPT_RUN_FAILED "Build script start failed"

#define SCRIPT_RUN_FAILED_TEXT \
"There was an error while trying to run the build script.\n" \
"Please ensure the existance of the script."

#endif // MESSAGES_H_INCLUDED
