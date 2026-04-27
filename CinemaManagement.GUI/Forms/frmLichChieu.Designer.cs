using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmLichChieu.Designer.cs
//
// TUÂN THỦ NGHIÊM NGẶT:
//   ✓ KHÔNG dùng lambda (=>) để gán sự kiện
//   ✓ KHÔNG tạo hàm helper sinh control
//   ✓ KHÔNG dùng thuộc tính AdaptToRounding
//   ✓ Tất cả sự kiện: new System.EventHandler(this.MethodName)
//   ✓ AutoGenerateColumns sẽ được xử lý trong ThietLapCotDgv()
//     ở file code-behind (phải đặt TRƯỚC Columns.Clear())
//
// LAYOUT (DockStyle.Fill, nhúng vào pnlChildContainer frmMain):
// ┌──────────────────────────────────────────────────────────┐
// │ pnlInput (DockStyle.Top, 245px)                          │
// │  [Phim*]            [Ngày*]       [Giờ bắt đầu*] [TT*] │
// │  [Phòng*]           [Giá vé*]     [→ Giờ kết thúc]     │
// │  [+Thêm][✎Sửa][✕Xóa][↺Làm mới]   lblThongBao           │
// ├──────────────────────────────────────────────────────────┤
// │ pnlList (DockStyle.Fill)                                 │
// │  [🔍 Tìm kiếm theo tên phim...]       [Tìm kiếm]        │
// │  [ DataGridView ]                                        │
// └──────────────────────────────────────────────────────────┘
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    partial class frmLichChieu
    {
        private System.ComponentModel.IContainer components = null;

        // ─── PANEL NHẬP LIỆU ─────────────────────────────────
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Panel pnlHeaderAccent;
        private System.Windows.Forms.Label lblHeader;

        // Hàng 1 - Phim
        private System.Windows.Forms.Label lblPhim;
        private System.Windows.Forms.ComboBox cboPhim;

        // Hàng 1 - Ngày chiếu
        private System.Windows.Forms.Label lblNgayChieu;
        private System.Windows.Forms.DateTimePicker dtpNgayChieu;

        // Hàng 1 - Giờ bắt đầu
        private System.Windows.Forms.Label lblGioBatDau;
        private System.Windows.Forms.DateTimePicker dtpGioBatDau;

        // Hàng 1 - Trạng thái
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;

        // Hàng 2 - Phòng chiếu
        private System.Windows.Forms.Label lblPhong;
        private System.Windows.Forms.ComboBox cboPhong;

        // Hàng 2 - Giá vé
        private System.Windows.Forms.Label lblGiaVe;
        private Guna.UI2.WinForms.Guna2TextBox txtGiaVe;

        // Hàng 2 - Giờ kết thúc (tính tự động, chỉ hiển thị)
        private System.Windows.Forms.Label lblGioKetThucTitle;
        private System.Windows.Forms.Label lblGioKetThucValue;

        // Nút hành động
        private Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;

        // Label thông báo
        private System.Windows.Forms.Label lblThongBao;

        // Hidden field lưu LichChieuId đang chọn
        private System.Windows.Forms.TextBox txtLichChieuIdHidden;

        // ─── PANEL DANH SÁCH ─────────────────────────────────
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Panel pnlSearchBar;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvLichChieu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtLichChieuIdHidden = new System.Windows.Forms.TextBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.pnlHeaderAccent = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblPhim = new System.Windows.Forms.Label();
            this.cboPhim = new System.Windows.Forms.ComboBox();
            this.lblNgayChieu = new System.Windows.Forms.Label();
            this.dtpNgayChieu = new System.Windows.Forms.DateTimePicker();
            this.lblGioBatDau = new System.Windows.Forms.Label();
            this.dtpGioBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblPhong = new System.Windows.Forms.Label();
            this.cboPhong = new System.Windows.Forms.ComboBox();
            this.lblGiaVe = new System.Windows.Forms.Label();
            this.txtGiaVe = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGioKetThucTitle = new System.Windows.Forms.Label();
            this.lblGioKetThucValue = new System.Windows.Forms.Label();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.btnSua = new Guna.UI2.WinForms.Guna2Button();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.lblThongBao = new System.Windows.Forms.Label();
            this.pnlList = new System.Windows.Forms.Panel();
            this.dgvLichChieu = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlSearchBar = new System.Windows.Forms.Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.pnlInput.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichChieu)).BeginInit();
            this.pnlSearchBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLichChieuIdHidden
            // 
            this.txtLichChieuIdHidden.Location = new System.Drawing.Point(0, 0);
            this.txtLichChieuIdHidden.Name = "txtLichChieuIdHidden";
            this.txtLichChieuIdHidden.Size = new System.Drawing.Size(100, 20);
            this.txtLichChieuIdHidden.TabIndex = 2;
            this.txtLichChieuIdHidden.Text = "0";
            this.txtLichChieuIdHidden.Visible = false;
            // 
            // pnlInput
            // 
            this.pnlInput.BackColor = System.Drawing.Color.White;
            this.pnlInput.Controls.Add(this.pnlHeaderAccent);
            this.pnlInput.Controls.Add(this.lblHeader);
            this.pnlInput.Controls.Add(this.lblPhim);
            this.pnlInput.Controls.Add(this.cboPhim);
            this.pnlInput.Controls.Add(this.lblNgayChieu);
            this.pnlInput.Controls.Add(this.dtpNgayChieu);
            this.pnlInput.Controls.Add(this.lblGioBatDau);
            this.pnlInput.Controls.Add(this.dtpGioBatDau);
            this.pnlInput.Controls.Add(this.lblTrangThai);
            this.pnlInput.Controls.Add(this.cboTrangThai);
            this.pnlInput.Controls.Add(this.lblPhong);
            this.pnlInput.Controls.Add(this.cboPhong);
            this.pnlInput.Controls.Add(this.lblGiaVe);
            this.pnlInput.Controls.Add(this.txtGiaVe);
            this.pnlInput.Controls.Add(this.lblGioKetThucTitle);
            this.pnlInput.Controls.Add(this.lblGioKetThucValue);
            this.pnlInput.Controls.Add(this.btnThem);
            this.pnlInput.Controls.Add(this.btnSua);
            this.pnlInput.Controls.Add(this.btnXoa);
            this.pnlInput.Controls.Add(this.btnLamMoi);
            this.pnlInput.Controls.Add(this.lblThongBao);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(1020, 245);
            this.pnlInput.TabIndex = 1;
            // 
            // pnlHeaderAccent
            // 
            this.pnlHeaderAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.pnlHeaderAccent.Location = new System.Drawing.Point(20, 14);
            this.pnlHeaderAccent.Name = "pnlHeaderAccent";
            this.pnlHeaderAccent.Size = new System.Drawing.Size(60, 3);
            this.pnlHeaderAccent.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblHeader.Location = new System.Drawing.Point(20, 22);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(146, 20);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Thông tin lịch chiếu";
            // 
            // lblPhim
            // 
            this.lblPhim.AutoSize = true;
            this.lblPhim.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblPhim.Location = new System.Drawing.Point(20, 52);
            this.lblPhim.Name = "lblPhim";
            this.lblPhim.Size = new System.Drawing.Size(43, 15);
            this.lblPhim.TabIndex = 2;
            this.lblPhim.Text = "Phim *";
            // 
            // cboPhim
            // 
            this.cboPhim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhim.DropDownWidth = 420;
            this.cboPhim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPhim.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboPhim.Location = new System.Drawing.Point(20, 72);
            this.cboPhim.Name = "cboPhim";
            this.cboPhim.Size = new System.Drawing.Size(300, 25);
            this.cboPhim.TabIndex = 3;
            this.cboPhim.SelectedIndexChanged += new System.EventHandler(this.cboPhim_SelectedIndexChanged);
            // 
            // lblNgayChieu
            // 
            this.lblNgayChieu.AutoSize = true;
            this.lblNgayChieu.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNgayChieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblNgayChieu.Location = new System.Drawing.Point(340, 52);
            this.lblNgayChieu.Name = "lblNgayChieu";
            this.lblNgayChieu.Size = new System.Drawing.Size(76, 15);
            this.lblNgayChieu.TabIndex = 4;
            this.lblNgayChieu.Text = "Ngày chiếu *";
            // 
            // dtpNgayChieu
            // 
            this.dtpNgayChieu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayChieu.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayChieu.Location = new System.Drawing.Point(340, 72);
            this.dtpNgayChieu.MinDate = new System.DateTime(2026, 4, 1, 0, 0, 0, 0);
            this.dtpNgayChieu.Name = "dtpNgayChieu";
            this.dtpNgayChieu.Size = new System.Drawing.Size(180, 25);
            this.dtpNgayChieu.TabIndex = 5;
            this.dtpNgayChieu.ValueChanged += new System.EventHandler(this.dtpNgayGio_ValueChanged);
            // 
            // lblGioBatDau
            // 
            this.lblGioBatDau.AutoSize = true;
            this.lblGioBatDau.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGioBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblGioBatDau.Location = new System.Drawing.Point(540, 52);
            this.lblGioBatDau.Name = "lblGioBatDau";
            this.lblGioBatDau.Size = new System.Drawing.Size(80, 15);
            this.lblGioBatDau.TabIndex = 6;
            this.lblGioBatDau.Text = "Giờ bắt đầu *";
            // 
            // dtpGioBatDau
            // 
            this.dtpGioBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpGioBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpGioBatDau.Location = new System.Drawing.Point(540, 72);
            this.dtpGioBatDau.Name = "dtpGioBatDau";
            this.dtpGioBatDau.ShowUpDown = true;
            this.dtpGioBatDau.Size = new System.Drawing.Size(160, 25);
            this.dtpGioBatDau.TabIndex = 7;
            this.dtpGioBatDau.ValueChanged += new System.EventHandler(this.dtpNgayGio_ValueChanged);
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblTrangThai.Location = new System.Drawing.Point(720, 52);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(70, 15);
            this.lblTrangThai.TabIndex = 8;
            this.lblTrangThai.Text = "Trạng thái *";
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTrangThai.Items.AddRange(new object[] {
            "ChuaChieu",
            "DangChieu",
            "DaKetThuc",
            "HuyChieu"});
            this.cboTrangThai.Location = new System.Drawing.Point(720, 72);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(270, 25);
            this.cboTrangThai.TabIndex = 9;
            // 
            // lblPhong
            // 
            this.lblPhong.AutoSize = true;
            this.lblPhong.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPhong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblPhong.Location = new System.Drawing.Point(20, 118);
            this.lblPhong.Name = "lblPhong";
            this.lblPhong.Size = new System.Drawing.Size(83, 15);
            this.lblPhong.TabIndex = 10;
            this.lblPhong.Text = "Phòng chiếu *";
            // 
            // cboPhong
            // 
            this.cboPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPhong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboPhong.Location = new System.Drawing.Point(20, 138);
            this.cboPhong.Name = "cboPhong";
            this.cboPhong.Size = new System.Drawing.Size(300, 25);
            this.cboPhong.TabIndex = 11;
            // 
            // lblGiaVe
            // 
            this.lblGiaVe.AutoSize = true;
            this.lblGiaVe.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGiaVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblGiaVe.Location = new System.Drawing.Point(340, 118);
            this.lblGiaVe.Name = "lblGiaVe";
            this.lblGiaVe.Size = new System.Drawing.Size(127, 15);
            this.lblGiaVe.TabIndex = 12;
            this.lblGiaVe.Text = "Giá vé cơ bản (VNĐ) *";
            // 
            // txtGiaVe
            // 
            this.txtGiaVe.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtGiaVe.BorderRadius = 6;
            this.txtGiaVe.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGiaVe.DefaultText = "";
            this.txtGiaVe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtGiaVe.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtGiaVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtGiaVe.Location = new System.Drawing.Point(340, 138);
            this.txtGiaVe.MaxLength = 10;
            this.txtGiaVe.Name = "txtGiaVe";
            this.txtGiaVe.PasswordChar = '\0';
            this.txtGiaVe.PlaceholderText = "VD: 120000";
            this.txtGiaVe.SelectedText = "";
            this.txtGiaVe.Size = new System.Drawing.Size(180, 41);
            this.txtGiaVe.TabIndex = 13;
            // 
            // lblGioKetThucTitle
            // 
            this.lblGioKetThucTitle.AutoSize = true;
            this.lblGioKetThucTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGioKetThucTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblGioKetThucTitle.Location = new System.Drawing.Point(540, 118);
            this.lblGioKetThucTitle.Name = "lblGioKetThucTitle";
            this.lblGioKetThucTitle.Size = new System.Drawing.Size(161, 15);
            this.lblGioKetThucTitle.TabIndex = 14;
            this.lblGioKetThucTitle.Text = "Giờ kết thúc (tính tự động):";
            // 
            // lblGioKetThucValue
            // 
            this.lblGioKetThucValue.AutoSize = true;
            this.lblGioKetThucValue.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblGioKetThucValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.lblGioKetThucValue.Location = new System.Drawing.Point(538, 138);
            this.lblGioKetThucValue.Name = "lblGioKetThucValue";
            this.lblGioKetThucValue.Size = new System.Drawing.Size(33, 25);
            this.lblGioKetThucValue.TabIndex = 15;
            this.lblGioKetThucValue.Text = "---";
            // 
            // btnThem
            // 
            this.btnThem.BorderRadius = 7;
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(20, 192);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 38);
            this.btnThem.TabIndex = 16;
            this.btnThem.Text = "+ Thêm mới";
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.BorderRadius = 7;
            this.btnSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSua.Enabled = false;
            this.btnSua.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(150, 192);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 38);
            this.btnSua.TabIndex = 17;
            this.btnSua.Text = "✎ Cập nhật";
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BorderRadius = 7;
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.Enabled = false;
            this.btnXoa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(280, 192);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 38);
            this.btnXoa.TabIndex = 18;
            this.btnXoa.Text = "✕ Xóa";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BorderRadius = 7;
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(390, 192);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 38);
            this.btnLamMoi.TabIndex = 19;
            this.btnLamMoi.Text = "↺ Làm mới";
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // lblThongBao
            // 
            this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblThongBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblThongBao.Location = new System.Drawing.Point(520, 192);
            this.lblThongBao.Name = "lblThongBao";
            this.lblThongBao.Size = new System.Drawing.Size(480, 38);
            this.lblThongBao.TabIndex = 20;
            this.lblThongBao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlList.Controls.Add(this.dgvLichChieu);
            this.pnlList.Controls.Add(this.pnlSearchBar);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(0, 245);
            this.pnlList.Name = "pnlList";
            this.pnlList.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlList.Size = new System.Drawing.Size(1020, 415);
            this.pnlList.TabIndex = 0;
            // 
            // dgvLichChieu
            // 
            this.dgvLichChieu.AllowUserToAddRows = false;
            this.dgvLichChieu.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvLichChieu.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichChieu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvLichChieu.ColumnHeadersHeight = 38;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLichChieu.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvLichChieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLichChieu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvLichChieu.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvLichChieu.Location = new System.Drawing.Point(20, 64);
            this.dgvLichChieu.MultiSelect = false;
            this.dgvLichChieu.Name = "dgvLichChieu";
            this.dgvLichChieu.ReadOnly = true;
            this.dgvLichChieu.RowHeadersVisible = false;
            this.dgvLichChieu.Size = new System.Drawing.Size(980, 341);
            this.dgvLichChieu.TabIndex = 0;
            this.dgvLichChieu.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvLichChieu.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvLichChieu.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvLichChieu.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvLichChieu.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvLichChieu.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvLichChieu.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvLichChieu.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvLichChieu.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLichChieu.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvLichChieu.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLichChieu.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLichChieu.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvLichChieu.ThemeStyle.ReadOnly = true;
            this.dgvLichChieu.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLichChieu.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLichChieu.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvLichChieu.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvLichChieu.ThemeStyle.RowsStyle.Height = 22;
            this.dgvLichChieu.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.dgvLichChieu.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvLichChieu.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLichChieu_CellClick);
            // 
            // pnlSearchBar
            // 
            this.pnlSearchBar.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearchBar.Controls.Add(this.txtSearch);
            this.pnlSearchBar.Controls.Add(this.btnSearch);
            this.pnlSearchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchBar.Location = new System.Drawing.Point(20, 10);
            this.pnlSearchBar.Name = "pnlSearchBar";
            this.pnlSearchBar.Size = new System.Drawing.Size(980, 54);
            this.pnlSearchBar.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtSearch.BorderRadius = 7;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtSearch.Location = new System.Drawing.Point(0, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderText = "🔍  Tìm kiếm theo tên phim...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(380, 43);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.BorderRadius = 7;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(390, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 38);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmLichChieu
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1020, 660);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.txtLichChieuIdHidden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLichChieu";
            this.Text = "Quản lý Lịch Chiếu";
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichChieu)).EndInit();
            this.pnlSearchBar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}