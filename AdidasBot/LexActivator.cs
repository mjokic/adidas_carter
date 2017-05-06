using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Cryptlex
{
    public static class LexActivator
    {
        private const string DLL_FILE_NAME = "LexActivator.dll";

        /*
            In order to use "Any CPU" configuration, rename 64 bit LexActivator.dll to LexActivator64.dll and add "LA_ANY_CPU"
	        conditional compilation symbol in your project properties.
        */
#if LA_ANY_CPU
        private const string DLL_FILE_NAME_X64 = "LexActivator64.dll";
#endif
        public enum PermissionFlags : uint
        {
            LA_USER = 1,
            LA_SYSTEM = 2,
        }

        public enum TrialType : uint
        {
            LA_V_TRIAL = 1,
            LA_UV_TRIAL = 2,
        }

        /*
            FUNCTION: SetProductFile()

            PURPOSE: Sets the path of the Product.dat file. This should be
            used if your application and Product.dat file are in different
            folders or you have renamed the Product.dat file.

            If this function is used, it must be called on every start of
            your program before any other functions are called.

            PARAMETERS:
            * filePath - path of the product file (Product.dat)

            RETURN CODES: LA_OK, LA_E_FPATH, LA_E_PFILE

            NOTE: If this function fails to set the path of product file, none of the
            other functions will work.
        */

        public static int SetProductFile(string filePath)
        {
#if LA_ANY_CPU
            Console.WriteLine("FILE FUCKING PATH:" + filePath);
            return IntPtr.Size == 8 ? Native.SetProductFile_x64(filePath) : Native.SetProductFile(filePath);
#else
            return Native.SetProductFile(filePath);
#endif

        }

        /*
            FUNCTION: SetVersionGUID()

            PURPOSE: Sets the version GUID of your application. 

            This function must be called on every start of your program before
            any other functions are called, with the exception of SetProductFile()
            function.

            PARAMETERS:
            * versionGUID - the unique version GUID of your application as mentioned
              on the product version page of your application in the dashboard.

            * flags - depending upon whether your application requires admin/root 
              permissions to run or not, this parameter can have one of the following
              values: LA_SYSTEM, LA_USER

            RETURN CODES: LA_OK, LA_E_WMIC, LA_E_PFILE, LA_E_GUID, LA_E_PERMISSION

            NOTE: If this function fails to set the version GUID, none of the other
            functions will work.
        */

        public static int SetVersionGUID(string versionGUID, PermissionFlags flags)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetVersionGUID_x64(versionGUID, flags) : Native.SetVersionGUID(versionGUID, flags);
#else
            return Native.SetVersionGUID(versionGUID, flags);
#endif

        }

        /*
            FUNCTION: SetProductKey()

            PURPOSE: Sets the product key required to activate the application.

            PARAMETERS:
            * productKey - a valid product key generated for the application.

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_PKEY
        */

        public static int SetProductKey(string productKey)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetProductKey_x64(productKey) : Native.SetProductKey(productKey);
#else
            return Native.SetProductKey(productKey);
#endif

        }

        /*
            FUNCTION: SetExtraActivationData()

            PURPOSE: Sets the extra data which you may want to fetch from the user
            at the time of activation. 

            The extra data appears along with activation details of the product key
            in dashboard.

            PARAMETERS:
            * extraData - string of maximum length 256 characters with utf-8 encoding.

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_EDATA_LEN

            NOTE: If the length of the string is more than 256, it is truncated to the
            allowed size.
        */

        public static int SetExtraActivationData(string extraData)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.SetExtraActivationData_x64(extraData) : Native.SetExtraActivationData(extraData);
#else
            return Native.SetExtraActivationData(extraData);
#endif

        }

        /*
            FUNCTION: ActivateProduct()

            PURPOSE: Activates your application by contacting the Cryptlex servers. It 
            validates the key and returns with encrypted and digitally signed response
            which it stores and uses to activate your application.

            This function should be executed at the time of registration, ideally on
            a button click. 

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_FAIL, LA_E_GUID, LA_E_PKEY,
            LA_E_INET, LA_E_VM, LA_E_TIME
        */

        public static int ActivateProduct()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.ActivateProduct_x64() : Native.ActivateProduct();
#else
            return Native.ActivateProduct();
