using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.BLL.Services;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmPOS.cs
// Tầng GUI: Toàn bộ logic vẽ ghế động và xử lý bán vé
//
// QUY TẮC THIẾT KẾ SƠ ĐỒ GHẾ:
//   - Mỗi ghế là 1 Button thường (System.Windows.Forms.Button)
//   - Tag của mỗi Button = LichChieuGheId (int)
//   - Màu Trống  : BackColor = (230, 230, 240) - Xám nhạt
//   - Màu Chọn   : BackColor = (46, 204, 113)  - Xanh lá
//   - Màu Đã bán : BackColor = (231, 76, 60)   - Đỏ + Disabled
//   - Màu VIP(trống): BackColor = (241, 196, 15) - Vàng
//   - Sự kiện click ghế: GheButton_Click (gán bằng EventHandler cổ điển)
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmPOS : Form
    {
        // ─────────────────────────────────────────────────────
        // FIELDS
        // ─────────────────────────────────────────────────────
        private readonly DatVeService _datVeService;

        // Trạng thái hiện tại của POS
        private int _lichChieuIdHienTai = 0;
        private decimal _giaVeCoBanHienTai = 0;
        private string _tenPhimHienTai = "";
        private string _tenPhongHienTai = "";
        private string _gioChieuHienTai = "";
        private int? _khachHangId = null;

        // Danh sách ghế đang chọn (key = LichChieuGheId)
        private List<GheSoDoDto> _danhSachGheDangChon;

        // Toàn bộ dữ liệu ghế của suất chiếu hiện tại
        private List<GheSoDoDto> _danhSachGheTatCa;

        // Màu sắc ghế
        private readonly Color COLOR_GHE_TRONG = Color.FromArgb(230, 230, 240);
        private readonly Color COLOR_GHE_CHON = Color.FromArgb(46, 204, 113);
        private readonly Color COLOR_GHE_DA_BAN = Color.FromArgb(231, 76, 60);
        private readonly Color COLOR_GHE_VIP_TRONG = Color.FromArgb(241, 196, 15);

        // ─────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────
        public frmPOS()
        {
            InitializeComponent();
            _datVeService = new DatVeService();
            _danhSachGheDangChon = new List<GheSoDoDto>();
            _danhSachGheTatCa = new List<GheSoDoDto>();

            this.Load += new EventHandler(frmPOS_Load);
        }

        // ─────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────
        private void frmPOS_Load(object sender, EventArgs e)
        {
            NapDanhSachPhim();
            CapNhatHienThiTienKhachDua(); // Ẩn/hiện txtTienKhachDua theo PTTT mặc định
        }

        // ─────────────────────────────────────────────────────
        // NẠP DANH SÁCH PHIM VÀO COMBOBOX
        // ─────────────────────────────────────────────────────
        private void NapDanhSachPhim()
        {
            try
            {
                List<PhimPosDto> danhSach = _datVeService.GetPhimHomNay();
                cboPhim.Items.Clear();

                if (danhSach.Count == 0)
                {
                    cboPhim.Items.Add("-- Không có phim chiếu hôm nay --");
                    cboPhim.SelectedIndex = 0;
                    cboPhim.Enabled = false;
                    return;
                }

                cboPhim.Enabled = true;
                foreach (PhimPosDto phim in danhSach)
                {
                    cboPhim.Items.Add(phim);
                }
                cboPhim.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tải danh sách phim: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN: CHỌN PHIM → NẠP CÁC SUẤT CHIẾU
        // ─────────────────────────────────────────────────────
        private void cboPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhimPosDto phimChon = cboPhim.SelectedItem as PhimPosDto;
            if (phimChon == null)
                return;

            // Xóa sơ đồ ghế cũ
            XoaSoDoGhe();
            _lichChieuIdHienTai = 0;

            // Vẽ các nút suất chiếu
            VeSuatChieuButtons(phimChon.PhimId);
        }

        // ─────────────────────────────────────────────────────
        // VẼ CÁC NÚT SUẤT CHIẾU (ĐỘNG)
        // ─────────────────────────────────────────────────────
        private void VeSuatChieuButtons(int phimId)
        {
            flpSuatChieu.Controls.Clear();
            lblKhongCoSuat.Visible = false;

            try
            {
                List<SuatChieuPosDto> danhSach = _datVeService.GetSuatChieuByPhim(phimId);

                if (danhSach.Count == 0)
                {
                    lblKhongCoSuat.Text = "Không có suất chiếu\ntrong hôm nay";
                    lblKhongCoSuat.Visible = true;
                    return;
                }

                foreach (SuatChieuPosDto sc in danhSach)
                {
                    // Tạo Button suất chiếu
                    Button btnSuat = new Button();
                    btnSuat.Text = sc.GioHienThi + "\n" + sc.TenPhong;
                    btnSuat.Size = new Size(210, 52);
                    btnSuat.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    btnSuat.FlatStyle = FlatStyle.Flat;
                    btnSuat.Cursor = Cursors.Hand;
                    btnSuat.Margin = new Padding(0, 0, 0, 6);
                    btnSuat.TextAlign = ContentAlignment.MiddleCenter;

                    // Lưu toàn bộ object suất chiếu vào Tag
                    btnSuat.Tag = sc;

                    // Màu theo trạng thái
                    if (sc.TrangThai == "HuyChieu" || sc.TrangThai == "DaKetThuc")
                    {
                        btnSuat.BackColor = Color.FromArgb(189, 195, 199);
                        btnSuat.ForeColor = Color.FromArgb(100, 100, 100);
                        btnSuat.Enabled = false;
                        btnSuat.Text = sc.GioHienThi + "\n[Hủy/Kết thúc]";
                    }
                    else
                    {
                        btnSuat.BackColor = Color.FromArgb(40, 40, 66);
                        btnSuat.ForeColor = Color.White;
                        btnSuat.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 110);
                        btnSuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 90);
                    }

                    // Gán sự kiện click theo chuẩn C# 7.3 (không dùng lambda)
                    btnSuat.Click += new EventHandler(this.BtnSuatChieu_Click);

                    flpSuatChieu.Controls.Add(btnSuat);
                }
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tải suất chiếu: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN: CLICK NÚT SUẤT CHIẾU → VẼ SƠ ĐỒ GHẾ
        // ─────────────────────────────────────────────────────
        private void BtnSuatChieu_Click(object sender, EventArgs e)
        {
            Button btnClicked = sender as Button;
            if (btnClicked == null)
                return;

            SuatChieuPosDto sc = btnClicked.Tag as SuatChieuPosDto;
            if (sc == null)
                return;

            // Đánh dấu nút đang được chọn (active)
            foreach (Control ctrl in flpSuatChieu.Controls)
            {
                Button b = ctrl as Button;
                if (b != null && b.Enabled)
                    b.BackColor = Color.FromArgb(40, 40, 66);
            }
            btnClicked.BackColor = Color.FromArgb(233, 69, 96);

            // Lưu thông tin suất chiếu đang chọn
            _lichChieuIdHienTai = sc.LichChieuId;
            _giaVeCoBanHienTai = sc.GiaVeCoBan;
            _tenPhongHienTai = sc.TenPhong;
            _gioChieuHienTai = sc.GioBatDau.ToString("HH:mm  dd/MM/yyyy");

            // Cập nhật panel phải
            PhimPosDto phimChon = cboPhim.SelectedItem as PhimPosDto;
            _tenPhimHienTai = phimChon != null ? phimChon.TenPhim : "";
            CapNhatThongTinPanel();

            // Xóa ghế đang chọn cũ và vẽ sơ đồ mới
            _danhSachGheDangChon.Clear();
            VeSoDoGhe(sc.LichChieuId);
            CapNhatTongTien();
        }

        // ─────────────────────────────────────────────────────
        // *** VẼ SƠ ĐỒ GHẾ ĐỘNG ***
        // Đây là hàm quan trọng nhất của module POS
        // Logic: Group ghế theo DayGhe → mỗi dãy là 1 Panel ngang
        // ─────────────────────────────────────────────────────
        private void VeSoDoGhe(int lichChieuId)
        {
            // Xóa sơ đồ cũ
            XoaSoDoGhe();
            lblChonSuatDeTrong.Visible = false;

            try
            {
                _danhSachGheTatCa = _datVeService.GetSoDoGhe(lichChieuId);
                if (_danhSachGheTatCa.Count == 0)
                {
                    lblChonSuatDeTrong.Text = "Phòng chiếu này chưa\ncó dữ liệu ghế.";
                    lblChonSuatDeTrong.Visible = true;
                    return;
                }

                // Nhóm ghế theo DayGhe (A, B, C...)
                List<string> danhSachDay = new List<string>();
                foreach (GheSoDoDto ghe in _danhSachGheTatCa)
                {
                    if (!danhSachDay.Contains(ghe.DayGhe))
                        danhSachDay.Add(ghe.DayGhe);
                }

                // --- ĐÃ ÉP CÂN CHO GHẾ ĐỂ VỪA KHÍT 490px ---
                int GHE_W = 38;  // (Cũ là 42)
                int GHE_H = 38;  // (Cũ là 42)
                int GHE_GAP = 5;   // Khoảng cách
                int LABEL_W = 28;
                int ROW_H = GHE_H + 8;
                int startY = 10;

                foreach (string day in danhSachDay)
                {
                    // 1. TẠO PANEL
                    Panel pnlRow = new Panel();
                    pnlRow.Height = ROW_H;

                    // --- CHIẾU CHỈ MỚI: Bật AutoSize thay vì tính Width ---
                    pnlRow.AutoSize = true;
                    pnlRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    // ------------------------------------------------------

                    // 2. LỌC VÀ SẮP XẾP GHẾ 
                    List<GheSoDoDto> gheThiDay = new List<GheSoDoDto>();
                    foreach (GheSoDoDto g in _danhSachGheTatCa)
                    {
                        if (g.DayGhe == day)
                            gheThiDay.Add(g);
                    }
                    gheThiDay.Sort(delegate (GheSoDoDto a, GheSoDoDto b)
                    {
                        return a.CotGhe.CompareTo(b.CotGhe);
                    });

                    // 3. TÍNH VỊ TRÍ
                    pnlRow.Location = new Point(10, startY);
                    pnlRow.BackColor = Color.Transparent;
                    // XÓA HẲN DÒNG pnlRow.Anchor ĐI, KHÔNG DÙNG NỮA!

                    // Label tên dãy (A, B, C...)
                    Label lblDay = new Label();
                    lblDay.Text = day;
                    lblDay.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    lblDay.ForeColor = Color.FromArgb(100, 100, 130);
                    lblDay.Size = new Size(LABEL_W, ROW_H);
                    lblDay.Location = new Point(0, 0);
                    lblDay.TextAlign = ContentAlignment.MiddleCenter;
                    pnlRow.Controls.Add(lblDay);

                    // Vẽ từng nút ghế trong dãy
                    int posX = LABEL_W + 4;
                    foreach (GheSoDoDto ghe in gheThiDay)
                    {
                        Button btnGhe = new Button();
                        btnGhe.Text = ghe.TenGhe;
                        btnGhe.Size = new Size(GHE_W, GHE_H);
                        btnGhe.Location = new Point(posX, (ROW_H - GHE_H) / 2);
                        btnGhe.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                        btnGhe.FlatStyle = FlatStyle.Flat;
                        btnGhe.FlatAppearance.BorderSize = 1;
                        btnGhe.Tag = ghe; // Lưu toàn bộ GheSoDoDto vào Tag

                        // Đặt màu theo trạng thái ghế
                        DatMauGhe(btnGhe, ghe);

                        // Ghế đã bán: disable
                        if (ghe.TrangThaiGhe == "DaBan" || ghe.TrangThaiGhe == "DangGiu")
                        {
                            btnGhe.Enabled = false;
                            btnGhe.Cursor = Cursors.No;
                        }
                        else
                        {
                            btnGhe.Cursor = Cursors.Hand;
                            // Gán sự kiện click ghế chuẩn C# 7.3
                            btnGhe.Click += new EventHandler(this.GheButton_Click);
                        }

                        pnlRow.Controls.Add(btnGhe);
                        posX += GHE_W + GHE_GAP;
                    }

                    pnlSoDoGhe.Controls.Add(pnlRow);
                    startY += ROW_H + 4;
                }
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tải sơ đồ ghế: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN: CLICK VÀO GHẾ → CHỌN / BỎ CHỌN
        // ─────────────────────────────────────────────────────
        private void GheButton_Click(object sender, EventArgs e)
        {
            Button btnGhe = sender as Button;
            if (btnGhe == null)
                return;

            GheSoDoDto ghe = btnGhe.Tag as GheSoDoDto;
            if (ghe == null || ghe.TrangThaiGhe == "DaBan")
                return;

            // Kiểm tra xem ghế đã được chọn chưa
            bool daChon = false;
            GheSoDoDto gheTimDuoc = null;
            foreach (GheSoDoDto g in _danhSachGheDangChon)
            {
                if (g.LichChieuGheId == ghe.LichChieuGheId)
                {
                    daChon = true;
                    gheTimDuoc = g;
                    break;
                }
            }

            if (daChon)
            {
                // Bỏ chọn ghế
                _danhSachGheDangChon.Remove(gheTimDuoc);
                DatMauGhe(btnGhe, ghe); // Trả về màu gốc
            }
            else
            {
                // Giới hạn tối đa 8 ghế/lần
                if (_danhSachGheDangChon.Count >= 8)
                {
                    HienThiThongBao("Tối đa 8 ghế mỗi lần giao dịch.", false);
                    return;
                }

                // Chọn ghế mới
                _danhSachGheDangChon.Add(ghe);
                btnGhe.BackColor = COLOR_GHE_CHON;
                btnGhe.ForeColor = Color.White;
                btnGhe.FlatAppearance.BorderColor = Color.FromArgb(39, 174, 96);
            }

            // Cập nhật panel phải (danh sách ghế + tổng tiền + nút thanh toán)
            CapNhatTongTien();
        }

        // ─────────────────────────────────────────────────────
        // ĐẶT MÀU GHẾ THEO TRẠNG THÁI VÀ LOẠI GHẾ
        // ─────────────────────────────────────────────────────
        private void DatMauGhe(Button btnGhe, GheSoDoDto ghe)
        {
            if (ghe.TrangThaiGhe == "DaBan" || ghe.TrangThaiGhe == "DangGiu")
            {
                btnGhe.BackColor = COLOR_GHE_DA_BAN;
                btnGhe.ForeColor = Color.White;
                btnGhe.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);
            }
            else
            {
                // Ghế trống: màu theo loại (VIP = vàng, thường = xám nhạt)
                if (ghe.TenLoaiGhe == "VIP" || ghe.TenLoaiGhe == "Sweetbox")
                {
                    btnGhe.BackColor = COLOR_GHE_VIP_TRONG;
                    btnGhe.ForeColor = Color.FromArgb(80, 60, 0);
                    btnGhe.FlatAppearance.BorderColor = Color.FromArgb(180, 140, 0);
                }
                else
                {
                    btnGhe.BackColor = COLOR_GHE_TRONG;
                    btnGhe.ForeColor = Color.FromArgb(60, 60, 90);
                    btnGhe.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 200);
                }
            }
        }

        // ─────────────────────────────────────────────────────
        // CẬP NHẬT TỔNG TIỀN VÀ DANH SÁCH GHẾ PANEL PHẢI
        // ─────────────────────────────────────────────────────
        private void CapNhatTongTien()
        {
            lstGheDangChon.Items.Clear();

            TomTatDonHangDto tomTat = _datVeService.TinhTienDonHang(
                _tenPhimHienTai,
                _tenPhongHienTai,
                _gioChieuHienTai,
                _danhSachGheDangChon,
                _giaVeCoBanHienTai);

            foreach (string tenGhe in tomTat.DanhSachTenGhe)
            {
                lstGheDangChon.Items.Add(tenGhe);
            }

            lblTongTienVal.Text = tomTat.TongTienHienThi;

            // Chỉ bật nút thanh toán khi đã chọn ít nhất 1 ghế
            btnThanhToan.Enabled = (_danhSachGheDangChon.Count > 0);
        }

        // ─────────────────────────────────────────────────────
        // CẬP NHẬT THÔNG TIN SUẤT CHIẾU TRÊN PANEL PHẢI
        // ─────────────────────────────────────────────────────
        private void CapNhatThongTinPanel()
        {
            lblInfoPhimVal.Text = _tenPhimHienTai;
            lblInfoPhongVal.Text = _tenPhongHienTai;
            lblInfoGioVal.Text = _gioChieuHienTai;
        }

        // ─────────────────────────────────────────────────────
        // NÚT TÌM KHÁCH HÀNG THEO SĐT
        // ─────────────────────────────────────────────────────
        private void btnTimKhachHang_Click(object sender, EventArgs e)
        {
            string sdt = txtSDTKhachHang.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                lblThongTinKH.Text = "";
                _khachHangId = null;
                return;
            }

            DAL.KhachHang kh = _datVeService.TimKhachHangBySoDT(sdt);
            if (kh == null)
            {
                lblThongTinKH.Text = "Không tìm thấy thành viên";
                lblThongTinKH.ForeColor = Color.FromArgb(200, 100, 100);
                _khachHangId = null;
            }
            else
            {
                string tenHang = kh.HangThanhVien != null ? kh.HangThanhVien.TenHang : "Thành viên";
                lblThongTinKH.Text = string.Format("✓ {0} | {1} | {2} điểm",
                    kh.HoTen, tenHang, kh.DiemTichLuy);
                lblThongTinKH.ForeColor = Color.FromArgb(46, 204, 113);
                _khachHangId = kh.KhachHangId;
            }
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN: ĐỔI PHƯƠNG THỨC THANH TOÁN
        // ─────────────────────────────────────────────────────
        private void cboPTTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatHienThiTienKhachDua();
        }

        private void CapNhatHienThiTienKhachDua()
        {
            bool laTienMat = (cboPTTT.SelectedItem != null &&
                              cboPTTT.SelectedItem.ToString() == "TienMat");
            lblTienKhachDua.Visible = laTienMat;
            txtTienKhachDua.Visible = laTienMat;
        }

        // ─────────────────────────────────────────────────────
        // NÚT THANH TOÁN
        // ─────────────────────────────────────────────────────
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_danhSachGheDangChon.Count == 0)
            {
                HienThiThongBao("Chưa chọn ghế nào.", false);
                return;
            }

            string pttt = cboPTTT.SelectedItem != null
                ? cboPTTT.SelectedItem.ToString()
                : "TienMat";

            decimal tienKhachDua = 0;
            if (pttt == "TienMat")
            {
                if (!decimal.TryParse(txtTienKhachDua.Text.Trim(), out tienKhachDua))
                {
                    HienThiThongBao("Vui lòng nhập tiền khách đưa.", false);
                    txtTienKhachDua.Focus();
                    return;
                }
            }

            // Disable nút trong khi xử lý
            btnThanhToan.Enabled = false;
            btnThanhToan.Text = "Đang xử lý...";

            try
            {
                ServiceResult ketQua = _datVeService.BanVe(
                    _lichChieuIdHienTai,
                    _danhSachGheDangChon,
                    _giaVeCoBanHienTai,
                    _khachHangId,
                    pttt,
                    tienKhachDua);

                if (ketQua.Success)
                {
                    // Hiển thị thông báo thành công
                    HienThiThongBao(ketQua.Message, true);

                    // Cập nhật màu các ghế vừa bán thành đỏ trên sơ đồ
                    DanhDauGheDaBan();

                    // Reset trạng thái chọn ghế
                    _danhSachGheDangChon.Clear();
                    lstGheDangChon.Items.Clear();
                    lblTongTienVal.Text = "0 ₫";
                    txtSDTKhachHang.Text = "";
                    lblThongTinKH.Text = "";
                    txtTienKhachDua.Text = "";
                    _khachHangId = null;
                }
                else
                {
                    HienThiThongBao(ketQua.Message, false);

                    // Nếu ghế bị người khác mua: reload lại sơ đồ ghế
                    if (ketQua.Message.Contains("người khác mua"))
                    {
                        _danhSachGheDangChon.Clear();
                        VeSoDoGhe(_lichChieuIdHienTai);
                        CapNhatTongTien();
                    }
                }
            }
            finally
            {
                btnThanhToan.Enabled = (_danhSachGheDangChon.Count > 0);
                btnThanhToan.Text = "THANH TOÁN";
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT HỦY CHỌN GHẾ
        // ─────────────────────────────────────────────────────
        private void btnHuyChon_Click(object sender, EventArgs e)
        {
            if (_danhSachGheDangChon.Count == 0)
                return;

            // Trả màu về gốc cho tất cả ghế đang chọn
            foreach (GheSoDoDto ghe in _danhSachGheDangChon)
            {
                TimVaDoiMauGhe(ghe.LichChieuGheId, false);
            }

            _danhSachGheDangChon.Clear();
            CapNhatTongTien();
            HienThiThongBao("", true);
        }

        // ─────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ─────────────────────────────────────────────────────

        /// <summary>Xóa toàn bộ sơ đồ ghế đang hiển thị.</summary>
        private void XoaSoDoGhe()
        {
            // Xóa các row panel (giữ lại lblChonSuatDeTrong)
            List<Control> toRemove = new List<Control>();
            foreach (Control ctrl in pnlSoDoGhe.Controls)
            {
                if (ctrl != lblChonSuatDeTrong)
                    toRemove.Add(ctrl);
            }
            foreach (Control ctrl in toRemove)
            {
                pnlSoDoGhe.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            lblChonSuatDeTrong.Visible = true;
            _danhSachGheTatCa.Clear();
            _danhSachGheDangChon.Clear();
        }

        /// <summary>
        /// Sau khi bán vé thành công, đổi màu các ghế đã mua thành đỏ.
        /// Duyệt qua tất cả Button trong sơ đồ ghế và cập nhật.
        /// </summary>
        private void DanhDauGheDaBan()
        {
            // Build HashSet LichChieuGheId của các ghế vừa bán
            List<int> idsDaBan = new List<int>();
            foreach (GheSoDoDto g in _danhSachGheDangChon)
            {
                idsDaBan.Add(g.LichChieuGheId);
            }

            // Duyệt qua các row panel
            foreach (Control rowCtrl in pnlSoDoGhe.Controls)
            {
                Panel pnlRow = rowCtrl as Panel;
                if (pnlRow == null)
                    continue;

                foreach (Control gheCtrl in pnlRow.Controls)
                {
                    Button btnGhe = gheCtrl as Button;
                    if (btnGhe == null || btnGhe.Tag == null)
                        continue;

                    GheSoDoDto ghe = btnGhe.Tag as GheSoDoDto;
                    if (ghe == null)
                        continue;

                    if (idsDaBan.Contains(ghe.LichChieuGheId))
                    {
                        btnGhe.BackColor = COLOR_GHE_DA_BAN;
                        btnGhe.ForeColor = Color.White;
                        btnGhe.Enabled = false;
                        btnGhe.Cursor = Cursors.No;
                        btnGhe.FlatAppearance.BorderColor = Color.FromArgb(192, 57, 43);

                        // Cập nhật TrangThaiGhe trong Tag để đồng bộ
                        ghe.TrangThaiGhe = "DaBan";
                    }
                }
            }
        }

        /// <summary>
        /// Tìm nút ghế theo LichChieuGheId và đổi màu.
        /// daDaBan = false: trả về màu trống gốc.
        /// </summary>
        private void TimVaDoiMauGhe(int lichChieuGheId, bool daDaBan)
        {
            foreach (Control rowCtrl in pnlSoDoGhe.Controls)
            {
                Panel pnlRow = rowCtrl as Panel;
                if (pnlRow == null)
                    continue;

                foreach (Control gheCtrl in pnlRow.Controls)
                {
                    Button btnGhe = gheCtrl as Button;
                    if (btnGhe == null || btnGhe.Tag == null)
                        continue;

                    GheSoDoDto ghe = btnGhe.Tag as GheSoDoDto;
                    if (ghe == null || ghe.LichChieuGheId != lichChieuGheId)
                        continue;

                    if (daDaBan)
                    {
                        btnGhe.BackColor = COLOR_GHE_DA_BAN;
                        btnGhe.ForeColor = Color.White;
                        btnGhe.Enabled = false;
                    }
                    else
                    {
                        // Trả về màu gốc (trống)
                        DatMauGhe(btnGhe, ghe);
                    }
                    return; // Tìm thấy rồi, thoát
                }
            }
        }

        /// <summary>Hiển thị thông báo với màu tương ứng, tự xóa sau 5 giây nếu thành công.</summary>
        private void HienThiThongBao(string thongBao, bool thanhCong)
        {
            lblThongBao.Text = thongBao;
            lblThongBao.ForeColor = thanhCong
                ? Color.FromArgb(46, 204, 113)
                : Color.FromArgb(233, 69, 96);

            if (thanhCong && !string.IsNullOrEmpty(thongBao))
            {
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                t.Interval = 5000;
                t.Tick += delegate (object s, EventArgs ev)
                {
                    lblThongBao.Text = "";
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
        }
    }
}