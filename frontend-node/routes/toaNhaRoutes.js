const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/ToaNhas");
    res.render("toanha/index", { title: "Quan ly Toa nha", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/ToaNhas", {
      maToa: Number(req.body.maToa),
      tenToa: req.body.tenToa,
      soTang: Number(req.body.soTang)
    });
    res.redirect("/toanha");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/ToaNhas/${req.params.id}`);
    res.render("toanha/edit", { title: "Cap nhat Toa nha", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/ToaNhas/${req.params.id}`, {
      maToa: Number(req.params.id),
      tenToa: req.body.tenToa,
      soTang: Number(req.body.soTang)
    });
    res.redirect("/toanha");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/ToaNhas/${req.params.id}`);
    res.redirect("/toanha");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
