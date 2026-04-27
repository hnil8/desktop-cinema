using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.BLL.Services;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmLichChieu.cs
// Tầng GUI: Xử lý sự kiện, KHÔNG chứa logic nghiệp vụ
//
// TUÂN THỦ LUẬT:
//   ✓ AutoGenerateColumns = false TRƯỚC Columns.Clear()
//   ✓ Dùng BindingSource trong HienThiLenLuoi() để bind DGV
//   ✓ Không dùng C# 8+ syntax
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmLichChieu : Form
    {
        // ─────────────────────────────────────────────────────
        // FIELDS
        // ─────────────────────────────────────────────────────
        private readonly LichChieuService _lichChieuService;

        private readonly Color _colorSuccess = Color.FromArgb(46, 204, 113);
        private readonly Color _colorError = Color.FromArgb(233, 69, 96);

        // ─────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────
        public frmLichChieu()
        {
            InitializeComponent();
            _lichChieuService = new LichChieuService();
            this.Load += new EventHandler(frmLichChieu_Load);
        }

        // ─────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────
        private void frmLichChieu_Load(object sender, EventArgs e)
        {
            // Thứ tự quan trọng: thiết lập cột TRƯỚC khi tải dữ liệu
            ThietLapCotDgv();
            NapDuLieuComboBox();
            TaiDanhSachLichChieu();
            DatTrangThaiNut(false);
        }

        // ─────────────────────────────────────────────────────
        // THIẾT LẬP CỘT DataGridView
        // *** LUẬT BẮT BUỘC: AutoGenerateColumns = false TRƯỚC Clear ***
        // ─────────────────────────────────────────────────────
        private void ThietLapCotDgv()
        {
            // BẮT BUỘC: đặt false TRƯỚC khi Clear để DGV không tự sinh cột
            dgvLichChieu.AutoGenerateColumns = false;
            dgvLichChieu.Columns.Clear();

            // Cột ẩn: LichChieuId
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "LichChieuId";
            colId.DataPropertyName = "LichChieuId";
            colId.Visible = false;
            dgvLichChieu.Columns.Add(colId);

            // Cột ẩn: PhimId (cần để populate ComboBox khi click hàng)
            DataGridViewTextBoxColumn colPhimId = new DataGridViewTextBoxColumn();
            colPhimId.Name = "PhimId";
            colPhimId.DataPropertyName = "PhimId";
            colPhimId.Visible = false;
            dgvLichChieu.Columns.Add(colPhimId);

            // Cột ẩn: PhongId
            DataGridViewTextBoxColumn colPhongId = new DataGridViewTextBoxColumn();
            colPhongId.Name = "PhongId";
            colPhongId.DataPropertyName = "PhongId";
            colPhongId.Visible = false;
            dgvLichChieu.Columns.Add(colPhongId);

            // Cột ẩn: GioBatDau (raw DateTime, dùng để set DateTimePicker)
            DataGridViewTextBoxColumn colGioBDRaw = new DataGridViewTextBoxColumn();
            colGioBDRaw.Name = "GioBatDau";
            colGioBDRaw.DataPropertyName = "GioBatDau";
            colGioBDRaw.Visible = false;
            dgvLichChieu.Columns.Add(colGioBDRaw);

            // Cột ẩn: GioKetThuc (raw DateTime)
            DataGridViewTextBoxColumn colGioKTRaw = new DataGridViewTextBoxColumn();
            colGioKTRaw.Name = "GioKetThuc";
            colGioKTRaw.DataPropertyName = "GioKetThuc";
            colGioKTRaw.Visible = false;
            dgvLichChieu.Columns.Add(colGioKTRaw);

            // Cột ẩn: GiaVeCoBan (raw decimal)
            DataGridViewTextBoxColumn colGiaRaw = new DataGridViewTextBoxColumn();
            colGiaRaw.Name = "GiaVeCoBan";
            colGiaRaw.DataPropertyName = "GiaVeCoBan";
            colGiaRaw.Visible = false;
            dgvLichChieu.Columns.Add(colGiaRaw);

            // Cột ẩn: TrangThai (raw string cho ComboBox)
            DataGridViewTextBoxColumn colTTRaw = new DataGridViewTextBoxColumn();
            colTTRaw.Name = "TrangThai";
            colTTRaw.DataPropertyName = "TrangThai";
            colTTRaw.Visible = false;
            dgvLichChieu.Columns.Add(colTTRaw);

            // --- Các cột hiển thị ---

            // Tên phim
            DataGridViewTextBoxColumn colTenPhim = new DataGridViewTextBoxColumn();
            colTenPhim.Name = "TenPhim";
            colTenPhim.HeaderText = "Tên phim";
            colTenPhim.DataPropertyName = "TenPhim";
            colTenPhim.FillWeight = 28;
            dgvLichChieu.Columns.Add(colTenPhim);

            // Tên phòng
            DataGridViewTextBoxColumn colTenPhong = new DataGridViewTextBoxColumn();
            colTenPhong.Name = "TenPhong";
            colTenPhong.HeaderText = "Phòng chiếu";
            colTenPhong.DataPropertyName = "TenPhong";
            colTenPhong.FillWeight = 12;
            colTenPhong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLichChieu.Columns.Add(colTenPhong);

            // Giờ bắt đầu (formatted)
            DataGridViewTextBoxColumn colGioBD = new DataGridViewTextBoxColumn();
            colGioBD.Name = "GioBatDauHienThi";
            colGioBD.HeaderText = "Giờ bắt đầu";
            colGioBD.DataPropertyName = "GioBatDauHienThi";
            colGioBD.FillWeight = 17;
            colGioBD.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLichChieu.Columns.Add(colGioBD);

            // Giờ kết thúc (formatted)
            DataGridViewTextBoxColumn colGioKT = new DataGridViewTextBoxColumn();
            colGioKT.Name = "GioKetThucHienThi";
            colGioKT.HeaderText = "Giờ kết thúc";
            colGioKT.DataPropertyName = "GioKetThucHienThi";
            colGioKT.FillWeight = 17;
            colGioKT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLichChieu.Columns.Add(colGioKT);

            // Giá vé (formatted)
            DataGridViewTextBoxColumn colGia = new DataGridViewTextBoxColumn();
            colGia.Name = "GiaVeHienThi";
            colGia.HeaderText = "Giá vé";
            colGia.DataPropertyName = "GiaVeHienThi";
            colGia.FillWeight = 13;
            colGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvLichChieu.Columns.Add(colGia);

            // Trạng thái (tiếng Việt)
            DataGridViewTextBoxColumn colTT = new DataGridViewTextBoxColumn();
            colTT.Name = "TrangThaiHienThi";
            colTT.HeaderText = "Trạng thái";
            colTT.DataPropertyName = "TrangThaiHienThi";
            colTT.FillWeight = 13;
            colTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLichChieu.Columns.Add(colTT);

            // Style header
            dgvLichChieu.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvLichChieu.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(26, 26, 46);
            dgvLichChieu.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLichChieu.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgvLichChieu.EnableHeadersVisualStyles = false;
        }

        // ─────────────────────────────────────────────────────
        // NẠP DỮ LIỆU COMBOBOX
        // ─────────────────────────────────────────────────────
        private void NapDuLieuComboBox()
        {
            try
            {
                // ComboBox Phim
                List<PhimComboItem> danhSachPhim =
                    _lichChieuService.GetDanhSachPhimChoCombo();
                cboPhim.DataSource = null;
                cboPhim.DataSource = danhSachPhim;
                cboPhim.DisplayMember = ""; // ToString() của PhimComboItem sẽ hiện
                if (cboPhim.Items.Count > 0)
                    cboPhim.SelectedIndex = -1; // Không chọn sẵn

                // ComboBox Phòng
                List<PhongComboItem> danhSachPhong =
                    _lichChieuService.GetDanhSachPhongChoCombo();
                cboPhong.DataSource = null;
                cboPhong.DataSource = danhSachPhong;
                cboPhong.DisplayMember = "";
                if (cboPhong.Items.Count > 0)
                    cboPhong.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tải danh sách phim/phòng: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // TẢI DANH SÁCH LỊCH CHIẾU
        // ─────────────────────────────────────────────────────
        private void TaiDanhSachLichChieu()
        {
            try
            {
                List<LichChieuDto> danhSach = _lichChieuService.GetDanhSach();
                HienThiLenLuoi(danhSach);
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tải dữ liệu: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // *** HÀM TRUNG TÂM: HIỂN THỊ DỮ LIỆU LÊN LƯỚI ***
        // BẮT BUỘC dùng BindingSource để tránh DGV tàng hình
        // Áp dụng cho cả Load lần đầu và khi Tìm kiếm
        // ─────────────────────────────────────────────────────
        private void HienThiLenLuoi(object data)
        {
            // Bước 1: Wrap qua BindingSource trước khi gán DataSource
            BindingSource bs = new BindingSource();
            bs.DataSource = data;

            // Bước 2: Gán BindingSource (không gán list trực tiếp)
            dgvLichChieu.DataSource = bs;

            // Bước 3: Tô màu trạng thái sau khi bind xong
            ToMauTrangThai();
        }

        // ─────────────────────────────────────────────────────
        // TÔ MÀU TRẠNG THÁI
        // ─────────────────────────────────────────────────────
        private void ToMauTrangThai()
        {
            foreach (DataGridViewRow row in dgvLichChieu.Rows)
            {
                if (row.Cells["TrangThaiHienThi"].Value == null)
                    continue;

                string tt = row.Cells["TrangThaiHienThi"].Value.ToString();

                if (tt == "Đang chiếu")
                {
                    row.Cells["TrangThaiHienThi"].Style.ForeColor =
                        Color.FromArgb(46, 204, 113);
                    row.Cells["TrangThaiHienThi"].Style.Font =
                        new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (tt == "Chưa chiếu")
                {
                    row.Cells["TrangThaiHienThi"].Style.ForeColor =
                        Color.FromArgb(52, 152, 219);
                    row.Cells["TrangThaiHienThi"].Style.Font =
                        new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (tt == "Hủy chiếu")
                {
                    row.Cells["TrangThaiHienThi"].Style.ForeColor =
                        Color.FromArgb(233, 69, 96);
                }
                else if (tt == "Đã kết thúc")
                {
                    row.Cells["TrangThaiHienThi"].Style.ForeColor =
                        Color.FromArgb(149, 165, 166);
                }
            }
        }

        // ─────────────────────────────────────────────────────
        // CLICK VÀO HÀNG TRÊN LƯỚI → FILL DỮ LIỆU LÊN FORM
        // ─────────────────────────────────────────────────────
        private void dgvLichChieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvLichChieu.Rows[e.RowIndex];

            // Lưu ID vào hidden field
            int lichChieuId = Convert.ToInt32(row.Cells["LichChieuId"].Value);
            txtLichChieuIdHidden.Text = lichChieuId.ToString();

            // Lấy raw values từ các cột ẩn
            int phimId = Convert.ToInt32(row.Cells["PhimId"].Value);
            int phongId = Convert.ToInt32(row.Cells["PhongId"].Value);
            DateTime gioBatDau = Convert.ToDateTime(row.Cells["GioBatDau"].Value);
            decimal giaVeCoBan = Convert.ToDecimal(row.Cells["GiaVeCoBan"].Value);
            string trangThai = row.Cells["TrangThai"].Value.ToString();

            // Chọn đúng item trong cboPhim theo PhimId
            ChonComboTheoId(cboPhim, phimId, "PhimId");

            // Chọn đúng item trong cboPhong theo PhongId
            ChonComboTheoId(cboPhong, phongId, "PhongId");

            // Set DateTimePicker ngày và giờ (tách từ raw DateTime)
            dtpNgayChieu.Value = gioBatDau.Date;
            dtpGioBatDau.Value = gioBatDau; // DateTimePicker lấy phần giờ qua TimeOfDay

            // Set giá vé
            txtGiaVe.Text = giaVeCoBan.ToString("N0").Replace(",", "");

            // Chọn trạng thái
            ChonComboBoxTheoGiaTri(cboTrangThai, trangThai);

            // Tính và hiển thị giờ kết thúc
            TinhVaHienThiGioKetThuc();

            // Bật nút Sửa và Xóa
            DatTrangThaiNut(true);
            HienThiThongBao("", true);
        }

        // ─────────────────────────────────────────────────────
        // TÍNH VÀ HIỂN THỊ GIỜ KẾT THÚC (realtime)
        // Gọi khi: chọn phim, đổi ngày, đổi giờ
        // ─────────────────────────────────────────────────────
        private void TinhVaHienThiGioKetThuc()
        {
            if (cboPhim.SelectedItem == null)
            {
                lblGioKetThucValue.Text = "---";
                return;
            }

            PhimComboItem phimChon = cboPhim.SelectedItem as PhimComboItem;
            if (phimChon == null)
            {
                lblGioKetThucValue.Text = "---";
                return;
            }

            try
            {
                // Ghép ngày từ dtpNgayChieu và giờ từ dtpGioBatDau
                DateTime ngay = dtpNgayChieu.Value.Date;
                TimeSpan gio = dtpGioBatDau.Value.TimeOfDay;
                DateTime gioBatDau = ngay + gio;

                DateTime gioKetThuc = _lichChieuService.TinhGioKetThuc(
                    gioBatDau, phimChon.ThoiLuongPhut);

                lblGioKetThucValue.Text = gioKetThuc.ToString("HH:mm  dd/MM/yyyy");
            }
            catch
            {
                lblGioKetThucValue.Text = "---";
            }
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN: Thay đổi phim hoặc ngày/giờ → tính lại GioKetThuc
        // ─────────────────────────────────────────────────────
        private void cboPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinhVaHienThiGioKetThuc();
        }

        private void dtpNgayGio_ValueChanged(object sender, EventArgs e)
        {
            TinhVaHienThiGioKetThuc();
        }

        // ─────────────────────────────────────────────────────
        // NÚT THÊM MỚI
        // ─────────────────────────────────────────────────────
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Lấy PhimId và PhongId từ ComboBox
            int phimId = LayIdTuCombo(cboPhim, "PhimId");
            int phongId = LayIdTuCombo(cboPhong, "PhongId");

            // Ghép DateTime
            DateTime gioBatDau;
            bool ghepOk = GhepNgayGio(out gioBatDau);
            if (!ghepOk)
            {
                HienThiThongBao("Ngày hoặc giờ bắt đầu không hợp lệ.", false);
                return;
            }

            // Parse giá vé
            decimal giaVe = 0;
            if (!decimal.TryParse(txtGiaVe.Text.Trim(), out giaVe))
            {
                HienThiThongBao("Giá vé phải là số (VD: 120000). Không nhập dấu phẩy.", false);
                txtGiaVe.Focus();
                return;
            }

            string trangThai = cboTrangThai.SelectedItem != null
                ? cboTrangThai.SelectedItem.ToString()
                : "ChuaChieu";

            // Lấy thời lượng phim để tính GioKetThuc trong BLL
            int thoiLuong = 0;
            PhimComboItem phimChon = cboPhim.SelectedItem as PhimComboItem;
            if (phimChon != null)
                thoiLuong = phimChon.ThoiLuongPhut;

            ServiceResult ketQua = _lichChieuService.ThemLichChieu(
                phimId, phongId, gioBatDau, thoiLuong, giaVe, trangThai);

            HienThiThongBao(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                LamMoiForm();
                TaiDanhSachLichChieu();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT CẬP NHẬT
        // ─────────────────────────────────────────────────────
        private void btnSua_Click(object sender, EventArgs e)
        {
            int lichChieuId = 0;
            if (!int.TryParse(txtLichChieuIdHidden.Text, out lichChieuId) || lichChieuId <= 0)
            {
                HienThiThongBao("Vui lòng chọn lịch chiếu từ danh sách.", false);
                return;
            }

            int phimId = LayIdTuCombo(cboPhim, "PhimId");
            int phongId = LayIdTuCombo(cboPhong, "PhongId");

            DateTime gioBatDau;
            bool ghepOk = GhepNgayGio(out gioBatDau);
            if (!ghepOk)
            {
                HienThiThongBao("Ngày hoặc giờ bắt đầu không hợp lệ.", false);
                return;
            }

            decimal giaVe = 0;
            if (!decimal.TryParse(txtGiaVe.Text.Trim(), out giaVe))
            {
                HienThiThongBao("Giá vé phải là số (VD: 120000).", false);
                txtGiaVe.Focus();
                return;
            }

            string trangThai = cboTrangThai.SelectedItem != null
                ? cboTrangThai.SelectedItem.ToString()
                : "ChuaChieu";

            int thoiLuong = 0;
            PhimComboItem phimChon = cboPhim.SelectedItem as PhimComboItem;
            if (phimChon != null)
                thoiLuong = phimChon.ThoiLuongPhut;

            ServiceResult ketQua = _lichChieuService.SuaLichChieu(
                lichChieuId, phimId, phongId, gioBatDau, thoiLuong, giaVe, trangThai);

            HienThiThongBao(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
                TaiDanhSachLichChieu();
        }

        // ─────────────────────────────────────────────────────
        // NÚT XÓA
        // ─────────────────────────────────────────────────────
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int lichChieuId = 0;
            if (!int.TryParse(txtLichChieuIdHidden.Text, out lichChieuId) || lichChieuId <= 0)
            {
                HienThiThongBao("Vui lòng chọn lịch chiếu cần xóa.", false);
                return;
            }

            string tenPhim = "";
            string gioBD = "";
            if (dgvLichChieu.CurrentRow != null)
            {
                tenPhim = dgvLichChieu.CurrentRow.Cells["TenPhim"].Value.ToString();
                gioBD = dgvLichChieu.CurrentRow.Cells["GioBatDauHienThi"].Value.ToString();
            }

            DialogResult confirm = MessageBox.Show(
                string.Format(
                    "Bạn có chắc muốn xóa lịch chiếu sau?\n\nPhim: {0}\nGiờ: {1}",
                    tenPhim, gioBD),
                "Xác nhận xóa lịch chiếu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            ServiceResult ketQua = _lichChieuService.XoaLichChieu(lichChieuId);
            HienThiThongBao(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                LamMoiForm();
                TaiDanhSachLichChieu();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT LÀM MỚI
        // ─────────────────────────────────────────────────────
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            HienThiThongBao("", true);
        }

        // ─────────────────────────────────────────────────────
        // TÌM KIẾM REALTIME (TextChanged)
        // ─────────────────────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                List<LichChieuDto> ketQua =
                    _lichChieuService.TimKiemTheoTenPhim(keyword);
                HienThiLenLuoi(ketQua); // Dùng hàm chung với BindingSource
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tìm kiếm: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT TÌM KIẾM (click)
        // ─────────────────────────────────────────────────────
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch_TextChanged(sender, e);
        }

        // ─────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ─────────────────────────────────────────────────────

        /// <summary>Reset toàn bộ form về trạng thái ban đầu.</summary>
        private void LamMoiForm()
        {
            txtLichChieuIdHidden.Text = "0";
            cboPhim.SelectedIndex = -1;
            cboPhong.SelectedIndex = -1;
            dtpNgayChieu.Value = DateTime.Today;
            dtpGioBatDau.Value = DateTime.Today.Date.AddHours(8);
            txtGiaVe.Text = "";
            cboTrangThai.SelectedIndex = 0;
            lblGioKetThucValue.Text = "---";
            DatTrangThaiNut(false);
            dgvLichChieu.ClearSelection();
        }

        /// <summary>Bật/tắt nút Sửa và Xóa.</summary>
        private void DatTrangThaiNut(bool daDaChon)
        {
            btnSua.Enabled = daDaChon;
            btnXoa.Enabled = daDaChon;
        }

        /// <summary>Hiển thị thông báo với màu thành công/lỗi.</summary>
        private void HienThiThongBao(string thongBao, bool thanhCong)
        {
            lblThongBao.Text = thongBao;
            lblThongBao.ForeColor = thanhCong ? _colorSuccess : _colorError;

            if (thanhCong && !string.IsNullOrEmpty(thongBao))
            {
                System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                t.Interval = 4000;
                t.Tick += delegate (object s, EventArgs ev)
                {
                    lblThongBao.Text = "";
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
        }

        /// <summary>
        /// Lấy Id (int) từ item đang chọn trong ComboBox.
        /// Dùng reflection-free: cast sang đúng type rồi đọc property.
        /// </summary>
        private int LayIdTuCombo(System.Windows.Forms.ComboBox cbo, string tenProperty)
        {
            if (cbo.SelectedItem == null)
                return 0;

            if (tenProperty == "PhimId")
            {
                PhimComboItem item = cbo.SelectedItem as PhimComboItem;
                return item != null ? item.PhimId : 0;
            }
            else if (tenProperty == "PhongId")
            {
                PhongComboItem item = cbo.SelectedItem as PhongComboItem;
                return item != null ? item.PhongId : 0;
            }

            return 0;
        }

        /// <summary>
        /// Chọn item trong ComboBox theo giá trị Id.
        /// Dùng vòng lặp for cổ điển (C# 7.3 compatible).
        /// </summary>
        private void ChonComboTheoId(System.Windows.Forms.ComboBox cbo, int id, string tenProperty)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (tenProperty == "PhimId")
                {
                    PhimComboItem item = cbo.Items[i] as PhimComboItem;
                    if (item != null && item.PhimId == id)
                    {
                        cbo.SelectedIndex = i;
                        return;
                    }
                }
                else if (tenProperty == "PhongId")
                {
                    PhongComboItem item = cbo.Items[i] as PhongComboItem;
                    if (item != null && item.PhongId == id)
                    {
                        cbo.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Chọn item trong ComboBox theo giá trị chuỗi (cho cboTrangThai).
        /// </summary>
        private void ChonComboBoxTheoGiaTri(System.Windows.Forms.ComboBox cbo, string giaTri)
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

        /// <summary>
        /// Ghép ngày từ dtpNgayChieu và giờ từ dtpGioBatDau thành DateTime hoàn chỉnh.
        /// </summary>
        private bool GhepNgayGio(out DateTime ketQua)
        {
            try
            {
                DateTime ngay = dtpNgayChieu.Value.Date;
                TimeSpan gio = dtpGioBatDau.Value.TimeOfDay;
                ketQua = ngay + gio;
                return true;
            }
            catch
            {
                ketQua = DateTime.MinValue;
                return false;
            }
        }
    }
}