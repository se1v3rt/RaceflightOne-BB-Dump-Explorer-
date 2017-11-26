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

        private void cb_LogPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("dev");
                this.Text = sTitle + " - FW: " + Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("#vr NAME:")).Split(':')[2].Split(';')[0];
                // #vr NAME:
                rtb_Log.Text = String.Join("", saLogs[cb_LogPeriod.SelectedIndex]);
                // populate at a glance
                //craft name
                lbl_craftname.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set craft_name")).Split('=')[1];
                //profiles
                //profile 1
                tp_profile1.Text = "Profile 1 - " + Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("pname1")).Split('=')[1];
                //profile 2
                tp_profile2.Text = "Profile 2 - " + Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("pname2")).Split('=')[1];
                //profile 3
                tp_profile3.Text = "Profile 3 - " + Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("pname3")).Split('=')[1];
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
                lbl_rollp1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kp1")).Split('=')[1];
                lbl_rolli1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_ki1")).Split('=')[1];
                lbl_rolld1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kd1")).Split('=')[1];
                //p2
                lbl_rollp2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kp2")).Split('=')[1];
                lbl_rolli2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_ki2")).Split('=')[1];
                lbl_rolld2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kd2")).Split('=')[1];
                //p3
                lbl_rollp3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kp3")).Split('=')[1];
                lbl_rolli3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_ki3")).Split('=')[1];
                lbl_rolld3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kd3")).Split('=')[1];
                // pid pitch
                lbl_pitchp1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kp1")).Split('=')[1];
                lbl_pitchi1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_ki1")).Split('=')[1];
                lbl_pitchd1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kd1")).Split('=')[1];
                //p2
                lbl_pitchp2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kp2")).Split('=')[1];
                lbl_pitchi2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_ki2")).Split('=')[1];
                lbl_pitchd2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kd2")).Split('=')[1];
                //p3
                lbl_pitchp3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kp3")).Split('=')[1];
                lbl_pitchi3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_ki3")).Split('=')[1];
                lbl_pitchd3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kd3")).Split('=')[1];
                // pid yaw
                lbl_yawp1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kp1")).Split('=')[1];
                lbl_yawi1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_ki1")).Split('=')[1];
                lbl_yawd1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kd1")).Split('=')[1];
                //p2
                lbl_yawp2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kp2")).Split('=')[1];
                lbl_yawi2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_ki2")).Split('=')[1];
                lbl_yawd2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kd2")).Split('=')[1];
                //p3
                lbl_yawp3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kp3")).Split('=')[1];
                lbl_yawi3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_ki3")).Split('=')[1];
                lbl_yawd3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kd3")).Split('=')[1];
                //filtering
                //filter type
                if (Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set filter_type1")).Split('=')[1] == "0")
                    lbl_filter_type1.Text = "Filters - Type 0";
                else
                    lbl_filter_type1.Text = "Filters - Type 1";
                //p2
                if (Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set filter_type2")).Split('=')[1] == "0")
                    lbl_filter_type2.Text = "Filters - Type 0";
                else
                    lbl_filter_type2.Text = "Filters - Type 1";
                //p3
                if (Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set filter_type3")).Split('=')[1] == "0")
                    lbl_filter_type3.Text = "Filters - Type 0";
                else
                    lbl_filter_type3.Text = "Filters - Type 1";
                //filter rap
                lbl_yaw_rap1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_rap1")).Split('=')[1];
                lbl_roll_rap1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_rap1")).Split('=')[1];
                lbl_pitch_rap1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_rap1")).Split('=')[1];
                //p2
                lbl_yaw_rap2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_rap2")).Split('=')[1];
                lbl_roll_rap2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_rap2")).Split('=')[1];
                lbl_pitch_rap2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_rap2")).Split('=')[1];
                //p3 
                lbl_yaw_rap3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_rap3")).Split('=')[1];
                lbl_roll_rap3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_rap3")).Split('=')[1];
                lbl_pitch_rap3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_rap3")).Split('=')[1];
                //filter ga
                lbl_yaw_ga1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_ga1")).Split('=')[1];
                lbl_roll_ga1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_ga1")).Split('=')[1];
                lbl_pitch_ga1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_ga1")).Split('=')[1];
                //p2
                lbl_yaw_ga2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_ga2")).Split('=')[1];
                lbl_roll_ga2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_ga2")).Split('=')[1];
                lbl_pitch_ga2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_ga2")).Split('=')[1];
                //p3
                lbl_yaw_ga3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_ga3")).Split('=')[1];
                lbl_roll_ga3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_ga3")).Split('=')[1];
                lbl_pitch_ga3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_ga3")).Split('=')[1];
                //filter quick
                lbl_yaw_quick1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_quick1")).Split('=')[1];
                lbl_roll_quick1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_quick1")).Split('=')[1];
                lbl_pitch_quick1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_quick1")).Split('=')[1];
                //p2
                lbl_yaw_quick2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_quick2")).Split('=')[1];
                lbl_roll_quick2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_quick2")).Split('=')[1];
                lbl_pitch_quick2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_quick2")).Split('=')[1];
                //p3
                lbl_yaw_quick3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_quick3")).Split('=')[1];
                lbl_roll_quick3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_quick3")).Split('=')[1];
                lbl_pitch_quick3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_quick3")).Split('=')[1];
                // filter kd rap
                lbl_yaw_kd_rap1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kd_rap1")).Split('=')[1];
                lbl_roll_kd_rap1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kd_rap1")).Split('=')[1];
                lbl_pitch_kd_rap1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kd_rap1")).Split('=')[1];
                //p2
                lbl_yaw_kd_rap2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kd_rap2")).Split('=')[1];
                lbl_roll_kd_rap2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kd_rap2")).Split('=')[1];
                lbl_pitch_kd_rap2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kd_rap2")).Split('=')[1];
                //p3
                lbl_yaw_kd_rap3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_kd_rap3")).Split('=')[1];
                lbl_roll_kd_rap3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_kd_rap3")).Split('=')[1];
                lbl_pitch_kd_rap3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_kd_rap3")).Split('=')[1];
                //TPA
                //tpa kp
                lbl_tpa_kp1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpakp1")).Split(' ')[1];
                //p2
                lbl_tpa_kp2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpakp2")).Split(' ')[1];
                //p3
                lbl_tpa_kp3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpakp3")).Split(' ')[1];
                //tpa ki
                lbl_tpa_ki1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpaki1")).Split(' ')[1];
                //p2
                lbl_tpa_ki2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpaki2")).Split(' ')[1];
                //p3
                lbl_tpa_ki3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpaki3")).Split(' ')[1];
                //tpa kd
                lbl_tpa_kd1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpakd1")).Split(' ')[1];
                //p2
                lbl_tpa_kd2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpakd2")).Split(' ')[1];
                //p3
                lbl_tpa_kd3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("tpakd3")).Split(' ')[1];
                //Misc
                //misc throttle curve
                lbl_throttle_curve1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("throttlecurve1")).Split(' ')[1];
                //p2
                lbl_throttle_curve2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("throttlecurve2")).Split(' ')[1];
                //p3
                lbl_throttle_curve3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("throttlecurve3")).Split(' ')[1];
                // misc Witchcraft
                //miz wc roll
                lbl_roll_wc1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_wc1")).Split('=')[1];
                lbl_pitch_wc1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_wc1")).Split('=')[1];
                lbl_yaw_wc1.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_wc1")).Split('=')[1];
                //p2
                lbl_roll_wc2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_wc2")).Split('=')[1];
                lbl_pitch_wc2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_wc2")).Split('=')[1];
                lbl_yaw_wc2.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_wc2")).Split('=')[1];
                //p3
                lbl_roll_wc3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set roll_wc3")).Split('=')[1];
                lbl_pitch_wc3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set pitch_wc3")).Split('=')[1];
                lbl_yaw_wc3.Text = Array.Find(saLogs[cb_LogPeriod.SelectedIndex], x => x.Contains("set yaw_wc3")).Split('=')[1];
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
