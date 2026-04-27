using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CinemaManagement.BLL.Services;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.GUI/Forms/frmThongKe.cs
// Tầng GUI: Dashboard thống kê
//
// *** CHART CHỈ ĐƯỢC KHỞI TẠO Ở ĐÂY, KHÔNG BAO GIỜ TRONG Designer.cs ***
//
// TUÂN THỦ LUẬT:
//   ✓ AutoGenerateColumns = false TRƯỚC Columns.Clear()
//   ✓ BindingSource khi bind dữ liệu vào DGV
//   ✓ Chart tạo động trong VeBieuDoDoanhThu()
//   ✓ C# 7.3: không using var, không pattern matching
// ============================================================

namespace CinemaManagement.GUI.Forms
{
    public partial class frmThongKe : Form
    {
        // ─────────────────────────────────────────────────────
        // FIELDS
        // ─────────────────────────────────────────────────────
        private readonly ThongKeService _thongKeService;

        // Màu accent cho 4 cards (tương ứng thứ tự)
        private readonly string[] _mauCards = {
            "#E94560",   // Đỏ     - Doanh thu hôm nay
            "#2ECC71",   // Xanh   - Số vé
            "#3498DB",   // Xanh D - Doanh thu tháng
            "#9B59B6"    // Tím    - Phim đang chiếu
        };

        // ─────────────────────────────────────────────────────
        // CONSTRUCTOR
        // ─────────────────────────────────────────────────────
        public frmThongKe()
        {
            InitializeComponent();
            _thongKeService = new ThongKeService();
            this.Load += new EventHandler(frmThongKe_Load);
            // Resize: cập nhật lại kích thước chart khi panel thay đổi
            this.Resize += new EventHandler(frmThongKe_Resize);
        }

        // ─────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────
        private void frmThongKe_Load(object sender, EventArgs e)
        {
            ThietLapCotDgvTop5();
            TaiTatCaDuLieu();
        }

        // ─────────────────────────────────────────────────────
        // TẢI TẤT CẢ DỮ LIỆU (dùng cho cả Load và Reload)
        // ─────────────────────────────────────────────────────
        private void TaiTatCaDuLieu()
        {
            TaoCards();
            TaiTop5Phim();
            VeBieuDoDoanhThu();
            lblCapNhatLuc.Text = "Cập nhật: " + DateTime.Now.ToString("HH:mm:ss  dd/MM/yyyy");
        }

