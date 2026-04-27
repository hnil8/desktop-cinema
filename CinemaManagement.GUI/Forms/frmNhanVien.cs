using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.BLL.Helpers;
using CinemaManagement.BLL.Services;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmNhanVien.cs
// Tầng GUI: Quản lý nhân viên + ca làm việc
//
// TUÂN THỦ LUẬT:
//   ✓ AutoGenerateColumns = false TRƯỚC Columns.Clear()
//   ✓ Dùng BindingSource trong HienThiLenLuoi()
//   ✓ Không dùng C# 8+ syntax
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmNhanVien : Form
    {
        // ─────────────────────────────────────────────────────
        // FIELDS
        // ─────────────────────────────────────────────────────
        private readonly NhanVienService _nhanVienService;

        private readonly Color _colorSuccess = Color.FromArgb(46, 204, 113);
        private readonly Color _colorError = Color.FromArgb(233, 69, 96);
        private readonly Color _colorInfo = Color.FromArgb(52, 152, 219);

        // ─────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────
        public frmNhanVien()
        {
            InitializeComponent();
            _nhanVienService = new NhanVienService();
            this.Load += new EventHandler(frmNhanVien_Load);
        }

        // ─────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────
        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            ThietLapCotDgv();
            TaiDanhSachNhanVien();
            DatTrangThaiNut(false);
            CapNhatTrangThaiCa();
        }

        // ─────────────────────────────────────────────────────
        // THIẾT LẬP CỘT DataGridView
        // *** LUẬT: AutoGenerateColumns = false TRƯỚC Clear ***
        // ─────────────────────────────────────────────────────
        private void ThietLapCotDgv()
        {
            // BẮT BUỘC: đặt false TRƯỚC khi Clear
            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.Columns.Clear();

            // Cột ẩn ID
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "NhanVienId";
            colId.DataPropertyName = "NhanVienId";
            colId.Visible = false;
            dgvNhanVien.Columns.Add(colId);

            // Họ tên
            DataGridViewTextBoxColumn colTen = new DataGridViewTextBoxColumn();
            colTen.Name = "HoTen";
            colTen.HeaderText = "Họ và tên";
            colTen.DataPropertyName = "HoTen";
            colTen.FillWeight = 25;
            dgvNhanVien.Columns.Add(colTen);

            // SĐT
            DataGridViewTextBoxColumn colSDT = new DataGridViewTextBoxColumn();
            colSDT.Name = "SoDienThoai";
            colSDT.HeaderText = "Số điện thoại";
            colSDT.DataPropertyName = "SoDienThoai";
            colSDT.FillWeight = 14;
            colSDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNhanVien.Columns.Add(colSDT);

            // Email
            DataGridViewTextBoxColumn colEmail = new DataGridViewTextBoxColumn();
            colEmail.Name = "Email";
            colEmail.HeaderText = "Email";
            colEmail.DataPropertyName = "Email";
            colEmail.FillWeight = 22;
            dgvNhanVien.Columns.Add(colEmail);

            // Giới tính
            DataGridViewTextBoxColumn colGT = new DataGridViewTextBoxColumn();
            colGT.Name = "GioiTinh";
            colGT.HeaderText = "Giới tính";
            colGT.DataPropertyName = "GioiTinh";
            colGT.FillWeight = 9;
            colGT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNhanVien.Columns.Add(colGT);

            // Ngày vào làm
            DataGridViewTextBoxColumn colNgay = new DataGridViewTextBoxColumn();
            colNgay.Name = "NgayVaoLam";
            colNgay.HeaderText = "Ngày vào làm";
            colNgay.DataPropertyName = "NgayVaoLam";
            colNgay.FillWeight = 12;
            colNgay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNhanVien.Columns.Add(colNgay);

            // Vai trò
            DataGridViewTextBoxColumn colVT = new DataGridViewTextBoxColumn();
            colVT.Name = "TenVaiTro";
            colVT.HeaderText = "Vai trò";
            colVT.DataPropertyName = "TenVaiTro";
            colVT.FillWeight = 11;
            colVT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNhanVien.Columns.Add(colVT);

            // Trạng thái TK
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn();
            colTT.Name = "TrangThaiTK";
            colTT.HeaderText = "Tài khoản";
            colTT.DataPropertyName = "TrangThaiTK";
            colTT.FillWeight = 10;
            colTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNhanVien.Columns.Add(colTT);

            // Style header
            dgvNhanVien.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvNhanVien.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(26, 26, 46);
            dgvNhanVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNhanVien.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgvNhanVien.EnableHeadersVisualStyles = false;
        }

        // ─────────────────────────────────────────────────────
        // TẢI DANH SÁCH NHÂN VIÊN
        // ─────────────────────────────────────────────────────
        private void TaiDanhSachNhanVien()
        {
            try
            {
                List<NhanVienDto> danhSach = _nhanVienService.GetDanhSach();
                HienThiLenLuoi(danhSach);
            }
            catch (Exception ex)
            {
                HienThiThongBaoLeft("Lỗi tải dữ liệu: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // *** HÀM TRUNG TÂM: HIỂN THỊ LÊN LƯỚI BẰNG BindingSource ***
        // BẮT BUỘC dùng BindingSource, KHÔNG gán List trực tiếp
        // ─────────────────────────────────────────────────────
        private void HienThiLenLuoi(object data)
        {
            // Bước 1: Wrap qua BindingSource
            BindingSource bs = new BindingSource();
            bs.DataSource = data;

            // Bước 2: Gán BindingSource vào DataGridView
            dgvNhanVien.DataSource = bs;

            // Bước 3: Tô màu cột Trạng thái TK
            ToMauTrangThai();
        }

        // ─────────────────────────────────────────────────────
        // TÔ MÀU CỘT TRẠNG THÁI
        // ─────────────────────────────────────────────────────
        private void ToMauTrangThai()
        {
            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.Cells["TrangThaiTK"].Value == null)
                    continue;

                string tt = row.Cells["TrangThaiTK"].Value.ToString();

                if (tt == "Hoạt động")
                {
                    row.Cells["TrangThaiTK"].Style.ForeColor =
                        Color.FromArgb(46, 204, 113);
                    row.Cells["TrangThaiTK"].Style.Font =
                        new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (tt == "Bị khóa")
                {
                    row.Cells["TrangThaiTK"].Style.ForeColor =
                        Color.FromArgb(233, 69, 96);
                }
                else
                {
                    row.Cells["TrangThaiTK"].Style.ForeColor =
                        Color.FromArgb(149, 165, 166);
                }
            }
        }

        // ─────────────────────────────────────────────────────
        // CLICK VÀO HÀNG → FILL DỮ LIỆU LÊN FORM
        // ─────────────────────────────────────────────────────
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            int nhanVienId = Convert.ToInt32(row.Cells["NhanVienId"].Value);
            txtNhanVienIdHidden.Text = nhanVienId.ToString();

            // Gọi BLL để lấy đầy đủ dữ liệu
            DAL.NhanVien nv = _nhanVienService.GetById(nhanVienId);
            if (nv == null)
            {
                HienThiThongBaoLeft("Không tìm thấy nhân viên. Hãy làm mới.", false);
                return;
            }

            // Điền dữ liệu lên form
            txtHoTen.Text = nv.HoTen;
            txtSoDienThoai.Text = nv.SoDienThoai ?? "";
            txtEmail.Text = nv.Email ?? "";

            ChonComboBoxTheoGiaTri(cboGioiTinh, nv.GioiTinh ?? "Nam");

            dtpNgayVaoLam.Value = nv.NgayVaoLam;

            DatTrangThaiNut(true);
            HienThiThongBaoLeft("", true);
        }

        // ─────────────────────────────────────────────────────
        // NÚT THÊM
        // ─────────────────────────────────────────────────────
        private void btnThem_Click(object sender, EventArgs e)
        {
            string gioiTinh = cboGioiTinh.SelectedItem != null
                ? cboGioiTinh.SelectedItem.ToString()
                : "Nam";

            ServiceResult ketQua = _nhanVienService.ThemNhanVien(
                txtHoTen.Text,
                txtSoDienThoai.Text,
                txtEmail.Text,
                gioiTinh,
                null,          // NgaySinh - chưa có TextBox, để null
                null,          // DiaChi
                dtpNgayVaoLam.Value.Date);

            HienThiThongBaoLeft(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                LamMoiForm();
                TaiDanhSachNhanVien();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT SỬA
        // ─────────────────────────────────────────────────────
        private void btnSua_Click(object sender, EventArgs e)
        {
            int nhanVienId = 0;
            if (!int.TryParse(txtNhanVienIdHidden.Text, out nhanVienId) || nhanVienId <= 0)
            {
                HienThiThongBaoLeft("Vui lòng chọn nhân viên từ danh sách.", false);
                return;
            }

            string gioiTinh = cboGioiTinh.SelectedItem != null
                ? cboGioiTinh.SelectedItem.ToString()
                : "Nam";

            ServiceResult ketQua = _nhanVienService.SuaNhanVien(
                nhanVienId,
                txtHoTen.Text,
                txtSoDienThoai.Text,
                txtEmail.Text,
                gioiTinh,
                null,
                null,
                dtpNgayVaoLam.Value.Date);

            HienThiThongBaoLeft(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
                TaiDanhSachNhanVien();
        }

        // ─────────────────────────────────────────────────────
        // NÚT XÓA
        // ─────────────────────────────────────────────────────
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int nhanVienId = 0;
            if (!int.TryParse(txtNhanVienIdHidden.Text, out nhanVienId) || nhanVienId <= 0)
            {
                HienThiThongBaoLeft("Vui lòng chọn nhân viên cần xóa.", false);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                string.Format("Bạn có chắc muốn xóa nhân viên:\n\"{0}\" không?", txtHoTen.Text),
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            ServiceResult ketQua = _nhanVienService.XoaNhanVien(nhanVienId);
            HienThiThongBaoLeft(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                LamMoiForm();
                TaiDanhSachNhanVien();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT LÀM MỚI
        // ─────────────────────────────────────────────────────
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            HienThiThongBaoLeft("", true);
        }

        // ─────────────────────────────────────────────────────
        // TÌM KIẾM REALTIME
        // ─────────────────────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                List<NhanVienDto> ketQua = _nhanVienService.TimKiem(keyword);
                HienThiLenLuoi(ketQua);
            }
            catch (Exception ex)
            {
                HienThiThongBaoLeft("Lỗi tìm kiếm: " + ex.Message, false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch_TextChanged(sender, e);
        }

        // ─────────────────────────────────────────────────────
        // CẬP NHẬT TRẠNG THÁI CA LÀM VIỆC TRÊN PANEL PHẢI
        // ─────────────────────────────────────────────────────
        private void CapNhatTrangThaiCa()
        {
            int nhanVienId = SessionManager.NhanVienId;
            if (nhanVienId <= 0)
            {
                lblTrangThaiCa.Text = "Chưa đăng nhập";
                lblTrangThaiCa.ForeColor = Color.FromArgb(149, 165, 166);
                btnMoCa.Enabled = false;
                btnDongCa.Enabled = false;
                return;
            }

            TrangThaiCaDto trangThai = _nhanVienService.GetTrangThaiCa(nhanVienId);

            if (trangThai.DangMoCa)
            {
                // Đang có ca mở
                lblTrangThaiCa.Text = trangThai.MoTaTrangThai;
                lblTrangThaiCa.ForeColor = Color.FromArgb(46, 204, 113);

                lblDoanhThuTienMat.Text = string.Format("Tiền mặt:         {0} ₫",
                    trangThai.TongThuTienMat.ToString("N0"));
                lblDoanhThuCK.Text = string.Format("Chuyển khoản:  {0} ₫",
                    trangThai.TongThuChuyenKhoan.ToString("N0"));
                lblDoanhThuThe.Text = string.Format("Thẻ:                {0} ₫",
                    trangThai.TongThuThe.ToString("N0"));
                lblDoanhThuTong.Text = string.Format("Tổng:  {0} ₫",
                    trangThai.TongThuTatCa.ToString("N0"));

                btnMoCa.Enabled = false;
                btnDongCa.Enabled = true;

                // THAY VÌ ĐỂ TRỐNG HOẶC DISABLE BÌNH THƯỜNG, TA HIỂN THỊ LẠI SỐ TIỀN
                txtTienDauCa.Text = trangThai.TienDauCa.ToString("N0");
                txtTienDauCa.Enabled = false;
            }
            else
            {
                // Chưa mở ca
                lblTrangThaiCa.Text = trangThai.MoTaTrangThai;
                lblTrangThaiCa.ForeColor = Color.FromArgb(149, 165, 166);

                lblDoanhThuTienMat.Text = "Tiền mặt:         ---";
                lblDoanhThuCK.Text = "Chuyển khoản:  ---";
                lblDoanhThuThe.Text = "Thẻ:                ---";
                lblDoanhThuTong.Text = "Tổng:  0 ₫";

                btnMoCa.Enabled = true;
                btnDongCa.Enabled = false;
                txtTienDauCa.Enabled = true;

                // THÊM DÒNG NÀY ĐỂ QUÉT SẠCH SỐ TIỀN CỦA CA CŨ:
                txtTienDauCa.Text = "";
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT MỞ CA
        // ─────────────────────────────────────────────────────
        private void btnMoCa_Click(object sender, EventArgs e)
        {
            decimal tienDauCa = 0;
            if (!string.IsNullOrWhiteSpace(txtTienDauCa.Text))
            {
                if (!decimal.TryParse(txtTienDauCa.Text.Trim(), out tienDauCa))
                {
                    HienThiThongBaoCa("Tiền đầu ca phải là số (VD: 500000).", false);
                    txtTienDauCa.Focus();
                    return;
                }
            }

            ServiceResult ketQua = _nhanVienService.MoCa(
                SessionManager.NhanVienId, tienDauCa);

            HienThiThongBaoCa(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                txtTienDauCa.Text = "";
                CapNhatTrangThaiCa();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT ĐÓNG CA
        // ─────────────────────────────────────────────────────
        private void btnDongCa_Click(object sender, EventArgs e)
        {
            // Lấy thông tin ca để hiển thị trong confirm dialog
            TrangThaiCaDto trangThai = _nhanVienService.GetTrangThaiCa(SessionManager.NhanVienId);
            if (!trangThai.DangMoCa)
            {
                HienThiThongBaoCa("Không tìm thấy ca đang mở.", false);
                return;
            }

            // Tính số tiền thực tế phải có trong két
            decimal tienMatTrongKet = trangThai.TienDauCa + trangThai.TongThuTienMat;

            DialogResult confirm = MessageBox.Show(
                string.Format(
                    "XÁC NHẬN CHỐT CA LÀM VIỆC?\n" +
                    "────────────────────────────────\n" +
                    "1. DOANH THU BÁN VÉ:\n" +
                    "  - Tiền mặt:        {0} ₫\n" +
                    "  - Chuyển khoản: {1} ₫\n" +
                    "  - Thẻ:               {2} ₫\n" +
                    "  => TỔNG DOANH THU: {3} ₫\n\n" +
                    "2. KIỂM KÊ KÉT TIỀN:\n" +
                    "  - Tiền vốn đầu ca: {4} ₫\n" +
                    "  => TỔNG TIỀN MẶT TRONG KÉT: {5} ₫\n" +
                    "────────────────────────────────\n" +
                    "Ghi chú chốt ca: {6}",
                    trangThai.TongThuTienMat.ToString("N0"),
                    trangThai.TongThuChuyenKhoan.ToString("N0"),
                    trangThai.TongThuThe.ToString("N0"),
                    trangThai.TongThuTatCa.ToString("N0"),
                    trangThai.TienDauCa.ToString("N0"),
                    tienMatTrongKet.ToString("N0"),
                    string.IsNullOrWhiteSpace(txtGhiChu.Text) ? "(Không có)" : txtGhiChu.Text
                ),
                "Xác nhận đóng ca",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            ServiceResult ketQua = _nhanVienService.DongCa(
                SessionManager.NhanVienId,
                txtGhiChu.Text);

            HienThiThongBaoCa(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                txtGhiChu.Text = "";
                CapNhatTrangThaiCa();
            }
        }

        // ─────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ─────────────────────────────────────────────────────

        private void LamMoiForm()
        {
            txtNhanVienIdHidden.Text = "0";
            txtHoTen.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            cboGioiTinh.SelectedIndex = 0;
            dtpNgayVaoLam.Value = DateTime.Today;
            DatTrangThaiNut(false);
            dgvNhanVien.ClearSelection();
            txtHoTen.Focus();
        }

        private void DatTrangThaiNut(bool daDaChon)
        {
            btnSua.Enabled = daDaChon;
            btnXoa.Enabled = daDaChon;
        }

        private void HienThiThongBaoLeft(string thongBao, bool thanhCong)
        {
            lblThongBaoLeft.Text = thongBao;
            lblThongBaoLeft.ForeColor = thanhCong ? _colorSuccess : _colorError;
            lblThongBaoLeft.Visible = !string.IsNullOrEmpty(thongBao);

            if (thanhCong && !string.IsNullOrEmpty(thongBao))
            {
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                t.Interval = 4000;
                t.Tick += delegate (object s, EventArgs ev)
                {
                    lblThongBaoLeft.Text = "";
                    lblThongBaoLeft.Visible = false;
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
        }

        private void HienThiThongBaoCa(string thongBao, bool thanhCong)
        {
            lblThongBaoCa.Text = thongBao;
            lblThongBaoCa.ForeColor = thanhCong ? _colorSuccess : _colorError;

            if (thanhCong && !string.IsNullOrEmpty(thongBao))
            {
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                t.Interval = 5000;
                t.Tick += delegate (object s, EventArgs ev)
                {
                    lblThongBaoCa.Text = "";
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
        }

        private void ChonComboBoxTheoGiaTri(ComboBox cbo, string giaTri)
        {
            if (string.IsNullOrEmpty(giaTri))
                return;

            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (cbo.Items[i].ToString() == giaTri)
                {
                    cbo.SelectedIndex = i;
                    return;
                }
            }
        }
    }
}