using System;
using System.Runtime.InteropServices;
using System.Text;

namespace wyDay.TurboActivate
{
    [Flags]
    public enum TA_Flags : uint
    {
        TA_SYSTEM = 1,
        TA_USER = 2,

        /// <summary>
        /// Use the TA_DISALLOW_VM in UseTrial() to disallow trials in virtual machines. 
        /// If you use this flag in UseTrial() and the customer's machine is a Virtual
        /// Machine, then UseTrial() will throw VirtualMachineException.
        /// </summary>
        TA_DISALLOW_VM = 4,

        /// <summary>
        /// Use this flag in TA_UseTrial() to tell TurboActivate to use client-side
        /// unverified trials. For more information about verified vs. unverified trials,
        /// see here: https://wyday.com/limelm/help/trials/
        /// Note: unverified trials are unsecured and can be reset by malicious customers.
        /// </summary>
        TA_UNVERIFIED_TRIAL = 16,

        /// <summary>
        /// Use the TA_VERIFIED_TRIAL flag to use verified trials instead
        /// of unverified trials. This means the trial is locked to a particular computer.
        /// The customer can't reset the trial.
        /// </summary>
        TA_VERIFIED_TRIAL = 32
    }

    [Flags]
    public enum TA_DateCheckFlags : uint
    {
        TA_HAS_NOT_EXPIRED = 1,
    }


    public class TurboActivate
    {
        readonly string versGUID;
        readonly UInt32 handle = 0;

        /// <summary>The GUID for this product version. This is found on the LimeLM site on the version overview.</summary>
        public string VersionGUID
        {
            get { return versGUID; }
        }

        /// <summary>Creates a TurboActivate object instance.</summary>
        /// <param name="vGUID">The GUID for this product version. This is found on the LimeLM site on the version overview.</param>
        /// <param name="pdetsFilename">The absolute location to the TurboActivate.dat file on the disk.</param>
        public TurboActivate(string vGUID, string pdetsFilename = null)
        {
            if (pdetsFilename != null)
            {
#if TA_BOTH_DLL
                switch (IntPtr.Size == 8 ? Native64.TA_PDetsFromPath(pdetsFilename) : Native.TA_PDetsFromPath(pdetsFilename))
#else
                switch (Native.TA_PDetsFromPath(pdetsFilename))
#endif
                {
                    case TA_OK: // successful
                        break;
                    case TA_E_PDETS:
                        throw new ProductDetailsException();
                    case TA_FAIL:
                        // the TurboActivate.dat already loaded.
                        break;
                    default:
                        throw new TurboActivateException("The TurboActivate.dat file failed to load.");
                }
            }

            versGUID = vGUID;

#if TA_BOTH_DLL
            handle = IntPtr.Size == 8 ? Native64.TA_GetHandle(versGUID) : Native.TA_GetHandle(versGUID);
#else
            handle = Native.TA_GetHandle(versGUID);
#endif

            // if the handle is still unset then immediately throw an exception
            // telling the user that they need to actually load the correct
            // TurboActivate.dat and/or use the correct GUID for the TurboActivate.dat
            if (handle == 0)
                throw new ProductDetailsException();
        }

        /// <summary>Creates a TurboActivate object instance.</summary>
        /// <param name="vGUID">The GUID for this product version. This is found on the LimeLM site on the version overview.</param>
        /// <param name="pdetsData">The TurboActivate.dat file loaded into a byte array.</param>
        public TurboActivate(string vGUID, byte[] pdetsData)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_PDetsFromByteArray(pdetsData, pdetsData.Length) : Native.TA_PDetsFromByteArray(pdetsData, pdetsData.Length))
#else
            switch (Native.TA_PDetsFromByteArray(pdetsData, pdetsData.Length))
#endif
            {
                case TA_OK: // successful
                    break;
                case TA_E_PDETS:
                    throw new ProductDetailsException();
                case TA_FAIL:
                    // the TurboActivate.dat already loaded.
                    break;
                default:
                    throw new TurboActivateException("The TurboActivate.dat file failed to load.");
            }
            

            versGUID = vGUID;

#if TA_BOTH_DLL
            handle = IntPtr.Size == 8 ? Native64.TA_GetHandle(versGUID) : Native.TA_GetHandle(versGUID);
#else
            handle = Native.TA_GetHandle(versGUID);
#endif

