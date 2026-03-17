const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

function renderLogin(res, payload = {}) {
  return res.render("auth/login", {
    title: "Đăng nhập",
    error: null,
    success: null,
    form: { email: "" },
    ...payload
  });
}

function renderRegister(res, payload = {}) {
  return res.render("auth/register", {
    title: "Đăng ký",
    error: null,
    form: {
      hoTen: "",
      email: "",
      ngaySinh: "",
      gioiTinh: "Nam",
      soDienThoai: ""
    },
    ...payload
  });
}

router.get("/login", (req, res) => {
  if (req.session?.token) {
    return res.redirect("/");
  }

  return renderLogin(res, { success: req.query.success || null });
});

router.post("/login", async (req, res) => {
  try {
    const api = createApiClient();
    const { data } = await api.post("/Auth/login", {
      email: req.body.email,
      matKhau: req.body.matKhau
    });

    req.session.token = data.accessToken;
    req.session.user = {
      id: data.id,
      email: data.email,
      role: data.role,
      hoTen: data.hoTen
    };

    return res.redirect("/");
  } catch (error) {
    const message = error?.response?.data?.message || "Đăng nhập thất bại.";
    return res.status(401).render("auth/login", {
      title: "Đăng nhập",
      error: message,
      success: null,
      form: {
        email: req.body.email || ""
      }
    });
  }
});

router.get("/register", (req, res) => {
  if (req.session?.token) {
    return res.redirect("/");
  }

  return renderRegister(res);
});

router.post("/register", async (req, res) => {
  try {
    const body = {
      hoTen: req.body.hoTen,
      email: req.body.email,
      matKhau: req.body.matKhau,
      xacNhanMatKhau: req.body.xacNhanMatKhau,
      ngaySinh: req.body.ngaySinh,
      gioiTinh: req.body.gioiTinh,
      soDienThoai: req.body.soDienThoai
    };

    if (!body.hoTen || !body.email || !body.matKhau || !body.xacNhanMatKhau || !body.ngaySinh || !body.gioiTinh || !body.soDienThoai) {
      return res.status(400).render("auth/register", {
        title: "Đăng ký",
        error: "Vui lòng nhập đầy đủ thông tin.",
        form: {
          hoTen: body.hoTen || "",
          email: body.email || "",
          ngaySinh: body.ngaySinh || "",
          gioiTinh: body.gioiTinh || "Nam",
          soDienThoai: body.soDienThoai || ""
        }
      });
    }

    const api = createApiClient();
    const { data } = await api.post("/Auth/register", body);

    req.session.token = data.accessToken;
    req.session.user = {
      id: data.id,
      email: data.email,
      role: data.role,
      hoTen: data.hoTen
    };

    return res.redirect("/");
  } catch (error) {
    const message = error?.response?.data?.message || "Đăng ký thất bại.";
    return res.status(400).render("auth/register", {
      title: "Đăng ký",
      error: message,
      form: {
        hoTen: req.body.hoTen || "",
        email: req.body.email || "",
        ngaySinh: req.body.ngaySinh || "",
        gioiTinh: req.body.gioiTinh || "Nam",
        soDienThoai: req.body.soDienThoai || ""
      }
    });
  }
});

router.post("/logout", (req, res) => {
  req.session.destroy(() => {
    res.redirect("/login");
  });
});

module.exports = router;