using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CinemaManagement.BLL.Helpers;
using Guna.UI2.WinForms;
using System.Runtime.InteropServices;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmMain.cs
// Mục đích: Logic Dashboard chính
//   - Hiển thị thông tin user từ SessionManager
//   - Phân quyền ẩn/hiện nút menu
//   - Nhúng form con vào pnlChildContainer
//   - Đánh dấu nút menu đang active
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmMain : Form
    {
        // ─────────────────────────────────────────────────────
        // FIELDS
        // ─────────────────────────────────────────────────────

        /// <summary>Form con đang hiển thị trong pnlChildContainer</summary>
        private Form _activeChildForm = null;

        /// <summary>Nút menu đang được chọn (để đổi màu active)</summary>
        private Guna2Button _activeMenuButton = null;

        // Màu trạng thái active của nút menu
        private readonly Color _colorMenuActive = Color.FromArgb(233, 69, 96);  // #E94560
        private readonly Color _colorMenuDefault = Color.Transparent;
        private readonly Color _colorTextActive = Color.White;
        private readonly Color _colorTextDefault = Color.FromArgb(180, 180, 200);

        // Kéo form (vì FormBorderStyle.None)
        private bool _isDragging = false;
        private Point _dragStart;

        // P/Invoke để bo góc form
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        // --- PHÉP THUẬT DI CHUYỂN FORM KHÔNG VIỀN ---
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        // ─────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────
        public frmMain()
        {
            InitializeComponent();
        }

        // ─────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────
        private void frmMain_Load(object sender, EventArgs e)
        {
            // 1. Bo góc form
            this.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, Width, Height, 12, 12));

            // 2. Bật đồng hồ
            timerClock.Start();

            // 3. Hiển thị thông tin user từ SessionManager
            HienThiThongTinUser();

            // 4. Phân quyền: ẩn nút theo vai trò
            ApDungPhanQuyen();

            // 5. Mở màn hình mặc định (Dashboard/Home)
            // Tạm thời show welcome panel
            HienThiWelcomePanel();

            // 6. Bo tròn avatar bằng cách vẽ lại
            lblAvatarCircle.Paint += LblAvatarCircle_Paint;

        }

        // ─────────────────────────────────────────────────────
        // HIỂN THỊ THÔNG TIN USER
        // ─────────────────────────────────────────────────────
        private void HienThiThongTinUser()
        {
            string hoTen = SessionManager.HoTenNhanVien ?? "Người dùng";
            lblUserName.Text = hoTen.Length > 18 ? hoTen.Substring(0, 16) + "..." : hoTen;
            lblUserRole.Text = SessionManager.TenVaiTro ?? "Nhân viên";
            lblAvatarCircle.Text = string.IsNullOrEmpty(hoTen) ? "U" : hoTen[0].ToString().ToUpper();

            // Ép nền thành Trong suốt để hiển thị hình tròn
            lblAvatarCircle.BackColor = Color.Transparent;

            // Lưu trữ màu theo chức vụ vào biến Tag thay vì tô thẳng vào nền
            if (SessionManager.IsAdmin())
                lblAvatarCircle.Tag = Color.FromArgb(52, 152, 219);
            else if (SessionManager.IsQuanLy())
                lblAvatarCircle.Tag = Color.FromArgb(46, 204, 113);
            else
                lblAvatarCircle.Tag = Color.FromArgb(233, 69, 96);
        }

        // ─────────────────────────────────────────────────────
        // PHÂN QUYỀN: ẨN/HIỆN NÚT MENU
        // ─────────────────────────────────────────────────────
        private void ApDungPhanQuyen()
        {
            // Nhân viên bán vé (không phải quản lý):
            // CHỈ thấy nút "Bán Vé" và "Đăng Xuất"
            if (!SessionManager.IsQuanLy())
            {
                btnQuanLyPhim.Visible = false;
                btnLichChieu.Visible = false;
                btnThongKe.Visible = false;
                btnQuanLyNhanVien.Visible = false;

                // Tự động active nút "Bán Vé" và mở POS ngay
                DanhDauNutActive(btnBanVe);
                // TODO: OpenChildForm(new frmPOS());
            }
            else
            {
                // Quản lý/Admin: hiện tất cả
                btnQuanLyPhim.Visible = true;
                btnLichChieu.Visible = true;
                btnThongKe.Visible = true;
                btnQuanLyNhanVien.Visible = true;
            }
        }

        // ─────────────────────────────────────────────────────
        // HÀM NHÚNG FORM CON *** QUAN TRỌNG NHẤT ***
        // ─────────────────────────────────────────────────────
        /// <summary>
        /// Nhúng một Form con vào vùng pnlChildContainer.
        /// Kỹ thuật: Set form con thành TopLevel=false, sau đó Add vào Panel.
        /// Form cũ sẽ được Dispose trước khi mở form mới.
        /// </summary>
        /// <param name="childForm">Form cần hiển thị</param>
        /// <param name="pageTitle">Tiêu đề hiển thị trên header bar</param>
        public void OpenChildForm(Form childForm, string pageTitle = "")
        {
            // 1. Đóng & giải phóng form con cũ (nếu có)
            if (_activeChildForm != null)
            {
                _activeChildForm.Close();
                _activeChildForm.Dispose();
                _activeChildForm = null;
            }

            // 2. Cập nhật tiêu đề trang trên header
            lblPageTitle.Text = string.IsNullOrEmpty(pageTitle)
                ? childForm.Text
                : pageTitle;

            // 3. Cấu hình form con để nhúng vào panel
            _activeChildForm = childForm;
            childForm.TopLevel = false;   // KHÔNG hiện như cửa sổ độc lập
            childForm.FormBorderStyle = FormBorderStyle.None; // Ẩn border
            childForm.Dock = DockStyle.Fill;       // Tự co giãn theo panel

            // 4. Thêm vào container và hiển thị
            pnlChildContainer.Controls.Add(childForm);
            pnlChildContainer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // ─────────────────────────────────────────────────────
        // ĐÁNH DẤU NÚT MENU ĐANG ACTIVE
        // ─────────────────────────────────────────────────────
        /// <summary>
        /// Đổi màu nút được chọn thành màu accent đỏ,
        /// reset các nút khác về mặc định.
        /// </summary>
        private void DanhDauNutActive(Guna2Button nutDuocChon)
        {
            // Reset nút cũ (nếu có)
            if (_activeMenuButton != null)
            {
                _activeMenuButton.FillColor = _colorMenuDefault;
                _activeMenuButton.ForeColor = _colorTextDefault;
            }

            // Tô màu nút mới
            nutDuocChon.FillColor = _colorMenuActive;
            nutDuocChon.ForeColor = _colorTextActive;

            _activeMenuButton = nutDuocChon;
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN CÁC NÚT MENU
        // ─────────────────────────────────────────────────────

        private void btnBanVe_Click(object sender, EventArgs e)
        {
            DanhDauNutActive(btnBanVe);
            OpenChildForm(new frmPOS(), "Bán Vé - POS");
        }

        private void btnQuanLyPhim_Click(object sender, EventArgs e)
        {
            DanhDauNutActive(btnQuanLyPhim);
            // Xóa dòng HienThiPlaceholder cũ đi và thay bằng dòng này:
            OpenChildForm(new frmQuanLyPhim(), "Quản lý Phim");
        }

        private void btnLichChieu_Click(object sender, EventArgs e)
        {
            DanhDauNutActive(btnLichChieu);
            OpenChildForm(new frmLichChieu(), "Quản lý Lịch Chiếu");
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DanhDauNutActive(btnThongKe);
            OpenChildForm(new frmThongKe(), "Thống Kê Doanh Thu");
        }

        private void btnQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            DanhDauNutActive(btnQuanLyNhanVien);
            OpenChildForm(new frmNhanVien(), "Quản lý Nhân Viên");
        }

        // ─────────────────────────────────────────────────────
        // ĐĂNG XUẤT
        // ─────────────────────────────────────────────────────
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // Dừng timer trước khi đóng
                timerClock.Stop();

                // Xóa session
                SessionManager.DangXuat();

                // Đóng frmMain → frmLogin sẽ tự hiện lại
                // (xem frmLogin.cs: formTiepTheo.FormClosed += ...)
                this.Close();
            }
        }

        // ─────────────────────────────────────────────────────
        // WELCOME PANEL - Hiển thị khi mới đăng nhập
        // ─────────────────────────────────────────────────────
        private void HienThiWelcomePanel()
        {
            // Xóa nội dung cũ trong container
            pnlChildContainer.Controls.Clear();

            // Tạo panel chào mừng đơn giản
            var pnlWelcome = new Panel();
            pnlWelcome.Dock = DockStyle.Fill;
            pnlWelcome.BackColor = Color.FromArgb(245, 246, 250);

            // Tên chào
            var lblChao = new Label();
            lblChao.Text = $"Xin chào, {SessionManager.HoTenNhanVien}! 👋";
            lblChao.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblChao.ForeColor = Color.FromArgb(30, 30, 60);
            lblChao.AutoSize = true;
            lblChao.Location = new Point(60, 120);

            // Gợi ý hành động
            var lblGợiY = new Label();
            lblGợiY.Text = "Chọn một chức năng từ thanh menu bên trái để bắt đầu.";
            lblGợiY.Font = new Font("Segoe UI", 12F);
            lblGợiY.ForeColor = Color.FromArgb(120, 120, 150);
            lblGợiY.AutoSize = true;
            lblGợiY.Location = new Point(60, 172);

            // Đường kẻ accent
            var pnlAccentLine = new Panel();
            pnlAccentLine.Size = new Size(80, 4);
            pnlAccentLine.Location = new Point(60, 108);
            pnlAccentLine.BackColor = Color.FromArgb(233, 69, 96);

            // Cards thống kê nhanh (placeholder)
            var pnlCards = TaoQuickStatCards();
            pnlCards.Location = new Point(60, 230);

            pnlWelcome.Controls.Add(pnlAccentLine);
            pnlWelcome.Controls.Add(lblChao);
            pnlWelcome.Controls.Add(lblGợiY);
            pnlWelcome.Controls.Add(pnlCards);

            pnlChildContainer.Controls.Add(pnlWelcome);
        }

        /// <summary>
        /// Tạo 4 card thống kê nhanh trên màn hình chào mừng.
        /// Sau này sẽ connect vào BLL để lấy số liệu thật.
        /// </summary>
        private Panel TaoQuickStatCards()
        {
            var pnl = new Panel();
            pnl.Size = new Size(880, 150);
            pnl.BackColor = Color.Transparent;

            var cards = new (string Icon, string Value, string Label, Color Accent)[]
            {
                ("🎬", "12",      "Phim đang chiếu",   Color.FromArgb(52,  152, 219)),
                ("🎫", "348",     "Vé bán hôm nay",    Color.FromArgb(46,  204, 113)),
                ("💰", "8.4tr",   "Doanh thu hôm nay", Color.FromArgb(233, 69,  96)),
                ("👥", "1,240",   "Khách hàng",        Color.FromArgb(155, 89,  182)),
            };

            int x = 0;
            foreach (var (icon, value, label, accent) in cards)
            {
                var card = new Panel();
                card.Size = new Size(200, 130);
                card.Location = new Point(x, 0);
                card.BackColor = Color.White;
                card.Cursor = Cursors.Hand;
                // Bo góc bằng Paint
                card.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 12))
                    {
                        g.FillPath(new SolidBrush(Color.White), path);
                        g.DrawPath(new System.Drawing.Pen(Color.FromArgb(230, 230, 238), 1), path);
                    }
                };

                // Thanh màu accent trên đầu card
                var pnlTop = new Panel();
                pnlTop.Size = new Size(200, 4);
                pnlTop.Location = new Point(0, 0);
                pnlTop.BackColor = accent;

                var lblIcon = new Label();
                lblIcon.Text = icon;
                lblIcon.Font = new Font("Segoe UI Emoji", 20F);
                lblIcon.AutoSize = true;
                lblIcon.Location = new Point(16, 14);
                lblIcon.BackColor = Color.Transparent;

                var lblValue = new Label();
                lblValue.Text = value;
                lblValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
                lblValue.ForeColor = Color.FromArgb(30, 30, 60);
                lblValue.AutoSize = true;
                lblValue.Location = new Point(16, 62);
                lblValue.BackColor = Color.Transparent;

                var lblLab = new Label();
                lblLab.Text = label;
                lblLab.Font = new Font("Segoe UI", 8.5F);
                lblLab.ForeColor = Color.FromArgb(130, 130, 160);
                lblLab.AutoSize = true;
                lblLab.Location = new Point(16, 100);
                lblLab.BackColor = Color.Transparent;

                card.Controls.Add(pnlTop);
                card.Controls.Add(lblIcon);
                card.Controls.Add(lblValue);
                card.Controls.Add(lblLab);
                pnl.Controls.Add(card);

                x += 218; // Khoảng cách giữa các card
            }

            return pnl;
        }

        // ─────────────────────────────────────────────────────
        // PLACEHOLDER (dùng tạm khi chức năng chưa làm)
        // ─────────────────────────────────────────────────────
        private void HienThiPlaceholder(string tenChucNang, string moTa)
        {
            pnlChildContainer.Controls.Clear();
            lblPageTitle.Text = tenChucNang;

            var pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.BackColor = Color.FromArgb(245, 246, 250);

            var lbl = new Label();
            lbl.Text = $"🚧  {tenChucNang}\n\n{moTa}";
            lbl.Font = new Font("Segoe UI", 13F);
            lbl.ForeColor = Color.FromArgb(150, 150, 180);
            lbl.AutoSize = false;
            lbl.Size = new Size(600, 200);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Location = new Point(200, 200);

            pnl.Controls.Add(lbl);
            pnlChildContainer.Controls.Add(pnl);
        }

        // ─────────────────────────────────────────────────────
        // GFX HELPERS
        // ─────────────────────────────────────────────────────

        /// <summary>Tạo GraphicsPath hình chữ nhật bo góc (dùng cho Paint events)</summary>
        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>Vẽ avatar tròn thay vì vuông (Paint event của lblAvatarCircle)</summary>
        // private void LblAvatarCircle_Paint(object sender, PaintEventArgs e)
        // {
        //     var lbl = (Label)sender;
        //      var g   = e.Graphics;
        //      g.SmoothingMode = SmoothingMode.AntiAlias;
        //
        //       // Vẽ hình tròn với màu BackColor
        //       using (var brush = new SolidBrush(lbl.BackColor))
        //           {
        //             g.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
        //         }
        //
        //          // Vẽ text chữ cái đầu
        //         var sf = new StringFormat
        //          {
        //               Alignment = StringAlignment.Center,
        //               LineAlignment = StringAlignment.Center
        ///         };
        ////          using (var font = new Font("Segoe UI", 14F, FontStyle.Bold))
        //         using (var fBrush = new SolidBrush(Color.White))
        //        {
        //             g.DrawString(lbl.Text, font, fBrush, new RectangleF(0, 0, lbl.Width, lbl.Height), sf);
        //          }

        //          // Ẩn background mặc định của Label
        //         lbl.BackColor = Color.Transparent;
        // (Trick: Paint vẽ đè lên → avatar trông như tròn thật)
        //     }

        // ─────────────────────────────────────────────────────
        // KÉO FORM (FormBorderStyle.None)
        // ─────────────────────────────────────────────────────
        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }

        // ─────────────────────────────────────────────────────
        // RESIZE: Cập nhật bo góc khi form thay đổi kích thước
        // ─────────────────────────────────────────────────────
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width > 0 && this.Height > 0)
            {
                this.Region = Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, Width, Height, 12, 12));

                // Đồng bộ chiều cao sidebar khi resize
                if (pnlSidebar != null)
                    pnlSidebar.Height = this.Height;

                if (pnlBottomMenu != null)
                    pnlBottomMenu.Location = new Point(0, this.Height - 60);
            }
        }
        // ─────────────────────────────────────────────────────
        // CÁC SỰ KIỆN CHUẨN (Khắc phục lỗi sập Designer)
        // ─────────────────────────────────────────────────────
        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new System.Drawing.Pen(Color.FromArgb(230, 230, 238), 1), 0, 59, pnlHeader.Width, 59);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCloseMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy  HH:mm:ss");
        }

        // ─────────────────────────────────────────────────────
        // GHI ĐÈ HÀM VẼ AVATAR TRÒN (Trị lỗi hình vuông)
        // ─────────────────────────────────────────────────────
        private void LblAvatarCircle_Paint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Lấy màu từ Tag, nếu chưa có thì lấy màu đỏ mặc định
            Color bgColor = lbl.Tag is Color c ? c : Color.FromArgb(233, 69, 96);

            using (var brush = new SolidBrush(bgColor))
            {
                g.FillEllipse(brush, 0, 0, lbl.Width - 1, lbl.Height - 1);
            }

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using (var font = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (var fBrush = new SolidBrush(Color.White))
            {
                g.DrawString(lbl.Text, font, fBrush, new RectangleF(0, 0, lbl.Width, lbl.Height), sf);
            }
        }

      
      }

   }