#endif

        }

        /*
            FUNCTION: DeactivateProduct()

            PURPOSE: Deactivates the application and frees up the correponding activation
            slot by contacting the Cryptlex servers.

            This function should be executed at the time of deregistration, ideally on
            a button click. 

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_FAIL, LA_E_GUID, LA_E_PKEY,
            LA_E_INET, LA_E_DEACT_LIMIT
        */

        public static int DeactivateProduct()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.DeactivateProduct_x64() : Native.DeactivateProduct();
#else
            return Native.DeactivateProduct(); 
#endif
        }

        /*
            FUNCTION: ActivateProductOffline()

            PURPOSE: Activates your application using the offline activation response
            file.
			
			PARAMETERS:
            * filePath - path of the offline activation response file.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_PKEY, LA_E_OFILE
            LA_E_VM, LA_E_TIME
        */

        public static int ActivateProductOffline(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.ActivateProductOffline_x64(filePath) : Native.ActivateProductOffline(filePath);
#else 
            return Native.ActivateProductOffline(filePath); 
#endif
        }

        /*
            FUNCTION: GenerateOfflineActivationRequest()

            PURPOSE: Generates the offline activation request needed for generating
            offline activation response in the dashboard.

            PARAMETERS:
            * filePath - path of the file, needed to be created, for the offline request.

            RETURN CODES: LA_OK, LA_FAIL, LA_E_GUID, LA_E_PKEY
        */

        public static int GenerateOfflineActivationRequest(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GenerateOfflineActivationRequest_x64(filePath) : Native.GenerateOfflineActivationRequest(filePath);
#else 
            return Native.GenerateOfflineActivationRequest(filePath);
#endif
        }

        /*
            FUNCTION: GenerateOfflineDeactivationRequest()

            PURPOSE: Generates the offline deactivation request needed for deactivation of 
            the product key in the dashboard and deactivates the application.

            A valid offline deactivation file confirms that the application has been successfully
            deactivated on the user's machine.

            PARAMETERS:
            * filePath - path of the file, needed to be created, for the offline request.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_FAIL, LA_E_GUID, LA_E_PKEY
        */

        public static int GenerateOfflineDeactivationRequest(string filePath)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GenerateOfflineDeactivationRequest_x64(filePath) : Native.GenerateOfflineDeactivationRequest(filePath);
#else 
            return Native.GenerateOfflineDeactivationRequest(filePath); 
#endif
        }

        /*
        FUNCTION: IsProductGenuine()

        PURPOSE: It verifies whether your app is genuinely activated or not. The verification is
        done locally by verifying the cryptographic digital signature fetched at the time of
        activation.

        After verifying locally, it schedules a server check in a separate thread on due dates.
        The default interval for server check is 100 days and this can be changed if required.

        In case server validation fails due to network error, it retries every 15 minutes. If it
        continues to fail for fixed number of days (grace period), the function returns LA_GP_OVER
        instead of LA_OK. The default length of grace period is 30 days and this can be changed if 
        required.

        This function must be called on every start of your program to verify the activation
        of your app.

        RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_GP_OVER, LA_FAIL, LA_E_GUID, LA_E_PKEY

        NOTE: If application was activated offline using ActivateProductOffline() function, you
        may want to set grace period to 0 to ignore grace period.
    */

        public static int IsProductGenuine()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.IsProductGenuine_x64() : Native.IsProductGenuine();
#else 
            return Native.IsProductGenuine();
#endif
        }

        /*
            FUNCTION: IsProductActivated()

            PURPOSE: It verifies whether your app is genuinely activated or not. The verification is
            done locally by verifying the cryptographic digital signature fetched at the time of
            activation.

            This is just an auxiliary function which you may use in some specific cases.

            RETURN CODES: LA_OK, LA_EXPIRED, LA_REVOKED, LA_GP_OVER, LA_FAIL, LA_E_GUID, LA_E_PKEY
        */

        public static int IsProductActivated()
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.IsProductActivated_x64() : Native.IsProductActivated();
#else 
            return Native.IsProductActivated(); 
