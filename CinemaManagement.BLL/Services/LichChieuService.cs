using System;
using System.Collections.Generic;
using CinemaManagement.DAL;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.BLL/Services/LichChieuService.cs
// Tầng BLL: Xử lý nghiệp vụ lịch chiếu
//   - Tính GioKetThuc tự động (ThoiLuong + 15 phút dọn phòng)
//   - Validate trùng lịch phòng chiếu
//   - Trả về ServiceResult (đã định nghĩa trong PhimService.cs)
// ============================================================

namespace CinemaManagement.BLL.Services
{
    // ----------------------------------------------------------
    // DTO: Dữ liệu lịch chiếu hiển thị lên DataGridView
    // ----------------------------------------------------------
    public class LichChieuDto
    {
        public int      LichChieuId         { get; set; }
        public int      PhimId              { get; set; }
        public int      PhongId             { get; set; }
        public string   TenPhim             { get; set; }
        public string   TenPhong            { get; set; }
        public DateTime GioBatDau           { get; set; }   // Raw value (dùng khi fill form)
        public DateTime GioKetThuc          { get; set; }   // Raw value
        public string   GioBatDauHienThi    { get; set; }   // "Thứ Sáu, 10/01/2025 19:30"
        public string   GioKetThucHienThi   { get; set; }   // "10/01/2025 21:19"
        public decimal  GiaVeCoBan          { get; set; }   // Raw value
        public string   GiaVeHienThi        { get; set; }   // "120,000 VNĐ"
        public string   TrangThai           { get; set; }   // Raw value cho ComboBox
        public string   TrangThaiHienThi    { get; set; }   // Tiếng Việt cho lưới
    }

    // ----------------------------------------------------------
    // DTO: Item cho ComboBox Phim (kèm ThoiLuongPhut để tính giờ)
    // ----------------------------------------------------------
    public class PhimComboItem
    {
        public int    PhimId        { get; set; }
        public string TenPhim       { get; set; }
        public int    ThoiLuongPhut { get; set; }

        // ToString() quyết định text hiển thị trong ComboBox
        public override string ToString()
        {
            return string.Format("{0} ({1} phút)", TenPhim, ThoiLuongPhut);
        }
    }

    // ----------------------------------------------------------
    // DTO: Item cho ComboBox Phong
    // ----------------------------------------------------------
    public class PhongComboItem
    {
        public int    PhongId   { get; set; }
        public string TenPhong  { get; set; }

        public override string ToString()
        {
            return TenPhong;
        }
    }

    public class LichChieuService
    {
        private readonly LichChieuRepository _lichChieuRepo;

        // Hằng số nghiệp vụ: thời gian dọn phòng giữa 2 suất chiếu
        private const int PHUT_DON_PHONG = 15;

