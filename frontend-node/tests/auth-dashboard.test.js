const { test, afterEach } = require("node:test");
const assert = require("node:assert/strict");
const request = require("supertest");
const createApiClient = require("../services/apiClient");
const { createApp } = require("../server");

afterEach(() => {
  createApiClient.__resetFactoryForTests();
});

function taoMockFactory(handlers) {
  const calls = [];

  createApiClient.__setFactoryForTests((token) => ({
    get: async (url) => {
      calls.push({ method: "get", url, token });
      if (handlers.get) {
        return handlers.get(url, token);
      }

      throw new Error(`Unexpected GET ${url}`);
    },
    post: async (url, body) => {
      calls.push({ method: "post", url, token, body });
      if (handlers.post) {
        return handlers.post(url, body, token);
      }

      throw new Error(`Unexpected POST ${url}`);
    },
    put: async (url, body) => {
      calls.push({ method: "put", url, token, body });
      if (handlers.put) {
        return handlers.put(url, body, token);
      }

      throw new Error(`Unexpected PUT ${url}`);
    },
    delete: async (url) => {
      calls.push({ method: "delete", url, token });
      if (handlers.delete) {
        return handlers.delete(url, token);
      }

      throw new Error(`Unexpected DELETE ${url}`);
    }
  }));

  return calls;
}

async function taoAgent(user) {
  const app = createApp({ enableTestRoutes: true });
  const agent = request.agent(app);

  if (user) {
    await agent
      .post("/__test__/session")
      .send({ token: "session-token", user })
      .expect(204);
  }

  return agent;
}

test("Dang nhap thanh cong luu session va chuyen huong", async () => {
  taoMockFactory({
    post: async (url, body) => {
      assert.equal(url, "/Auth/login");
      assert.deepEqual(body, {
        email: "admin@kytu.com",
        matKhau: "Admin@123"
      });

      return {
        data: {
          accessToken: "jwt-token",
          id: "0",
          email: "admin@kytu.com",
          role: "Admin",
          hoTen: "Quan tri vien"
        }
      };
    }
  });

  const agent = await taoAgent();

  const loginResponse = await agent
    .post("/login")
    .type("form")
    .send({ email: "admin@kytu.com", matKhau: "Admin@123" });

  assert.equal(loginResponse.status, 302);
  assert.equal(loginResponse.headers.location, "/");

  const reloginResponse = await agent.get("/login");
  assert.equal(reloginResponse.status, 302);
  assert.equal(reloginResponse.headers.location, "/");
});

test("Dang nhap that bai hien thong bao loi", async () => {
  taoMockFactory({
    post: async () => {
      const error = new Error("Unauthorized");
      error.response = {
        data: {
          message: "Thong tin dang nhap khong hop le."
        }
      };
      throw error;
    }
  });

  const agent = await taoAgent();

  const response = await agent
    .post("/login")
    .type("form")
    .send({ email: "admin@kytu.com", matKhau: "SaiMatKhau" });

  assert.equal(response.status, 401);
  assert.match(response.text, /Thong tin dang nhap khong hop le\./);
});

test("Dang ky thanh cong tu dong dang nhap", async () => {
  taoMockFactory({
    post: async (url, body) => {
      assert.equal(url, "/Auth/register");
      assert.equal(body.email, "new@kytu.com");
      assert.equal(body.hoTen, "Sinh vien moi");

      return {
        data: {
          accessToken: "register-token",
          id: "6",
          email: "new@kytu.com",
          role: "SinhVien",
          hoTen: "Sinh vien moi"
        }
      };
    }
  });

  const agent = await taoAgent();

  const registerResponse = await agent
    .post("/register")
    .type("form")
    .send({
      hoTen: "Sinh vien moi",
      email: "new@kytu.com",
      ngaySinh: "2004-06-01",
      gioiTinh: "Nam",
      soDienThoai: "0909999999",
      matKhau: "MatKhau123",
      xacNhanMatKhau: "MatKhau123"
    });

  assert.equal(registerResponse.status, 302);
  assert.equal(registerResponse.headers.location, "/");

  const reloginResponse = await agent.get("/login");
  assert.equal(reloginResponse.status, 302);
  assert.equal(reloginResponse.headers.location, "/");
});

test("Admin dashboard render dung thong ke va goi du tat ca endpoint", async () => {
  const calls = taoMockFactory({
    get: async (url, token) => {
      assert.equal(token, "session-token");

      const map = {
        "/SinhViens": [{}, {}, {}],
        "/Phongs": [{ trangThai: "ConCho" }, { trangThai: "DaDay" }, { trangThai: "ConCho" }],
        "/HopDongs": [{}, {}],
        "/HoaDons": [{}, {}, {}, {}],
        "/NhanViens": [{}, {}],
        "/ToaNhas": [{}, {}],
        "/YeuCauSuaChuas": [{}, {}, {}],
        "/TaiSans": [{}, {}]
      };

      return { data: map[url] };
    }
  });

  const agent = await taoAgent({ id: "0", role: "Admin", hoTen: "Quan tri vien", email: "admin@kytu.com" });

  const response = await agent.get("/");

  assert.equal(response.status, 200);
  assert.match(response.text, />3<\/p>/);
  assert.match(response.text, />2<\/p>/);
  assert.match(response.text, /href="\/nhanvien"/);
  assert.equal(calls.filter((item) => item.method === "get").length, 8);
});

test("SinhVien dashboard bo qua endpoint khong thuoc quyen", async () => {
  const calls = taoMockFactory({
    get: async (url, token) => {
      assert.equal(token, "session-token");

      const map = {
        "/SinhViens": [{}],
        "/HopDongs": [{}, {}],
        "/HoaDons": [{}, {}],
        "/YeuCauSuaChuas": [{}]
      };

      return { data: map[url] };
    }
  });

  const agent = await taoAgent({ id: "1", role: "SinhVien", hoTen: "Nguyen Van An", email: "an@kytu.com" });

  const response = await agent.get("/");

  assert.equal(response.status, 200);
  assert.doesNotMatch(response.text, /href="\/nhanvien"/);
  assert.doesNotMatch(response.text, /href="\/phong"/);
  assert.equal(calls.filter((item) => item.method === "get").length, 4);
  assert.deepEqual(
    calls.filter((item) => item.method === "get").map((item) => item.url),
    ["/SinhViens", "/HopDongs", "/HoaDons", "/YeuCauSuaChuas"]
  );
});

test("Dang xuat xoa session va dua nguoi dung ve login", async () => {
  const agent = await taoAgent({ id: "0", role: "Admin", hoTen: "Quan tri vien", email: "admin@kytu.com" });

  const logoutResponse = await agent.post("/logout");

  assert.equal(logoutResponse.status, 302);
  assert.equal(logoutResponse.headers.location, "/login");

  const protectedResponse = await agent.get("/phong");
  assert.equal(protectedResponse.status, 302);
  assert.equal(protectedResponse.headers.location, "/login");
});