#endif
        }

        /*
           FUNCTION: GetExtraActivationData()

           PURPOSE: Gets the value of the extra activation data.

           PARAMETERS:
           * extraData - pointer to a buffer that receives the value of the string
           * length - size of the buffer pointed to by the fieldValue parameter

           RETURN CODES: LA_OK, LA_E_GUID, LA_E_BUFFER_SIZE
       */

        public static int GetExtraActivationData(StringBuilder extraData, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetExtraActivationData_x64(extraData, length) : Native.GetExtraActivationData(extraData, length);
#else 
            return Native.GetExtraActivationData(extraData, length);
#endif
        }

        /*
            FUNCTION: GetCustomLicenseField()

            PURPOSE: Gets the value of the custom field associated with the product key.

            PARAMETERS:
            * fieldId - id of the custom field whose value you want to get
            * fieldValue - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the fieldValue parameter

            RETURN CODES: LA_OK, LA_E_CUSTOM_FIELD_ID, LA_E_GUID, LA_E_BUFFER_SIZE
        */

        public static int GetCustomLicenseField(string fieldId, StringBuilder fieldValue, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetCustomLicenseField_x64(fieldId, fieldValue, length) : Native.GetCustomLicenseField(fieldId, fieldValue, length);
#else 
            return Native.GetCustomLicenseField(fieldId, fieldValue, length);
#endif
        }

        /*
            FUNCTION: GetProductKey()

            PURPOSE: Gets the stored product key which was used for activation.

            PARAMETERS:
            * productKey - pointer to a buffer that receives the value of the string
            * length - size of the buffer pointed to by the productKey parameter

            RETURN CODES: LA_OK, LA_E_PKEY, LA_E_GUID, LA_E_BUFFER_SIZE
        */

        public static int GetProductKey(StringBuilder productKey, int length)
        {
#if LA_ANY_CPU
            return IntPtr.Size == 8 ? Native.GetProductKey_x64(productKey, length) : Native.GetProductKey(productKey, length);
#else
            return Native.GetProductKey(productKey, length);
#endif
        }

        /*
            FUNCTION: GetDaysLeftToExpiration()

            PURPOSE: Gets the number of remaining days after which the license expires.

            PARAMETERS:
            * daysLeft - pointer to the integer that receives the value

            RETURN CODES: LA_OK, LA_FAIL, LA_E_GUID
        */

        public static int GetDaysLeftToExpiration(ref uint daysLeft)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.GetDaysLeftToExpiration_x64(ref daysLeft) : Native.GetDaysLeftToExpiration(ref daysLeft);
#else 
            return Native.GetDaysLeftToExpiration(ref daysLeft);
#endif
        }

        /*
            FUNCTION: SetTrialKey()

            PURPOSE: Sets the trial key required to activate the verified trial.

            PARAMETERS:
            * trialKey - trial key corresponding to the product version

            RETURN CODES: LA_OK, LA_E_GUID, LA_E_TKEY
        */

        public static int SetTrialKey(string trialKey)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetTrialKey_x64(trialKey) : Native.SetTrialKey(trialKey);
#else 
            return Native.SetTrialKey(trialKey); 
#endif
        }

        /*
            FUNCTION: ActivateTrial()

            PURPOSE: Starts the verified trial in your application by contacting the 
            Cryptlex servers. 

            This function should be executed when your application starts first time on
            the user's computer, ideally on a button click. 

            RETURN CODES: LA_OK, LA_T_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_TKEY, LA_E_INET,
            LA_E_VM, LA_E_TIME
        */

        public static int ActivateTrial()
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.ActivateTrial_x64() : Native.ActivateTrial();
#else 
            return Native.ActivateTrial(); 
#endif
        }

        /*
            FUNCTION: IsTrialGenuine()

            PURPOSE: It verifies whether trial has started and is genuine or not. The
            verification is done locally by verifying the cryptographic digital signature
            fetched at the time of trial activation.

            This function must be called on every start of your program during the trial period.

            RETURN CODES: LA_OK, LA_T_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_TKEY

            NOTE: The function is only meant for verified trials.
        */

        public static int IsTrialGenuine()
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.IsTrialGenuine_x64() : Native.IsTrialGenuine();
#else 
            return Native.IsTrialGenuine(); 
#endif
        }

        /*
            FUNCTION: ExtendTrial()

            PURPOSE: Extends the trial using the trial extension key generated in the dashboard
            for the product version.

            PARAMETERS:
            * trialExtensionKey - trial extension key generated for the product version

            RETURN CODES: LA_OK, LA_TEXT_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_TEXT_KEY, LA_E_TKEY,
            LA_E_INET, LA_E_VM, LA_E_TIME

            NOTE: The function is only meant for verified trials.
        */

        public static int ExtendTrial(string trialExtensionKey)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.ExtendTrial_x64(trialExtensionKey) : Native.ExtendTrial(trialExtensionKey);
#else 
            return Native.ExtendTrial(trialExtensionKey); 
