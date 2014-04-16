﻿// <auto-generated>
// This code was "auto-generated" from pinvoke.net and MSDN.
// (Doesn't really count as auto-generated but we circumvent StyleCop this way)
// </auto-generated>

using System;
using System.Runtime.InteropServices;

namespace Sharparam.SharpBlade.Native.WinAPI
{
    /// <summary>
    /// Static class containing all functions
    /// provided by the Windows Kernel32 library.
    /// </summary>
    public static class Kernel32
    {
        /// <summary>
        /// The code page recommended for consoles spawned with <see cref="NativeMethods.AllocConsole" />.
        /// </summary>
        public const int CODE_PAGE = 437;

        /// <summary>
        /// The standard error device. Initially, this is the active console screen buffer, CONOUT$.
        /// </summary>
        public const int STD_ERROR_HANDLE = -12;

        /// <summary>
        /// The standard input device. Initially, this is the console input buffer, CONIN$.
        /// </summary>
        public const int STD_INPUT_HANDLE = -10;

        /// <summary>
        /// The standard output device. Initially, this is the active console screen buffer, CONOUT$.
        /// </summary>
        public const int STD_OUTPUT_HANDLE = -11;

        /// <summary>
        /// Native methods for Kernel32.
        /// </summary>
        internal static class NativeMethods
        {
            /// <summary>
            /// DLL file to import functions from.
            /// </summary>
            private const string DllName = "kernel32.dll";

            /// <summary>
            /// Allocates a new console for the calling process.
            /// </summary>
            /// <returns>
            /// If the function succeeds, the return value is nonzero (true).
            /// If the function fails, the return value is zero (false).
            /// To get extended error information, call <c>GetLastError</c>.
            /// </returns>
            /// <remarks>
            /// A process can be associated with only one console,
            /// so the <c>AllocConsole</c> function fails if the calling process already has a console.
            /// A process can use the <c>FreeConsole</c> function to detach itself from its current console,
            /// then it can call <c>AllocConsole</c> to create a new console or AttachConsole to attach to another console.
            /// If the calling process creates a child process, the child inherits the new console.
            /// <c>AllocConsole</c> initializes standard input, standard output,
            /// and standard error handles for the new console.
            /// The standard input handle is a handle to the console's input buffer,
            /// and the standard output and standard error handles are handles to the console's screen buffer.
            /// To retrieve these handles, use the <c>GetStdHandle</c> function.
            /// This function is primarily used by graphical user interface (GUI) application to create a console window.
            /// GUI applications are initialized without a console. Console applications are initialized with a console,
            /// unless they are created as detached processes (by calling the <c>CreateProcess</c> function with the <c>DETACHED_PROCESS</c> flag).
            /// </remarks>
            [DllImport(DllName, EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall)]
            internal static extern bool AllocConsole();

            /// <summary>
            /// Detaches the calling process from its console.
            /// </summary>
            /// <returns>
            /// If the function succeeds, the return value is nonzero (true).
            /// If the function fails, the return value is zero (false).
            /// To get extended error information, call <c>GetLastError</c>.
            /// </returns>
            /// <remarks>
            /// A process can be attached to at most one console.
            /// If the calling process is not already attached to a console,
            /// the error code returned is <c>ERROR_INVALID_PARAMETER</c> (87).
            /// A process can use the <c>FreeConsole</c> function to detach itself from its console.
            /// If other processes share the console, the console is not destroyed,
            /// but the process that called <c>FreeConsole</c> cannot refer to it.
            /// A console is closed when the last process attached to it terminates or calls <c>FreeConsole</c>.
            /// After a process calls <c>FreeConsole</c>,
            /// it can call the <c>AllocConsole</c> function to create a new console or <c>AttachConsole</c> to attach to another console.
            /// </remarks>
            [DllImport(DllName, EntryPoint = "FreeConsole", SetLastError = true, CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall)]
            internal static extern int FreeConsole();

            /// <summary>
            /// Retrieves a handle to the specified standard device (standard input, standard output, or standard error).
            /// </summary>
            /// <param name="nStdHandle">The standard device.</param>
            /// <returns>
            /// If the function succeeds, the return value is a handle to the specified device,
            /// or a redirected handle set by a previous call to <c>SetStdHandle</c>.
            /// The handle has <c>GENERIC_READ</c> and <c>GENERIC_WRITE</c> access rights,
            /// unless the application has used <c>SetStdHandle</c> to set a standard handle with lesser access.
            /// If the function fails, the return value is <c>INVALID_HANDLE_VALUE</c>.
            /// To get extended error information, call <c>GetLastError</c>.
            /// If an application does not have associated standard handles,
            /// such as a service running on an interactive desktop,
            /// and has not redirected them, the return value is <c>NULL</c>.
            /// </returns>
            [DllImport(DllName, EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto,
                CallingConvention = CallingConvention.StdCall)]
            internal static extern IntPtr GetStdHandle(int nStdHandle);
        }
    }
}
