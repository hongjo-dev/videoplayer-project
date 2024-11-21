
using System;
using System.Windows.Forms;
using System.Drawing;

using QuartzTypeLib;
using WMPLib;

namespace TestProject
{
    /// <summary>
    /// 메인 폼
    /// </summary>
    public partial class MainForm : Form
    {
        WindowsMediaPlayer Player = new WindowsMediaPlayer(); //add



        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// WM_APP
        /// </summary>
        private const int WM_APP = 0x8000;

        /// <summary>
        /// WM_GRAPHNOTIFY
        /// </summary>
        private const int WM_GRAPHNOTIFY = WM_APP + 1;

        /// <summary>
        /// EC_COMPLETE
        /// </summary>
        private const int EC_COMPLETE = 0x01;

        /// <summary>
        /// WS_CHILD
        /// </summary>
        private const int WS_CHILD = 0x40000000;

        /// <summary>
        /// WS_CLIPCHILDREN
        /// </summary>
        private const int WS_CLIPCHILDREN = 0x2000000;


        /// <summary>
        /// 필터 그래프 관리자
        /// </summary>
        private FilgraphManager filterGraphManager = null;

        /// <summary>
        /// 기본 오디오
        /// </summary>
        private IBasicAudio basicAudio = null;

        /// <summary>
        /// 비디오 윈도우
        /// </summary>
        private IVideoWindow videoWindow = null;

        /// <summary>
        /// 미디어 이벤트
        /// </summary>
        private IMediaEvent mediaEvent = null;

        /// <summary>
        /// 미디어 이벤트 확장
        /// </summary>
        private IMediaEventEx mediaEventEX = null;

        /// <summary>
        /// 미디어 위치
        /// </summary>
        private IMediaPosition mediaPosition = null;

        /// <summary>
        /// 미디어 컨트롤
        /// </summary>
        private IMediaControl mediaControl = null;

        /// <summary>
        /// 미디어 상태
        /// </summary>
        private MediaStatus mediaStatus = MediaStatus.NONE;

        private Point mousePoint;

        private Boolean button_handled;
        
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - MainForm()

        /// <summary>
        /// 생성자
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            UpdateToolBar();
            UpdateStatusBar();

            FormClosing             += Form_FormClosing;
            SizeChanged += Form_SizeChanged;

            this.bunifuImageButton5.Click += bunifuImageButton5_Click; // 창닫기
            this.bunifuImageButton1.Click += bunifuImageButton1_Click_1;  //시작
            this.bunifuImageButton4.Click += bunifulmageButton4_Click;    //일시정지
            this.bunifuImageButton3.Click += bunifuImageButton3_Click;    //중단
            this.timer.Tick += timer_Tick;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 윈도우 프로시저 처리하기 - WndProc(message)

