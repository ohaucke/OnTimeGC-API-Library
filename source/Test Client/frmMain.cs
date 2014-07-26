using OnTimeGC_API;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Test_Client
{
    public partial class frmMain : Form
    {
        private FastColoredTextBoxNS.FastColoredTextBox fctbResult;
        private string[] methodList;
        private OnTimeGC_API.Client otgcClient;

        public frmMain()
        {
            InitializeComponent();
            initFctb();
        }

        private void initFctb()
        {
            this.fctbResult = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fctbResult.BackBrush = null;
            this.fctbResult.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctbResult.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctbResult.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.fctbResult.Language = FastColoredTextBoxNS.Language.CSharp;
            this.fctbResult.LeftBracket = '(';
            this.fctbResult.Location = new System.Drawing.Point(6, 19);
            this.fctbResult.Name = "fctbResult";
            this.fctbResult.Paddings = new System.Windows.Forms.Padding(0);
            this.fctbResult.RightBracket = ')';
            this.fctbResult.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctbResult.Size = new System.Drawing.Size(616, 590);
            this.fctbResult.TabIndex = 0;
            this.fctbResult.Text = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n";
            this.fctbResult.ShowLineNumbers = true;

            this.grpResult.Controls.Add(this.fctbResult);
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            this.methodList = new string[]
            {
                "constructor",
                "Login",
                "Logout",
                "Version",
                "Calendars",
                "GroupList",
                "LanguageList",
                "LanguageText",
                "Legends",
                "RegionList",
                "RegionText",
                "UsersAll",
                "UsersInfo",
                "UsersPhoto",
                "AppointmentCreate",
                "AppointmentRemove",
                "AppointmentChange"
            };

            this.cbMethod.Items.AddRange(this.methodList);

            this.cbAvailableAC.Items.AddRange(new object[] { "true", "false" });
            this.cbPrivateAC.Items.AddRange(new object[] { "true", "false" });
            this.cbAvailableAC.Text = "false";
            this.cbPrivateAC.Text = "false";

            this.cbNewAvailable.Items.AddRange(new object[] { "true", "false" });
            this.cbNewPrivate.Items.AddRange(new object[] { "true", "false" });
            this.cbNewAvailable.Text = "false";
            this.cbNewPrivate.Text = "false";

            this.cbMethod.Text = "constructor";
        }

        private void changeUi(string method)
        {
            hideAll();
            Control[] temp = this.grpComposer.Controls.Find("p" + method, true);

            if (temp.Length != 0)
            {
                temp[0].Visible = true;
                this.grpComposer.Height = temp[0].Height + 45;
            }
        }

        private void hideAll()
        {
            foreach (var item in this.grpComposer.Controls)
            {
                if (item.GetType() == typeof(Panel))
                {
                    Panel temp = (Panel)item;
                    temp.Visible = false;
                }
            }
        }

        private void cbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox temp = (ComboBox)sender;
            changeUi(temp.SelectedItem.ToString());
        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            int methodIndex = this.cbMethod.SelectedIndex;

            try
            {
                switch (this.methodList[methodIndex])
                {
                    case "constructor":
                        this.otgcClient = new OnTimeGC_API.Client(this.txtEndpoint.Text, (int)this.nudApiVersion.Value, this.txtAppId.Text, this.txtAppVersion.Text);
                        this.fctbResult.Text = debug("initialize successful completed");
                        break;
                    case "Login":
                        this.fctbResult.Text = debug(this.otgcClient.Login(this.txtUsername.Text, this.txtPassword.Text));
                        break;
                    case "Logout":
                        this.fctbResult.Text = debug(this.otgcClient.Logout());
                        break;
                    case "Version":
                        this.fctbResult.Text = debug(this.otgcClient.Version());
                        break;
                    case "Calendars":
                        this.fctbResult.Text = debug(this.otgcClient.Calendars(this.txtOnTimeIds.Text.Split(','), this.dtpStartDate.Value, this.dtpEndDate.Value));
                        break;
                    case "GroupList":
                        this.fctbResult.Text = debug(this.otgcClient.GroupList());
                        break;
                    case "LanguageList":
                        this.fctbResult.Text = debug(this.otgcClient.LanguageList());
                        break;
                    case "LanguageText":
                        this.fctbResult.Text = debug(this.otgcClient.LanguageText(this.txtLanguageCodeLT.Text));
                        break;
                    case "Legends":
                        this.fctbResult.Text = debug(this.otgcClient.Legends(this.txtLanguageCodeL.Text));
                        break;
                    case "RegionList":
                        this.fctbResult.Text = debug(this.otgcClient.RegionList());
                        break;
                    case "RegionText":
                        this.fctbResult.Text = debug(this.otgcClient.RegionText(this.txtLanguageCodeR.Text));
                        break;
                    case "UsersAll":
                        this.fctbResult.Text = debug(this.otgcClient.UsersAll());
                        break;
                    case "UsersInfo":
                        this.fctbResult.Text = debug(this.otgcClient.UsersInfo(this.txtOnTimeIdsU.Text.Split(',')));
                        break;
                    case "UsersPhoto":
                        this.fctbResult.Text = debug(this.otgcClient.UsersPhoto(this.txtOnTimeIdsUP.Text.Split(',')));
                        break;
                    case "AppointmentCreate":
                        this.fctbResult.Text = debug(this.otgcClient.AppointmentCreate(new OnTimeGC_API.Objects.AllDayEvent(this.txtUserIdAC.Text, this.dtpStartDtAC.Value, this.dtpEndDtAC.Value, this.txtSubjectAC.Text, this.txtLocationAC.Text, this.txtCategoriesAC.Text.Split(','), (this.cbPrivateAC.SelectedIndex == 0 ? true : false), (this.cbAvailableAC.SelectedIndex == 0 ? true : false), this.txtBodyAC.Text)));
                        break;
                    case "AppointmentRemove":
                        this.fctbResult.Text = debug(this.otgcClient.AppointmentRemove(this.txtUserIdAR.Text, this.txtUnIdAR.Text, int.Parse(this.txtLastModAR.Text).ToDateTime()));
                        break;
                    case "AppointmentChange":
                        this.fctbResult.Text = debug(this.otgcClient.AppointmentChange(this.txtUserIdACC.Text, this.txtUnIdACC.Text, int.Parse(this.txtLastModACC.Text).ToDateTime(), this.txtNewUserId.Text, int.Parse(this.txtNewStartDt.Text).ToDateTime(), int.Parse(this.txtNewEndDt.Text).ToDateTime(), this.txtNewSubject.Text, this.txtNewLocation.Text, this.txtNewCategories.Text.Split(','), (this.cbNewPrivate.SelectedIndex == 0 ? true : false), (this.cbNewAvailable.SelectedIndex == 0 ? true : false)));
                        break;
                }
            }
            catch (Exception ex)
            {
                this.fctbResult.Text = debug(ex);
            }
            finally
            {
                customKeywords();
                this.Enabled = true;
            }

        }

        private void customKeywords()
        {
            FastColoredTextBoxNS.Style KeywordsStyle = new FastColoredTextBoxNS.TextStyle(new SolidBrush(Color.FromArgb(43, 163, 213)), null, FontStyle.Regular);
            this.fctbResult.Range.ClearStyle(KeywordsStyle);
            this.fctbResult.Range.SetStyle(KeywordsStyle, new Regex(@"\b(NullReferenceException|InvalidTokenException|InvalidApiResponseException|ListDictionaryInternal|Exception|RuntimeMethodInfo|MethodBase|List|UsersInfoTyp|UsersAllTyp|AppointmentType|TokenResult|User|AppointmentChangeItem|AppointmentCreateItem|AppointmentRemoveItem|CalendarsItem|CalendarUser|Calendar|FreeTimeSearchItem|GroupListItem|LanguageListItem|LanguageTextItem|LegendsItem|Legend|LoginItem|LogoutItem|RegionListItem|RegionText|UsersAllItem|UsersInfoItem|UsersPhotoItem|Version)\b", FastColoredTextBoxNS.SyntaxHighlighter.RegexCompiledOption));
        }

        private string debug(object source)
        {
            return XSharper.Core.Dump.ToDump(source);
        }
    }
}
