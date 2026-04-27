using System;
using CinemaManagement.DAL;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.BLL.Services;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmQuanLyPhim.cs
// Tầng GUI - Chỉ gọi BLL, xử lý sự kiện UI
// KHÔNG chứa logic nghiệp vụ, KHÔNG query DB trực tiếp
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmQuanLyPhim : Form
    {
        // ─────────────────────────────────────────────────────
        // FIELDS
        // ─────────────────────────────────────────────────────
        private readonly PhimService _phimService;

        // Màu thông báo
        private readonly Color _colorSuccess = Color.FromArgb(46, 204, 113);   // Xanh lá
        private readonly Color _colorError = Color.FromArgb(233, 69, 96);    // Đỏ
        private readonly Color _colorInfo = Color.FromArgb(52, 152, 219);   // Xanh dương

        // ─────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────
        public frmQuanLyPhim()
        {
            InitializeComponent();
            _phimService = new PhimService();

            // Gắn sự kiện Load
            this.Load += new EventHandler(frmQuanLyPhim_Load);
        }

        // ─────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────
        private void frmQuanLyPhim_Load(object sender, EventArgs e)
        {
            ThietLapCotDgv();           // 1. Tạo cột (đã có lệnh chặn rác bên trong hàm này rồi)
            TaiDanhSachPhim();          // 2. Đổ dữ liệu vào cột
            DatTrangThaiNut(false);     // 3. Khóa nút Sửa/Xóa
        }

        // ─────────────────────────────────────────────────────
        // THIẾT LẬP CỘT DataGridView
        // ─────────────────────────────────────────────────────
        private void ThietLapCotDgv()
        {
            // 1. Khóa cửa: Chặn DataGridView tự động đẻ cột rác
            dgvPhim.AutoGenerateColumns = false;
            dgvPhim.Columns.Clear();

            // 2. Tạo đúng 6 cột chúng ta cần
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhimId",
                DataPropertyName = "PhimId",
                Visible = false
            });

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenPhim",
                HeaderText = "Tên phim",
                DataPropertyName = "TenPhim",
                FillWeight = 30
            });

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DaoDien",
                HeaderText = "Đạo diễn",
                DataPropertyName = "DaoDien",
                FillWeight = 15
            });

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThoiLuongPhut",
                HeaderText = "Thời lượng",
                DataPropertyName = "ThoiLuongPhut",
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GioiHanDoTuoi",
                HeaderText = "Độ tuổi",
                DataPropertyName = "GioiHanDoTuoi",
                FillWeight = 8,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayKhoiChieuHienThi",
                HeaderText = "Khởi chiếu",
                DataPropertyName = "NgayKhoiChieuHienThi",
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThaiHienThi",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThaiHienThi",
                FillWeight = 12,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // 3. Style chung cho Header và Dữ liệu (chống tàng hình)
            dgvPhim.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvPhim.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 26, 46);
            dgvPhim.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPhim.DefaultCellStyle.ForeColor = Color.Black; // Chữ màu đen
            dgvPhim.EnableHeadersVisualStyles = false;
        }

        // ─────────────────────────────────────────────────────
        // TẢI DANH SÁCH PHIM LÊN LƯỚI
        // ─────────────────────────────────────────────────────
        // ─────────────────────────────────────────────────────
        // HÀM DÙNG CHUNG ĐỂ ĐỔ DỮ LIỆU LÊN LƯỚI (An toàn 100%)
        // ─────────────────────────────────────────────────────
        private void HienThiLenLuoi(object data)
        {
            // 1. Nhắc lại lệnh cấm tự đẻ cột
            dgvPhim.AutoGenerateColumns = false;

            // 2. Bọc dữ liệu qua BindingSource để chống lỗi mất cột
            var bs = new BindingSource();
            bs.DataSource = data;

            // 3. Gắn vào lưới
            dgvPhim.DataSource = null;
            dgvPhim.DataSource = bs;

            ToMauTrangThai();
        }

        // ─────────────────────────────────────────────────────
        // TẢI DANH SÁCH PHIM LÊN LƯỚI (Lúc mới mở Form)
        // ─────────────────────────────────────────────────────
        private void TaiDanhSachPhim()
        {
            try
            {
                var danhSach = _phimService.GetDanhSach();
                HienThiLenLuoi(danhSach); // Gọi hàm dùng chung
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tải dữ liệu: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // TÌM KIẾM (TextChanged - Gõ tới đâu tìm tới đó)
        // ─────────────────────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                var ketQua = _phimService.TimKiemTheoTen(keyword);
                HienThiLenLuoi(ketQua); // Gọi hàm dùng chung
            }
            catch (Exception ex)
            {
                HienThiThongBao("Lỗi tìm kiếm: " + ex.Message, false);
            }
        }

        // ─────────────────────────────────────────────────────
        // TÔ MÀU TRẠNG THÁI TRÊN LƯỚI
        // ─────────────────────────────────────────────────────
        private void ToMauTrangThai()
        {
            // Dùng vòng lặp foreach truyền thống (.NET 4.8)
            foreach (DataGridViewRow row in dgvPhim.Rows)
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
                else if (tt == "Sắp chiếu")
                {
                    row.Cells["TrangThaiHienThi"].Style.ForeColor =
                        Color.FromArgb(52, 152, 219);
                    row.Cells["TrangThaiHienThi"].Style.Font =
                        new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (tt == "Ngừng chiếu")
                {
                    row.Cells["TrangThaiHienThi"].Style.ForeColor =
                        Color.FromArgb(149, 165, 166);
                }
            }
        }

        // ─────────────────────────────────────────────────────
        // CLICK VÀO HÀNG TRÊN LƯỚI → ĐIỀN DỮ LIỆU VÀO FORM
        // ─────────────────────────────────────────────────────
        private void dgvPhim_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvPhim.Rows[e.RowIndex];

            // Lấy PhimId từ hàng đang chọn
            int phimId = Convert.ToInt32(row.Cells["PhimId"].Value);
            txtPhimIdHidden.Text = phimId.ToString();

            // Gọi BLL để lấy đầy đủ thông tin (bao gồm MoTa không hiện trên lưới)
            DAL.Phim phim = _phimService.GetById(phimId);
            if (phim == null)
            {
                HienThiThongBao("Không tìm thấy phim. Hãy làm mới danh sách.", false);
                return;
            }

            // Điền dữ liệu ngược lên form nhập liệu
            txtTenPhim.Text = phim.TenPhim;
            txtDaoDien.Text = phim.DaoDien ?? "";
            txtDienVien.Text = phim.DienVienChinh ?? "";
            txtThoiLuong.Text = phim.ThoiLuongPhut.ToString();
            txtNuocSanXuat.Text = phim.NuocSanXuat ?? "";
            txtMoTa.Text = phim.MoTa ?? "";

            // ComboBox: tìm và chọn đúng giá trị
            ChonComboBoxTheoGiaTri(cboGioiHanDoTuoi, phim.GioiHanDoTuoi);
            ChonComboBoxTheoGiaTri(cboNgonNgu, phim.NgonNgu ?? "VietSub");
            ChonComboBoxTheoGiaTri(cboTrangThai, phim.TrangThai);

            // Ngày khởi chiếu
            if (phim.NgayKhoiChieu.HasValue)
            {
                chkCoNgayKhoiChieu.Checked = true;
                dtpNgayKhoiChieu.Enabled = true;
                dtpNgayKhoiChieu.Value = phim.NgayKhoiChieu.Value;
            }
            else
            {
                chkCoNgayKhoiChieu.Checked = false;
                dtpNgayKhoiChieu.Enabled = false;
            }

            // Bật nút Sửa và Xóa khi đã chọn phim
            DatTrangThaiNut(true);
            HienThiThongBao("", true); // Xóa thông báo cũ
        }

        // ─────────────────────────────────────────────────────
        // NÚT THÊM MỚI
        // ─────────────────────────────────────────────────────
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Đọc giá trị từ form
            string tenPhim = txtTenPhim.Text;
            string daoDien = txtDaoDien.Text;
            string dienVien = txtDienVien.Text;
            string nuocSanXuat = txtNuocSanXuat.Text;
            string moTa = txtMoTa.Text;
            string doTuoi = cboGioiHanDoTuoi.SelectedItem != null
                                    ? cboGioiHanDoTuoi.SelectedItem.ToString()
                                    : "P";
            string ngonNgu = cboNgonNgu.SelectedItem != null
                                    ? cboNgonNgu.SelectedItem.ToString()
                                    : "VietSub";
            string trangThai = cboTrangThai.SelectedItem != null
                                    ? cboTrangThai.SelectedItem.ToString()
                                    : "SapChieu";

            // Parse thời lượng (validate số ngay tại UI)
            int thoiLuong = 0;
            if (!int.TryParse(txtThoiLuong.Text.Trim(), out thoiLuong))
            {
                HienThiThongBao("Thời lượng phải là số nguyên (VD: 120).", false);
                txtThoiLuong.Focus();
                return;
            }

            DateTime? ngayKhoiChieu = chkCoNgayKhoiChieu.Checked
                ? (DateTime?)dtpNgayKhoiChieu.Value.Date
                : null;

            // Gọi BLL
            ServiceResult ketQua = _phimService.ThemPhim(
                tenPhim, "",   // TenGoc để trống (có thể bổ sung TextBox sau)
                daoDien, dienVien, thoiLuong,
                nuocSanXuat, null,
                doTuoi, ngonNgu, moTa, "",
                trangThai, ngayKhoiChieu);

            HienThiThongBao(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                LamMoiForm();
                TaiDanhSachPhim();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT CẬP NHẬT (SỬA)
        // ─────────────────────────────────────────────────────
        private void btnSua_Click(object sender, EventArgs e)
        {
            int phimId = 0;
            if (!int.TryParse(txtPhimIdHidden.Text, out phimId) || phimId <= 0)
            {
                HienThiThongBao("Vui lòng chọn phim từ danh sách.", false);
                return;
            }

            string tenPhim = txtTenPhim.Text;
            string daoDien = txtDaoDien.Text;
            string dienVien = txtDienVien.Text;
            string nuocSanXuat = txtNuocSanXuat.Text;
            string moTa = txtMoTa.Text;
            string doTuoi = cboGioiHanDoTuoi.SelectedItem != null
                                    ? cboGioiHanDoTuoi.SelectedItem.ToString()
                                    : "P";
            string ngonNgu = cboNgonNgu.SelectedItem != null
                                    ? cboNgonNgu.SelectedItem.ToString()
                                    : "VietSub";
            string trangThai = cboTrangThai.SelectedItem != null
                                    ? cboTrangThai.SelectedItem.ToString()
                                    : "SapChieu";

            int thoiLuong = 0;
            if (!int.TryParse(txtThoiLuong.Text.Trim(), out thoiLuong))
            {
                HienThiThongBao("Thời lượng phải là số nguyên.", false);
                txtThoiLuong.Focus();
                return;
            }

            DateTime? ngayKhoiChieu = chkCoNgayKhoiChieu.Checked
                ? (DateTime?)dtpNgayKhoiChieu.Value.Date
                : null;

            ServiceResult ketQua = _phimService.SuaPhim(
                phimId,
                tenPhim, "", daoDien, dienVien, thoiLuong,
                nuocSanXuat, null,
                doTuoi, ngonNgu, moTa, "",
                trangThai, ngayKhoiChieu);

            HienThiThongBao(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                TaiDanhSachPhim();
                // Giữ nguyên dữ liệu trên form để user thấy kết quả
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT XÓA
        // ─────────────────────────────────────────────────────
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int phimId = 0;
            if (!int.TryParse(txtPhimIdHidden.Text, out phimId) || phimId <= 0)
            {
                HienThiThongBao("Vui lòng chọn phim cần xóa.", false);
                return;
            }

            // Xác nhận trước khi xóa
            string tenPhim = txtTenPhim.Text;
            DialogResult confirm = MessageBox.Show(
                string.Format("Bạn có chắc muốn xóa phim:\n\"{0}\" không?", tenPhim),
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            ServiceResult ketQua = _phimService.XoaPhim(phimId);

            HienThiThongBao(ketQua.Message, ketQua.Success);

            if (ketQua.Success)
            {
                LamMoiForm();
                TaiDanhSachPhim();
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT LÀM MỚI FORM
        // ─────────────────────────────────────────────────────
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
            HienThiThongBao("", true);
        }

        // ─────────────────────────────────────────────────────
        // TÌM KIẾM (TextChanged - realtime)
        // ─────────────────────────────────────────────────────
 //       private void txtSearch_TextChanged(object sender, EventArgs e)
 //       {
            // Tìm kiếm realtime khi gõ
 //           string keyword = txtSearch.Text.Trim();

 //           try
 //           {
 //               var ketQua = _phimService.TimKiemTheoTen(keyword);
 //               dgvPhim.DataSource = null;
 //               dgvPhim.DataSource = ketQua;
 //               ToMauTrangThai();
 //           }
 //           catch (Exception ex)
 //           {
 //               HienThiThongBao("Lỗi tìm kiếm: " + ex.Message, false);
 //           }
 //       }

        // ─────────────────────────────────────────────────────
        // NÚT TÌM KIẾM (click button)
        // ─────────────────────────────────────────────────────
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch_TextChanged(sender, e);
        }

        // ─────────────────────────────────────────────────────
        // CHECKBOX NGÀY KHỞI CHIẾU
        // ─────────────────────────────────────────────────────
        private void chkCoNgayKhoiChieu_CheckedChanged(object sender, EventArgs e)
        {
            dtpNgayKhoiChieu.Enabled = chkCoNgayKhoiChieu.Checked;
        }

        // ─────────────────────────────────────────────────────
        // PRIVATE HELPERS
        // ─────────────────────────────────────────────────────

        /// <summary>
        /// Reset toàn bộ controls nhập liệu về trạng thái ban đầu.
        /// </summary>
        private void LamMoiForm()
        {
            txtPhimIdHidden.Text = "0";
            txtTenPhim.Text = "";
            txtDaoDien.Text = "";
            txtDienVien.Text = "";
            txtThoiLuong.Text = "";
            txtNuocSanXuat.Text = "";
            txtMoTa.Text = "";
            cboGioiHanDoTuoi.SelectedIndex = 0;
            cboNgonNgu.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            chkCoNgayKhoiChieu.Checked = false;
            dtpNgayKhoiChieu.Enabled = false;
            dtpNgayKhoiChieu.Value = DateTime.Today;

            DatTrangThaiNut(false);
            dgvPhim.ClearSelection();
            txtTenPhim.Focus();
        }

        /// <summary>
        /// Bật/tắt nút Sửa và Xóa dựa vào trạng thái chọn phim.
        /// </summary>
        private void DatTrangThaiNut(bool daDaChonPhim)
        {
            btnSua.Enabled = daDaChonPhim;
            btnXoa.Enabled = daDaChonPhim;
        }

        /// <summary>
        /// Hiển thị thông báo kết quả thao tác với màu tương ứng.
        /// </summary>
        private void HienThiThongBao(string thongBao, bool thanhCong)
        {
            lblThongBao.Text = thongBao;
            lblThongBao.ForeColor = thanhCong ? _colorSuccess : _colorError;

            // Tự xóa thông báo thành công sau 4 giây
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
        /// Tìm và chọn item trong ComboBox theo giá trị chuỗi.
        /// </summary>
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

            // Không tìm thấy → giữ nguyên lựa chọn hiện tại
        }
    }
}