            // if the handle is still unset then immediately throw an exception
            // telling the user that they need to actually load the correct
            // TurboActivate.dat and/or use the correct GUID for the TurboActivate.dat
            if (handle == 0)
                throw new ProductDetailsException();
        }


        static class Native
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct ACTIVATE_OPTIONS
            {
                public UInt32 nLength;

                [MarshalAs(UnmanagedType.LPWStr)]
                public string sExtraData;
            }

            [Flags]
            public enum GenuineFlags : uint
            {
                TA_SKIP_OFFLINE = 1,
                TA_OFFLINE_SHOW_INET_ERR = 2
            };

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct GENUINE_OPTIONS
            {
                public UInt32 nLength;
                public GenuineFlags flags;
                public UInt32 nDaysBetweenChecks;
                public UInt32 nGraceDaysOnInetErr;
            }

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt32 TA_GetHandle(string versionGUID);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_Activate(UInt32 handle, ref ACTIVATE_OPTIONS options);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_Activate(UInt32 handle, IntPtr options);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ActivationRequestToFile(UInt32 handle, string filename, ref ACTIVATE_OPTIONS options);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ActivationRequestToFile(UInt32 handle, string filename, IntPtr options);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ActivateFromFile(UInt32 handle, string filename);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_CheckAndSavePKey(UInt32 handle, string productKey, TA_Flags flags);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_Deactivate(UInt32 handle, byte erasePkey);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_DeactivationRequestToFile(UInt32 handle, string filename, byte erasePkey);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GetExtraData(UInt32 handle, StringBuilder lpValueStr, int cchValue);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GetFeatureValue(UInt32 handle, string featureName, StringBuilder lpValueStr, int cchValue);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GetPKey(UInt32 handle, StringBuilder lpPKeyStr, int cchPKey);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsActivated(UInt32 handle);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsDateValid(UInt32 handle, string date_time, TA_DateCheckFlags flags);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsGenuine(UInt32 handle);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsGenuineEx(UInt32 handle, ref GENUINE_OPTIONS options);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GenuineDays(UInt32 handle, uint nDaysBetweenChecks, uint nGraceDaysOnInetErr, ref uint DaysRemaining, ref char inGracePeriod);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsProductKeyValid(UInt32 handle);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_SetCustomProxy(string proxy);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_TrialDaysRemaining(UInt32 handle, TA_Flags useTrialFlags, ref uint DaysRemaining);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_UseTrial(UInt32 handle, TA_Flags flags, string extra_data);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_UseTrialVerifiedRequest(UInt32 handle, string filename, string extra_data);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_UseTrialVerifiedFromFile(UInt32 handle, string filename, TA_Flags flags);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ExtendTrial(UInt32 handle, TA_Flags flags, string trialExtension);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_PDetsFromPath(string filename);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_PDetsFromByteArray(byte[] pArray, int nSize);

            [DllImport("TurboActivate.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_SetCustomActDataPath(UInt32 handle, string directory);
        }

        /*
         To use "AnyCPU" Target CPU type, first copy the x64 TurboActivate.dll and rename to TurboActivate64.dll
         Then in your project properties go to the Build panel, and add the TA_BOTH_DLL conditional compilation symbol.
        */

#if TA_BOTH_DLL
        static class Native64
        {
            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern UInt32 TA_GetHandle(string versionGUID);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_Activate(UInt32 handle, ref Native.ACTIVATE_OPTIONS options);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_Activate(UInt32 handle, IntPtr options);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ActivationRequestToFile(UInt32 handle, string filename, ref Native.ACTIVATE_OPTIONS options);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ActivationRequestToFile(UInt32 handle, string filename, IntPtr options);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ActivateFromFile(UInt32 handle, string filename);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_CheckAndSavePKey(UInt32 handle, string productKey, TA_Flags flags);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_Deactivate(UInt32 handle, byte erasePkey);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_DeactivationRequestToFile(UInt32 handle, string filename, byte erasePkey);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GetExtraData(UInt32 handle, StringBuilder lpValueStr, int cchValue);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GetFeatureValue(UInt32 handle, string featureName, StringBuilder lpValueStr, int cchValue);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GetPKey(UInt32 handle, StringBuilder lpPKeyStr, int cchPKey);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsActivated(UInt32 handle);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsDateValid(UInt32 handle, string date_time, TA_DateCheckFlags flags);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsGenuine(UInt32 handle);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsGenuineEx(UInt32 handle, ref Native.GENUINE_OPTIONS options);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_GenuineDays(UInt32 handle, uint nDaysBetweenChecks, uint nGraceDaysOnInetErr, ref uint DaysRemaining, ref char inGracePeriod);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_IsProductKeyValid(UInt32 handle);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_SetCustomProxy(string proxy);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_TrialDaysRemaining(UInt32 handle, TA_Flags useTrialFlags, ref uint DaysRemaining);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_UseTrial(UInt32 handle, TA_Flags flags, string extra_data);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_UseTrialVerifiedRequest(UInt32 handle, string filename, string extra_data);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_UseTrialVerifiedFromFile(UInt32 handle, string filename, TA_Flags flags);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_ExtendTrial(UInt32 handle, TA_Flags flags, string trialExtension);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_PDetsFromPath(string filename);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_PDetsFromByteArray(byte[] pArray, int nSize);

            [DllImport("TurboActivate64.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TA_SetCustomActDataPath(UInt32 handle, string directory);
        }
#endif

        const int TA_OK = 0x00;
        const int TA_FAIL = 0x01;
        const int TA_E_PKEY = 0x02;
        const int TA_E_ACTIVATE = 0x03;
        const int TA_E_INET = 0x04;
        const int TA_E_INUSE = 0x05;
        const int TA_E_REVOKED = 0x06;
        const int TA_E_PDETS = 0x08;
        const int TA_E_TRIAL = 0x09;
        const int TA_E_COM = 0x0B;
        const int TA_E_TRIAL_EUSED = 0x0C;
        const int TA_E_TRIAL_EEXP = 0x0D;
        const int TA_E_EXPIRED = 0x0D;
        const int TA_E_INSUFFICIENT_BUFFER = 0x0E;
        const int TA_E_PERMISSION = 0x0F;
        const int TA_E_INVALID_FLAGS = 0x10;
        const int TA_E_IN_VM = 0x11;
        const int TA_E_EDATA_LONG = 0x12;
        const int TA_E_INVALID_ARGS = 0x13;
        const int TA_E_KEY_FOR_TURBOFLOAT = 0x14;
        const int TA_E_INET_DELAYED = 0x15;
        const int TA_E_FEATURES_CHANGED = 0x16;
        const int TA_E_NO_MORE_DEACTIVATIONS = 0x18;
        const int TA_E_ACCOUNT_CANCELED = 0x19;
        const int TA_E_ALREADY_ACTIVATED = 0x1A;
        const int TA_E_INVALID_HANDLE = 0x1B;
        const int TA_E_ENABLE_NETWORK_ADAPTERS = 0x1C;
        const int TA_E_ALREADY_VERIFIED_TRIAL = 0x1D;
        const int TA_E_TRIAL_EXPIRED = 0x1E;
        const int TA_E_MUST_SPECIFY_TRIAL_TYPE = 0x1F;
        const int TA_E_MUST_USE_TRIAL = 0x20;
        const int TA_E_NO_MORE_TRIALS_ALLOWED = 0x21;


        /// <summary>Activates the product on this computer. You must call <see cref="CheckAndSavePKey(string)"/> with a valid product key or have used the TurboActivate wizard sometime before calling this function.</summary>
        /// <param name="extraData">Extra data to pass to the LimeLM servers that will be visible for you to see and use. Maximum size is 255 UTF-8 characters.</param>
        public void Activate(string extraData = null)
        {
            int ret;

            if (extraData != null)
            {
                Native.ACTIVATE_OPTIONS opts = new Native.ACTIVATE_OPTIONS {sExtraData = extraData};
                opts.nLength = (uint)Marshal.SizeOf(opts);

#if TA_BOTH_DLL
                ret = IntPtr.Size == 8 ? Native64.TA_Activate(handle, ref opts) : Native.TA_Activate(handle, ref opts);
#else
                ret = Native.TA_Activate(handle, ref opts);
#endif
            }
            else
            {
#if TA_BOTH_DLL
                ret = IntPtr.Size == 8 ? Native64.TA_Activate(handle, IntPtr.Zero) : Native.TA_Activate(handle, IntPtr.Zero);
#else
                ret = Native.TA_Activate(handle, IntPtr.Zero);
#endif
            }

            switch (ret)
            {
                case TA_E_PKEY:
                    throw new InvalidProductKeyException();
                case TA_E_INET:
                    throw new InternetException();
                case TA_E_INUSE:
                    throw new PkeyMaxUsedException();
                case TA_E_REVOKED:
                    throw new PkeyRevokedException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_EXPIRED:
                    throw new DateTimeException(false);
                case TA_E_IN_VM:
                    throw new VirtualMachineException();
                case TA_E_EDATA_LONG:
                    throw new ExtraDataTooLongException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_E_KEY_FOR_TURBOFLOAT:
                    throw new TurboFloatKeyException();
                case TA_E_ACCOUNT_CANCELED:
                    throw new AccountCanceledException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to activate.");
            }
        }

        /// <summary>Get the "activation request" file for offline activation. You must call <see cref="CheckAndSavePKey(string)"/> with a valid product key or have used the TurboActivate wizard sometime before calling this function.</summary>
        /// <param name="filename">The location where you want to save the activation request file.</param>
        /// <param name="extraData">Extra data to pass to the LimeLM servers that will be visible for you to see and use. Maximum size is 255 UTF-8 characters.</param>
        public void ActivationRequestToFile(string filename, string extraData)
        {
            int ret;

            if (extraData != null)
            {
                Native.ACTIVATE_OPTIONS opts = new Native.ACTIVATE_OPTIONS { sExtraData = extraData };
                opts.nLength = (uint)Marshal.SizeOf(opts);

#if TA_BOTH_DLL
                ret = IntPtr.Size == 8 ? Native64.TA_ActivationRequestToFile(handle, filename, ref opts) : Native.TA_ActivationRequestToFile(handle, filename, ref opts);
#else
                ret = Native.TA_ActivationRequestToFile(handle, filename, ref opts);
#endif
            }
            else
            {
#if TA_BOTH_DLL
                ret = IntPtr.Size == 8 ? Native64.TA_ActivationRequestToFile(handle, filename, IntPtr.Zero) : Native.TA_ActivationRequestToFile(handle, filename, IntPtr.Zero);
#else
                ret = Native.TA_ActivationRequestToFile(handle, filename, IntPtr.Zero);
#endif
            }

            switch (ret)
            {
                case TA_E_PKEY:
                    throw new InvalidProductKeyException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_EDATA_LONG:
                    throw new ExtraDataTooLongException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to save the activation request file.");
            }
        }

        /// <summary>Activate from the "activation response" file for offline activation.</summary>
        /// <param name="filename">The location of the activation response file.</param>
        public void ActivateFromFile(string filename)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_ActivateFromFile(handle, filename) : Native.TA_ActivateFromFile(handle, filename))
#else
            switch (Native.TA_ActivateFromFile(handle, filename))
#endif
            {
                case TA_E_PKEY:
                    throw new InvalidProductKeyException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_EXPIRED:
                    throw new DateTimeException(true);
                case TA_E_IN_VM:
                    throw new VirtualMachineException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to activate.");
            }
        }

        /// <summary>Checks and saves the product key.</summary>
        /// <param name="productKey">The product key you want to save.</param>
        /// <param name="flags">Whether to create the activation either user-wide or system-wide. Valid flags are <see cref="TA_Flags.TA_SYSTEM"/> and <see cref="TA_Flags.TA_USER"/>.</param>
        /// <returns>True if the product key is valid, false if it's not</returns>
        public bool CheckAndSavePKey(string productKey, TA_Flags flags = TA_Flags.TA_SYSTEM)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_CheckAndSavePKey(handle, productKey, flags) : Native.TA_CheckAndSavePKey(handle, productKey, flags))
