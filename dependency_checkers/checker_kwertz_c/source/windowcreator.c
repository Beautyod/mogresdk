#include <windows.h>
#include <shellapi.h>
#include "windowcreator.h"
#include "dependencycheckers.h"
#include "strings.h"

//+---------------------------------------------------------------------------

void CenterWindow(HWND hwnd_self)
{
	RECT rw_self, rc_parent, rw_parent; HWND hwnd_parent;
	hwnd_parent = GetParent(hwnd_self);
	if (hwnd_parent == NULL) hwnd_parent = GetDesktopWindow();
	GetWindowRect(hwnd_parent, &rw_parent);
	GetClientRect(hwnd_parent, &rc_parent);
	GetWindowRect(hwnd_self, &rw_self);
	SetWindowPos(hwnd_self, NULL,
		rw_parent.left + (rc_parent.right + rw_self.left - rw_self.right) / 2,
		rw_parent.top  + (rc_parent.bottom + rw_self.top - rw_self.bottom) / 2,
		0, 0,
		SWP_NOSIZE|SWP_NOZORDER|SWP_NOACTIVATE
		);
}

//+---------------------------------------------------------------------------

//+---------------------------------------------------------------------------

enum
{
    ID_CHECKBOX_1,
    ID_CHECKBOX_2,
    ID_CHECKBOX_3,
    ID_BUTTON_1,
    ID_BUTTON_2
};

HWND button1, button2;
HWND dotnetCheckBox, dxCheckBox, msvcrCheckBox;

// -- Our designer method --
void AddControls(HWND hwnd, HINSTANCE hinst)
{
    // WHAT WE NEED:
    // * 3 check-boxes (which indicate the status of the dependencies, i.e. unchecked = missing, semi-checked = dependency not yet checked, checked = installed)
    // * 2 command buttons (one invokes the checker functions, the other one starts the build process)
    // * 1 label (header text)

    // -- ".NET Framework 2.0/3.5" check box --
    dotnetCheckBox = CreateWindow("Button", ".NET Framework 2.0/3.5",
        WS_CHILD | WS_VISIBLE | BS_3STATE, 12, 53, 144, 17, hwnd, (HMENU)ID_CHECKBOX_1, hinst, 0);
    CheckDlgButton(hwnd, ID_CHECKBOX_1, BST_INDETERMINATE);

    // -- "DirectX 9.0c" check box --
    dxCheckBox = CreateWindow("Button", "DirectX 9.0c",
        WS_CHILD | WS_VISIBLE | BS_3STATE, 12, 76, 85, 17, hwnd, (HMENU)ID_CHECKBOX_2, hinst, 0);
    CheckDlgButton(hwnd, ID_CHECKBOX_2, BST_INDETERMINATE);

    // -- "Visual C++ 2008 Runtime" check box --
    msvcrCheckBox = CreateWindow("Button", "Visual C++ 2008 Runtime",
        WS_CHILD | WS_VISIBLE | BS_3STATE, 12, 99, 145, 17, hwnd, (HMENU)ID_CHECKBOX_3, hinst, 0);
    CheckDlgButton(hwnd, ID_CHECKBOX_3, BST_INDETERMINATE);

    // -- "Check" command button --
    button1 = CreateWindow("Button", "Check",
        BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE, 197, 122, 75, 23, hwnd, (HMENU)ID_BUTTON_1, hinst, 0);

    // -- "Build" command button --
    button2 = CreateWindow("Button", "Build",
        BS_PUSHBUTTON | WS_CHILD | WS_VISIBLE, 12, 122, 75, 23, hwnd, (HMENU)ID_BUTTON_2, hinst, 0);
    EnableWindow(button2, FALSE);

    // -- Header text label --
    HWND label = CreateWindow("Static", "With this tool, you can check for the dependencies MOGRE-based applications will need to run. To perform the checks, click on \"Check\".",
        WS_CHILD | WS_VISIBLE, 9, 9, 263, 41, hwnd, 0, hinst, 0);

    // We need our system font to replace the ugly default one
    HFONT hFont = WINDOWS_STANDARD_FONT;

    // Set fonts
    SendMessage(dotnetCheckBox, WM_SETFONT, (WPARAM)hFont, MAKELPARAM(TRUE, 0));
    SendMessage(dxCheckBox, WM_SETFONT, (WPARAM)hFont, MAKELPARAM(TRUE, 0));
    SendMessage(msvcrCheckBox, WM_SETFONT, (WPARAM)hFont, MAKELPARAM(TRUE, 0));
    SendMessage(button1, WM_SETFONT, (WPARAM)hFont, MAKELPARAM(TRUE, 0));
    SendMessage(button2, WM_SETFONT, (WPARAM)hFont, MAKELPARAM(TRUE, 0));
    SendMessage(label, WM_SETFONT, (WPARAM)hFont, MAKELPARAM(TRUE, 0));
}

