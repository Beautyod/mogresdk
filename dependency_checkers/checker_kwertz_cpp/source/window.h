#include <windows.h>

namespace tinyplayer
{
	class Window  
	{
	public:
		Window(char* windowTitle) 
		{
			HINSTANCE hInstance = 0;
			// create dialog

			char wndCount[100];
			_windowCount++;
			int i = _windowCount;
			wsprintf(wndCount, "twindow_%i", i);
			char* Win32ClassName = wndCount;

			WNDCLASSEX wcex;
			wcex.cbSize                     = sizeof(WNDCLASSEX);
			wcex.style                      = CS_HREDRAW | CS_VREDRAW;
			wcex.lpfnWndProc        = (WNDPROC)CustomWndProc;
			wcex.cbClsExtra         = 0;
			wcex.cbWndExtra         = DLGWINDOWEXTRA;
			wcex.hInstance          = hInstance;
			wcex.hIcon                      = NULL;
			wcex.hCursor            = LoadCursor(NULL, IDC_ARROW);
			wcex.hbrBackground      = (HBRUSH)(COLOR_WINDOW);
			wcex.lpszMenuName       = 0;
			wcex.lpszClassName      = Win32ClassName;
			wcex.hIconSm            = 0;

			RegisterClassEx(&wcex);

			DWORD style = WS_SYSMENU | WS_BORDER | WS_CAPTION |
				WS_CLIPCHILDREN | WS_CLIPSIBLINGS | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_SIZEBOX;

			int windowWidth = 440;
			int windowHeight = 380;

			_hWnd = CreateWindow(Win32ClassName, windowTitle/*"tlibc :: win32 window"*/,
				style, 100, 100, windowWidth, windowHeight,
				NULL, NULL, hInstance, NULL);
			
			RECT clientRect;
			GetClientRect(_hWnd, &clientRect);
			windowWidth = clientRect.right;
			windowHeight = clientRect.bottom;
			
			//HWND okButton = CreateWindow("BUTTON", "OK", WS_CHILD | WS_VISIBLE | BS_TEXT,
			//	windowWidth - 80, windowHeight - 28, 75, 23, _hWnd, NULL, hInstance, NULL);
			//HFONT hFont = CreateFont (13, 0, 0, 0, FW_DONTCARE, FALSE, FALSE, FALSE, ANSI_CHARSET, 
			//	OUT_TT_PRECIS, CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY, 
			//	DEFAULT_PITCH | FF_DONTCARE, TEXT("Tahoma"));
			//SendMessage(okButton, WM_SETFONT, (WPARAM)hFont, TRUE);
		};

		void Show()
		{
			ShowWindow(_hWnd, SW_SHOW);
			UpdateWindow(_hWnd);
		};

		void DoMessageLoop()
		{
			//for (;;)
			//	UpdateWindow(hWnd);

			MSG msg;
			for (;;) 
			{ 
				if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE)) 
				{ 
					TranslateMessage(&msg); DispatchMessage(&msg);
					if (msg.message == WM_QUIT) break; 
				}
			}
		};

		HWND _getHandle()
		{
			return _hWnd;
		};

		~Window()
		{
			MessageBox(0, "Destructor called!", "This is the caption!", 0);
		};

	private:
		HWND _hWnd;

		static LRESULT CALLBACK CustomWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
		{
			switch (message)
			{
				//case WM_COMMAND:
				//		break;
				case WM_DESTROY:
						PostQuitMessage(0);
						return 0;
			}

			return DefWindowProc(hWnd, message, wParam, lParam);
		};

		static int _windowCount;
	};

	int Window::_windowCount = 0;
}