#else
            switch (Native.TA_CheckAndSavePKey(handle, productKey, flags))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // successful
                    return true;
                case TA_E_PERMISSION:
                    throw new PermissionException();
                case TA_E_INVALID_FLAGS:
                    throw new InvalidFlagsException();
                case TA_E_ALREADY_ACTIVATED:
                    throw new AlreadyActivatedException();
                default:
                    return false;
            }
        }

        /// <summary>Deactivates the product on this computer.</summary>
        /// <param name="eraseProductKey">Erase the product key so the user will have to enter a new product key if they wish to reactivate.</param>
        public void Deactivate(bool eraseProductKey = false)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_Deactivate(handle, (byte)(eraseProductKey ? 1 : 0)) : Native.TA_Deactivate(handle, (byte)(eraseProductKey ? 1 : 0)))
#else
            switch (Native.TA_Deactivate(handle, (byte)(eraseProductKey ? 1 : 0)))
    #endif
            {
                case TA_E_PKEY:
                    throw new InvalidProductKeyException();
                case TA_E_ACTIVATE:
                    throw new NotActivatedException();
                case TA_E_INET:
                    throw new InternetException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_NO_MORE_DEACTIVATIONS:
                    throw new NoMoreDeactivationsException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to deactivate.");
            }
        }

        /// <summary>Get the "deactivation request" file for offline deactivation.</summary>
        /// <param name="filename">The location where you want to save the deactivation request file.</param>
        /// <param name="eraseProductKey">Erase the product key so the user will have to enter a new product key if they wish to reactivate.</param>
        public void DeactivationRequestToFile(string filename, bool eraseProductKey = false)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_DeactivationRequestToFile(handle, filename, (byte)(eraseProductKey ? 1 : 0)) : Native.TA_DeactivationRequestToFile(handle, filename, (byte)(eraseProductKey ? 1 : 0)))
