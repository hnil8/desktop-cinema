using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmMain.Designer.cs
// Mục đích: Định nghĩa toàn bộ giao diện Dashboard chính
//
// LAYOUT TỔNG QUAN (1280 × 720):
// ┌─────────────────────────────────────────────────────────┐
// │  SIDEBAR (240px)    │  MAIN PANEL (1040px)              │
// │  ┌───────────────┐  │  ┌─────────────────────────────┐  │
// │  │  Logo + Tên   │  │  │  HEADER BAR (60px)          │  │
// │  ├───────────────┤  │  ├─────────────────────────────┤  │
// │  │  [Bán Vé]     │  │  │                             │  │
// │  │  [Quản lý P]  │  │  │   pnlChildContainer         │  │
// │  │  [Lịch Chiếu] │  │  │   (Form con nhúng vào đây)  │  │
// │  │  [Thống Kê]   │  │  │                             │  │
// │  ├───────────────┤  │  │                             │  │
// │  │  [Đăng Xuất]  │  │  │                             │  │
// │  └───────────────┘  │  └─────────────────────────────┘  │
// └─────────────────────────────────────────────────────────┘
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        // ─── SIDEBAR ─────────────────────────────────────────
        private Panel           pnlSidebar;
        private Panel           pnlLogoArea;         // Vùng logo + tên rạp
        private Label           lblLogoIcon;          // Icon film (ký tự unicode)
        private Label           lblLogoText;          // "CINEMA PRIME"
        private Panel           pnlDividerTop;        // Đường kẻ phân cách
        private Panel           pnlUserInfo;          // Avatar + tên + vai trò
        private Label           lblAvatarCircle;      // Avatar chữ cái đầu tên
        private Label           lblUserName;          // Tên nhân viên
        private Label           lblUserRole;          // Vai trò
        private Panel           pnlDividerMid;        // Đường kẻ giữa
        private Panel           pnlMenuItems;         // Container chứa các nút menu
        private Panel           pnlBottomMenu;        // Container chứa nút Đăng Xuất

        // Nút menu (Guna2Button để có ripple effect + border radius)
        private Guna2Button     btnBanVe;
        private Guna2Button     btnQuanLyPhim;
        private Guna2Button     btnLichChieu;
        private Guna2Button     btnThongKe;
        private Guna2Button     btnQuanLyNhanVien;
        private Guna2Button     btnDangXuat;

        // ─── MAIN AREA ───────────────────────────────────────
        private Panel           pnlMainArea;          // Toàn bộ phần phải
        private Panel           pnlHeader;            // Thanh tiêu đề trên cùng
        private Label           lblPageTitle;          // Tên trang hiện tại
        private Label           lblDateTime;           // Đồng hồ realtime
        private Panel           pnlChildContainer;    // NƠI NHÚNG FORM CON

        // Timer cập nhật đồng hồ
        private System.Windows.Forms.Timer timerClock;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlLogoArea = new System.Windows.Forms.Panel();
            this.lblLogoIcon = new System.Windows.Forms.Label();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.pnlDividerTop = new System.Windows.Forms.Panel();
            this.pnlUserInfo = new System.Windows.Forms.Panel();
            this.lblAvatarCircle = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.pnlDividerMid = new System.Windows.Forms.Panel();
            this.pnlMenuItems = new System.Windows.Forms.Panel();
            this.btnBanVe = new Guna.UI2.WinForms.Guna2Button();
            this.btnQuanLyPhim = new Guna.UI2.WinForms.Guna2Button();
            this.btnLichChieu = new Guna.UI2.WinForms.Guna2Button();
            this.btnThongKe = new Guna.UI2.WinForms.Guna2Button();
            this.btnQuanLyNhanVien = new Guna.UI2.WinForms.Guna2Button();
            this.pnlBottomMenu = new System.Windows.Forms.Panel();
            this.btnDangXuat = new Guna.UI2.WinForms.Guna2Button();
            this.pnlMainArea = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnCloseMain = new Guna.UI2.WinForms.Guna2Button();
            this.pnlChildContainer = new System.Windows.Forms.Panel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.pnlSidebar.SuspendLayout();
            this.pnlLogoArea.SuspendLayout();
            this.pnlUserInfo.SuspendLayout();
            this.pnlMenuItems.SuspendLayout();
            this.pnlBottomMenu.SuspendLayout();
            this.pnlMainArea.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.pnlSidebar.Controls.Add(this.pnlLogoArea);
            this.pnlSidebar.Controls.Add(this.pnlDividerTop);
            this.pnlSidebar.Controls.Add(this.pnlUserInfo);
            this.pnlSidebar.Controls.Add(this.pnlDividerMid);
            this.pnlSidebar.Controls.Add(this.pnlMenuItems);
            this.pnlSidebar.Controls.Add(this.pnlBottomMenu);
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(240, 720);
            this.pnlSidebar.TabIndex = 0;
            // 
            // pnlLogoArea
            // 
            this.pnlLogoArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(36)))));
            this.pnlLogoArea.Controls.Add(this.lblLogoIcon);
            this.pnlLogoArea.Controls.Add(this.lblLogoText);
            this.pnlLogoArea.Location = new System.Drawing.Point(0, 0);
            this.pnlLogoArea.Name = "pnlLogoArea";
            this.pnlLogoArea.Size = new System.Drawing.Size(240, 70);
            this.pnlLogoArea.TabIndex = 0;
            this.pnlLogoArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            // 
            // lblLogoIcon
            // 
            this.lblLogoIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblLogoIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 18F);
            this.lblLogoIcon.Location = new System.Drawing.Point(18, 17);
            this.lblLogoIcon.Name = "lblLogoIcon";
            this.lblLogoIcon.Size = new System.Drawing.Size(36, 36);
            this.lblLogoIcon.TabIndex = 0;
            this.lblLogoIcon.Text = "🎬";
            // 
            // lblLogoText
            // 
            this.lblLogoText.BackColor = System.Drawing.Color.Transparent;
            this.lblLogoText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLogoText.ForeColor = System.Drawing.Color.White;
            this.lblLogoText.Location = new System.Drawing.Point(60, 17);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(160, 36);
            this.lblLogoText.TabIndex = 1;
            this.lblLogoText.Text = "CINEMA PRIME";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDividerTop
            // 
            this.pnlDividerTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.pnlDividerTop.Location = new System.Drawing.Point(20, 70);
            this.pnlDividerTop.Name = "pnlDividerTop";
            this.pnlDividerTop.Size = new System.Drawing.Size(200, 1);
            this.pnlDividerTop.TabIndex = 1;
            // 
            // pnlUserInfo
            // 
            this.pnlUserInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlUserInfo.Controls.Add(this.lblAvatarCircle);
            this.pnlUserInfo.Controls.Add(this.lblUserName);
            this.pnlUserInfo.Controls.Add(this.lblUserRole);
            this.pnlUserInfo.Location = new System.Drawing.Point(0, 72);
            this.pnlUserInfo.Name = "pnlUserInfo";
            this.pnlUserInfo.Size = new System.Drawing.Size(240, 80);
            this.pnlUserInfo.TabIndex = 2;
            // 
            // lblAvatarCircle
            // 
            this.lblAvatarCircle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.lblAvatarCircle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAvatarCircle.ForeColor = System.Drawing.Color.White;
            this.lblAvatarCircle.Location = new System.Drawing.Point(18, 19);
            this.lblAvatarCircle.Name = "lblAvatarCircle";
            this.lblAvatarCircle.Size = new System.Drawing.Size(42, 42);
            this.lblAvatarCircle.TabIndex = 0;
            this.lblAvatarCircle.Text = "U";
            this.lblAvatarCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUserName
            // 
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(70, 20);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(155, 20);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "Người dùng";
            // 
            // lblUserRole
            // 
            this.lblUserRole.BackColor = System.Drawing.Color.Transparent;
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(176)))));
            this.lblUserRole.Location = new System.Drawing.Point(70, 42);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(155, 18);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "Vai trò";
            // 
            // pnlDividerMid
            // 
            this.pnlDividerMid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(90)))));
            this.pnlDividerMid.Location = new System.Drawing.Point(20, 153);
            this.pnlDividerMid.Name = "pnlDividerMid";
            this.pnlDividerMid.Size = new System.Drawing.Size(200, 1);
            this.pnlDividerMid.TabIndex = 3;
            // 
            // pnlMenuItems
            // 
            this.pnlMenuItems.BackColor = System.Drawing.Color.Transparent;
            this.pnlMenuItems.Controls.Add(this.btnBanVe);
            this.pnlMenuItems.Controls.Add(this.btnQuanLyPhim);
            this.pnlMenuItems.Controls.Add(this.btnLichChieu);
            this.pnlMenuItems.Controls.Add(this.btnThongKe);
            this.pnlMenuItems.Controls.Add(this.btnQuanLyNhanVien);
            this.pnlMenuItems.Location = new System.Drawing.Point(0, 160);
            this.pnlMenuItems.Name = "pnlMenuItems";
            this.pnlMenuItems.Size = new System.Drawing.Size(240, 400);
            this.pnlMenuItems.TabIndex = 4;
            // 
            // btnBanVe
            // 
            this.btnBanVe.BorderRadius = 8;
            this.btnBanVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBanVe.FillColor = System.Drawing.Color.Transparent;
            this.btnBanVe.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBanVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.btnBanVe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnBanVe.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnBanVe.Location = new System.Drawing.Point(8, 0);
            this.btnBanVe.Name = "btnBanVe";
            this.btnBanVe.Size = new System.Drawing.Size(224, 44);
            this.btnBanVe.TabIndex = 0;
            this.btnBanVe.Text = "💰  Bán Vé (POS)";
            this.btnBanVe.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnBanVe.Click += new System.EventHandler(this.btnBanVe_Click);
            // 
            // btnQuanLyPhim
            // 
            this.btnQuanLyPhim.BorderRadius = 8;
            this.btnQuanLyPhim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuanLyPhim.FillColor = System.Drawing.Color.Transparent;
            this.btnQuanLyPhim.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnQuanLyPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.btnQuanLyPhim.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnQuanLyPhim.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyPhim.Location = new System.Drawing.Point(8, 54);
            this.btnQuanLyPhim.Name = "btnQuanLyPhim";
            this.btnQuanLyPhim.Size = new System.Drawing.Size(224, 44);
            this.btnQuanLyPhim.TabIndex = 1;
            this.btnQuanLyPhim.Text = "🎬  Quản lý Phim";
            this.btnQuanLyPhim.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnQuanLyPhim.Click += new System.EventHandler(this.btnQuanLyPhim_Click);
            // 
            // btnLichChieu
            // 
            this.btnLichChieu.BorderRadius = 8;
            this.btnLichChieu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLichChieu.FillColor = System.Drawing.Color.Transparent;
            this.btnLichChieu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLichChieu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.btnLichChieu.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnLichChieu.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnLichChieu.Location = new System.Drawing.Point(8, 108);
            this.btnLichChieu.Name = "btnLichChieu";
            this.btnLichChieu.Size = new System.Drawing.Size(224, 44);
            this.btnLichChieu.TabIndex = 2;
            this.btnLichChieu.Text = "📅  Lịch Chiếu";
            this.btnLichChieu.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnLichChieu.Click += new System.EventHandler(this.btnLichChieu_Click);
            // 
            // btnThongKe
            // 
            this.btnThongKe.BorderRadius = 8;
            this.btnThongKe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThongKe.FillColor = System.Drawing.Color.Transparent;
            this.btnThongKe.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnThongKe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.btnThongKe.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnThongKe.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnThongKe.Location = new System.Drawing.Point(8, 162);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(224, 44);
            this.btnThongKe.TabIndex = 3;
            this.btnThongKe.Text = "📊  Thống Kê";
            this.btnThongKe.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // btnQuanLyNhanVien
            // 
            this.btnQuanLyNhanVien.BorderRadius = 8;
            this.btnQuanLyNhanVien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuanLyNhanVien.FillColor = System.Drawing.Color.Transparent;
            this.btnQuanLyNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnQuanLyNhanVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(200)))));
            this.btnQuanLyNhanVien.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnQuanLyNhanVien.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyNhanVien.Location = new System.Drawing.Point(8, 216);
            this.btnQuanLyNhanVien.Name = "btnQuanLyNhanVien";
            this.btnQuanLyNhanVien.Size = new System.Drawing.Size(224, 44);
            this.btnQuanLyNhanVien.TabIndex = 4;
            this.btnQuanLyNhanVien.Text = "👥  Nhân Viên";
            this.btnQuanLyNhanVien.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnQuanLyNhanVien.Click += new System.EventHandler(this.btnQuanLyNhanVien_Click);
            // 
            // pnlBottomMenu
            // 
            this.pnlBottomMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlBottomMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(36)))));
            this.pnlBottomMenu.Controls.Add(this.btnDangXuat);
            this.pnlBottomMenu.Location = new System.Drawing.Point(0, 660);
            this.pnlBottomMenu.Name = "pnlBottomMenu";
            this.pnlBottomMenu.Size = new System.Drawing.Size(240, 60);
            this.pnlBottomMenu.TabIndex = 5;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.BorderRadius = 8;
            this.btnDangXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangXuat.FillColor = System.Drawing.Color.Transparent;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDangXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.btnDangXuat.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnDangXuat.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.Location = new System.Drawing.Point(8, 8);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(224, 44);
            this.btnDangXuat.TabIndex = 0;
            this.btnDangXuat.Text = "🚪  Đăng Xuất";
            this.btnDangXuat.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // pnlMainArea
            // 
            this.pnlMainArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMainArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlMainArea.Controls.Add(this.pnlHeader);
            this.pnlMainArea.Controls.Add(this.pnlChildContainer);
            this.pnlMainArea.Location = new System.Drawing.Point(240, 0);
            this.pnlMainArea.Name = "pnlMainArea";
            this.pnlMainArea.Size = new System.Drawing.Size(1040, 720);
            this.pnlMainArea.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblPageTitle);
            this.pnlHeader.Controls.Add(this.lblDateTime);
            this.pnlHeader.Controls.Add(this.btnMinimize);
            this.pnlHeader.Controls.Add(this.btnCloseMain);
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1040, 60);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblPageTitle.Location = new System.Drawing.Point(24, 16);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(101, 25);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Trang chủ";
            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.lblDateTime.Location = new System.Drawing.Point(718, 22);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(178, 17);
            this.lblDateTime.TabIndex = 1;
            this.lblDateTime.Text = "Thứ Tư, 01/04/2026  19:30:53";
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BorderRadius = 6;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnMinimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.btnMinimize.Location = new System.Drawing.Point(935, 16);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(36, 28);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.Text = "─";
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnCloseMain
            // 
            this.btnCloseMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseMain.BorderRadius = 6;
            this.btnCloseMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseMain.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(69)))), ((int)(((byte)(96)))));
            this.btnCloseMain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCloseMain.ForeColor = System.Drawing.Color.White;
            this.btnCloseMain.Location = new System.Drawing.Point(980, 16);
            this.btnCloseMain.Name = "btnCloseMain";
            this.btnCloseMain.Size = new System.Drawing.Size(36, 28);
            this.btnCloseMain.TabIndex = 3;
            this.btnCloseMain.Text = "✕";
            this.btnCloseMain.Click += new System.EventHandler(this.btnCloseMain_Click);
            // 
            // pnlChildContainer
            // 
            this.pnlChildContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChildContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.pnlChildContainer.Location = new System.Drawing.Point(0, 60);
            this.pnlChildContainer.Name = "pnlChildContainer";
            this.pnlChildContainer.Size = new System.Drawing.Size(1040, 660);
            this.pnlChildContainer.TabIndex = 1;
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // frmMain
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlMainArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1100, 640);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cinema Prime – Hệ thống quản lý rạp chiếu phim";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlLogoArea.ResumeLayout(false);
            this.pnlUserInfo.ResumeLayout(false);
            this.pnlMenuItems.ResumeLayout(false);
            this.pnlBottomMenu.ResumeLayout(false);
            this.pnlMainArea.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        private Guna2Button btnMinimize;
        private Guna2Button btnCloseMain;

        // ─────────────────────────────────────────────────────
        // HELPER: Tạo nút menu sidebar theo chuẩn design
        // Tái sử dụng để tránh copy-paste code
        // ─────────────────────────────────────────────────────

    }
}