        // ─────────────────────────────────────────────────────
        // TẠO 4 CARDS ĐỘNG
        // ─────────────────────────────────────────────────────
        private void TaoCards()
        {
            flpCards.Controls.Clear();

            List<CardDto> danhSach = _thongKeService.GetDanhSachCards();

            // Tính kích thước card: chia đều width, trừ padding
            int tongChieu = flpCards.Width > 0 ? flpCards.Width : 960;
            int cardW     = (tongChieu - (danhSach.Count + 1) * 12) / danhSach.Count;
            if (cardW < 160) cardW = 200;
            int cardH     = flpCards.Height > 0 ? flpCards.Height - 4 : 96;
            if (cardH < 80) cardH = 96;

            for (int i = 0; i < danhSach.Count; i++)
            {
                CardDto card    = danhSach[i];
                string hexMau   = (i < _mauCards.Length) ? _mauCards[i] : "#95A5A6";
                Color mauAccent = ParseHexColor(hexMau);

                Panel pnlCard           = new Panel();
                pnlCard.Size            = new Size(cardW, cardH);
                pnlCard.BackColor       = Color.White;
                pnlCard.Margin          = new Padding(0, 0, 12, 0);
                pnlCard.Cursor          = Cursors.Default;

                // Vẽ border bo góc và dải màu trên đầu card qua Paint
                // Dùng delegate để truyền mauAccent vào closure
                PaintCardDelegate paintDelegate = new PaintCardDelegate(mauAccent);
                pnlCard.Paint += new PaintEventHandler(paintDelegate.OnPaint);

                // Dải màu accent trên cùng (4px)
                Panel pnlAccentBar          = new Panel();
                pnlAccentBar.Size           = new Size(cardW, 4);
                pnlAccentBar.Location       = new Point(0, 0);
                pnlAccentBar.BackColor      = mauAccent;

                // Label tiêu đề
                Label lblTitle              = new Label();
                lblTitle.Text               = card.TieuDe.ToUpper();
                lblTitle.Font               = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                lblTitle.ForeColor          = Color.FromArgb(150, 150, 170);
                lblTitle.Location           = new Point(14, 14);
                lblTitle.AutoSize           = false;
                lblTitle.Size               = new Size(cardW - 20, 18);
                lblTitle.BackColor          = Color.Transparent;

                // Label giá trị chính (to nhất)
                Label lblGiaTri             = new Label();
                lblGiaTri.Text              = card.GiaTri;
                lblGiaTri.Font              = new Font("Segoe UI", 16F, FontStyle.Bold);
                lblGiaTri.ForeColor         = Color.FromArgb(30, 30, 60);
                lblGiaTri.Location          = new Point(12, 32);
                lblGiaTri.AutoSize          = false;
                lblGiaTri.Size              = new Size(cardW - 16, 36);
                lblGiaTri.BackColor         = Color.Transparent;
                lblGiaTri.TextAlign         = ContentAlignment.MiddleLeft;

                // Label phụ (nhỏ hơn, màu xám)
                Label lblPhu                = new Label();
                lblPhu.Text                 = card.GiaTriPhu ?? "";
                lblPhu.Font                 = new Font("Segoe UI", 8F);
                lblPhu.ForeColor            = Color.FromArgb(150, 150, 170);
                lblPhu.Location             = new Point(14, 72);
                lblPhu.AutoSize             = false;
                lblPhu.Size                 = new Size(cardW - 20, 18);
                lblPhu.BackColor            = Color.Transparent;

                pnlCard.Controls.Add(pnlAccentBar);
                pnlCard.Controls.Add(lblTitle);
                pnlCard.Controls.Add(lblGiaTri);
                pnlCard.Controls.Add(lblPhu);

                flpCards.Controls.Add(pnlCard);
            }
        }

        // ─────────────────────────────────────────────────────
        // THIẾT LẬP CỘT DataGridView TOP 5
        // *** LUẬT: AutoGenerateColumns = false TRƯỚC Clear ***
        // ─────────────────────────────────────────────────────
        private void ThietLapCotDgvTop5()
        {
            // BẮT BUỘC: đặt false TRƯỚC khi Clear
            dgvTop5Phim.AutoGenerateColumns = false;
            dgvTop5Phim.Columns.Clear();

            // Cột ẩn PhimId
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name             = "PhimId";
            colId.DataPropertyName = "PhimId";
            colId.Visible          = false;
            dgvTop5Phim.Columns.Add(colId);

            // Tên phim (kèm số thứ tự)
            DataGridViewTextBoxColumn colTen = new DataGridViewTextBoxColumn();
            colTen.Name             = "TenPhim";
            colTen.HeaderText       = "Tên phim";
            colTen.DataPropertyName = "TenPhim";
            colTen.FillWeight       = 48;
            dgvTop5Phim.Columns.Add(colTen);

            // Số vé
            DataGridViewTextBoxColumn colVe = new DataGridViewTextBoxColumn();
            colVe.Name             = "SoVeDaBan";
            colVe.HeaderText       = "Số vé";
            colVe.DataPropertyName = "SoVeDaBan";
            colVe.FillWeight       = 16;
            colVe.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTop5Phim.Columns.Add(colVe);

            // Doanh thu
            DataGridViewTextBoxColumn colDT = new DataGridViewTextBoxColumn();
            colDT.Name             = "DoanhThuHienThi";
            colDT.HeaderText       = "Doanh thu";
            colDT.DataPropertyName = "DoanhThuHienThi";
            colDT.FillWeight       = 36;
            colDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colDT.DefaultCellStyle.ForeColor = Color.FromArgb(233, 69, 96);
            colDT.DefaultCellStyle.Font      = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvTop5Phim.Columns.Add(colDT);

            // Style header
            dgvTop5Phim.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvTop5Phim.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(26, 26, 46);
            dgvTop5Phim.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTop5Phim.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgvTop5Phim.EnableHeadersVisualStyles = false;
        }

