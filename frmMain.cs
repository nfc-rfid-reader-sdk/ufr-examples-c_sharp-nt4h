using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using uFR;

namespace ufr_examples_c_sharp_nt4h
{

    public partial class frmMain : Form
    {


        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        
        public frmMain()
        {
            InitializeComponent();

            uint dll_ver = 0;
            byte dll_major_ver = 0;
            byte dll_minor_ver = 0;
            ushort dll_build = 0;

            try { // try to fetch dll version 
                dll_ver = uFCoder.GetDllVersion();

                dll_major_ver = (byte)dll_ver;
                dll_minor_ver = (byte)(dll_ver >> 8);
                dll_build = (byte)(dll_ver >> 16);

                DevInfoDLL.Text = " DLL: " + (dll_major_ver) + "." + (dll_minor_ver) +
                      "." + (dll_build);
            } catch (Exception ex)
            {
                DevInfoDLL.Text = "NO DLL FOUND!";
            }
        }

        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.d-logic.net/code/nfc-rfid-reader-sdk");
        }

        private void AdvancedCheck_CheckedChanged(object sender, EventArgs e)
        {
            labelReaderType.Enabled = !labelReaderType.Enabled;
            labelPortName.Enabled = !labelPortName.Enabled;
            labelPortInterface.Enabled = !labelPortInterface.Enabled;
            labelArg.Enabled = !labelArg.Enabled;

            txtReaderType.Enabled = !txtReaderType.Enabled;
            txtPortName.Enabled = !txtPortName.Enabled;
            txtPortInterface.Enabled = !txtPortInterface.Enabled;
            txtArg.Enabled = !txtArg.Enabled;
        }

        private void btnReaderOpen_Click(object sender, EventArgs e)
        {
            DL_STATUS status = DL_STATUS.UFR_READER_PORT_NOT_OPENED; 
            // device data
            byte[] reader_sn = new byte[8];
            byte fw_major_ver = 0;
            byte fw_minor_ver = 0;
            byte fw_build = 0;
            byte hw_major = 0;
            byte hw_minor = 0;

            if (AdvancedCheck.Checked)
            {
                string reader_type_ex = txtReaderType.Text;
                string port_name = txtPortName.Text;
                string port_interface = txtPortInterface.Text;
                string arg = txtArg.Text;
                UInt32 reader_type_int = 0, port_interface_int = 0;

                try
                {
                    reader_type_int = Convert.ToUInt32(reader_type_ex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid Advanced options parameter: Reader type");
                    txtReaderType.Focus();
                    return;
                }

                try
                {
                    port_interface_int = (UInt32)port_interface[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid Advanced options parameter: Port interface");
                    txtPortInterface.Focus();
                    return;
                }

                status = uFCoder.ReaderOpenEx(reader_type_int, port_name, port_interface_int, arg);

            }
            else
            {
                status = uFCoder.ReaderOpen();
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Error: " + status;
                statusReader.Text = "Reader not opened.";
                return;
            }
            else
            {
                uFCoder.ReaderUISignal(1, 1);

                status = uFCoder.GetReaderSerialDescription(reader_sn);

                status |= uFCoder.GetReaderHardwareVersion(ref hw_major, ref hw_minor);

                status |= uFCoder.GetReaderFirmwareVersion(out fw_major_ver, out fw_minor_ver);

                status |= uFCoder.GetBuildNumber(out fw_build);


                DevInfoSN.Text = " SN : " + System.Text.Encoding.UTF8.GetString(reader_sn);

                DevInfoHW.Text = " HW : " + (int)hw_major + "." + hw_minor;

                DevInfoFW.Text = " FW : " + (fw_major_ver) + "." +
                                        (fw_minor_ver) + "." +
                                        (fw_build);

            }

            if (status == DL_STATUS.UFR_OK)
            {
                statusReader.Text = " CONNECTED ";
                statusResult.Text = "Status: " + status;
            }


        }

        private void btnGetFileSettings_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte file_no = 0, file_type = 0, communication_mode = 0, sdm_enable = 0;
            byte read_key_no = 0, write_key_no = 0, read_write_key_no = 0, change_key_no = 0;
            byte uid_enable = 0, read_ctr_enable = 0, read_ctr_limit_enable = 0, enc_file_data_enable = 0;
            byte meta_data_key_no = 0, file_data_read_key_no = 0, read_ctr_key_no = 0;
            uint file_size = 0, uid_offset = 0, read_ctr_offset = 0, picc_data_offset = 0;
            uint mac_input_offset = 0, enc_offset = 0, enc_length = 0, mac_offset = 0, read_ctr_limit = 0;

            try
            {
                file_no = Byte.Parse(txtGetFileSettings_FileNo.Text);
            } catch (Exception ex)
            {
                txtGetFileSettings_FileNo.Focus();
                MessageBox.Show("Invalid parameter: File No.");
                return;
            }

            status = uFCoder.nt4h_get_file_settings(file_no, ref file_type, ref communication_mode, ref sdm_enable, ref file_size,
                                                    ref read_key_no, ref write_key_no, ref read_write_key_no, ref change_key_no,
                                                    ref uid_enable, ref read_ctr_enable, ref read_ctr_limit_enable, ref enc_file_data_enable,
                                                    ref meta_data_key_no, ref file_data_read_key_no, ref read_ctr_key_no,
                                                    ref uid_offset, ref read_ctr_offset, ref picc_data_offset, ref mac_input_offset,
                                                    ref enc_offset, ref enc_length, ref mac_offset, ref read_ctr_limit);

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Get file settings failed: " + status;
            } else
            {
                txtGetFileSettings_FileType.Text = file_type.ToString();
                txtGetFileSettings_CommunicationMode.Text = communication_mode.ToString();
                txtGetFileSettings_SDMEnable.Text = sdm_enable.ToString();
                txtGetFileSettings_FileSize.Text = file_size.ToString();
                txtGetFileSettings_ReadKeyNo.Text = read_key_no.ToString();
                txtGetFileSettings_WriteKeyNo.Text = write_key_no.ToString();
                txtGetFileSettings_ReadWriteKeyNo.Text = read_write_key_no.ToString();
                txtGetFileSettings_ChangeKeyNo.Text = change_key_no.ToString();
                txtGetFileSettings_UIDEnable.Text = uid_enable.ToString();
                txtGetFileSettings_ReadCTREnable.Text = read_ctr_enable.ToString();
                txtGetFileSettings_ReadCTRLimitEnable.Text = read_ctr_limit_enable.ToString();
                txtGetFileSettings_ENCFileDataEnable.Text = enc_file_data_enable.ToString();
                txtGetFileSettings_MetaDataKeyNo.Text = meta_data_key_no.ToString();
                txtGetFileSettings_FileDataReadKeyNo.Text = file_data_read_key_no.ToString();
                txtGetFileSettings_ReadCTRKeyNo.Text = read_ctr_key_no.ToString();
                txtGetFileSettings_UIDOffset.Text = uid_offset.ToString();
                txtGetFileSettings_ReadCTROffset.Text = read_ctr_offset.ToString();
                txtGetFileSettings_PiccDataOffset.Text = picc_data_offset.ToString();
                txtGetFileSettings_MacInputOffset.Text = mac_input_offset.ToString();
                txtGetFileSettings_EncOffset.Text = enc_offset.ToString();
                txtGetFileSettings_EncLength.Text = enc_length.ToString();
                txtGetFileSettings_MacOffset.Text = mac_offset.ToString();
                txtGetFileSettings_ReadCtrLimit.Text = read_ctr_limit.ToString();

                statusResult.Text = "Get file settings successful: " + status;
            }
        }

