using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.BLL/Services/NhanVienService.cs
// Tầng BLL: Nghiệp vụ quản lý nhân viên và ca làm việc
// Tái dùng class ServiceResult đã định nghĩa trong PhimService.cs
// ============================================================

namespace CinemaManagement.BLL.Services
{
    // ----------------------------------------------------------
    // DTO: Dữ liệu nhân viên hiển thị trên DataGridView
    // Phẳng hóa navigation properties (VaiTro → TenVaiTro)
    // ----------------------------------------------------------
    public class NhanVienDto
    {
        public int NhanVienId { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string NgayVaoLam { get; set; }  // Formatted string
        public string TenVaiTro { get; set; }  // Từ navigation TaiKhoan.VaiTro
        public string TrangThaiTK { get; set; }  // "Hoạt động" / "Chưa có TK"
    }

    // ----------------------------------------------------------
    // DTO: Trạng thái ca làm việc hiển thị trên panel phải
    // ----------------------------------------------------------
    public class TrangThaiCaDto
    {
        public bool DangMoCa { get; set; }
        public int CaId { get; set; }
        public string ThoiGianMoCa { get; set; }  // "07:30 - 01/06/2025"
        public decimal TienDauCa { get; set; }
        public decimal TongThuTienMat { get; set; }
        public decimal TongThuChuyenKhoan { get; set; }
        public decimal TongThuThe { get; set; }
        public decimal TongThuTatCa { get; set; }
        public string MoTaTrangThai { get; set; }  // Text hiển thị lên UI
    }

    public class NhanVienService
    {
        private readonly NhanVienRepository _nhanVienRepo;