//+---------------------------------------------------------------------------

//+---------------------------------------------------------------------------

void OnWindowCreating(HWND hwnd, HINSTANCE hinst)
{
    // Store the window styles into a variable
    DWORD dwStyle = (DWORD)GetWindowLong(hwnd, GWL_STYLE);
    // Remove the styles
    dwStyle &= ~WS_MINIMIZEBOX & ~WS_MAXIMIZEBOX & ~WS_THICKFRAME;
    // Re-set the new window styles
    SetWindowLong(hwnd, GWL_STYLE, (DWORD)dwStyle);

    // Finally, tell Windows to refresh/repaint the caption area
    RedrawWindow(hwnd, NULL, NULL, RDW_INVALIDATE | RDW_FRAME | RDW_ERASENOW);

    AddControls(hwnd, hinst);

    CenterWindow(hwnd);
}

//+---------------------------------------------------------------------------

//+---------------------------------------------------------------------------

void OnCommand(HWND hwnd, WPARAM wParam)
{
    switch(wParam)
    {
        case ID_BUTTON_1:
        {
            // The user clicked the "Check" button. Let's do the checks...
            int dotnet = CheckDotNet(3, 5, 0);
            int dx = CheckDirectX();
            int msvcr90 = CheckMSVCR90();

            int result = dotnet & dx & msvcr90;

            CheckDlgButton(hwnd, ID_CHECKBOX_1, dotnet ? BST_CHECKED : BST_UNCHECKED);
            CheckDlgButton(hwnd, ID_CHECKBOX_2, dx ? BST_CHECKED : BST_UNCHECKED);
            CheckDlgButton(hwnd, ID_CHECKBOX_3, msvcr90 ? BST_CHECKED : BST_UNCHECKED);

            if (result)
            {
                MessageBox(hwnd, CHECKS_SUCCESSFUL_TEXT, CHECKS_COMPLETE, MB_ICONINFORMATION);
                EnableWindow(button1, FALSE);
                EnableWindow(button2, TRUE);
            }
            else
                MessageBox(hwnd, CHECKS_FAILED_TEXT, CHECKS_FAILED, MB_ICONWARNING);

            break;
        }

        case ID_BUTTON_2:
        {
            int result = (int)ShellExecute(NULL, "open", BUILD_SAMPLES_SCRIPT, NULL, ".", SW_SHOWNORMAL);
            if (result <= 32)
                MessageBox(hwnd, SCRIPT_RUN_FAILED_TEXT, SCRIPT_RUN_FAILED, MB_ICONWARNING);
            else
                DestroyWindow(hwnd);

            break;
        }

        case ID_CHECKBOX_1:
        {
            int checkState = SendMessage(dotnetCheckBox, BM_GETCHECK, 0, 0);
            if (checkState == BST_UNCHECKED)
                ShellExecute(NULL, "open", URL_DOTNET_INSTALL, NULL, ".", SW_SHOWNORMAL);
            else if (checkState == BST_CHECKED)
                MessageBox(hwnd, "You do not need to install the .NET Framework, because you already have the latest version.", "Not needed", MB_ICONINFORMATION);

            break;
        }

        case ID_CHECKBOX_2:
        {
            int checkState = SendMessage(dxCheckBox, BM_GETCHECK, 0, 0);
            if (checkState == BST_UNCHECKED)
                ShellExecute(NULL, "open", URL_DIRECTX_90_INSTALL, NULL, ".", SW_SHOWNORMAL);
            else if (checkState == BST_CHECKED)
                MessageBox(hwnd, "You do not need to install DirectX, because you already have the latest version.", "Not needed", MB_ICONINFORMATION);

            break;
        }

        case ID_CHECKBOX_3:
        {
            int checkState = SendMessage(msvcrCheckBox, BM_GETCHECK, 0, 0);
            if (checkState == BST_UNCHECKED)
                ShellExecute(NULL, "open", URL_MSVCR_90_INSTALL, NULL, ".", SW_SHOWNORMAL);
            else if (checkState == BST_CHECKED)
                MessageBox(hwnd, "You do not need to install the Visual C++ Runtime, because you already have the latest version.", "Not needed", MB_ICONINFORMATION);

            break;
        }
    }
}

//+---------------------------------------------------------------------------