        // ─────────────────────────────────────────────────────
        // TẢI TOP 5 PHIM → DGV qua BindingSource
        // ─────────────────────────────────────────────────────
        private void TaiTop5Phim()
        {
            try
            {
                List<TopPhimDto> danhSach = _thongKeService.GetTop5Phim();

                // BẮT BUỘC: dùng BindingSource, không gán List trực tiếp
                BindingSource bs = new BindingSource();
                bs.DataSource    = danhSach;
                dgvTop5Phim.DataSource = bs;

                // Cập nhật tiêu đề tháng
                lblTop5ThangHienThi.Text = string.Format(
                    "Tháng {0}/{1}",
                    DateTime.Today.Month.ToString("D2"),
                    DateTime.Today.Year);

                // Tô màu nền các hàng theo thứ hạng (gold, silver, bronze)
                ToMauHangTop5();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[frmThongKe.TaiTop5Phim] " + ex.Message);
            }
        }

        // ─────────────────────────────────────────────────────
        // TÔ MÀU HẠNG 1, 2, 3 TRONG TOP 5
        // ─────────────────────────────────────────────────────
        private void ToMauHangTop5()
        {
            Color[] mauHang = {
                Color.FromArgb(255, 215, 0),    // Gold  - Hạng 1
                Color.FromArgb(192, 192, 192),  // Silver- Hạng 2
                Color.FromArgb(205, 127, 50),   // Bronze- Hạng 3
            };

            for (int i = 0; i < dgvTop5Phim.Rows.Count && i < 3; i++)
            {
                dgvTop5Phim.Rows[i].DefaultCellStyle.BackColor = mauHang[i];
                dgvTop5Phim.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 60);
                dgvTop5Phim.Rows[i].DefaultCellStyle.SelectionBackColor =
                    Color.FromArgb(233, 69, 96);
            }
        }

