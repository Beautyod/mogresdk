//+---------------------------------------------------------------------------
//
//  MAIN.C - Provides the entry point for our dependency checker
//
//+---------------------------------------------------------------------------

#include <windows.h>

#include "windowcreator.h"

#define APPNAME "MOGRE_DEP"

char szAppName[] = APPNAME; // The name of this application
char szTitle[]   = "MOGRE Dependency Checker"; // The title bar text

HINSTANCE g_hInst;          // current instance

//+---------------------------------------------------------------------------
//
//  Function:   WndProc
//
//  Synopsis:   very unusual type of function - gets called by system to
//              process windows messages.
//
//  Arguments:  same as always.
//----------------------------------------------------------------------------

LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
		// ----------------------- First and last
		case WM_CREATE:
			OnWindowCreating(hwnd, g_hInst);
			break;

		case WM_DESTROY:
			PostQuitMessage(0);
			break;


		// ----------------------- Get out of it...

		case WM_KEYDOWN:
			if (VK_ESCAPE == wParam)
				DestroyWindow(hwnd);
			break;

        case WM_COMMAND:
            OnCommand(hwnd, wParam);
            break;

		// ----------------------- Let windows do all other stuff
		default:
			return DefWindowProc(hwnd, message, wParam, lParam);
	}
	return 0;
}

//+---------------------------------------------------------------------------
//
//  Function:   WinMain
//
//  Synopsis:   standard entrypoint for GUI Win32 apps
//
//----------------------------------------------------------------------------
int APIENTRY WinMain(
				HINSTANCE hInstance,
				HINSTANCE hPrevInstance,
				LPSTR     lpCmdLine,
				int       nCmdShow)
{
	MSG msg;

	WNDCLASS wc;

	HWND hwnd;

	// Fill in window class structure with parameters that describe
	// the main window.

	ZeroMemory(&wc, sizeof wc);
	wc.hInstance     = hInstance;
	wc.lpszClassName = szAppName;
	wc.lpfnWndProc   = (WNDPROC)WndProc;
	wc.style         = CS_DBLCLKS|CS_VREDRAW|CS_HREDRAW;
	//wc.hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
	wc.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
	wc.hCursor       = LoadCursor(NULL, IDC_ARROW);

	if (FALSE == RegisterClass(&wc)) return 0;

	// create the browser
	hwnd = CreateWindow(
		szAppName,
		szTitle,
		WS_OVERLAPPEDWINDOW|WS_VISIBLE,
		CW_USEDEFAULT,
		CW_USEDEFAULT,
		300,//CW_USEDEFAULT,
		193,//CW_USEDEFAULT,
		0,
		0,
		g_hInst,
		0);

	if (NULL == hwnd) return 0;

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0) > 0)
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return msg.wParam;
}

//+---------------------------------------------------------------------------