        public NhanVienService()
        {
            _nhanVienRepo = new NhanVienRepository();
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH NHÂN VIÊN → List<NhanVienDto>
        // ──────────────────────────────────────────────────────
        public List<NhanVienDto> GetDanhSach()
        {
            try
            {
                List<DAL.NhanVien> list = _nhanVienRepo.GetList();
                return ChuyenDoiSangDto(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.GetDanhSach] " + ex.Message);
                return new List<NhanVienDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // TÌM KIẾM theo tên hoặc SĐT
        // ──────────────────────────────────────────────────────
        public List<NhanVienDto> TimKiem(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetDanhSach();

            try
            {
                List<DAL.NhanVien> list = _nhanVienRepo.Search(keyword);
                return ChuyenDoiSangDto(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.TimKiem] " + ex.Message);
                return new List<NhanVienDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // LẤY THEO ID (điền lại form khi click lưới)
        // ──────────────────────────────────────────────────────
        public DAL.NhanVien GetById(int nhanVienId)
        {
            try
            {
                return _nhanVienRepo.GetById(nhanVienId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.GetById] " + ex.Message);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────
        // THÊM NHÂN VIÊN
        // ──────────────────────────────────────────────────────
        public ServiceResult ThemNhanVien(
            string hoTen, string soDienThoai, string email,
            string gioiTinh, DateTime? ngaySinh,
            string diaChi, DateTime ngayVaoLam)
        {
            ServiceResult validateResult = ValidateDauVao(hoTen, soDienThoai, email);
            if (!validateResult.Success)
                return validateResult;

            if (_nhanVienRepo.IsSoDienThoaiTonTai(soDienThoai, 0))
                return ServiceResult.Fail(
                    string.Format("Số điện thoại '{0}' đã được đăng ký cho nhân viên khác.", soDienThoai.Trim()));

            try
            {
                DAL.NhanVien nv = new DAL.NhanVien
                {
                    HoTen = hoTen.Trim(),
                    SoDienThoai = soDienThoai.Trim(),
                    Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                    GioiTinh = string.IsNullOrWhiteSpace(gioiTinh) ? null : gioiTinh,
                    NgaySinh = ngaySinh,
                    DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi.Trim(),
                    NgayVaoLam = ngayVaoLam
                };

                int idMoi = _nhanVienRepo.Insert(nv);
                return ServiceResult.Ok(
                    string.Format("Thêm nhân viên '{0}' thành công!", hoTen.Trim()), idMoi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.ThemNhanVien] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi thêm nhân viên. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // SỬA NHÂN VIÊN
        // ──────────────────────────────────────────────────────
        public ServiceResult SuaNhanVien(
            int nhanVienId,
            string hoTen, string soDienThoai, string email,
            string gioiTinh, DateTime? ngaySinh,
            string diaChi, DateTime ngayVaoLam)
        {
            if (nhanVienId <= 0)
                return ServiceResult.Fail("Vui lòng chọn nhân viên cần sửa từ danh sách.");

            ServiceResult validateResult = ValidateDauVao(hoTen, soDienThoai, email);
            if (!validateResult.Success)
                return validateResult;

            if (_nhanVienRepo.IsSoDienThoaiTonTai(soDienThoai, nhanVienId))
                return ServiceResult.Fail(
                    string.Format("Số điện thoại '{0}' đã được đăng ký cho nhân viên khác.", soDienThoai.Trim()));

            try
            {
                DAL.NhanVien nvCapNhat = new DAL.NhanVien
                {
                    NhanVienId = nhanVienId,
                    HoTen = hoTen.Trim(),
                    SoDienThoai = soDienThoai.Trim(),
                    Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                    GioiTinh = string.IsNullOrWhiteSpace(gioiTinh) ? null : gioiTinh,
                    NgaySinh = ngaySinh,
                    DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi.Trim(),
                    NgayVaoLam = ngayVaoLam
                };

                bool ok = _nhanVienRepo.Update(nvCapNhat);
                if (!ok)
                    return ServiceResult.Fail("Không tìm thấy nhân viên cần sửa.");

                return ServiceResult.Ok(string.Format("Cập nhật nhân viên '{0}' thành công!", hoTen.Trim()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.SuaNhanVien] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi cập nhật. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // XÓA MỀM NHÂN VIÊN
        // ──────────────────────────────────────────────────────
        public ServiceResult XoaNhanVien(int nhanVienId)
        {
            if (nhanVienId <= 0)
                return ServiceResult.Fail("Vui lòng chọn nhân viên cần xóa.");

            if (_nhanVienRepo.CoTaiKhoan(nhanVienId))
                return ServiceResult.Fail(
                    "Không thể xóa! Nhân viên này đang có tài khoản đăng nhập.\n" +
                    "Hãy vô hiệu hóa tài khoản trước rồi mới xóa nhân viên.");

            try
            {
                bool ok = _nhanVienRepo.SoftDelete(nhanVienId);
                if (!ok)
                    return ServiceResult.Fail("Không tìm thấy nhân viên hoặc đã bị xóa trước đó.");

                return ServiceResult.Ok("Xóa nhân viên thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.XoaNhanVien] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi xóa. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // KIỂM TRA TRẠNG THÁI CA LÀM VIỆC
        // ──────────────────────────────────────────────────────
        public TrangThaiCaDto GetTrangThaiCa(int nhanVienId)
        {
            try
            {
                DAL.CaLamViec ca = _nhanVienRepo.GetCaDangMo(nhanVienId);

                if (ca == null)
                {
                    return new TrangThaiCaDto
                    {
                        DangMoCa = false,
                        CaId = 0,
                        MoTaTrangThai = "Chưa mở ca làm việc"
                    };
                }

                decimal tongTatCa = ca.TongThuTienMat
                                  + ca.TongThuChuyenKhoan
                                  + ca.TongThuThe;

                return new TrangThaiCaDto
                {
                    DangMoCa = true,
                    CaId = ca.CaId,
                    TienDauCa = ca.TienDauCa,
                    ThoiGianMoCa = ca.ThoiGianMoCa.ToString("HH:mm  dd/MM/yyyy"),
                    TongThuTienMat = ca.TongThuTienMat,
                    TongThuChuyenKhoan = ca.TongThuChuyenKhoan,
                    TongThuThe = ca.TongThuThe,
                    TongThuTatCa = tongTatCa,
                    MoTaTrangThai = string.Format(
                        "Đang mở ca từ {0}", ca.ThoiGianMoCa.ToString("HH:mm"))
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.GetTrangThaiCa] " + ex.Message);
                return new TrangThaiCaDto { DangMoCa = false, MoTaTrangThai = "Lỗi đọc trạng thái ca" };
            }
        }

        // ──────────────────────────────────────────────────────
        // MỞ CA LÀM VIỆC
        // ──────────────────────────────────────────────────────
        public ServiceResult MoCa(int nhanVienId, decimal tienDauCa)
        {
            if (nhanVienId <= 0)
                return ServiceResult.Fail("Không xác định được nhân viên. Vui lòng đăng nhập lại.");

            if (tienDauCa < 0)
                return ServiceResult.Fail("Tiền đầu ca không được âm.");

            // Kiểm tra đã có ca đang mở chưa
            DAL.CaLamViec caDangMo = _nhanVienRepo.GetCaDangMo(nhanVienId);
            if (caDangMo != null)
                return ServiceResult.Fail(
                    string.Format(
                        "Đã có ca đang mở từ {0}.\nVui lòng đóng ca hiện tại trước.",
                        caDangMo.ThoiGianMoCa.ToString("HH:mm dd/MM/yyyy")));

            try
            {
                DAL.CaLamViec caMoi = new DAL.CaLamViec
                {
                    NhanVienId = nhanVienId,
                    TienDauCa = tienDauCa
                };

                int caId = _nhanVienRepo.MoCa(caMoi);
                return ServiceResult.Ok(
                    string.Format("Đã mở ca làm việc lúc {0}. Tiền đầu ca: {1} ₫",
                        DateTime.Now.ToString("HH:mm"),
                        tienDauCa.ToString("N0")),
                    caId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.MoCa] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi mở ca. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // ĐÓNG CA & CHỐT DOANH THU
        // Doanh thu được đọc từ DB (đã cập nhật tự động khi bán vé)
        // BLL chỉ ghi lại thời gian và ghi chú chốt ca
        // ──────────────────────────────────────────────────────
        public ServiceResult DongCa(int nhanVienId, string ghiChu)
        {
            if (nhanVienId <= 0)
                return ServiceResult.Fail("Không xác định được nhân viên.");

            DAL.CaLamViec ca = _nhanVienRepo.GetCaDangMo(nhanVienId);
            if (ca == null)
                return ServiceResult.Fail("Không tìm thấy ca đang mở. Vui lòng kiểm tra lại.");

            try
            {
                bool ok = _nhanVienRepo.DongCa(
                    ca.CaId,
                    ca.TongThuTienMat,
                    ca.TongThuChuyenKhoan,
                    ca.TongThuThe,
                    string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu.Trim());

                if (!ok)
                    return ServiceResult.Fail("Không thể đóng ca. Vui lòng thử lại.");

                decimal tongThu = ca.TongThuTienMat
                                                + ca.TongThuChuyenKhoan
                                                + ca.TongThuThe;

                // Tính số tiền thực tế trong két
                decimal tienMatBanGiao = ca.TienDauCa + ca.TongThuTienMat;

                return ServiceResult.Ok(
                    string.Format(
                        "Đã chốt ca lúc {0}.\nDoanh thu: {1} ₫ | Đã bàn giao két: {2} ₫",
                        DateTime.Now.ToString("HH:mm dd/MM/yyyy"),
                        tongThu.ToString("N0"),
                        tienMatBanGiao.ToString("N0")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NhanVienService.DongCa] " + ex.Message);
                return ServiceResult.Fail("Lỗi hệ thống khi đóng ca. Vui lòng thử lại.");
            }
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Validate đầu vào nhân viên
        // ──────────────────────────────────────────────────────
        private ServiceResult ValidateDauVao(string hoTen, string soDienThoai, string email)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
                return ServiceResult.Fail("Họ tên không được để trống.");

            if (hoTen.Trim().Length < 2 || hoTen.Trim().Length > 100)
                return ServiceResult.Fail("Họ tên phải từ 2 đến 100 ký tự.");

            if (string.IsNullOrWhiteSpace(soDienThoai))
                return ServiceResult.Fail("Số điện thoại không được để trống.");

            // Validate SĐT Việt Nam: 10 số, bắt đầu bằng 0
            bool sdtHopLe = Regex.IsMatch(soDienThoai.Trim(), @"^0[0-9]{9}$");
            if (!sdtHopLe)
                return ServiceResult.Fail("Số điện thoại không hợp lệ (phải là 10 số, bắt đầu bằng 0).");

            // Validate email nếu có nhập
            if (!string.IsNullOrWhiteSpace(email))
            {
                bool emailHopLe = Regex.IsMatch(email.Trim(),
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailHopLe)
                    return ServiceResult.Fail("Địa chỉ email không đúng định dạng.");
            }

            return ServiceResult.Ok();
        }

        // ──────────────────────────────────────────────────────
        // PRIVATE: Chuyển đổi Entity sang DTO
        // ──────────────────────────────────────────────────────
        private List<NhanVienDto> ChuyenDoiSangDto(List<DAL.NhanVien> list)
        {
            List<NhanVienDto> result = new List<NhanVienDto>();

            foreach (DAL.NhanVien nv in list)
            {
                // Lấy TenVaiTro từ navigation property (nếu có)
                string tenVaiTro = "Chưa có TK";
                string trangThaiTK = "Chưa có TK";

                if (nv.TaiKhoans != null && nv.TaiKhoans.Count > 0)
                {
                    DAL.TaiKhoan tk = null;
                    foreach (DAL.TaiKhoan t in nv.TaiKhoans)
                    {
                        if (t.IsDeleted == false) { tk = t; break; }
                    }
                    if (tk != null)
                    {
                        tenVaiTro = tk.VaiTro != null ? tk.VaiTro.TenVaiTro : "---";
                        trangThaiTK = tk.IsActive == true ? "Hoạt động" : "Bị khóa";
                    }
                }

                result.Add(new NhanVienDto
                {
                    NhanVienId = nv.NhanVienId,
                    HoTen = nv.HoTen,
                    SoDienThoai = nv.SoDienThoai ?? "",
                    Email = nv.Email ?? "",
                    GioiTinh = nv.GioiTinh ?? "",
                    NgayVaoLam = nv.NgayVaoLam != null
                                    ? nv.NgayVaoLam.ToString("dd/MM/yyyy")
                                    : "---",
                    TenVaiTro = tenVaiTro,
                    TrangThaiTK = trangThaiTK
                });
            }

            return result;
        }

        public void Dispose()
        {
            _nhanVienRepo.Dispose();
        }
    }
}