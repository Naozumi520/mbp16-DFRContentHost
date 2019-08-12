﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Avalonia.DfrFrameBuffer.Interop
{
    class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            string filename,
            FileAccess access,
            FileShare sharing,
            IntPtr SecurityAttributes,
            FileMode mode,
            FileOptions options,
            IntPtr template
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            IntPtr device,
            uint ctlcode,
            IntPtr inbuffer,
            int inbuffersize,
            IntPtr outbuffer,
            int outbufferSize,
            IntPtr bytesreturned,
            IntPtr overlapped
        );

        [DllImport("kernel32.dll")]
        public static extern void CloseHandle(IntPtr hdl);

        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory")]
        public unsafe static extern bool ZeroMemory(byte* destination, int length);

        [DllImport(@"C:\Users\ben\Source\Repos\DFRContentHost\x64\Debug\DfrKmBridge.dll")]
        public unsafe static extern void DfrKmBridgeTranslateBuffer(byte* FrameBufferRgba, byte* FrameBufferRgb, ulong FrameBufferSize);
    }
}
