using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmPOS.Designer.cs
//
// TUYỆT ĐỐI TUÂN THỦ:
//   ✓ KHÔNG có lambda (=>) để gán sự kiện
//   ✓ KHÔNG có hàm helper tạo control
//   ✓ KHÔNG có BSON/JSON hoặc C# 8+ syntax
//   ✓ KHÔNG dùng AdaptToRounding
//   ✓ KHÔNG khởi tạo ghế động ở đây
//
// CHỈ KHAI BÁO: Khung layout tĩnh 3 cột + các control cố định
// TOÀN BỘ logic vẽ ghế động → frmPOS.cs
//
// LAYOUT (DockStyle.Fill):
// ┌──────────────┬─────────────────────────────┬──────────────┐
// │  pnlLeft     │  pnlCenter                  │  pnlRight    │
// │  (250px)     │  (Fill)                     │  (280px)     │
// │              │                             │              │
// │ [Chọn phim]  │ [lblManHinh]                │ [Tóm tắt]    │
// │ [cboPhim]    │ [pnlLegend]                 │ [lstGhe]     │
// │              │ [pnlSoDoGhe - EMPTY]        │ [TongTien]   │
// │ [Suất chiếu] │  ↑ vẽ ghế động ở đây       │ [SĐT KH]     │
// │ [flpSuat     │   bằng code trong frmPOS.cs │ [PTTT]       │
// │  - EMPTY]    │                             │ [btnTT]      │
// └──────────────┴─────────────────────────────┴──────────────┘
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    partial class frmPOS
    {
        private System.ComponentModel.IContainer components = null;

        // ─── LAYOUT PANELS ───────────────────────────────────
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlRight;

        // ─── CỘT TRÁI: Chọn phim + Suất chiếu ───────────────
        private System.Windows.Forms.Label lblTieuDeLeft;
        private System.Windows.Forms.Panel pnlLeftAccent;
        private System.Windows.Forms.Label lblChonPhim;
        private System.Windows.Forms.ComboBox cboPhim;
        private System.Windows.Forms.Label lblChonSuat;
        // FlowLayoutPanel chứa các nút suất chiếu (sẽ populate động trong frmPOS.cs)
        private System.Windows.Forms.FlowLayoutPanel flpSuatChieu;
        private System.Windows.Forms.Label lblKhongCoSuat;

        // ─── CỘT GIỮA: Sơ đồ ghế ────────────────────────────
        // Label màn hình chiếu
        private System.Windows.Forms.Panel pnlManHinhWrapper;
        private System.Windows.Forms.Label lblManHinh;
        // Legend màu
        private System.Windows.Forms.Panel pnlLegend;
        private System.Windows.Forms.Panel pnlLegTrong;
        private System.Windows.Forms.Label lblLegTrong;
        private System.Windows.Forms.Panel pnlLegDangChon;
        private System.Windows.Forms.Label lblLegDangChon;
        private System.Windows.Forms.Panel pnlLegDaBan;
        private System.Windows.Forms.Label lblLegDaBan;
        private System.Windows.Forms.Panel pnlLegVip;
        private System.Windows.Forms.Label lblLegVip;
        // Panel chứa sơ đồ ghế (GHẾ ĐƯỢC VẼ ĐỘNG TỪ frmPOS.cs)
        private System.Windows.Forms.Panel pnlSoDoGhe;
        private System.Windows.Forms.Label lblChonSuatDeTrong;

        // ─── CỘT PHẢI: Tóm tắt & Thanh toán ─────────────────
        private System.Windows.Forms.Label lblTieuDeRight;
        private System.Windows.Forms.Panel pnlRightAccent;

        // Thông tin suất chiếu đang chọn
        private System.Windows.Forms.Label lblInfoPhim;
        private System.Windows.Forms.Label lblInfoPhimVal;
        private System.Windows.Forms.Label lblInfoPhong;
        private System.Windows.Forms.Label lblInfoPhongVal;
        private System.Windows.Forms.Label lblInfoGio;
        private System.Windows.Forms.Label lblInfoGioVal;

        // Danh sách ghế đang chọn
        private System.Windows.Forms.Label lblDanhSachGhe;
        private System.Windows.Forms.ListBox lstGheDangChon;

        // Tổng tiền
        private System.Windows.Forms.Label lblTongTienTitle;
        private System.Windows.Forms.Label lblTongTienVal;

        // Khách hàng thành viên
        private System.Windows.Forms.Label lblSDTKhachHang;
        private Guna.UI2.WinForms.Guna2TextBox txtSDTKhachHang;
        private Guna.UI2.WinForms.Guna2Button btnTimKhachHang;
        private System.Windows.Forms.Label lblThongTinKH;

        // Phương thức thanh toán
        private System.Windows.Forms.Label lblPTTT;
        private System.Windows.Forms.ComboBox cboPTTT;

        // Tiền khách đưa (chỉ hiện khi chọn Tiền mặt)
        private System.Windows.Forms.Label lblTienKhachDua;
        private Guna.UI2.WinForms.Guna2TextBox txtTienKhachDua;

        // Nút thanh toán
        private Guna.UI2.WinForms.Guna2Button btnThanhToan;
        private Guna.UI2.WinForms.Guna2Button btnHuyChon;

        // Thông báo
        private System.Windows.Forms.Label lblThongBao;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlRightAccent = new System.Windows.Forms.Panel();
            this.lblTieuDeRight = new System.Windows.Forms.Label();
            this.lblInfoPhim = new System.Windows.Forms.Label();
            this.lblInfoPhimVal = new System.Windows.Forms.Label();
            this.lblInfoPhong = new System.Windows.Forms.Label();
            this.lblInfoPhongVal = new System.Windows.Forms.Label();
            this.lblInfoGio = new System.Windows.Forms.Label();
            this.lblInfoGioVal = new System.Windows.Forms.Label();
            this.lblDanhSachGhe = new System.Windows.Forms.Label();
            this.lstGheDangChon = new System.Windows.Forms.ListBox();
            this.lblTongTienTitle = new System.Windows.Forms.Label();
            this.lblTongTienVal = new System.Windows.Forms.Label();
            this.lblSDTKhachHang = new System.Windows.Forms.Label();
            this.txtSDTKhachHang = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnTimKhachHang = new Guna.UI2.WinForms.Guna2Button();
            this.lblThongTinKH = new System.Windows.Forms.Label();
            this.lblPTTT = new System.Windows.Forms.Label();
            this.cboPTTT = new System.Windows.Forms.ComboBox();
            this.lblTienKhachDua = new System.Windows.Forms.Label();
            this.txtTienKhachDua = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnHuyChon = new Guna.UI2.WinForms.Guna2Button();
            this.btnThanhToan = new Guna.UI2.WinForms.Guna2Button();
            this.lblThongBao = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlLeftAccent = new System.Windows.Forms.Panel();
            this.lblTieuDeLeft = new System.Windows.Forms.Label();
            this.lblChonPhim = new System.Windows.Forms.Label();
            this.cboPhim = new System.Windows.Forms.ComboBox();
            this.lblChonSuat = new System.Windows.Forms.Label();
            this.flpSuatChieu = new System.Windows.Forms.FlowLayoutPanel();
            this.lblKhongCoSuat = new System.Windows.Forms.Label();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlSoDoGhe = new System.Windows.Forms.Panel();
            this.lblChonSuatDeTrong = new System.Windows.Forms.Label();
            this.pnlLegend = new System.Windows.Forms.Panel();
            this.pnlLegTrong = new System.Windows.Forms.Panel();
            this.lblLegTrong = new System.Windows.Forms.Label();
            this.pnlLegDangChon = new System.Windows.Forms.Panel();
            this.lblLegDangChon = new System.Windows.Forms.Label();
            this.pnlLegDaBan = new System.Windows.Forms.Panel();
            this.lblLegDaBan = new System.Windows.Forms.Label();
            this.pnlLegVip = new System.Windows.Forms.Panel();
            this.lblLegVip = new System.Windows.Forms.Label();
            this.pnlManHinhWrapper = new System.Windows.Forms.Panel();
            this.lblManHinh = new System.Windows.Forms.Label();
            this.pnlRight.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.pnlSoDoGhe.SuspendLayout();
            this.pnlLegend.SuspendLayout();
            this.pnlManHinhWrapper.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.pnlRightAccent);
            this.pnlRight.Controls.Add(this.lblTieuDeRight);
            this.pnlRight.Controls.Add(this.lblInfoPhim);
            this.pnlRight.Controls.Add(this.lblInfoPhimVal);
            this.pnlRight.Controls.Add(this.lblInfoPhong);
            this.pnlRight.Controls.Add(this.lblInfoPhongVal);
            this.pnlRight.Controls.Add(this.lblInfoGio);
            this.pnlRight.Controls.Add(this.lblInfoGioVal);
            this.pnlRight.Controls.Add(this.lblDanhSachGhe);
            this.pnlRight.Controls.Add(this.lstGheDangChon);
            this.pnlRight.Controls.Add(this.lblTongTienTitle);
            this.pnlRight.Controls.Add(this.lblTongTienVal);
            this.pnlRight.Controls.Add(this.lblSDTKhachHang);
            this.pnlRight.Controls.Add(this.txtSDTKhachHang);
            this.pnlRight.Controls.Add(this.btnTimKhachHang);
            this.pnlRight.Controls.Add(this.lblThongTinKH);
            this.pnlRight.Controls.Add(this.lblPTTT);
            this.pnlRight.Controls.Add(this.cboPTTT);
            this.pnlRight.Controls.Add(this.lblTienKhachDua);
            this.pnlRight.Controls.Add(this.txtTienKhachDua);
            this.pnlRight.Controls.Add(this.btnHuyChon);
            this.pnlRight.Controls.Add(this.btnThanhToan);
            this.pnlRight.Controls.Add(this.lblThongBao);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(740, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(14);
            this.pnlRight.Size = new System.Drawing.Size(280, 660);
            this.pnlRight.TabIndex = 2;
            // 
            // pnlRightAccent
            // 
            this.pnlRightAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.pnlRightAccent.Location = new System.Drawing.Point(14, 14);
            this.pnlRightAccent.Name = "pnlRightAccent";
            this.pnlRightAccent.Size = new System.Drawing.Size(40, 3);
            this.pnlRightAccent.TabIndex = 0;
            // 
            // lblTieuDeRight
            // 
            this.lblTieuDeRight.AutoSize = true;
            this.lblTieuDeRight.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTieuDeRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblTieuDeRight.Location = new System.Drawing.Point(14, 22);
            this.lblTieuDeRight.Name = "lblTieuDeRight";
            this.lblTieuDeRight.Size = new System.Drawing.Size(123, 19);
            this.lblTieuDeRight.TabIndex = 1;
            this.lblTieuDeRight.Text = "Chi tiết đơn hàng";
            // 
            // lblInfoPhim
            // 
            this.lblInfoPhim.AutoSize = true;
            this.lblInfoPhim.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblInfoPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblInfoPhim.Location = new System.Drawing.Point(14, 52);
            this.lblInfoPhim.Name = "lblInfoPhim";
            this.lblInfoPhim.Size = new System.Drawing.Size(38, 15);
            this.lblInfoPhim.TabIndex = 2;
            this.lblInfoPhim.Text = "Phim:";
            // 
            // lblInfoPhimVal
            // 
            this.lblInfoPhimVal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInfoPhimVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblInfoPhimVal.Location = new System.Drawing.Point(14, 68);
            this.lblInfoPhimVal.Name = "lblInfoPhimVal";
            this.lblInfoPhimVal.Size = new System.Drawing.Size(248, 36);
            this.lblInfoPhimVal.TabIndex = 3;
            this.lblInfoPhimVal.Text = "---";
            // 
            // lblInfoPhong
            // 
            this.lblInfoPhong.AutoSize = true;
            this.lblInfoPhong.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblInfoPhong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblInfoPhong.Location = new System.Drawing.Point(14, 104);
            this.lblInfoPhong.Name = "lblInfoPhong";
            this.lblInfoPhong.Size = new System.Drawing.Size(45, 15);
            this.lblInfoPhong.TabIndex = 4;
            this.lblInfoPhong.Text = "Phòng:";
            // 
            // lblInfoPhongVal
            // 
            this.lblInfoPhongVal.AutoSize = true;
            this.lblInfoPhongVal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInfoPhongVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblInfoPhongVal.Location = new System.Drawing.Point(80, 104);
            this.lblInfoPhongVal.Name = "lblInfoPhongVal";
            this.lblInfoPhongVal.Size = new System.Drawing.Size(23, 17);
            this.lblInfoPhongVal.TabIndex = 5;
            this.lblInfoPhongVal.Text = "---";
            // 
            // lblInfoGio
            // 
            this.lblInfoGio.AutoSize = true;
            this.lblInfoGio.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblInfoGio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblInfoGio.Location = new System.Drawing.Point(14, 130);
            this.lblInfoGio.Name = "lblInfoGio";
            this.lblInfoGio.Size = new System.Drawing.Size(63, 15);
            this.lblInfoGio.TabIndex = 6;
            this.lblInfoGio.Text = "Giờ chiếu:";
            // 
            // lblInfoGioVal
            // 
            this.lblInfoGioVal.AutoSize = true;
            this.lblInfoGioVal.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblInfoGioVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.lblInfoGioVal.Location = new System.Drawing.Point(86, 130);
            this.lblInfoGioVal.Name = "lblInfoGioVal";
            this.lblInfoGioVal.Size = new System.Drawing.Size(23, 17);
            this.lblInfoGioVal.TabIndex = 7;
            this.lblInfoGioVal.Text = "---";
            // 
            // lblDanhSachGhe
            // 
            this.lblDanhSachGhe.AutoSize = true;
            this.lblDanhSachGhe.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDanhSachGhe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblDanhSachGhe.Location = new System.Drawing.Point(14, 158);
            this.lblDanhSachGhe.Name = "lblDanhSachGhe";
            this.lblDanhSachGhe.Size = new System.Drawing.Size(94, 15);
            this.lblDanhSachGhe.TabIndex = 8;
            this.lblDanhSachGhe.Text = "Ghế đang chọn:";
            // 
            // lstGheDangChon
            // 
            this.lstGheDangChon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.lstGheDangChon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstGheDangChon.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstGheDangChon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lstGheDangChon.ItemHeight = 17;
            this.lstGheDangChon.Location = new System.Drawing.Point(14, 176);
            this.lstGheDangChon.Name = "lstGheDangChon";
            this.lstGheDangChon.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstGheDangChon.Size = new System.Drawing.Size(248, 70);
            this.lstGheDangChon.TabIndex = 9;
            // 
            // lblTongTienTitle
            // 
            this.lblTongTienTitle.AutoSize = true;
            this.lblTongTienTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTongTienTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblTongTienTitle.Location = new System.Drawing.Point(14, 264);
            this.lblTongTienTitle.Name = "lblTongTienTitle";
            this.lblTongTienTitle.Size = new System.Drawing.Size(82, 17);
            this.lblTongTienTitle.TabIndex = 10;
            this.lblTongTienTitle.Text = "TỔNG TIỀN:";
            // 
            // lblTongTienVal
            // 
            this.lblTongTienVal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTongTienVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.lblTongTienVal.Location = new System.Drawing.Point(14, 282);
            this.lblTongTienVal.Name = "lblTongTienVal";
            this.lblTongTienVal.Size = new System.Drawing.Size(248, 36);
            this.lblTongTienVal.TabIndex = 11;
            this.lblTongTienVal.Text = "0 ₫";
            this.lblTongTienVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSDTKhachHang
            // 
            this.lblSDTKhachHang.AutoSize = true;
            this.lblSDTKhachHang.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblSDTKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblSDTKhachHang.Location = new System.Drawing.Point(14, 326);
            this.lblSDTKhachHang.Name = "lblSDTKhachHang";
            this.lblSDTKhachHang.Size = new System.Drawing.Size(154, 15);
            this.lblSDTKhachHang.TabIndex = 12;
            this.lblSDTKhachHang.Text = "SĐT thành viên (tùy chọn):";
            // 
            // txtSDTKhachHang
            // 
            this.txtSDTKhachHang.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtSDTKhachHang.BorderRadius = 6;
            this.txtSDTKhachHang.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSDTKhachHang.DefaultText = "";
            this.txtSDTKhachHang.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtSDTKhachHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSDTKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtSDTKhachHang.Location = new System.Drawing.Point(14, 344);
            this.txtSDTKhachHang.MaxLength = 15;
            this.txtSDTKhachHang.Name = "txtSDTKhachHang";
            this.txtSDTKhachHang.PasswordChar = '\0';
            this.txtSDTKhachHang.PlaceholderText = "Nhập SĐT...";
            this.txtSDTKhachHang.SelectedText = "";
            this.txtSDTKhachHang.Size = new System.Drawing.Size(158, 39);
            this.txtSDTKhachHang.TabIndex = 13;
            // 
            // btnTimKhachHang
            // 
            this.btnTimKhachHang.BorderRadius = 6;
            this.btnTimKhachHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKhachHang.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnTimKhachHang.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTimKhachHang.ForeColor = System.Drawing.Color.White;
            this.btnTimKhachHang.Location = new System.Drawing.Point(181, 345);
            this.btnTimKhachHang.Name = "btnTimKhachHang";
            this.btnTimKhachHang.Size = new System.Drawing.Size(82, 34);
            this.btnTimKhachHang.TabIndex = 14;
            this.btnTimKhachHang.Text = "Tìm";
            this.btnTimKhachHang.Click += new System.EventHandler(this.btnTimKhachHang_Click);
            // 
            // lblThongTinKH
            // 
            this.lblThongTinKH.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblThongTinKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblThongTinKH.Location = new System.Drawing.Point(14, 382);
            this.lblThongTinKH.Name = "lblThongTinKH";
            this.lblThongTinKH.Size = new System.Drawing.Size(248, 20);
            this.lblThongTinKH.TabIndex = 15;
            // 
            // lblPTTT
            // 
            this.lblPTTT.AutoSize = true;
            this.lblPTTT.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPTTT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblPTTT.Location = new System.Drawing.Point(14, 406);
            this.lblPTTT.Name = "lblPTTT";
            this.lblPTTT.Size = new System.Drawing.Size(146, 15);
            this.lblPTTT.TabIndex = 16;
            this.lblPTTT.Text = "Phương thức thanh toán:";
            // 
            // cboPTTT
            // 
            this.cboPTTT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPTTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPTTT.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboPTTT.Items.AddRange(new object[] {
            "TienMat",
            "ChuyenKhoan",
            "The"});
            this.cboPTTT.Location = new System.Drawing.Point(14, 424);
            this.cboPTTT.Name = "cboPTTT";
            this.cboPTTT.Size = new System.Drawing.Size(248, 25);
            this.cboPTTT.TabIndex = 17;
            this.cboPTTT.SelectedIndexChanged += new System.EventHandler(this.cboPTTT_SelectedIndexChanged);
            // 
            // lblTienKhachDua
            // 
            this.lblTienKhachDua.AutoSize = true;
            this.lblTienKhachDua.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTienKhachDua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblTienKhachDua.Location = new System.Drawing.Point(14, 460);
            this.lblTienKhachDua.Name = "lblTienKhachDua";
            this.lblTienKhachDua.Size = new System.Drawing.Size(132, 15);
            this.lblTienKhachDua.TabIndex = 18;
            this.lblTienKhachDua.Text = "Tiền khách đưa (VNĐ):";
            // 
            // txtTienKhachDua
            // 
            this.txtTienKhachDua.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(230)))));
            this.txtTienKhachDua.BorderRadius = 6;
            this.txtTienKhachDua.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienKhachDua.DefaultText = "";
            this.txtTienKhachDua.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.txtTienKhachDua.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTienKhachDua.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.txtTienKhachDua.Location = new System.Drawing.Point(14, 478);
            this.txtTienKhachDua.MaxLength = 12;
            this.txtTienKhachDua.Name = "txtTienKhachDua";
            this.txtTienKhachDua.PasswordChar = '\0';
            this.txtTienKhachDua.PlaceholderText = "VD: 300000";
            this.txtTienKhachDua.SelectedText = "";
            this.txtTienKhachDua.Size = new System.Drawing.Size(248, 39);
            this.txtTienKhachDua.TabIndex = 19;
            // 
            // btnHuyChon
            // 
            this.btnHuyChon.BorderRadius = 8;
            this.btnHuyChon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuyChon.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnHuyChon.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuyChon.ForeColor = System.Drawing.Color.White;
            this.btnHuyChon.Location = new System.Drawing.Point(14, 525);
            this.btnHuyChon.Name = "btnHuyChon";
            this.btnHuyChon.Size = new System.Drawing.Size(116, 40);
            this.btnHuyChon.TabIndex = 20;
            this.btnHuyChon.Text = "↺ Chọn lại";
            this.btnHuyChon.Click += new System.EventHandler(this.btnHuyChon_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BorderRadius = 8;
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThanhToan.Enabled = false;
            this.btnThanhToan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(138, 525);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(124, 40);
            this.btnThanhToan.TabIndex = 21;
            this.btnThanhToan.Text = "THANH TOÁN";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // lblThongBao
            // 
            this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblThongBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblThongBao.Location = new System.Drawing.Point(14, 568);
            this.lblThongBao.Name = "lblThongBao";
            this.lblThongBao.Size = new System.Drawing.Size(248, 52);
            this.lblThongBao.TabIndex = 22;
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.pnlLeft.Controls.Add(this.pnlLeftAccent);
            this.pnlLeft.Controls.Add(this.lblTieuDeLeft);
            this.pnlLeft.Controls.Add(this.lblChonPhim);
            this.pnlLeft.Controls.Add(this.cboPhim);
            this.pnlLeft.Controls.Add(this.lblChonSuat);
            this.pnlLeft.Controls.Add(this.flpSuatChieu);
            this.pnlLeft.Controls.Add(this.lblKhongCoSuat);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(14);
            this.pnlLeft.Size = new System.Drawing.Size(250, 660);
            this.pnlLeft.TabIndex = 1;
            // 
            // pnlLeftAccent
            // 
            this.pnlLeftAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.pnlLeftAccent.Location = new System.Drawing.Point(14, 14);
            this.pnlLeftAccent.Name = "pnlLeftAccent";
            this.pnlLeftAccent.Size = new System.Drawing.Size(40, 3);
            this.pnlLeftAccent.TabIndex = 0;
            // 
            // lblTieuDeLeft
            // 
            this.lblTieuDeLeft.AutoSize = true;
            this.lblTieuDeLeft.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTieuDeLeft.ForeColor = System.Drawing.Color.White;
            this.lblTieuDeLeft.Location = new System.Drawing.Point(14, 22);
            this.lblTieuDeLeft.Name = "lblTieuDeLeft";
            this.lblTieuDeLeft.Size = new System.Drawing.Size(85, 20);
            this.lblTieuDeLeft.TabIndex = 1;
            this.lblTieuDeLeft.Text = "Chọn phim";
            // 
            // lblChonPhim
            // 
            this.lblChonPhim.AutoSize = true;
            this.lblChonPhim.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblChonPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(176)))));
            this.lblChonPhim.Location = new System.Drawing.Point(14, 52);
            this.lblChonPhim.Name = "lblChonPhim";
            this.lblChonPhim.Size = new System.Drawing.Size(148, 12);
            this.lblChonPhim.TabIndex = 2;
            this.lblChonPhim.Text = "PHIM ĐANG CHIẾU HÔM NAY";
            // 
            // cboPhim
            // 
            this.cboPhim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(66)))));
            this.cboPhim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhim.DropDownWidth = 300;
            this.cboPhim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPhim.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboPhim.ForeColor = System.Drawing.Color.White;
            this.cboPhim.Location = new System.Drawing.Point(14, 70);
            this.cboPhim.Name = "cboPhim";
            this.cboPhim.Size = new System.Drawing.Size(218, 25);
            this.cboPhim.TabIndex = 3;
            this.cboPhim.SelectedIndexChanged += new System.EventHandler(this.cboPhim_SelectedIndexChanged);
            // 
            // lblChonSuat
            // 
            this.lblChonSuat.AutoSize = true;
            this.lblChonSuat.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblChonSuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(176)))));
            this.lblChonSuat.Location = new System.Drawing.Point(14, 116);
            this.lblChonSuat.Name = "lblChonSuat";
            this.lblChonSuat.Size = new System.Drawing.Size(63, 12);
            this.lblChonSuat.TabIndex = 4;
            this.lblChonSuat.Text = "SUẤT CHIẾU";
            // 
            // flpSuatChieu
            // 
            this.flpSuatChieu.AutoScroll = true;
            this.flpSuatChieu.BackColor = System.Drawing.Color.Transparent;
            this.flpSuatChieu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSuatChieu.Location = new System.Drawing.Point(14, 136);
            this.flpSuatChieu.Name = "flpSuatChieu";
            this.flpSuatChieu.Size = new System.Drawing.Size(218, 400);
            this.flpSuatChieu.TabIndex = 5;
            this.flpSuatChieu.WrapContents = false;
            // 
            // lblKhongCoSuat
            // 
            this.lblKhongCoSuat.AutoSize = true;
            this.lblKhongCoSuat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKhongCoSuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblKhongCoSuat.Location = new System.Drawing.Point(14, 156);
            this.lblKhongCoSuat.Name = "lblKhongCoSuat";
            this.lblKhongCoSuat.Size = new System.Drawing.Size(108, 30);
            this.lblKhongCoSuat.TabIndex = 6;
            this.lblKhongCoSuat.Text = "Chọn phim để xem\ncác suất chiếu";
            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlCenter.Controls.Add(this.pnlSoDoGhe);
            this.pnlCenter.Controls.Add(this.pnlLegend);
            this.pnlCenter.Controls.Add(this.pnlManHinhWrapper);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(250, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.pnlCenter.Size = new System.Drawing.Size(490, 660);
            this.pnlCenter.TabIndex = 0;
            // 
            // pnlSoDoGhe
            // 
            this.pnlSoDoGhe.AutoScroll = true;
            this.pnlSoDoGhe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlSoDoGhe.Controls.Add(this.lblChonSuatDeTrong);
            this.pnlSoDoGhe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSoDoGhe.Location = new System.Drawing.Point(16, 82);
            this.pnlSoDoGhe.Name = "pnlSoDoGhe";
            this.pnlSoDoGhe.Size = new System.Drawing.Size(458, 568);
            this.pnlSoDoGhe.TabIndex = 0;
            // 
            // lblChonSuatDeTrong
            // 
            this.lblChonSuatDeTrong.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblChonSuatDeTrong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.lblChonSuatDeTrong.Location = new System.Drawing.Point(36, 200);
            this.lblChonSuatDeTrong.Name = "lblChonSuatDeTrong";
            this.lblChonSuatDeTrong.Size = new System.Drawing.Size(400, 80);
            this.lblChonSuatDeTrong.TabIndex = 0;
            this.lblChonSuatDeTrong.Text = "👈  Chọn suất chiếu bên trái\nđể hiển thị sơ đồ ghế";
            this.lblChonSuatDeTrong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLegend
            // 
            this.pnlLegend.BackColor = System.Drawing.Color.Transparent;
            this.pnlLegend.Controls.Add(this.pnlLegTrong);
            this.pnlLegend.Controls.Add(this.lblLegTrong);
            this.pnlLegend.Controls.Add(this.pnlLegDangChon);
            this.pnlLegend.Controls.Add(this.lblLegDangChon);
            this.pnlLegend.Controls.Add(this.pnlLegDaBan);
            this.pnlLegend.Controls.Add(this.lblLegDaBan);
            this.pnlLegend.Controls.Add(this.pnlLegVip);
            this.pnlLegend.Controls.Add(this.lblLegVip);
            this.pnlLegend.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLegend.Location = new System.Drawing.Point(16, 46);
            this.pnlLegend.Name = "pnlLegend";
            this.pnlLegend.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlLegend.Size = new System.Drawing.Size(458, 36);
            this.pnlLegend.TabIndex = 1;
            // 
            // pnlLegTrong
            // 
            this.pnlLegTrong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pnlLegTrong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLegTrong.Location = new System.Drawing.Point(15, 9);
            this.pnlLegTrong.Name = "pnlLegTrong";
            this.pnlLegTrong.Size = new System.Drawing.Size(18, 18);
            this.pnlLegTrong.TabIndex = 0;
            // 
            // lblLegTrong
            // 
            this.lblLegTrong.AutoSize = true;
            this.lblLegTrong.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLegTrong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblLegTrong.Location = new System.Drawing.Point(37, 9);
            this.lblLegTrong.Name = "lblLegTrong";
            this.lblLegTrong.Size = new System.Drawing.Size(38, 15);
            this.lblLegTrong.TabIndex = 1;
            this.lblLegTrong.Text = "Trống";
            // 
            // pnlLegDangChon
            // 
            this.pnlLegDangChon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.pnlLegDangChon.Location = new System.Drawing.Point(95, 9);
            this.pnlLegDangChon.Name = "pnlLegDangChon";
            this.pnlLegDangChon.Size = new System.Drawing.Size(18, 18);
            this.pnlLegDangChon.TabIndex = 2;
            // 
            // lblLegDangChon
            // 
            this.lblLegDangChon.AutoSize = true;
            this.lblLegDangChon.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLegDangChon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblLegDangChon.Location = new System.Drawing.Point(117, 9);
            this.lblLegDangChon.Name = "lblLegDangChon";
            this.lblLegDangChon.Size = new System.Drawing.Size(65, 15);
            this.lblLegDangChon.TabIndex = 3;
            this.lblLegDangChon.Text = "Đang chọn";
            // 
            // pnlLegDaBan
            // 
            this.pnlLegDaBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.pnlLegDaBan.Location = new System.Drawing.Point(209, 9);
            this.pnlLegDaBan.Name = "pnlLegDaBan";
            this.pnlLegDaBan.Size = new System.Drawing.Size(18, 18);
            this.pnlLegDaBan.TabIndex = 4;
            // 
            // lblLegDaBan
            // 
            this.lblLegDaBan.AutoSize = true;
            this.lblLegDaBan.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLegDaBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblLegDaBan.Location = new System.Drawing.Point(231, 9);
            this.lblLegDaBan.Name = "lblLegDaBan";
            this.lblLegDaBan.Size = new System.Drawing.Size(44, 15);
            this.lblLegDaBan.TabIndex = 5;
            this.lblLegDaBan.Text = "Đã bán";
            // 
            // pnlLegVip
            // 
            this.pnlLegVip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.pnlLegVip.Location = new System.Drawing.Point(299, 9);
            this.pnlLegVip.Name = "pnlLegVip";
            this.pnlLegVip.Size = new System.Drawing.Size(18, 18);
            this.pnlLegVip.TabIndex = 6;
            // 
            // lblLegVip
            // 
            this.lblLegVip.AutoSize = true;
            this.lblLegVip.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblLegVip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(110)))));
            this.lblLegVip.Location = new System.Drawing.Point(321, 9);
            this.lblLegVip.Name = "lblLegVip";
            this.lblLegVip.Size = new System.Drawing.Size(24, 15);
            this.lblLegVip.TabIndex = 7;
            this.lblLegVip.Text = "VIP";
            // 
            // pnlManHinhWrapper
            // 
            this.pnlManHinhWrapper.BackColor = System.Drawing.Color.Transparent;
            this.pnlManHinhWrapper.Controls.Add(this.lblManHinh);
            this.pnlManHinhWrapper.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlManHinhWrapper.Location = new System.Drawing.Point(16, 10);
            this.pnlManHinhWrapper.Name = "pnlManHinhWrapper";
            this.pnlManHinhWrapper.Size = new System.Drawing.Size(458, 36);
            this.pnlManHinhWrapper.TabIndex = 2;
            // 
            // lblManHinh
            // 
            this.lblManHinh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblManHinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblManHinh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblManHinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(130)))));
            this.lblManHinh.Location = new System.Drawing.Point(0, 0);
            this.lblManHinh.Name = "lblManHinh";
            this.lblManHinh.Size = new System.Drawing.Size(458, 36);
            this.lblManHinh.TabIndex = 0;
            this.lblManHinh.Text = "▬▬▬▬▬▬▬  MÀN HÌNH  ▬▬▬▬▬▬▬";
            this.lblManHinh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmPOS
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1020, 660);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPOS";
            this.Text = "Bán Vé - POS";
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
            this.pnlSoDoGhe.ResumeLayout(false);
            this.pnlLegend.ResumeLayout(false);
            this.pnlLegend.PerformLayout();
            this.pnlManHinhWrapper.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}