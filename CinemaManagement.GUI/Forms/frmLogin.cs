using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CinemaManagement.BLL.Helpers;
using CinemaManagement.BLL.Services;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmLogin.cs
// Mục đích: Code-behind của Form Đăng nhập
// Tầng GUI chỉ gọi BLL, KHÔNG viết logic xác thực ở đây
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmLogin : Form
    {
        // -------------------------------------------------------
        // FIELDS
        // -------------------------------------------------------
        private readonly AuthService _authService;

        // Dùng để kéo form (vì đã tắt title bar mặc định)
        private bool   _isDragging = false;
        private Point  _dragStartPoint;

        // -------------------------------------------------------
        // P/Invoke: Hiệu ứng shadow cho form không có border
        // -------------------------------------------------------
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        // -------------------------------------------------------
        // CONSTRUCTOR
        // -------------------------------------------------------
        public frmLogin()
        {
            InitializeComponent();

            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

            _authService = new AuthService();

            // Bo góc form (tạo viền mềm mại)
            this.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, Width, Height, 16, 16));

            // Focus vào TextBox đầu tiên khi form mở
            this.Load += (s, e) => {
                txtTenDangNhap.Focus();

                // Animation: fade-in form khi mở (tùy chọn)
                this.Opacity = 0;
                var fadeTimer = new Timer { Interval = 15 };
                fadeTimer.Tick += (ts, te) => {
                    if (this.Opacity < 1.0) this.Opacity += 0.07;
                    else fadeTimer.Stop();
                };
                fadeTimer.Start();
            };
        }

        // -------------------------------------------------------
        // SỰ KIỆN CHÍNH: ĐĂNG NHẬP
        // -------------------------------------------------------
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ThucHienDangNhap();
        }

        /// <summary>
        /// Hàm thực hiện đăng nhập - tách ra riêng để tái sử dụng
        /// (gọi được từ cả button Click lẫn KeyDown Enter)
        /// </summary>
        private void ThucHienDangNhap()
        {
            // 1. Xóa thông báo lỗi cũ
            HienThiLoi("");

            // 2. Lấy input từ TextBox
            string tenDangNhap  = txtTenDangNhap.Text.Trim();
            string matKhau      = txtMatKhau.Text;

            // 3. Validate UI cơ bản (trước khi gọi BLL)
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                HienThiLoi("⚠  Vui lòng nhập tên đăng nhập.");
                txtTenDangNhap.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(matKhau))
            {
                HienThiLoi("⚠  Vui lòng nhập mật khẩu.");
                txtMatKhau.Focus();
                return;
            }

            // 4. Hiện trạng thái loading trên button
            btnDangNhap.Enabled = false;
            btnDangNhap.Text    = "Đang kiểm tra...";

            try
            {
                // 5. Gọi BLL → AuthService để xác thực
                LoginResultDto ketQua = _authService.DangNhap(tenDangNhap, matKhau);

                if (ketQua.ThanhCong)
                {
                    // 6a. Đăng nhập thành công:
                    //     Lưu thông tin vào SessionManager
                    SessionManager.DangNhap(
                        ketQua.TaiKhoanId,
                        ketQua.NhanVienId,
                        ketQua.TenDangNhap,
                        ketQua.HoTenNhanVien,
                        ketQua.TenVaiTro,
                        ketQua.VaiTroId
                    );

                    // 7. Chuyển sang MainForm tương ứng với vai trò
                    MoFormTuongUngVaiTro();
                }
                else
                {
                    // 6b. Đăng nhập thất bại: hiện thông báo lỗi
                    HienThiLoi("✗  " + ketQua.ThongBaoLoi);

                    // Rung TextBox mật khẩu (hiệu ứng cảnh báo)
                    RunTextBox(txtMatKhau);
                    txtMatKhau.Clear();
                    txtMatKhau.Focus();
                }
            }
            catch (Exception ex)
            {
                // Lỗi không mong đợi (mất kết nối DB, v.v.)
                HienThiLoi("✗  Không thể kết nối. Kiểm tra lại SQL Server.");
                Console.WriteLine($"[frmLogin] Exception: {ex.Message}");

            }
            finally
            {
                // Khôi phục button dù thành công hay thất bại
                btnDangNhap.Enabled = true;
                btnDangNhap.Text    = "ĐĂNG NHẬP";
            }
        }

        /// <summary>
        /// Mở Form chính tương ứng với vai trò của user vừa đăng nhập.
        /// Admin/Quản lý → frmMain (full menu)
        /// Nhân viên     → frmPOS (chỉ màn hình bán vé)
        /// </summary>
        private void MoFormTuongUngVaiTro()
        {
            Form formTiepTheo;

            if (SessionManager.IsQuanLy())
            {
                // Quản lý / Admin: mở form đầy đủ
                formTiepTheo = new frmMain();
            }
            else
            {
                // Nhân viên bán vé: chỉ mở màn hình POS
                // formTiepTheo = new frmPOS();
                // Tạm thời dùng frmMain cho đến khi làm xong frmPOS
                formTiepTheo = new frmMain();
            }

            formTiepTheo.Show();
            this.Hide(); // Ẩn form login, KHÔNG đóng (để quay lại khi logout)

            // Khi MainForm đóng → show lại Login (hoặc thoát app)
            formTiepTheo.FormClosed += (s, e) => {
                SessionManager.DangXuat();
                this.Show();
                txtMatKhau.Clear();
                txtTenDangNhap.Focus();
            };
        }

        // -------------------------------------------------------
        // SỰ KIỆN PHÍM
        // -------------------------------------------------------

        /// <summary>Nhấn Enter ở txtTenDangNhap → chuyển sang txtMatKhau</summary>
        private void txtTenDangNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Tắt tiếng "ding"
                txtMatKhau.Focus();
            }
        }

        /// <summary>Nhấn Enter ở txtMatKhau → đăng nhập luôn</summary>
        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ThucHienDangNhap();
            }
        }

        // -------------------------------------------------------
        // HIỆN / ẨN MẬT KHẨU
        // -------------------------------------------------------
        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienMatKhau.Checked)
            {
                // Tắt che mật khẩu của hệ thống và bỏ ký tự che
                txtMatKhau.UseSystemPasswordChar = false;
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
                // Bật lại che mật khẩu
                txtMatKhau.UseSystemPasswordChar = true;
            }
        }

        // -------------------------------------------------------
        // NÚT ĐÓNG FORM
        // -------------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // -------------------------------------------------------
        // KÉO FORM (vì đã tắt title bar mặc định)
        // -------------------------------------------------------
        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging    = true;
                _dragStartPoint = e.Location;
            }
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point diff  = Point.Subtract(e.Location, new Size(_dragStartPoint));
                this.Location = Point.Add(this.Location, new Size(diff));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            base.OnMouseUp(e);
        }

        // -------------------------------------------------------
        // PRIVATE HELPERS
        // -------------------------------------------------------

        /// <summary>Hiển thị thông báo lỗi dưới checkbox "Hiện mật khẩu"</summary>
        private void HienThiLoi(string thongBao)
        {
            lblThongBaoLoi.Text = thongBao;
        }

        /// <summary>
        /// Hiệu ứng "rung" control khi nhập sai mật khẩu.
        /// Tạo cảm giác phản hồi trực quan mà không cần MessageBox.
        /// </summary>
        private void RunTextBox(Control control)
        {
            Point viTriGoc  = control.Location;
            int   buocRun   = 5;
            int   soLanRun  = 6;
            int   demRun    = 0;

            var timer = new Timer { Interval = 30 };
            timer.Tick += (s, e) => {
                if (demRun >= soLanRun * 2)
                {
                    control.Location = viTriGoc; // Về vị trí gốc
                    timer.Stop();
                    timer.Dispose();
                    return;
                }
                // Di chuyển trái/phải xen kẽ
                int offset = (demRun % 2 == 0) ? buocRun : -buocRun;
                control.Location = new Point(viTriGoc.X + offset, viTriGoc.Y);
                demRun++;
            };
            timer.Start();
        }
    }
}
