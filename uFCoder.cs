using System;
using System.Text;

namespace uFR
{
    using System.Runtime.InteropServices;

    enum CARD_SAK
    {
        UNKNOWN = 0x00,
        MIFARE_CLASSIC_1k = 0x08,
        MF1ICS50 = 0x08,
        SLE66R35 = 0x88,
        MIFARE_CLASSIC_4k = 0x18,
        MF1ICS70 = 0x18,
        MIFARE_CLASSIC_MINI = 0x09,
        MF1ICS20 = 0x09,
    }

    enum DLOGIC_CARD_TYPE
    {
        DL_NO_CARD = 0x00,
        DL_MIFARE_ULTRALIGHT = 0x01,
        DL_MIFARE_ULTRALIGHT_EV1_11 = 0x02,
        DL_MIFARE_ULTRALIGHT_EV1_21 = 0x03,
        DL_MIFARE_ULTRALIGHT_C = 0x04,
        DL_NTAG_203 = 0x05,
        DL_NTAG_210 = 0x06,
        DL_NTAG_212 = 0x07,
        DL_NTAG_213 = 0x08,
        DL_NTAG_215 = 0x09,
        DL_NTAG_216 = 0x0A,
        DL_MIKRON_MIK640D = 0x0B,
        NFC_T2T_GENERIC = 0x0C,
        DL_NT3H_1101 = 0x0D,
        DL_NT3H_1201 = 0x0E,
        DL_NT3H_2111 = 0x0F,
        DL_NT3H_2211 = 0x10,
        DL_MIFARE_MINI = 0x20,
        DL_MIFARE_CLASSIC_1K = 0x21,
        DL_MIFARE_CLASSIC_4K = 0x22,
        DL_MIFARE_PLUS_S_2K_SL0 = 0x23,
        DL_MIFARE_PLUS_S_4K_SL0 = 0x24,
        DL_MIFARE_PLUS_X_2K_SL0 = 0x25,
        DL_MIFARE_PLUS_X_4K_SL0 = 0x26,
        DL_MIFARE_DESFIRE = 0x27,
        DL_MIFARE_DESFIRE_EV1_2K = 0x28,
        DL_MIFARE_DESFIRE_EV1_4K = 0x29,
        DL_MIFARE_DESFIRE_EV1_8K = 0x2A,
        DL_MIFARE_DESFIRE_EV2_2K = 0x2B,
        DL_MIFARE_DESFIRE_EV2_4K = 0x2C,
        DL_MIFARE_DESFIRE_EV2_8K = 0x2D,
        DL_MIFARE_PLUS_S_2K_SL1 = 0x2E,
        DL_MIFARE_PLUS_X_2K_SL1 = 0x2F,
        DL_MIFARE_PLUS_EV1_2K_SL1 = 0x30,
        DL_MIFARE_PLUS_X_2K_SL2 = 0x31,
        DL_MIFARE_PLUS_S_2K_SL3 = 0x32,
        DL_MIFARE_PLUS_X_2K_SL3 = 0x33,
        DL_MIFARE_PLUS_EV1_2K_SL3 = 0x34,
        DL_MIFARE_PLUS_S_4K_SL1 = 0x35,
        DL_MIFARE_PLUS_X_4K_SL1 = 0x36,
        DL_MIFARE_PLUS_EV1_4K_SL1 = 0x37,
        DL_MIFARE_PLUS_X_4K_SL2 = 0x38,
        DL_MIFARE_PLUS_S_4K_SL3 = 0x39,
        DL_MIFARE_PLUS_X_4K_SL3 = 0x3A,
        DL_MIFARE_PLUS_EV1_4K_SL3 = 0x3B,
        // Special card type
        DL_GENERIC_ISO14443_4 = 0x40,
        DL_GENERIC_ISO14443_4_TYPE_B = 0x41,
        DL_GENERIC_ISO14443_3_TYPE_B = 0x42,
        DL_UNKNOWN_ISO_14443_4 = 0x40
    }

    // MIFARE CLASSIC Authentication Modes:
    enum MIFARE_AUTHENTICATION
    {
        MIFARE_AUTHENT1A = 0x60,
        MIFARE_AUTHENT1B = 0x61,
    }

