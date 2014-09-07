﻿// ---------------------------------------------------------------------------------------
// <copyright file="FlickEventArgs.cs" company="SharpBlade">
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
//     Disclaimer: SharpBlade is in no way affiliated with Razer and/or any of
//     its employees and/or licensors. Adam Hellberg and/or Brandon Scott do not
//     take responsibility for any harm caused, direct or indirect, to any Razer
//     peripherals via the use of SharpBlade.
//
//     "Razer" is a trademark of Razer USA Ltd.
// </copyright>
// ---------------------------------------------------------------------------------------

using System;

using SharpBlade.Razer;

namespace SharpBlade.Events
{
    /// <summary>
    /// Gesture event generated by Razer API when the user
    /// has performed a flick on the touchpad.
    /// </summary>
    public class FlickEventArgs : EventArgs
    {
        /// <summary>
        /// Direction of the flick.
        /// </summary>
        private readonly Direction _direction;

        /// <summary>
        /// Number of touch points.
        /// </summary>
        private readonly uint _touchpointCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlickEventArgs" /> class.
        /// </summary>
        /// <param name="touchpointCount">Number of points touched.</param>
        /// <param name="direction">Direction of the flick.</param>
        internal FlickEventArgs(uint touchpointCount, Direction direction)
        {
            _touchpointCount = touchpointCount;
            _direction = direction;
        }

        /// <summary>
        /// Gets the direction of the flick.
        /// </summary>
        public Direction Direction
        {
            get { return _direction; }
        }

        /// <summary>
        /// Gets the number of touch points.
        /// </summary>
        [CLSCompliant(false)]
        public uint TouchpointCount
        {
            get { return _touchpointCount; }
        }
    }
}
