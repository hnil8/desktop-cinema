using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmNhanVien.Designer.cs
//
// TUÂN THỦ NGHIÊM NGẶT:
//   ✓ KHÔNG dùng lambda (=>) gán sự kiện
//   ✓ KHÔNG dùng FocusedBorderColor (thuộc tính Guna mới)
//   ✓ KHÔNG dùng AdaptToRounding
//   ✓ Tất cả sự kiện: new System.EventHandler(this.MethodName)
//   ✓ AutoGenerateColumns = false TRƯỚC Columns.Clear()
//     (thực hiện trong ThietLapCotDgv ở frmNhanVien.cs)
//
// LAYOUT (DockStyle.Fill - nhúng vào pnlChildContainer frmMain):
// ┌─────────────────────────────────┬──────────────────────────┐
// │  pnlLeft (Fill)                 │  pnlRight (350px)        │
// │  ┌─── pnlForm (Top, 260px) ───┐ │  [Accent + Tiêu đề]      │
// │  │ [Tên][SĐT][Email][Giới    │ │  [lblTrangThaiCa]        │
// │  │  tính][Ngày vào làm]       │ │  (to, nổi bật)           │
// │  │ [Thêm][Sửa][Xóa][Làm mới]│ │                           │
// │  │ [lblThongBaoLeft]          │ │  [txtTienDauCa]          │
// │  └────────────────────────────┘ │  [btnMoCa] - Xanh lá    │
// │  ┌─── pnlSearch (Top, 54px) ─┐  │                          │
// │  │ [txtSearch] [btnSearch]   │  │  ── Phân cách ──          │
// │  └────────────────────────────┘  │                          │
// │  [dgvNhanVien - Fill]            │  [lblDoanhThu]           │
// │                                  │  [txtGhiChu]             │
// │                                  │  [btnDongCa] - Đỏ        │
// │                                  │  [lblThongBaoCa]         │
// └─────────────────────────────────┴──────────────────────────┘
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    partial class frmNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        // ─── LAYOUT PANELS ────────────────────────────────────
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;

        // ─── PANEL TRÁI - Form nhập liệu ──────────────────────
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Panel pnlFormAccent;
        private System.Windows.Forms.Label lblFormHeader;

        private System.Windows.Forms.Label lblHoTen;
        private Guna.UI2.WinForms.Guna2TextBox txtHoTen;

        private System.Windows.Forms.Label lblSoDienThoai;
        private Guna.UI2.WinForms.Guna2TextBox txtSoDienThoai;

        private System.Windows.Forms.Label lblEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;

        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.ComboBox cboGioiTinh;

        private System.Windows.Forms.Label lblNgayVaoLam;
        private System.Windows.Forms.DateTimePicker dtpNgayVaoLam;

        private Guna.UI2.WinForms.Guna2Button btnThem;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2Button btnLamMoi;
        private System.Windows.Forms.Label lblThongBaoLeft;

        // Hidden field lưu ID đang chọn
        private System.Windows.Forms.TextBox txtNhanVienIdHidden;

        // ─── PANEL TRÁI - Search + DataGridView ───────────────
        private System.Windows.Forms.Panel pnlSearch;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvNhanVien;

        // ─── PANEL PHẢI - Ca làm việc ─────────────────────────
        private System.Windows.Forms.Panel pnlCaAccent;
        private System.Windows.Forms.Label lblCaHeader;

        private System.Windows.Forms.Label lblTrangThaiCa;

        private System.Windows.Forms.Label lblTienDauCaTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtTienDauCa;

        private Guna.UI2.WinForms.Guna2Button btnMoCa;

        private System.Windows.Forms.Panel pnlDividerCa;

        private System.Windows.Forms.Label lblDoanhThuTitle;
        private System.Windows.Forms.Label lblDoanhThuTienMat;
        private System.Windows.Forms.Label lblDoanhThuCK;
        private System.Windows.Forms.Label lblDoanhThuThe;
        private System.Windows.Forms.Label lblDoanhThuTong;

        private System.Windows.Forms.Label lblGhiChuTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtGhiChu;

        private Guna.UI2.WinForms.Guna2Button btnDongCa;

        private System.Windows.Forms.Label lblThongBaoCa;

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
            this.txtNhanVienIdHidden = new System.Windows.Forms.TextBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlCaAccent = new System.Windows.Forms.Panel();
            this.lblCaHeader = new System.Windows.Forms.Label();
            this.lblTrangThaiCa = new System.Windows.Forms.Label();
            this.lblTienDauCaTitle = new System.Windows.Forms.Label();
            this.txtTienDauCa = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnMoCa = new Guna.UI2.WinForms.Guna2Button();
            this.pnlDividerCa = new System.Windows.Forms.Panel();
            this.lblDoanhThuTitle = new System.Windows.Forms.Label();
            this.lblDoanhThuTienMat = new System.Windows.Forms.Label();
            this.lblDoanhThuCK = new System.Windows.Forms.Label();
            this.lblDoanhThuThe = new System.Windows.Forms.Label();
            this.lblDoanhThuTong = new System.Windows.Forms.Label();
            this.lblGhiChuTitle = new System.Windows.Forms.Label();
            this.txtGhiChu = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnDongCa = new Guna.UI2.WinForms.Guna2Button();
            this.lblThongBaoCa = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dgvNhanVien = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.pnlFormAccent = new System.Windows.Forms.Panel();
            this.lblFormHeader = new System.Windows.Forms.Label();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSoDienThoai = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.cboGioiTinh = new System.Windows.Forms.ComboBox();
            this.lblNgayVaoLam = new System.Windows.Forms.Label();
            this.dtpNgayVaoLam = new System.Windows.Forms.DateTimePicker();
            this.btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.btnSua = new Guna.UI2.WinForms.Guna2Button();
            this.btnXoa = new Guna.UI2.WinForms.Guna2Button();
            this.btnLamMoi = new Guna.UI2.WinForms.Guna2Button();
            this.lblThongBaoLeft = new System.Windows.Forms.Label();
            this.pnlRight.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNhanVienIdHidden
            // 
            this.txtNhanVienIdHidden.Location = new System.Drawing.Point(0, 0);
            this.txtNhanVienIdHidden.Name = "txtNhanVienIdHidden";
            this.txtNhanVienIdHidden.Size = new System.Drawing.Size(100, 20);
            this.txtNhanVienIdHidden.TabIndex = 2;
            this.txtNhanVienIdHidden.Text = "0";
            this.txtNhanVienIdHidden.Visible = false;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.pnlCaAccent);
            this.pnlRight.Controls.Add(this.lblCaHeader);
            this.pnlRight.Controls.Add(this.lblTrangThaiCa);
            this.pnlRight.Controls.Add(this.lblTienDauCaTitle);
            this.pnlRight.Controls.Add(this.txtTienDauCa);
            this.pnlRight.Controls.Add(this.btnMoCa);
            this.pnlRight.Controls.Add(this.pnlDividerCa);
            this.pnlRight.Controls.Add(this.lblDoanhThuTitle);
            this.pnlRight.Controls.Add(this.lblDoanhThuTienMat);
            this.pnlRight.Controls.Add(this.lblDoanhThuCK);
            this.pnlRight.Controls.Add(this.lblDoanhThuThe);
            this.pnlRight.Controls.Add(this.lblDoanhThuTong);
            this.pnlRight.Controls.Add(this.lblGhiChuTitle);
            this.pnlRight.Controls.Add(this.txtGhiChu);
            this.pnlRight.Controls.Add(this.btnDongCa);
            this.pnlRight.Controls.Add(this.lblThongBaoCa);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(670, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.pnlRight.Size = new System.Drawing.Size(350, 660);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlCaAccent
            // 
            this.pnlCaAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.pnlCaAccent.Location = new System.Drawing.Point(20, 14);
            this.pnlCaAccent.Name = "pnlCaAccent";
            this.pnlCaAccent.Size = new System.Drawing.Size(40, 3);
            this.pnlCaAccent.TabIndex = 0;
            // 
            // lblCaHeader
            // 
            this.lblCaHeader.AutoSize = true;
            this.lblCaHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCaHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblCaHeader.Location = new System.Drawing.Point(20, 22);
            this.lblCaHeader.Name = "lblCaHeader";
            this.lblCaHeader.Size = new System.Drawing.Size(87, 20);
            this.lblCaHeader.TabIndex = 1;
            this.lblCaHeader.Text = "Ca làm việc";
            // 
            // lblTrangThaiCa
            // 
            this.lblTrangThaiCa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTrangThaiCa.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTrangThaiCa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.lblTrangThaiCa.Location = new System.Drawing.Point(20, 52);
            this.lblTrangThaiCa.Name = "lblTrangThaiCa";
            this.lblTrangThaiCa.Size = new System.Drawing.Size(306, 56);
            this.lblTrangThaiCa.TabIndex = 2;
            this.lblTrangThaiCa.Text = "Chưa mở ca làm việc";
            this.lblTrangThaiCa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTienDauCaTitle
            // 
            this.lblTienDauCaTitle.AutoSize = true;
            this.lblTienDauCaTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTienDauCaTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblTienDauCaTitle.Location = new System.Drawing.Point(20, 118);
            this.lblTienDauCaTitle.Name = "lblTienDauCaTitle";
            this.lblTienDauCaTitle.Size = new System.Drawing.Size(189, 15);
            this.lblTienDauCaTitle.TabIndex = 3;
            this.lblTienDauCaTitle.Text = "Tiền đầu ca nhận bàn giao (VNĐ):";
            // 
            // txtTienDauCa
            // 
            this.txtTienDauCa.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtTienDauCa.BorderRadius = 7;
            this.txtTienDauCa.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienDauCa.DefaultText = "";
            this.txtTienDauCa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtTienDauCa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTienDauCa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtTienDauCa.Location = new System.Drawing.Point(20, 136);
            this.txtTienDauCa.MaxLength = 10;
            this.txtTienDauCa.Name = "txtTienDauCa";
            this.txtTienDauCa.PasswordChar = '\0';
            this.txtTienDauCa.PlaceholderText = "VD: 500000";
            this.txtTienDauCa.SelectedText = "";
            this.txtTienDauCa.Size = new System.Drawing.Size(306, 41);
            this.txtTienDauCa.TabIndex = 4;
            // 
            // btnMoCa
            // 
            this.btnMoCa.BorderRadius = 8;
            this.btnMoCa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoCa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnMoCa.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnMoCa.ForeColor = System.Drawing.Color.White;
            this.btnMoCa.Location = new System.Drawing.Point(20, 182);
            this.btnMoCa.Name = "btnMoCa";
            this.btnMoCa.Size = new System.Drawing.Size(306, 48);
            this.btnMoCa.TabIndex = 5;
            this.btnMoCa.Text = "▶  MỞ CA LÀM VIỆC";
            this.btnMoCa.Click += new System.EventHandler(this.btnMoCa_Click);
            // 
            // pnlDividerCa
            // 
            this.pnlDividerCa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(235)))));
            this.pnlDividerCa.Location = new System.Drawing.Point(20, 244);
            this.pnlDividerCa.Name = "pnlDividerCa";
            this.pnlDividerCa.Size = new System.Drawing.Size(306, 1);
            this.pnlDividerCa.TabIndex = 6;
            // 
            // lblDoanhThuTitle
            // 
            this.lblDoanhThuTitle.AutoSize = true;
            this.lblDoanhThuTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDoanhThuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblDoanhThuTitle.Location = new System.Drawing.Point(20, 254);
            this.lblDoanhThuTitle.Name = "lblDoanhThuTitle";
            this.lblDoanhThuTitle.Size = new System.Drawing.Size(145, 15);
            this.lblDoanhThuTitle.TabIndex = 7;
            this.lblDoanhThuTitle.Text = "DOANH THU TRONG CA:";
            // 
            // lblDoanhThuTienMat
            // 
            this.lblDoanhThuTienMat.AutoSize = true;
            this.lblDoanhThuTienMat.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDoanhThuTienMat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblDoanhThuTienMat.Location = new System.Drawing.Point(20, 274);
            this.lblDoanhThuTienMat.Name = "lblDoanhThuTienMat";
            this.lblDoanhThuTienMat.Size = new System.Drawing.Size(84, 17);
            this.lblDoanhThuTienMat.TabIndex = 8;
            this.lblDoanhThuTienMat.Text = "Tiền mặt:  ---";
            // 
            // lblDoanhThuCK
            // 
            this.lblDoanhThuCK.AutoSize = true;
            this.lblDoanhThuCK.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDoanhThuCK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblDoanhThuCK.Location = new System.Drawing.Point(20, 296);
            this.lblDoanhThuCK.Name = "lblDoanhThuCK";
            this.lblDoanhThuCK.Size = new System.Drawing.Size(115, 17);
            this.lblDoanhThuCK.TabIndex = 9;
            this.lblDoanhThuCK.Text = "Chuyển khoản:  ---";
            // 
            // lblDoanhThuThe
            // 
            this.lblDoanhThuThe.AutoSize = true;
            this.lblDoanhThuThe.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDoanhThuThe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.lblDoanhThuThe.Location = new System.Drawing.Point(20, 318);
            this.lblDoanhThuThe.Name = "lblDoanhThuThe";
            this.lblDoanhThuThe.Size = new System.Drawing.Size(55, 17);
            this.lblDoanhThuThe.TabIndex = 10;
            this.lblDoanhThuThe.Text = "Thẻ:  ---";
            // 
            // lblDoanhThuTong
            // 
            this.lblDoanhThuTong.AutoSize = true;
            this.lblDoanhThuTong.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblDoanhThuTong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.lblDoanhThuTong.Location = new System.Drawing.Point(20, 342);
            this.lblDoanhThuTong.Name = "lblDoanhThuTong";
            this.lblDoanhThuTong.Size = new System.Drawing.Size(98, 25);
            this.lblDoanhThuTong.TabIndex = 11;
            this.lblDoanhThuTong.Text = "Tổng:  0 ₫";
            // 
            // lblGhiChuTitle
            // 
            this.lblGhiChuTitle.AutoSize = true;
            this.lblGhiChuTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGhiChuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblGhiChuTitle.Location = new System.Drawing.Point(20, 376);
            this.lblGhiChuTitle.Name = "lblGhiChuTitle";
            this.lblGhiChuTitle.Size = new System.Drawing.Size(154, 15);
            this.lblGhiChuTitle.TabIndex = 12;
            this.lblGhiChuTitle.Text = "Ghi chú chốt ca (tùy chọn):";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtGhiChu.BorderRadius = 7;
            this.txtGhiChu.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGhiChu.DefaultText = "";
            this.txtGhiChu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtGhiChu.Location = new System.Drawing.Point(20, 394);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.PasswordChar = '\0';
            this.txtGhiChu.PlaceholderText = "VD: Bàn giao cho ca tối...";
            this.txtGhiChu.SelectedText = "";
            this.txtGhiChu.Size = new System.Drawing.Size(306, 41);
            this.txtGhiChu.TabIndex = 13;
            // 
            // btnDongCa
            // 
            this.btnDongCa.BorderRadius = 8;
            this.btnDongCa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDongCa.Enabled = false;
            this.btnDongCa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.btnDongCa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDongCa.ForeColor = System.Drawing.Color.White;
            this.btnDongCa.Location = new System.Drawing.Point(20, 440);
            this.btnDongCa.Name = "btnDongCa";
            this.btnDongCa.Size = new System.Drawing.Size(306, 48);
            this.btnDongCa.TabIndex = 14;
            this.btnDongCa.Text = "■  ĐÓNG CA - CHỐT DOANH THU";
            this.btnDongCa.Click += new System.EventHandler(this.btnDongCa_Click);
            // 
            // lblThongBaoCa
            // 
            this.lblThongBaoCa.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblThongBaoCa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblThongBaoCa.Location = new System.Drawing.Point(20, 498);
            this.lblThongBaoCa.Name = "lblThongBaoCa";
            this.lblThongBaoCa.Size = new System.Drawing.Size(306, 72);
            this.lblThongBaoCa.TabIndex = 15;
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlLeft.Controls.Add(this.dgvNhanVien);
            this.pnlLeft.Controls.Add(this.pnlSearch);
            this.pnlLeft.Controls.Add(this.pnlForm);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(670, 660);
            this.pnlLeft.TabIndex = 0;
            // 
            // dgvNhanVien
            // 
            this.dgvNhanVien.AllowUserToAddRows = false;
            this.dgvNhanVien.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvNhanVien.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvNhanVien.ColumnHeadersHeight = 38;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNhanVien.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhanVien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvNhanVien.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvNhanVien.Location = new System.Drawing.Point(0, 354);
            this.dgvNhanVien.MultiSelect = false;
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.ReadOnly = true;
            this.dgvNhanVien.RowHeadersVisible = false;
            this.dgvNhanVien.Size = new System.Drawing.Size(670, 306);
            this.dgvNhanVien.TabIndex = 0;
            this.dgvNhanVien.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvNhanVien.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvNhanVien.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvNhanVien.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvNhanVien.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvNhanVien.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvNhanVien.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvNhanVien.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvNhanVien.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvNhanVien.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvNhanVien.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvNhanVien.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvNhanVien.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvNhanVien.ThemeStyle.ReadOnly = true;
            this.dgvNhanVien.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvNhanVien.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvNhanVien.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvNhanVien.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvNhanVien.ThemeStyle.RowsStyle.Height = 22;
            this.dgvNhanVien.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.dgvNhanVien.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvNhanVien.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhanVien_CellClick);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 300);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(20, 8, 20, 8);
            this.pnlSearch.Size = new System.Drawing.Size(670, 54);
            this.pnlSearch.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtSearch.BorderRadius = 7;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtSearch.Location = new System.Drawing.Point(20, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PasswordChar = '\0';
            this.txtSearch.PlaceholderText = "🔍  Tìm theo tên hoặc SĐT...";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(340, 43);
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
            this.btnSearch.Location = new System.Drawing.Point(370, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 38);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.White;
            this.pnlForm.Controls.Add(this.pnlFormAccent);
            this.pnlForm.Controls.Add(this.lblFormHeader);
            this.pnlForm.Controls.Add(this.lblHoTen);
            this.pnlForm.Controls.Add(this.txtHoTen);
            this.pnlForm.Controls.Add(this.lblSoDienThoai);
            this.pnlForm.Controls.Add(this.txtSoDienThoai);
            this.pnlForm.Controls.Add(this.lblEmail);
            this.pnlForm.Controls.Add(this.txtEmail);
            this.pnlForm.Controls.Add(this.lblGioiTinh);
            this.pnlForm.Controls.Add(this.cboGioiTinh);
            this.pnlForm.Controls.Add(this.lblNgayVaoLam);
            this.pnlForm.Controls.Add(this.dtpNgayVaoLam);
            this.pnlForm.Controls.Add(this.btnThem);
            this.pnlForm.Controls.Add(this.btnSua);
            this.pnlForm.Controls.Add(this.btnXoa);
            this.pnlForm.Controls.Add(this.btnLamMoi);
            this.pnlForm.Controls.Add(this.lblThongBaoLeft);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlForm.Location = new System.Drawing.Point(0, 0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(670, 300);
            this.pnlForm.TabIndex = 2;
            // 
            // pnlFormAccent
            // 
            this.pnlFormAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.pnlFormAccent.Location = new System.Drawing.Point(20, 14);
            this.pnlFormAccent.Name = "pnlFormAccent";
            this.pnlFormAccent.Size = new System.Drawing.Size(40, 3);
            this.pnlFormAccent.TabIndex = 0;
            // 
            // lblFormHeader
            // 
            this.lblFormHeader.AutoSize = true;
            this.lblFormHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblFormHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblFormHeader.Location = new System.Drawing.Point(20, 22);
            this.lblFormHeader.Name = "lblFormHeader";
            this.lblFormHeader.Size = new System.Drawing.Size(149, 20);
            this.lblFormHeader.TabIndex = 1;
            this.lblFormHeader.Text = "Thông tin nhân viên";
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblHoTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblHoTen.Location = new System.Drawing.Point(20, 52);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(69, 15);
            this.lblHoTen.TabIndex = 2;
            this.lblHoTen.Text = "Họ và tên *";
            // 
            // txtHoTen
            // 
            this.txtHoTen.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtHoTen.BorderRadius = 6;
            this.txtHoTen.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtHoTen.DefaultText = "";
            this.txtHoTen.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtHoTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtHoTen.Location = new System.Drawing.Point(20, 70);
            this.txtHoTen.MaxLength = 100;
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.PasswordChar = '\0';
            this.txtHoTen.PlaceholderText = "Nhập họ và tên...";
            this.txtHoTen.SelectedText = "";
            this.txtHoTen.Size = new System.Drawing.Size(280, 41);
            this.txtHoTen.TabIndex = 3;
            // 
            // lblSoDienThoai
            // 
            this.lblSoDienThoai.AutoSize = true;
            this.lblSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblSoDienThoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblSoDienThoai.Location = new System.Drawing.Point(320, 52);
            this.lblSoDienThoai.Name = "lblSoDienThoai";
            this.lblSoDienThoai.Size = new System.Drawing.Size(88, 15);
            this.lblSoDienThoai.TabIndex = 4;
            this.lblSoDienThoai.Text = "Số điện thoại *";
            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtSoDienThoai.BorderRadius = 6;
            this.txtSoDienThoai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSoDienThoai.DefaultText = "";
            this.txtSoDienThoai.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtSoDienThoai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSoDienThoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtSoDienThoai.Location = new System.Drawing.Point(320, 70);
            this.txtSoDienThoai.MaxLength = 15;
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.PasswordChar = '\0';
            this.txtSoDienThoai.PlaceholderText = "0912345678";
            this.txtSoDienThoai.SelectedText = "";
            this.txtSoDienThoai.Size = new System.Drawing.Size(180, 41);
            this.txtSoDienThoai.TabIndex = 5;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblEmail.Location = new System.Drawing.Point(20, 118);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(36, 15);
            this.lblEmail.TabIndex = 6;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtEmail.BorderRadius = 6;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtEmail.Location = new System.Drawing.Point(20, 136);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PasswordChar = '\0';
            this.txtEmail.PlaceholderText = "example@cinema.vn";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(280, 41);
            this.txtEmail.TabIndex = 7;
            // 
            // lblGioiTinh
            // 
            this.lblGioiTinh.AutoSize = true;
            this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblGioiTinh.Location = new System.Drawing.Point(320, 118);
            this.lblGioiTinh.Name = "lblGioiTinh";
            this.lblGioiTinh.Size = new System.Drawing.Size(55, 15);
            this.lblGioiTinh.TabIndex = 8;
            this.lblGioiTinh.Text = "Giới tính";
            // 
            // cboGioiTinh
            // 
            this.cboGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGioiTinh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboGioiTinh.Items.AddRange(new object[] {
            "Nam",
            "Nu",
            "Khac"});
            this.cboGioiTinh.Location = new System.Drawing.Point(320, 136);
            this.cboGioiTinh.Name = "cboGioiTinh";
            this.cboGioiTinh.Size = new System.Drawing.Size(180, 25);
            this.cboGioiTinh.TabIndex = 9;
            // 
            // lblNgayVaoLam
            // 
            this.lblNgayVaoLam.AutoSize = true;
            this.lblNgayVaoLam.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNgayVaoLam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblNgayVaoLam.Location = new System.Drawing.Point(20, 184);
            this.lblNgayVaoLam.Name = "lblNgayVaoLam";
            this.lblNgayVaoLam.Size = new System.Drawing.Size(81, 15);
            this.lblNgayVaoLam.TabIndex = 10;
            this.lblNgayVaoLam.Text = "Ngày vào làm";
            // 
            // dtpNgayVaoLam
            // 
            this.dtpNgayVaoLam.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayVaoLam.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayVaoLam.Location = new System.Drawing.Point(20, 202);
            this.dtpNgayVaoLam.MaxDate = new System.DateTime(2026, 4, 2, 0, 0, 0, 0);
            this.dtpNgayVaoLam.Name = "dtpNgayVaoLam";
            this.dtpNgayVaoLam.Size = new System.Drawing.Size(180, 25);
            this.dtpNgayVaoLam.TabIndex = 11;
            this.dtpNgayVaoLam.Value = new System.DateTime(2026, 4, 2, 0, 0, 0, 0);
            // 
            // btnThem
            // 
            this.btnThem.BorderRadius = 7;
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(20, 232);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(106, 36);
            this.btnThem.TabIndex = 12;
            this.btnThem.Text = "+ Thêm";
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
            this.btnSua.Location = new System.Drawing.Point(134, 232);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(106, 36);
            this.btnSua.TabIndex = 13;
            this.btnSua.Text = "✎ Sửa";
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
            this.btnXoa.Location = new System.Drawing.Point(248, 232);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(94, 36);
            this.btnXoa.TabIndex = 14;
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
            this.btnLamMoi.Location = new System.Drawing.Point(350, 232);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(104, 36);
            this.btnLamMoi.TabIndex = 15;
            this.btnLamMoi.Text = "↺ Làm mới";
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // lblThongBaoLeft
            // 
            this.lblThongBaoLeft.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblThongBaoLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblThongBaoLeft.Location = new System.Drawing.Point(20, 276);
            this.lblThongBaoLeft.Name = "lblThongBaoLeft";
            this.lblThongBaoLeft.Size = new System.Drawing.Size(460, 22);
            this.lblThongBaoLeft.TabIndex = 16;
            this.lblThongBaoLeft.Visible = false;
            // 
            // frmNhanVien
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1020, 660);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.txtNhanVienIdHidden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmNhanVien";
            this.Text = "Quản lý Nhân Viên";
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}