#else
            switch (Native.TA_DeactivationRequestToFile(handle, filename, (byte)(eraseProductKey ? 1 : 0)))
#endif
            {
                case TA_E_PKEY:
                    throw new InvalidProductKeyException();
                case TA_E_ACTIVATE:
                    throw new NotActivatedException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to deactivate.");
            }
        }

        /// <summary>Gets the extra data value you passed in when activating.</summary>
        /// <returns>Returns the extra data if it exists, otherwise it returns null.</returns>
        public string GetExtraData()
        {
#if TA_BOTH_DLL
            int length = IntPtr.Size == 8 ? Native64.TA_GetExtraData(handle, null, 0) : Native.TA_GetExtraData(handle, null, 0);
#else
            int length = Native.TA_GetExtraData(handle, null, 0);
#endif

            StringBuilder sb = new StringBuilder(length);

#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_GetExtraData(handle, sb, length) : Native.TA_GetExtraData(handle, sb, length))
#else
            switch (Native.TA_GetExtraData(handle, sb, length))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // success
                    return sb.ToString();
                default:
                    return null;
            }
        }

        /// <summary>Gets the value of a feature.</summary>
        /// <param name="featureName">The name of the feature to retrieve the value for.</param>
        /// <returns>Returns the feature value.</returns>
        public string GetFeatureValue(string featureName)
        {
            string value = GetFeatureValue(featureName, null);

            if (value == null)
                throw new TurboActivateException("Failed to get feature value. The feature doesn't exist.");

            return value;
        }

        /// <summary>Gets the value of a feature.</summary>
        /// <param name="featureName">The name of the feature to retrieve the value for.</param>
        /// <param name="defaultValue">The default value to return if the feature doesn't exist.</param>
        /// <returns>Returns the feature value if it exists, otherwise it returns the default value.</returns>
        public string GetFeatureValue(string featureName, string defaultValue)
        {
#if TA_BOTH_DLL
            int length = IntPtr.Size == 8 ? Native64.TA_GetFeatureValue(handle, featureName, null, 0) : Native.TA_GetFeatureValue(handle, featureName, null, 0);
#else
            int length = Native.TA_GetFeatureValue(handle, featureName, null, 0);
#endif

            StringBuilder sb = new StringBuilder(length);

#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_GetFeatureValue(handle, featureName, sb, length) : Native.TA_GetFeatureValue(handle, featureName, sb, length))
#else
            switch (Native.TA_GetFeatureValue(handle, featureName, sb, length))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // success
                    return sb.ToString();
                default:
                    return defaultValue;
            }
        }

        /// <summary>Gets the stored product key. NOTE: if you want to check if a product key is valid simply call <see cref="IsProductKeyValid()"/>.</summary>
        /// <returns>string Product key.</returns>
        public string GetPKey()
        {
            // this makes the assumption that the PKey is 34+NULL characters long.
            // This may or may not be true in the future.
            StringBuilder sb = new StringBuilder(35);

#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_GetPKey(handle, sb, 35) : Native.TA_GetPKey(handle, sb, 35))
#else
            switch (Native.TA_GetPKey(handle, sb, 35))
#endif
            {
                case TA_E_PKEY:
                    throw new InvalidProductKeyException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // success
                    return sb.ToString();
                default:
                    throw new TurboActivateException("Failed to get the product key.");
            }
        }

        /// <summary>Checks whether the computer has been activated.</summary>
        /// <returns>True if the computer is activated. False otherwise.</returns>
        public bool IsActivated()
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_IsActivated(handle) : Native.TA_IsActivated(handle))
#else
            switch (Native.TA_IsActivated(handle))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_IN_VM:
                    throw new VirtualMachineException();
                case TA_OK: // is activated
                    return true;
            }

            return false;
        }

        /// <summary>Checks if the string in the form "YYYY-MM-DD HH:mm:ss" is a valid date/time. The date must be in UTC time and "24-hour" format. If your date is in some other time format first convert it to UTC time before passing it into this function.</summary>
        /// <param name="date_time">The date time string to check.</param>
        /// <param name="flags">The type of date time check. Valid flags are <see cref="TA_DateCheckFlags.TA_HAS_NOT_EXPIRED"/>.</param>
        /// <returns>True if the date is valid, false if it's not</returns>
        public bool IsDateValid(string date_time, TA_DateCheckFlags flags)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_IsDateValid(handle, date_time, flags) : Native.TA_IsDateValid(handle, date_time, flags))