        /// <summary>
        /// 윈도우 프로시저 처리하기
        /// </summary>
        /// <param name="message">메시지</param>
        protected override void WndProc(ref Message message)
        {
            if(message.Msg == WM_GRAPHNOTIFY)
            {
                int eventCode;
                int parameter1;
                int parameter2;

                while(true)
                {
                    try
                    {
                        this.mediaEventEX.GetEvent(out eventCode, out parameter1, out parameter2, 0);

                        this.mediaEventEX.FreeEventParams(eventCode, parameter1, parameter2);

                        if(eventCode == EC_COMPLETE)
                        {
                            this.mediaControl.Stop();

                            this.mediaPosition.CurrentPosition = 0;

                            this.mediaStatus = MediaStatus.STOPPED;

                            UpdateToolBar();
                            UpdateStatusBar();
                        }
                    } 
                    catch(Exception)
                    {
                        break;
                    }
                }
            }

            base.WndProc(ref message);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private
        //////////////////////////////////////////////////////////////////////////////// Event

        #region 폼 크기 변경시 처리하기 - bunifuImageButton6_Click_SizeChanged(sender, e)

        /// <summary>
        /// 폼 크기 변경시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Up:
                    if (bunifuVSlider1.Value + 10 <= 100)
                        this.bunifuVSlider1.Value += 10;
                    else
                        this.bunifuVSlider1.Value = 100;
                    break;
                case Keys.Down:
                    if (bunifuVSlider1.Value - 10 >= 0)
                        this.bunifuVSlider1.Value -= 10;
                    else
                        bunifuVSlider1.Value = 0;
                    break;
                case Keys.Right:
                    if(this.mediaPosition != null)
                        if (this.mediaPosition.CurrentPosition + 10 <= this.mediaPosition.Duration)
                            this.mediaPosition.CurrentPosition += 10;
                        else
                            this.mediaPosition.CurrentPosition = this.mediaPosition.Duration;
                    break;
                case Keys.Left:
                    if(this.mediaPosition != null)
                        if (this.mediaPosition.CurrentPosition - 10 >= 0)
                            this.mediaPosition.CurrentPosition -= 10;
                        else
                            this.mediaPosition.CurrentPosition = 0;
                    break;
                case Keys.Space:
                    if(this.mediaControl != null)
                        if (this.mediaStatus == MediaStatus.RUNNING)
                        {
                            this.mediaControl.Pause();

                            this.mediaStatus = MediaStatus.PAUSED;

                            UpdateToolBar();
                            UpdateStatusBar();
                        }
                        else
                        {
                            this.mediaControl.Run();

                            this.mediaStatus = MediaStatus.RUNNING;

                            UpdateToolBar();
                            UpdateStatusBar();
                        }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            
            if (this.videoWindow != null)
            {
                this.videoWindow.SetWindowPosition
                (
                    this.panel5.ClientRectangle.Left,
                    this.panel5.ClientRectangle.Top,
                    this.panel5.ClientRectangle.Width,
                    this.panel5.ClientRectangle.Height
                );
            }
        }
        #endregion
        #region 폼 닫을 경우 처리하기 - Form_FormClosing(sender, e)

        /// <summary>
        /// 폼 닫을 경우 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReleaseResource();
        }

        #endregion
        #region 열기 메뉴 항목 클릭시 처리하기 - bunifuImageButton7_Click(sender, e)

        /// <summary>
        /// 열기 메뉴 항목 클릭시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>


        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            if(button_handled)
            {
                button_handled = false;
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "미디어 파일|*.mpg;*.avi;*.wma;*.wmv;*.mov;*.wav;*.mp2;*.mp3;*.mp4;|모든 파일|*.*";

            if(DialogResult.OK == openFileDialog.ShowDialog())
            {
                ReleaseResource();

                this.filterGraphManager = new FilgraphManager();

                this.filterGraphManager.RenderFile(openFileDialog.FileName);

                this.basicAudio = this.filterGraphManager as IBasicAudio;
                
                try
                {
                    this.videoWindow = this.filterGraphManager as IVideoWindow;

                    this.videoWindow.Owner     = (int)this.panel5.Handle;
                    this.videoWindow.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;

                    this.videoWindow.SetWindowPosition
                    (
                      this.panel5.ClientRectangle.Left,
                      this.panel5.ClientRectangle.Top,
                      this.panel5.ClientRectangle.Width,
                      this.panel5.ClientRectangle.Height
                    );
                }
                catch (Exception)
                {
                    this.videoWindow = null;
                }

                this.mediaEvent = this.filterGraphManager as IMediaEvent;

                this.mediaEventEX = this.filterGraphManager as IMediaEventEx;

                this.mediaEventEX.SetNotifyWindow((int) this.Handle,WM_GRAPHNOTIFY, 0);

                this.mediaPosition = this.filterGraphManager as IMediaPosition;

                this.mediaControl = this.filterGraphManager as IMediaControl;

                this.Text = "DirectShow를 사용해 동영상 재생하기 - [" + openFileDialog.FileName +  "]";

                this.mediaControl.Run();

                mediaStatus = MediaStatus.RUNNING;

                UpdateToolBar();
                UpdateStatusBar();
            }
        }