    // T4T authentication constants
    enum T4T_AUTHENTICATION
    {
        T4T_WITHOUT_PWD_AUTH = 0x60,
        T4T_PK_PWD_AUTH = 0x80,
        T4T_RKA_PWD_AUTH = 0x02,
    }

    // API Status Codes Type:
    public enum DL_STATUS
    {
        UFR_OK = 0x00,
        UFR_COMMUNICATION_ERROR = 0x01,
        UFR_CHKSUM_ERROR = 0x02,
        UFR_READING_ERROR = 0x03,
        UFR_WRITING_ERROR = 0x04,
        UFR_BUFFER_OVERFLOW = 0x05,
        UFR_MAX_ADDRESS_EXCEEDED = 0x06,
        UFR_MAX_KEY_INDEX_EXCEEDED = 0x07,
        UFR_NO_CARD = 0x08,
        UFR_COMMAND_NOT_SUPPORTED = 0x09,
        UFR_FORBIDEN_DIRECT_WRITE_IN_SECTOR_TRAILER = 0x0A,
        UFR_ADDRESSED_BLOCK_IS_NOT_SECTOR_TRAILER = 0x0B,
        UFR_WRONG_ADDRESS_MODE = 0x0C,
        UFR_WRONG_ACCESS_BITS_VALUES = 0x0D,
        UFR_AUTH_ERROR = 0x0E,
        UFR_PARAMETERS_ERROR = 0x0F, // ToDo, tačka 5.
        UFR_MAX_SIZE_EXCEEDED = 0x10,
        UFR_UNSUPPORTED_CARD_TYPE = 0x11,
        UFR_COUNTER_ERROR = 0x12,
        UFR_WRITE_VERIFICATION_ERROR = 0x70,
        UFR_BUFFER_SIZE_EXCEEDED = 0x71,
        UFR_VALUE_BLOCK_INVALID = 0x72,
        UFR_VALUE_BLOCK_ADDR_INVALID = 0x73,
        UFR_VALUE_BLOCK_MANIPULATION_ERROR = 0x74,
        UFR_WRONG_UI_MODE = 0x75,
        UFR_KEYS_LOCKED = 0x76,
        UFR_KEYS_UNLOCKED = 0x77,
        UFR_WRONG_PASSWORD = 0x78,
        UFR_CAN_NOT_LOCK_DEVICE = 0x79,
        UFR_CAN_NOT_UNLOCK_DEVICE = 0x7A,
        UFR_DEVICE_EEPROM_BUSY = 0x7B,
        UFR_RTC_SET_ERROR = 0x7C,
        ANTI_COLLISION_DISABLED = 0x7D,
        NO_TAGS_ENUMERRATED = 0x7E,
        CARD_ALREADY_SELECTED = 0x7F,
        UFR_COMMUNICATION_BREAK = 0x50,
        UFR_NO_MEMORY_ERROR = 0x51,
        UFR_CAN_NOT_OPEN_READER = 0x52,
        UFR_READER_NOT_SUPPORTED = 0x53,
        UFR_READER_OPENING_ERROR = 0x54,
        UFR_READER_PORT_NOT_OPENED = 0x55,
        UFR_CANT_CLOSE_READER_PORT = 0x56,
        UFR_FT_STATUS_ERROR_1 = 0xA0,
        UFR_FT_STATUS_ERROR_2 = 0xA1,
        UFR_FT_STATUS_ERROR_3 = 0xA2,
        UFR_FT_STATUS_ERROR_4 = 0xA3,
        UFR_FT_STATUS_ERROR_5 = 0xA4,
        UFR_FT_STATUS_ERROR_6 = 0xA5,
        UFR_FT_STATUS_ERROR_7 = 0xA6,
        UFR_FT_STATUS_ERROR_8 = 0xA7,
        UFR_FT_STATUS_ERROR_9 = 0xA8,
        //NDEF error codes
        UFR_WRONG_NDEF_CARD_FORMAT = 0x80,
        UFR_NDEF_MESSAGE_NOT_FOUND = 0x81,
        UFR_NDEF_UNSUPPORTED_CARD_TYPE = 0x82,
        UFR_NDEF_CARD_FORMAT_ERROR = 0x83,
        UFR_MAD_NOT_ENABLED = 0x84,
        UFR_MAD_VERSION_NOT_SUPPORTED = 0x85,
        UFR_NDEF_MESSAGE_NOT_COMPATIBLE = 0x86,
        FORBIDDEN_IN_TAG_EMULATION_MODE = 0x90,
        UFR_MFP_COMMAND_OVERFLOW = 0xB0,
        UFR_MFP_INVALID_MAC = 0xB1,
        UFR_MFP_INVALID_BLOCK_NR = 0xB2,
        UFR_MFP_NOT_EXIST_BLOCK_NR = 0xB3,
        UFR_MFP_COND_OF_USE_ERROR = 0xB4,
        UFR_MFP_LENGTH_ERROR = 0xB5,
        UFR_MFP_GENERAL_MANIP_ERROR = 0xB6,
        UFR_MFP_SWITCH_TO_ISO14443_4_ERROR = 0xB7,
        UFR_MFP_ILLEGAL_STATUS_CODE = 0xB8,
        UFR_MFP_MULTI_BLOCKS_READ = 0xB9,
        NT4H_COMMAND_ABORTED = 0xC0,
        NT4H_LENGTH_ERROR = 0xC1,
        NT4H_PARAMETER_ERROR = 0xC2,
        NT4H_NO_SUCH_KEY = 0xC3,
        NT4H_PERMISSION_DENIED = 0xC4,
        NT4H_AUTHENTICATION_DELAY = 0xC5,
        NT4H_MEMORY_ERROR = 0xC6,
        NT4H_INTEGRITY_ERROR = 0xC7,
        NT4H_FILE_NOT_FOUND = 0xC8,
        NT4H_BOUNDARY_ERROR = 0xC9,
        NT4H_INVALID_MAC = 0xCA,
        NT4H_NO_CHANGES = 0xCB,
        //multiple units - return from the functions with ReaderList_ prefix in name
        UFR_DEVICE_WRONG_HANDLE = 0x100,
        UFR_DEVICE_INDEX_OUT_OF_BOUND = 0x101,
        UFR_DEVICE_ALREADY_OPENED = 0x102,
        UFR_DEVICE_ALREADY_CLOSED = 0x103,
        UFR_DEVICE_IS_NOT_CONNECTED = 0x104,
        //Originality check status codes:
        UFR_NOT_NXP_GENUINE = 0x200,
        UFR_OPEN_SSL_DYNAMIC_LIB_FAILED = 0x201,
        UFR_OPEN_SSL_DYNAMIC_LIB_NOT_FOUND = 0x202,
        //uFCoder library status codes:
        UFR_NOT_IMPLEMENTED = 0x1000,
        UFR_COMMAND_FAILED = 0x1001,
        UFR_TIMEOUT_ERR = 0x1002,
        UFR_FILE_SYSTEM_ERROR = 0x1003,
        UFR_FILE_SYSTEM_PATH_NOT_EXISTS = 0x1004,
        UFR_FILE_NOT_EXISTS = 0x1005,
        //APDU status codes:,
        UFR_APDU_TRANSCEIVE_ERROR = 0xAE,
        UFR_APDU_JC_APP_NOT_SELECTED = 0x6000,
        UFR_APDU_JC_APP_BUFF_EMPTY = 0x6001,
        UFR_APDU_WRONG_SELECT_RESPONSE = 0x6002,
        UFR_APDU_WRONG_KEY_TYPE = 0x6003,
        UFR_APDU_WRONG_KEY_SIZE = 0x6004,
        UFR_APDU_WRONG_KEY_PARAMS = 0x6005,
        UFR_APDU_WRONG_SIGNING_ALGORITHM = 0x6006,
        UFR_APDU_PLAIN_TEXT_MAX_SIZE_EXCEEDED = 0x6007,
        UFR_APDU_UNSUPPORTED_KEY_SIZE = 0x6008,
        UFR_APDU_UNSUPPORTED_ALGORITHMS = 0x6009,
        UFR_APDU_PKI_OBJECT_NOT_FOUND = 0x600A,
        UFR_APDU_MAX_PIN_LENGTH_EXCEEDED = 0x600B,
        UFR_DIGEST_LENGTH_DOES_NOT_MATCH = 0x600C,
        //JCApp status codes:
        UFR_APDU_SW_TAG = 0x000A0000,
        UFR_APDU_SW_OPERATION_IS_FAILED = 0x000A6300,
        UFR_APDU_SW_WRONG_LENGTH = 0x000A6700,
        UFR_APDU_SW_SECURITY_STATUS_NOT_SATISFIED = 0x000A6982,
        UFR_APDU_SW_AUTHENTICATION_METHOD_BLOCKED = 0x000A6983,
        UFR_APDU_SW_DATA_INVALID = 0x000A6984,
        UFR_APDU_SW_CONDITIONS_NOT_SATISFIED = 0x000A6985,
        UFR_APDU_SW_WRONG_DATA = 0x000A6A80,
        UFR_APDU_SW_FILE_NOT_FOUND = 0x000A6A82,
        UFR_APDU_SW_RECORD_NOT_FOUND = 0x000A6A83,
        UFR_APDU_SW_DATA_NOT_FOUND = 0x000A6A88,
        UFR_APDU_SW_ENTITY_ALREADY_EXISTS = 0x000A6A89,
        UFR_APDU_SW_INS_NOT_SUPPORTED = 0x000A6D00,
        UFR_APDU_SW_NO_PRECISE_DIAGNOSTIC = 0x000A6F00,
        //Cryptographic subsystem status codes:
        CRYPTO_SUBSYS_NOT_INITIALIZED = 0x6101,
        CRYPTO_SUBSYS_SIGNATURE_VERIFICATION_ERROR = 0x6102,
        CRYPTO_SUBSYS_MAX_HASH_INPUT_EXCEEDED = 0x6103,
        CRYPTO_SUBSYS_INVALID_HASH_ALGORITHM = 0x6104,
        CRYPTO_SUBSYS_INVALID_CIPHER_ALGORITHM = 0x6105,
        CRYPTO_SUBSYS_INVALID_PADDING_ALGORITHM = 0x6106,
        CRYPTO_SUBSYS_WRONG_SIGNATURE = 0x6107,
        CRYPTO_SUBSYS_WRONG_HASH_OUTPUT_LENGTH = 0x6108,
        CRYPTO_SUBSYS_UNKNOWN_ECC_CURVE = 0x6109,
        CRYPTO_SUBSYS_HASHING_ERROR = 0x610A,
        CRYPTO_SUBSYS_INVALID_SIGNATURE_PARAMS = 0x610B,
        CRYPTO_SUBSYS_INVALID_RSA_PUB_KEY = 0x610C,
        CRYPTO_SUBSYS_INVALID_ECC_PUB_KEY_PARAMS = 0x610D,
        CRYPTO_SUBSYS_INVALID_ECC_PUB_KEY = 0x610E,
        UFR_WRONG_PEM_CERT_FORMAT = 0x61C0,
        //X509 error codes
        X509_CAN_NOT_OPEN_FILE = 0x6200,
        X509_WRONG_DATA = 0x6201,
        X509_WRONG_LENGTH = 0x6202,
        X509_UNSUPPORTED_PUBLIC_KEY_TYPE = 0x6203,
        X509_UNSUPPORTED_PUBLIC_KEY_SIZE = 0x6204,
        X509_UNSUPPORTED_PUBLIC_KEY_EXPONENT = 0x6205,
        X509_EXTENSION_NOT_FOUND = 0x6206,
        X509_WRONG_SIGNATURE = 0x6207,
        X509_UNKNOWN_PUBLIC_KEY_TYPE = 0x6208,
        X509_WRONG_RSA_PUBLIC_KEY_FORMAT = 0x6209,
        X509_WRONG_ECC_PUBLIC_KEY_FORMAT = 0x620A,
        X509_SIGNATURE_NOT_MATCH_CA_PUBLIC_KEY = 0x620B,
        X509_UNSUPPORTED_SIGNATURE_SCH = 0x620C,
        X509_UNSUPPORTED_ECC_CURVE = 0x620D,
        //PKCS7 error codes
        PKCS7_WRONG_DATA = 0x6241,
        PKCS7_UNSUPPORTED_SIGNATURE_SCHEME = 0x6242,
        PKCS7_SIG_SCH_NOT_MATCH_CERT_KEY_TYPE = 0x6243,
        PKCS7_WRONG_SIGNATURE = 0x6247,
        //MRTD error codes
        MRTD_SECURE_CHANNEL_SESSION_FAILED = 0x6280,
        MRTD_WRONG_SOD_LENGTH = 0x6282,
        MRTD_UNKNOWN_DIGEST_ALGORITHM = 0x6283,
        MRTD_WARNING_DOES_NOT_CONTAINS_DS_CERT = 0x6284,
        MRTD_DATA_GROUOP_INDEX_NOT_EXIST = 0x6285,
        MRTD_EF_COM_WRONG_DATA = 0x6286,
        MRTD_EF_DG_WRONG_DATA = 0x6287,
        MRTD_EF_DG1_WRONG_LDS_VERSION_LENGTH = 0x6288,
        MRTD_VERIFY_CSCA_NOT_EXIST = 0x6289,
        MRTD_VERIFY_WRONG_DS_SIGNATURE = 0x628A,
        MRTD_VERIFY_WRONG_CSCA_SIGNATURE = 0x628B,
        MRTD_MRZ_CHECK_ERROR = 0x628C,
        //ICAO master list status codes
        ICAO_ML_WRONG_FORMAT = 0x6300,
        ICAO_ML_CAN_NOT_OPEN_FILE = 0x6301,
        ICAO_ML_CAN_NOT_READ_FILE = 0x6302,
        ICAO_ML_CERTIFICATE_NOT_FOUND = 0x6303,
        ICAO_ML_WRONG_SIGNATURE = 0x6307,