#else
            switch (Native.TA_IsDateValid(handle, date_time, flags))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // successful
                    return true;
                case TA_E_INVALID_FLAGS:
                    throw new InvalidFlagsException();
                default:
                    return false;
            }
        }

        /// <summary>Checks whether the computer is genuinely activated by verifying with the LimeLM servers.</summary>
        /// <returns>IsGenuineResult</returns>
        public IsGenuineResult IsGenuine()
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_IsGenuine(handle) : Native.TA_IsGenuine(handle))
#else
            switch (Native.TA_IsGenuine(handle))
#endif
            {
                case TA_E_INET:
                    return IsGenuineResult.InternetError;
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_EXPIRED:
                    throw new DateTimeException(false);
                case TA_E_IN_VM:
                    return IsGenuineResult.NotGenuineInVM;
                case TA_E_FEATURES_CHANGED:
                    return IsGenuineResult.GenuineFeaturesChanged;
                case TA_OK: // is activated
                    return IsGenuineResult.Genuine;
            }

            // not genuine (TA_FAIL, TA_E_REVOKED, TA_E_ACTIVATE)
            return IsGenuineResult.NotGenuine;
        }

        /// <summary>Checks whether the computer is activated, and every "daysBetweenChecks" days it check if the customer is genuinely activated by verifying with the LimeLM servers.</summary>
        /// <param name="daysBetweenChecks">How often to contact the LimeLM servers for validation. 90 days recommended.</param>
        /// <param name="graceDaysOnInetErr">If the call fails because of an internet error, how long, in days, should the grace period last (before returning deactivating and returning TA_FAIL).
        /// 
        /// 14 days is recommended.</param>
        /// <param name="skipOffline">If the user activated using offline activation 
        /// (ActivateRequestToFile(), ActivateFromFile() ), then with this
        /// option IsGenuineEx() will still try to validate with the LimeLM
        /// servers, however instead of returning <see cref="IsGenuineResult.InternetError"/> (when within the
        /// grace period) or <see cref="IsGenuineResult.NotGenuine"/> (when past the grace period) it will
        /// instead only return <see cref="IsGenuineResult.Genuine"/> (if IsActivated()).
        /// 
        /// If the user activated using online activation then this option
        /// is ignored.</param>
        /// <param name="offlineShowInetErr">If the user activated using offline activation, and you're
        /// using this option in tandem with skipOffline, then IsGenuineEx()
        /// will return <see cref="IsGenuineResult.InternetError"/> on internet failure instead of <see cref="IsGenuineResult.Genuine"/>.
        ///
        /// If the user activated using online activation then this flag
        /// is ignored.</param>
        /// <returns>IsGenuineResult</returns>
        public IsGenuineResult IsGenuine(uint daysBetweenChecks, uint graceDaysOnInetErr, bool skipOffline = false, bool offlineShowInetErr = false)
        {
            Native.GENUINE_OPTIONS opts = new Native.GENUINE_OPTIONS { nDaysBetweenChecks = daysBetweenChecks, nGraceDaysOnInetErr = graceDaysOnInetErr, flags = 0 };
            opts.nLength = (uint)Marshal.SizeOf(opts);

            if (skipOffline)
            {
                opts.flags |= Native.GenuineFlags.TA_SKIP_OFFLINE;

                if (offlineShowInetErr)
                    opts.flags |= Native.GenuineFlags.TA_OFFLINE_SHOW_INET_ERR;
            }

#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_IsGenuineEx(handle, ref opts) : Native.TA_IsGenuineEx(handle, ref opts))
#else
            switch (Native.TA_IsGenuineEx(handle, ref opts))
#endif
            {
                case TA_E_INET:
                case TA_E_INET_DELAYED:
                    return IsGenuineResult.InternetError;
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_EXPIRED:
                    throw new DateTimeException(false);
                case TA_E_IN_VM:
                    return IsGenuineResult.NotGenuineInVM;
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_E_FEATURES_CHANGED:
                    return IsGenuineResult.GenuineFeaturesChanged;
                case TA_OK: // is activated and/or Genuine
                    return IsGenuineResult.Genuine;
            }

            // not genuine (TA_FAIL, TA_E_REVOKED, TA_E_ACTIVATE)
            return IsGenuineResult.NotGenuine;
        }

        /// <summary>Get the number of days until the next time that the <see cref="IsGenuine(uint, uint, bool, bool)"/> function contacts the LimeLM activation servers to reverify the activation.</summary>
        /// <param name="daysBetweenChecks">How often to contact the LimeLM servers for validation. Use the exact same value as used in <see cref="IsGenuine(uint, uint, bool, bool)"/>.</param>
        /// <param name="graceDaysOnInetErr">If the call fails because of an internet error, how long, in days, should the grace period last (before returning deactivating and returning TA_FAIL). Again, use the exact same value as used in <see cref="IsGenuine(uint, uint, bool, bool)"/>.</param>
        /// <param name="inGracePeriod">Get whether the user is in the grace period.</param>
        /// <returns>The number of days remaining. 0 days if both the days between checks and the grace period have expired. (E.g. 1 day means *at most* 1 day. That is, it could be 30 seconds.)</returns>
        public uint GenuineDays(uint daysBetweenChecks, uint graceDaysOnInetErr, ref bool inGracePeriod)
        {
            uint daysRemain = 0;
            char inGrace = (char)0;

#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_GenuineDays(handle, daysBetweenChecks, graceDaysOnInetErr, ref daysRemain, ref inGrace) : Native.TA_GenuineDays(handle, daysBetweenChecks, graceDaysOnInetErr, ref daysRemain, ref inGrace))
#else
            switch (Native.TA_GenuineDays(handle, daysBetweenChecks, graceDaysOnInetErr, ref daysRemain, ref inGrace))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_ACTIVATE:
                    throw new NotActivatedException();
                case TA_OK: // successful
                    break;
                default:
                    throw new TurboActivateException("Failed to get the genuine days.");
            }

            // set whether we're in a grace period or not
            inGracePeriod = inGrace == (char) 1;

            return daysRemain;
        }

        /// <summary>Checks if the product key installed for this product is valid. This does NOT check if the product key is activated or genuine. Use <see cref="IsActivated()"/> and <see cref="IsGenuine(ref bool)"/> instead.</summary>
        /// <returns>True if the product key is valid.</returns>
        public bool IsProductKeyValid()
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_IsProductKeyValid(handle) : Native.TA_IsProductKeyValid(handle))
#else
            switch (Native.TA_IsProductKeyValid(handle))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // is valid
                    return true;
            }

            // not valid
            return false;
        }

        /// <summary>Sets the custom proxy to be used by functions that connect to the internet.</summary>
        /// <param name="proxy">The proxy to use. Proxy must be in the form "http://username:password@host:port/".</param>
        public static void SetCustomProxy(string proxy)
        {
#if TA_BOTH_DLL
            if ((IntPtr.Size == 8 ? Native64.TA_SetCustomProxy(proxy) : Native.TA_SetCustomProxy(proxy)) != 0)
#else
            if (Native.TA_SetCustomProxy(proxy) != TA_OK)
#endif
                throw new TurboActivateException("Failed to set the custom proxy.");
        }

        /// <summary>Get the number of trial days remaining. You must call <see cref="UseTrial()"/> at least once in the past before calling this function.</summary>
        /// <param name="useTrialFlags">The same exact flags you passed to <see cref="UseTrial()"/>.</param>
        /// <returns>The number of days remaining. 0 days if the trial has expired. (E.g. 1 day means *at most* 1 day. That is it could be 30 seconds.)</returns>
        public uint TrialDaysRemaining(TA_Flags useTrialFlags = TA_Flags.TA_SYSTEM | TA_Flags.TA_VERIFIED_TRIAL)
        {
            uint daysRemain = 0;

#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_TrialDaysRemaining(handle, useTrialFlags, ref daysRemain) : Native.TA_TrialDaysRemaining(handle, useTrialFlags, ref daysRemain))
#else
            switch (Native.TA_TrialDaysRemaining(handle, useTrialFlags, ref daysRemain))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_OK: // successful
                    break;
                case TA_E_ALREADY_VERIFIED_TRIAL:
                    throw new AlreadyVerifiedTrialException();
                case TA_E_MUST_SPECIFY_TRIAL_TYPE:
                    throw new MustSpecifyTrialTypeException();
                case TA_E_MUST_USE_TRIAL:
                    throw new MustUseTrialException();
                default:
                    throw new TurboActivateException("Failed to get the trial data.");
            }

            return daysRemain;
        }

        /// <summary>Begins the trial the first time it's called. Calling it again will validate the trial data hasn't been tampered with.</summary>
        /// <param name="flags">Whether to create the trial (verified or unverified) either user-wide or system-wide and whether to allow trials in virtual machines. Valid flags are <see cref="TA_Flags.TA_SYSTEM"/>, <see cref="TA_Flags.TA_USER"/>, <see cref="TA_Flags.TA_DISALLOW_VM"/>, <see cref="TA_Flags.TA_VERIFIED_TRIAL"/>, and <see cref="TA_Flags.TA_UNVERIFIED_TRIAL"/>.</param>
        /// <param name="extraData">Extra data to pass to the LimeLM servers that will be visible for you to see and use. Maximum size is 255 UTF-8 characters.</param>
        public void UseTrial(TA_Flags flags = TA_Flags.TA_SYSTEM | TA_Flags.TA_VERIFIED_TRIAL, string extraData = null)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_UseTrial(handle, flags, extraData) : Native.TA_UseTrial(handle, flags, extraData))
