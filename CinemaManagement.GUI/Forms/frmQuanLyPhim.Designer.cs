using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmQuanLyPhim.Designer.cs
//
// QUY TẮC NGHIÊM NGẶT (tránh sập Visual Studio Designer):
//   1. KHÔNG dùng lambda (=>) để gán sự kiện
//   2. KHÔNG viết hàm helper trong file này
//   3. KHÔNG dùng var, using var, hay C# 8+ syntax
//   4. Tất cả sự kiện phải dùng: new System.EventHandler(...)
//   5. Tất cả controls khai báo tường minh ở trên
//
// LAYOUT (nhúng vào pnlChildContainer của frmMain - DockStyle.Fill):
// ┌─────────────────────────────────────────────────────┐
// │  pnlInput (300px height) - Form nhập liệu           │
// │  ┌──────────────────────┬──────────────────────┐    │
// │  │ Cột TRÁI (thông tin) │ Cột PHẢI (mô tả)     │    │
// │  └──────────────────────┴──────────────────────┘    │
// │  [Nút Thêm] [Nút Sửa] [Nút Xóa] [Nút Làm mới]     │
// ├─────────────────────────────────────────────────────┤
// │  pnlList (Fill) - Danh sách phim                    │
// │  [Search TextBox]                                    │
// │  [DataGridView]                                      │
// └─────────────────────────────────────────────────────┘
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    partial class frmQuanLyPhim
    {
        private System.ComponentModel.IContainer components = null;

        // ─── PANEL NHẬP LIỆU ─────────────────────────────────
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlHeaderAccent;

        // Cột TRÁI
        private System.Windows.Forms.Label lblTenPhim;
        private Guna.UI2.WinForms.Guna2TextBox txtTenPhim;

        private System.Windows.Forms.Label lblDaoDien;
        private Guna.UI2.WinForms.Guna2TextBox txtDaoDien;

        private System.Windows.Forms.Label lblThoiLuong;
        private Guna.UI2.WinForms.Guna2TextBox txtThoiLuong;

        private System.Windows.Forms.Label lblNgayKhoiChieu;
        private System.Windows.Forms.DateTimePicker dtpNgayKhoiChieu;
        private System.Windows.Forms.CheckBox chkCoNgayKhoiChieu;

        private System.Windows.Forms.Label lblNuocSanXuat;
        private Guna.UI2.WinForms.Guna2TextBox txtNuocSanXuat;

        // Cột PHẢI
        private System.Windows.Forms.Label lblGioiHanDoTuoi;
        private System.Windows.Forms.ComboBox cboGioiHanDoTuoi;

        private System.Windows.Forms.Label lblNgonNgu;
        private System.Windows.Forms.ComboBox cboNgonNgu;

        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;

        private System.Windows.Forms.Label lblDienVien;
        private Guna.UI2.WinForms.Guna2TextBox txtDienVien;

        private System.Windows.Forms.Label lblMoTa;
        private Guna.UI2.WinForms.Guna2TextBox txtMoTa;

        // Nút hành động
        private Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;

        // Label thông báo
        private System.Windows.Forms.Label lblThongBao;

        // ─── PANEL DANH SÁCH ─────────────────────────────────
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.Panel pnlSearchBar;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvPhim;

        // Hidden field lưu ID đang chọn
        private System.Windows.Forms.TextBox txtPhimIdHidden;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtPhimIdHidden = new System.Windows.Forms.TextBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.pnlHeaderAccent = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblTenPhim = new System.Windows.Forms.Label();
            this.txtTenPhim = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblDaoDien = new System.Windows.Forms.Label();
            this.txtDaoDien = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblThoiLuong = new System.Windows.Forms.Label();
            this.txtThoiLuong = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNuocSanXuat = new System.Windows.Forms.Label();
            this.txtNuocSanXuat = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNgayKhoiChieu = new System.Windows.Forms.Label();
            this.chkCoNgayKhoiChieu = new System.Windows.Forms.CheckBox();
            this.dtpNgayKhoiChieu = new System.Windows.Forms.DateTimePicker();
            this.lblGioiHanDoTuoi = new System.Windows.Forms.Label();
            this.cboGioiHanDoTuoi = new System.Windows.Forms.ComboBox();
            this.lblNgonNgu = new System.Windows.Forms.Label();
            this.cboNgonNgu = new System.Windows.Forms.ComboBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblDienVien = new System.Windows.Forms.Label();
            this.txtDienVien = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.btnSua = new Guna.UI2.WinForms.Guna2Button();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.lblThongBao = new System.Windows.Forms.Label();
            this.pnlList = new System.Windows.Forms.Panel();
            this.dgvPhim = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlSearchBar = new System.Windows.Forms.Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.pnlInput.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhim)).BeginInit();
            this.pnlSearchBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPhimIdHidden
            // 
            this.txtPhimIdHidden.Location = new System.Drawing.Point(0, 0);
            this.txtPhimIdHidden.Name = "txtPhimIdHidden";
            this.txtPhimIdHidden.Size = new System.Drawing.Size(100, 20);
            this.txtPhimIdHidden.TabIndex = 2;
            this.txtPhimIdHidden.Text = "0";
            this.txtPhimIdHidden.Visible = false;
            // 
            // pnlInput
            // 
            this.pnlInput.BackColor = System.Drawing.Color.White;
            this.pnlInput.Controls.Add(this.pnlHeaderAccent);
            this.pnlInput.Controls.Add(this.lblHeader);
            this.pnlInput.Controls.Add(this.lblTenPhim);
            this.pnlInput.Controls.Add(this.txtTenPhim);
            this.pnlInput.Controls.Add(this.lblDaoDien);
            this.pnlInput.Controls.Add(this.txtDaoDien);
            this.pnlInput.Controls.Add(this.lblThoiLuong);
            this.pnlInput.Controls.Add(this.txtThoiLuong);
            this.pnlInput.Controls.Add(this.lblNuocSanXuat);
            this.pnlInput.Controls.Add(this.txtNuocSanXuat);
            this.pnlInput.Controls.Add(this.lblNgayKhoiChieu);
            this.pnlInput.Controls.Add(this.chkCoNgayKhoiChieu);
            this.pnlInput.Controls.Add(this.dtpNgayKhoiChieu);
            this.pnlInput.Controls.Add(this.lblGioiHanDoTuoi);
            this.pnlInput.Controls.Add(this.cboGioiHanDoTuoi);
            this.pnlInput.Controls.Add(this.lblNgonNgu);
            this.pnlInput.Controls.Add(this.cboNgonNgu);
            this.pnlInput.Controls.Add(this.lblTrangThai);
            this.pnlInput.Controls.Add(this.cboTrangThai);
            this.pnlInput.Controls.Add(this.lblDienVien);
            this.pnlInput.Controls.Add(this.txtDienVien);
            this.pnlInput.Controls.Add(this.lblMoTa);
            this.pnlInput.Controls.Add(this.txtMoTa);
            this.pnlInput.Controls.Add(this.btnThem);
            this.pnlInput.Controls.Add(this.btnSua);
            this.pnlInput.Controls.Add(this.btnXoa);
            this.pnlInput.Controls.Add(this.btnLamMoi);
            this.pnlInput.Controls.Add(this.lblThongBao);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Padding = new System.Windows.Forms.Padding(20, 14, 20, 10);
            this.pnlInput.Size = new System.Drawing.Size(1020, 310);
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
            this.lblHeader.Location = new System.Drawing.Point(16, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(117, 20);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Thông tin phim";
            // 
            // lblTenPhim
            // 
            this.lblTenPhim.AutoSize = true;
            this.lblTenPhim.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTenPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblTenPhim.Location = new System.Drawing.Point(21, 47);
            this.lblTenPhim.Name = "lblTenPhim";
            this.lblTenPhim.Size = new System.Drawing.Size(66, 15);
            this.lblTenPhim.TabIndex = 2;
            this.lblTenPhim.Text = "Tên phim *";
            // 
            // txtTenPhim
            // 
            this.txtTenPhim.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtTenPhim.BorderRadius = 6;
            this.txtTenPhim.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenPhim.DefaultText = "";
            this.txtTenPhim.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtTenPhim.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtTenPhim.Location = new System.Drawing.Point(20, 65);
            this.txtTenPhim.MaxLength = 200;
            this.txtTenPhim.Name = "txtTenPhim";
            this.txtTenPhim.PasswordChar = '\0';
            this.txtTenPhim.PlaceholderText = "Nhập tên phim...";
            this.txtTenPhim.SelectedText = "";
            this.txtTenPhim.Size = new System.Drawing.Size(440, 41);
            this.txtTenPhim.TabIndex = 3;
            // 
            // lblDaoDien
            // 
            this.lblDaoDien.AutoSize = true;
            this.lblDaoDien.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDaoDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblDaoDien.Location = new System.Drawing.Point(21, 118);
            this.lblDaoDien.Name = "lblDaoDien";
            this.lblDaoDien.Size = new System.Drawing.Size(56, 15);
            this.lblDaoDien.TabIndex = 4;
            this.lblDaoDien.Text = "Đạo diễn";
            // 
            // txtDaoDien
            // 
            this.txtDaoDien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtDaoDien.BorderRadius = 6;
            this.txtDaoDien.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDaoDien.DefaultText = "";
            this.txtDaoDien.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtDaoDien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDaoDien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtDaoDien.Location = new System.Drawing.Point(20, 136);
            this.txtDaoDien.Name = "txtDaoDien";
            this.txtDaoDien.PasswordChar = '\0';
            this.txtDaoDien.PlaceholderText = "Tên đạo diễn...";
            this.txtDaoDien.SelectedText = "";
            this.txtDaoDien.Size = new System.Drawing.Size(210, 41);
            this.txtDaoDien.TabIndex = 5;
            // 
            // lblThoiLuong
            // 
            this.lblThoiLuong.AutoSize = true;
            this.lblThoiLuong.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblThoiLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblThoiLuong.Location = new System.Drawing.Point(250, 118);
            this.lblThoiLuong.Name = "lblThoiLuong";
            this.lblThoiLuong.Size = new System.Drawing.Size(113, 15);
            this.lblThoiLuong.TabIndex = 6;
            this.lblThoiLuong.Text = "Thời lượng (phút) *";
            // 
            // txtThoiLuong
            // 
            this.txtThoiLuong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtThoiLuong.BorderRadius = 6;
            this.txtThoiLuong.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtThoiLuong.DefaultText = "";
            this.txtThoiLuong.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtThoiLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtThoiLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtThoiLuong.Location = new System.Drawing.Point(253, 136);
            this.txtThoiLuong.MaxLength = 4;
            this.txtThoiLuong.Name = "txtThoiLuong";
            this.txtThoiLuong.PasswordChar = '\0';
            this.txtThoiLuong.PlaceholderText = "VD: 120";
            this.txtThoiLuong.SelectedText = "";
            this.txtThoiLuong.Size = new System.Drawing.Size(210, 41);
            this.txtThoiLuong.TabIndex = 7;
            // 
            // lblNuocSanXuat
            // 
            this.lblNuocSanXuat.AutoSize = true;
            this.lblNuocSanXuat.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNuocSanXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblNuocSanXuat.Location = new System.Drawing.Point(21, 184);
            this.lblNuocSanXuat.Name = "lblNuocSanXuat";
            this.lblNuocSanXuat.Size = new System.Drawing.Size(87, 15);
            this.lblNuocSanXuat.TabIndex = 8;
            this.lblNuocSanXuat.Text = "Nước sản xuất";
            // 
            // txtNuocSanXuat
            // 
            this.txtNuocSanXuat.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtNuocSanXuat.BorderRadius = 6;
            this.txtNuocSanXuat.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNuocSanXuat.DefaultText = "";
            this.txtNuocSanXuat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtNuocSanXuat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNuocSanXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtNuocSanXuat.Location = new System.Drawing.Point(20, 202);
            this.txtNuocSanXuat.Name = "txtNuocSanXuat";
            this.txtNuocSanXuat.PasswordChar = '\0';
            this.txtNuocSanXuat.PlaceholderText = "VD: Mỹ, Việt Nam...";
            this.txtNuocSanXuat.SelectedText = "";
            this.txtNuocSanXuat.Size = new System.Drawing.Size(210, 41);
            this.txtNuocSanXuat.TabIndex = 9;
            // 
            // lblNgayKhoiChieu
            // 
            this.lblNgayKhoiChieu.AutoSize = true;
            this.lblNgayKhoiChieu.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNgayKhoiChieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblNgayKhoiChieu.Location = new System.Drawing.Point(250, 186);
            this.lblNgayKhoiChieu.Name = "lblNgayKhoiChieu";
            this.lblNgayKhoiChieu.Size = new System.Drawing.Size(96, 15);
            this.lblNgayKhoiChieu.TabIndex = 10;
            this.lblNgayKhoiChieu.Text = "Ngày khởi chiếu";
            // 
            // chkCoNgayKhoiChieu
            // 
            this.chkCoNgayKhoiChieu.AutoSize = true;
            this.chkCoNgayKhoiChieu.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkCoNgayKhoiChieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.chkCoNgayKhoiChieu.Location = new System.Drawing.Point(422, 182);
            this.chkCoNgayKhoiChieu.Name = "chkCoNgayKhoiChieu";
            this.chkCoNgayKhoiChieu.Size = new System.Drawing.Size(41, 19);
            this.chkCoNgayKhoiChieu.TabIndex = 11;
            this.chkCoNgayKhoiChieu.Text = "Có";
            this.chkCoNgayKhoiChieu.CheckedChanged += new System.EventHandler(this.chkCoNgayKhoiChieu_CheckedChanged);
            // 
            // dtpNgayKhoiChieu
            // 
            this.dtpNgayKhoiChieu.Enabled = false;
            this.dtpNgayKhoiChieu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayKhoiChieu.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayKhoiChieu.Location = new System.Drawing.Point(253, 202);
            this.dtpNgayKhoiChieu.Name = "dtpNgayKhoiChieu";
            this.dtpNgayKhoiChieu.Size = new System.Drawing.Size(210, 25);
            this.dtpNgayKhoiChieu.TabIndex = 12;
            // 
            // lblGioiHanDoTuoi
            // 
            this.lblGioiHanDoTuoi.AutoSize = true;
            this.lblGioiHanDoTuoi.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGioiHanDoTuoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblGioiHanDoTuoi.Location = new System.Drawing.Point(490, 41);
            this.lblGioiHanDoTuoi.Name = "lblGioiHanDoTuoi";
            this.lblGioiHanDoTuoi.Size = new System.Drawing.Size(104, 15);
            this.lblGioiHanDoTuoi.TabIndex = 13;
            this.lblGioiHanDoTuoi.Text = "Giới hạn độ tuổi *";
            // 
            // cboGioiHanDoTuoi
            // 
            this.cboGioiHanDoTuoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGioiHanDoTuoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboGioiHanDoTuoi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboGioiHanDoTuoi.Items.AddRange(new object[] {
            "P",
            "C13",
            "C16",
            "C18"});
            this.cboGioiHanDoTuoi.Location = new System.Drawing.Point(490, 61);
            this.cboGioiHanDoTuoi.Name = "cboGioiHanDoTuoi";
            this.cboGioiHanDoTuoi.Size = new System.Drawing.Size(150, 25);
            this.cboGioiHanDoTuoi.TabIndex = 14;
            // 
            // lblNgonNgu
            // 
            this.lblNgonNgu.AutoSize = true;
            this.lblNgonNgu.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNgonNgu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblNgonNgu.Location = new System.Drawing.Point(660, 41);
            this.lblNgonNgu.Name = "lblNgonNgu";
            this.lblNgonNgu.Size = new System.Drawing.Size(62, 15);
            this.lblNgonNgu.TabIndex = 15;
            this.lblNgonNgu.Text = "Ngôn ngữ";
            // 
            // cboNgonNgu
            // 
            this.cboNgonNgu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNgonNgu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboNgonNgu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboNgonNgu.Items.AddRange(new object[] {
            "VietSub",
            "ThuyetMinh",
            "Goc"});
            this.cboNgonNgu.Location = new System.Drawing.Point(660, 61);
            this.cboNgonNgu.Name = "cboNgonNgu";
            this.cboNgonNgu.Size = new System.Drawing.Size(150, 25);
            this.cboNgonNgu.TabIndex = 16;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblTrangThai.Location = new System.Drawing.Point(830, 41);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(70, 15);
            this.lblTrangThai.TabIndex = 17;
            this.lblTrangThai.Text = "Trạng thái *";
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTrangThai.Items.AddRange(new object[] {
            "SapChieu",
            "DangChieu",
            "NgungChieu"});
            this.cboTrangThai.Location = new System.Drawing.Point(830, 61);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(160, 25);
            this.cboTrangThai.TabIndex = 18;
            // 
            // lblDienVien
            // 
            this.lblDienVien.AutoSize = true;
            this.lblDienVien.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDienVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblDienVien.Location = new System.Drawing.Point(493, 89);
            this.lblDienVien.Name = "lblDienVien";
            this.lblDienVien.Size = new System.Drawing.Size(93, 15);
            this.lblDienVien.TabIndex = 19;
            this.lblDienVien.Text = "Diễn viên chính";
            // 
            // txtDienVien
            // 
            this.txtDienVien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtDienVien.BorderRadius = 6;
            this.txtDienVien.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDienVien.DefaultText = "";
            this.txtDienVien.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtDienVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDienVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtDienVien.Location = new System.Drawing.Point(493, 109);
            this.txtDienVien.Name = "txtDienVien";
            this.txtDienVien.PasswordChar = '\0';
            this.txtDienVien.PlaceholderText = "VD: Tom Hanks, Brad Pitt...";
            this.txtDienVien.SelectedText = "";
            this.txtDienVien.Size = new System.Drawing.Size(500, 41);
            this.txtDienVien.TabIndex = 20;
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblMoTa.Location = new System.Drawing.Point(493, 162);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(90, 15);
            this.lblMoTa.TabIndex = 21;
            this.lblMoTa.Text = "Mô tả nội dung";
            // 
            // txtMoTa
            // 
            this.txtMoTa.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtMoTa.BorderRadius = 6;
            this.txtMoTa.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMoTa.DefaultText = "";
            this.txtMoTa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtMoTa.Location = new System.Drawing.Point(493, 180);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.PasswordChar = '\0';
            this.txtMoTa.PlaceholderText = "Nhập nội dung / synopsis phim...";
            this.txtMoTa.SelectedText = "";
            this.txtMoTa.Size = new System.Drawing.Size(500, 77);
            this.txtMoTa.TabIndex = 22;
            // 
            // btnThem
            // 
            this.btnThem.BorderRadius = 7;
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(7, 256);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 38);
            this.btnThem.TabIndex = 23;
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
            this.btnSua.Location = new System.Drawing.Point(137, 256);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 38);
            this.btnSua.TabIndex = 24;
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
            this.btnXoa.Location = new System.Drawing.Point(267, 256);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 38);
            this.btnXoa.TabIndex = 25;
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
            this.btnLamMoi.Location = new System.Drawing.Point(377, 256);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 38);
            this.btnLamMoi.TabIndex = 26;
            this.btnLamMoi.Text = "↺ Làm mới";
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // lblThongBao
            // 
            this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblThongBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblThongBao.Location = new System.Drawing.Point(510, 270);
            this.lblThongBao.Name = "lblThongBao";
            this.lblThongBao.Size = new System.Drawing.Size(500, 24);
            this.lblThongBao.TabIndex = 27;
            this.lblThongBao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlList
            // 
            this.pnlList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlList.Controls.Add(this.dgvPhim);
            this.pnlList.Controls.Add(this.pnlSearchBar);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(0, 310);
            this.pnlList.Name = "pnlList";
            this.pnlList.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlList.Size = new System.Drawing.Size(1020, 350);
            this.pnlList.TabIndex = 0;
            // 
            // dgvPhim
            // 
            this.dgvPhim.AllowUserToAddRows = false;
            this.dgvPhim.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvPhim.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhim.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPhim.ColumnHeadersHeight = 38;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhim.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvPhim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhim.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvPhim.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvPhim.Location = new System.Drawing.Point(20, 64);
            this.dgvPhim.MultiSelect = false;
            this.dgvPhim.Name = "dgvPhim";
            this.dgvPhim.ReadOnly = true;
            this.dgvPhim.RowHeadersVisible = false;
            this.dgvPhim.Size = new System.Drawing.Size(980, 276);
            this.dgvPhim.TabIndex = 0;
            this.dgvPhim.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvPhim.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvPhim.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvPhim.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvPhim.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvPhim.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvPhim.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvPhim.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvPhim.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPhim.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvPhim.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvPhim.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPhim.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvPhim.ThemeStyle.ReadOnly = true;
            this.dgvPhim.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvPhim.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPhim.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvPhim.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvPhim.ThemeStyle.RowsStyle.Height = 22;
            this.dgvPhim.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.dgvPhim.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvPhim.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhim_CellClick);
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
            // frmQuanLyPhim
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1020, 660);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.txtPhimIdHidden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmQuanLyPhim";
            this.Text = "Quản lý Phim";
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhim)).EndInit();
            this.pnlSearchBar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}