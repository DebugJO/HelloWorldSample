### bun ts electron

```bash
powershell -c "irm bun.sh/install.ps1 | iex"
bun -v
mkdir my-app && cd my-app
bun init
bun index.ts
bun add [라이브러리명]
bun build ./index.ts --compile --outfile my-app.exe
```

```bash
bun create @quick-start/electron my-electron-app
cd my-electron-app
bun install
bun dev
bun run build:win
```
### 예제

**Node.js**

```bash
mkdir node-web-server && cd node-web-server
npm init -y
npm install express
npm install -D typescript ts-node @types/node @types/express
npx tsc --init
npx ts-node index.ts
npx tsc  # index.js 파일이 생성됨
node index.js  # 변환된 파일 실행
# bun에서 실행파일 만들기
bun install # index.ts 가정
bun build ./index.ts --compile --minify --outfile my-node-app.exe
```
```ts
const path = require('path');
const exePath = path.dirname(process.execPath);
const configPath = path.join(exePath, 'config.json');
```

```ts
// index.ts
import express from 'express';
const app = express();

app.get('/', (req, res) => {
  res.json({ message: "Node.js 스타일 서버" });
});

app.listen(3000, () => {
  console.log("Server running on port 3000");
});
```

**Bun**

```ts
// index.ts
const server = Bun.serve({
  port: 3000,
  fetch(request) {
    // URL 경로에 따른 분기 처리
    const url = new URL(request.url);
    if (url.pathname === "/") {
      return Response.json({ message: "Bun 최적화 서버" });
    }
    return new Response("Not Found", { status: 404 });
  },
});

console.log(`🚀 서버가 http://localhost:${server.port} 에서 실행 중입니다.`);
```

```bash
mkdir bun-web && cd bun-web
bun init -y
bun add express
bun add -D @types/express
bun index.ts
bun build ./index.ts --compile --outfile server.exe
```

### Bun Desktop

```bash
# Vanilla + TypeScript
bun create @quick-start/electron my-desktop-app
cd my-desktop-app
bun install
bun dev
bun run build:win
bun run build:mac
bun run build:linux
```

### 일렉트론 + rust(napi-rs)

```bash
npm install -g @napi-rs/cli
napi new native-lib
```

```rs
use napi_derive::napi;

#[napi]
pub fn heavy_computation(input: i32) -> i32 {
    // Rust의 강력한 성능을 활용한 로직
    input * 2 
}
```

```bash
npm run build # rust 코드를 .node 파일로 빌드
```

```ts
# 일렉트론(TypeScript)
import { heavyComputation } from './native-lib'
console.log(heavyComputation(100)); // 200
```
