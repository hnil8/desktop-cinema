using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmThongKe.Designer.cs
//
// TUÂN THỦ NGHIÊM NGẶT:
//   ✓ KHÔNG có Chart (System.Windows.Forms.DataVisualization.Charting)
//   ✓ KHÔNG có lambda (=>) gán sự kiện
//   ✓ KHÔNG có FocusedBorderColor hay AdaptToRounding
//   ✓ AutoGenerateColumns = false TRƯỚC Clear (trong frmThongKe.cs)
//   ✓ Chart khởi tạo trong frmThongKe.cs → VeBieuDoDoanhThu()
//
// LAYOUT (DockStyle.Fill, nhúng vào pnlChildContainer frmMain):
// ┌──────────────────────────────────────────────────────────┐
// │  pnlHeader (DockStyle.Top, 56px) - Tiêu đề + nút Reload │
// ├──────────────────────────────────────────────────────────┤
// │  pnlCards  (DockStyle.Top, 130px) - 4 Cards số liệu     │
// ├───────────────────────────┬──────────────────────────────┤
// │  pnlBottom (Fill)         │                               │
// │  ├─pnlBottomLeft(40%)     │  pnlBottomRight (60%)        │
// │  │  [Header Top5]         │  [pnlChartContainer] ← EMPTY │
// │  │  [dgvTop5Phim]         │   Biểu đồ vẽ dynamic        │
// │  └──────────────────────  │  ở frmThongKe.cs             │
// └───────────────────────────┴──────────────────────────────┘
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    partial class frmThongKe
    {
        private System.ComponentModel.IContainer components = null;

        // ─── HEADER ───────────────────────────────────────────
        private System.Windows.Forms.Panel              pnlHeader;
        private System.Windows.Forms.Panel              pnlHeaderAccent;
        private System.Windows.Forms.Label              lblTieuDe;
        private System.Windows.Forms.Label              lblCapNhatLuc;
        private Guna.UI2.WinForms.Guna2Button           btnReload;

        // ─── PANEL CARDS (4 thẻ số liệu) ─────────────────────
        private System.Windows.Forms.Panel              pnlCards;
        // 4 card panels (nội dung sẽ được tạo dynamic trong frmThongKe.cs)
        // Chỉ khai báo container cha ở đây
        private System.Windows.Forms.FlowLayoutPanel    flpCards;

        // ─── PANEL DƯỚI (Bottom) - chứa 2 cột ────────────────
        private System.Windows.Forms.Panel              pnlBottom;

        // Cột trái: Top 5 Phim (40%)
        private System.Windows.Forms.Panel              pnlBottomLeft;
        private System.Windows.Forms.Panel              pnlTop5Accent;
        private System.Windows.Forms.Label              lblTop5Header;
        private System.Windows.Forms.Label              lblTop5ThangHienThi;
        private Guna.UI2.WinForms.Guna2DataGridView     dgvTop5Phim;

        // Cột phải: Biểu đồ (60%) - EMPTY, chart vẽ trong frmThongKe.cs
        private System.Windows.Forms.Panel              pnlBottomRight;
        private System.Windows.Forms.Panel              pnlChartAccent;
        private System.Windows.Forms.Label              lblChartHeader;
        // pnlChartContainer: nơi code frmThongKe.cs sẽ Add Chart vào
        private System.Windows.Forms.Panel              pnlChartContainer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ─────────────────────────────────────────────────
            // FORM
            // ─────────────────────────────────────────────────
            this.Name            = "frmThongKe";
            this.Text            = "Thống Kê Doanh Thu";
            this.BackColor       = System.Drawing.Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Size            = new System.Drawing.Size(1020, 660);

            // =================================================
            // HEADER (DockStyle.Top, 56px)
            // =================================================
            this.pnlHeader              = new System.Windows.Forms.Panel();
            this.pnlHeader.Dock         = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height       = 56;
            this.pnlHeader.BackColor    = System.Drawing.Color.White;

            this.pnlHeaderAccent            = new System.Windows.Forms.Panel();
            this.pnlHeaderAccent.Size       = new System.Drawing.Size(4, 56);
            this.pnlHeaderAccent.Location   = new System.Drawing.Point(0, 0);
            this.pnlHeaderAccent.BackColor  = System.Drawing.Color.FromArgb(233, 69, 96);

            this.lblTieuDe              = new System.Windows.Forms.Label();
            this.lblTieuDe.Text         = "Bảng điều khiển thống kê";
            this.lblTieuDe.Font         = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor    = System.Drawing.Color.FromArgb(30, 30, 60);
            this.lblTieuDe.Location     = new System.Drawing.Point(20, 12);
            this.lblTieuDe.AutoSize     = true;

            this.lblCapNhatLuc          = new System.Windows.Forms.Label();
            this.lblCapNhatLuc.Text     = "Cập nhật: ---";
            this.lblCapNhatLuc.Font     = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCapNhatLuc.ForeColor = System.Drawing.Color.FromArgb(150, 150, 170);
            this.lblCapNhatLuc.Location = new System.Drawing.Point(20, 36);
            this.lblCapNhatLuc.AutoSize = true;

            this.btnReload              = new Guna.UI2.WinForms.Guna2Button();
            this.btnReload.Text         = "↻  Làm mới dữ liệu";
            this.btnReload.Size         = new System.Drawing.Size(160, 36);
            this.btnReload.Location     = new System.Drawing.Point(840, 10);
            this.btnReload.BorderRadius = 7;
            this.btnReload.FillColor    = System.Drawing.Color.FromArgb(26, 26, 46);
            this.btnReload.ForeColor    = System.Drawing.Color.White;
            this.btnReload.Font         = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnReload.Cursor       = System.Windows.Forms.Cursors.Hand;
            this.btnReload.Anchor       = System.Windows.Forms.AnchorStyles.Top
                                        | System.Windows.Forms.AnchorStyles.Right;
            this.btnReload.Click       += new System.EventHandler(this.btnReload_Click);

            this.pnlHeader.Controls.Add(this.pnlHeaderAccent);
            this.pnlHeader.Controls.Add(this.lblTieuDe);
            this.pnlHeader.Controls.Add(this.lblCapNhatLuc);
            this.pnlHeader.Controls.Add(this.btnReload);

            // =================================================
            // PANEL CARDS (DockStyle.Top, 130px)
            // FlowLayoutPanel chứa 4 card panel
            // Card panels được tạo DYNAMIC trong frmThongKe.cs
            // =================================================
            this.pnlCards               = new System.Windows.Forms.Panel();
            this.pnlCards.Dock          = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Height        = 130;
            this.pnlCards.BackColor     = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlCards.Padding       = new System.Windows.Forms.Padding(16, 12, 16, 8);

            // FlowLayoutPanel: chứa các card động (Add trong frmThongKe.cs)
            this.flpCards               = new System.Windows.Forms.FlowLayoutPanel();
            this.flpCards.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.flpCards.BackColor     = System.Drawing.Color.Transparent;
            this.flpCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpCards.WrapContents  = false;
            this.flpCards.Padding       = new System.Windows.Forms.Padding(0);

            this.pnlCards.Controls.Add(this.flpCards);

            // =================================================
            // PANEL DƯỚI (DockStyle.Fill)
            // =================================================
            this.pnlBottom              = new System.Windows.Forms.Panel();
            this.pnlBottom.Dock         = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.BackColor    = System.Drawing.Color.FromArgb(245, 246, 250);
            this.pnlBottom.Padding      = new System.Windows.Forms.Padding(16, 8, 16, 16);

            // ── CỘT PHẢI (pnlBottomRight) - 60%, DockStyle.Fill ──
            this.pnlBottomRight             = new System.Windows.Forms.Panel();
            this.pnlBottomRight.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottomRight.BackColor   = System.Drawing.Color.White;
            this.pnlBottomRight.Padding     = new System.Windows.Forms.Padding(16, 14, 16, 16);

            this.pnlChartAccent             = new System.Windows.Forms.Panel();
            this.pnlChartAccent.Dock        = System.Windows.Forms.DockStyle.Top;
            this.pnlChartAccent.Height      = 3;
            this.pnlChartAccent.BackColor   = System.Drawing.Color.FromArgb(52, 152, 219);

            this.lblChartHeader             = new System.Windows.Forms.Label();
            this.lblChartHeader.Dock        = System.Windows.Forms.DockStyle.Top;
            this.lblChartHeader.Text        = "Biểu đồ doanh thu 7 ngày gần nhất";
            this.lblChartHeader.Font        = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblChartHeader.ForeColor   = System.Drawing.Color.FromArgb(30, 30, 60);
            this.lblChartHeader.Height      = 36;
            this.lblChartHeader.TextAlign   = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlChartContainer: NƠI CODE frmThongKe.cs SẼ ADD CHART VÀO
            this.pnlChartContainer          = new System.Windows.Forms.Panel();
            this.pnlChartContainer.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartContainer.BackColor = System.Drawing.Color.White;

            // Thêm vào pnlBottomRight (Fill add trước, Top add sau)
            this.pnlBottomRight.Controls.Add(this.pnlChartContainer);
            this.pnlBottomRight.Controls.Add(this.lblChartHeader);
            this.pnlBottomRight.Controls.Add(this.pnlChartAccent);

            // ── CỘT TRÁI (pnlBottomLeft) - 40%, DockStyle.Left ──
            this.pnlBottomLeft              = new System.Windows.Forms.Panel();
            this.pnlBottomLeft.Dock         = System.Windows.Forms.DockStyle.Left;
            this.pnlBottomLeft.Width        = 380;
            this.pnlBottomLeft.BackColor    = System.Drawing.Color.White;
            this.pnlBottomLeft.Margin       = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.pnlBottomLeft.Padding      = new System.Windows.Forms.Padding(16, 14, 16, 16);

            this.pnlTop5Accent              = new System.Windows.Forms.Panel();
            this.pnlTop5Accent.Dock         = System.Windows.Forms.DockStyle.Top;
            this.pnlTop5Accent.Height       = 3;
            this.pnlTop5Accent.BackColor    = System.Drawing.Color.FromArgb(233, 69, 96);

            this.lblTop5Header              = new System.Windows.Forms.Label();
            this.lblTop5Header.Dock         = System.Windows.Forms.DockStyle.Top;
            this.lblTop5Header.Text         = "Top 5 phim doanh thu cao nhất";
            this.lblTop5Header.Font         = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTop5Header.ForeColor    = System.Drawing.Color.FromArgb(30, 30, 60);
            this.lblTop5Header.Height       = 30;
            this.lblTop5Header.TextAlign    = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblTop5ThangHienThi            = new System.Windows.Forms.Label();
            this.lblTop5ThangHienThi.Dock       = System.Windows.Forms.DockStyle.Top;
            this.lblTop5ThangHienThi.Text       = "";  // Set trong frmThongKe.cs
            this.lblTop5ThangHienThi.Font       = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTop5ThangHienThi.ForeColor  = System.Drawing.Color.FromArgb(150, 150, 170);
            this.lblTop5ThangHienThi.Height     = 20;
            this.lblTop5ThangHienThi.TextAlign  = System.Drawing.ContentAlignment.MiddleLeft;

            // DataGridView Top 5
            this.dgvTop5Phim                                = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dgvTop5Phim.Dock                           = System.Windows.Forms.DockStyle.Fill;
            this.dgvTop5Phim.AllowUserToAddRows             = false;
            this.dgvTop5Phim.AllowUserToDeleteRows          = false;
            this.dgvTop5Phim.ReadOnly                       = true;
            this.dgvTop5Phim.SelectionMode                  = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTop5Phim.MultiSelect                    = false;
            this.dgvTop5Phim.RowHeadersVisible              = false;
            this.dgvTop5Phim.BackgroundColor                = System.Drawing.Color.White;
            this.dgvTop5Phim.Font                           = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvTop5Phim.BorderStyle                    = System.Windows.Forms.BorderStyle.None;
            this.dgvTop5Phim.ColumnHeadersHeight            = 38;
            this.dgvTop5Phim.ColumnHeadersHeightSizeMode    = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTop5Phim.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(233, 69, 96);
            this.dgvTop5Phim.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTop5Phim.RowTemplate.Height             = 44;
            this.dgvTop5Phim.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(252, 248, 248);

            // Thêm vào pnlBottomLeft (Fill add trước)
            this.pnlBottomLeft.Controls.Add(this.dgvTop5Phim);
            this.pnlBottomLeft.Controls.Add(this.lblTop5ThangHienThi);
            this.pnlBottomLeft.Controls.Add(this.lblTop5Header);
            this.pnlBottomLeft.Controls.Add(this.pnlTop5Accent);

            // Thêm 2 cột vào pnlBottom (Left add trước, Fill sau)
            this.pnlBottom.Controls.Add(this.pnlBottomRight);
            this.pnlBottom.Controls.Add(this.pnlBottomLeft);

            // =================================================
            // GẮN VÀO FORM (Fill add trước, Top add sau)
            // =================================================
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);
        }
    }
}