#else
            switch (Native.TA_UseTrial(handle, flags, extraData))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_INET:
                    throw new InternetException();
                case TA_OK: // successful
                    return;
                case TA_E_PERMISSION:
                    throw new PermissionException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_INVALID_FLAGS:
                    throw new InvalidFlagsException();
                case TA_E_IN_VM:
                    throw new VirtualMachineException();
                case TA_E_ACCOUNT_CANCELED:
                    throw new AccountCanceledException();
                case TA_E_ALREADY_VERIFIED_TRIAL:
                    throw new AlreadyVerifiedTrialException();
                case TA_E_EXPIRED:
                    throw new DateTimeException(false);
                case TA_E_TRIAL_EXPIRED:
                    throw new TrialExpiredException();
                case TA_E_MUST_SPECIFY_TRIAL_TYPE:
                    throw new MustSpecifyTrialTypeException();
                case TA_E_EDATA_LONG:
                    throw new ExtraDataTooLongException();
                case TA_E_NO_MORE_TRIALS_ALLOWED:
                    throw new NoMoreTrialsAllowedException();
                default:
                    throw new TurboActivateException("Failed to save the trial data.");
            }
        }

        /// <summary>Generate a "verified trial" offline request file. This file will then need to be submitted to LimeLM. You will then need to use the TA_UseTrialVerifiedFromFile() function with the response file from LimeLM to actually start the trial.</summary>
        /// <param name="filename">The location where you want to save the trial request file.</param>
        /// <param name="extraData">Extra data to pass to the LimeLM servers that will be visible for you to see and use. Maximum size is 255 UTF-8 characters.</param>
        public void UseTrialVerifiedRequest(string filename, string extraData = null)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_UseTrialVerifiedRequest(handle, filename, extraData) : Native.TA_UseTrialVerifiedRequest(handle, filename, extraData))
