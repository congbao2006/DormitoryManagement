const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

function khongChoSinhVienSuaXoa(req, res, next) {
  if (req.session?.user?.role === "SinhVien") {
    return res.status(403).render("dashboard/error", {
      title: "Khong du quyen",
      message: "Sinh vien khong duoc sua hoac xoa yeu cau sua chua."
    });
  }

  return next();
}

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/YeuCauSuaChuas");
    res.render("yeucau/index", { title: "Quan ly Yeu cau sua chua", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const query = req.body.maSinhVien ? `?maSinhVien=${Number(req.body.maSinhVien)}` : "";

    await api.post(`/YeuCauSuaChuas${query}`, {
      maYeuCau: Number(req.body.maYeuCau),
      moTa: req.body.moTa,
      ngayYeuCau: req.body.ngayYeuCau,
      trangThai: req.body.trangThai
    });
    res.redirect("/yeucau");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", khongChoSinhVienSuaXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/YeuCauSuaChuas/${req.params.id}`);
    res.render("yeucau/edit", { title: "Cap nhat Yeu cau", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", khongChoSinhVienSuaXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/YeuCauSuaChuas/${req.params.id}`, {
      maYeuCau: Number(req.params.id),
      moTa: req.body.moTa,
      ngayYeuCau: req.body.ngayYeuCau,
      trangThai: req.body.trangThai
    });
    res.redirect("/yeucau");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", khongChoSinhVienSuaXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/YeuCauSuaChuas/${req.params.id}`);
    res.redirect("/yeucau");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
