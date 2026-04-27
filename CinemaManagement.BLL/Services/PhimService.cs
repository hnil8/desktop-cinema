using System;
using System.Collections.Generic;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.BLL/Services/PhimService.cs
// Tầng BLL - Xử lý nghiệp vụ và validate dữ liệu
// KHÔNG biết gì về SQL, chỉ gọi Repository
// KHÔNG biết gì về UI, trả về DTO/bool + message
// ============================================================

namespace CinemaManagement.BLL.Services
{
    // ----------------------------------------------------------
    // DTO: Kết quả trả về sau mỗi thao tác nghiệp vụ
    // Giúp GUI biết thành công/thất bại mà không cần exception
    // ----------------------------------------------------------
    public class ServiceResult
    {
        public bool   Success   { get; set; }
        public string Message   { get; set; }
        public object Data      { get; set; } // Dữ liệu kèm theo (tùy thao tác)

        public static ServiceResult Ok(string message = "Thành công.", object data = null)
        {
            return new ServiceResult { Success = true,  Message = message, Data = data };
        }

        public static ServiceResult Fail(string message)
        {
            return new ServiceResult { Success = false, Message = message, Data = null };
        }
    }

    // ----------------------------------------------------------
    // DTO hiển thị phim trên DataGridView (phẳng hóa dữ liệu)
    // ----------------------------------------------------------
    public class PhimDto
    {
        public int      PhimId          { get; set; }
        public string   TenPhim         { get; set; }
        public string   DaoDien         { get; set; }
        public int      ThoiLuongPhut   { get; set; }
        public string   GioiHanDoTuoi   { get; set; }
        public string   TrangThai       { get; set; }
        public string   TrangThaiHienThi { get; set; } // Tiếng Việt để hiện lên lưới
        public DateTime? NgayKhoiChieu  { get; set; }
        public string   NgayKhoiChieuHienThi { get; set; }
        public string   NgonNgu         { get; set; }
        public string   NuocSanXuat     { get; set; }
        public string   TenGoc          { get; set; }
        public string   DienVienChinh   { get; set; }
        public string   MoTa            { get; set; }
    }

    public class PhimService
    {
        private readonly PhimRepository _phimRepo;