#else
            switch (Native.TA_UseTrialVerifiedRequest(handle, filename, extraData))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_EDATA_LONG:
                    throw new ExtraDataTooLongException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to save the verified trial request file.");
            }
        }

        /// <summary>Use the "verified trial response" from LimeLM to start the verified trial.</summary>
        /// <param name="filename">The location of the trial response file.</param>
        /// <param name="flags">Whether to create the trial (verified or unverified) either user-wide or system-wide and whether to allow trials in virtual machines. Valid flags are <see cref="TA_Flags.TA_SYSTEM"/>, <see cref="TA_Flags.TA_USER"/>, <see cref="TA_Flags.TA_DISALLOW_VM"/>, <see cref="TA_Flags.TA_VERIFIED_TRIAL"/>, and <see cref="TA_Flags.TA_UNVERIFIED_TRIAL"/>.</param>
        public void UseTrialVerifiedFromFile(string filename, TA_Flags flags = TA_Flags.TA_SYSTEM | TA_Flags.TA_VERIFIED_TRIAL)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_UseTrialVerifiedFromFile(handle, filename, flags) : Native.TA_UseTrialVerifiedFromFile(handle, filename, flags))
#else
            switch (Native.TA_UseTrialVerifiedFromFile(handle, filename, flags))
#endif
            {
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_PERMISSION:
                    throw new PermissionException();
                case TA_E_INVALID_FLAGS:
                    throw new InvalidFlagsException();
                case TA_E_INVALID_ARGS:
                    throw new InvalidArgsException();
                case TA_E_COM:
                    throw new COMException();
                case TA_E_ENABLE_NETWORK_ADAPTERS:
                    throw new EnableNetworkAdaptersException();
                case TA_E_MUST_SPECIFY_TRIAL_TYPE:
                    throw new MustSpecifyTrialTypeException();
                case TA_E_IN_VM:
                    throw new VirtualMachineException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to save the trial data.");
            }
        }

        /// <summary>Extends the trial using a trial extension created in LimeLM.</summary>
        /// <param name="useTrialFlags">The same exact flags you passed to <see cref="UseTrial()"/>.</param>
        /// <param name="trialExtension">The trial extension generated from LimeLM.</param>
        public void ExtendTrial(string trialExtension, TA_Flags useTrialFlags = TA_Flags.TA_SYSTEM | TA_Flags.TA_VERIFIED_TRIAL)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_ExtendTrial(handle, useTrialFlags, trialExtension) : Native.TA_ExtendTrial(handle, useTrialFlags, trialExtension))
#else
            switch (Native.TA_ExtendTrial(handle, useTrialFlags, trialExtension))
