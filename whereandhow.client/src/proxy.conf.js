const { env } = require("process");

let target;

if (env.ASPNETCORE_HTTP_PORT) {
  target = `http://localhost:${env.ASPNETCORE_HTTP_PORT}`;
} else if (env.ASPNETCORE_URLS) {
  const urls = env.ASPNETCORE_URLS.split(";");
  const httpUrl = urls.find((url) => url.startsWith("http://"));
  target = httpUrl || "http://localhost:5293";
} else {
  target = "http://localhost:5293";
}


const PROXY_CONFIG = [
  {
    context: ["/api/", "/uploads"],
    target,
    secure: false,
  },
];

module.exports = PROXY_CONFIG;
