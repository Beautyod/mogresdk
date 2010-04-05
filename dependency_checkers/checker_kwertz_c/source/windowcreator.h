#ifndef WINDOWCREATOR_H_INCLUDED
#define WINDOWCREATOR_H_INCLUDED

#define WINDOWS_STANDARD_FONT (HFONT)GetStockObject(DEFAULT_GUI_FONT)

void CenterWindow(HWND hwnd_self);
void AddControls(HWND hwnd, HINSTANCE hinst);
void OnWindowCreating(HWND hwnd, HINSTANCE hinst);
void OnCommand(HWND hwnd, WPARAM wParam);

#endif // WINDOWCREATOR_H_INCLUDED
