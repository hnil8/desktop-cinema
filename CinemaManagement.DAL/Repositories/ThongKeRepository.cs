using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================
// FILE: CinemaManagement.DAL/Repositories/ThongKeRepository.cs
// Tầng DAL: Các truy vấn thống kê doanh thu
// Context: CinemaDBEntities
// Tên bảng: HoaDons, VePhims, Phims (EF6 pluralized)
// ============================================================

namespace CinemaManagement.DAL.Repositories
{
    // ----------------------------------------------------------
    // DTO: Doanh thu tổng quan hôm nay
    // ----------------------------------------------------------
    public class TongQuanHomNayDto
    {
        public decimal  DoanhThuHomNay    { get; set; }
        public int      SoVeBanHomNay     { get; set; }
        public decimal  DoanhThuThangNay  { get; set; }
        public int      SoVeBanThangNay   { get; set; }
    }

    // ----------------------------------------------------------
    // DTO: Doanh thu theo từng ngày (dùng cho biểu đồ cột)
    // ----------------------------------------------------------
    public class DoanhThuNgayDto
    {
        public DateTime Ngay           { get; set; }
        public decimal  TongDoanhThu   { get; set; }
        public string   NhanNgay       { get; set; } // "T2 15/06" để hiện trục X
    }

    // ----------------------------------------------------------
    // DTO: Top phim doanh thu cao nhất
    // ----------------------------------------------------------
    public class TopPhimDto
    {
        public int      PhimId       { get; set; }
        public string   TenPhim      { get; set; }
        public int      SoVeDaBan    { get; set; }
        public decimal  DoanhThu     { get; set; }
        public string   DoanhThuHienThi { get; set; }
    }

    public class ThongKeRepository
    {
        private readonly CinemaDBEntities _context;

        public ThongKeRepository()
        {
            _context = new CinemaDBEntities();
        }

        // ──────────────────────────────────────────────────────
        // TỔNG QUAN HÔM NAY & THÁNG NÀY
        // Join HoaDons → VePhims để tính số vé
        // Chỉ tính HoaDon có TrangThai = 'HoanThanh'
        // ──────────────────────────────────────────────────────
        public TongQuanHomNayDto GetTongQuan()
        {
            DateTime homNay     = DateTime.Today;
            DateTime ngayMai    = homNay.AddDays(1);
            DateTime dauThang   = new DateTime(homNay.Year, homNay.Month, 1);
            DateTime dauThangSau = dauThang.AddMonths(1);

            // Doanh thu hôm nay
            decimal doanhThuHomNay = _context.HoaDons
                .Where(hd => hd.TrangThai     == "HoanThanh"
                          && hd.ThoiGianTao   >= homNay
                          && hd.ThoiGianTao   <  ngayMai)
                .Select(hd => hd.ThanhTien)
                .DefaultIfEmpty(0)
                .Sum();

            // Số vé hôm nay: đếm VePhim qua HoaDon hôm nay
            int soVeHomNay = _context.VePhims
                .Count(v => v.TrangThai == "DaBan"
                         && v.HoaDon != null
                         && v.HoaDon.TrangThai   == "HoanThanh"
                         && v.HoaDon.ThoiGianTao >= homNay
                         && v.HoaDon.ThoiGianTao <  ngayMai);

            // Doanh thu tháng này
            decimal doanhThuThang = _context.HoaDons
                .Where(hd => hd.TrangThai     == "HoanThanh"
                          && hd.ThoiGianTao   >= dauThang
                          && hd.ThoiGianTao   <  dauThangSau)
                .Select(hd => hd.ThanhTien)
                .DefaultIfEmpty(0)
                .Sum();

            // Số vé tháng này
            int soVeThang = _context.VePhims
                .Count(v => v.TrangThai == "DaBan"
                         && v.HoaDon != null
                         && v.HoaDon.TrangThai   == "HoanThanh"
                         && v.HoaDon.ThoiGianTao >= dauThang
                         && v.HoaDon.ThoiGianTao <  dauThangSau);

            return new TongQuanHomNayDto
            {
                DoanhThuHomNay   = doanhThuHomNay,
                SoVeBanHomNay    = soVeHomNay,
                DoanhThuThangNay = doanhThuThang,
                SoVeBanThangNay  = soVeThang
            };
        }