#endif
        }

        /*
            FUNCTION: InitializeTrial()

            PURPOSE: Starts the unverified trial if trial has not started yet and if
            trial has already started, it verifies the validity of trial.

            This function must be called on every start of your program during the trial period.

            PARAMETERS:
            * trialLength - trial length as set in the dashboard for the product version

            RETURN CODES: LA_OK, LA_T_EXPIRED, LA_FAIL, LA_E_GUID, LA_E_TRIAL_LEN

            NOTE: The function is only meant for unverified trials.
        */

        public static int InitializeTrial(uint trialLength)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.InitializeTrial_x64(trialLength) : Native.InitializeTrial(trialLength);
#else 
            return Native.InitializeTrial(trialLength); 
#endif
        }

        /*
            FUNCTION: GetTrialDaysLeft()

            PURPOSE: Gets the number of remaining trial days.

            If the trial has expired or has been tampered, daysLeft is set to 0 days.

            PARAMETERS:
            * daysLeft - pointer to the integer that receives the value
            * trialType - depending upon whether your application uses verified trial or not,
              this parameter can have one of the following values: LA_V_TRIAL, LA_UV_TRIAL

            RETURN CODES: LA_OK, LA_FAIL, LA_E_GUID

            NOTE: The trial must be started by calling ActivateTrial() or  InitializeTrial() at least
            once in the past before calling this function.
        */

        public static int GetTrialDaysLeft(ref uint daysLeft, TrialType trialType)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.GetTrialDaysLeft_x64(ref daysLeft, trialType) : Native.GetTrialDaysLeft(ref daysLeft, trialType);
#else 
            return Native.GetTrialDaysLeft(ref daysLeft, trialType); 
#endif
        }

        /*
            FUNCTION: SetDayIntervalForServerCheck()

            PURPOSE: Sets the interval for server checks done by IsProductGenuine() function.

            To disable sever check pass 0 as the day interval.

            PARAMETERS:
            * dayInterval - length of the interval in days

            RETURN CODES: LA_OK, LA_E_GUID
        */

        public static int SetDayIntervalForServerCheck(uint dayInterval)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetDayIntervalForServerCheck_x64(dayInterval) : Native.SetDayIntervalForServerCheck(dayInterval);
#else 
            return Native.SetDayIntervalForServerCheck(dayInterval);
#endif
        }

        /*
            FUNCTION: SetGracePeriodForNetworkError()

            PURPOSE: Sets the grace period for failed re-validation requests sent
            by IsProductGenuine() function, caused due to network errors.
    
            It determines how long in days, should IsProductGenuine() function retry
            contacting CryptLex Servers, before returning LA_GP_OVER instead of LA_OK.

            To ignore grace period pass 0 as the grace period. This may be useful in
            case of offline activations.

            PARAMETERS:
            * gracePeriod - length of the grace period in days

            RETURN CODES: LA_OK, LA_E_GUID
        */

        public static int SetGracePeriodForNetworkError(uint gracePeriod)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetGracePeriodForNetworkError_x64(gracePeriod) : Native.SetGracePeriodForNetworkError(gracePeriod);
#else 
            return Native.SetGracePeriodForNetworkError(gracePeriod);
#endif
        }

        /*
            FUNCTION: SetNetworkProxy()

            PURPOSE: Sets the network proxy to be used when contacting CryptLex servers.

            The proxy format should be: [protocol://][username:password@]machine[:port]

            Following are some examples of the valid proxy strings:
                - http://127.0.0.1:8000/
                - http://user:pass@127.0.0.1:8000/
                - socks5://127.0.0.1:8000/

            PARAMETERS:
            * proxy - proxy string having correct proxy format

            RETURN CODES: LA_OK, LA_E_NET_PROXY, LA_E_GUID

            NOTE: Proxy settings of the computer are automatically detected. So, in most of the 
            cases you don't need to care whether your user is behind a proxy server or not.
        */

        public static int SetNetworkProxy(string proxy)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetNetworkProxy_x64(proxy) : Native.SetNetworkProxy(proxy);
#else 
            return Native.SetNetworkProxy(proxy); 
#endif
        }

        /*
            FUNCTION: SetUserLock()

            PURPOSE: Enables the user locked licensing.

            It adds an additional user lock to the product key. Activations by different users in
            the same OS are treated as separate activations.

            PARAMETERS:
            * userLock - boolean value to enable or disable the lock

            RETURN CODES: LA_OK, LA_E_GUID

            NOTE: User lock is disabled by default. You should enable it in case your application
            is used through remote desktop services where multiple users access individual sessions
            on a single machine instance at the same time.
        */

        public static int SetUserLock(bool userLock)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetUserLock_x64(userLock) : Native.SetUserLock(userLock);
