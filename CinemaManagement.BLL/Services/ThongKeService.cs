using System;
using System.Collections.Generic;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.BLL/Services/ThongKeService.cs
// Tầng BLL: Nghiệp vụ thống kê, format dữ liệu cho GUI
// ============================================================

namespace CinemaManagement.BLL.Services
{
    // ----------------------------------------------------------
    // DTO: Một thẻ (card) số liệu nổi bật
    // ----------------------------------------------------------
    public class CardDto
    {
        public string TieuDe          { get; set; }   // "Doanh thu hôm nay"
        public string GiaTri          { get; set; }   // "8,400,000 ₫"
        public string GiaTriPhu       { get; set; }   // "Tăng 12% so với hôm qua" (tùy chọn)
        public string MauAccent       { get; set; }   // Hex màu accent của card
    }

    // ----------------------------------------------------------
    // DTO: Điểm dữ liệu cho biểu đồ cột
    // ----------------------------------------------------------
    public class BieuDoDoanhThuDto
    {
        public List<string>  NhanTrucX    { get; set; }   // Nhãn trục X ["T2\n10/06", ...]
        public List<double>  GiaTriCot    { get; set; }   // Giá trị mỗi cột (double cho Chart)
        public decimal       GiaTriCaoNhat { get; set; }  // Dùng để scale trục Y
        public string        TieuDeBieuDo { get; set; }
    }

    public class ThongKeService
    {
        private readonly ThongKeRepository _thongKeRepo;

        public ThongKeService()
        {
            _thongKeRepo = new ThongKeRepository();
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH CARDS TỔNG QUAN
        // ──────────────────────────────────────────────────────
        public List<CardDto> GetDanhSachCards()
        {
            List<CardDto> result = new List<CardDto>();

            try
            {
                TongQuanHomNayDto tongQuan = _thongKeRepo.GetTongQuan();
                int soPhimDangChieu        = _thongKeRepo.GetSoPhimDangChieu();

                result.Add(new CardDto
                {
                    TieuDe      = "Doanh thu hôm nay",
                    GiaTri      = FormatTien(tongQuan.DoanhThuHomNay),
                    GiaTriPhu   = string.Format("Tháng này: {0}", FormatTien(tongQuan.DoanhThuThangNay)),
                    MauAccent   = "#E94560"   // Đỏ
                });

                result.Add(new CardDto
                {
                    TieuDe      = "Vé bán hôm nay",
                    GiaTri      = tongQuan.SoVeBanHomNay.ToString("N0") + " vé",
                    GiaTriPhu   = string.Format("Tháng này: {0} vé", tongQuan.SoVeBanThangNay.ToString("N0")),
                    MauAccent   = "#2ECC71"   // Xanh lá
                });

                result.Add(new CardDto
                {
                    TieuDe      = "Doanh thu tháng",
                    GiaTri      = FormatTien(tongQuan.DoanhThuThangNay),
                    GiaTriPhu   = string.Format("Từ ngày 01/{0}", DateTime.Today.ToString("MM/yyyy")),
                    MauAccent   = "#3498DB"   // Xanh dương
                });

                result.Add(new CardDto
                {
                    TieuDe      = "Phim đang chiếu",
                    GiaTri      = soPhimDangChieu.ToString() + " phim",
                    GiaTriPhu   = "Đang hoạt động trong hệ thống",
                    MauAccent   = "#9B59B6"   // Tím
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ThongKeService.GetDanhSachCards] " + ex.Message);
                // Trả về cards rỗng để GUI không crash
                result.Clear();
                result.Add(new CardDto { TieuDe = "Lỗi tải dữ liệu", GiaTri = "---", MauAccent = "#95A5A6" });
            }

            return result;
        }

        // ──────────────────────────────────────────────────────
        // LẤY DỮ LIỆU BIỂU ĐỒ 7 NGÀY
        // ──────────────────────────────────────────────────────
        public BieuDoDoanhThuDto GetDuLieuBieuDo7Ngay()
        {
            BieuDoDoanhThuDto result = new BieuDoDoanhThuDto
            {
                NhanTrucX    = new List<string>(),
                GiaTriCot    = new List<double>(),
                GiaTriCaoNhat = 0,
                TieuDeBieuDo  = "Doanh thu 7 ngày gần nhất"
            };

            try
            {
                List<DoanhThuNgayDto> danhSach = _thongKeRepo.GetDoanhThu7NgayGanNhat();

                foreach (DoanhThuNgayDto item in danhSach)
                {
                    result.NhanTrucX.Add(item.NhanNgay);
                    result.GiaTriCot.Add((double)item.TongDoanhThu);

                    if (item.TongDoanhThu > result.GiaTriCaoNhat)
                        result.GiaTriCaoNhat = item.TongDoanhThu;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ThongKeService.GetDuLieuBieuDo7Ngay] " + ex.Message);
            }

            return result;
        }

        // ──────────────────────────────────────────────────────
        // LẤY TOP 5 PHIM → DTO cho DataGridView
        // ──────────────────────────────────────────────────────
        public List<TopPhimDto> GetTop5Phim()
        {
            try
            {
                return _thongKeRepo.GetTop5PhimThangNay();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ThongKeService.GetTop5Phim] " + ex.Message);
                return new List<TopPhimDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Format tiền tệ chuẩn VNĐ
        // Ví dụ: 1500000 → "1,500,000 ₫"
        // ──────────────────────────────────────────────────────
        private string FormatTien(decimal soTien)
        {
            return soTien.ToString("N0") + " ₫";
        }

        public void Dispose()
        {
            _thongKeRepo.Dispose();
        }
    }
}
