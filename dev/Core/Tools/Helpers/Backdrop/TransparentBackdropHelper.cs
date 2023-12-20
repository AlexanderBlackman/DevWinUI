﻿using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Hosting;
using WinRT;
namespace WinUICommunity;
public class TransparentBackdropHelper : CompositionBrushBackdrop
{
    private WindowMessageMonitor? monitor;
    private Windows.UI.Composition.CompositionColorBrush? brush;

    public TransparentBackdropHelper() : this(Colors.Transparent)
    {
    }

    public TransparentBackdropHelper(Color tintColor)
    {
        _color = tintColor;
    }

    private Color _color;

    public Color TintColor
    {
        get { return _color; }
        set
        {
            _color = value;
            if (brush != null)
            {
                brush.Color = value;
            }
        }
    }

    protected override Windows.UI.Composition.CompositionBrush CreateBrush(Windows.UI.Composition.Compositor compositor)
    {
        return Compositor.CreateColorBrush(TintColor);
    }

    protected override void OnTargetConnected(ICompositionSupportsSystemBackdrop connectedTarget, XamlRoot xamlRoot)
    {
        var inspectable = connectedTarget.As<IInspectable>();
        var xamlSource = DesktopWindowXamlSource.FromAbi(inspectable.ThisPtr);
        var hWnd = xamlSource.SiteBridge.SiteView.EnvironmentView.AppWindowId.Value;

        monitor = new WindowMessageMonitor((IntPtr)hWnd);
        monitor.WindowMessageReceived += Monitor_WindowMessageReceived;

        ConfigureDwm(hWnd);

        base.OnTargetConnected(connectedTarget, xamlRoot);

        var hdc = NativeMethods.GetDC(new IntPtr((nint)hWnd));
        ClearBackground((nint)hWnd, hdc);
    }
    protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop disconnectedTarget)
    {
        monitor?.Dispose();
        monitor = null;
        var backdrop = disconnectedTarget.SystemBackdrop;
        disconnectedTarget.SystemBackdrop = null;
        backdrop?.Dispose();
        brush?.Dispose();
        brush = null;
        base.OnTargetDisconnected(disconnectedTarget);
    }

    private static void ConfigureDwm(ulong hWnd)
    {
        IntPtr handle = new IntPtr((nint)hWnd);
        var margins = new NativeValues.MARGINS(); // You may need to set appropriate values for margins

        NativeMethods.DwmExtendFrameIntoClientArea(handle, ref margins);
        var dwm = new NativeValues.DWM_BLURBEHIND()
        {
            dwFlags = NativeValues.DWM_BLURBEHIND_Mask.Enable | NativeValues.DWM_BLURBEHIND_Mask.BlurRegion,
            fEnable = true,
            hRgnBlur = NativeMethods.CreateRectRgn(-2, -2, -1, -1),
        };
        NativeMethods.DwmEnableBlurBehindWindow(handle, ref dwm);
    }

    private bool ClearBackground(nint hwnd, nint hdc)
    {
        if (NativeMethods.GetClientRect(new IntPtr(hwnd), out var rect))
        {
            var brush = NativeMethods.CreateSolidBrush(0);
            NativeMethods.FillRect(hdc, ref rect, brush);
            return true;
        }
        return false;
    }

    private void Monitor_WindowMessageReceived(object? sender, WindowMessageEventArgs e)
    {
        if (e.MessageType == NativeValues.WindowMessage.WM_ERASEBKGND)
        {
            if (ClearBackground(e.Message.Hwnd, (nint)e.Message.WParam))
            {
                e.Result = 1;
                e.Handled = true;
            }
        }
        else if ((int)e.MessageType == 798 /*WM_DWMCOMPOSITIONCHANGED*/)
        {
            ConfigureDwm((ulong)e.Message.Hwnd);
            e.Handled = true;
            e.Result = 0;
        }
    }
}
