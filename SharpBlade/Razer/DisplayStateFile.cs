﻿// ---------------------------------------------------------------------------------------
// <copyright file="DisplayStateFile.cs" company="SharpBlade">
//     Copyright © 2013-2014 by Adam Hellberg and Brandon Scott.
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy of
//     this software and associated documentation files (the "Software"), to deal in
//     the Software without restriction, including without limitation the rights to
//     use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//     of the Software, and to permit persons to whom the Software is furnished to do
//     so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//     CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//     Disclaimer: SharpBlade is in no way affiliated
//     with Razer and/or any of its employees and/or licensors.
//     Adam Hellberg does not take responsibility for any harm caused, direct
//     or indirect, to any Razer peripherals via the use of SharpBlade.
//
//     "Razer" is a trademark of Razer USA Ltd.
// </copyright>
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

using SharpBlade.Logging;

namespace SharpBlade.Razer
{
    /// <summary>
    /// Class to manage the monitoring and fixing of invalid <c>RzDisplayState</c>
    /// files generated by Razer's SDK.
    /// </summary>
    public sealed class DisplayStateFile : IDisposable
    {
        /// <summary>
        /// The name of the app running on the device.
        /// </summary>
        private readonly string _app;

        /// <summary>
        /// A value indicating whether the app is compatible with Razer's
        /// existing code for generating <c>RzDisplayState</c> files.
        /// </summary>
        private readonly bool _appCompatible;

        /// <summary>
        /// The (expected) file name of the <c>RzDisplayState</c> file.
        /// </summary>
        private readonly string _file;

        /// <summary>
        /// Log object for the <see cref="DisplayStateFile" />.
        /// </summary>
        private readonly log4net.ILog _log;

        /// <summary>
        /// The <see cref="FileSystemWatcher" /> keeping track of the <c>rzdisplaystate</c>
        /// file generation.
        /// </summary>
        private readonly FileSystemWatcher _watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayStateFile" /> class.
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "The set actions on the properties on FSW are highly unlikely to throw exceptions.")]
        internal DisplayStateFile()
        {
            Contract.Ensures(_watcher != null);
            Contract.Ensures(!string.IsNullOrEmpty(_file));

            _log = LogManager.GetLogger(this);

            _log.Debug("Initializing rzdisplaystate FileSystemWatcher");

            // Find the file name of the application
            // This seems to be the most reliable way
            // See http://stackoverflow.com/questions/616584/how-do-i-get-the-name-of-the-current-executable-in-c
            var executable = Process.GetCurrentProcess().MainModule.FileName.Replace(".vshost", string.Empty);
            _app = Path.GetFileNameWithoutExtension(executable);

            Contract.Assume(!string.IsNullOrEmpty(_app));

            // Razer's code for generating rzdisplaystate files breaks when there's a dot involved.
            _appCompatible = !_app.Contains('.');

            _file = _app + ".rzdisplaystate";

            Contract.Assume(!string.IsNullOrEmpty(_file));

            _log.InfoFormat(
                "Serving {0}:{1} (RzDisplayState compatible: {2})!",
                _app,
                _file,
                _appCompatible ? "YES" : "NO");

            _watcher = new FileSystemWatcher(Directory.GetCurrentDirectory(), _app)
            {
                NotifyFilter =
                    NotifyFilters.Attributes | NotifyFilters.CreationTime
                    | NotifyFilters.LastAccess | NotifyFilters.LastWrite
                    | NotifyFilters.Size,
                IncludeSubdirectories = false
            };

            Contract.Assert(_watcher != null);

            _watcher.Changed += (o, e) => FixDisplayStateFile();
            _watcher.Created += (o, e) => FixDisplayStateFile();
        }

        /// <summary>
        /// Gets or sets a value indicating whether SharpBlade should monitor for changes
        /// to the <c>RzDisplayState</c> file and rename it in case it's improperly named
        /// by Razer's SDK.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is only really needed when the app's executable file contains a dot in
        /// its file name (For example Foo.Bar.exe). SharpBlade will monitor for changes to
        /// files named like the app but without any extension and rename them with an
        /// <c>rzdisplaystate</c> file extension, which makes it possible for the SBUI SDK
        /// to use them as thumbnails on the device.
        /// </para>
        /// <para>
        /// Use the value from <see cref="WorkaroundRequired" /> to determine whether
        /// the workaround is needed or not.
        /// </para>
        /// </remarks>
        public bool Enabled
        {
            get
            {
                return _watcher.EnableRaisingEvents;
            }

            set
            {
                if (!WorkaroundRequired)
                {
                    _log.Warn("Tried to enable RzDisplayState monitoring on app that is already compatible, aborting.");
                    _watcher.EnableRaisingEvents = false; // Safety
                    return;
                }

                _watcher.EnableRaisingEvents = value;

                // Run an initial check if checking was enabled
                if (value)
                {
                    _log.Info("Now monitoring for invalid RzDisplayState generation.");
                    FixDisplayStateFile();
                }
                else
                    _log.Info("No longer monitoring for invalid RzDisplayState generation.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the workaround method for generating <c>RzDisplayState</c>
        /// files is required for thumbnail functionality on the device.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, the SharpBlade workaround should be enabled by setting
        /// the value of <see cref="Enabled" /> to <c>true</c>.
        /// </remarks>
        /// <example>
        /// // This will enable the SharpBlade workaround if the app is not compatible with Razer's code.
        /// RazerManager.Instance.DisplayStateFile.Enabled = RazerManager.Instance.DisplayStateFile.WorkaroundRequired;
        /// </example>
        public bool WorkaroundRequired
        {
            get { return !_appCompatible; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }

        /// <summary>
        /// Force running the workaround regardless of compatibility.
        /// </summary>
        /// <remarks>
        /// This may have unintended side-effects when run in an app that
        /// is already compatible with Razer's existing generation code.
        /// </remarks>
        public void Fix()
        {
            FixDisplayStateFile();
        }

        /// <summary>
        /// Checks for the presence of an improperly named <c>RzDisplayState</c> file
        /// and renames it with the proper <c>.rzdisplaystate</c> file extension.
        /// </summary>
        private void FixDisplayStateFile()
        {
            if (!File.Exists(_app))
                return;

            try
            {
                if (File.Exists(_file))
                    File.Delete(_file);

                File.Copy(_app, _file);
            }
            catch (IOException ex)
            {
                _log.ErrorFormat("Failed to fix RzDisplayState file, IOException: {0}", ex.Message);
            }
        }

        /// <summary>
        /// The invariant method for <see cref="DisplayStateFile" />.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_log != null);
            Contract.Invariant(_watcher != null);
            Contract.Invariant(!string.IsNullOrEmpty(_file));
        }
    }
}