        MAX_UFR_STATUS = 10000000
    }

    enum E_PRINT_VERBOSE_LEVELS
    {
        PRINT_NONE,
        PRINT_ESSENTIALS,
        PRINT_DETAILS,
        PRINT_ALL_PLUS_STATUSES,
    };

    public static class uFCoder
    {
        //--------------------------------------------------------------------------------------------------

#if WIN64
        public const string DLL_NAME = "uFCoder-x86_64.dll"; // for x64 target
#else
        public const string DLL_NAME = "uFCoder-x86.dll";// for x86 target

#endif
        //--------------------------------------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "UFR_Status2String")]
        private static extern IntPtr UFR_Status2String(DL_STATUS status);

        public static string status2str(DL_STATUS status)
        {
            IntPtr str_ret = UFR_Status2String(status);
            return Marshal.PtrToStringAnsi(str_ret);
        }

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpen")]
        public static extern DL_STATUS ReaderOpen();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderOpenEx")]
        private static extern DL_STATUS ReaderOpenEx(UInt32 reader_type, [In] byte[] port_name, UInt32 port_interface, [In] byte[] arg);

        public static DL_STATUS ReaderOpenEx(UInt32 reader_type, string port_name, UInt32 port_interface, string arg)
        {

            byte[] port_name_p = Encoding.ASCII.GetBytes(port_name);
            byte[] port_name_param = new byte[port_name_p.Length + 1];
            Array.Copy(port_name_p, 0, port_name_param, 0, port_name_p.Length);
            port_name_param[port_name_p.Length] = 0;

            byte[] arg_p = Encoding.ASCII.GetBytes(arg);
            byte[] arg_param = new byte[arg_p.Length + 1];
            Array.Copy(arg_p, 0, arg_param, 0, arg_p.Length);
            arg_param[arg_p.Length] = 0;

            return ReaderOpenEx(reader_type, port_name_param, port_interface, arg_param);
        }

