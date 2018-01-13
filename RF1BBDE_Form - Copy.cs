using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RF1_BB_Dump_Explorer
{
    public partial class fm_RF1BBE : Form
    {
        String[][] saLogs = { };
        string sTitle = "";
        public fm_RF1BBE()
        {
            InitializeComponent();
            
        }
        private void btn_Open_Click(object sender, EventArgs e)
        {

            if (ofd_Main.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sTitle = "RF1 BB Dump Explorer - " + Path.GetFileName(ofd_Main.FileName);
                this.Text = sTitle;
                Array.Resize(ref saLogs, 0);
                cb_LogPeriod.Items.Clear();
                StreamReader reader = new StreamReader(ofd_Main.FileName);
                try
                {
                    
                    string line;
                    int flag = -1;
                    line = reader.ReadLine();
                    while (line != null)
                    {

                        // start of new array salogs code
                        if (line.Contains("H RF1 Dump"))
                        {
                            flag = 1;
                            if (saLogs.Length <= 0)
                                Array.Resize(ref saLogs, 1);
                            else
                                Array.Resize(ref saLogs, saLogs.Length + 1);
                        }
                        if (line.Contains("set"))
                            flag = 2;
                        else if (flag == 2)
                        {
                            flag = -1;
                        }
                        if (flag > 0)
                        {
                            if (saLogs[saLogs.Length - 1] == null)
                                Array.Resize(ref saLogs[saLogs.Length - 1], 1);
                            else
                                Array.Resize(ref saLogs[saLogs.Length - 1], saLogs[saLogs.Length - 1].Length + 1);

                            saLogs[saLogs.Length - 1][saLogs[saLogs.Length - 1].Length - 1] = line + Environment.NewLine;
                        }
                        line = reader.ReadLine();
                    }
                    for (int i = 0; i < saLogs.Length; i++)
                    {
                        cb_LogPeriod.Items.Add("Log: " + (i + 1).ToString() + "/" + (saLogs.Length));
                    }
                    cb_LogPeriod.SelectedIndex = 0;

                    // end new dropdown code from salogs array
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        private string GetSetting(string needle, char separator, int period)
        {
            try
            {
                return Array.Find(saLogs[period], x => x.Contains(needle)).Split(separator)[1];
            }
            catch (NullReferenceException)
            {

                return "NODATA";
            }
        }

        private void cb_LogPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("dev");
                this.Text = sTitle + " - FW: " + Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("#vr NAME:")).Split(':')[2].Split(';')[0];
                // #vr NAME:
                rtb_Log.Text = String.Join("", saLogs[cb_LogPeriod.SelectedIndex]);
                // populate at a glance
                //craft name .Text = GetSetting("", '=', cb_LogPeriod.SelectedIndex);
                lbl_craftname.Text = GetSetting("set craft_name", '=', cb_LogPeriod.SelectedIndex);
                //profiles
                //profile 1
                tp_profile1.Text = GetSetting("pname1", '=', cb_LogPeriod.SelectedIndex);
                //profile 2
                tp_profile2.Text = GetSetting("pname2", '=', cb_LogPeriod.SelectedIndex);
                //profile 3
                tp_profile3.Text = GetSetting("pname3", '=', cb_LogPeriod.SelectedIndex);
                int a = int.Parse(Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("modes active:")).Split(':')[1].Trim());
                if ((a & 4096) == 4096)
                {
                    tp_profile3.Text = "* " + tp_profile3.Text;
                    tc_Profiles.SelectedIndex = 2;
                }
                else if ((a & 2048) == 2048)
                {
                    tp_profile2.Text = "* " + tp_profile2.Text;
                    tc_Profiles.SelectedIndex = 1;
                }
                else
                {
                    tp_profile1.Text = "* " + tp_profile1.Text;
                    tc_Profiles.SelectedIndex = 0;
                }
                // pids
                // pid  roll
                lbl_rollp1.Text = GetSetting("set roll_kp1", '=', cb_LogPeriod.SelectedIndex);
                lbl_rolli1.Text = GetSetting("set roll_ki1", '=', cb_LogPeriod.SelectedIndex);
                lbl_rolld1.Text = GetSetting("set roll_kd1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_rollp2.Text = GetSetting("set roll_kp2", '=', cb_LogPeriod.SelectedIndex);
                lbl_rolli2.Text = GetSetting("set roll_ki2", '=', cb_LogPeriod.SelectedIndex);
                lbl_rolld2.Text = GetSetting("set roll_kd2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_rollp3.Text = GetSetting("set roll_kp3", '=', cb_LogPeriod.SelectedIndex);
                lbl_rolli3.Text = GetSetting("set roll_ki3", '=', cb_LogPeriod.SelectedIndex);
                lbl_rolld3.Text = GetSetting("set roll_kd3", '=', cb_LogPeriod.SelectedIndex);
                // pid pitch
                lbl_pitchp1.Text = GetSetting("set pitch_kp1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitchi1.Text = GetSetting("set pitch_ki1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitchd1.Text = GetSetting("set pitch_kd1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_pitchp2.Text = GetSetting("set pitch_kp2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitchi2.Text = GetSetting("set pitch_ki2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitchd2.Text = GetSetting("set pitch_kd2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_pitchp3.Text = GetSetting("set pitch_kp3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitchi3.Text = GetSetting("set pitch_ki3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitchd3.Text = GetSetting("set pitch_kd3", '=', cb_LogPeriod.SelectedIndex);
                // pid yaw
                lbl_yawp1.Text = GetSetting("set yaw_kp1", '=', cb_LogPeriod.SelectedIndex);
                lbl_yawi1.Text = GetSetting("set yaw_ki1", '=', cb_LogPeriod.SelectedIndex);
                lbl_yawd1.Text = GetSetting("set yaw_kd1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_yawp2.Text = GetSetting("set yaw_kp2", '=', cb_LogPeriod.SelectedIndex);
                lbl_yawi2.Text = GetSetting("set yaw_ki2", '=', cb_LogPeriod.SelectedIndex);
                lbl_yawd2.Text = GetSetting("set yaw_kd2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_yawp3.Text = GetSetting("set yaw_kp3", '=', cb_LogPeriod.SelectedIndex);
                lbl_yawi3.Text = GetSetting("set yaw_ki3", '=', cb_LogPeriod.SelectedIndex);
                lbl_yawd3.Text = GetSetting("set yaw_kd3", '=', cb_LogPeriod.SelectedIndex);
                //filtering
                //filter type
                lbl_filter_type1.Text = "Filters - " + GetSetting("set filter_type1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_filter_type2.Text = "Filters - " + GetSetting("set filter_type2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_filter_type3.Text = "Filters - " + GetSetting("set filter_type3", '=', cb_LogPeriod.SelectedIndex);
                //filter rap
                lbl_yaw_rap1.Text = GetSetting("set yaw_rap1", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_rap1.Text = GetSetting("set roll_rap1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_rap1.Text = GetSetting("set pitch_rap1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_yaw_rap2.Text = GetSetting("set yaw_rap2", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_rap2.Text = GetSetting("set roll_rap2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_rap2.Text = GetSetting("set pitch_rap2", '=', cb_LogPeriod.SelectedIndex);
                //p3 
                lbl_yaw_rap3.Text = GetSetting("set yaw_rap3", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_rap3.Text = GetSetting("set roll_rap3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_rap3.Text = GetSetting("set pitch_rap3", '=', cb_LogPeriod.SelectedIndex);
                //filter ga
                lbl_yaw_ga1.Text = GetSetting("set yaw_ga1", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_ga1.Text = GetSetting("set roll_ga1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_ga1.Text = GetSetting("set pitch_ga1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_yaw_ga2.Text = GetSetting("set yaw_ga2", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_ga2.Text = GetSetting("set roll_ga2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_ga2.Text = GetSetting("set pitch_ga2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_yaw_ga3.Text = GetSetting("set yaw_ga3", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_ga3.Text = GetSetting("set roll_ga3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_ga3.Text = GetSetting("set pitch_ga3", '=', cb_LogPeriod.SelectedIndex);
                //filter quick
                lbl_yaw_quick1.Text = GetSetting("set yaw_quick1", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_quick1.Text = GetSetting("set roll_quick1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_quick1.Text = GetSetting("set pitch_quick1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_yaw_quick2.Text = GetSetting("set yaw_quick2", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_quick2.Text = GetSetting("set roll_quick2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_quick2.Text = GetSetting("set pitch_quick2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_yaw_quick3.Text = GetSetting("set yaw_quick3", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_quick3.Text = GetSetting("set roll_quick3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_quick3.Text = GetSetting("set pitch_quick3", '=', cb_LogPeriod.SelectedIndex);
                // filter kd rap
                lbl_yaw_kd_rap1.Text = GetSetting("set yaw_kd_rap1", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_kd_rap1.Text = GetSetting("set roll_kd_rap1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_kd_rap1.Text = GetSetting("set pitch_kd_rap1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_yaw_kd_rap2.Text = GetSetting("set yaw_kd_rap2", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_kd_rap2.Text = GetSetting("set roll_kd_rap2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_kd_rap2.Text = GetSetting("set pitch_kd_rap2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_yaw_kd_rap3.Text = GetSetting("set yaw_kd_rap3", '=', cb_LogPeriod.SelectedIndex);
                lbl_roll_kd_rap3.Text = GetSetting("set roll_kd_rap3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_kd_rap3.Text = GetSetting("set pitch_kd_rap3", '=', cb_LogPeriod.SelectedIndex);
                //TPA
                //tpa kp
                lbl_tpa_kp1.Text = GetSetting("tpakp1", ' ', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_tpa_kp2.Text = GetSetting("tpakp2", ' ', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_tpa_kp3.Text = GetSetting("tpakp3", ' ', cb_LogPeriod.SelectedIndex);
                //tpa ki
                lbl_tpa_ki1.Text = GetSetting("tpaki1", ' ', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_tpa_ki2.Text = GetSetting("tpaki2", ' ', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_tpa_ki3.Text = GetSetting("tpaki3", ' ', cb_LogPeriod.SelectedIndex);
                //tpa kd
                lbl_tpa_kd1.Text = GetSetting("tpakd1", ' ', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_tpa_kd2.Text = GetSetting("tpakd2", ' ', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_tpa_kd3.Text = GetSetting("tpakd3", ' ', cb_LogPeriod.SelectedIndex);
                //Misc
                //misc throttle curve
                lbl_throttle_curve1.Text = GetSetting("throttlecurve1", ' ', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_throttle_curve2.Text = GetSetting("throttlecurve2", ' ', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_throttle_curve3.Text = GetSetting("throttlecurve3", ' ', cb_LogPeriod.SelectedIndex);
                // misc Witchcraft
                //miz wc roll
                lbl_roll_wc1.Text = GetSetting("set roll_wc1", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_wc1.Text = GetSetting("set pitch_wc1", '=', cb_LogPeriod.SelectedIndex);
                lbl_yaw_wc1.Text = GetSetting("set yaw_wc1", '=', cb_LogPeriod.SelectedIndex);
                //p2
                lbl_roll_wc2.Text = GetSetting("set roll_wc2", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_wc2.Text = GetSetting("set pitch_wc2", '=', cb_LogPeriod.SelectedIndex);
                lbl_yaw_wc2.Text = GetSetting("set yaw_wc2", '=', cb_LogPeriod.SelectedIndex);
                //p3
                lbl_roll_wc3.Text = GetSetting("set roll_wc3", '=', cb_LogPeriod.SelectedIndex);
                lbl_pitch_wc3.Text = GetSetting("set pitch_wc3", '=', cb_LogPeriod.SelectedIndex);
                lbl_yaw_wc3.Text = GetSetting("set yaw_wc3", '=', cb_LogPeriod.SelectedIndex);
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("This period's dump contains corrupted data." + Environment.NewLine + "You may contenue to view readable parts of the dump.", "Dump Extractor", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            
            btn_CpyCpb.Enabled = true;
        }

        private void btn_CpyCpb_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            int cnt = 1;
            string[] cpytxt = { "DEV" };
            string needle = (tc_Profiles.SelectedIndex + 1).ToString() + "=";
            string temp;
            for (int i = 0; i < saLogs[cb_LogPeriod.SelectedIndex].Length; i++)
            {
                temp = saLogs[cb_LogPeriod.SelectedIndex][i];
                if (saLogs[cb_LogPeriod.SelectedIndex][i].Contains(needle)
                    && !saLogs[cb_LogPeriod.SelectedIndex][i].Contains("modes")
                    && !saLogs[cb_LogPeriod.SelectedIndex][i].Contains("esc_" + (tc_Profiles.SelectedIndex + 1).ToString())
                    && !saLogs[cb_LogPeriod.SelectedIndex][i].Contains("mout" + (tc_Profiles.SelectedIndex + 1).ToString())
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("mixer_")
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("fmax")
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("bounce_guard")
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("curve" + (tc_Profiles.SelectedIndex + 1).ToString())
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("tpakp" + (tc_Profiles.SelectedIndex + 1).ToString())
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("tpaki" + (tc_Profiles.SelectedIndex + 1).ToString())
                    || saLogs[cb_LogPeriod.SelectedIndex][i].Contains("tpakd" + (tc_Profiles.SelectedIndex + 1).ToString())
                    )
                {
                    Array.Resize(ref cpytxt, cnt + 1);
                    if (saLogs[cb_LogPeriod.SelectedIndex][i].Contains("pname"))
                        cpytxt[0] = saLogs[cb_LogPeriod.SelectedIndex][i];
                    else
                        cpytxt[cnt] = saLogs[cb_LogPeriod.SelectedIndex][i];
                    cnt++;
                }
            }
#if DEBUG
            MessageBox.Show(String.Join("", cpytxt ));
#endif
            Clipboard.SetText(String.Join("", cpytxt));
        }
    }
}