        private void btnSetFileSettings_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte[] aes_key_ext = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte aes_key_no = 0, key_no = 0;
            byte file_no = 0, file_type = 0, communication_mode = 0, sdm_enable = 0;
            byte read_key_no = 0, write_key_no = 0, read_write_key_no = 0, change_key_no = 0;
            byte uid_enable = 0, read_ctr_enable = 0, read_ctr_limit_enable = 0, enc_file_data_enable = 0;
            byte meta_data_key_no = 0, file_data_read_key_no = 0, read_ctr_key_no = 0;
            uint file_size = 0, uid_offset = 0, read_ctr_offset = 0, picc_data_offset = 0;
            uint mac_input_offset = 0, enc_offset = 0, enc_length = 0, mac_offset = 0, read_ctr_limit = 0;
            byte curr_communication_mode = 3;

            try
            {
                file_no = Byte.Parse(txtSetFileSettings_FileNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_FileNo.Focus();
                MessageBox.Show("Invalid parameter: File No.");
                return;
            }

            try
            {
                key_no = Byte.Parse(txtSetFileSettings_KeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_KeyNo.Focus();
                MessageBox.Show("Invalid parameter: Key No.");
                return;
            }

            try
            {
                file_type = Byte.Parse(txtSetFileSettings_FileType.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_FileType.Focus();
                MessageBox.Show("Invalid parameter: File Type.");
                return;
            }

            try
            {
                communication_mode = Byte.Parse(txtSetFileSettings_CommunicationMode.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_CommunicationMode.Focus();
                MessageBox.Show("Invalid parameter: Communication Mode.");
                return;
            }

            try
            {
                sdm_enable = Byte.Parse(txtSetFileSettings_SDMEnable.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_SDMEnable.Focus();
                MessageBox.Show("Invalid parameter: SDM Enable.");
                return;
            }

            try
            {
                file_size = UInt32.Parse(txtSetFileSettings_FileSize.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_FileSize.Focus();
                MessageBox.Show("Invalid parameter: File Size.");
                return;
            }

            try
            {
                read_key_no = Byte.Parse(txtSetFileSettings_ReadKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadKeyNo.Focus();
                MessageBox.Show("Invalid parameter: Read Key No.");
                return;
            }

            try
            {
                write_key_no = Byte.Parse(txtSetFileSettings_WriteKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_WriteKeyNo.Focus();
                MessageBox.Show("Invalid parameter: Write Key No.");
                return;
            }

            try
            {
                read_write_key_no = Byte.Parse(txtSetFileSettings_ReadWriteKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadWriteKeyNo.Focus();
                MessageBox.Show("Invalid parameter: Read Write Key No.");
                return;
            }

            try
            {
                change_key_no = Byte.Parse(txtSetFileSettings_ChangeKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ChangeKeyNo.Focus();
                MessageBox.Show("Invalid parameter: Change Key No.");
                return;
            }

            try
            {
                uid_enable = Byte.Parse(txtSetFileSettings_UIDEnable.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_UIDEnable.Focus();
                MessageBox.Show("Invalid parameter: UID enable.");
                return;
            }

            try
            {
                read_ctr_enable = Byte.Parse(txtSetFileSettings_ReadCTREnable.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadCTREnable.Focus();
                MessageBox.Show("Invalid parameter: Read ctr enable.");
                return;
            }

            try
            {
                read_ctr_limit_enable = Byte.Parse(txtSetFileSettings_ReadCTRLimitEnable.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadCTRLimitEnable.Focus();
                MessageBox.Show("Invalid parameter: Read ctr limit enable.");
                return;
            }

            try
            {
                enc_file_data_enable = Byte.Parse(txtSetFileSettings_ENCFileDataEnable.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ENCFileDataEnable.Focus();
                MessageBox.Show("Invalid parameter: Enc file data enable.");
                return;
            }

            try
            {
                meta_data_key_no = Byte.Parse(txtSetFileSettings_MetaDataKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_MetaDataKeyNo.Focus();
                MessageBox.Show("Invalid parameter: Meta data key no.");
                return;
            }

            try
            {
                file_data_read_key_no = Byte.Parse(txtSetFileSettings_FileDataReadKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_FileDataReadKeyNo.Focus();
                MessageBox.Show("Invalid parameter: File data read key no.");
                return;
            }

            try
            {
                read_ctr_key_no = Byte.Parse(txtSetFileSettings_ReadCTRKeyNo.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadCTRKeyNo.Focus();
                MessageBox.Show("Invalid parameter: Read ctr key no.");
                return;
            }

            try
            {
                uid_offset = UInt32.Parse(txtSetFileSettings_UIDOffset.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_UIDOffset.Focus();
                MessageBox.Show("Invalid parameter: UID offset.");
                return;
            }

            try
            {
                uid_offset = UInt32.Parse(txtSetFileSettings_ReadCTROffset.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadCTROffset.Focus();
                MessageBox.Show("Invalid parameter: Read ctr offset.");
                return;
            }

            try
            {
                picc_data_offset = UInt32.Parse(txtSetFileSettings_PICCDataOffset.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_PICCDataOffset.Focus();
                MessageBox.Show("Invalid parameter: PICC data offset.");
                return;
            }

            try
            {
                mac_input_offset = UInt32.Parse(txtSetFileSettings_MACInputOffset.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_MACInputOffset.Focus();
                MessageBox.Show("Invalid parameter: MAC input offset.");
                return;
            }

            try
            {
                enc_offset = UInt32.Parse(txtSetFileSettings_ENCOffset.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ENCOffset.Focus();
                MessageBox.Show("Invalid parameter: ENC offset.");
                return;
            }

            try
            {
                enc_length = UInt32.Parse(txtSetFileSettings_ENCLength.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ENCLength.Focus();
                MessageBox.Show("Invalid parameter: ENC length.");
                return;
            }

            try
            {
                mac_offset = UInt32.Parse(txtSetFileSettings_MACOffset.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_MACOffset.Focus();
                MessageBox.Show("Invalid parameter: MAC offset.");
                return;
            }

            try
            {
                read_ctr_limit = UInt32.Parse(txtSetFileSettings_ReadCTRLimit.Text);
            }
            catch (Exception ex)
            {
                txtSetFileSettings_ReadCTRLimit.Focus();
                MessageBox.Show("Invalid parameter: Read ctr limit.");
                return;
            }


            if (rbSetFileSettings_ProvidedKey.Checked)
            {
                if (txtSetFileSettings_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtSetFileSettings_ProvidedKey.Focus();
                    return;
                }

                aes_key_no = 0;

                aes_key_ext = StringToByteArray(txtSetFileSettings_ProvidedKey.Text);

            } else
            {
                try
                {
                    aes_key_no = Byte.Parse(txtSetFileSettings_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtSetFileSettings_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }
            }

            if (file_type == 0) //standard data file
            {
                if (rbSetFileSettings_ProvidedKey.Checked)
                {
                    status = uFCoder.nt4h_change_standard_file_settings_pk(aes_key_ext, file_no, key_no, curr_communication_mode,
                                                                       communication_mode, read_key_no, write_key_no, read_write_key_no, change_key_no);
                } else
                {
                    status = uFCoder.nt4h_change_standard_file_settings(aes_key_no, file_no, key_no, curr_communication_mode,
                                                                       communication_mode, read_key_no, write_key_no, read_write_key_no, change_key_no);
                }
            } else
            {
                if (rbSetFileSettings_ProvidedKey.Checked)
                {
                    status = uFCoder.nt4h_change_sdm_file_settings_pk(aes_key_ext, file_no, key_no, curr_communication_mode,
                                                        communication_mode, read_key_no, write_key_no, read_write_key_no, change_key_no,
                                                        uid_enable, read_ctr_enable, read_ctr_limit_enable, enc_file_data_enable,
                                                        meta_data_key_no, file_data_read_key_no, read_ctr_key_no,
                                                        uid_offset, read_ctr_offset, picc_data_offset,
                                                        mac_input_offset, enc_offset, enc_length, mac_offset, read_ctr_limit);
                } else
                {
                    status = uFCoder.nt4h_change_sdm_file_settings(aes_key_no, file_no, key_no, curr_communication_mode,
                                                                    communication_mode, read_key_no, write_key_no, read_write_key_no, change_key_no,
                                                                    uid_enable, read_ctr_enable, read_ctr_limit_enable, enc_file_data_enable,
                                                                    meta_data_key_no, file_data_read_key_no, read_ctr_key_no,
                                                                    uid_offset, read_ctr_offset, picc_data_offset,
                                                                    mac_input_offset, enc_offset, enc_length, mac_offset, read_ctr_limit);
                }
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Set file settings failed: " + status;
            }
            else
            {
                statusResult.Text = "Set file settings successful: " + status;
            }

        }

        private void btnGetUID_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte[] auth_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte key_no = 0;
            byte[] uid = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte internal_key_no = 0;

            try
            {
                key_no = Byte.Parse(txtGetUID_KeyNo.Text);
            }
            catch (Exception ex)
            {
                txtGetUID_KeyNo.Focus();
                MessageBox.Show("Invalid parameter: Key No.");
                return;
            }

            if (rbGetUID_ProvidedKey.Checked)
            {
                if (txtGetUID_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtGetUID_ProvidedKey.Focus();
                    return;
                }

                auth_key = StringToByteArray(txtGetUID_ProvidedKey.Text);

                status = uFCoder.nt4h_get_uid_pk(auth_key, key_no, uid);

            } else
            {
                try
                {
                    internal_key_no = Byte.Parse(txtGetUID_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtGetUID_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }

                status = uFCoder.nt4h_get_uid(internal_key_no, key_no, uid);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Get UID failed: " + status;
                txtGetUID_UID.Text = "";
            }
            else
            {
                statusResult.Text = "Get UID successful: " + status;
                txtGetUID_UID.Text = BitConverter.ToString(uid).Replace("-", ":");
            }
        }

        private void btnSetRID_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte[] auth_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte internal_key_no = 0;

            if (rbSetRandomID_ProvidedKey.Checked)
            {
                if (txtSetRandomID_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtSetRandomID_ProvidedKey.Focus();
                    return;
                }

                auth_key = StringToByteArray(txtGetUID_ProvidedKey.Text);

                status = uFCoder.nt4h_set_rid_pk(auth_key);
            }
            else
            {
                try
                {
                    internal_key_no = Byte.Parse(txtSetRandomID_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtSetRandomID_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }

                status = uFCoder.nt4h_set_rid(internal_key_no);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Set random ID failed: " + status;
                txtGetUID_UID.Text = "";
            }
            else
            {
                statusResult.Text = "Set random ID successful: " + status;
            }
        }

        private void btnChangeAESKey_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte key_no = 0, internal_key_no = 0;
            byte[] auth_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] new_key = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            byte[] old_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            txtLinearRead_DataRead.Text = "";
            txtLinearRead_BytesReturned.Text = "0";


            try
            {
                key_no = Byte.Parse(txtChangeAESKey_KeyNo.Text);
            }
            catch (Exception ex)
            {
                txtChangeAESKey_KeyNo.Focus();
                MessageBox.Show("Invalid parameter: Key No.");
                return;
            }


            if (txtChangeAESKey_NewKey.Text.Length != 32)
            {
                MessageBox.Show("New key must be 16 bytes long!");
                txtChangeAESKey_NewKey.Focus();
                return;
            } else
            {
                new_key = StringToByteArray(txtChangeAESKey_NewKey.Text);
            }

            if (txtChangeAESKey_OldKey.Text.Length != 32)
            {
                MessageBox.Show("Old key must be 16 bytes long!");
                txtChangeAESKey_OldKey.Focus();
                return;
            }
            else
            {
                old_key = StringToByteArray(txtChangeAESKey_OldKey.Text);
            }

            if (rbChangeAESKey_ProvidedKey.Checked)
            {
                if (txtChangeAESKey_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtChangeAESKey_ProvidedKey.Focus();
                    return;
                }

                auth_key = StringToByteArray(txtGetUID_ProvidedKey.Text);

                status = uFCoder.nt4h_change_key_pk(auth_key, key_no, new_key, old_key);
            }
            else
            {
                try
                {
                    internal_key_no = Byte.Parse(txtChangeAESKey_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtChangeAESKey_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }

                status = uFCoder.nt4h_change_key(internal_key_no, key_no, new_key, old_key);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Change AES key failed: " + status;
                txtGetUID_UID.Text = "";
            }
            else
            {
                statusResult.Text = "Change AES key successful: " + status;
            }
        }

        private void btnLinearRead_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte file_no = 0, key_no = 0, communication_mode = 0, internal_key_no = 0;
            ushort linear_address = 0, length = 0, bytes_returned = 0;
            byte[] auth_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] data = new byte[1024];

            try
            {
                file_no = Byte.Parse(txtLinearRead_FileNo.Text);
            }
            catch (Exception ex)
            {
                txtLinearRead_FileNo.Focus();
                MessageBox.Show("Invalid parameter: File no.");
                return;
            }

            try
            {
                key_no = Byte.Parse(txtLinearRead_KeyNo.Text);
            }
            catch (Exception ex)
            {
                txtLinearRead_KeyNo.Focus();
                MessageBox.Show("Invalid parameter: Key no.");
                return;
            }

            try
            {
                communication_mode = Byte.Parse(txtLinearRead_CommunicationMode.Text);
            }
            catch (Exception ex)
            {
                txtLinearRead_CommunicationMode.Focus();
                MessageBox.Show("Invalid parameter: Communication mode.");
                return;
            }
            // set global parameters
            status = uFCoder.nt4h_set_global_parameters(file_no, key_no, communication_mode);
            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Set global parameters failed: " + status;
                return;
            }

            try
            {
                linear_address = UInt16.Parse(txtLinearRead_LinearAddress.Text);
            }
            catch (Exception ex)
            {
                txtLinearRead_LinearAddress.Focus();
                MessageBox.Show("Invalid parameter: Linear address.");
                return;
            }

            try
            {
                length = UInt16.Parse(txtLinearRead_Length.Text);
            }
            catch (Exception ex)
            {
                txtLinearRead_Length.Focus();
                MessageBox.Show("Invalid parameter: Length.");
                return;
            }

            if (rbLinearRead_ProvidedKey.Checked)
            {
                if (txtLinearRead_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtLinearRead_ProvidedKey.Focus();
                    return;
                }

                auth_key = StringToByteArray(txtGetUID_ProvidedKey.Text);

                status = uFCoder.LinearRead_PK(data, linear_address, length, ref bytes_returned, (byte)T4T_AUTHENTICATION.T4T_PK_PWD_AUTH, auth_key);
            }
            else if (rbLinearRead_ReaderKey.Checked)
            {
                try
                {
                    internal_key_no = Byte.Parse(txtLinearRead_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtLinearRead_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }

                status = uFCoder.LinearRead(data, linear_address, length, ref bytes_returned, (byte)T4T_AUTHENTICATION.T4T_RKA_PWD_AUTH, internal_key_no);

            } else
            {
                status = uFCoder.LinearRead(data, linear_address, length, ref bytes_returned, (byte)T4T_AUTHENTICATION.T4T_WITHOUT_PWD_AUTH, 0);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Linear read failed: " + status;
                txtLinearRead_BytesReturned.Text = "0";
                txtLinearRead_DataRead.Text = "";
            }
            else
            {
                statusResult.Text = "Linear read successful: " + status;
                txtLinearRead_BytesReturned.Text = bytes_returned.ToString();
                if (rbLinearRead_Hex.Checked)
                    txtLinearRead_DataRead.Text = BitConverter.ToString(data, 0, bytes_returned).Replace("-", ":");
                else
                {
                    txtLinearRead_DataRead.Text = System.Text.Encoding.Default.GetString(data, 0, bytes_returned);
                    txtLinearRead_DataRead.Text = System.Text.RegularExpressions.Regex.Replace(txtLinearRead_DataRead.Text, @"\p{C}+", String.Empty);

                }
            }

        }

        private void btnLinearWrite_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte file_no = 0, key_no = 0, communication_mode = 0, internal_key_no = 0;
            ushort linear_address = 0, length = 0, bytes_written = 0;
            ushort byte_count = 0;
            byte[] auth_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] data = new byte[1024];
            try
            {
                file_no = Byte.Parse(txtLinearWrite_FileNo.Text);
            }
            catch (Exception ex)
            {
                txtLinearWrite_FileNo.Focus();
                MessageBox.Show("Invalid parameter: File no.");
                return;
            }

            try
            {
                key_no = Byte.Parse(txtLinearWrite_KeyNo.Text);
            }
            catch (Exception ex)
            {
                txtLinearWrite_KeyNo.Focus();
                MessageBox.Show("Invalid parameter: Key no.");
                return;
            }

            try
            {
                communication_mode = Byte.Parse(txtLinearWrite_CommunicationMode.Text);
            }
            catch (Exception ex)
            {
                txtLinearWrite_CommunicationMode.Focus();
                MessageBox.Show("Invalid parameter: Communication mode.");
                return;
            }


            // set global parameters
            status = uFCoder.nt4h_set_global_parameters(file_no, key_no, communication_mode);
            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Set global parameters failed: " + status;
                return;
            }

            try
            {
                linear_address = UInt16.Parse(txtLinearWrite_LinearAddress.Text);
            }
            catch (Exception ex)
            {
                txtLinearWrite_LinearAddress.Focus();
                MessageBox.Show("Invalid parameter: Linear address.");
                return;
            }

            if (rbLinearWrite_Hex.Checked)
            {
                if (txtLinearWrite_DataToWrite.Text.Length % 2 != 0)
                {
                    MessageBox.Show("Invalid data. Check if byte count is an even number.");
                    txtLinearWrite_DataToWrite.Focus();
                    return;
                }
                data = StringToByteArray(txtLinearWrite_DataToWrite.Text);
            } else
            {
                data = System.Text.Encoding.ASCII.GetBytes(txtLinearWrite_DataToWrite.Text);
            }

            length = (ushort)data.Length;


            if (rbLinearWrite_ProvidedKey.Checked)
            {
                if (txtLinearWrite_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtLinearWrite_ProvidedKey.Focus();
                    return;
                }

                auth_key = StringToByteArray(txtGetUID_ProvidedKey.Text);

                status = uFCoder.LinearWrite_PK(data, linear_address, length, ref bytes_written, (byte)T4T_AUTHENTICATION.T4T_PK_PWD_AUTH, auth_key);
            }
            else if (rbLinearWrite_ReaderKey.Checked)
            {
                try
                {
                    internal_key_no = Byte.Parse(txtLinearWrite_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtLinearWrite_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }

                status = uFCoder.LinearWrite(data, linear_address, length, ref bytes_written, (byte)T4T_AUTHENTICATION.T4T_RKA_PWD_AUTH, internal_key_no);

            }
            else
            {
                status = uFCoder.LinearWrite(data, linear_address, length, ref bytes_written, (byte)T4T_AUTHENTICATION.T4T_WITHOUT_PWD_AUTH, 0);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Linear write failed: " + status;
                txtLinearWrite_BytesWritten.Text = "0";
                txtLinearRead_DataRead.Text = "";
            }
            else
            {
                statusResult.Text = "Linear write successful: " + status;
                txtLinearWrite_BytesWritten.Text = bytes_written.ToString();

            }
        }

        private void txtLinearWrite_DataToWrite_TextChanged(object sender, EventArgs e)
        {
            if (rbLinearWrite_Hex.Checked)
            {
                txtLinearWrite_Length.Text = (txtLinearWrite_DataToWrite.Text.Length / 2).ToString();
            } else
            {
                txtLinearWrite_Length.Text = txtLinearWrite_DataToWrite.Text.Length.ToString();
            }
        }

        private void rbLinearWrite_ASCII_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLinearWrite_Hex.Checked)
            {
                txtLinearWrite_Length.Text = (txtLinearWrite_DataToWrite.Text.Length / 2).ToString();
            }
            else
            {
                txtLinearWrite_Length.Text = txtLinearWrite_DataToWrite.Text.Length.ToString();
            }
        }

        private void btnGetSDMReadingCounter_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte[] auth_key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte aes_internal_key_no = 0;
            byte file_no = 0, key_no = 0;
            uint sdm_read_ctr = 0;

            try
            {
                file_no = Byte.Parse(txtGetSDMReadingCounter_FileNo.Text);
            }
            catch (Exception ex)
            {
                txtGetSDMReadingCounter_FileNo.Focus();
                MessageBox.Show("Invalid parameter: File no.");
                return;
            }

            try
            {
                key_no = Byte.Parse(txtGetSDMReadingCounter_KeyNo.Text);
            }
            catch (Exception ex)
            {
                txtGetSDMReadingCounter_KeyNo.Focus();
                MessageBox.Show("Invalid parameter: Key no.");
                return;
            }


            if (rbGetSDMReadingCounter_ProvidedKey.Checked)
            {
                if (txtGetSDMReadingCounter_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtGetSDMReadingCounter_ProvidedKey.Focus();
                    return;
                }

                auth_key = StringToByteArray(txtGetSDMReadingCounter_ProvidedKey.Text);

                status = uFCoder.nt4h_get_sdm_ctr_pk(auth_key, file_no, key_no, ref sdm_read_ctr);

            } else if (rbGetSDMReadingCounter_ReaderKey.Checked)
            {
                try
                {
                    aes_internal_key_no = Byte.Parse(txtGetSDMReadingCounter_KeyIndex.Text);
                }
                catch (Exception ex)
                {
                    txtGetSDMReadingCounter_KeyIndex.Focus();
                    MessageBox.Show("Invalid parameter: Key index.");
                    return;
                }

                status = uFCoder.nt4h_get_sdm_ctr(aes_internal_key_no, file_no, key_no, ref sdm_read_ctr);
            }
            else
            {
                status = uFCoder.nt4h_get_sdm_ctr_no_auth(file_no, ref sdm_read_ctr);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Get SDM counter failed: " + status;
                txtGetSDMReadingCounter_SDMReadingCounter.Text = "0";
            }
            else
            {
                statusResult.Text = "Linear write successful: " + status;
                txtGetSDMReadingCounter_SDMReadingCounter.Text = sdm_read_ctr.ToString();
            }
        }

        private void btnStoreReaderKey_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte key_index = 0;
            byte[] key = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            status = uFCoder.uFR_int_DesfireWriteAesKey(key_index, key);

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Store AES key failed: " + status;
            }
            else
            {
                statusResult.Text = "Store AES key successful: " + status;
            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            DL_STATUS status;

            byte[] password = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (txtReaderKeysPassword.Text.Length != 8)
            {
                MessageBox.Show("Password must be 8 characters long!");
                txtReaderKeysPassword.Focus();
                return;
            }

            password = System.Text.Encoding.ASCII.GetBytes(txtReaderKeysPassword.Text);

            status = uFCoder.ReaderKeysLock(password);

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Reader keys lock failed: " + status;
            }
            else
            {
                statusResult.Text = "Reader keys lock successful: " + status;
            }

        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            DL_STATUS status;

            byte[] password = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (txtReaderKeysPassword.Text.Length != 8)
            {
                MessageBox.Show("Password must be 8 characters long!");
                txtReaderKeysPassword.Focus();
                return;
            }

            password = System.Text.Encoding.ASCII.GetBytes(txtReaderKeysPassword.Text);

            status = uFCoder.ReaderKeysUnlock(password);

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Reader keys unlock failed: " + status;
            }
            else
            {
                statusResult.Text = "Reader keys unlock successful: " + status;
            }
        }

        private void btnSDMWrite_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte[] aes_key_ext = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte file_no = 0, key_no = 0, communication_mode = 0;
            byte read_key_no = 0, write_key_no = 0, read_write_key_no = 0, new_change_key_no = 0;
            byte internal_key_no = 0;
            byte uid_enable = 0, read_ctr_enable = 0, read_ctr_limit_enable = 0, enc_file_data_enable = 0;
            byte meta_data_key_no = 0, file_data_read_key_no = 0, read_ctr_key_no = 0;
            uint uid_offset = 0, read_ctr_offset = 0, picc_data_offset = 0, enc_length = 0, mac_offset = 0, read_ctr_limit = 0;
            uint mac_input_offset = 0, enc_offset = 0;
            int ndef_len = 0;
            int url_len = 0;
            byte[] ndef_data = new byte[256];
            byte[] url;

            file_no = 2; // NDEF File
            communication_mode = 0; //plain
            read_key_no = 0x0E; // free read access

            //file access key number
            try
            {
                key_no = Byte.Parse(txtSDMWrite_KeyNo.Text);
            } catch (Exception ex)
            {
                MessageBox.Show("Invalid parameter: Key No");
                txtSDMWrite_KeyNo.Focus();
                return;
            }

            try
            {
                write_key_no = Byte.Parse(txtSDMWrite_WriteKeyNo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid parameter: Write key no");
                txtSDMWrite_WriteKeyNo.Focus();
                return;
            }

            try
            {
                read_write_key_no = Byte.Parse(txtSDMWrite_ReadWriteKeyNo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid parameter: Read write key no");
                txtSDMWrite_ReadWriteKeyNo.Focus();
                return;
            }

            try
            {
                new_change_key_no = Byte.Parse(txtSDMWrite_NewChangeKeyNo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid parameter: New change key no");
                txtSDMWrite_NewChangeKeyNo.Focus();
                return;
            }

            if (cbSDMWrite_PiccDataExists.Checked)
            {
                if (cbSDMWrite_PiccDataEncrypted.Checked)
                {
                    try
                    {
                        meta_data_key_no = Byte.Parse(txtSDMWrite_MetaReadAccess.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid parameter: Meta read access.");
                        txtSDMWrite_MetaReadAccess.Focus();
                        return;
                    }
                }
                else
                    meta_data_key_no = 0x0E;

                if (cbSDMWrite_UIDMirroringEnable.Checked)
                    uid_enable = 1;
                else
                    uid_enable = 0;

                if (cbSDMWrite_ReadingCounterMirroringEnable.Checked)
                    read_ctr_enable = 1;
                else
                    read_ctr_enable = 0;
            }
            else
                meta_data_key_no = 0x0F;

            if (cbSDMWrite_EncFileDataEnable.Checked == true)
                enc_file_data_enable = 1;
            else
                enc_file_data_enable = 0;

            if (cbSDMWrite_MacExist.Checked)
            {
                try
                {
                    file_data_read_key_no = Byte.Parse(txtSDMWrite_FileDataReadAccess.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid parameter: SDM file data read access.");
                    txtSDMWrite_FileDataReadAccess.Focus();
                    return;
                }
            }

            if (tcbSDMWrite_CounterLimitEnable.Checked)
            {
                read_ctr_limit_enable = 1;

                try
                {
                    read_ctr_limit = UInt32.Parse(txtSDMWrite_ReadingCounterLimit.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid parameter: SDM reading counter limit:.");
                    txtSDMWrite_ReadingCounterLimit.Focus();
                    return;
                }
            }
            else
                read_ctr_limit_enable = 0;

            byte[] ndef_header = { 0x00, 0x00, 0xD1, 0x01, 0x00, 0x55, 0x00 };

            url = System.Text.Encoding.ASCII.GetBytes(txtSDMWrite_URL.Text);
            url_len = url.Length;
            Array.Copy(url, 0, ndef_data, 7, url.Length);

            ndef_len = url.Length;
            ndef_data[url_len + 7] = (byte)'?';
            url_len++;

            if (meta_data_key_no != 0x0F)
            {
                //PICC DATA EXIST
                if (meta_data_key_no == 0x0E)
                {
                    if (uid_enable == 1)
                    {
                        ndef_data[url_len + 7] = (byte)'u';
                        ndef_data[url_len + 7 + 1] = (byte)'=';
                        Array.Copy(Enumerable.Repeat((byte)'0', 14).ToArray(), 0, ndef_data, url_len + 7 + 2, 14);
                        uid_offset = (uint)(url_len + 7 + 2);
                        url_len += 16;
                    }

                    if (read_ctr_enable == 1)
                    {
                        ndef_data[url_len + 7] = (byte)'c';
                        ndef_data[url_len + 7 + 1] = (byte)'=';
                        Array.Copy(Enumerable.Repeat((byte)'0', 6).ToArray(), 0, ndef_data, url_len + 7 + 2, 6);
                        read_ctr_offset = (uint)(url_len + 7 + 2);
                        url_len += 8;
                    }
                }
                else
                {
                    //Encrypted PICC data
                    ndef_data[url_len + 7] = (byte)'p';
                    ndef_data[url_len + 7 + 1] = (byte)'=';
                    Array.Copy(Enumerable.Repeat((byte)'0', 32).ToArray(), 0, ndef_data, url_len + 7 + 2, 32);
                    picc_data_offset = (uint)(url_len + 7 + 2);
                    url_len += 34;
                }
            }

            if (file_data_read_key_no != 0x0E)
            {
                //MAC exist
                byte mac_input_ctr = 0;

                try
                {
                    mac_input_ctr = Byte.Parse(txtSDMWrite_AdditionalMacChars.Text);
                } catch (Exception ex)
                {
                    MessageBox.Show("Invalid parameter: No of additional characters for MAC encryption");
                    txtSDMWrite_AdditionalMacChars.Focus();
                    return;
                }

                if (enc_file_data_enable == 1)
                {
                    byte[] enc_file_data = new byte[100];
                    byte enc_file_len = 0, total_enc_file_len = 0;

                    enc_file_data = System.Text.Encoding.ASCII.GetBytes(txtSDMWrite_DataForEncryption.Text);
                    enc_file_len = (byte)enc_file_data.Length;

                    ndef_data[url_len + 7] = (byte)'e';
                    ndef_data[url_len + 7 + 1] = (byte)'=';
                    total_enc_file_len = enc_file_len;
                    int bre = enc_file_len % 16;
                    bool sta_bre = Convert.ToBoolean(enc_file_len % 16);
                    if (Convert.ToBoolean(enc_file_len % 16))
                        total_enc_file_len += (byte)(16 - enc_file_len % 16);
                    total_enc_file_len *= 2;
                    Array.Copy(Enumerable.Repeat((byte)0, total_enc_file_len).ToArray(), 0, ndef_data, url_len + 7 + 2, total_enc_file_len);
                    Array.Copy(enc_file_data, 0, ndef_data, url_len + 7 + 2, enc_file_len);

                    enc_offset = (uint)(url_len + 7 + 2);
                    mac_input_offset = enc_offset - mac_input_ctr;
                    enc_length = total_enc_file_len;
                    url_len += total_enc_file_len + 2;
                }

                ndef_data[url_len + 7] = (byte)'m';
                ndef_data[url_len + 7 + 1] = (byte)'=';

                Array.Copy(Enumerable.Repeat((byte)'0', 16).ToArray(), 0, ndef_data, url_len + 7 + 2, 16);
                if (enc_file_data_enable == 0)
                    mac_input_offset = (uint)(url_len + 7 + 2 - mac_input_ctr);
                mac_offset = (uint)(url_len + 7 + 2);
                url_len += 18;


            }
            ndef_header[1] = (byte)(url_len + 5);
            ndef_header[4] = (byte)(url_len + 1);
            //memcpy(ndef_data, ndef_header, 7);
            Array.Copy(ndef_header, ndef_data, 7);

            ndef_len = url_len + 7;

            if (rbSDMWrite_ProvidedKey.Checked)
            {
                if (txtSDMWrite_ProvidedKey.Text.Length != 32)
                {
                    MessageBox.Show("Provided key must be 16 bytes long!");
                    txtSDMWrite_ProvidedKey.Focus();
                    return;
                }
                aes_key_ext = StringToByteArray(txtSDMWrite_ProvidedKey.Text);

                status = uFCoder.nt4h_change_sdm_file_settings_pk(aes_key_ext, file_no, key_no, 3,
                                            communication_mode, read_key_no, write_key_no, read_write_key_no, new_change_key_no,
                                            uid_enable, read_ctr_enable, read_ctr_limit_enable, enc_file_data_enable,
                                            meta_data_key_no, file_data_read_key_no, read_ctr_key_no,
                                            uid_offset, read_ctr_offset, picc_data_offset,
                                            mac_input_offset, enc_offset, enc_length, mac_offset, read_ctr_limit);
            }
            else
            {
                try
                {
                    internal_key_no = Byte.Parse(txtSDMWrite_KeyIndex.Text);
                } catch (Exception ex)
                {
                    MessageBox.Show("Invalid parameter: Key index.");
                    txtSDMWrite_KeyIndex.Focus();
                    return;
                }

                status = uFCoder.nt4h_change_sdm_file_settings(internal_key_no, file_no, key_no, 3,
                                                        communication_mode, read_key_no, write_key_no, read_write_key_no, new_change_key_no,
                                                        uid_enable, read_ctr_enable, read_ctr_limit_enable, enc_file_data_enable,
                                                        meta_data_key_no, file_data_read_key_no, read_ctr_key_no,
                                                        uid_offset, read_ctr_offset, picc_data_offset,
                                                        mac_input_offset, enc_offset, enc_length, mac_offset, read_ctr_limit);
            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Changde sdm file settings failed: " + status;
                return;
            }

            ushort bytes_written = 0;

            //Console.WriteLine("\n Write NDEF into file number 2\n");
            status = uFCoder.nt4h_set_global_parameters(file_no, write_key_no, communication_mode);
            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Set global parameters failed: " + status;
                return;
            }

            if (write_key_no == 0x0E)
            {
                status = uFCoder.LinearWrite(ndef_data, 0, (ushort)(ndef_len + 1), ref bytes_written, (byte)T4T_AUTHENTICATION.T4T_WITHOUT_PWD_AUTH, 0);
            }
            else
            {
                if (rbSDMWrite_ProvidedKey.Checked)
                {
                    status = uFCoder.LinearWrite_PK(ndef_data, 0, (ushort)(ndef_len + 1), ref bytes_written, (byte)T4T_AUTHENTICATION.T4T_PK_PWD_AUTH, aes_key_ext);
                } else
                {
                    status = uFCoder.LinearWrite(ndef_data, 0, (ushort)(ndef_len + 1), ref bytes_written, (byte)T4T_AUTHENTICATION.T4T_RKA_PWD_AUTH, internal_key_no);
                }

            }

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Linear write failed: " + status;
            }
            else
                statusResult.Text = "Secure dynamic message write successful";

        }

        private void btnSDMRead_Click(object sender, EventArgs e)
        {
            DL_STATUS status;
            byte file_no = 0, file_type = 0, communication_mode = 0, sdm_enable = 0;
            byte read_key_no = 0, write_key_no = 0, read_write_key_no = 0, change_key_no = 0;
            byte uid_enable = 0, read_ctr_enable = 0, read_ctr_limit_enable = 0, enc_file_data_enable = 0;
            byte meta_data_key_no = 0, file_data_read_key_no = 0, read_ctr_key_no = 0;
            uint file_size = 0, uid_offset = 0, read_ctr_offset = 0, picc_data_offset = 0;
            uint mac_input_offset = 0, enc_offset = 0, enc_length = 0, mac_offset = 0, read_ctr_limit = 0;
            byte[] data = new byte[257];
            ushort ret_bytes = 0;
            byte picc_data_tag = 0;
            byte[] uid = new byte[7];
            uint sdm_read_cnt = 0;
            byte[] file_data_aes_key = new byte[16];


            file_no = 2;

            status = uFCoder.nt4h_get_file_settings(file_no, ref file_type, ref communication_mode, ref sdm_enable, ref file_size,
                                    ref read_key_no, ref write_key_no, ref read_write_key_no, ref change_key_no,
                                    ref uid_enable, ref read_ctr_enable, ref read_ctr_limit_enable, ref enc_file_data_enable,
                                    ref meta_data_key_no, ref file_data_read_key_no, ref read_ctr_key_no,
                                    ref uid_offset, ref read_ctr_offset, ref picc_data_offset,
                                    ref mac_input_offset, ref enc_offset, ref enc_length, ref mac_offset, ref read_ctr_limit);


            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Get file settings failed: " + status;
                return;
            }
            //check file parameters
            if ((communication_mode == 0 && Convert.ToBoolean(sdm_enable) && read_key_no == 0x0E) == false)
            {
                statusResult.Text = "File is not in SDM mode";
                return;
            }

            //read file data
            status = uFCoder.nt4h_set_global_parameters(2, 0x0E, 0);
            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Set global parameters failed: " + status;
                return;
            }

            status = uFCoder.LinearRead(data, 0, file_size, ref ret_bytes, (byte)T4T_AUTHENTICATION.T4T_WITHOUT_PWD_AUTH, 0);
            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Reading data failed " + status;
                return;
            }


            txtSDMRead_RawHexData.Text = BitConverter.ToString(data).Replace("-", ":");
            txtSDMRead_NDEFFileContext.Text = System.Text.Encoding.Default.GetString(data, 7, data.Length - 7);

            if (meta_data_key_no <= 4)
            {
                
                //PICC encrypted data
                byte[] enc_picc_data = new byte[32];
                byte[] picc_data = new byte[16];
                byte[] meta_data_aes_key = new byte[16];

                //enc_picc_data[32] = 0;
                Array.Copy(data, picc_data_offset, enc_picc_data, 0, 32);
                txtSDMRead_PICCEncryptedData.Text = System.Text.Encoding.Default.GetString(enc_picc_data);

                picc_data = StringToByteArray(System.Text.Encoding.UTF8.GetString(enc_picc_data));
                //picc_data = 


                if (txtSDMRead_MetaDataKey.Text.Length != 32)
                {
                    MessageBox.Show("Meta data key must be 16 bytes long!");
                    txtSDMRead_MetaDataKey.Focus();
                    return;
                }

                meta_data_aes_key = StringToByteArray(txtSDMRead_MetaDataKey.Text);

                //RESI DEKRIPTOVANJE
               
                status = uFCoder.nt4h_decrypt_picc_data(picc_data, meta_data_aes_key, ref picc_data_tag, uid, ref sdm_read_cnt);
                if (status != DL_STATUS.UFR_OK)
                {
                    statusResult.Text = "PICC data decrypting failed: " + status;
                    return;
                }

                if  (Convert.ToBoolean(picc_data_tag & 0x80))
                {
                    txtSDMRead_UID.Text = BitConverter.ToString(uid).Replace("-", ":");
                }
                if  (Convert.ToBoolean(picc_data_tag & 0x40))
                {
                    txtSDMRead_ReadingCounter.Text = sdm_read_cnt.ToString();
                }

                if (meta_data_key_no == 0x0E)
                {
                    //PICC data isn't encrypted
                    byte[] ascii_uid = new byte[15];
                    byte[] ascii_sdm_read_cnt = new byte[7];
                    byte[] sdm_read_cnt_array = new byte[3];
                    byte temp = 0;

                    if (Convert.ToBoolean(uid_enable))
                    {
                        ascii_uid[14] = 0;
                        Array.Copy(data, uid_offset, ascii_uid, 0, 14);
                        txtSDMRead_ASCIIUID.Text = System.Text.Encoding.Default.GetString(ascii_uid);
                    }

                    if (Convert.ToBoolean(read_ctr_enable))
                    {
                        ascii_sdm_read_cnt[6] = 0;
                        Array.Copy(data, read_ctr_offset, ascii_sdm_read_cnt, 0, 6);
                        txtSDMRead_SDMReadingCounter.Text = System.Text.Encoding.Default.GetString(ascii_sdm_read_cnt);

                        temp = sdm_read_cnt_array[2];
                        sdm_read_cnt_array[2] = sdm_read_cnt_array[0];
                        sdm_read_cnt_array[0] = temp;
                       
                        for (int i = 0; i < sdm_read_cnt_array.Length; i++)
                        {
                            sdm_read_cnt += (uint) (sdm_read_cnt_array[i] * Convert.ToInt32(Math.Pow(10, sdm_read_cnt_array.Length - i - 1)));
                        }
                    }
                }

                if (Convert.ToBoolean(enc_file_data_enable))
                {
                    //Part of file data encrypted
                    byte[] enc_file_data = new byte[256];
                    byte[] file_data = new byte[128];

                    Array.Copy(data, enc_offset, enc_file_data, 0, enc_length);
                    
                    txtSDMRead_EncryptedPartFileData.Text = System.Text.Encoding.Default.GetString(enc_file_data);
                    string test = System.Text.Encoding.UTF8.GetString(enc_file_data).Replace(Convert.ToChar(0x0).ToString(), "");

                    file_data = StringToByteArray(System.Text.Encoding.UTF8.GetString(enc_file_data).Replace(Convert.ToChar(0x0).ToString(), ""));

                    if (txtSDMRead_FileDataReadKey.Text.Length != 32)
                    {
                        MessageBox.Show("File data read key must be 16 bytes long!");
                        txtSDMRead_FileDataReadKey.Focus();
                        return;
                    }

                    file_data_aes_key = StringToByteArray(txtSDMRead_FileDataReadKey.Text);


                    status = uFCoder.nt4h_decrypt_sdm_enc_file_data(sdm_read_cnt, uid, file_data_aes_key, file_data, (byte)(enc_length / 2));
                    if (status != DL_STATUS.UFR_OK)
                    {
                        statusResult.Text = "Part of file data decrypting failed: " + status;
                        return;
                    }

                    txtSDMRead_PartFileData.Text = System.Text.Encoding.Default.GetString(file_data);
                }

                if (file_data_read_key_no != 0x0F)
                {
                    //MAC exist
                    byte[] ascii_mac_data = new byte[16];
                    byte[] mac = new byte[8];
                    byte[] ascii_mac_in = new byte[256];
                    byte mac_in_len;

                    if (Convert.ToBoolean(enc_file_data_enable) == false)
                    {
                        if (txtSDMRead_FileDataReadKey.Text.Length != 32)
                        {
                            MessageBox.Show("File data read key must be 16 bytes long!");
                            txtSDMRead_FileDataReadKey.Focus();
                            return;
                        }
                    }
                    Array.Copy(data, mac_offset, ascii_mac_data, 0, 16);

                    txtSDMRead_ASCIIMacData.Text = System.Text.Encoding.Default.GetString(ascii_mac_data);

                    mac = StringToByteArray(System.Text.Encoding.UTF8.GetString(ascii_mac_data).Replace(Convert.ToChar(0x0).ToString(), ""));

                    mac_in_len = (byte)(mac_offset - mac_input_offset);
                    if (Convert.ToBoolean(mac_in_len))
                    {
                        Array.Copy(data, mac_input_offset, ascii_mac_in, 0, mac_in_len);
                        txtSDMRead_ACIIMacInputData.Text = System.Text.Encoding.Default.GetString(ascii_mac_in);
                    }

                    status = uFCoder.nt4h_check_sdm_mac(sdm_read_cnt, uid, file_data_aes_key, ascii_mac_in, mac_in_len, mac);
                    if (status != DL_STATUS.UFR_OK)
                    {
                        statusResult.Text = "MAC is not correct. Error: " + status;
                        return;
                    }

                    statusResult.Text = "MAC is correct";
                }

            }

        }

        private void btnReaderClose_Click(object sender, EventArgs e)
        {
            DL_STATUS status;

            status = uFCoder.ReaderClose();

            if (status != DL_STATUS.UFR_OK)
            {
                statusResult.Text = "Reader Close failed. Error: " + status;
            }

            statusResult.Text = "Reader Close successful. Error: " + status;
            statusReader.Text = "NOT CONNECTED";
            DevInfoSN.Text = " SN : ";
            DevInfoHW.Text = " HW : ";
            DevInfoFW.Text = " FW : ";
        }
    }
}