        //--------------------------------------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderClose")]
        public static extern DL_STATUS ReaderClose();

        //--------------------------------------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCardIdEx")]
        public static extern DL_STATUS GetCardIdEx(ref byte bSak,
                                              byte[] bCardUID,
                                              ref byte bUidSize);

        //--------------------------------------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderUISignal")]
        public static extern DL_STATUS ReaderUISignal(byte light_signal_mode, byte beep_signal_mode);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderType")]
        public static extern DL_STATUS GetReaderType(ref uint lpulReaderType);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderSerialDescription")]
        public static extern DL_STATUS GetReaderSerialDescription(byte[] pSerialDescription);

        //--------------------------------------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDlogicCardType")]
        public static extern DL_STATUS GetDlogicCardType(ref byte lpucCardType);

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDllVersion")]
        public static extern uint GetDllVersion();

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderHardwareVersion")]
        public static extern DL_STATUS GetReaderHardwareVersion(ref byte version_major, ref byte version_minor);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetReaderFirmwareVersion")]
        public static extern DL_STATUS GetReaderFirmwareVersion(out byte version_major, out byte version_minor);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetBuildNumber")]
        public static extern DL_STATUS GetBuildNumber(out byte build);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearRead")]
        public static extern DL_STATUS LinearRead(byte[] data, ushort linear_address, uint length, ref ushort bytes_returned, byte key_mode, byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearRead_PK")]
        public static extern DL_STATUS LinearRead_PK(byte[] data, ushort linear_address, ushort length, ref ushort bytes_returned, byte key_mode, [In] byte[] key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearWrite")]
        public static extern DL_STATUS LinearWrite([In] byte[] data, ushort linear_address, ushort length, ref ushort bytes_returned, byte key_mode, byte key_index);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "LinearWrite_PK")]
        public static extern DL_STATUS LinearWrite_PK([In] byte[] data, ushort linear_address, ushort length, ref ushort bytes_returned, byte key_mode, [In] byte[] key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "uFR_int_DesfireWriteAesKey")]
        public static extern DL_STATUS uFR_int_DesfireWriteAesKey(byte key_index, [In] byte[] key);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderKeysLock")]
        public static extern DL_STATUS ReaderKeysLock([In] byte[] password);

        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "ReaderKeysUnlock")]
        public static extern DL_STATUS ReaderKeysUnlock([In] byte[] password);

        //---------------------------------------------------------------------
        //------------------------  NT4H FUNCTIONS  ---------------------------
        //---------------------------------------------------------------------

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_set_global_parameters")]
        public static extern DL_STATUS nt4h_set_global_parameters(byte file_no, byte key_no, byte communication_mode);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_change_standard_file_settings_pk")]
        public static extern DL_STATUS nt4h_change_standard_file_settings_pk([In] byte[] aes_key_ext, byte file_no, byte key_no, byte communication_mode,
                                                                             byte new_communication_mode, byte read_key_no, byte write_key_no,  byte read_write_key_no, byte change_key_no);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_change_standard_file_settings")]
        public static extern DL_STATUS nt4h_change_standard_file_settings(byte aes_key_no, byte file_no, byte key_no, byte communication_mode,
                                                                          byte new_communication_mode, byte read_key_no, byte write_key_no, byte read_write_key_no, byte change_key_no);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_change_sdm_file_settings_pk")]
        public static extern DL_STATUS nt4h_change_sdm_file_settings_pk([In] byte[] aes_key_ext, byte file_no, byte key_no, byte curr_communication_mode, byte new_communication_mode,
                                                                        byte read_key_no, byte write_key_no, byte read_write_key_no, byte change_key_no, 
                                                                        byte uid_enable, byte read_ctr_enable, byte read_ctr_limit_enable, byte enc_file_data_enable,
                                                                        byte meta_data_key_no, byte file_data_read_key_no, byte read_ctr_key_no,
                                                                        uint uid_offset, uint read_ctr_offset, uint picc_data_offset, uint mac_input_offset,
                                                                        uint enc_offset, uint enc_length, uint mac_offset, uint read_ctr_limit);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_change_sdm_file_settings")]
        public static extern DL_STATUS nt4h_change_sdm_file_settings(byte aes_key_no, byte file_no, byte key_no, byte curr_communication_mode, byte new_communication_mode,
                                                                        byte read_key_no, byte write_key_no, byte read_write_key_no, byte change_key_no,
                                                                        byte uid_enable, byte read_ctr_enable, byte read_ctr_limit_enable, byte enc_file_data_enable,
                                                                        byte meta_data_key_no, byte file_data_read_key_no, byte read_ctr_key_no,
                                                                        uint uid_offset, uint read_ctr_offset, uint picc_data_offset, uint mac_input_offset,
                                                                        uint enc_offset, uint enc_length, uint mac_offset, uint read_ctr_limit);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_get_file_settings")]
        public static extern DL_STATUS nt4h_get_file_settings(byte file_no, ref byte file_type, ref byte communication_mode, ref byte sdm_enable, ref uint file_size,
                                                              ref byte read_key_no, ref byte write_key_no, ref byte read_write_key_no, ref byte change_key_no,
                                                              ref byte uid_enable, ref byte read_ctr_enable, ref byte read_ctr_limit_enable, ref byte enc_file_data_enable,
                                                              ref byte meta_data_key_no, ref byte file_data_read_key_no, ref byte read_ctr_key_no,
                                                              ref uint uid_offset, ref uint read_ctr_offset, ref uint picc_data_offset, ref uint mac_input_offset,
                                                              ref uint enc_offset, ref uint enc_length, ref uint mac_offset, ref uint read_ctr_limit);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_set_rid_pk")]
        public static extern DL_STATUS nt4h_set_rid_pk([In] byte[] aes_key_ext);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_set_rid")]
        public static extern DL_STATUS nt4h_set_rid(byte aes_key_no);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_get_uid_pk")]
        public static extern DL_STATUS nt4h_get_uid_pk([In] byte[] auth_key, byte key_no, byte[] uid);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_get_uid")]
        public static extern DL_STATUS nt4h_get_uid(byte auth_key_no, byte key_no, byte[] uid);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_change_key_pk")]
        public static extern DL_STATUS nt4h_change_key_pk([In] byte[] auth_key, byte key_no, [In] byte[] new_key, [In] byte[] old_key);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_change_key")]
        public static extern DL_STATUS nt4h_change_key(byte auth_key_no, byte key_no, [In] byte[] new_key, [In] byte[] old_key);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_get_sdm_ctr_pk")]
        public static extern DL_STATUS nt4h_get_sdm_ctr_pk([In] byte[] auth_key, byte file_no, byte key_no, ref uint sdm_read_ctr);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_get_sdm_ctr")]
        public static extern DL_STATUS nt4h_get_sdm_ctr(byte auth_key_no, byte file_no, byte key_no, ref uint sdm_read_ctr);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_get_sdm_ctr_no_auth")]
        public static extern DL_STATUS nt4h_get_sdm_ctr_no_auth(byte file_no, ref uint sdm_read_ctr);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_check_sdm_mac")]
        public static extern DL_STATUS nt4h_check_sdm_mac(uint sdm_read_counter, [In] byte[] uid, [In] byte[] auth_key, [In] byte[] mac_in_data, byte mac_in_len, [In] byte[] sdm_mac);

        //UFR_STATUS DL_API nt4h_decrypt_sdm_enc_file_data(uint32_t smd_read_counter, IN uint8_t *uid, IN uint8_t *auth_key, IN uint8_t *enc_file_data, IN uint8_t enc_file_data_len);
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_decrypt_sdm_enc_file_data")]
        public static extern DL_STATUS nt4h_decrypt_sdm_enc_file_data(uint sdm_read_counter, [In] byte[] uid, [In] byte[] auth_key, [In] byte[] enc_file_data, byte enc_file_data_len);


        //---------------------------------------------------------------------
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, EntryPoint = "nt4h_decrypt_picc_data")]
        public static extern DL_STATUS nt4h_decrypt_picc_data([In] byte[] picc_data, [In] byte[] auth_key, ref byte picc_data_tag, byte[] uid, ref uint sdm_read_cnt);


    }
}