        // ──────────────────────────────────────────────────────
        // DOANH THU 7 NGÀY GẦN NHẤT (nhóm theo ngày)
        // Dùng cho biểu đồ cột Bar Chart
        // ──────────────────────────────────────────────────────
        public List<DoanhThuNgayDto> GetDoanhThu7NgayGanNhat()
        {
            DateTime ngayBatDau = DateTime.Today.AddDays(-6); // 7 ngày kể cả hôm nay
            DateTime ngayKetThuc = DateTime.Today.AddDays(1);

            // Lấy dữ liệu thô từ DB
            var rawData = _context.HoaDons
                .Where(hd => hd.TrangThai   == "HoanThanh"
                          && hd.ThoiGianTao >= ngayBatDau
                          && hd.ThoiGianTao <  ngayKetThuc)
                .GroupBy(hd => hd.ThoiGianTao.Day)  // GroupBy ngày trong tháng
                .Select(g => new
                {
                    NgayTrongThang = g.Key,
                    TongThu        = g.Sum(hd => hd.ThanhTien),
                    NgayDayDu      = g.Min(hd => hd.ThoiGianTao) // Lấy đại diện DateTime
                })
                .ToList();

            // Build danh sách đủ 7 ngày (có ngày không có doanh thu thì = 0)
            List<DoanhThuNgayDto> result = new List<DoanhThuNgayDto>();

            for (int i = 6; i >= 0; i--)
            {
                DateTime ngay = DateTime.Today.AddDays(-i);

                decimal tongThu = 0;
                foreach (var item in rawData)
                {
                    // So sánh theo ngày trong tháng VÀ tháng để tránh nhầm lẫn
                    if (item.NgayDayDu.Date == ngay.Date)
                    {
                        tongThu = item.TongThu;
                        break;
                    }
                }

                // Tên ngày tiếng Việt viết tắt
                string[] tenThu = { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };
                string nhanNgay = string.Format("{0}\n{1}/{2}",
                    tenThu[(int)ngay.DayOfWeek],
                    ngay.Day.ToString("D2"),
                    ngay.Month.ToString("D2"));

                result.Add(new DoanhThuNgayDto
                {
                    Ngay         = ngay,
                    TongDoanhThu = tongThu,
                    NhanNgay     = nhanNgay
                });
            }

            return result;
        }

        // ──────────────────────────────────────────────────────
        // TOP 5 PHIM DOANH THU CAO NHẤT THÁNG NÀY
        // Join VePhims → LichChieu_Ghe → LichChieu → Phim
        // Tính tổng GiaBan của các vé trong tháng
        // ──────────────────────────────────────────────────────
        public List<TopPhimDto> GetTop5PhimThangNay()
        {
            DateTime dauThang    = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime dauThangSau = dauThang.AddMonths(1);

            // Join qua navigation properties EF
            var query =
                from ve   in _context.VePhims
                join lcg  in _context.LichChieu_Ghe on ve.LichChieuGheId equals lcg.LichChieuGheId
                join lc   in _context.LichChieux      on lcg.LichChieuId   equals lc.LichChieuId
                join p    in _context.Phims           on lc.PhimId         equals p.PhimId
                join hd   in _context.HoaDons         on ve.HoaDonId       equals hd.HoaDonId
                where ve.TrangThai  == "DaBan"
                   && hd.TrangThai  == "HoanThanh"
                   && hd.ThoiGianTao >= dauThang
                   && hd.ThoiGianTao <  dauThangSau
                group new { ve, p } by new { lc.PhimId, p.TenPhim }
                into grp
                orderby grp.Sum(x => x.ve.GiaBan) descending
                select new
                {
                    PhimId    = grp.Key.PhimId,
                    TenPhim   = grp.Key.TenPhim,
                    SoVe      = grp.Count(),
                    DoanhThu  = grp.Sum(x => x.ve.GiaBan)
                };

            List<TopPhimDto> result = new List<TopPhimDto>();
            int rank = 1;

            foreach (var item in query.Take(5))
            {
                result.Add(new TopPhimDto
                {
                    PhimId           = item.PhimId,
                    TenPhim          = string.Format("{0}. {1}", rank, item.TenPhim),
                    SoVeDaBan        = item.SoVe,
                    DoanhThu         = item.DoanhThu,
                    DoanhThuHienThi  = item.DoanhThu.ToString("N0") + " ₫"
                });
                rank++;
            }

            return result;
        }

        // ──────────────────────────────────────────────────────
        // SỐ PHIM ĐANG CHIẾU (dùng cho Card thứ 4)
        // ──────────────────────────────────────────────────────
        public int GetSoPhimDangChieu()
        {
            return _context.Phims
                           .Count(p => p.TrangThai == "DangChieu"
                                    && p.IsDeleted == false);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
