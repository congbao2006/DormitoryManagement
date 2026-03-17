const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/Phongs");
    res.render("phong/index", { title: "Quan ly Phong", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/Phongs", {
      maPhong: Number(req.body.maPhong),
      soPhong: req.body.soPhong,
      loaiPhong: req.body.loaiPhong,
      sucChua: Number(req.body.sucChua),
      giaPhong: Number(req.body.giaPhong),
      trangThai: req.body.trangThai
    });
    res.redirect("/phong");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/Phongs/${req.params.id}`);
    res.render("phong/edit", { title: "Cap nhat Phong", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/Phongs/${req.params.id}`, {
      maPhong: Number(req.params.id),
      soPhong: req.body.soPhong,
      loaiPhong: req.body.loaiPhong,
      sucChua: Number(req.body.sucChua),
      giaPhong: Number(req.body.giaPhong),
      trangThai: req.body.trangThai
    });
    res.redirect("/phong");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/Phongs/${req.params.id}`);
    res.redirect("/phong");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