        // ─────────────────────────────────────────────────────
        // *** VẼ BIỂU ĐỒ DOANH THU - AN TOÀN ***
        // Chart CHỈ được tạo ở đây, TUYỆT ĐỐI KHÔNG trong Designer.cs
        // Tham chiếu: System.Windows.Forms.DataVisualization.Charting
        // (Thêm Reference: Right-click References → Add Reference
        //  → Assemblies → Framework → System.Windows.Forms.DataVisualization)
        // ─────────────────────────────────────────────────────
        private void VeBieuDoDoanhThu()
        {
            // Xóa chart cũ nếu có
            pnlChartContainer.Controls.Clear();

            try
            {
                BieuDoDoanhThuDto duLieu = _thongKeService.GetDuLieuBieuDo7Ngay();

                // ── Bước 1: Tạo Chart object ──────────────────
                Chart chart         = new Chart();
                chart.Dock          = DockStyle.Fill;
                chart.BackColor     = Color.White;
                chart.BorderlineDashStyle = ChartDashStyle.NotSet;

                // ── Bước 2: Tạo ChartArea (vùng vẽ) ──────────
                ChartArea chartArea             = new ChartArea();
                chartArea.Name                  = "MainArea";
                chartArea.BackColor             = Color.White;
                chartArea.BorderColor           = Color.Transparent;
                chartArea.BorderDashStyle       = ChartDashStyle.NotSet;

                // Trục X
                chartArea.AxisX.LabelStyle.Font     = new Font("Segoe UI", 8F);
                chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(100, 100, 130);
                chartArea.AxisX.LineColor           = Color.FromArgb(220, 220, 235);
                chartArea.AxisX.MajorGrid.Enabled   = false;
                chartArea.AxisX.MajorTickMark.Enabled = false;
                chartArea.AxisX.IsLabelAutoFit      = false;

                // Trục Y
                chartArea.AxisY.LabelStyle.Font     = new Font("Segoe UI", 7.5F);
                chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(100, 100, 130);
                chartArea.AxisY.LabelStyle.Format   = "N0";
                chartArea.AxisY.LineColor           = Color.FromArgb(220, 220, 235);
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(235, 235, 245);
                chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chartArea.AxisY.MajorTickMark.Enabled   = false;
                chartArea.AxisY.Minimum             = 0;

                // Scale trục Y dựa vào giá trị cao nhất
                if (duLieu.GiaTriCaoNhat > 0)
                    chartArea.AxisY.Maximum = (double)(duLieu.GiaTriCaoNhat * 1.2m);
                else
                    chartArea.AxisY.Maximum = 1000000;

                // Padding trong vùng biểu đồ
                chartArea.InnerPlotPosition = new ElementPosition(8, 5, 88, 82);

                chart.ChartAreas.Add(chartArea);

                // ── Bước 3: Tạo Legend (chú thích) ───────────
                Legend legend           = new Legend();
                legend.Name             = "MainLegend";
                legend.Font             = new Font("Segoe UI", 8.5F);
                legend.ForeColor        = Color.FromArgb(80, 80, 110);
                legend.BackColor        = Color.Transparent;
                legend.BorderColor      = Color.Transparent;
                legend.Docking          = Docking.Bottom;
                chart.Legends.Add(legend);

                // ── Bước 4: Tạo Series (chuỗi dữ liệu) ───────
                Series series               = new Series();
                series.Name                 = "DoanhThu";
                series.LegendText           = "Doanh thu (₫)";
                series.ChartType            = SeriesChartType.Column;
                series.ChartArea            = "MainArea";
                series.Legend               = "MainLegend";
                series.Color                = Color.FromArgb(233, 69, 96);
                series.BorderColor          = Color.FromArgb(200, 40, 70);
                series.BorderWidth          = 1;
                // Hiển thị nhãn giá trị trên đỉnh cột
                series.IsValueShownAsLabel  = true;
                series.LabelFormat          = "N0";
                series.Font                 = new Font("Segoe UI", 7F);
                series.LabelForeColor       = Color.FromArgb(60, 60, 90);

                // ── Bước 5: Nhồi dữ liệu vào Series ──────────
                for (int i = 0; i < duLieu.NhanTrucX.Count; i++)
                {
                    double giaTriY  = (i < duLieu.GiaTriCot.Count) ? duLieu.GiaTriCot[i] : 0.0;
                    string nhanX    = (i < duLieu.NhanTrucX.Count) ? duLieu.NhanTrucX[i] : "";

                    int pointIndex = series.Points.AddY(giaTriY);
                    series.Points[pointIndex].AxisLabel = nhanX;

                    // Tô màu cột hôm nay đậm hơn (cột cuối cùng)
                    if (i == duLieu.NhanTrucX.Count - 1)
                    {
                        series.Points[pointIndex].Color       = Color.FromArgb(26, 26, 46);
                        series.Points[pointIndex].BorderColor = Color.FromArgb(10, 10, 30);
                    }
                }

                chart.Series.Add(series);

                // ── Bước 6: Tiêu đề biểu đồ ─────────────────
                Title title                 = new Title();
                title.Text                  = "";  // Đã có lblChartHeader bên ngoài
                title.Font                  = new Font("Segoe UI", 9F);
                title.ForeColor             = Color.FromArgb(80, 80, 110);
                title.Docking               = Docking.Top;
                chart.Titles.Add(title);

                // ── Bước 7: Add vào pnlChartContainer ────────
                pnlChartContainer.Controls.Add(chart);
            }
            catch (Exception ex)
            {
                // Nếu lỗi (VD: chưa có reference), hiện label thay thế
                Label lblLoi            = new Label();
                lblLoi.Text             = "Không thể tải biểu đồ.\n" +
                                          "Kiểm tra Reference: System.Windows.Forms.DataVisualization\n\n" +
                                          "Lỗi: " + ex.Message;
                lblLoi.Font             = new Font("Segoe UI", 9F);
                lblLoi.ForeColor        = Color.FromArgb(150, 150, 170);
                lblLoi.Dock             = DockStyle.Fill;
                lblLoi.TextAlign        = ContentAlignment.MiddleCenter;
                pnlChartContainer.Controls.Add(lblLoi);
            }
        }