       /* private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            this.videoWindow = WindowsMediaPlayer.Forms.FormWindowState.Minimized;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
       */
        #endregion
        #region 종료 메뉴 항목 클릭시 처리하기 - bunifuImageButton5_Click(sender, e)

        /// <summary>
        /// 종료 메뉴 항목 클릭시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
        #region 재생 버튼 클릭시 처리하기 - playButton_Click(sender, e)

        /// <summary>
        /// 재생 버튼 클릭시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            this.mediaControl.Run();

            this.mediaStatus = MediaStatus.RUNNING;

            UpdateToolBar();
            UpdateStatusBar();


            
        }
        #endregion

        #region 중지 버튼 클릭시 처리하기 - bunifuImageButton4_Click(sender, e)

        /// <summary>
        /// 중지 버튼 클릭시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void bunifulmageButton4_Click(object sender, EventArgs e)
        {
            this.mediaControl.Pause();

            this.mediaStatus = MediaStatus.PAUSED;

            UpdateToolBar();
            UpdateStatusBar();
        }

        #endregion
        #region 중단 버튼 클릭시 처리하기 - bunifuImageButton3_Click(sender, e)

        /// <summary>
        /// 중단 버튼 클릭시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.mediaControl.Stop();

            this.mediaPosition.CurrentPosition = 0;

            this.mediaStatus = MediaStatus.STOPPED;

            UpdateToolBar();
            UpdateStatusBar();
        }

        #endregion
        
        #region 타이머 틱 처리하기 - timer_Tick(sender, e)

        /// <summary>
        /// 타이머 틱 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if(this.mediaStatus == MediaStatus.RUNNING)
            {
                UpdateStatusBar();
            }
        }

        #endregion

        #region 리소스 해제하기 - ReleaseResource()

        /// <summary>
        /// 리소스 해제하기
        /// </summary>
        private void ReleaseResource()
        {
            if(this.mediaControl != null)
            {
                this.mediaControl.Stop();
            }

            this.mediaStatus = MediaStatus.STOPPED;

            if(this.mediaEventEX != null)
            {
                this.mediaEventEX.SetNotifyWindow(0, 0, 0);
            }

            if(this.videoWindow != null)
            {
                this.videoWindow.Visible = 0;
                this.videoWindow.Owner   = 0;
            }

            if(this.mediaControl != null)
            {
                this.mediaControl = null;
            }

            if(this.mediaPosition != null)
            {
                this.mediaPosition = null;
            }

            if(this.mediaEventEX != null)
            {
                this.mediaEventEX = null;
            }

            if(this.mediaEvent != null)
            {
                this.mediaEvent = null;
            }

            if(this.videoWindow != null)
            {
                this.videoWindow = null;
            }

            if(this.basicAudio != null)
            {
                this.basicAudio = null;
            }

            if(this.filterGraphManager != null)
            {
                this.filterGraphManager = null;
            }
        }

        #endregion
        #region 툴바 갱신하기 - UpdateToolBar()

        /// <summary>
        /// 툴바 갱신하기
        /// </summary>
        private void UpdateToolBar()
        {
            switch(this.mediaStatus)
            {
                case MediaStatus.NONE :
                
                    this.bunifuImageButton1.Enabled  = false;
                    this.bunifuImageButton4.Enabled = false;
                    this.bunifuImageButton3.Enabled  = false;
                    
                    break;
                                          
                case MediaStatus.PAUSED :
                
                    this.bunifuImageButton1.Enabled  = true;
                    this.bunifuImageButton4.Enabled = false;
                    this.bunifuImageButton3.Enabled  = true;
                    
                    break;
                                          
                case MediaStatus.RUNNING :
                
                    this.bunifuImageButton1.Enabled  = false;
                    this.bunifuImageButton4.Enabled = true;
                    this.bunifuImageButton3.Enabled  = true;
                    
                    break;
                                          
                case MediaStatus.STOPPED :
                
                    this.bunifuImageButton1.Enabled  = true;
                    this.bunifuImageButton4.Enabled = false;
                    this.bunifuImageButton3.Enabled  = false;
                    
                    break;
            }
        }

        #endregion
        #region 상태바 갱신하기 - UpdateStatusBar()

        /// <summary>
        /// 상태바 갱신하기
        /// </summary>
        private void UpdateStatusBar()
        {
            switch(this.mediaStatus)
            {
              //    case MediaStatus.NONE    : this.messageToolStripStatusLabel.Text = "중단";   break;
                //  case MediaStatus.PAUSED  : this.messageToolStripStatusLabel.Text = "중지";   break;
                 // case MediaStatus.RUNNING : this.messageToolStripStatusLabel.Text = "재생중"; break;
                  //case MediaStatus.STOPPED : this.messageToolStripStatusLabel.Text = "중단";   break;
            }

            if (this.mediaPosition != null)
            {
                int secondCount = (int) this.mediaPosition.Duration;
                int hourCount   = secondCount / 3600;
                int minuteCount = (secondCount  - (hourCount * 3600)) / 60;

                secondCount = secondCount - (hourCount * 3600 + minuteCount * 60);

                this.bunifuLabel3.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", hourCount, minuteCount, secondCount);

                secondCount = (int) this.mediaPosition.CurrentPosition;
                hourCount   = secondCount / 3600;
                minuteCount = (secondCount  - (hourCount * 3600)) / 60;
                secondCount = secondCount - (hourCount * 3600 + minuteCount * 60);

                this.bunifuLabel2.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", hourCount, minuteCount, secondCount);

                bunifuHSlider1.Value = (int)(this.mediaPosition.CurrentPosition / this.mediaPosition.Duration * 100);
            }
            else
            {
                this.bunifuLabel2.Text = "00:00:00";
                this.bunifuLabel3.Text  = "00:00:00";
            }

            try
            {
                if (this.basicAudio != null)
                {
                    this.basicAudio.Volume = bunifuVSlider1.Value;
                }
            }
            catch (NotImplementedException)
            {
                // 로그로만 처리하고 사용자에게 알리지 않음
                Console.WriteLine("IBasicAudio 인터페이스를 사용할 수 없습니다. WindowsMediaPlayer로 처리 중...");
                Player.settings.volume = bunifuVSlider1.Value;
            }
        }

        #endregion

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void messageToolStripStatusLabel_Click(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bunifuVSlider1.Visible = false;
            bunifuVSlider1.Minimum = -10000; // 음소거
            bunifuVSlider1.Maximum = 0;      // 최대 볼륨
            bunifuVSlider1.Value = -5000;

        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {

        }

        
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {

        }

        

       

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton26_Click(object sender, EventArgs e)
        {

        }

        private void bunifuHSlider1_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            if (mediaPosition != null)
                this.mediaPosition.CurrentPosition = this.bunifuHSlider1.Value * this.mediaPosition.Duration / 100.0;
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton2_Click_1(object sender, EventArgs e)
        {
            bunifuVSlider1.Visible = !bunifuVSlider1.Visible;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            panel5.Margin = new Padding(30, 10, 5, 40);
            panel5.Padding = new Padding(40, 10, 0, 20);
        }

        private void bunifuImageButton7_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton5_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuVSlider1_Scroll(object sender, Utilities.BunifuSlider.BunifuVScrollBar.ScrollEventArgs e)
        {
            if (this.basicAudio != null)
            {
                this.basicAudio.Volume = bunifuVSlider1.Value;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void panel6_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void bunifuLabel1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void bunifuLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void bunifuButton21_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                button_handled = true;
        }
    }
}