        public LichChieuService()
        {
            _lichChieuRepo = new LichChieuRepository();
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH lịch chiếu → List<LichChieuDto>
        // ──────────────────────────────────────────────────────
        public List<LichChieuDto> GetDanhSach()
        {
            try
            {
                List<LichChieuHienThi> rawList = _lichChieuRepo.GetList();
                return ChuyenDoiSangDto(rawList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.GetDanhSach] " + ex.Message);
                return new List<LichChieuDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // TÌM KIẾM theo tên phim
        // ──────────────────────────────────────────────────────
        public List<LichChieuDto> TimKiemTheoTenPhim(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetDanhSach();

            try
            {
                List<LichChieuHienThi> rawList = _lichChieuRepo.SearchByTenPhim(keyword);
                return ChuyenDoiSangDto(rawList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.TimKiemTheoTenPhim] " + ex.Message);
                return new List<LichChieuDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH PHIM cho ComboBox
        // ──────────────────────────────────────────────────────
        public List<PhimComboItem> GetDanhSachPhimChoCombo()
        {
            try
            {
                List<Phim> danhSach = _lichChieuRepo.GetDanhSachPhimDangChieu();
                List<PhimComboItem> result = new List<PhimComboItem>();

                foreach (Phim p in danhSach)
                {
                    result.Add(new PhimComboItem
                    {
                        PhimId        = p.PhimId,
                        TenPhim       = p.TenPhim,
                        ThoiLuongPhut = p.ThoiLuongPhut
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.GetDanhSachPhimChoCombo] " + ex.Message);
                return new List<PhimComboItem>();
            }
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH PHÒNG cho ComboBox
        // ──────────────────────────────────────────────────────
        public List<PhongComboItem> GetDanhSachPhongChoCombo()
        {
            try
            {
                List<PhongChieu> danhSach = _lichChieuRepo.GetDanhSachPhongChieu();
                List<PhongComboItem> result = new List<PhongComboItem>();

                foreach (PhongChieu pc in danhSach)
                {
                    result.Add(new PhongComboItem
                    {
                        PhongId  = pc.PhongId,
                        TenPhong = pc.TenPhong
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.GetDanhSachPhongChoCombo] " + ex.Message);
                return new List<PhongComboItem>();
            }
        }

        // ──────────────────────────────────────────────────────
        // TÍNH GIỜ KẾT THÚC
        // Công thức: GioBatDau + ThoiLuongPhim + 15 phút dọn phòng
        // Exposed ra BLL để GUI gọi khi user chọn phim / chọn giờ
        // ──────────────────────────────────────────────────────
        public DateTime TinhGioKetThuc(DateTime gioBatDau, int thoiLuongPhut)
        {
            return gioBatDau.AddMinutes(thoiLuongPhut + PHUT_DON_PHONG);
        }

        // ──────────────────────────────────────────────────────
        // THÊM LỊCH CHIẾU
        // ──────────────────────────────────────────────────────
        public ServiceResult ThemLichChieu(
            int phimId, int phongId,
            DateTime gioBatDau, int thoiLuongPhut,
            decimal giaVeCoBan, string trangThai)
        {
            // --- Bước 1: Validate đầu vào ---
            ServiceResult validateResult = ValidateDauVao(
                phimId, phongId, gioBatDau, giaVeCoBan, trangThai);
            if (!validateResult.Success)
                return validateResult;

            // --- Bước 2: Tính giờ kết thúc ---
            DateTime gioKetThuc = TinhGioKetThuc(gioBatDau, thoiLuongPhut);

            // --- Bước 3: Kiểm tra trùng lịch phòng chiếu ---
            ServiceResult trungLichResult = KiemTraVaThongBaoTrungLich(
                phongId, gioBatDau, gioKetThuc, 0);
            if (!trungLichResult.Success)
                return trungLichResult;

            try
            {
                LichChieu lichChieuMoi = new LichChieu
                {
                    PhimId      = phimId,
                    PhongId     = phongId,
                    GioBatDau   = gioBatDau,
                    GioKetThuc  = gioKetThuc,
                    GiaVeCoBan  = giaVeCoBan,
                    TrangThai   = trangThai
                };

                int idMoi = _lichChieuRepo.Insert(lichChieuMoi);
                return ServiceResult.Ok(
                    string.Format(
                        "Thêm lịch chiếu thành công! Giờ chiếu: {0} → {1}",
                        gioBatDau.ToString("HH:mm dd/MM"),
                        gioKetThuc.ToString("HH:mm dd/MM")),
                    idMoi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.ThemLichChieu] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi thêm lịch chiếu. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // SỬA LỊCH CHIẾU
        // ──────────────────────────────────────────────────────
        public ServiceResult SuaLichChieu(
            int lichChieuId,
            int phimId, int phongId,
            DateTime gioBatDau, int thoiLuongPhut,
            decimal giaVeCoBan, string trangThai)
        {
            if (lichChieuId <= 0)
                return ServiceResult.Fail("Vui lòng chọn lịch chiếu cần sửa từ danh sách.");

            // --- Kiểm tra đã có vé bán chưa (không cho sửa giờ nếu đã bán) ---
            if (_lichChieuRepo.CoBanVe(lichChieuId))
                return ServiceResult.Fail(
                    "Không thể sửa! Suất chiếu này đã có vé được bán.\n" +
                    "Chỉ được phép đổi trạng thái (VD: Hủy chiếu).");

            ServiceResult validateResult = ValidateDauVao(
                phimId, phongId, gioBatDau, giaVeCoBan, trangThai);
            if (!validateResult.Success)
                return validateResult;

            DateTime gioKetThuc = TinhGioKetThuc(gioBatDau, thoiLuongPhut);

            // Kiểm tra trùng lịch, loại trừ record đang sửa
            ServiceResult trungLichResult = KiemTraVaThongBaoTrungLich(
                phongId, gioBatDau, gioKetThuc, lichChieuId);
            if (!trungLichResult.Success)
                return trungLichResult;

            try
            {
                LichChieu lichChieuCapNhat = new LichChieu
                {
                    LichChieuId = lichChieuId,
                    PhimId      = phimId,
                    PhongId     = phongId,
                    GioBatDau   = gioBatDau,
                    GioKetThuc  = gioKetThuc,
                    GiaVeCoBan  = giaVeCoBan,
                    TrangThai   = trangThai
                };

                bool ok = _lichChieuRepo.Update(lichChieuCapNhat);
                if (!ok)
                    return ServiceResult.Fail("Không tìm thấy lịch chiếu cần sửa.");

                return ServiceResult.Ok("Cập nhật lịch chiếu thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.SuaLichChieu] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi cập nhật. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // XÓA MỀM
        // ──────────────────────────────────────────────────────
        public ServiceResult XoaLichChieu(int lichChieuId)
        {
            if (lichChieuId <= 0)
                return ServiceResult.Fail("Vui lòng chọn lịch chiếu cần xóa.");

            if (_lichChieuRepo.CoBanVe(lichChieuId))
                return ServiceResult.Fail(
                    "Không thể xóa! Suất chiếu này đã có vé được bán.\n" +
                    "Hãy đổi trạng thái sang 'Hủy chiếu' thay vì xóa.");

            try
            {
                bool ok = _lichChieuRepo.SoftDelete(lichChieuId);
                if (!ok)
                    return ServiceResult.Fail("Không tìm thấy lịch chiếu hoặc đã bị xóa trước đó.");

                return ServiceResult.Ok("Xóa lịch chiếu thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.XoaLichChieu] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi xóa. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Validate đầu vào chung
        // ──────────────────────────────────────────────────────
        private ServiceResult ValidateDauVao(
            int phimId, int phongId,
            DateTime gioBatDau, decimal giaVeCoBan, string trangThai)
        {
            if (phimId <= 0)
                return ServiceResult.Fail("Vui lòng chọn phim.");

            if (phongId <= 0)
                return ServiceResult.Fail("Vui lòng chọn phòng chiếu.");

            if (gioBatDau < DateTime.Now.AddMinutes(-5))
                return ServiceResult.Fail(
                    "Giờ chiếu không hợp lệ. Không thể tạo lịch chiếu trong quá khứ.");

            if (giaVeCoBan <= 0)
                return ServiceResult.Fail("Giá vé phải lớn hơn 0 VNĐ.");

            if (giaVeCoBan > 10000000)
                return ServiceResult.Fail("Giá vé không hợp lệ (tối đa 10,000,000 VNĐ).");

            string[] trangThaiHopLe = { "ChuaChieu", "DangChieu", "DaKetThuc", "HuyChieu" };
            bool ttOk = false;
            foreach (string tt in trangThaiHopLe)
            {
                if (tt == trangThai) { ttOk = true; break; }
            }
            if (!ttOk)
                return ServiceResult.Fail("Trạng thái lịch chiếu không hợp lệ.");

            return ServiceResult.Ok();
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Kiểm tra và tạo thông báo trùng lịch
        // Đây là logic nghiệp vụ quan trọng nhất của module này
        // ──────────────────────────────────────────────────────
        private ServiceResult KiemTraVaThongBaoTrungLich(
            int phongId, DateTime gioBatDau, DateTime gioKetThuc, int excludeId)
        {
            try
            {
                bool biTrung = _lichChieuRepo.KiemTraTrungLich(
                    phongId, gioBatDau, gioKetThuc, excludeId);

                if (biTrung)
                {
                    return ServiceResult.Fail(
                        string.Format(
                            "TRÙNG LỊCH! Phòng này đã có suất chiếu trong khung giờ {0} – {1}.\n" +
                            "Vui lòng chọn khung giờ khác hoặc phòng chiếu khác.",
                            gioBatDau.ToString("HH:mm dd/MM/yyyy"),
                            gioKetThuc.ToString("HH:mm dd/MM/yyyy")));
                }

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LichChieuService.KiemTraVaThongBaoTrungLich] " + ex.Message);
                return ServiceResult.Fail("Lỗi kiểm tra trùng lịch. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Chuyển đổi raw list sang DTO
        // ──────────────────────────────────────────────────────
        private List<LichChieuDto> ChuyenDoiSangDto(List<LichChieuHienThi> rawList)
        {
            List<LichChieuDto> result = new List<LichChieuDto>();

            foreach (LichChieuHienThi lc in rawList)
            {
                string trangThaiHienThi;
                switch (lc.TrangThai)
                {
                    case "ChuaChieu":   trangThaiHienThi = "Chưa chiếu";    break;
                    case "DangChieu":   trangThaiHienThi = "Đang chiếu";    break;
                    case "DaKetThuc":   trangThaiHienThi = "Đã kết thúc";   break;
                    case "HuyChieu":    trangThaiHienThi = "Hủy chiếu";     break;
                    default:            trangThaiHienThi = lc.TrangThai;    break;
                }

                result.Add(new LichChieuDto
                {
                    LichChieuId         = lc.LichChieuId,
                    PhimId              = lc.PhimId,
                    PhongId             = lc.PhongId,
                    TenPhim             = lc.TenPhim,
                    TenPhong            = lc.TenPhong,
                    GioBatDau           = lc.GioBatDau,
                    GioKetThuc          = lc.GioKetThuc,
                    GioBatDauHienThi    = lc.GioBatDau.ToString("HH:mm  dd/MM/yyyy"),
                    GioKetThucHienThi   = lc.GioKetThuc.ToString("HH:mm  dd/MM/yyyy"),
                    GiaVeCoBan          = lc.GiaVeCoBan,
                    GiaVeHienThi        = lc.GiaVeCoBan.ToString("N0") + " ₫",
                    TrangThai           = lc.TrangThai,
                    TrangThaiHienThi    = trangThaiHienThi
                });
            }

            return result;
        }

        public void Dispose()
        {
            _lichChieuRepo.Dispose();
        }
    }
}
