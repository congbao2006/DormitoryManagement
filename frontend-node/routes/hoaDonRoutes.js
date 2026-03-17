const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

function khongChoSinhVienGhi(req, res, next) {
  if (req.session?.user?.role === "SinhVien") {
    return res.status(403).render("dashboard/error", {
      title: "Khong du quyen",
      message: "Sinh vien khong duoc thay doi hoa don."
    });
  }

  return next();
}

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/HoaDons");
    res.render("hoadon/index", { title: "Quan ly Hoa don", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/HoaDons", {
      maHoaDon: Number(req.body.maHoaDon),
      ngayThanhToan: req.body.ngayThanhToan,
      tongTien: Number(req.body.tongTien),
      trangThai: req.body.trangThai
    });
    res.redirect("/hoadon");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/HoaDons/${req.params.id}`);
    res.render("hoadon/edit", { title: "Cap nhat Hoa don", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/HoaDons/${req.params.id}`, {
      maHoaDon: Number(req.params.id),
      ngayThanhToan: req.body.ngayThanhToan,
      tongTien: Number(req.body.tongTien),
      trangThai: req.body.trangThai
    });
    res.redirect("/hoadon");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/HoaDons/${req.params.id}`);
    res.redirect("/hoadon");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