#else
            return Native.SetUserLock(userLock);
#endif
        }

        /*
            FUNCTION: SetCryptlexHost()

            PURPOSE: In case you are running Cryptlex on a private web server, you can set the
            host for your private server.

            PARAMETERS:
            * host - the address of the private web server running Cryptlex

            RETURN CODES: LA_OK, LA_E_HOST_URL, LA_E_GUID

            NOTE: This function should never be used unless you have opted for a private Cryptlex
            Server.
        */

        public static int SetCryptlexHost(string host)
        {
#if LA_ANY_CPU 
            return IntPtr.Size == 8 ? Native.SetCryptlexHost_x64(host) : Native.SetCryptlexHost(host);
#else
            return Native.SetCryptlexHost(host);
#endif
        }

        /*** Return Codes ***/

        public const int LA_OK = 0x00;

        public const int LA_FAIL = 0x01;

        /*
            CODE: LA_EXPIRED

            MESSAGE: The product key has expired or system time has been tampered
            with. Ensure your date and time settings are correct.
        */

        public const int LA_EXPIRED = 0x02;

        /*
            CODE: LA_REVOKED

            MESSAGE: The product key has been revoked.
        */

        public const int LA_REVOKED = 0x03;

        /*
            CODE: LA_GP_OVER

            MESSAGE: The grace period is over.
        */

        public const int LA_GP_OVER = 0x04;

        /*
            CODE: LA_E_INET

            MESSAGE: Failed to connect to the server due to network error.
        */

        public const int LA_E_INET = 0x05;

        /*
            CODE: LA_E_PKEY

            MESSAGE: Invalid product key.
        */

        public const int LA_E_PKEY = 0x06;

        /*
            CODE: LA_E_PFILE

            MESSAGE: Invalid or corrupted product file.
        */

        public const int LA_E_PFILE = 0x07;

        /*
            CODE: LA_E_FPATH

            MESSAGE: Invalid product file path.
        */

        public const int LA_E_FPATH = 0x08;

        /*
            CODE: LA_E_GUID

            MESSAGE: The version GUID doesn't match that of the product file.
        */

        public const int LA_E_GUID = 0x09;

        /*
            CODE: LA_E_OFILE

            MESSAGE: Invalid offline activation response file.
        */

        public const int LA_E_OFILE = 0x0A;

        /*
            CODE: LA_E_PERMISSION

            MESSAGE: Insufficent system permissions. Occurs when LA_SYSTEM flag is used
            but application is not run with admin privileges.
        */

        public const int LA_E_PERMISSION = 0x0B;

        /*
            CODE: LA_E_EDATA_LEN

            MESSAGE: Extra activation data length is more than 256 characters.
        */


        public const int LA_E_EDATA_LEN = 0x0C;

        /*
            CODE: LA_E_TKEY

            MESSAGE: The trial key doesn't match that of the product file.
        */

        public const int LA_E_TKEY = 0x0D;

        /*
            CODE: LA_E_TIME

            MESSAGE: The system time has been tampered with. Ensure your date
            and time settings are correct.
        */

        public const int LA_E_TIME = 0x0E;

        /*
            CODE: LA_E_VM

            MESSAGE: Application is being run inside a virtual machine / hypervisor,
            and activation has been disallowed in the VM.
            but
        */

        public const int LA_E_VM = 0x0F;

        /*
            CODE: LA_E_WMIC

            MESSAGE: Fingerprint couldn't be generated because Windows Management 
            Instrumentation (WMI) service has been disabled. This error is specific
            to Windows only.
        */

        public const int LA_E_WMIC = 0x10;

        /*
            CODE: LA_E_TEXT_KEY

            MESSAGE: Invalid trial extension key.
        */

        public const int LA_E_TEXT_KEY = 0x11;

        /*
            CODE: LA_E_TRIAL_LEN

            MESSAGE: The trial length doesn't match that of the product file.
        */

        public const int LA_E_TRIAL_LEN = 0x12;

        /*
            CODE: LA_T_EXPIRED

            MESSAGE: The trial has expired or system time has been tampered
            with. Ensure your date and time settings are correct.
        */

        public const int LA_T_EXPIRED = 0x13;

        /*
            CODE: LA_TEXT_EXPIRED

            MESSAGE: The trial extension key being used has already expired or system
            time has been tampered with. Ensure your date and time settings are correct.
        */

        public const int LA_TEXT_EXPIRED = 0x14;

        /*
            CODE: LA_E_BUFFER_SIZE

            MESSAGE: The buffer size was smaller than required.
        */

        public const int LA_E_BUFFER_SIZE = 0x15;
		
		/*
			CODE: LA_E_CUSTOM_FIELD_ID

			MESSAGE: Invalid custom field id.
		*/

        public const int LA_E_CUSTOM_FIELD_ID = 0x16;

        /*
            CODE: LA_E_NET_PROXY

            MESSAGE: Invalid network proxy.
        */

        public const int LA_E_NET_PROXY = 0x17;

        /*
            CODE: LA_E_HOST_URL

            MESSAGE: Invalid Cryptlex host url.
        */

        public const int LA_E_HOST_URL = 0x18;

        /*
            CODE: LA_E_DEACT_LIMIT

            MESSAGE: Deactivation limit for key has reached.
        */

        public const int LA_E_DEACT_LIMIT = 0x19;


        static class Native
        {
            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductFile(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetVersionGUID(string versionGUID, PermissionFlags flags);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductKey(string productKey);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetExtraActivationData(string extraData);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProduct();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int DeactivateProduct();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProductOffline(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineActivationRequest(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineDeactivationRequest(string filePath);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductGenuine();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductActivated();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetExtraActivationData(StringBuilder extraData, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetCustomLicenseField(string fieldId, StringBuilder fieldValue, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKey(StringBuilder productKey, int length);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetDaysLeftToExpiration(ref uint daysLeft);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetTrialKey(string trialKey);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateTrial();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsTrialGenuine();

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ExtendTrial(string trialExtensionKey);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int InitializeTrial(uint trialLength);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetTrialDaysLeft(ref uint daysLeft, TrialType trialType);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetDayIntervalForServerCheck(uint dayInterval);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetGracePeriodForNetworkError(uint gracePeriod);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetNetworkProxy(string proxy);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetUserLock(bool userLock);

            [DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetCryptlexHost(string host);

#if LA_ANY_CPU
            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetProductFile", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductFile_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetVersionGUID", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetVersionGUID_x64(string versionGUID, PermissionFlags flags);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetProductKey", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetProductKey_x64(string productKey);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetExtraActivationData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetExtraActivationData_x64(string extraData);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateProduct", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProduct_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "DeactivateProduct", CallingConvention = CallingConvention.Cdecl)]
            public static extern int DeactivateProduct_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateProductOffline", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateProductOffline_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GenerateOfflineActivationRequest", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineActivationRequest_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GenerateOfflineDeactivationRequest", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GenerateOfflineDeactivationRequest_x64(string filePath);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsProductGenuine", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductGenuine_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsProductActivated", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsProductActivated_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetExtraActivationData", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetExtraActivationData_x64(StringBuilder extraData, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetCustomLicenseField", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetCustomLicenseField_x64(string fieldId, StringBuilder fieldValue, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetProductKey", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetProductKey_x64(StringBuilder productKey, int length);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetDaysLeftToExpiration", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetDaysLeftToExpiration_x64(ref uint daysLeft);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetTrialKey", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetTrialKey_x64(string trialKey);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ActivateTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ActivateTrial_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "IsTrialGenuine", CallingConvention = CallingConvention.Cdecl)]
            public static extern int IsTrialGenuine_x64();

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "ExtendTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ExtendTrial_x64(string trialExtensionKey);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "InitializeTrial", CallingConvention = CallingConvention.Cdecl)]
            public static extern int InitializeTrial_x64(uint trialLength);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "GetTrialDaysLeft", CallingConvention = CallingConvention.Cdecl)]
            public static extern int GetTrialDaysLeft_x64(ref uint daysLeft, TrialType trialType);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetDayIntervalForServerCheck", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetDayIntervalForServerCheck_x64(uint dayInterval);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetGracePeriodForNetworkError", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetGracePeriodForNetworkError_x64(uint gracePeriod);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetNetworkProxy", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetNetworkProxy_x64(string proxy);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetUserLock", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetUserLock_x64(bool userLock);

            [DllImport(DLL_FILE_NAME_X64, CharSet = CharSet.Unicode, EntryPoint = "SetCryptlexHost", CallingConvention = CallingConvention.Cdecl)]
            public static extern int SetCryptlexHost_x64(string host);
#endif
        }
    }
}