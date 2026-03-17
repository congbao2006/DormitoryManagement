const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

function khongChoSinhVienSuaXoa(req, res, next) {
  if (req.session?.user?.role === "SinhVien") {
    return res.status(403).render("dashboard/error", {
      title: "Khong du quyen",
      message: "Sinh vien khong duoc sua hoac xoa thanh toan."
    });
  }

  return next();
}

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/ThanhToans");
    res.render("thanhtoan/index", { title: "Quan ly Thanh toan", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post(`/ThanhToans?maHoaDon=${Number(req.body.maHoaDon)}`, {
      maThanhToan: Number(req.body.maThanhToan),
      ngayThanhToan: req.body.ngayThanhToan,
      soTien: Number(req.body.soTien),
      phuongThuc: req.body.phuongThuc
    });
    res.redirect("/thanhtoan");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", khongChoSinhVienSuaXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/ThanhToans/${req.params.id}`);
    res.render("thanhtoan/edit", { title: "Cap nhat Thanh toan", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", khongChoSinhVienSuaXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/ThanhToans/${req.params.id}`, {
      maThanhToan: Number(req.params.id),
      ngayThanhToan: req.body.ngayThanhToan,
      soTien: Number(req.body.soTien),
      phuongThuc: req.body.phuongThuc
    });
    res.redirect("/thanhtoan");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", khongChoSinhVienSuaXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/ThanhToans/${req.params.id}`);
    res.redirect("/thanhtoan");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
