#ifndef DEPENDENCYCHECKERS_H_INCLUDED
#define DEPENDENCYCHECKERS_H_INCLUDED

//#define ANFP

int FileExists(const char* filename);
int CheckDotNet(int major, int minor, int build);
int CheckDirectX();
int CheckMSVCR90();

#endif // DEPENDENCYCHECKERS_H_INCLUDED