        // ─────────────────────────────────────────────────────
        // NÚT RELOAD
        // ─────────────────────────────────────────────────────
        private void btnReload_Click(object sender, EventArgs e)
        {
            btnReload.Enabled   = false;
            btnReload.Text      = "Đang tải...";
            btnReload.FillColor = Color.FromArgb(100, 100, 120);

            try
            {
                TaiTatCaDuLieu();
            }
            finally
            {
                btnReload.Enabled   = true;
                btnReload.Text      = "↻  Làm mới dữ liệu";
                btnReload.FillColor = Color.FromArgb(26, 26, 46);
            }
        }

        // ─────────────────────────────────────────────────────
        // SỰ KIỆN RESIZE: Vẽ lại card khi form thay đổi kích thước
        // ─────────────────────────────────────────────────────
        private void frmThongKe_Resize(object sender, EventArgs e)
        {
            // Chỉ tái tạo cards nếu flpCards đã có control (đã load xong)
            if (flpCards.Controls.Count > 0)
                TaoCards();
        }

        // ─────────────────────────────────────────────────────
        // HELPER: Parse màu hex (#RRGGBB) → Color
        // ─────────────────────────────────────────────────────
        private Color ParseHexColor(string hex)
        {
            try
            {
                hex = hex.TrimStart('#');
                int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                return Color.FromArgb(r, g, b);
            }
            catch
            {
                return Color.FromArgb(150, 150, 170);
            }
        }

        // ─────────────────────────────────────────────────────
        // HELPER CLASS: Delegate vẽ card bo góc
        // Cần class riêng để mỗi card có closure màu sắc riêng
        // mà không dùng lambda (cấm bởi C# 7.3 rule của project)
        // ─────────────────────────────────────────────────────
        private class PaintCardDelegate
        {
            private readonly Color _mauAccent;

            public PaintCardDelegate(Color mauAccent)
            {
                _mauAccent = mauAccent;
            }

            public void OnPaint(object sender, PaintEventArgs e)
            {
                Panel panel = sender as Panel;
                if (panel == null)
                    return;

                Graphics g      = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Vẽ nền trắng bo góc
                Rectangle rect  = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                using (GraphicsPath path = RoundedRectPath(rect, 10))
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        g.FillPath(brush, path);
                    using (Pen pen = new Pen(Color.FromArgb(230, 230, 238), 1F))
                        g.DrawPath(pen, path);
                }
            }

            private GraphicsPath RoundedRectPath(Rectangle bounds, int radius)
            {
                int d = radius * 2;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(bounds.X,                  bounds.Y,                   d, d, 180, 90);
                path.AddArc(bounds.Right - d,          bounds.Y,                   d, d, 270, 90);
                path.AddArc(bounds.Right - d,          bounds.Bottom - d,          d, d, 0,   90);
                path.AddArc(bounds.X,                  bounds.Bottom - d,          d, d, 90,  90);
                path.CloseFigure();
                return path;
            }
        }
    }
}