#endif
            {
                case TA_E_INET:
                    throw new InternetException();
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                case TA_E_TRIAL_EUSED:
                    throw new TrialExtUsedException();
                case TA_E_TRIAL_EEXP:
                    throw new TrialExtExpiredException();
                case TA_E_MUST_SPECIFY_TRIAL_TYPE:
                    throw new MustSpecifyTrialTypeException();
                case TA_E_MUST_USE_TRIAL:
                    throw new MustUseTrialException();
                case TA_OK: // successful
                    return;
                default:
                    throw new TurboActivateException("Failed to extend trial.");
            }
        }


        /// <summary>This function allows you to set a custom folder to store the activation
        ///data files. For normal use we do not recommend you use this function.
        ///
        ///Only use this function if you absolutely must store data into a separate
        ///folder. For example if your application runs on a USB drive and can't write
        ///any files to the main disk, then you can use this function to save the activation
        ///data files to a directory on the USB disk.
        ///
        ///If you are using this function (which we only recommend for very special use-cases)
        ///then you must call this function on every start of your program at the very top of
        ///your app before any other functions are called.
        ///
        ///The directory you pass in must already exist. And the process using TurboActivate
        ///must have permission to create, write, and delete files in that directory.</summary>
        /// <param name="directory">The full directory to store the activation files.</param>
        public void SetCustomActDataPath(string directory)
        {
#if TA_BOTH_DLL
            switch (IntPtr.Size == 8 ? Native64.TA_SetCustomActDataPath(handle, directory) : Native.TA_SetCustomActDataPath(handle, directory))
#else
            switch (Native.TA_SetCustomActDataPath(handle, directory))
#endif
            {
                case TA_OK: // successful
                    return;
                case TA_E_INVALID_HANDLE:
                    throw new InvalidHandleException();
                default:
                    throw new TurboActivateException("The directory must exist and you must have access to it.");
            }
        }
    }

    public class COMException : TurboActivateException
    {
        public COMException()
            : base("CoInitializeEx failed. Re-enable Windows Management Instrumentation (WMI) service. Contact your system admin for more information.")
        {
        }
    }

    public class AccountCanceledException : TurboActivateException
    {
        public AccountCanceledException()
            : base("Can't activate because the LimeLM account is cancelled.")
        {
        }
    }

    public class PkeyRevokedException : TurboActivateException
    {
        public PkeyRevokedException()
            : base("The product key has been revoked.")
        {
        }
    }

    public class PkeyMaxUsedException : TurboActivateException
    {
        public PkeyMaxUsedException()
            : base("The product key has already been activated with the maximum number of computers.")
        {
        }
    }

    public class InternetException : TurboActivateException
    {
        public InternetException()
            : base("Connection to the servers failed.")
        {
        }
    }

    public class InvalidProductKeyException : TurboActivateException
    {
        public InvalidProductKeyException()
            : base("The product key is invalid or there's no product key.")
        {
        }
    }

    public class NotActivatedException : TurboActivateException
    {
        public NotActivatedException()
            : base("The product needs to be activated.")
        {
        }
    }

    public class ProductDetailsException : TurboActivateException
    {
        public ProductDetailsException()
            : base("The product details file \"TurboActivate.dat\" failed to load. It's either missing or corrupt.")
        {
        }
    }

    public class InvalidHandleException : TurboActivateException
    {
        public InvalidHandleException()
            : base("The handle is not valid. You must set the VersionGUID property.")
        { }
    }

    public class TrialExtUsedException : TurboActivateException
    {
        public TrialExtUsedException()
            : base("The trial extension has already been used.")
        {
        }
    }

    public class TrialExtExpiredException : TurboActivateException
    {
        public TrialExtExpiredException()
            : base("The trial extension has expired.")
        {
        }
    }

    public class DateTimeException : TurboActivateException
    {
        public DateTimeException(bool either)
            : base(either ? "Either the activation response file has expired or your date and time settings are incorrect. Fix your date and time settings, restart your computer, and try to activate again."
                    : "Failed because your system date and time settings are incorrect. Fix your date and time settings, restart your computer, and try to activate again.")
        {
        }
    }

    public class PermissionException : TurboActivateException
    {
        public PermissionException()
            : base("Insufficient system permission. Either start your process as an admin / elevated user or call the function again with the TA_USER flag.")
        {
        }
    }

    public class InvalidFlagsException : TurboActivateException
    {
        public InvalidFlagsException()
            : base("The flags you passed to the function were invalid (or missing). Flags like \"TA_SYSTEM\" and \"TA_USER\" are mutually exclusive -- you can only use one or the other.")
        {
        }
    }

    public class VirtualMachineException : TurboActivateException
    {
        public VirtualMachineException()
            : base("The function failed because this instance of your program is running inside a virtual machine / hypervisor and you've prevented the function from running inside a VM.")
        {
        }
    }

    public class ExtraDataTooLongException : TurboActivateException
    {
        public ExtraDataTooLongException()
            : base("The \"extra data\" was too long. You're limited to 255 UTF-8 characters. Or, on Windows, a Unicode string that will convert into 255 UTF-8 characters or less.")
        {
        }
    }

    public class InvalidArgsException : TurboActivateException
    {
        public InvalidArgsException()
            : base("The arguments passed to the function are invalid. Double check your logic.")
        {
        }
    }

    public class TurboFloatKeyException : TurboActivateException
    {
        public TurboFloatKeyException()
            : base("The product key used is for TurboFloat, not TurboActivate.")
        {
        }
    }

    public class NoMoreDeactivationsException : TurboActivateException
    {
        public NoMoreDeactivationsException()
            : base("No more deactivations are allowed for the product key. This product is still activated on this computer.")
        {
        }
    }

    public class EnableNetworkAdaptersException : TurboActivateException
    {
        public EnableNetworkAdaptersException()
            : base("There are network adapters on the system that are disabled and TurboActivate couldn't read their hardware properties (even after trying and failing to enable the adapters automatically). Enable the network adapters, re-run the function, and TurboActivate will be able to \"remember\" the adapters even if the adapters are disabled in the future.")
        {
        }
    }

    public class AlreadyActivatedException : TurboActivateException
    {
        public AlreadyActivatedException()
            : base("You can't use a product key because your app is already activated with a product key. To use a new product key, then first deactivate using either the Deactivate() or DeactivationRequestToFile().")
        {
        }
    }

    public class AlreadyVerifiedTrialException : TurboActivateException
    {
        public AlreadyVerifiedTrialException()
            : base("The trial is already a verified trial. You need to use the \"TA_VERIFIED_TRIAL\" flag. Can't \"downgrade\" a verified trial to an unverified trial.")
        {
        }
    }

    public class TrialExpiredException : TurboActivateException
    {
        public TrialExpiredException()
            : base("The verified trial has expired. You must request a trial extension from the company.")
        {
        }
    }

    public class NoMoreTrialsAllowedException : TurboActivateException
    {
        public NoMoreTrialsAllowedException()
            : base("In the LimeLM account either the trial days is set to 0, OR the account is set to not auto-upgrade and thus no more verified trials can be made.")
        {
        }
    }

    public class MustSpecifyTrialTypeException : TurboActivateException
    {
        public MustSpecifyTrialTypeException()
            : base("You must specify the trial type (TA_UNVERIFIED_TRIAL or TA_VERIFIED_TRIAL). And you can't use both flags. Choose one or the other. We recommend TA_VERIFIED_TRIAL.")
        {
        }
    }

    public class MustUseTrialException : TurboActivateException
    {
        public MustUseTrialException()
            : base("You must call TA_UseTrial() before you can get the number of trial days remaining.")
        {
        }
    }

    public class TurboActivateException : Exception
    {
        public TurboActivateException(string message) : base(message) { }
    }

    public enum IsGenuineResult
    {
        /// <summary>Is activated and genuine.</summary>
        Genuine = 0,

        /// <summary>Is activated and genuine and the features changed.</summary>
        GenuineFeaturesChanged = 1,

        /// <summary>Not genuine (note: use this in tandem with NotGenuineInVM).</summary>
        NotGenuine = 2,

        /// <summary>Not genuine because you're in a Virtual Machine.</summary>
        NotGenuineInVM = 3,

        /// <summary>Treat this error as a warning. That is, tell the user that the activation couldn't be validated with the servers and that they can manually recheck with the servers immediately.</summary>
        InternetError = 4
    }   
}