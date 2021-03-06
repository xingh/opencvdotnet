using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using OpenCVDotNet.Native;

namespace OpenCVDotNet
{
    public class CVUtils
	{
	
        /// <summary>
        /// Convert a string to a char pointer
        /// </summary>
        /// <param name="str"></param>
        /// <param name="output"></param>
        /// <param name="size"></param>
        [Obsolete("Use MarshalAsAttribute instead")]
        unsafe static void StringToCharPointer(string str, char* output, int size)
        {
            throw new NotImplementedException("Use Marshal as string");
            //unsafe
            //{
            //    output = &str.ToCharArray()[0];
            //}
                //char* p = PtrToStringChars(str);
                //wcstombs_s(NULL, output, size, p, size);
        }

		public static int Round(double val)
		{
			//return cvRound(val);
            return (int)Math.Round(val);
		}

        

        [ThreadStatic]
        internal static CvErrorContext _errorContext;

        static int ErrorHandler(
            int status,
            [MarshalAs(UnmanagedType.LPStr)]string func_name,
            [MarshalAs(UnmanagedType.LPStr)]string err_msg,
            [MarshalAs(UnmanagedType.LPStr)]string file_name,
            int line,
            [MarshalAs(UnmanagedType.LPStr)]string userdata)
        {
            _errorContext = new CvErrorContext(err_msg, line, file_name, status, userdata);
            return 0; // if this value is non-zero the program is terminated
        }

        internal static void CheckLastError()
        {
            if (_errorContext != null)
            {
                CVException cve = new CVException(_errorContext);
                _errorContext = null;
                throw cve;
            }
        }
       
        private static PInvoke.__CvErrorCallback _errHandler = ErrorHandler;

        public static void ErrorsToExceptions()
        {
            PInvoke.cvRedirectError(_errHandler);
        }

		internal static System.Drawing.Color ScalarToColor(__CvScalar scalar)
		{
			return System.Drawing.Color.FromArgb((int) scalar.v2, (int) scalar.v1, (int) scalar.v0);
		}
    }
}
