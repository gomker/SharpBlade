﻿// ---------------------------------------------------------------------------------------
// <copyright file="WinFormsRenderer.cs" company="SharpBlade">
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
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

using SharpBlade.Razer;

namespace SharpBlade.Integration
{
    /// <summary>
    /// Renders WinForms forms.
    /// </summary>
    internal sealed class WinFormsRenderer : Renderer<RenderTarget>
    {
        /// <summary>
        /// WinForms Form to render.
        /// Null if no WinForms Form assigned.
        /// </summary>
        private readonly Form _form;

        /// <summary>
        /// Timer used to control rendering of form when
        /// poll mode is in use.
        /// </summary>
        private readonly Timer _winformTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsRenderer" /> class.
        /// For rendering a WinForms form at the specified interval.
        /// </summary>
        /// <param name="renderTarget">Render target reference.</param>
        /// <param name="form">WinForms form to render.</param>
        /// <param name="interval">The interval to render the form at,
        /// in milliseconds (MAX PRECISION = 55ms).</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "It makes no sense to dispose _winformTimer.")]
        internal WinFormsRenderer(RenderTarget renderTarget, Form form, int interval = 55)
            : base(renderTarget)
        {
            _form = form;

            _winformTimer = new Timer { Interval = interval };

            _winformTimer.Tick += WinformTimerOnTick;

            _winformTimer.Start();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_winformTimer != null)
                _winformTimer.Dispose();
        }

        /// <summary>
        /// Callback for the tick event on the WinForms render timer.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void WinformTimerOnTick(object sender, EventArgs e)
        {
            RenderTarget.DrawForm(_form);
        }
    }
}
