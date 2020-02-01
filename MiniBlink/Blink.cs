using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MiniBlink.Core;

namespace MiniBlink
{
    public sealed class Blink : ContentControl, IDisposable
    {
        readonly wkePaintUpdatedCallback paintUpdated;
        readonly wkeLoadingFinishCallback loadingFinish;
        readonly wkeNavigationCallback navigation;
        readonly wkeConsoleMessageCallback consoleMessage;
        readonly Image image;

        public EventHandler<LoadingResult> LoadingCallback;

        public IntPtr Handle { get; private set; }

        public Blink()
        {
            Background = Brushes.White;
            Focusable = true;

            paintUpdated = OnPaintUpdated;
            loadingFinish = OnLoadingFinish;
            navigation = OnNavigation;
            consoleMessage = OnConsoleMessage;

            Content = image = new Image
            {
                SnapsToDevicePixels = true,
                UseLayoutRounding = true,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.None
            };

            CreateCore();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window == null)
                return;

            window.TextInput += WindowOnTextInput;
            window.KeyDown += WindowOnKeyDown;
            window.KeyUp += WindowOnKeyUp;
        }

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window == null)
                return;

            window.TextInput -= WindowOnTextInput;
            window.KeyDown -= WindowOnKeyDown;
            window.KeyUp -= WindowOnKeyUp;
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register(nameof(Url), typeof(string), typeof(Blink), new FrameworkPropertyMetadata(default, OnUrlChanged));

        [Category(nameof(Blink))]
        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = (Blink)d;
            if (el.isCreate)
                BlinkCore.wkeLoadURLW(el.Handle, el.Url);
        }

        bool isCreate;

        void CreateCore()
        {
            Handle = BlinkCore.wkeCreateWebView();

            // Blink.wkeSetNavigationToNewWindowEnable(Handle, true); // 造成卡顿
            BlinkCore.wkeSetNpapiPluginsEnabled(Handle, true);
            BlinkCore.wkeSetUserAgentW(Handle, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36");

            BlinkCore.wkeOnPaintUpdated(Handle, paintUpdated, Handle);
            BlinkCore.wkeOnLoadingFinish(Handle, loadingFinish, Handle);
            BlinkCore.wkeOnNavigation(Handle, navigation, Handle);
            BlinkCore.wkeOnConsole(Handle, consoleMessage, Handle);

            isCreate = true;
            if (string.IsNullOrWhiteSpace(Url))
                return;
            BlinkCore.wkeLoadURLW(Handle, Url);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (Handle == IntPtr.Zero)
                return;

            BlinkCore.wkeResize(Handle, (int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
        }

        void OnPaintUpdated(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int width, int height)
        {
            if (Handle == IntPtr.Zero)
                return;

            WebBrowserRender.Render(this, Handle, hdc, x, y, width, height, image);
        }

        void OnLoadingFinish(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason)
        {
            if (Handle == IntPtr.Zero)
                return;

            if (LoadingCallback == null)
                return;

            switch (result)
            {
                case wkeLoadingResult.WKE_LOADING_SUCCEEDED:
                    LoadingCallback(this, LoadingResult.Success);
                    break;

                case wkeLoadingResult.WKE_LOADING_FAILED:
                    LoadingCallback(this, LoadingResult.Failed);
                    break;

                case wkeLoadingResult.WKE_LOADING_CANCELED:
                    LoadingCallback(this, LoadingResult.Canceled);
                    break;
            }
        }

        bool OnNavigation(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url)
        {
            var urlStr = Marshal.PtrToStringUni(BlinkCore.wkeGetStringW(url));

            Debug.WriteLine($"Navigation Url: {urlStr}");

            return true;
        }

        void OnConsoleMessage(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, int sourceLine, IntPtr stacktrace)
        {
            var source = BlinkCore.Utf8IntptrToString(BlinkCore.wkeGetString(sourceName));
            var msg = BlinkCore.Utf8IntptrToString(BlinkCore.wkeGetString(message));
            var levelStr = "";

            switch (level)
            {
                case wkeConsoleLevel.wkeLevelDebug:
                    levelStr = "Debug";
                    break;

                case wkeConsoleLevel.wkeLevelLog:
                    levelStr = "Log";
                    break;

                case wkeConsoleLevel.wkeLevelInfo:
                    levelStr = "Info";
                    break;

                case wkeConsoleLevel.wkeLevelWarning:
                    levelStr = "Warning";
                    break;

                case wkeConsoleLevel.wkeLevelError:
                    levelStr = "Error";
                    break;

                case wkeConsoleLevel.wkeLevelRevokedError:
                    levelStr = "RevokedError";
                    break;
            }

            Debug.WriteLine($"[{source} Line: {sourceLine} {levelStr}]: {msg}");
        }

        [SuppressMessage("ReSharper", "CyclomaticComplexity")]
        void RefreshCursors()
        {
            switch (BlinkCore.wkeGetCursorInfoType(Handle))
            {
                case wkeCursorInfoType.WkeCursorInfoPointer:
                    Cursor = Cursors.Arrow;

                    break;

                case wkeCursorInfoType.WkeCursorInfoCross:
                    Cursor = Cursors.Cross;
                    break;

                case wkeCursorInfoType.WkeCursorInfoHand:
                    Cursor = Cursors.Hand;
                    break;

                case wkeCursorInfoType.WkeCursorInfoIBeam:
                    Cursor = Cursors.IBeam;
                    break;

                case wkeCursorInfoType.WkeCursorInfoWait:
                    Cursor = Cursors.Arrow;
                    break;

                case wkeCursorInfoType.WkeCursorInfoHelp:
                    Cursor = Cursors.Help;
                    break;

                case wkeCursorInfoType.WkeCursorInfoEastResize:
                    Cursor = Cursors.SizeWE;
                    break;

                case wkeCursorInfoType.WkeCursorInfoNorthResize:
                    Cursor = Cursors.SizeNS;
                    break;

                case wkeCursorInfoType.WkeCursorInfoNorthEastResize:
                    Cursor = Cursors.SizeNESW;
                    break;

                case wkeCursorInfoType.WkeCursorInfoNorthWestResize:
                    Cursor = Cursors.SizeNWSE;
                    break;

                case wkeCursorInfoType.WkeCursorInfoSouthResize:
                    Cursor = Cursors.SizeWE;
                    break;

                case wkeCursorInfoType.WkeCursorInfoSouthEastResize:
                    Cursor = Cursors.SizeNWSE;
                    break;

                case wkeCursorInfoType.WkeCursorInfoSouthWestResize:
                    Cursor = Cursors.SizeNESW;
                    break;

                case wkeCursorInfoType.WkeCursorInfoWestResize:
                    Cursor = Cursors.SizeWE;
                    break;

                case wkeCursorInfoType.WkeCursorInfoNorthSouthResize:
                    Cursor = Cursors.SizeNS;
                    break;

                case wkeCursorInfoType.WkeCursorInfoEastWestResize:
                    Cursor = Cursors.SizeWE;
                    break;

                case wkeCursorInfoType.WkeCursorInfoNorthEastSouthWestResize:
                    Cursor = Cursors.SizeAll;
                    break;

                case wkeCursorInfoType.WkeCursorInfoNorthWestSouthEastResize:
                    Cursor = Cursors.SizeAll;
                    break;

                case wkeCursorInfoType.WkeCursorInfoColumnResize:
                    Cursor = Cursors.Arrow;
                    break;

                case wkeCursorInfoType.WkeCursorInfoRowResize:
                    Cursor = Cursors.Arrow;
                    break;

                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (Handle == IntPtr.Zero)
                return;

            BlinkCore.wkeSetFocus(Handle);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (Handle == IntPtr.Zero)
                return;

            BlinkCore.wkeKillFocus(Handle);
        }

        uint GetMouseFlags()
        {
            uint flags = 0;

            if (changedButton == MouseButton.Left)
                flags |= (uint)wkeMouseFlags.WKE_LBUTTON;
            if (changedButton == MouseButton.Middle)
                flags |= (uint)wkeMouseFlags.WKE_MBUTTON;
            if (changedButton == MouseButton.Right)
                flags |= (uint)wkeMouseFlags.WKE_RBUTTON;
            if (Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down || Keyboard.GetKeyStates(Key.RightCtrl) == KeyStates.Down)
                flags |= (uint)wkeMouseFlags.WKE_CONTROL;
            if (Keyboard.GetKeyStates(Key.LeftShift) == KeyStates.Down || Keyboard.GetKeyStates(Key.RightShift) == KeyStates.Down)
                flags |= (uint)wkeMouseFlags.WKE_SHIFT;

            return flags;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Handle == IntPtr.Zero)
                return;

            var flags = GetMouseFlags();
            var position = e.GetPosition(this);
            BlinkCore.wkeFireMouseEvent(Handle, 0x200, (int)position.X, (int)position.Y, flags);
            RefreshCursors();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Handle == IntPtr.Zero)
                return;

            var flags = GetMouseFlags();
            var position = e.GetPosition(this);
            BlinkCore.wkeFireMouseWheelEvent(Handle, (int)position.X, (int)position.Y, e.Delta, flags);
        }

        MouseButton? changedButton;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Mouse.Capture(this);

            changedButton = e.ChangedButton;

            if (Handle == IntPtr.Zero)
                return;

            uint msg = 0;
            if (e.ChangedButton == MouseButton.Left)
                msg = (uint)wkeMouseMessage.WKE_MSG_LBUTTONDOWN;
            if (e.ChangedButton == MouseButton.Middle)
                msg = (uint)wkeMouseMessage.WKE_MSG_MBUTTONDOWN;
            if (e.ChangedButton == MouseButton.Right)
                msg = (uint)wkeMouseMessage.WKE_MSG_RBUTTONDOWN;

            var flags = GetMouseFlags();
            var position = e.GetPosition(this);

            BlinkCore.wkeFireMouseEvent(Handle, msg, (int)position.X, (int)position.Y, flags);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            Mouse.Capture(null);

            if (Handle == IntPtr.Zero)
                return;

            uint msg = 0;
            if (e.ChangedButton == MouseButton.Left)
                msg = (uint)wkeMouseMessage.WKE_MSG_LBUTTONUP;
            if (e.ChangedButton == MouseButton.Middle)
                msg = (uint)wkeMouseMessage.WKE_MSG_MBUTTONUP;
            if (e.ChangedButton == MouseButton.Right)
                msg = (uint)wkeMouseMessage.WKE_MSG_RBUTTONUP;

            changedButton = null;

            var flags = GetMouseFlags();
            var position = e.GetPosition(this);

            BlinkCore.wkeFireMouseEvent(Handle, msg, (int)position.X, (int)position.Y, flags);
        }

        void WindowOnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Handle == IntPtr.Zero)
                return;

            foreach (var keyChar in e.Text.ToCharArray())
            {
                var result = BlinkCore.wkeFireKeyPressEvent(Handle, keyChar, 0, false);
                Debug.WriteLine($"Key {keyChar}: {result}");
            }

            Native.SetCaretPos(100, 100);
        }

        void WindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (Handle == IntPtr.Zero)
                return;

            var key = KeyInterop.VirtualKeyFromKey(e.ImeProcessedKey == Key.None ? e.Key : e.ImeProcessedKey);
            var result = BlinkCore.wkeFireKeyDownEvent(Handle, (uint)key, 0, false);

            Debug.WriteLine($"DOWN {e.Key} - {key}: {result} ");
        }

        void WindowOnKeyUp(object sender, KeyEventArgs e)
        {
            if (Handle == IntPtr.Zero)
                return;

            var key = KeyInterop.VirtualKeyFromKey(e.ImeProcessedKey == Key.None ? e.Key : e.ImeProcessedKey);
            var result = BlinkCore.wkeFireKeyUpEvent(Handle, (uint)key, 0, false);

            Debug.WriteLine($"UP {e.Key} - {key}: {result}");
        }

        ~Blink()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (Handle == IntPtr.Zero)
                return;

            BlinkCore.wkeDestroyWebView(Handle);
            Handle = IntPtr.Zero;
        }
    }
}