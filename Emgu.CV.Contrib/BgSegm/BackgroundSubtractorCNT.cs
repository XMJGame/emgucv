﻿//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.BgSegm
{
    /// <summary>
    /// Background subtraction based on counting.
    /// </summary>
    /// <remarks>About as fast as MOG2 on a high end system. More than twice faster than MOG2 on cheap hardware (benchmarked on Raspberry Pi3).</remarks>
    public class BackgroundSubtractorCNT : UnmanagedObject, IBackgroundSubtractor
    {
        private IntPtr _sharedPtr;

        private IntPtr _algorithmPtr;
        private IntPtr _backgroundSubtractorPtr;

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }

        /// <summary>
        /// Pointer to the unmanaged BackgroundSubtractor object
        /// </summary>
        public IntPtr BackgroundSubtractorPtr { get { return _backgroundSubtractorPtr; } }

        /// <summary>
        /// Creates a CNT Background Subtractor.
        /// </summary>
        /// <param name="minPixelStability">number of frames with same pixel color to consider stable</param>
        /// <param name="useHistory">determines if we're giving a pixel credit for being stable for a long time</param>
        /// <param name="maxPixelStability">maximum allowed credit for a pixel in history</param>
        /// <param name="isParallel">determines if we're parallelizing the algorithm</param>
        public BackgroundSubtractorCNT(
            int minPixelStability = 15,
            bool useHistory = true,
            int maxPixelStability = 15 * 60,
            bool isParallel = true)
        {
            _ptr = ContribInvoke.cveBackgroundSubtractorCNTCreate(
                minPixelStability,
                useHistory,
                maxPixelStability,
                isParallel,
                ref _backgroundSubtractorPtr,
                ref _algorithmPtr, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                ContribInvoke.cveBackgroundSubtractorCNTRelease(ref _ptr, ref _sharedPtr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }
}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorCNTCreate(
            int minPixelStability,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useHistory,
            int maxPixelStability,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool isParallel,
            ref IntPtr bgSubtractor,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorCNTRelease(ref IntPtr bgSubstractor, ref IntPtr sharedPtr);
    }
}