        public PhimService()
        {
            _phimRepo = new PhimRepository();
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH → Trả về List<PhimDto> cho DataGridView
        // ──────────────────────────────────────────────────────
        public List<PhimDto> GetDanhSach()
        {
            try
            {
                var danhSach = _phimRepo.GetList();
                return ChuyenDoiSangDto(danhSach);
            }
            catch (Exception ex)
            {
                // Ghi log, trả về list rỗng để GUI không bị crash
                Console.WriteLine("[PhimService.GetDanhSach] " + ex.Message);
                return new List<PhimDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // TÌM KIẾM theo tên
        // ──────────────────────────────────────────────────────
        public List<PhimDto> TimKiemTheoTen(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetDanhSach();

            try
            {
                var ketQua = _phimRepo.SearchByTen(keyword);
                return ChuyenDoiSangDto(ketQua);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PhimService.TimKiemTheoTen] " + ex.Message);
                return new List<PhimDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // LẤY THEO ID (dùng để điền lại form khi click lưới)
        // ──────────────────────────────────────────────────────
        public DAL.Phim GetById(int phimId)
        {
            try
            {
                return _phimRepo.GetById(phimId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PhimService.GetById] " + ex.Message);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────
        // THÊM MỚI
        // Validate → Kiểm tra trùng tên → Gọi DAL Insert
        // ──────────────────────────────────────────────────────
        public ServiceResult ThemPhim(
            string tenPhim, string tenGoc, string daoDien,
            string dienVienChinh, int thoiLuongPhut,
            string nuocSanXuat, int? namPhatHanh,
            string gioiHanDoTuoi, string ngonNgu,
            string moTa, string posterUrl,
            string trangThai, DateTime? ngayKhoiChieu)
        {
            // --- Bước 1: Validate bắt buộc ---
            ServiceResult validateResult = ValidateDauVao(
                tenPhim, thoiLuongPhut, gioiHanDoTuoi, trangThai);
            if (!validateResult.Success)
                return validateResult;

            // --- Bước 2: Kiểm tra trùng tên ---
            if (_phimRepo.IsTenPhimTonTai(tenPhim, 0))
                return ServiceResult.Fail(
                    string.Format("Phim '{0}' đã tồn tại trong hệ thống.", tenPhim.Trim()));

            try
            {
                // --- Bước 3: Tạo entity và lưu ---
                var phimMoi = new DAL.Phim
                {
                    TenPhim         = tenPhim.Trim(),
                    TenGoc          = string.IsNullOrWhiteSpace(tenGoc)         ? null : tenGoc.Trim(),
                    DaoDien         = string.IsNullOrWhiteSpace(daoDien)        ? null : daoDien.Trim(),
                    DienVienChinh   = string.IsNullOrWhiteSpace(dienVienChinh)  ? null : dienVienChinh.Trim(),
                    ThoiLuongPhut   = thoiLuongPhut,
                    NuocSanXuat     = string.IsNullOrWhiteSpace(nuocSanXuat)    ? null : nuocSanXuat.Trim(),
                    NamPhatHanh     = namPhatHanh,
                    GioiHanDoTuoi   = gioiHanDoTuoi,
                    NgonNgu         = string.IsNullOrWhiteSpace(ngonNgu)        ? "VietSub" : ngonNgu,
                    MoTa            = string.IsNullOrWhiteSpace(moTa)           ? null : moTa.Trim(),
                    PosterUrl       = string.IsNullOrWhiteSpace(posterUrl)      ? null : posterUrl.Trim(),
                    TrangThai       = trangThai,
                    NgayKhoiChieu   = ngayKhoiChieu
                };

                int idMoi = _phimRepo.Insert(phimMoi);
                return ServiceResult.Ok(
                    string.Format("Thêm phim '{0}' thành công!", tenPhim.Trim()),
                    idMoi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PhimService.ThemPhim] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi thêm phim. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // SỬA
        // ──────────────────────────────────────────────────────
        public ServiceResult SuaPhim(
            int phimId,
            string tenPhim, string tenGoc, string daoDien,
            string dienVienChinh, int thoiLuongPhut,
            string nuocSanXuat, int? namPhatHanh,
            string gioiHanDoTuoi, string ngonNgu,
            string moTa, string posterUrl,
            string trangThai, DateTime? ngayKhoiChieu)
        {
            if (phimId <= 0)
                return ServiceResult.Fail("Vui lòng chọn phim cần sửa từ danh sách.");

            // --- Validate ---
            ServiceResult validateResult = ValidateDauVao(
                tenPhim, thoiLuongPhut, gioiHanDoTuoi, trangThai);
            if (!validateResult.Success)
                return validateResult;

            // --- Kiểm tra trùng tên (loại trừ chính nó) ---
            if (_phimRepo.IsTenPhimTonTai(tenPhim, phimId))
                return ServiceResult.Fail(
                    string.Format("Đã tồn tại phim khác tên '{0}'.", tenPhim.Trim()));

            try
            {
                var phimCapNhat = new DAL.Phim
                {
                    PhimId          = phimId,
                    TenPhim         = tenPhim.Trim(),
                    TenGoc          = string.IsNullOrWhiteSpace(tenGoc)         ? null : tenGoc.Trim(),
                    DaoDien         = string.IsNullOrWhiteSpace(daoDien)        ? null : daoDien.Trim(),
                    DienVienChinh   = string.IsNullOrWhiteSpace(dienVienChinh)  ? null : dienVienChinh.Trim(),
                    ThoiLuongPhut   = thoiLuongPhut,
                    NuocSanXuat     = string.IsNullOrWhiteSpace(nuocSanXuat)    ? null : nuocSanXuat.Trim(),
                    NamPhatHanh     = namPhatHanh,
                    GioiHanDoTuoi   = gioiHanDoTuoi,
                    NgonNgu         = string.IsNullOrWhiteSpace(ngonNgu)        ? "VietSub" : ngonNgu,
                    MoTa            = string.IsNullOrWhiteSpace(moTa)           ? null : moTa.Trim(),
                    PosterUrl       = string.IsNullOrWhiteSpace(posterUrl)      ? null : posterUrl.Trim(),
                    TrangThai       = trangThai,
                    NgayKhoiChieu   = ngayKhoiChieu
                };

                bool ok = _phimRepo.Update(phimCapNhat);
                if (!ok)
                    return ServiceResult.Fail("Không tìm thấy phim cần sửa hoặc phim đã bị xóa.");

                return ServiceResult.Ok(
                    string.Format("Cập nhật phim '{0}' thành công!", tenPhim.Trim()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PhimService.SuaPhim] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi cập nhật phim. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // XÓA MỀM
        // Cảnh báo nếu phim đang có lịch chiếu
        // ──────────────────────────────────────────────────────
        public ServiceResult XoaPhim(int phimId)
        {
            if (phimId <= 0)
                return ServiceResult.Fail("Vui lòng chọn phim cần xóa từ danh sách.");

            try
            {
                // Cảnh báo nghiệp vụ: phim đang có lịch chiếu
                if (_phimRepo.CoLichChieu(phimId))
                    return ServiceResult.Fail(
                        "Không thể xóa! Phim này đang có lịch chiếu đã lên kế hoạch.\n" +
                        "Hãy hủy lịch chiếu trước hoặc đổi trạng thái sang 'Ngừng chiếu'.");

                bool ok = _phimRepo.SoftDelete(phimId);
                if (!ok)
                    return ServiceResult.Fail("Không tìm thấy phim hoặc phim đã bị xóa trước đó.");

                return ServiceResult.Ok("Xóa phim thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PhimService.XoaPhim] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi xóa phim. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Validate đầu vào chung cho Thêm và Sửa
        // ──────────────────────────────────────────────────────
        private ServiceResult ValidateDauVao(
            string tenPhim, int thoiLuongPhut,
            string gioiHanDoTuoi, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(tenPhim))
                return ServiceResult.Fail("Tên phim không được để trống.");

            if (tenPhim.Trim().Length > 200)
                return ServiceResult.Fail("Tên phim không được vượt quá 200 ký tự.");

            if (thoiLuongPhut <= 0)
                return ServiceResult.Fail("Thời lượng phim phải lớn hơn 0 phút.");

            if (thoiLuongPhut > 600)
                return ServiceResult.Fail("Thời lượng phim không hợp lệ (tối đa 600 phút).");

            string[] doTuoiHopLe = { "P", "C13", "C16", "C18" };
            bool doTuoiOk = false;
            foreach (string dt in doTuoiHopLe)
            {
                if (dt == gioiHanDoTuoi) { doTuoiOk = true; break; }
            }
            if (!doTuoiOk)
                return ServiceResult.Fail("Giới hạn độ tuổi không hợp lệ.");

            string[] trangThaiHopLe = { "SapChieu", "DangChieu", "NgungChieu" };
            bool ttOk = false;
            foreach (string tt in trangThaiHopLe)
            {
                if (tt == trangThai) { ttOk = true; break; }
            }
            if (!ttOk)
                return ServiceResult.Fail("Trạng thái phim không hợp lệ.");

            return ServiceResult.Ok();
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Chuyển đổi List<Phim> entity sang List<PhimDto>
        // ──────────────────────────────────────────────────────
        private List<PhimDto> ChuyenDoiSangDto(List<DAL.Phim> danhSach)
        {
            var result = new List<PhimDto>();

            foreach (DAL.Phim p in danhSach)
            {
                string trangThaiHienThi;
                switch (p.TrangThai)
                {
                    case "SapChieu":    trangThaiHienThi = "Sắp chiếu";    break;
                    case "DangChieu":   trangThaiHienThi = "Đang chiếu";   break;
                    case "NgungChieu":  trangThaiHienThi = "Ngừng chiếu";  break;
                    default:            trangThaiHienThi = p.TrangThai;    break;
                }

                string ngayKhoiChieuHienThi = p.NgayKhoiChieu.HasValue
                    ? p.NgayKhoiChieu.Value.ToString("dd/MM/yyyy")
                    : "---";

                result.Add(new PhimDto
                {
                    PhimId                  = p.PhimId,
                    TenPhim                 = p.TenPhim,
                    TenGoc                  = p.TenGoc ?? "",
                    DaoDien                 = p.DaoDien ?? "",
                    DienVienChinh           = p.DienVienChinh ?? "",
                    ThoiLuongPhut           = p.ThoiLuongPhut,
                    NuocSanXuat             = p.NuocSanXuat ?? "",
                    GioiHanDoTuoi           = p.GioiHanDoTuoi,
                    NgonNgu                 = p.NgonNgu ?? "VietSub",
                    TrangThai               = p.TrangThai,
                    TrangThaiHienThi        = trangThaiHienThi,
                    NgayKhoiChieu           = p.NgayKhoiChieu,
                    NgayKhoiChieuHienThi    = ngayKhoiChieuHienThi,
                    MoTa                    = p.MoTa ?? ""
                });
            }

            return result;
        }

        public void Dispose()
        {
            _phimRepo.Dispose();
        }
    }
}
