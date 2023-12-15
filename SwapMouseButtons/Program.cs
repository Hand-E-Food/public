using System.Runtime.InteropServices;

[DllImport("user32.dll")]
static extern int SwapMouseButton(int bSwap);

int rightButtonIsAlreadyPrimary = SwapMouseButton(1);
if (rightButtonIsAlreadyPrimary != 0)
    SwapMouseButton(0);
